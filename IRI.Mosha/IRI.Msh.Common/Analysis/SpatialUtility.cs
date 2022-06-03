using IRI.Msh.Common.Primitives;
using IRI.Msh.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using IRI.Msh.MeasurementUnit;

namespace IRI.Msh.Common.Analysis
{
    public static class SpatialUtility
    {
        public const double EpsilonDistance = 0.0000001;

        /// <summary>
        /// return square (^2) of the Euclidian distance between two
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static double GetSquareEuclideanDistance(IPoint first, IPoint second)
        {
            var dx = first.X - second.X;

            var dy = first.Y - second.Y;

            return dx * dx + dy * dy;
        }

        /// <summary>
        /// Euclidian distance
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static double GetEuclideanDistance(IPoint first, IPoint second)
        {
            return Math.Sqrt(GetSquareEuclideanDistance(first, second));
        }

        #region Area

        /// <summary>
        /// Calculate Signed Euclidean area for ring. 
        /// Clockwise rings have negative area and CounterClockwise rings have positive area
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="points">last point should not be repeated for ring</param>
        /// <returns></returns>
        public static double GetSignedRingArea<T>(List<T> points) where T : IPoint, new()
        {
            if (points == null || points.Count < 3)
                return 0;

            if (SpatialUtility.GetEuclideanDistance(points[0], points[points.Count - 1]) == 0)
            {
                throw new NotImplementedException("SpatialUtility > CalculateSignedTriangleAreaForRing");
            }

            double area = 0;

            for (int i = 0; i < points.Count - 1; i++)
            {
                double temp = points[i].X * points[i + 1].Y - points[i].Y * points[i + 1].X;

                area += temp;
            }

            // 1399.06.11
            // تکرار نقطه اخر چند ضلعی
            // فرض بر این هست که داخل لیست نقطه‌ها
            // این نقطه تکرار نشده باشه
            area += points[points.Count - 1].X * points[0].Y - points[points.Count - 1].Y * points[0].X;

            return area / 2.0;
        }

        //1399.06.11
        //در این جا فرض شده که نقطه اخر چند حلقه تکرار 
        //نشده
        /// <summary>
        /// Calculate unsigned Euclidean area for ring. 
        /// </summary>
        /// <param name="points">last point should not be repeated for ring</param>
        /// <returns></returns>
        public static double GetUnsignedRingArea<T>(List<T> points) where T : IPoint, new()
        {
            return Math.Abs(GetSignedRingArea(points));
        }

        /// <summary>
        /// Calculate unsigned Euclidean area for triangle
        /// </summary>
        /// <param name="firstPoint"></param>
        /// <param name="middlePoint"></param>
        /// <param name="lastPoint"></param>
        /// <returns></returns>
        public static double GetUnsignedTriangleArea(IPoint firstPoint, IPoint middlePoint, IPoint lastPoint)
        {
            return Math.Abs(GetSignedTriangleArea(firstPoint, middlePoint, lastPoint));
        }

        /// <summary>
        /// Calculate signed Euclidean area for triangle
        /// Clockwise triangles have negative area and CounterClockwise triangles have positive area
        /// </summary>
        /// <param name="firstPoint"></param>
        /// <param name="middlePoint"></param>
        /// <param name="lastPoint"></param>
        /// <returns></returns>
        public static double GetSignedTriangleArea(IPoint firstPoint, IPoint middlePoint, IPoint lastPoint)
        {
            return (firstPoint.X * (middlePoint.Y - lastPoint.Y) + middlePoint.X * (lastPoint.Y - firstPoint.Y) + lastPoint.X * (firstPoint.Y - middlePoint.Y)) / 2.0;
        }

        #endregion


        #region Primitive Area

        //1399.06.11
        //مساحت مثلت‌های تشکیل دهنده یک خط یا 
        //حلقه توسط نقاط ورودی
        public static List<double> GetPrimitiveAreas<T>(IEnumerable<T> points, bool isRing) where T : IPoint
        {
            List<double> result = new List<double>();

            var n = points.Count();

            if (points == null || n < 3)
                return result;

            for (int i = 0; i < n - 2; i++)
            {
                result.Add(GetUnsignedTriangleArea(points.ElementAt(i), points.ElementAt(i + 1), points.ElementAt(i + 2)));
            }

            if (isRing && n > 3)
            {
                result.Add(GetUnsignedTriangleArea(points.ElementAt(n - 2), points.ElementAt(n - 1), points.ElementAt(0)));

                result.Add(GetUnsignedTriangleArea(points.ElementAt(n - 1), points.ElementAt(0), points.ElementAt(1)));
            }

            return result;
        }

