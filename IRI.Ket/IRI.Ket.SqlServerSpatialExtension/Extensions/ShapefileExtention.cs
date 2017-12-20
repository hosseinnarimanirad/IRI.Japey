using IRI.Ham.SpatialBase;
using IRI.Ket.ShapefileFormat.EsriType;
using Microsoft.SqlServer.Types;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;

namespace IRI.Ket.SpatialExtensions
{
    public static class ShapefileExtention
    { 
        private static bool AreConsistent(ShapeType esriType, OpenGisGeometryType ogcType)
        {
            switch (ogcType)
            {
                case OpenGisGeometryType.Point:
                    return esriType == ShapeType.Point || esriType == ShapeType.PointM || esriType == ShapeType.PointZ;

                case OpenGisGeometryType.MultiLineString:
                case OpenGisGeometryType.LineString:
                    return esriType == ShapeType.PolyLine || esriType == ShapeType.PolyLineM || esriType == ShapeType.PolyLineZ;

                case OpenGisGeometryType.Polygon:
                case OpenGisGeometryType.MultiPolygon:
                    return esriType == ShapeType.Polygon || esriType == ShapeType.PolygonM || esriType == ShapeType.PolygonZ;

                case OpenGisGeometryType.MultiPoint:
                    return esriType == ShapeType.MultiPoint || esriType == ShapeType.MultiPointM || esriType == ShapeType.MultiPointZ;

                case OpenGisGeometryType.GeometryCollection:
                case OpenGisGeometryType.CircularString:
                case OpenGisGeometryType.CompoundCurve:
                case OpenGisGeometryType.CurvePolygon:
                default:
                    return false;
            }
        }

        /// <summary>
        /// Removes Extera Geometries with inconsistent types (based on esriType argument)
        /// </summary>
        /// <param name="esriType"></param>
        /// <param name="geometry"></param>
        /// <returns></returns>
        private static SqlGeometry ClearGeometry(ShapeType esriType, SqlGeometry geometry, int srid)
        {
            var collection = new List<SqlGeometry>();

            for (int i = 0; i < geometry.STNumGeometries(); i++)
            {
                var temp = geometry.STGeometryN(i + 1);

                if (!temp.IsNotValidOrEmpty() && AreConsistent(esriType, temp.GetOpenGisType()))
                {
                    collection.Add(temp);
                }
            }

            var result = CreateDefault(esriType, srid);

            for (int i = 0; i < collection.Count; i++)
            {
                result = result.STUnion(collection[i]);
            }

            return result;
        }

        private static SqlGeometry CreateDefault(ShapeType esriType, int srid)
        {
            switch (esriType)
            {
                case ShapeType.Point:
                case ShapeType.PointM:
                case ShapeType.PointZ:
                    return SqlSpatialExtensions.CreateEmptyPoint(srid);

                case ShapeType.MultiPoint:
                case ShapeType.MultiPointM:
                case ShapeType.MultiPointZ:
                    return SqlSpatialExtensions.CreateEmptyMultipoint(srid);

                case ShapeType.PolyLine:
                case ShapeType.PolyLineM:
                case ShapeType.PolyLineZ:
                    return SqlSpatialExtensions.CreateEmptyLineString(srid);

                case ShapeType.Polygon:
                case ShapeType.PolygonM:
                case ShapeType.PolygonZ:
                    return SqlSpatialExtensions.CreateEmptyPolygon(srid);

                case ShapeType.NullShape:
                case ShapeType.MultiPatch:
                default:
                    return SqlGeometry.Null;
            }
        }

        public static List<SqlGeometry > AsSqlGeometry(this IShapeCollection shapes, int srid)
        {
            var count = shapes.Count();

            List<SqlGeometry> result = new List<SqlGeometry>(count);

            for (int i = 0; i < count; i++)
            {
                result.Add(shapes[i].AsSqlGeometry(srid));
            }

            return result;
        }

        public static SqlGeometry AsSqlGeometry(this IShape shape, int srid)
        {
            //try
            //{
            //    return SqlGeometry.STGeomFromWKB(new SqlBytes(shape.AsWkb()), srid).MakeValid();
            //}
            //catch (Exception ex)
            //{
            //    return SqlGeometry.Null;
            //}

            SqlGeometryBuilder builder = new SqlGeometryBuilder();
            
            builder.SetSrid(srid);
            
            try
            {
                switch (shape.Type)
                {
                    case ShapeType.NullShape:
                        return SqlGeometry.Null;

                    case ShapeType.Point:
                    case ShapeType.PointM:
                    case ShapeType.PointZ:
                        WriteEsriPoint(builder, (IPoint)shape);
                        break;

                    case ShapeType.MultiPoint:
                    case ShapeType.MultiPointM:
                    case ShapeType.MultiPointZ:
                        WriteEsriMultiPoint(builder, (ISimplePoints)shape);
                        break;

                    case ShapeType.PolyLine:
                    case ShapeType.PolyLineM:
                    case ShapeType.PolyLineZ:
                        WriteEsriPolyline(builder, (ISimplePoints)shape);
                        break;

                    case ShapeType.Polygon:
                    case ShapeType.PolygonM:
                    case ShapeType.PolygonZ:
                        WriteEsriPolygon(builder, (ISimplePoints)shape);
                        break;

                    case ShapeType.MultiPatch:
                    default:
                        break;
                }

                var result = builder.ConstructedGeometry.MakeValid();

                if (!result.IsNullOrEmpty() && !AreConsistent(shape.Type, result.GetOpenGisType()))
                {
                    return ClearGeometry(shape.Type, result, srid);
                }

                return result;
            }
            catch (Exception ex)
            {
                return CreateDefault(shape.Type, srid);
            }
        }

