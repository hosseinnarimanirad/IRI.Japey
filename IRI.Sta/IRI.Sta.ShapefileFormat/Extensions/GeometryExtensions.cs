using IRI.Sta.ShapefileFormat.EsriType;
using IRI.Extensions;
using IRI.Sta.Common.Primitives;
using IRI.Msh.CoordinateSystem.MapProjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRI.Extensions
{
    public static class GeometryExtensions
    {
        #region Geometry > Esri Shape

        public static IEsriShape AsEsriShape<T>(this Geometry<T> geometry, int srid, Func<IPoint, IPoint> mapFunction = null) where T : IPoint, new()
        {
            //var type = geometry.GeometryType;
            //if (geometry.IsNullOrEmpty())
            //{
            //    return SqlSpatialHelper.CreateEmptySqlGeometry(type, srid);
            //}

            if (geometry.IsNullOrEmpty())
            {
                return null;
            }

            var type = geometry.Type;

            switch (type)
            {
                case GeometryType.GeometryCollection:
                case GeometryType.CircularString:
                case GeometryType.CompoundCurve:
                case GeometryType.CurvePolygon:
                default:
                    throw new NotImplementedException();

                case GeometryType.Point:
                    return PointToEsriPoint(geometry, srid, mapFunction);

                case GeometryType.MultiPoint:
                    return MultiPointToEsriMultiPoint(geometry, srid, mapFunction);

                case GeometryType.LineString:
                    return LineStringToEsriPolyline(geometry, srid, mapFunction);

                case GeometryType.MultiLineString:
                    return MultiLineStringToEsriPolyline(geometry, srid, mapFunction);

                case GeometryType.Polygon:
                    return PolygonToEsriPolygon(geometry, srid, mapFunction);

                case GeometryType.MultiPolygon:
                    return MultiPolygonToEsriPolygon(geometry, srid, mapFunction);
            }
        }

        public static EsriPoint AsEsriPoint<T>(this Geometry<T> point, int srid) where T : IPoint, new()
        {
            if (point.IsNullOrEmpty())
            {
                return new EsriPoint(double.NaN, double.NaN, 0);
            }
            else
            {
                return new EsriPoint(point.Points.First().X, point.Points.First().Y, srid);
            }
        }

        //Not supportig Z and M Values
        private static EsriPoint PointToEsriPoint<T>(Geometry<T> point, int srid, Func<IPoint, IPoint> mapFunction) where T : IPoint, new()
        {
            var esriPoint = point.AsEsriPoint(srid);

            return mapFunction == null ? esriPoint : (EsriPoint)mapFunction(esriPoint);
        }

        //Not supportig Z and M Values
        private static EsriMultiPoint MultiPointToEsriMultiPoint<T>(Geometry<T> multiPoint, int srid, Func<IPoint, IPoint> mapFunction) where T : IPoint, new()
        {
            if (multiPoint.IsNullOrEmpty())
            {
                return new EsriMultiPoint();
            }

            return new EsriMultiPoint(GetPoints(multiPoint, srid, mapFunction).ToArray());
        }

        //Not supporting Z and M values
        private static EsriPolyline LineStringToEsriPolyline<T>(Geometry<T> lineString, int srid, Func<IPoint, IPoint> mapFunction) where T : IPoint, new()
        {
            if (lineString.IsNullOrEmpty())
            {
                return new EsriPolyline();
            }

            return new EsriPolyline(GetPoints(lineString, srid, mapFunction).ToArray());
        }

        //Not supporting Z and M values
        private static EsriPolyline MultiLineStringToEsriPolyline<T>(Geometry<T> multiLineString, int srid, Func<IPoint, IPoint> mapFunction) where T : IPoint, new()
        {
            if (multiLineString.IsNullOrEmpty())
            {
                return new EsriPolyline();
            }

            int numberOfGeometries = multiLineString.NumberOfGeometries;

            List<EsriPoint> points = new List<EsriPoint>(multiLineString.NumberOfPoints);

            List<int> parts = new List<int>(numberOfGeometries);

            for (int i = 0; i < numberOfGeometries; i++)
            {
                parts.Add(points.Count);

                points.AddRange(GetPoints(multiLineString.Geometries[i], srid, mapFunction));
            }

            return new EsriPolyline(points.ToArray(), parts.ToArray());
        }

        //Not supporting Z and M values
        //check for cw and cww criteria
        private static EsriPolygon PolygonToEsriPolygon<T>(Geometry<T> polygon, int srid, Func<IPoint, IPoint> mapFunction) where T : IPoint, new()
        {
            if (polygon.IsNullOrEmpty())
            {
                return new EsriPolygon();
            }

            int numberOfRings = polygon.NumberOfGeometries;

            List<EsriPoint> points = new List<EsriPoint>(polygon.NumberOfPoints);

            List<int> parts = new List<int>(numberOfRings);

            for (int i = 0; i < numberOfRings; i++)
            {
                parts.Add(points.Count);

                points.AddRange(GetPoints(polygon.Geometries[i], srid, mapFunction));
            }

            return new EsriPolygon(points.ToArray(), parts.ToArray());
        }

        //Not supporting Z and M values
        //check for cw and cww criteria
        private static EsriPolygon MultiPolygonToEsriPolygon<T>(Geometry<T> multiPolygon, int srid, Func<IPoint, IPoint> mapFunction) where T : IPoint, new()
        {
            if (multiPolygon.IsNullOrEmpty())
            {
                return new EsriPolygon();
            }

            int numberOfGeometries = multiPolygon.NumberOfGeometries;

            List<EsriPoint> points = new List<EsriPoint>(multiPolygon.NumberOfPoints);

            List<int> parts = new List<int>(numberOfGeometries);

            for (int i = 0; i < numberOfGeometries; i++)
            {
                var tempPolygon = multiPolygon.Geometries[i];

                var numberOfRings = tempPolygon == null ? 0 : tempPolygon.NumberOfGeometries;

                for (int j = 0; j < numberOfRings; j++)
                {
                    parts.Add(points.Count);

                    points.AddRange(GetPoints(tempPolygon.Geometries[j], srid, mapFunction));
                }
            }

            return new EsriPolygon(points.ToArray(), parts.ToArray());

        }

        private static IEnumerable<EsriPoint> GetPoints<T>(Geometry<T> geometry, int srid, Func<IPoint, IPoint> mapFunction) where T : IPoint, new()
        {
            if (geometry.IsNullOrEmpty())
            {
                return null;
            }

            List<EsriPoint> result = new List<EsriPoint>(geometry.Points.Count);

            foreach (var point in geometry.Points)
            {
                if (mapFunction == null)
                {
                    result.Add(new EsriPoint(point.X, point.Y, srid));
                }
                else
                {
                    var temporaryPoint = mapFunction(point);

                    result.Add(new EsriPoint(temporaryPoint.X, temporaryPoint.Y, srid));
                }
            }

            return result;
        }

        #endregion

        public static void SaveAsShapefile<T>(this Feature<T> feature, string shpFileName, SrsBase srs = null) where T : IPoint, new()
        {
            if (feature == null)
                return;

            IRI.Sta.ShapefileFormat.Shapefile.SaveAsShapefile(shpFileName, new List<Feature<T>>() { feature }, srs);
        }

        public static void SaveAsShapefile<T>(this IEnumerable<Feature<T>> features, string shpFileName, SrsBase srs = null) where T : IPoint, new()
        {
            if (features.IsNullOrEmpty())
                return;

            IRI.Sta.ShapefileFormat.Shapefile.SaveAsShapefile(shpFileName, features, srs);
        }

    }
}
