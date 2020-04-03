﻿using IRI.Msh.CoordinateSystem.MapProjection;
using IRI.Msh.Common.Primitives;
using IRI.Msh.CoordinateSystem.MapProjection;
using IRI.Msh.Common.Primitives;
using Microsoft.SqlServer.Types;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;

namespace IRI.Ket.SpatialExtensions
{
    public static class SpatialBaseExtensions
    {
        //without counting the last point
        const int minimumPolygonPoints = 3;

        public static double GetArea(this Geometry geometry)
        {
            var sqlGeometry = geometry.AsSqlGeometry();

            if (sqlGeometry != null && sqlGeometry.STIsValid().Value)
            {
                return geometry.AsSqlGeometry().STArea().Value;
            }
            else
            {
                return double.NaN;
            }

        }

        public static double GetArea(this Geometry geometry, Func<IPoint, IPoint> toWgs84Geodetic)
        {
            try
            {
                return geometry.AsSqlGeometry().Project(toWgs84Geodetic, SridHelper.GeodeticWGS84).MakeValid().STArea().Value;
            }
            catch (Exception)
            {
                return double.NaN;
            }
            //return GetArea(geometry.Transform(toWgs84Geodetic, 0));
        }

        public static double GetLength(this Geometry geometry, Func<IPoint, IPoint> toWgs84Geodetic)
        {
            try
            {
                return geometry.AsSqlGeometry().Project(toWgs84Geodetic, SridHelper.GeodeticWGS84).MakeValid().STLength().Value;
            }
            catch (Exception)
            {
                return double.NaN;
            }
            //return GetArea(geometry.Transform(toWgs84Geodetic, 0));
        }

        public static double GetMeasure(this Geometry geometry, Func<IPoint, IPoint> toWgs84Geodetic)
        {
            if (geometry == null)
            {
                return double.NaN;
            }
            else
            {
                switch (geometry.Type)
                {
                    case GeometryType.LineString:
                    case GeometryType.MultiLineString:
                        return geometry.GetLength(toWgs84Geodetic);

                    case GeometryType.Polygon:
                    case GeometryType.MultiPolygon:
                        return geometry.GetArea(toWgs84Geodetic);

                    case GeometryType.Point:
                    case GeometryType.MultiPoint:
                    case GeometryType.GeometryCollection:
                    case GeometryType.CircularString:
                    case GeometryType.CompoundCurve:
                    case GeometryType.CurvePolygon:
                    default:
                        throw new NotImplementedException();
                }
            }

        }

        public static string GetMeasureLabel(this Geometry geometry, Func<IPoint, IPoint> toWgs84Geodetic)
        {
            if (geometry == null)
            {
                return string.Empty;
            }
            else
            {
                switch (geometry.Type)
                {
                    case GeometryType.LineString:
                    case GeometryType.MultiLineString:
                        return Common.Helpers.UnitHelper.GetLengthLabel(geometry.GetLength(toWgs84Geodetic));

                    case GeometryType.Polygon:
                    case GeometryType.MultiPolygon:
                        return Common.Helpers.UnitHelper.GetAreaLabel(geometry.GetArea(toWgs84Geodetic));

                    case GeometryType.Point:
                    case GeometryType.MultiPoint:
                    case GeometryType.GeometryCollection:
                    case GeometryType.CircularString:
                    case GeometryType.CompoundCurve:
                    case GeometryType.CurvePolygon:
                    default:
                        throw new NotImplementedException();
                }
            }
            //
        }

        public static IPoint GetMeanOrLastPoint(this Geometry geometry)
        {
            if (geometry == null)
            {
                return null;
            }
            else
            {
                switch (geometry.Type)
                {
                    case GeometryType.LineString:
                    case GeometryType.MultiLineString:
                        return geometry.GetLastPoint();

                    case GeometryType.Polygon:
                    case GeometryType.MultiPolygon:
                        return geometry.GetMeanPoint();

                    case GeometryType.Point:
                    case GeometryType.MultiPoint:
                    case GeometryType.GeometryCollection:
                    case GeometryType.CircularString:
                    case GeometryType.CompoundCurve:
                    case GeometryType.CurvePolygon:
                    default:
                        throw new NotImplementedException();
                }
            }
        }