        private static void WriteEsriPoint(SqlGeometryBuilder builder, IPoint point)
        {
            builder.BeginGeometry(OpenGisGeometryType.Point);

            builder.BeginFigure(point.X, point.Y);

            builder.EndFigure();

            builder.EndGeometry();
        }

        private static void WriteEsriMultiPoint(SqlGeometryBuilder builder, ISimplePoints points)
        {
            builder.BeginGeometry(OpenGisGeometryType.MultiPoint);

            foreach (var point in points.Points)
            {
                WriteEsriPoint(builder, point);
            }

            builder.EndGeometry();
        }

        private static void WriteEsriPolyline(SqlGeometryBuilder builder, ISimplePoints points)
        {
            if (points.NumberOfParts == 1)
            {
                builder.BeginGeometry(OpenGisGeometryType.LineString);

                WriteLineOrRing(builder, points.Points);

                builder.EndGeometry();
            }
            else if (points.NumberOfParts > 1)
            {
                builder.BeginGeometry(OpenGisGeometryType.MultiLineString);

                for (int i = 0; i < points.NumberOfParts; i++)
                {
                    builder.BeginGeometry(OpenGisGeometryType.LineString);

                    WriteLineOrRing(builder, points.GetPart(i));

                    builder.EndGeometry();
                }

                builder.EndGeometry();

            }
            else
            {
                throw new NotImplementedException();
            }
        }

