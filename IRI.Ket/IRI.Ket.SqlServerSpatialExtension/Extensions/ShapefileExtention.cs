using IRI.Msh.Common.Primitives;
using IRI.Ket.ShapefileFormat.EsriType;
using Microsoft.SqlServer.Types;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using IRI.Ket.SqlServerSpatialExtension.Model;
using IRI.Msh.CoordinateSystem.MapProjection;
using System.Threading.Tasks;
using IRI.Ket.SqlServerSpatialExtension.Helpers;

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


        public static List<EsriPoint> ExtractPoints(this IEsriShapeCollection shapes)
        {
            List<EsriPoint> result = new List<EsriPoint>();

            foreach (IEsriSimplePoints shape in shapes)
            {
                result.AddRange(shape.Points);
            }

            return result;
        }


        #region Esri Shape > SqlGeometry

        public static List<SqlGeometry> AsSqlGeometry(this IEsriShapeCollection shapes)
        {
            var count = shapes.Count();

            List<SqlGeometry> result = new List<SqlGeometry>(count);

            for (int i = 0; i < count; i++)
            {
                result.Add(shapes[i].AsSqlGeometry());
            }

            return result;
        }

        public static SqlGeometry AsSqlGeometry(this IEsriShape shape)
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

            builder.SetSrid(shape.Srid);

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
                    return ClearGeometry(shape.Type, result, shape.Srid);
                }

                return result;
            }
            catch (Exception ex)
            {
                return CreateDefault(shape.Type, shape.Srid);
            }
        }

        private static SqlGeometry CreateDefault(EsriShapeType esriType, int srid)
        {
            switch (esriType)
            {
                case EsriShapeType.EsriPoint:
                case EsriShapeType.EsriPointM:
                case EsriShapeType.EsriPointZ:
                    return SqlSpatialHelper.CreateEmptyPoint(srid);

                case EsriShapeType.EsriMultiPoint:
                case EsriShapeType.EsriMultiPointM:
                case EsriShapeType.EsriMultiPointZ:
                    return SqlSpatialHelper.CreateEmptyMultipoint(srid);

                case EsriShapeType.EsriPolyLine:
                case EsriShapeType.EsriPolyLineM:
                case EsriShapeType.EsriPolyLineZ:
                    return SqlSpatialHelper.CreateEmptyLineString(srid);

                case EsriShapeType.EsriPolygon:
                case EsriShapeType.EsriPolygonM:
                case EsriShapeType.EsriPolygonZ:
                    return SqlSpatialHelper.CreateEmptyPolygon(srid);

                case EsriShapeType.NullShape:
                case EsriShapeType.EsriMultiPatch:
                default:
                    return SqlGeometry.Null;
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

        #endregion


          
    }
}