        #region SqlGeometry

        //ERROR PRONE: NaN and Infinity points are not supported
        public static SqlGeometry AsSqlGeometry(this IPoint point, int srid = 0)
        {
            //if (double.IsNaN(point.X + point.Y) || double.IsInfinity(point.X + point.Y))
            //{

            //}
            //return SqlGeometry.Parse(new System.Data.SqlTypes.SqlString(string.Format(CultureInfo.InvariantCulture, "POINT({0:G16} {1:G16})", point.X, point.Y)));
            return SqlGeometry.Point(point.X, point.Y, srid);
        }

        public static SqlGeometry AsSqlGeometry(this Geometry geometry)
        {
            var type = geometry.Type;

            if (geometry.IsNullOrEmpty())
            {
                return IRI.Ket.SqlServerSpatialExtension.Helpers.SqlSpatialHelper.CreateEmptySqlGeometry(type, geometry.Srid);
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
                    IRI.Ket.SqlServerSpatialExtension.Helpers.SqlSpatialHelper.AddEmptySqlGeometry(builder, item.Type);
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

        #endregion



        #region SqlGeography

        public static SqlGeography AsSqlGeography(this IPoint point)
        {
            return SqlGeography.Point(latitude: point.Y, longitude: point.X, srid: SridHelper.GeodeticWGS84);
        }

        public static SqlGeography AsSqlGeography(this Geometry geometry)
        {
            var type = geometry.Type;

            if (geometry.IsNullOrEmpty())
            {
                return IRI.Ket.SqlServerSpatialExtension.Helpers.SqlSpatialHelper.CreateEmptySqlGeography(type, geometry.Srid);
            }

            SqlGeographyBuilder builder = new SqlGeographyBuilder();

            builder.SetSrid(geometry.Srid);

            switch (type)
            {
                case GeometryType.GeometryCollection:
                    AddGeographyCollection(builder, geometry);
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

            return builder.ConstructedGeography.MakeValid();

        }

        private static void AddPoint(SqlGeographyBuilder builder, Geometry point)
        {
            builder.BeginGeography(OpenGisGeographyType.Point);

            builder.BeginFigure(longitude: point.Points[0].X, latitude: point.Points[0].Y);

            builder.EndFigure();

            builder.EndGeography();
        }

        private static void AddMultiPoint(SqlGeographyBuilder builder, Geometry geometry)
        {
            builder.BeginGeography(OpenGisGeographyType.MultiPoint);

            foreach (var item in geometry.Geometries)
            {
                builder.BeginGeography(OpenGisGeographyType.Point);

                builder.BeginFigure(longitude: item.Points[0].X, latitude: item.Points[0].Y);

                builder.EndFigure();

                builder.EndGeography();
            }

            builder.EndGeography();
        }

        private static void AddLineString(SqlGeographyBuilder builder, Geometry geometry)
        {
            builder.BeginGeography(OpenGisGeographyType.LineString);

            AddLineStringOrRing(builder, geometry, false);

            builder.EndGeography();
        }

        private static void AddMultiLineString(SqlGeographyBuilder builder, Geometry geometry)
        {
            builder.BeginGeography(OpenGisGeographyType.MultiLineString);

            foreach (var item in geometry.Geometries)
            {
                builder.BeginGeography(OpenGisGeographyType.LineString);

                AddLineStringOrRing(builder, item, false);

                builder.EndGeography();
            }

            builder.EndGeography();
        }

        private static void AddPolygon(SqlGeographyBuilder builder, Geometry geometry)
        {
            builder.BeginGeography(OpenGisGeographyType.Polygon);

            foreach (var item in geometry.Geometries)
            {
                if (item.NumberOfPoints < minimumPolygonPoints)
                {
                    Trace.WriteLine($"CreatePolygon escape!");
                    continue;
                }

                AddLineStringOrRing(builder, item, true);
            }

            builder.EndGeography();

            //return builder.ConstructedGeometry.MakeValid();
        }

        private static void AddMultiPolygon(SqlGeographyBuilder builder, Geometry geometry)
        {
            //return CreatePolygon(points, srid);
            //SqlGeometryBuilder builder = new SqlGeometryBuilder();

            //builder.SetSrid(geometry.Srid);

            builder.BeginGeography(OpenGisGeographyType.MultiPolygon);

            foreach (var item in geometry.Geometries)
            {
                builder.BeginGeography(OpenGisGeographyType.Polygon);

                foreach (var lines in item.Geometries)
                {
                    if (lines.NumberOfPoints < minimumPolygonPoints)
                    {
                        Trace.WriteLine($"CreateMultiPolygon escape!");
                        continue;
                    }

                    AddLineStringOrRing(builder, lines, true);
                }

                builder.EndGeography();
            }

            builder.EndGeography();

            //return builder.ConstructedGeometry.MakeValid();
        }

        private static void AddGeographyCollection(SqlGeographyBuilder builder, Geometry geometry)
        {
            //SqlGeometryBuilder builder = new SqlGeometryBuilder();

            //builder.SetSrid(geometry.Srid);

            builder.BeginGeography(OpenGisGeographyType.GeometryCollection);

            foreach (var item in geometry.Geometries)
            {
                if (geometry.IsNullOrEmpty())
                {
                    IRI.Ket.SqlServerSpatialExtension.Helpers.SqlSpatialHelper.AddEmptySqlGeometry(builder, item.Type);
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

            builder.EndGeography();

            //return builder.ConstructedGeometry.MakeValid();
        }

        private static void AddLineStringOrRing(SqlGeographyBuilder builder, Geometry geometry, bool isRing)
        {
            builder.BeginFigure(longitude: geometry.Points[0].X, latitude: geometry.Points[0].Y);

            for (int i = 1; i < geometry.Points.Length; i++)
            {
                builder.AddLine(longitude: geometry.Points[i].X, latitude: geometry.Points[i].Y);
            }

            if (isRing)
            {
                builder.AddLine(longitude: geometry.Points[0].X, latitude: geometry.Points[0].Y);
            }

            builder.EndFigure();
        }

        #endregion



        #region LineSegment

        public static double CalculateLength(this LineSegment line, Func<IPoint, IPoint> toGeodeticWgs84Func)
        {
            var start = toGeodeticWgs84Func(line.Start);

            var end = toGeodeticWgs84Func(line.End);

            var geodeticLine = SqlServerSpatialExtension.SqlSpatialUtility.MakeGeography(new System.Collections.Generic.List<Point>() { (Point)start, (Point)end }, false);

            return geodeticLine.STLength().Value;
        }

        public static string GetLengthLabel(this LineSegment line, Func<IPoint, IPoint> toGeodeticWgs84Func)
        {
            var length = CalculateLength(line, toGeodeticWgs84Func);

            return Common.Helpers.UnitHelper.GetLengthLabel(length);
        }

        #endregion

        #region Projection


        public static IPoint Project(this IPoint point, SrsBase sourceSrs, SrsBase targetSrs)
        {
            if (sourceSrs.Ellipsoid.AreTheSame(targetSrs.Ellipsoid))
            {
                var c1 = sourceSrs.ToGeodetic(point);

                return targetSrs.FromGeodetic(c1);
            }
            else
            {
                var c1 = sourceSrs.ToGeodetic(point);

                return targetSrs.FromGeodetic(c1, sourceSrs.Ellipsoid);

            }
        }

        #endregion



    }
}
