﻿using IRI.Msh.Common.Primitives;
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
        private static bool AreConsistent(EsriShapeType esriType, OpenGisGeometryType ogcType)
        {
            switch (ogcType)
            {
                case OpenGisGeometryType.Point:
                    return esriType == EsriShapeType.EsriPoint || esriType == EsriShapeType.EsriPointM || esriType == EsriShapeType.EsriPointZ;

                case OpenGisGeometryType.MultiLineString:
                case OpenGisGeometryType.LineString:
                    return esriType == EsriShapeType.EsriPolyLine || esriType == EsriShapeType.EsriPolyLineM || esriType == EsriShapeType.EsriPolyLineZ;

                case OpenGisGeometryType.Polygon:
                case OpenGisGeometryType.MultiPolygon:
                    return esriType == EsriShapeType.EsriPolygon || esriType == EsriShapeType.EsriPolygonM || esriType == EsriShapeType.EsriPolygonZ;

                case OpenGisGeometryType.MultiPoint:
                    return esriType == EsriShapeType.EsriMultiPoint || esriType == EsriShapeType.EsriMultiPointM || esriType == EsriShapeType.EsriMultiPointZ;

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
        private static SqlGeometry ClearGeometry(EsriShapeType esriType, SqlGeometry geometry, int srid)
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

        private static SqlGeometry CreateDefault(EsriShapeType esriType, int srid)
        {
            switch (esriType)
            {
                case EsriShapeType.EsriPoint:
                case EsriShapeType.EsriPointM:
                case EsriShapeType.EsriPointZ:
                    return SqlSpatialExtensions.CreateEmptyPoint(srid);

                case EsriShapeType.EsriMultiPoint:
                case EsriShapeType.EsriMultiPointM:
                case EsriShapeType.EsriMultiPointZ:
                    return SqlSpatialExtensions.CreateEmptyMultipoint(srid);

                case EsriShapeType.EsriPolyLine:
                case EsriShapeType.EsriPolyLineM:
                case EsriShapeType.EsriPolyLineZ:
                    return SqlSpatialExtensions.CreateEmptyLineString(srid);

                case EsriShapeType.EsriPolygon:
                case EsriShapeType.EsriPolygonM:
                case EsriShapeType.EsriPolygonZ:
                    return SqlSpatialExtensions.CreateEmptyPolygon(srid);

                case EsriShapeType.NullShape:
                case EsriShapeType.EsriMultiPatch:
                default:
                    return SqlGeometry.Null;
            }
        }

        public static List<SqlGeometry> AsSqlGeometry(this IEsriShapeCollection shapes, int srid)
        {
            var count = shapes.Count();

            List<SqlGeometry> result = new List<SqlGeometry>(count);

            for (int i = 0; i < count; i++)
            {
                result.Add(shapes[i].AsSqlGeometry(srid));
            }

            return result;
        }

        public static SqlGeometry AsSqlGeometry(this IEsriShape shape, int srid)
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
                    case EsriShapeType.NullShape:
                        return SqlGeometry.Null;

                    case EsriShapeType.EsriPoint:
                    case EsriShapeType.EsriPointM:
                    case EsriShapeType.EsriPointZ:
                        WriteEsriPoint(builder, (IPoint)shape);
                        break;

                    case EsriShapeType.EsriMultiPoint:
                    case EsriShapeType.EsriMultiPointM:
                    case EsriShapeType.EsriMultiPointZ:
                        WriteEsriMultiPoint(builder, (IEsriSimplePoints)shape);
                        break;

                    case EsriShapeType.EsriPolyLine:
                    case EsriShapeType.EsriPolyLineM:
                    case EsriShapeType.EsriPolyLineZ:
                        WriteEsriPolyline(builder, (IEsriSimplePoints)shape);
                        break;

                    case EsriShapeType.EsriPolygon:
                    case EsriShapeType.EsriPolygonM:
                    case EsriShapeType.EsriPolygonZ:
                        WriteEsriPolygon(builder, (IEsriSimplePoints)shape);
                        break;

                    case EsriShapeType.EsriMultiPatch:
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

        private static void WriteEsriMultiPoint(SqlGeometryBuilder builder, IEsriSimplePoints points)
        {
            builder.BeginGeometry(OpenGisGeometryType.MultiPoint);

            foreach (var point in points.Points)
            {
                WriteEsriPoint(builder, point);
            }

            builder.EndGeometry();
        }

        private static void WriteEsriPolyline(SqlGeometryBuilder builder, IEsriSimplePoints points)
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

        private static void WriteEsriPolygon(SqlGeometryBuilder builder, IEsriSimplePoints points)
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

        public static List<EsriPoint> ExtractPoints(this IEsriShapeCollection shapes)
        {
            List<EsriPoint> result = new List<EsriPoint>();

            foreach (IEsriSimplePoints shape in shapes)
            {
                result.AddRange(shape.Points);
            }

            return result;
        }



        public static EsriPoint AsEsriPoint(this SqlGeometry point)
        {
            if (point.IsNullOrEmpty() || point.STX.IsNull || point.STY.IsNull)
            {
                return new EsriPoint(double.NaN, double.NaN, 0);
            }
            else
            {
                return new EsriPoint(point.STX.Value, point.STY.Value, point.STSrid.Value);
            }
        }


        #region Convert To ESRI Shape



        public static IEsriShape AsEsriShape(this SqlGeometry geometry, Func<IPoint, IPoint> mapFunction = null)
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
        private static EsriMultiPoint MultiPointToEsriMultiPoint(SqlGeometry geometry, Func<IPoint, IPoint> mapFunction)
        {
            if (geometry.IsNullOrEmpty() || geometry.STNumGeometries().IsNull)
            {
                return new EsriMultiPoint();
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

            return new EsriMultiPoint(points.ToArray());
        }

        //Not supporting Z and M values
        private static EsriPolyline LineStringOrMultiLineStringToEsriPolyline(SqlGeometry geometry, Func<IPoint, IPoint> mapFunction)
        {
            if (geometry.IsNullOrEmpty())
            {
                return new EsriPolyline();
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

            return new EsriPolyline(points.ToArray(), parts.ToArray());
        }


        //Not supporting Z and M values
        //check for cw and cww criteria
        private static EsriPolygon PolygonToEsriPolygon(SqlGeometry geometry, Func<IPoint, IPoint> mapFunction)
        {
            if (geometry.IsNullOrEmpty())
            {
                return new EsriPolygon();
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

            return new EsriPolygon(points.ToArray(), parts.ToArray());
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
