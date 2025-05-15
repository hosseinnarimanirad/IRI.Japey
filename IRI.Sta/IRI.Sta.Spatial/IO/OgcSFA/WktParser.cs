using System.Text;

using IRI.Extensions;
using IRI.Sta.Common.Abstrations;
using IRI.Sta.Common.Primitives;
using IRI.Sta.Spatial.Primitives;

namespace IRI.Sta.Spatial.IO.OgcSFA;

public static class WktParser
{
    const string Point = "POINT";
    const string MultiPoint = "MULTIPOINT";
    const string LineString = "LINESTRING";
    const string MultiLineString = "MULTILINESTRING";
    const string Polygon = "POLYGON";
    const string MultiPolygon = "MULTIPOLYGON";

    #region Wkt to Geometry

    public static Geometry<Point> Parse(string wktString, int srid = 0)
    {
        if (string.IsNullOrWhiteSpace(wktString))
            return Geometry<Point>.Empty;

        var typeChars = wktString.TakeWhile(c => c != '(')?.ToArray();

        if (typeChars.IsNullOrEmpty())
            return Geometry<Point>.Empty;

        var type = new string(typeChars);

        var coordinates = wktString.Substring(type.Length, wktString.Length - type.Length);

        switch (type.Trim().ToUpper())
        {
            case Point:
                return ParsePoint(coordinates, srid, isRing: false);

            case MultiPoint:
                return ParseMultiPoint(coordinates, srid, isRing: false);

            case LineString:
                return ParseLineString(coordinates, srid, isRing: false);

            case MultiLineString:
                return ParseMultiLineString(coordinates, srid);

            case Polygon:
                return ParsePolygon(coordinates, srid);

            case MultiPolygon:
                return ParseMultiPolygon(coordinates, srid);

            default:
                throw new NotImplementedException("WktParser > Parse");
        }
    }

    private static Geometry<Point> ParsePoint(string wktString, int srid, bool isRing)
    {
        var points = GetPoints(wktString, isRing);

        return Geometry<Point>.CreatePointOrLineString(points, srid);
    }

    private static Geometry<Point> ParseMultiPoint(string wktString, int srid, bool isRing)
    {
        var cleanedString = wktString.Replace('(', ' ').Replace(')', ' ');

        var points = GetPoints(cleanedString, isRing);

        if (points.IsNullOrEmpty())
            return Geometry<Point>.Empty;

        return new Geometry<Point>(points.Select(p => Geometry<Point>.Create(p.X, p.Y, srid)).ToList(), GeometryType.MultiPoint, srid);
    }

    private static Geometry<Point> ParseLineString(string wktString, int srid, bool isRing)
    {
        var points = GetPoints(wktString, isRing);

        return Geometry<Point>.CreatePointOrLineString(points, srid);
    }

    private static Geometry<Point> ParseMultiLineString(string wktString, int srid)
    {
        var items = Process(wktString);

        List<Geometry<Point>> lineStrings = new List<Geometry<Point>>();

        foreach (var item in items.Where(i => i.level == 2))
        {
            var subString = wktString.Substring(item.start, item.end - item.start);

            lineStrings.Add(ParseLineString(subString, srid, isRing: false));
        }

        return new Geometry<Point>(lineStrings, GeometryType.MultiLineString, srid);
    }

    private static Geometry<Point> ParsePolygon(string wktString, int srid)
    {
        var items = Process(wktString);

        List<Geometry<Point>> rings = new List<Geometry<Point>>();

        foreach (var item in items.Where(i => i.level == 2))
        {
            var subString = wktString.Substring(item.start, item.end - item.start);

            rings.Add(ParseLineString(subString, srid, isRing: true));
        }

        return new Geometry<Point>(rings, GeometryType.Polygon, srid);
    }

    private static Geometry<Point> ParseMultiPolygon(string wktString, int srid)
    {
        var items = Process(wktString);

        List<Geometry<Point>> polygons = new List<Geometry<Point>>();

        foreach (var item in items.Where(i => i.level == 2))
        {
            List<Geometry<Point>> rings = new List<Geometry<Point>>();

            foreach (var ring in items.Where(i => i.level == 3 && i.end < item.end && i.start > item.start))
            {
                var subString = wktString.Substring(ring.start, ring.end - ring.start);

                rings.Add(ParseLineString(subString, srid, isRing: true));
            }

            polygons.Add(new Geometry<Point>(rings, GeometryType.Polygon, srid));
        }

        return new Geometry<Point>(polygons, GeometryType.MultiPolygon, srid);
    }