        private static void WriteEsriPolygon(SqlGeometryBuilder builder, ISimplePoints points)
        {
            if (points.NumberOfParts == 1)
            {
                builder.BeginGeometry(OpenGisGeometryType.Polygon);

                WriteLineOrRing(builder, points.Points);

                builder.EndGeometry();
            }
            else if (points.NumberOfParts > 1)
            {
                builder.BeginGeometry(OpenGisGeometryType.MultiPolygon);

                for (int i = 0; i < points.NumberOfParts; i++)
                {
                    builder.BeginGeometry(OpenGisGeometryType.Polygon);

                    WriteLineOrRing(builder, points.GetPart(i));

                    builder.EndGeometry();
                }

                builder.EndGeometry();
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        private static void WriteLineOrRing(SqlGeometryBuilder builder, EsriPoint[] points)
        {
            builder.BeginFigure(points[0].X, points[0].Y);

            for (int i = 1; i < points.Length; i++)
            {
                builder.AddLine(points[i].X, points[i].Y);
            }

            builder.EndFigure();
        }

        public static List<EsriPoint> ExtractPoints(this IShapeCollection shapes)
        {
            List<EsriPoint> result = new List<EsriPoint>();

            foreach (ISimplePoints shape in shapes)
            {
                result.AddRange(shape.Points);
            }

            return result;
        }



        public static EsriPoint AsEsriPoint(this SqlGeometry point)
        {
            if (point.IsNullOrEmpty() || point.STX.IsNull || point.STY.IsNull)
            {
                return new EsriPoint(double.NaN, double.NaN);
            }
            else
            {
                return new EsriPoint(point.STX.Value, point.STY.Value);
            }
        }


        #region Convert To ESRI Shape



        public static IShape ParseToEsriShape(this SqlGeometry geometry, Func<IPoint, IPoint> mapFunction = null)
        {
            if (geometry.IsNotValidOrEmpty())
            {
                throw new NotImplementedException();
            }

            OpenGisGeometryType geometryType = geometry.GetOpenGisType();

            switch (geometryType)
            {
                case OpenGisGeometryType.CircularString:
                case OpenGisGeometryType.CompoundCurve:
                case OpenGisGeometryType.CurvePolygon:
                case OpenGisGeometryType.GeometryCollection:
                default:
                    throw new NotImplementedException();

                case OpenGisGeometryType.LineString:
                    return LineStringOrMultiLineStringToEsriPolyline(geometry, mapFunction);

                case OpenGisGeometryType.MultiLineString:
                    return LineStringOrMultiLineStringToEsriPolyline(geometry, mapFunction);

                case OpenGisGeometryType.MultiPoint:
                    return MultiPointToEsriMultiPoint(geometry, mapFunction);

                case OpenGisGeometryType.MultiPolygon:
                    return PolygonToEsriPolygon(geometry, mapFunction);


                case OpenGisGeometryType.Point:
                    return PointToEsriPoint(geometry, mapFunction);

                case OpenGisGeometryType.Polygon:
                    return PolygonToEsriPolygon(geometry, mapFunction);
            }
        }

        //Not supportig Z and M Values
        private static EsriPoint PointToEsriPoint(SqlGeometry geometry, Func<IPoint, IPoint> mapFunction)
        {
            var point = geometry.AsEsriPoint();

            return mapFunction == null ? point : (EsriPoint)mapFunction(point);
        }

        //Not supportig Z and M Values
        private static MultiPoint MultiPointToEsriMultiPoint(SqlGeometry geometry, Func<IPoint, IPoint> mapFunction)
        {
            if (geometry.IsNullOrEmpty() || geometry.STNumGeometries().IsNull)
            {
                return new MultiPoint();
            }

            int numberOfGeometries = geometry.STNumGeometries().Value;

            List<EsriPoint> points = new List<EsriPoint>(geometry.STNumPoints().Value);

            for (int i = 0; i < numberOfGeometries; i++)
            {
                int index = i + 1;

                //EsriPoint point = new EsriPoint(geometry.STGeometryN(index).STX.Value, geometry.STGeometryN(index).STY.Value);
                var point = geometry.STGeometryN(index).AsEsriPoint();

                if (mapFunction != null)
                {
                    point = (EsriPoint)mapFunction(point);
                }

                points.Add(point);
            }

            return new MultiPoint(points.ToArray());
        }

        //Not supporting Z and M values
        private static PolyLine LineStringOrMultiLineStringToEsriPolyline(SqlGeometry geometry, Func<IPoint, IPoint> mapFunction)
        {
            if (geometry.IsNullOrEmpty())
            {
                return new PolyLine();
            }

            int numberOfGeometries = geometry.STNumGeometries().Value;

            List<EsriPoint> points = new List<EsriPoint>(geometry.STNumPoints().Value);

            List<int> parts = new List<int>(numberOfGeometries);

            for (int i = 0; i < numberOfGeometries; i++)
            {
                int index = i + 1;

                parts.Add(points.Count);

                points.AddRange(GetPoints(geometry.STGeometryN(index), mapFunction));
            }

            return new PolyLine(points.ToArray(), parts.ToArray());
        }


        //Not supporting Z and M values
        //check for cw and cww criteria
        private static Polygon PolygonToEsriPolygon(SqlGeometry geometry, Func<IPoint, IPoint> mapFunction)
        {
            if (geometry.IsNullOrEmpty())
            {
                return new Polygon();
            }

            int numberOfGeometries = geometry.STNumGeometries().Value;

            List<EsriPoint> points = new List<EsriPoint>(geometry.STNumPoints().Value);

            List<int> parts = new List<int>(numberOfGeometries);

            for (int i = 0; i < numberOfGeometries; i++)
            {
                int index = i + 1;

                SqlGeometry tempPolygon = geometry.STGeometryN(index);

                var exterior = tempPolygon.STExteriorRing();

                if (tempPolygon.IsNullOrEmpty() || exterior.IsNullOrEmpty())
                    continue;

                parts.Add(points.Count);

                points.AddRange(GetPoints(exterior, mapFunction));

                for (int j = 0; j < tempPolygon.STNumInteriorRing(); j++)
                {
                    var interior = tempPolygon.STInteriorRingN(j + 1);

                    if (interior.IsNullOrEmpty())
                        continue;

                    parts.Add(points.Count);

                    points.AddRange(GetPoints(interior, mapFunction));
                }
            }

            return new Polygon(points.ToArray(), parts.ToArray());
        }

        private static EsriPoint[] GetPoints(SqlGeometry geometry, Func<IPoint, IPoint> mapFunction)
        {
            if (geometry.IsNullOrEmpty())
            {
                return null;
            }

            int numberOfPoints = geometry.STNumPoints().Value;

            EsriPoint[] points = new EsriPoint[numberOfPoints];

            for (int i = 0; i < numberOfPoints; i++)
            {
                int index = i + 1;

                //EsriPoint point = new EsriPoint(geometry.STPointN(index).STX.Value, geometry.STPointN(index).STY.Value);
                var point = geometry.STPointN(index).AsEsriPoint();

                if (mapFunction == null)
                {
                    points[i] = point;
                }
                else
                {
                    points[i] = (EsriPoint)mapFunction(point);
                }

            }

            return points;
        }
      
        #endregion
    }
}
