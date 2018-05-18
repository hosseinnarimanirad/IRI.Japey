using IRI.Ham.SpatialBase;
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

            //if (transform == null)
            //{
            //    return string.Format(CultureInfo.InvariantCulture, "POLYGON(({0}))", string.Join(",", points));
            //}
            //else
            //{
            //    var transformedPoints = points.Select(i => transform(i).ToString()).ToList();

            //    return string.Format(CultureInfo.InvariantCulture, "POLYGON(({0}))", string.Join(",", transformedPoints));
            //}

        }

        public static string AsPolyline(this List<Point> points, Func<Point, Point> transform = null)
        {
            var stringArray = transform == null ? points.Select(i => i.AsExactString()) : points.Select(i => transform(i).AsExactString());

            return string.Format(CultureInfo.InvariantCulture, "LINESTRING({0})", string.Join(",", stringArray));

            //if (transform == null)
            //{
            //    return string.Format(CultureInfo.InvariantCulture, "POLYGON(({0}))", string.Join(",", points));
            //}
            //else
            //{
            //    var transformedPoints = points.Select(i => transform(i).ToString()).ToList();

            //    return string.Format(CultureInfo.InvariantCulture, "LINESTRING({0})", string.Join(",", transformedPoints));
            //}
        }
    }
}