    private static List<Point> GetPoints(string pointArray, bool isRing)
    {
        var cleanedPointArray = pointArray?.Trim(' ', ')', '(');

        if (string.IsNullOrEmpty(cleanedPointArray))
            return new List<Point>();

        var points = cleanedPointArray
            .Split(',')
            .Select(p => p.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                            .Select(i => double.Parse(i))
                            .ToList());

        // 1401.03.13
        // نقطه اخر در
        // wkt
        // تکراری است برای رینگ‌ها
        if (isRing)
        {
            return points.Select(p => new Point(p[0], p[1])).Take(points.Count() - 1).ToList();
        }
        else
        {
            return points.Select(p => new Point(p[0], p[1])).ToList();
        }
    }

    // 1400.03.21
    private static List<(int level, int start, int end)> Process(string wktString)
    {
        int currentLevel = 0;

        List<(int level, int start, int end)> result = new List<(int level, int start, int end)>();

        Stack<int> startIndex = new Stack<int>();

        for (int i = 0; i < wktString.Length; i++)
        {
            if (wktString[i] == '(')
            {
                startIndex.Push(i);
                currentLevel++;
            }

            if (wktString[i] == ')')
            {
                result.Add((currentLevel, startIndex.Pop(), i));
                currentLevel--;
            }
        }

        return result;
    }

    #endregion

    #region Geometry To Wkt

    internal static string AsWkt<T>(Geometry<T> geometry) where T : IPoint, new()
    {
        switch (geometry.Type)
        {
            case GeometryType.Point:
                return FormattableString.Invariant($"POINT{ToWktPointArrayString(geometry, isRingBase: false)}");

            case GeometryType.LineString:
                return FormattableString.Invariant($"LINESTRING{ToWktPointArrayString(geometry, isRingBase: false)}");

            case GeometryType.Polygon:
                return FormattableString.Invariant($"POLYGON{ToWktPointArrayString(geometry, isRingBase: true)}");

            case GeometryType.MultiPoint:
                return FormattableString.Invariant($"MULTIPOINT{ToWktPointArrayString(geometry, isRingBase: false)}");

            case GeometryType.MultiLineString:
                return FormattableString.Invariant($"MULTILINESTRING{ToWktPointArrayString(geometry, isRingBase: false)}");

            case GeometryType.MultiPolygon:
                return FormattableString.Invariant($"MULTIPOLYGON{ToWktPointArrayString(geometry, isRingBase: true)}");

            case GeometryType.GeometryCollection:
            case GeometryType.CircularString:
            case GeometryType.CompoundCurve:
            case GeometryType.CurvePolygon:
            default:
                throw new NotImplementedException("Geometry > ToWktPointArrayString");
        }
    }

    private static string ToWktPointArrayString<T>(Geometry<T> geometry, bool isRingBase) where T : IPoint, new()
    {
        switch (geometry.Type)
        {
            case GeometryType.Point:
                return FormattableString.Invariant($"({geometry.Points[0].X.ToInvariantString()} {geometry.Points[0].Y.ToInvariantString()})");

            case GeometryType.LineString:
                return GetWktLineString(geometry.Points, isRingBase);

            case GeometryType.Polygon:
            case GeometryType.MultiPoint:
            case GeometryType.MultiLineString:
            case GeometryType.MultiPolygon:
                return GetWktLineStringForGeometry(geometry, isRingBase);

            case GeometryType.GeometryCollection:
            case GeometryType.CircularString:
            case GeometryType.CompoundCurve:
            case GeometryType.CurvePolygon:
            default:
                throw new NotImplementedException("Geometry > ToWktPointArrayString");
        }
    }

    // polygon, multi point, multi linestring, multipolygon
    private static string GetWktLineStringForGeometry<T>(Geometry<T> geometry, bool isRingBase) where T : IPoint, new()
    {
        var items = geometry.Geometries.Select(g => ToWktPointArrayString(g, isRingBase));

        StringBuilder result = new StringBuilder("(");

        foreach (var ring in items)
        {
            result.Append(FormattableString.Invariant($"{ring},"));
        }

        result.Remove(result.Length - 1, 1);

        result.Append(")");

        return result.ToString();
    }

    private static string GetWktLineString<T>(List<T> points, bool isRingBase) where T : IPoint, new()
    {
        StringBuilder builder = new StringBuilder("(");

        foreach (var point in points)
        {
            builder.Append(FormattableString.Invariant($"{point.X} {point.Y},"));
        }

        if (isRingBase)
        {
            builder.Append(FormattableString.Invariant($"{points[0].X} {points[0].Y},"));
        }

        builder.Remove(builder.Length - 1, 1);

        builder.Append(")");

        return builder.ToString();
    }


    #endregion
}
