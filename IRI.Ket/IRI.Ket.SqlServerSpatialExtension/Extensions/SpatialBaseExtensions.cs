using IRI.Ham.SpatialBase;
using IRI.Ham.SpatialBase.Primitives;
using Microsoft.SqlServer.Types;
using System;
using System.Diagnostics;
using System.Globalization;

namespace IRI.Ket.SpatialExtensions
{
    public static class SpatialBaseExtensions
    {
        //without counting the last point
        const int minimumPolygonPoints = 3;

        //ERROR PRONE: NaN and Infinity points are not supported
        public static SqlGeometry AsSqlGeometry(this IPoint point, int srid = 0)
        {
            //if (double.IsNaN(point.X + point.Y) || double.IsInfinity(point.X + point.Y))
            //{

            //}
            //return SqlGeometry.Parse(new System.Data.SqlTypes.SqlString(string.Format(CultureInfo.InvariantCulture, "POINT({0:G16} {1:G16})", point.X, point.Y)));
            return SqlGeometry.Point(point.X, point.Y, srid);
        }

        public static SqlGeography AsSqlGeography(this IPoint point)
        {
            return SqlGeography.Point(point.Y, point.X, 4326);
        }

        //public static SqlGeometry AsSqlGeometry(this IPoint point, int srid)
        //{
        //    return SqlGeometry.STPointFromText(new System.Data.SqlTypes.SqlChars(new System.Data.SqlTypes.SqlString(string.Format(CultureInfo.InvariantCulture, "POINT({0:G16} {1:G16})", point.X, point.Y))), srid);
        //}

        public static SqlGeometry AsSqlGeometry(this Geometry geometry)
        {
            var type = geometry.Type;

            if (geometry.IsNullOrEmpty())
            {
                return SqlSpatialExtensions.CreateEmptySqlGeometry(type, geometry.Srid);
            }

            SqlGeometryBuilder builder = new SqlGeometryBuilder();

            builder.SetSrid(geometry.Srid);

            switch (type)
            {
                case GeometryType.GeometryCollection:
                    AddGeometryCollection(builder, geometry);
                    break;

                case GeometryType.Point:
                    AddPoint(builder, geometry);
                    break;

                case GeometryType.MultiPoint:
                    AddMultiPoint(builder, geometry);
                    break;

                case GeometryType.LineString:
                    AddLineString(builder, geometry);
                    break;

                case GeometryType.MultiLineString:
                    AddMultiLineString(builder, geometry);
                    break;

                case GeometryType.MultiPolygon:
                    AddMultiPolygon(builder, geometry);
                    break;

                case GeometryType.Polygon:
                    AddPolygon(builder, geometry);
                    break;

                case GeometryType.CircularString:
                case GeometryType.CompoundCurve:
                case GeometryType.CurvePolygon:
                default:
                    throw new NotImplementedException();
            }

            return builder.ConstructedGeometry.MakeValid();

        }

        public static OpenGisGeometryType ToOpenGisGeometryType(this GeometryType type)
        {
            return (OpenGisGeometryType)((int)type);
        }

        public static GeometryType ToGeometryType(this OpenGisGeometryType type)
        {
            return (GeometryType)((int)type);
        }

        private static void AddPoint(SqlGeometryBuilder builder, Geometry point)
        {
            //SqlGeometryBuilder builder = new SqlGeometryBuilder();

            //builder.SetSrid(geometry.Srid);

            builder.BeginGeometry(OpenGisGeometryType.Point);

            builder.BeginFigure(point.Points[0].X, point.Points[0].Y);

            builder.EndFigure();

            builder.EndGeometry();

        }

        private static void AddMultiPoint(SqlGeometryBuilder builder, Geometry geometry)
        {
            //SqlGeometryBuilder builder = new SqlGeometryBuilder();

            //builder.SetSrid(geometry.Srid);

            builder.BeginGeometry(OpenGisGeometryType.MultiPoint);

            foreach (var item in geometry.Geometries)
            {
                builder.BeginGeometry(OpenGisGeometryType.Point);

                builder.BeginFigure(item.Points[0].X, item.Points[0].Y);

                builder.EndFigure();

                builder.EndGeometry();
            }

            builder.EndGeometry();

            //return builder.ConstructedGeometry.MakeValid();
        }

