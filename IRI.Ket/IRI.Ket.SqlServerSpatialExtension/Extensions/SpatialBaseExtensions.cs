using IRI.Msh.CoordinateSystem.MapProjection;
using IRI.Msh.Common.Primitives;
using Microsoft.SqlServer.Types;
using System;
using System.Diagnostics;
using IRI.Msh.Common.Model.GeoJson;

namespace IRI.Ket.SpatialExtensions
{
    public static class SpatialBaseExtensions
    {
        //without counting the last point
        const int minimumPolygonPoints = 3;

        public static double GetArea<T>(this Geometry<T> geometry) where T : IPoint, new()
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

        public static double GetTrueArea<T>(this Geometry<T> geometry, Func<Point, Point> toWgs84Geodetic) where T : IPoint, new()
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

        public static double GetLength<T>(this Geometry<T> geometry, Func<Point, Point> toWgs84Geodetic) where T : IPoint, new()
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

        public static double GetMeasure<T>(this Geometry<T> geometry, Func<Point, Point> toWgs84Geodetic) where T : IPoint, new()
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
                        return geometry.GetTrueArea(toWgs84Geodetic);

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

        public static string GetMeasureLabel<T>(this Geometry<T> geometry, Func<Point, Point> toWgs84Geodetic) where T : IPoint, new()
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
                        return Common.Helpers.UnitHelper.GetAreaLabel(geometry.GetTrueArea(toWgs84Geodetic));

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

        public static IPoint GetMeanOrLastPoint<T>(this Geometry<T> geometry) where T : IPoint, new()
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


        #region Geometry To GeoJson

        public static IGeoJsonGeometry ParseToGeoJson<T>(this T point) where T : IPoint, new()
        {
            if (point == null)
                return new GeoJsonPoint()
                {
                    Type = GeoJson.Point,
                };

            return new GeoJsonPoint()
            {
                Type = GeoJson.Point,
                Coordinates = new double[] { point.X, point.Y }
            };
        }

        public static IGeoJsonGeometry ParseToGeoJson<T>(this Geometry<T> geometry) where T : IPoint, new()
        {
            //if (geometry.IsNullOrEmpty())
            //{
            //    throw new NotImplementedException();
            //}

            switch (geometry.Type)
            {
                case GeometryType.Point:
                    return geometry.GeometryPointToGeoJsonPoint();

                case GeometryType.LineString:
                    return GeometryLineStringToGeoJsonPolyline(geometry);

                case GeometryType.Polygon:
                    return GeometryPolygonToGeoJsonPolygon(geometry);

                case GeometryType.MultiPoint:
                    return GeometryMultiPointToGeoJsonMultiPoint(geometry);

                case GeometryType.MultiLineString:
                    return GeometryMultiLineStringToGeoJsonPolyline(geometry);

                case GeometryType.MultiPolygon:
                    return GeometryMultiPolygonToGeoJsonMultiPolygon(geometry);

                case GeometryType.GeometryCollection:
                case GeometryType.CircularString:
                case GeometryType.CompoundCurve:
                case GeometryType.CurvePolygon:
                default:
                    throw new NotImplementedException();
            }

            //OpenGisGeometryType geometryType = geometry.GetOpenGisType();

            //switch (geometryType)
            //{
            //    case OpenGisGeometryType.CircularString:
            //    case OpenGisGeometryType.CompoundCurve:
            //    case OpenGisGeometryType.CurvePolygon:
            //    case OpenGisGeometryType.GeometryCollection:
            //    default:
            //        throw new NotImplementedException();

            //    case OpenGisGeometryType.Point:
            //        return geometry.SqlPointToGeoJsonPoint();

            //    case OpenGisGeometryType.MultiPoint:
            //        return SqlMultiPointToGeoJsonMultiPoint(geometry);

            //    case OpenGisGeometryType.LineString:
            //        return SqlLineStringToGeoJsonPolyline(geometry);

            //    case OpenGisGeometryType.MultiLineString:
            //        return SqlMultiLineStringToGeoJsonPolyline(geometry);

            //    case OpenGisGeometryType.Polygon:
            //        return SqlPolygonToGeoJsonPolygon(geometry);

            //    case OpenGisGeometryType.MultiPolygon:
            //        return SqlMultiPolygonToGeoJsonMultiPolygon(geometry);
            //}
        }

        private static double[] GetGeoJsonObjectPoint<T>(T point) where T : IPoint, new()
        {
            if (point == null)
                return new double[0];

            return new double[] { point.X, point.Y };
        }

        private static double[][] GetGeoJsonLineStringOrRing<T>(Geometry<T> lineStringOrRing) where T : IPoint, new()
        {
            if (lineStringOrRing.IsNullOrEmpty())
                return new double[0][];

            int numberOfPoints = lineStringOrRing.NumberOfPoints;

            double[][] result = new double[numberOfPoints][];

            for (int i = 0; i < numberOfPoints; i++)
            {
                result[i] = GetGeoJsonObjectPoint<T>(lineStringOrRing.Points[i]);
            }

            return result;
        }

