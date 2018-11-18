﻿using IRI.Msh.Common.Primitives;
using IRI.Msh.Common.Primitives;
using IRI.Ket.ShapefileFormat.EsriType;
using IRI.Ket.SpatialExtensions;
using IRI.Msh.Common.Model.GeoJson;
using Microsoft.SqlServer.Types;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRI.Ket.SqlServerSpatialExtension.Helpers;

namespace IRI.Ket.SqlServerSpatialExtension.Extensions
{
    public static class GeoJsonExtensions
    {
        const int minimumPolygonPoints = 3;
        //public static ShapefileFormat.EsriType.IShape ParseToSqlGeometry(this IGeoJsonGeometry geometry, bool isLongitudeFirst = false, int srid = 0)
        //{

        //}

        #region SqlGeometry

        public static SqlGeometry AsSqlGeometry(this IGeoJsonGeometry geometry, bool isLongitudeFirst = false, int srid = 0)
        {
            var type = geometry.GeometryType;

            if (geometry.IsNullOrEmpty())
            {
                return SqlSpatialHelper.CreateEmptySqlGeometry(type, srid);
            }

            SqlGeometryBuilder builder = new SqlGeometryBuilder();

            builder.SetSrid(srid);

            switch (type)
            {
                case GeometryType.Point:
                    AddPoint(builder, (GeoJsonPoint)geometry, isLongitudeFirst);
                    break;

                case GeometryType.MultiPoint:
                    AddMultiPoint(builder, (GeoJsonMultiPoint)geometry, isLongitudeFirst);
                    break;

                case GeometryType.LineString:
                    AddLineString(builder, (GeoJsonLineString)geometry, isLongitudeFirst);
                    break;

                case GeometryType.MultiLineString:
                    AddMultiLineString(builder, (GeoJsonMultiLineString)geometry, isLongitudeFirst);
                    break;

                case GeometryType.MultiPolygon:
                    AddMultiPolygon(builder, (GeoJsonMultiPolygon)geometry, isLongitudeFirst);
                    break;

                case GeometryType.Polygon:
                    AddPolygon(builder, (GeoJsonPolygon)geometry, isLongitudeFirst);
                    break;


                case GeometryType.GeometryCollection:
                case GeometryType.CircularString:
                case GeometryType.CompoundCurve:
                case GeometryType.CurvePolygon:
                default:
                    throw new NotImplementedException();
            }

            return builder.ConstructedGeometry.MakeValid();
        }

        private static void AddPoint(SqlGeometryBuilder builder, GeoJsonPoint point, bool isLongitudeFirst)
        {
            builder.BeginGeometry(OpenGisGeometryType.Point);

            var temporaryPoint = IRI.Msh.Common.Primitives.Point.Parse(point.Coordinates, isLongitudeFirst);

            builder.BeginFigure(temporaryPoint.X, temporaryPoint.Y);

            builder.EndFigure();

            builder.EndGeometry();
        }

        private static void AddMultiPoint(SqlGeometryBuilder builder, GeoJsonMultiPoint geometry, bool isLongitudeFirst)
        {
            builder.BeginGeometry(OpenGisGeometryType.MultiPoint);

            foreach (var item in geometry.Coordinates)
            {
                builder.BeginGeometry(OpenGisGeometryType.Point);

                var temporaryPoint = IRI.Msh.Common.Primitives.Point.Parse(item, isLongitudeFirst);

                builder.BeginFigure(temporaryPoint.X, temporaryPoint.Y);

                builder.EndFigure();

                builder.EndGeometry();
            }

            builder.EndGeometry();
        }

        private static void AddLineString(SqlGeometryBuilder builder, GeoJsonLineString geometry, bool isLongitudeFirst)
        {
            builder.BeginGeometry(OpenGisGeometryType.LineString);

            AddLineStringOrRing(builder, geometry.Coordinates, false, isLongitudeFirst);

            builder.EndGeometry();
        }

        private static void AddMultiLineString(SqlGeometryBuilder builder, GeoJsonMultiLineString geometry, bool isLongitudeFirst)
        {
            builder.BeginGeometry(OpenGisGeometryType.MultiLineString);

            foreach (var item in geometry.Coordinates)
            {
                builder.BeginGeometry(OpenGisGeometryType.LineString);

                AddLineStringOrRing(builder, item, false, isLongitudeFirst);

                builder.EndGeometry();
            }

            builder.EndGeometry();
        }

