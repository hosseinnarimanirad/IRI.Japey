using IRI.Msh.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRI.Msh.Common.Primitives.Wkt
{
    public static class WktParser
    {
        const string Point = "POINT";
        const string MultiPoint = "MULTIPOINT";
        const string LineString = "LINESTRING";
        const string MultiLineString = "MULTILINESTRING";
        const string Polygon = "POLYGON";
        const string MultiPolygon = "MULTIPOLYGON";

        public static Geometry<Point> Parse(string wktString, int srid = 0)
        {
            if (string.IsNullOrWhiteSpace(wktString))
                return Geometry<Point>.Null;

            var typeChars = wktString.TakeWhile(c => c != '(')?.ToArray();

            if (typeChars.IsNullOrEmpty())
                return Geometry<Point>.Null;

            var type = new string(typeChars);

            var coordinates = wktString.Substring(type.Length, wktString.Length - type.Length);

            switch (type.Trim().ToUpper())
            {
                case Point:
                    return ParsePoint(coordinates, srid);

                case MultiPoint:
                    return ParseMultiPoint(coordinates, srid);

                case LineString:
                    return ParseLineString(coordinates, srid);

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

        private static Geometry<Point> ParsePoint(string wktString, int srid)
        {
            var points = GetPoints(wktString);

            return Geometry<Point>.CreatePointOrLineString(points, srid);
        }

        private static Geometry<Point> ParseMultiPoint(string wktString, int srid)
        {
            var cleanedString = wktString.Replace('(', ' ').Replace(')', ' ');

            var points = GetPoints(cleanedString);

            if (points.IsNullOrEmpty())
                return Geometry<Point>.Null;

            return new Geometry<Point>(points.Select(p => Geometry<Point>.Create(p.X, p.Y, srid)).ToList(), GeometryType.MultiPoint, srid);
        }

        private static Geometry<Point> ParseLineString(string wktString, int srid)
        {
            var points = GetPoints(wktString);

            return Geometry<Point>.CreatePointOrLineString(points, srid);
        }

        private static Geometry<Point> ParseMultiLineString(string wktString, int srid)
        {
            var items = Process(wktString);

            List<Geometry<Point>> lineStrings = new List<Geometry<Point>>();

            foreach (var item in items.Where(i => i.level == 2))
            {
                var subString = wktString.Substring(item.start, item.end - item.start);

                lineStrings.Add(ParseLineString(subString, srid));
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

                rings.Add(ParseLineString(subString, srid));
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

                    rings.Add(ParseLineString(subString, srid));
                }

                polygons.Add(new Geometry<Point>(rings, GeometryType.Polygon, srid));
            }

            return new Geometry<Point>(polygons, GeometryType.MultiPolygon, srid);
        }

        public static List<Point> GetPoints(string pointArray)
        {
            var cleanedPointArray = pointArray?.Trim(' ', ')', '(');

            if (string.IsNullOrEmpty(cleanedPointArray))
                return new List<Point>();

            var points = cleanedPointArray
                .Split(',')
                .Select(p => p.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                                .Select(i => double.Parse(i))
                                .ToList());

            return points.Select(p => new Point(p[0], p[1])).ToList();
        }

        // 1400.03.21
        public static List<(int level, int start, int end)> Process(string wktString)
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
    }
}