        private static GeoJsonPoint GeometryPointToGeoJsonPoint<T>(this Geometry<T> geometry) where T : IPoint, new()
        {
            //This check is required
            if (geometry.IsNullOrEmpty())
                return new GeoJsonPoint()
                {
                    Type = GeoJson.Point,
                };

            return new GeoJsonPoint()
            {
                Type = GeoJson.Point,
                Coordinates = new double[] { geometry.Points[0].X, geometry.Points[0].Y }
            };
        }

        private static GeoJsonMultiPoint GeometryMultiPointToGeoJsonMultiPoint<T>(this Geometry<T> geometry) where T : IPoint, new()
        {
            //This check is required
            if (geometry.IsNullOrEmpty())
                return new GeoJsonMultiPoint()
                {
                    Type = GeoJson.MultiPoint,
                    Coordinates = new double[0][],
                };

            var numberOfGeometries = geometry.NumberOfGeometries;

            double[][] points = new double[numberOfGeometries][];

            for (int i = 0; i < numberOfGeometries; i++)
            {
                points[i] = GetGeoJsonObjectPoint(geometry.Geometries[i].Points[0]);
            }

            return new GeoJsonMultiPoint()
            {
                Coordinates = points,
                Type = GeoJson.MultiPoint,
            };
        }

        //Not supportig Z and M Values
        private static GeoJsonLineString GeometryLineStringToGeoJsonPolyline<T>(this Geometry<T> geometry) where T : IPoint, new()
        {
            //This check is required
            if (geometry.IsNullOrEmpty())
                return new GeoJsonLineString()
                {
                    Type = GeoJson.LineString,
                    Coordinates = new double[0][],
                };

            double[][] paths = GetGeoJsonLineStringOrRing(geometry);

            return new GeoJsonLineString()
            {
                Coordinates = paths,
                Type = GeoJson.LineString,
            };
        }

        //Not supportig Z and M Values
        private static GeoJsonMultiLineString GeometryMultiLineStringToGeoJsonPolyline<T>(this Geometry<T> geometry) where T : IPoint, new()
        {
            //This check is required
            if (geometry.IsNullOrEmpty())
                return new GeoJsonMultiLineString()
                {
                    Type = GeoJson.MultiLineString,
                    Coordinates = new double[0][][],
                };

            int numberOfParts = geometry.NumberOfGeometries;

            double[][][] result = new double[numberOfParts][][];

            for (int i = 0; i < numberOfParts; i++)
            {
                result[i] = GetGeoJsonLineStringOrRing(geometry.Geometries[i]);
            }

            return new GeoJsonMultiLineString()
            {
                Coordinates = result,
                Type = GeoJson.MultiLineString,
            };
        }

        //Not supportig Z and M Values
        private static GeoJsonPolygon GeometryPolygonToGeoJsonPolygon<T>(this Geometry<T> geometry) where T : IPoint, new()
        {
            //This check is required
            if (geometry.IsNullOrEmpty())
                return new GeoJsonPolygon()
                {
                    Type = GeoJson.Polygon,
                    Coordinates = new double[0][][],
                };


            int numberOfParts = geometry.NumberOfGeometries;

            double[][][] result = new double[numberOfParts][][];

            for (int i = 0; i < numberOfParts; i++)
            {
                result[i] = GetGeoJsonLineStringOrRing(geometry.Geometries[i]);
            }

            //double[][][] rings = new double[1][][] { GetGeoJsonLineStringOrRing(geometry) };

            return new GeoJsonPolygon()
            {
                Coordinates = result,
                Type = GeoJson.Polygon,
            };
        }

        //Not supportig Z and M Values
        private static GeoJsonMultiPolygon GeometryMultiPolygonToGeoJsonMultiPolygon<T>(this Geometry<T> geometry) where T : IPoint, new()
        {
            //This check is required
            if (geometry.IsNullOrEmpty())
                return new GeoJsonMultiPolygon()
                {
                    Type = GeoJson.MultiPolygon,
                    Coordinates = new double[0][][][],
                };

            int numberOfParts = geometry.NumberOfGeometries;

            double[][][][] rings = new double[numberOfParts][][][];

            for (int i = 0; i < numberOfParts; i++)
            {
                rings[i] = geometry.Geometries[i].GeometryPolygonToGeoJsonPolygon().Coordinates;
            }

            return new GeoJsonMultiPolygon()
            {
                Coordinates = rings,
                Type = GeoJson.MultiPolygon,
            };
        }

        #endregion


        #region SqlGeometry

        //ERROR PRONE: NaN and Infinity points are not supported
        public static SqlGeometry AsSqlGeometry<T>(this T point, int srid = 0) where T : IPoint, new()
        {
            //if (double.IsNaN(point.X + point.Y) || double.IsInfinity(point.X + point.Y))
            //{

            //}
            //return SqlGeometry.Parse(new System.Data.SqlTypes.SqlString(string.Format(CultureInfo.InvariantCulture, "POINT({0:G16} {1:G16})", point.X, point.Y)));
            return SqlGeometry.Point(point.X, point.Y, srid);
        }

