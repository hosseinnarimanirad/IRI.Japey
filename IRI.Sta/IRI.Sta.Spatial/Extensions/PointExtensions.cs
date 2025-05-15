using IRI.Sta.Spatial.Primitives;
using IRI.Sta.SpatialReferenceSystem;
using System.Globalization;
using IRI.Sta.Common.Helpers;
using IRI.Sta.Spatial.Primitives;
using IRI.Sta.Common.Primitives;
using IRI.Sta.Common.Enums;

namespace IRI.Extensions;

public static class PointExtensions
{
    // https://medium.com/swlh/calculating-the-distance-between-two-points-on-earth-bac5cd50c840
    // https://www.movable-type.co.uk/scripts/latlong.html
    // https://stormconsultancy.co.uk/blog/storm-news/the-haversine-formula-in-c-and-sql/
    // https://social.msdn.microsoft.com/Forums/sqlserver/en-US/6a0cc084-5056-4f97-9978-a5f88cb57d0f/stdistance-vs-doing-the-math-manually?forum=sqlspatial
    // https://stackoverflow.com/questions/42237521/sql-server-geography-stdistance-function-is-returning-big-difference-than-other
    // https://stackoverflow.com/questions/27708490/haversine-formula-definition-for-sql
    // https://medium.com/swlh/calculating-the-distance-between-two-points-on-earth-bac5cd50c840
    public static double SphericalDistance(this IPoint firstPoint, IPoint secondPoint)
    {
        //var radius = 6371008.8; // in meters

        //var radius = 6368045.28;
        //var radius = 6367538.5803727582

        var radius = (Ellipsoids.WGS84.SemiMajorAxis.Value + Ellipsoids.WGS84.SemiMinorAxis.Value) / 2.0;

        //            Haversine
        //formula: 	a = sin²(Δφ / 2) + cos φ1 ⋅ cos φ2 ⋅ sin²(Δλ / 2)
        //c = 2 ⋅ atan2( √a, √(1−a) )
        //d = R ⋅ c
        var phi1 = firstPoint.Y * Math.PI / 180.0;

        var phi2 = secondPoint.Y * Math.PI / 180.0;

        var a = Ellipsoids.WGS84.SemiMajorAxis.Value;
        var b = Ellipsoids.WGS84.SemiMinorAxis.Value;
        var meanPhi = (phi1 + phi2) / 2.0;
        var newR = Math.Sqrt(a * a * Math.Cos(meanPhi) * Math.Cos(meanPhi) + b * b * Math.Sin(meanPhi) * Math.Sin(meanPhi));

        var deltaPhi = (secondPoint.Y - firstPoint.Y) * Math.PI / 180.0;

        var deltaLambda = (secondPoint.X - firstPoint.X) * Math.PI / 180.0;

        //var temp = radius * Math.Acos(Math.Cos(phi1) * Math.Cos(phi2) * Math.Cos(deltaLambda) + Math.Sin(phi1) * Math.Sin(phi2)); //72092.799646276282

        var haversine = Math.Sin(deltaPhi / 2.0) * Math.Sin(deltaPhi / 2.0) +
                        Math.Cos(phi1) * Math.Cos(phi2) * Math.Sin(deltaLambda / 2.0) * Math.Sin(deltaLambda / 2.0);

        var c = 2.0 * Math.Atan2(Math.Sqrt(haversine), Math.Sqrt(1 - haversine));

        //var c2 = 2.0 * Math.Asin(Math.Min(1, Math.Sqrt(haversine)));
        //var t3 = radius * c2;

        return newR * c; // in meters
    }

    public static Geometry<Point> AsGeometry(this Point point, int srid)
    {
        return Geometry<Point>.Create(point.X, point.Y, srid);
    }
    public static string AsPolygon(this List<Point> points, Func<Point, Point> transform = null)
    {
        var stringArray = transform == null ? points.Select(i => i.AsExactString()) : points.Select(i => transform(i).AsExactString());

        return string.Format(CultureInfo.InvariantCulture, "POLYGON(({0}))", string.Join(",", stringArray));

    }

    public static string AsPolyline(this List<Point> points, Func<Point, Point> transform = null)
    {
        var stringArray = transform == null ? points.Select(i => i.AsExactString()) : points.Select(i => transform(i).AsExactString());

        return string.Format(CultureInfo.InvariantCulture, "LINESTRING({0})", string.Join(",", stringArray));

    }

    public static byte[] AsWkb(this IPoint point)
    {
        byte[] result = new byte[21];

        result[0] = (byte)WkbByteOrder.WkbNdr;

        Array.Copy(BitConverter.GetBytes((int)WkbGeometryType.Point), 0, result, 1, BaseConversionHelper.IntegerSize);

        Array.Copy(BitConverter.GetBytes(point.X), 0, result, 5, BaseConversionHelper.DoubleSize);

        Array.Copy(BitConverter.GetBytes(point.Y), 0, result, 13, BaseConversionHelper.DoubleSize);

        return result;
    }


}
