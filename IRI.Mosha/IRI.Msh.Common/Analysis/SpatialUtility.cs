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


        /// <summary>
        /// Returns the signed area
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        public static double CalculateArea(IPoint[] points)
        {
            double area = 0;

            for (int i = 0; i < points.Length - 1; i++)
            {
                double temp = points[i].X * points[i + 1].Y - points[i].Y * points[i + 1].X;

                area += temp;
            }

            return area;
        }

        public static double CalculateTriangleArea(IPoint firstPoint, IPoint middlePoint, IPoint lastPoint)
        {
            return Math.Abs(firstPoint.X * (middlePoint.Y - lastPoint.Y) + middlePoint.X * (lastPoint.Y - firstPoint.Y) + lastPoint.X * (firstPoint.Y - middlePoint.Y)) / 2.0;
        }

        public static double CalculateTriangleSignedArea(IPoint firstPoint, IPoint middlePoint, IPoint lastPoint)
        {
            return (firstPoint.X * (middlePoint.Y - lastPoint.Y) + middlePoint.X * (lastPoint.Y - firstPoint.Y) + lastPoint.X * (firstPoint.Y - middlePoint.Y)) / 2.0;
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