        private static void AddPolygon(SqlGeometryBuilder builder, GeoJsonPolygon geometry, bool isLongitudeFirst)
        {
            builder.BeginGeometry(OpenGisGeometryType.Polygon);

            foreach (var item in geometry.Coordinates)
            {
                if (item.Length < minimumPolygonPoints)
                {
                    Trace.WriteLine($"CreatePolygon escape!");
                    continue;
                }

                AddLineStringOrRing(builder, item, true, isLongitudeFirst);
            }

            builder.EndGeometry();
        }

        private static void AddMultiPolygon(SqlGeometryBuilder builder, GeoJsonMultiPolygon geometry, bool isLongitudeFirst)
        {
            builder.BeginGeometry(OpenGisGeometryType.MultiPolygon);

            foreach (var item in geometry.Coordinates)
            {
                builder.BeginGeometry(OpenGisGeometryType.Polygon);

                foreach (var lines in item)
                {
                    if (lines.Length < minimumPolygonPoints)
                    {
                        Trace.WriteLine($"CreateMultiPolygon escape!");
                        continue;
                    }

                    AddLineStringOrRing(builder, lines, true, isLongitudeFirst);
                }

                builder.EndGeometry();
            }

            builder.EndGeometry();
        }

        //private static void AddGeometryCollection(SqlGeometryBuilder builder, IGeoJsonGeometry geometry, bool isLongitudeFirst)
        //{ 
        //    builder.BeginGeometry(OpenGisGeometryType.GeometryCollection);

        //    foreach (var item in geometry.Geometries)
        //    {
        //        if (geometry.IsNullOrEmpty())
        //        {
        //            SqlSpatialExtensions.AddEmptySqlGeometry(builder, item.Type);
        //        }

        //        switch (item.Type)
        //        {
        //            case GeometryType.Point:
        //                AddPoint(builder, item);
        //                break;
        //            case GeometryType.LineString:
        //                AddLineString(builder, item);
        //                break;
        //            case GeometryType.Polygon:
        //                AddPolygon(builder, item);
        //                break;
        //            case GeometryType.MultiPoint:
        //                AddMultiPoint(builder, item);
        //                break;
        //            case GeometryType.MultiLineString:
        //                AddMultiLineString(builder, item);
        //                break;
        //            case GeometryType.MultiPolygon:
        //                AddMultiPolygon(builder, item);
        //                break;
        //            case GeometryType.GeometryCollection:
        //            case GeometryType.CircularString:
        //            case GeometryType.CompoundCurve:
        //            case GeometryType.CurvePolygon:
        //            default:
        //                throw new NotImplementedException();
        //        }
        //    }

        //    builder.EndGeometry();             
        //}

        private static void AddLineStringOrRing(SqlGeometryBuilder builder, double[][] geometry, bool isRing, bool isLongitudeFirst)
        {
            var firstPoint = Point.Parse(geometry[0], isLongitudeFirst);

            builder.BeginFigure(firstPoint.X, firstPoint.Y);

            for (int i = 1; i < geometry.Length; i++)
            {
                var point = Point.Parse(geometry[i], isLongitudeFirst);

                builder.AddLine(point.X, point.Y);
            }

            if (isRing)
            {
                builder.AddLine(firstPoint.X, firstPoint.Y);
            }

            builder.EndFigure();
        }

        #endregion


        #region Shapefile

        public static IEsriShape AsToShapefileShape(this IGeoJsonGeometry geometry, bool isLongitudeFirst = false, int srid = 0)
        {
            return geometry.AsSqlGeometry(isLongitudeFirst, srid).AsEsriShape();
        }

        public static void WriteAsShapefile(this IEnumerable<GeoJsonFeature> features, string shpFileName, bool isLongitudeFirst = false)
        {
            var shapes = features.Select(f => f.geometry.AsToShapefileShape(isLongitudeFirst));

            IRI.Ket.ShapefileFormat.Shapefile.Save(shpFileName, shapes, false, true);

            var fields = new List<ShapefileFormat.Model.ObjectToDbfTypeMap<GeoJsonFeature>>();

            foreach (var item in features.First().properties)
            {
                var propertyName = item.Key;

                fields.Add(
                    new ShapefileFormat.Model.ObjectToDbfTypeMap<GeoJsonFeature>(
                            ShapefileFormat.Dbf.DbfFieldDescriptors.GetStringField(propertyName, 255), 
                            new Func<GeoJsonFeature, string>(g => g.properties[propertyName])));
            }

            var dbfFile = IRI.Ket.ShapefileFormat.Shapefile.GetDbfFileName(shpFileName);

            IRI.Ket.ShapefileFormat.Dbf.DbfFile.Write<GeoJsonFeature>(dbfFile, features, fields, Encoding.GetEncoding(1256), true);
        }

        #endregion
    }
}