        public static SqlGeometry AsSqlGeometry<T>(this Geometry<T> geometry) where T : IPoint, new()
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

        private static void AddPoint<T>(SqlGeometryBuilder builder, Geometry<T> point) where T : IPoint, new()
        {
            //SqlGeometryBuilder builder = new SqlGeometryBuilder();

            //builder.SetSrid(geometry.Srid);

            builder.BeginGeometry(OpenGisGeometryType.Point);

            builder.BeginFigure(point.Points[0].X, point.Points[0].Y);

            builder.EndFigure();

            builder.EndGeometry();

        }

        private static void AddMultiPoint<T>(SqlGeometryBuilder builder, Geometry<T> geometry) where T : IPoint, new()
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

        private static void AddLineString<T>(SqlGeometryBuilder builder, Geometry<T> geometry) where T : IPoint, new()
        {
            //SqlGeometryBuilder builder = new SqlGeometryBuilder();

            //builder.SetSrid(geometry.Srid);

            builder.BeginGeometry(OpenGisGeometryType.LineString);

            AddLineStringOrRing(builder, geometry, false);

            builder.EndGeometry();

            //return builder.ConstructedGeometry.MakeValid();
        }

        private static void AddMultiLineString<T>(SqlGeometryBuilder builder, Geometry<T> geometry) where T : IPoint, new()
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

        private static void AddPolygon<T>(SqlGeometryBuilder builder, Geometry<T> geometry) where T : IPoint, new()
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

        private static void AddMultiPolygon<T>(SqlGeometryBuilder builder, Geometry<T> geometry) where T : IPoint, new()
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

        private static void AddGeometryCollection<T>(SqlGeometryBuilder builder, Geometry<T> geometry) where T : IPoint, new()
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

        private static void AddLineStringOrRing<T>(SqlGeometryBuilder builder, Geometry<T> geometry, bool isRing) where T : IPoint, new()
        {
            builder.BeginFigure(geometry.Points[0].X, geometry.Points[0].Y);

            for (int i = 1; i < geometry.Points.Count; i++)
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

        public static SqlGeography AsSqlGeography<T>(this Geometry<T> geometry) where T : IPoint, new()
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

        private static void AddPoint<T>(SqlGeographyBuilder builder, Geometry<T> point) where T : IPoint, new()
        {
            builder.BeginGeography(OpenGisGeographyType.Point);

            builder.BeginFigure(longitude: point.Points[0].X, latitude: point.Points[0].Y);

            builder.EndFigure();

            builder.EndGeography();
        }

        private static void AddMultiPoint<T>(SqlGeographyBuilder builder, Geometry<T> geometry) where T : IPoint, new()
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

        private static void AddLineString<T>(SqlGeographyBuilder builder, Geometry<T> geometry) where T : IPoint, new()
        {
            builder.BeginGeography(OpenGisGeographyType.LineString);

            AddLineStringOrRing(builder, geometry, false);

            builder.EndGeography();
        }

        private static void AddMultiLineString<T>(SqlGeographyBuilder builder, Geometry<T> geometry) where T : IPoint, new()
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

        private static void AddPolygon<T>(SqlGeographyBuilder builder, Geometry<T> geometry) where T : IPoint, new()
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

        private static void AddMultiPolygon<T>(SqlGeographyBuilder builder, Geometry<T> geometry) where T : IPoint, new()
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

        private static void AddGeographyCollection<T>(SqlGeographyBuilder builder, Geometry<T> geometry) where T : IPoint, new()
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

        private static void AddLineStringOrRing<T>(SqlGeographyBuilder builder, Geometry<T> geometry, bool isRing) where T : IPoint, new()
        {
            builder.BeginFigure(longitude: geometry.Points[0].X, latitude: geometry.Points[0].Y);

            for (int i = 1; i < geometry.Points.Count; i++)
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

        public static double CalculateLength<T>(this LineSegment<T> line, Func<T, T> toGeodeticWgs84Func) where T : IPoint, new()
        {
            var start = toGeodeticWgs84Func(line.Start);

            var end = toGeodeticWgs84Func(line.End);

            var geodeticLine = SqlServerSpatialExtension.SqlSpatialUtility.MakeGeography(new System.Collections.Generic.List<T>() { start, end }, false);

            return geodeticLine.STLength().Value;
        }

        public static string GetLengthLabel<T>(this LineSegment<T> line, Func<T, T> toGeodeticWgs84Func) where T : IPoint, new()
        {
            var length = CalculateLength(line, toGeodeticWgs84Func);

            return Common.Helpers.UnitHelper.GetLengthLabel(length);
        }

        #endregion


        #region Projection

        public static T Project<T>(this T point, SrsBase sourceSrs, SrsBase targetSrs) where T : IPoint, new()
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