        //1399.06.11
        //مساحت مثلت‌های تشکیل دهنده شکل هندسی
        public static List<double> GetPrimitiveAreas<T>(Geometry<T> geometry) where T : IPoint, new()
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

        public static List<double> GetPrimitiveAreas<T>(List<Geometry<T>> geometries) where T : IPoint, new()
        {
            if (geometries == null)
            {
                return new List<double>();
            }

            return geometries.SelectMany(g => GetPrimitiveAreas(g)).ToList();
        }

        #endregion


        #region Angle


        /// <summary>
        /// returns the angle in desired mode between 0 and 180
        /// </summary>
        /// <param name="firstPoint"></param>
        /// <param name="middlePoint"></param>
        /// <param name="lastPoint"></param>
        /// <returns></returns>
        public static double GetAngle<T>(T firstPoint, T middlePoint, T lastPoint, AngleMode mode = AngleMode.Radian) where T : IPoint
        {
            var radianAngle = Math.Acos(GetCosineOfAngle(firstPoint, middlePoint, lastPoint));

            if (mode == AngleMode.Radian)
            {
                return radianAngle;
            }
            else if (mode == AngleMode.Degree)
            {
                return UnitConversion.RadianToDegree(radianAngle);
            }
            else if (mode == AngleMode.Grade)
            {
                return UnitConversion.RadianToGrade(radianAngle);
            }

            throw new NotImplementedException("SpatialUtility > GetAngle");
        }

        /// <summary>
        /// returns cos(theta)
        /// </summary>
        /// <param name="firstPoint"></param>
        /// <param name="middlePoint"></param>
        /// <param name="lastPoint"></param>
        /// <returns></returns>
        public static double GetCosineOfAngle<T>(T firstPoint, T middlePoint, T lastPoint) where T : IPoint
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

            var result = dotProduct / (Math.Sqrt((ax * ax + ay * ay) * (bx * bx + by * by)));

            if (double.IsNaN(result))
            {

            }

            return result;
        }

        public static double[] GetCosineOfAngles<T>(T[] points) where T : IPoint
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

        /// <summary>
        /// return cos(theta)^2
        /// </summary>
        /// <param name="firstPoint"></param>
        /// <param name="middlePoint"></param>
        /// <param name="lastPoint"></param>
        /// <returns></returns>
        public static double GetSquareCosineOfAngle<T>(T firstPoint, T middlePoint, T lastPoint) where T : IPoint
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

