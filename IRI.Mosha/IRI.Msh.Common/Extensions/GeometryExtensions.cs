using IRI.Msh.Common.Analysis;
using IRI.Msh.Common.Model.GeoJson;
using IRI.Msh.Common.Primitives;
using IRI.Msh.CoordinateSystem.MapProjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace IRI.Msh.Common.Extensions
{
    public static class GeometryExtensions
    {

        public static List<Geometry<T>> Project<T>(this List<Geometry<T>> values, SrsBase sourceSrs, SrsBase targetSrs) where T : IPoint, new()
        {
            List<Geometry<T>> result = new List<Geometry<T>>(values.Count);

            if (sourceSrs.Ellipsoid.AreTheSame(targetSrs.Ellipsoid))
            {
                for (int i = 0; i < values.Count; i++)
                {
                    var c1 = values[i].Transform(p => sourceSrs.ToGeodetic(p), SridHelper.GeodeticWGS84);

                    result.Add(c1.Transform(p => targetSrs.FromGeodetic(p), targetSrs.Srid));
                }
            }
            else
            {
                for (int i = 0; i < values.Count; i++)
                {
                    var c1 = values[i].Transform(p => sourceSrs.ToGeodetic(p), SridHelper.GeodeticWGS84);

                    result.Add(c1.Transform(p => targetSrs.FromGeodetic(p, sourceSrs.Ellipsoid), targetSrs.Srid));
                }
            }

            return result;
        }


        #region Geometry To GeoJson

        public static IGeoJsonGeometry AsGeoJson<T>(this T point) where T : IPoint, new()
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

        public static IGeoJsonGeometry AsGeoJson<T>(this Geometry<T> geometry, bool isLongitudeFirst = true) where T : IPoint, new()
        {
            //if (geometry.IsNullOrEmpty())
            //{
            //    throw new NotImplementedException();
            //}            

            switch (geometry.Type)
            {
                case GeometryType.Point:
                    return geometry.GeometryPointToGeoJsonPoint(isLongitudeFirst);

                case GeometryType.LineString:
                    return GeometryLineStringToGeoJsonPolyline(geometry, isLongitudeFirst);

                case GeometryType.Polygon:
                    return GeometryPolygonToGeoJsonPolygon(geometry, isLongitudeFirst);

                case GeometryType.MultiPoint:
                    return GeometryMultiPointToGeoJsonMultiPoint(geometry, isLongitudeFirst);

                case GeometryType.MultiLineString:
                    return GeometryMultiLineStringToGeoJsonPolyline(geometry, isLongitudeFirst);

                case GeometryType.MultiPolygon:
                    return GeometryMultiPolygonToGeoJsonMultiPolygon(geometry, isLongitudeFirst);

                case GeometryType.GeometryCollection:
                case GeometryType.CircularString:
                case GeometryType.CompoundCurve:
                case GeometryType.CurvePolygon:
                default:
                    throw new NotImplementedException();
            }
        }

        private static double[] GetGeoJsonObjectPoint<T>(T point, bool isLongitudeFirst) where T : IPoint, new()
        {
            if (point == null)
                return new double[0];

            if (isLongitudeFirst)
            {
                return new double[] { point.X, point.Y };
            }
            else
            {
                return new double[] { point.Y, point.X };
            }

        }

        private static double[][] GetGeoJsonLineStringOrRing<T>(Geometry<T> lineStringOrRing, bool isLongitudeFirst, bool isRing) where T : IPoint, new()
        {
            if (lineStringOrRing.IsNullOrEmpty())
                return new double[0][];

            int numberOfPoints = lineStringOrRing.NumberOfPoints;

            double[][] result;

            if (isRing)
            {
                // 1400.02.04
                // In GeoJson polygons the last point must be repeated
                result = new double[numberOfPoints + 1][];

                result[numberOfPoints] = GetGeoJsonObjectPoint<T>(lineStringOrRing.Points[0], isLongitudeFirst);
            }
            else
            {
                result = new double[numberOfPoints][];
            }

            for (int i = 0; i < numberOfPoints; i++)
            {
                result[i] = GetGeoJsonObjectPoint<T>(lineStringOrRing.Points[i], isLongitudeFirst);
            }

            return result;
        }

        private static GeoJsonPoint GeometryPointToGeoJsonPoint<T>(this Geometry<T> geometry, bool isLongitudeFirst) where T : IPoint, new()
        {
            //This check is required
            if (geometry.IsNullOrEmpty())
                return new GeoJsonPoint()
                {
                    Type = GeoJson.Point,
                };

            if (isLongitudeFirst)
            {
                return new GeoJsonPoint()
                {
                    Type = GeoJson.Point,
                    Coordinates = new double[] { geometry.Points[0].X, geometry.Points[0].Y }
                };
            }
            else
            {
                return new GeoJsonPoint()
                {
                    Type = GeoJson.Point,
                    Coordinates = new double[] { geometry.Points[0].Y, geometry.Points[0].X }
                };
            }

        }

        private static GeoJsonMultiPoint GeometryMultiPointToGeoJsonMultiPoint<T>(this Geometry<T> geometry, bool isLongitudeFirst) where T : IPoint, new()
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
                points[i] = GetGeoJsonObjectPoint(geometry.Geometries[i].Points[0], isLongitudeFirst);
            }

            return new GeoJsonMultiPoint()
            {
                Coordinates = points,
                Type = GeoJson.MultiPoint,
            };
        }

        //Not supportig Z and M Values
        private static GeoJsonLineString GeometryLineStringToGeoJsonPolyline<T>(this Geometry<T> geometry, bool isLongitudeFirst) where T : IPoint, new()
        {
            //This check is required
            if (geometry.IsNullOrEmpty())
                return new GeoJsonLineString()
                {
                    Type = GeoJson.LineString,
                    Coordinates = new double[0][],
                };

            double[][] paths = GetGeoJsonLineStringOrRing(geometry, isLongitudeFirst, false);

            return new GeoJsonLineString()
            {
                Coordinates = paths,
                Type = GeoJson.LineString,
            };
        }

        //Not supportig Z and M Values
        private static GeoJsonMultiLineString GeometryMultiLineStringToGeoJsonPolyline<T>(this Geometry<T> geometry, bool isLongitudeFirst) where T : IPoint, new()
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
                result[i] = GetGeoJsonLineStringOrRing(geometry.Geometries[i], isLongitudeFirst, false);
            }

            return new GeoJsonMultiLineString()
            {
                Coordinates = result,
                Type = GeoJson.MultiLineString,
            };
        }

        //Not supportig Z and M Values
        private static GeoJsonPolygon GeometryPolygonToGeoJsonPolygon<T>(this Geometry<T> geometry, bool isLongitudeFirst) where T : IPoint, new()
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
                result[i] = GetGeoJsonLineStringOrRing(geometry.Geometries[i], isLongitudeFirst, true);
            }

            //double[][][] rings = new double[1][][] { GetGeoJsonLineStringOrRing(geometry) };

            return new GeoJsonPolygon()
            {
                Coordinates = result,
                Type = GeoJson.Polygon,
            };
        }

        //Not supportig Z and M Values
        private static GeoJsonMultiPolygon GeometryMultiPolygonToGeoJsonMultiPolygon<T>(this Geometry<T> geometry, bool isLongitudeFirst) where T : IPoint, new()
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
                rings[i] = geometry.Geometries[i].GeometryPolygonToGeoJsonPolygon(isLongitudeFirst).Coordinates;
            }

            return new GeoJsonMultiPolygon()
            {
                Coordinates = rings,
                Type = GeoJson.MultiPolygon,
            };
        }

        #endregion

    }
}