        private static void AddLineString(SqlGeometryBuilder builder, Geometry geometry)
        {
            //SqlGeometryBuilder builder = new SqlGeometryBuilder();

            //builder.SetSrid(geometry.Srid);

            builder.BeginGeometry(OpenGisGeometryType.LineString);

            AddLineStringOrRing(builder, geometry, false);

            builder.EndGeometry();

            //return builder.ConstructedGeometry.MakeValid();
        }

        private static void AddMultiLineString(SqlGeometryBuilder builder, Geometry geometry)
        {
            //SqlGeometryBuilder builder = new SqlGeometryBuilder();

            //builder.SetSrid(geometry.Srid);

            builder.BeginGeometry(OpenGisGeometryType.MultiLineString);

            foreach (var item in geometry.Geometries)
            {
                builder.BeginGeometry(OpenGisGeometryType.LineString);

                AddLineStringOrRing(builder, item, false);

                builder.EndGeometry();
            }

            builder.EndGeometry();

            //return builder.ConstructedGeometry.MakeValid();
        }

        private static void AddPolygon(SqlGeometryBuilder builder, Geometry geometry)
        {
            //SqlGeometryBuilder builder = new SqlGeometryBuilder();

            //builder.SetSrid(geometry.Srid);

            builder.BeginGeometry(OpenGisGeometryType.Polygon);

            foreach (var item in geometry.Geometries)
            {
                if (item.NumberOfPoints < minimumPolygonPoints)
                {
                    Trace.WriteLine($"CreatePolygon escape!");
                    continue;
                }

                AddLineStringOrRing(builder, item, true);
            }

            builder.EndGeometry();

            //return builder.ConstructedGeometry.MakeValid();
        }

        private static void AddMultiPolygon(SqlGeometryBuilder builder, Geometry geometry)
        {
            //return CreatePolygon(points, srid);
            //SqlGeometryBuilder builder = new SqlGeometryBuilder();

            //builder.SetSrid(geometry.Srid);

            builder.BeginGeometry(OpenGisGeometryType.MultiPolygon);

            foreach (var item in geometry.Geometries)
            {
                builder.BeginGeometry(OpenGisGeometryType.Polygon);

                foreach (var lines in item.Geometries)
                {
                    if (lines.NumberOfPoints < minimumPolygonPoints)
                    {
                        Trace.WriteLine($"CreateMultiPolygon escape!");
                        continue;
                    }

                    AddLineStringOrRing(builder, lines, true);
                }

                builder.EndGeometry();
            }

            builder.EndGeometry();

            //return builder.ConstructedGeometry.MakeValid();
        }

        private static void AddGeometryCollection(SqlGeometryBuilder builder, Geometry geometry)
        {
            //SqlGeometryBuilder builder = new SqlGeometryBuilder();

            //builder.SetSrid(geometry.Srid);

            builder.BeginGeometry(OpenGisGeometryType.GeometryCollection);

            foreach (var item in geometry.Geometries)
            {
                if (geometry.IsNullOrEmpty())
                {
                    SqlSpatialExtensions.AddEmptySqlGeometry(builder, item.Type);
                }

                switch (item.Type)
                {
                    case GeometryType.Point:
                        AddPoint(builder, item);
                        break;
                    case GeometryType.LineString:
                        AddLineString(builder, item);
                        break;
                    case GeometryType.Polygon:
                        AddPolygon(builder, item);
                        break;
                    case GeometryType.MultiPoint:
                        AddMultiPoint(builder, item);
                        break;
                    case GeometryType.MultiLineString:
                        AddMultiLineString(builder, item);
                        break;
                    case GeometryType.MultiPolygon:
                        AddMultiPolygon(builder, item);
                        break;
                    case GeometryType.GeometryCollection:
                    case GeometryType.CircularString:
                    case GeometryType.CompoundCurve:
                    case GeometryType.CurvePolygon:
                    default:
                        throw new NotImplementedException();
                }
            }

            builder.EndGeometry();

            //return builder.ConstructedGeometry.MakeValid();
        }


        private static void AddLineStringOrRing(SqlGeometryBuilder builder, Geometry geometry, bool isRing)
        {
            builder.BeginFigure(geometry.Points[0].X, geometry.Points[0].Y);

            for (int i = 1; i < geometry.Points.Length; i++)
            {
                builder.AddLine(geometry.Points[i].X, geometry.Points[i].Y);
            }

            if (isRing)
            {
                builder.AddLine(geometry.Points[0].X, geometry.Points[0].Y);
            }

            builder.EndFigure();
        }

    }
}
