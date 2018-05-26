using IRI.Sta.Common.Primitives;
using IRI.Sta.Common.Ogc;
using IRI.Sta.Common.Primitives;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace System.Collections.Generic
{
    public static class PointExtension
    {
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

            Array.Copy(BitConverter.GetBytes((int)WkbGeometryType.Point), 0, result, 1, 4);

            Array.Copy(BitConverter.GetBytes(point.X), 0, result, 5, 8);

            Array.Copy(BitConverter.GetBytes(point.Y), 0, result, 13, 8);

            return result;
        }
    }
}
