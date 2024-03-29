﻿using IRI.Msh.Common.Analysis;
using IRI.Msh.Common.Model.GeoJson;
using IRI.Msh.Common.Primitives;
using IRI.Extensions;
using IRI.Msh.CoordinateSystem.MapProjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using IRI.Msh.Common.Ogc;
using System.Diagnostics;

namespace IRI.Extensions
{
    public static class Msh_GeometryExtensions
    {
        public static BoundingBox GetBoundingBox<T>(this IEnumerable<Geometry<T>> spatialFeatures) where T : IPoint, new()
        {
            if (spatialFeatures.IsNullOrEmpty() /*== null || spatialFeatures.Count < 1*/)
                return new BoundingBox(double.NaN, double.NaN, double.NaN, double.NaN);

            var envelopes = spatialFeatures.Select(i => i?.GetBoundingBox()).Where(i => i != null).Select(i => i.Value).ToList();

            return BoundingBox.GetMergedBoundingBox(envelopes, true);
        }


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


        public static bool IsNullOrEmpty<T>(this Geometry<T> geometry) where T : IPoint, new()
        {
            return geometry is null ||
                    (geometry.Points.IsNullOrEmpty() && geometry.Geometries.IsNullOrEmpty() || geometry.TotalNumberOfPoints == 0);
        }

        public static bool IsNotValidOrEmpty<T>(this Geometry<T> geometry) where T : IPoint, new()
        {
            return geometry.IsNullOrEmpty() || !geometry.IsValid();
        }

        #region Geometry To GeoJson

        // public methods
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

        // private methods
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

        private static GeoJsonPoint GeometryPointToGeoJsonPoint<T>(this Geometry<T> point, bool isLongitudeFirst) where T : IPoint, new()
        {
            //This check is required
            if (point.IsNullOrEmpty())
                return new GeoJsonPoint()
                {
                    Type = GeoJson.Point,
                };

            if (isLongitudeFirst)
            {
                return new GeoJsonPoint()
                {
                    Type = GeoJson.Point,
                    Coordinates = new double[] { point.Points[0].X, point.Points[0].Y }
                };
            }
            else
            {
                return new GeoJsonPoint()
                {
                    Type = GeoJson.Point,
                    Coordinates = new double[] { point.Points[0].Y, point.Points[0].X }
                };
            }

        }

        private static GeoJsonMultiPoint GeometryMultiPointToGeoJsonMultiPoint<T>(this Geometry<T> multiPoint, bool isLongitudeFirst) where T : IPoint, new()
        {
            //This check is required
            if (multiPoint.IsNullOrEmpty())
                return new GeoJsonMultiPoint()
                {
                    Type = GeoJson.MultiPoint,
                    Coordinates = new double[0][],
                };

            var numberOfGeometries = multiPoint.NumberOfGeometries;

            double[][] points = new double[numberOfGeometries][];

            for (int i = 0; i < numberOfGeometries; i++)
            {
                points[i] = GetGeoJsonObjectPoint(multiPoint.Geometries[i].Points[0], isLongitudeFirst);
            }

            return new GeoJsonMultiPoint()
            {
                Coordinates = points,
                Type = GeoJson.MultiPoint,
            };
        }

        //Not supportig Z and M Values
        private static GeoJsonLineString GeometryLineStringToGeoJsonPolyline<T>(this Geometry<T> lineString, bool isLongitudeFirst) where T : IPoint, new()
        {
            //This check is required
            if (lineString.IsNullOrEmpty())
                return new GeoJsonLineString()
                {
                    Type = GeoJson.LineString,
                    Coordinates = new double[0][],
                };

            double[][] paths = GetGeoJsonLineStringOrRing(lineString, isLongitudeFirst, false);

            return new GeoJsonLineString()
            {
                Coordinates = paths,
                Type = GeoJson.LineString,
            };
        }

        //Not supportig Z and M Values
        private static GeoJsonMultiLineString GeometryMultiLineStringToGeoJsonPolyline<T>(this Geometry<T> multiLineString, bool isLongitudeFirst) where T : IPoint, new()
        {
            //This check is required
            if (multiLineString.IsNullOrEmpty())
                return new GeoJsonMultiLineString()
                {
                    Type = GeoJson.MultiLineString,
                    Coordinates = new double[0][][],
                };

            int numberOfParts = multiLineString.NumberOfGeometries;

            double[][][] result = new double[numberOfParts][][];

            for (int i = 0; i < numberOfParts; i++)
            {
                result[i] = GetGeoJsonLineStringOrRing(multiLineString.Geometries[i], isLongitudeFirst, false);
            }

            return new GeoJsonMultiLineString()
            {
                Coordinates = result,
                Type = GeoJson.MultiLineString,
            };
        }

        //Not supportig Z and M Values
        private static GeoJsonPolygon GeometryPolygonToGeoJsonPolygon<T>(this Geometry<T> polygon, bool isLongitudeFirst) where T : IPoint, new()
        {
            //This check is required
            if (polygon.IsNullOrEmpty())
                return new GeoJsonPolygon()
                {
                    Type = GeoJson.Polygon,
                    Coordinates = new double[0][][],
                };


            int numberOfParts = polygon.NumberOfGeometries;

            double[][][] result = new double[numberOfParts][][];

            for (int i = 0; i < numberOfParts; i++)
            {
                result[i] = GetGeoJsonLineStringOrRing(polygon.Geometries[i], isLongitudeFirst, true);
            }

            //double[][][] rings = new double[1][][] { GetGeoJsonLineStringOrRing(geometry) };

            return new GeoJsonPolygon()
            {
                Coordinates = result,
                Type = GeoJson.Polygon,
            };
        }

        //Not supportig Z and M Values
        private static GeoJsonMultiPolygon GeometryMultiPolygonToGeoJsonMultiPolygon<T>(this Geometry<T> multiPolygon, bool isLongitudeFirst) where T : IPoint, new()
        {
            //This check is required
            if (multiPolygon.IsNullOrEmpty())
                return new GeoJsonMultiPolygon()
                {
                    Type = GeoJson.MultiPolygon,
                    Coordinates = new double[0][][][],
                };

            int numberOfParts = multiPolygon.NumberOfGeometries;

            double[][][][] rings = new double[numberOfParts][][][];

            for (int i = 0; i < numberOfParts; i++)
            {
                rings[i] = multiPolygon.Geometries[i].GeometryPolygonToGeoJsonPolygon(isLongitudeFirst).Coordinates;
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
