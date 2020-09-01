using IRI.Msh.Common.Primitives;
using IRI.Msh.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IRI.Msh.Common.Analysis
{
    public class SpatialUtility
    {
        public static double GetSemiDistance(IPoint first, IPoint second)
        {
            var dx = first.X - second.X;

            var dy = first.Y - second.Y;

            return dx * dx + dy * dy;
        }


        //1399.06.11
        //در این جا فرض شده که نقطه اخر چند حلقه تکرار 
        //نشده
        /// <summary>
        /// Calculate unsigned Euclidean area for ring. 
        /// </summary>
        /// <param name="points">last point should not be repeated for ring</param>
        /// <returns></returns>
        public static double CalculateUnsignedAreaForRing(IPoint[] points)
        {
            if (points == null || points.Length < 3)
                return 0;

            double area = 0;

            for (int i = 0; i < points.Length - 1; i++)
            {
                double temp = points[i].X * points[i + 1].Y - points[i].Y * points[i + 1].X;

                area += temp;
            }

            //1399.06.11
            //تکرار نقطه اخر چند ضلعی
            //فرض بر این هست که داخل لیست نقطه‌ها
            //این نقطه تکرار نشده باشه
            area += points[points.Length - 1].X * points[0].Y - points[points.Length - 1].Y * points[0].X;

            return Math.Abs(area / 2.0);
        }


        /// <summary>
        /// Calculate unsigned Euclidean area for triangle
        /// </summary>
        /// <param name="firstPoint"></param>
        /// <param name="middlePoint"></param>
        /// <param name="lastPoint"></param>
        /// <returns></returns>
        public static double CalculateUnsignedTriangleArea(IPoint firstPoint, IPoint middlePoint, IPoint lastPoint)
        {
            return Math.Abs(CalculateSignedTriangleArea(firstPoint, middlePoint, lastPoint));
        }


        /// <summary>
        /// Calculate signed Euclidean area for triangle
        /// </summary>
        /// <param name="firstPoint"></param>
        /// <param name="middlePoint"></param>
        /// <param name="lastPoint"></param>
        /// <returns></returns>
        public static double CalculateSignedTriangleArea(IPoint firstPoint, IPoint middlePoint, IPoint lastPoint)
        {
            return (firstPoint.X * (middlePoint.Y - lastPoint.Y) + middlePoint.X * (lastPoint.Y - firstPoint.Y) + lastPoint.X * (firstPoint.Y - middlePoint.Y)) / 2.0;
        }

        //1399.06.11
        //مساحت مثلت‌های تشکیل دهنده یک خط یا 
        //حلقه توسط نقاط ورودی
        public static List<double> GetPrimitiveAreas(IPoint[] points, bool isClosed)
        {
            List<double> result = new List<double>();

            var n = points.Length;

            if (points == null || n < 3)
                return result;

            //double[] result = new double[n - 1];

            for (int i = 0; i < n - 2; i++)
            {
                //result[i] = CalculateUnsignedTriangleArea(points[i], points[i + 1], points[i + 2]);
                result.Add(CalculateUnsignedTriangleArea(points[i], points[i + 1], points[i + 2]));
            }

            if (isClosed && n > 3)
            {
                //result[n - 2] = CalculateUnsignedTriangleArea(points[n - 2], points[n - 1], points[0]);
                result.Add(CalculateUnsignedTriangleArea(points[n - 2], points[n - 1], points[0]));
            }

            return result;
        }

        //1399.06.11
        //مساحت مثلت‌های تشکیل دهنده شکل هندسی
        public static List<double> GetPrimitiveAreas(Geometry geometry)
        {
            var result = new List<double>();

            if (geometry == null)
            {
                return result;
            }

            switch (geometry.Type)
            {
                case GeometryType.Point:
                case GeometryType.MultiPoint:
                    return result;

                case GeometryType.LineString:
                    return GetPrimitiveAreas(geometry.Points, false);

                case GeometryType.Polygon:
                    return geometry.Geometries.SelectMany(g => GetPrimitiveAreas(g.Points, true)).ToList();

                case GeometryType.MultiLineString:
                case GeometryType.MultiPolygon:
                    return geometry.Geometries.SelectMany(g => GetPrimitiveAreas(g)).ToList();

                case GeometryType.GeometryCollection:
                case GeometryType.CircularString:
                case GeometryType.CompoundCurve:
                case GeometryType.CurvePolygon:
                default:
                    throw new NotImplementedException("SpatialUtility.cs > GetPrimitiveAreas");
            }
        }

        public static List<double> GetPrimitiveAreas(List<Geometry> geometries)
        {
            if (geometries == null)
            {
                return new List<double>();
            }

            return geometries.SelectMany(g => GetPrimitiveAreas(g)).ToList();
        }

        public static double GetCosineOfAngle(IPoint p1, IPoint p2, IPoint p3)
        {
            if (p1.Equals(p2) || p2.Equals(p3))
            {
                return 1;
            }

            //cos(theta) = (A.B)/(|A|*|B|)
            var ax = p3.X - p2.X;
            var ay = p3.Y - p2.Y;

            var bx = p2.X - p1.X;
            var by = p2.Y - p1.Y;

            var dotProduct = ax * bx + ay * by;

            var result = dotProduct * dotProduct / ((ax * ax + ay * ay) * (bx * bx + by * by));

            if (double.IsNaN(result))
            {

            }

            return result;
        }

        public static double[] GetCosineOfAngles(IPoint[] points)
        {
            if (points == null || points.Length == 0 || points.Length == 2)
                return null;

            double[] result = new double[points.Length - 2];

            for (int i = 0; i < points.Length - 2; i++)
            {
                result[i] = GetCosineOfAngle(points[i], points[i + 1], points[i + 2]);
            }

            return result;
        }

        private static double CalculateDotProduct(IPoint firstPoint, IPoint middlePoint, IPoint lastPoint)
        {
            if (firstPoint.Equals(middlePoint) || middlePoint.Equals(lastPoint))
            {
                return 1;
            }

            //cos(theta) = (A.B)/(|A|*|B|)
            var ax = lastPoint.X - middlePoint.X;
            var ay = lastPoint.Y - middlePoint.Y;

            var bx = middlePoint.X - firstPoint.X;
            var by = middlePoint.Y - firstPoint.Y;

            var dotProduct = ax * bx + ay * by;

            return dotProduct;
        }

        public static double CalculateSemiCosineOfAngle(IPoint firstPoint, IPoint middlePoint, IPoint lastPoint)
        {
            if (firstPoint.Equals(middlePoint) || middlePoint.Equals(lastPoint))
            {
                return 1;
            }

            //cos(theta) = (A.B)/(|A|*|B|)
            var ax = lastPoint.X - middlePoint.X;
            var ay = lastPoint.Y - middlePoint.Y;

            var bx = middlePoint.X - firstPoint.X;
            var by = middlePoint.Y - firstPoint.Y;

            var dotProduct = ax * bx + ay * by;

            //result: cos(theta)^2
            var result = dotProduct * dotProduct / ((ax * ax + ay * ay) * (bx * bx + by * by));

            if (double.IsNaN(result))
            {

            }

            return dotProduct < 0 ? -1 * result : result;
        }



        /// <summary>
        /// Checks if sequence of points are clockwise or not
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        public static bool IsClockwise(IList<IPoint> points)
        {
            int numberOfPoints = points.Count;

            List<double> values = new List<double>(numberOfPoints);

            for (int i = 0; i < numberOfPoints - 1; i++)
            {
                values.Add((points[i + 1].X - points[i].X) * (points[i + 1].Y + points[i].Y));
            }

            return values.Sum() > 0;
        }

        /// <summary>
        /// Checks if sequence of points are clockwise or not
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        public static bool IsClockwise(IPoint[] points)
        {
            int numberOfPoints = points.Length;

            List<double> values = new List<double>(numberOfPoints);

            for (int i = 0; i < numberOfPoints - 1; i++)
            {
                values.Add((points[i + 1].X - points[i].X) * (points[i + 1].Y + points[i].Y));
            }

            return values.Sum() > 0;
        }



        public static bool CircleRectangleIntersects(IPoint circleCenter, double circleRadius, BoundingBox axisAlignedRectangle)
        {
            var rectangleCenter = axisAlignedRectangle.Center;

            //Circle.Center.X - Rectangle.Center.X
            var xDifference = Math.Abs(circleCenter.X - rectangleCenter.X);

            //Circle.Center.Y - Rectangle.Center.Y
            var yDifference = Math.Abs(circleCenter.Y - rectangleCenter.Y);

            var rectangleHalfWidth = axisAlignedRectangle.Width / 2.0;
            var rectangleHalfHeight = axisAlignedRectangle.Height / 2.0;

            if (xDifference > (rectangleHalfWidth + circleRadius)) { return false; }
            if (yDifference > (rectangleHalfHeight + circleRadius)) { return false; }

            if (xDifference <= rectangleHalfWidth) { return true; }
            if (yDifference <= rectangleHalfHeight) { return true; }


            var cornerDistance = (xDifference - rectangleHalfWidth) * (xDifference - rectangleHalfWidth) +
                                    (yDifference - rectangleHalfHeight) * (yDifference - rectangleHalfHeight);

            return cornerDistance <= (circleRadius * circleRadius);
        }

        public static bool IsAxisAlignedRectangleInsideCircle(IPoint circleCenter, double circleRadius, BoundingBox axisAlignedRectangle)
        {
            var rectangleCenter = axisAlignedRectangle.Center;

            var xDifference = Math.Abs(circleCenter.X - rectangleCenter.X);
            var yDifference = Math.Abs(circleCenter.Y - rectangleCenter.Y);

            var rectangleHalfWidth = axisAlignedRectangle.Width / 2.0;
            var rectangleHalfHeight = axisAlignedRectangle.Height / 2.0;

            if (xDifference > (rectangleHalfWidth + circleRadius)) { return false; }
            if (yDifference > (rectangleHalfHeight + circleRadius)) { return false; }

            var cornerDistance = (xDifference + rectangleHalfWidth) * (xDifference + rectangleHalfWidth) +
                                    (yDifference + rectangleHalfHeight) * (yDifference + rectangleHalfHeight);

            return cornerDistance <= (circleRadius * circleRadius);
        }

        /// <summary>
        /// returns whether the rectangle is contained by the circle or they intersects or they are disjoint
        /// </summary>
        /// <param name="circleCenter"></param>
        /// <param name="circleRadius"></param>
        /// <param name="axisAlignedRectangle"></param>
        /// <returns></returns>
        public static SpatialRelation CalculateAxisAlignedRectangleRelationToCircle(IPoint circleCenter, double circleRadius, BoundingBox axisAlignedRectangle)
        {
            var rectangleCenter = axisAlignedRectangle.Center;

            var xDifference = Math.Abs(circleCenter.X - rectangleCenter.X);
            var yDifference = Math.Abs(circleCenter.Y - rectangleCenter.Y);

            var rectangleHalfWidth = axisAlignedRectangle.Width / 2.0;
            var rectangleHalfHeight = axisAlignedRectangle.Height / 2.0;

            if (xDifference > (rectangleHalfWidth + circleRadius)) { return SpatialRelation.Disjoint; }
            if (yDifference > (rectangleHalfHeight + circleRadius)) { return SpatialRelation.Disjoint; }

            var semiRadius = circleRadius * circleRadius;

            if (xDifference <= rectangleHalfWidth || yDifference <= rectangleHalfHeight)
            {
                var farCornerDistance = (xDifference + rectangleHalfWidth) * (xDifference + rectangleHalfWidth) +
                                        (yDifference + rectangleHalfHeight) * (yDifference + rectangleHalfHeight);

                if (farCornerDistance <= semiRadius)
                {
                    return SpatialRelation.Contained;
                }

                return SpatialRelation.Intersects;
            }

            var nearCornerDistance = (xDifference - rectangleHalfWidth) * (xDifference - rectangleHalfWidth) +
                                    (yDifference - rectangleHalfHeight) * (yDifference - rectangleHalfHeight);

            if (nearCornerDistance <= semiRadius)
                return SpatialRelation.Intersects;
            else
            {
                return SpatialRelation.Disjoint;
            }
        }






    }
}