            //return dotProduct < 0 ? -1 * result : result;
            return result;
        }

        #endregion


        #region Rotation

        /// <summary>
        /// Checks if sequence of points are clockwise or not
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        public static bool IsClockwise<T>(IList<T> points) where T : IPoint
        {
            int numberOfPoints = points.Count;

            List<double> values = new List<double>(numberOfPoints);

            for (int i = 0; i < numberOfPoints - 1; i++)
            {
                values.Add((points[i + 1].X - points[i].X) * (points[i + 1].Y + points[i].Y));
            }

            return values.Sum() > 0;
        }

        ///// <summary>
        ///// Checks if sequence of points are clockwise or not
        ///// </summary>
        ///// <param name="points"></param>
        ///// <returns></returns>
        //public static bool IsClockwise(IPoint[] points)
        //{
        //    int numberOfPoints = points.Length;

        //    List<double> values = new List<double>(numberOfPoints);

        //    for (int i = 0; i < numberOfPoints - 1; i++)
        //    {
        //        values.Add((points[i + 1].X - points[i].X) * (points[i + 1].Y + points[i].Y));
        //    }

        //    return values.Sum() > 0;
        //}

        #endregion


        #region Circle-Rectangle Topology

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

        #endregion


        #region Point-Line 

        public static double GetPointToLineSegmentDistance(IPoint lineSegmentStart, IPoint lineSegmentEnd, IPoint targetPoint)
        {
            var dySegment = (lineSegmentEnd.Y - lineSegmentStart.Y);

            var dxSegment = (lineSegmentEnd.X - lineSegmentStart.X);

            //اگر دو نقطه پاره خط منطبق بودند
            //فاصله بین آن‌ها تا نقطه هدف در 
            //نظر گرفته می‌شود.
            if (dxSegment == 0 && dySegment == 0)
            {
                return SpatialUtility.GetEuclideanDistance(lineSegmentStart, targetPoint);
            }

            return Math.Abs(dySegment * targetPoint.X - dxSegment * targetPoint.Y + lineSegmentEnd.X * lineSegmentStart.Y - lineSegmentEnd.Y * lineSegmentStart.X)
                    /
                    Math.Sqrt(dySegment * dySegment + dxSegment * dxSegment);
        }

        public static double GetPointToLineSegmentSquareDistance<T>(T lineSegmentStart, T lineSegmentEnd, T targetPoint) where T : IPoint, new()
        {
            var dySegment = (lineSegmentEnd.Y - lineSegmentStart.Y);

            var dxSegment = (lineSegmentEnd.X - lineSegmentStart.X);

            //اگر دو نقطه پاره خط منطبق بودند
            //فاصله بین آن‌ها تا نقطه هدف در 
            //نظر گرفته می‌شود.
            if (dxSegment == 0 && dySegment == 0)
            {
                return SpatialUtility.GetSquareEuclideanDistance(lineSegmentStart, targetPoint);
            }

            var numerator = (dySegment * targetPoint.X - dxSegment * targetPoint.Y + lineSegmentEnd.X * lineSegmentStart.Y - lineSegmentEnd.Y * lineSegmentStart.X);

            return (numerator * numerator)
                    /
                    (dySegment * dySegment + dxSegment * dxSegment);
        }

        #endregion


        // McMaster, R. B. (1986). A statistical analysis of mathematical measures for linear simplification. The American Cartographer, 13(2), 103-116.
        #region Measurement of Displacement

        // todo: consider ring mode
        public static double CalculateTotalVectorDisplacement<T>(List<T> originalPoints, List<T> simplifiedPoints, bool isRingMode) where T : IPoint, new()
        {
            int currentSimplifiedIndex_Start = 0;
            int currentSimplifiedIndex_End = 1;

            double result = 0;


            // تعیین ارتباط بین اندکس نقطه در لیست 
            // اصلی و اندکس نقطه در لیست ساده شده
            //Dictionary<int, (int, int)?> indexMap = new Dictionary<int, (int, int)?>();

            for (int originalIndex = 0; originalIndex < originalPoints.Count; originalIndex++)
            {
                var currentPoint = originalPoints[originalIndex];

                //if (currentPoint.DistanceTo(simplifiedPoints[currentSimplifiedIndex_Start]) < EpsilonDistance)
                //{
                //    //indexMap.Add(originalIndex, null);
                //}
                /*else */
                if (SpatialUtility.GetEuclideanDistance(currentPoint, simplifiedPoints[currentSimplifiedIndex_End]) < EpsilonDistance)
                {
                    //indexMap.Add(originalIndex, null);
                    currentSimplifiedIndex_Start = currentSimplifiedIndex_End;

                    if (isRingMode && currentSimplifiedIndex_End == simplifiedPoints.Count - 1)
                        currentSimplifiedIndex_End = 0;

                    else
                        currentSimplifiedIndex_End++;

                    continue;
                }
                //else
                //{ 
                var distance = GetPointToLineSegmentDistance(simplifiedPoints[currentSimplifiedIndex_Start], simplifiedPoints[currentSimplifiedIndex_End], currentPoint);

                result += distance;
                //}
            }

            return result;
        }


        #endregion

        //
        public static bool IsPointInPolygon<T>(Geometry<T> ring, T point) where T : IPoint, new()
        {
            if (ring == null || point == null)
            {
                return false;
            }

            var numberOfPoints = ring.Points.Count;

            if (ring.Type != GeometryType.LineString || numberOfPoints < 3)
            {
                throw new NotImplementedException("SpatialUtility.cs > IsPointInPolygon");
            }

            var boundingBox = ring.GetBoundingBox();

            var encomapss = boundingBox.Encomapss(point);

            if (!encomapss)
            {
                return false;
            }

            double totalAngle = 0.0;

            for (int i = 0; i < numberOfPoints - 1; i++)
            {
                var angle = GetAngle(ring.Points[i], point, ring.Points[i + 1]);

                totalAngle += angle;
            }

            totalAngle += GetAngle(ring.Points[numberOfPoints - 1], point, ring.Points[0]);

            if (Math.Abs(totalAngle - 2 * Math.PI) < 0.1)
                return true;

            return false;
        }
    }
}
