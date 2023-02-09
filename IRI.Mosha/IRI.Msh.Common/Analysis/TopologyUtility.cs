using IRI.Msh.Algebra;
using IRI.Msh.Common.Analysis.Topology;
using IRI.Msh.Common.Primitives;
using System;
using System.Collections.Generic;
using System.Text;

namespace IRI.Msh.Common.Analysis
{
    public class TopologyUtility
    {
        public static PointCircleRelation GetPointCircleRelation<T>(T sightlyPoint, T point1, T point2, T point3) where T : IPoint, new()
        {
            // check if point1, point2, point3 are in ccw form
            double tempValue = ((point1.X - point3.X) * (point2.Y - point3.Y) - (point2.X - point3.X) * (point1.Y - point3.Y));

            bool isCounterClockWise = (tempValue > 0);

            double x0, x1, x2, x3, y0, y1, y2, y3;

            if (Math.Abs(tempValue) < double.Epsilon)
            {
                throw new NotImplementedException();
            }
            else if (isCounterClockWise)
            {
                x0 = sightlyPoint.X; y0 = sightlyPoint.Y;

                x1 = point1.X; y1 = point1.Y;

                x2 = point2.X; y2 = point2.Y;

                x3 = point3.X; y3 = point3.Y;
            }
            else
            {
                x0 = sightlyPoint.X; y0 = sightlyPoint.Y;

                x1 = point1.X; y1 = point1.Y;

                x2 = point3.X; y2 = point3.Y;

                x3 = point2.X; y3 = point2.Y;
            }

            Matrix tempMatrix = new Matrix(
                                   new double[][] {
                                        new double[] { x1 - x0, x2 - x0, x3 - x0 },
                                        new double[] { y1 - y0, y2 - y0, y3 - y0 },
                                        new double[] { x1 * x1 - x0 * x0 + y1 * y1 - y0 * y0,
                                                        x2 * x2 - x0 * x0 + y2 * y2 - y0 * y0,
                                                        x3 * x3 - x0 * x0 + y3 * y3 - y0 * y0 } });
            double result = tempMatrix.Determinant;

            return (result > 0 ? PointCircleRelation.In : (result == 0 ? PointCircleRelation.On : PointCircleRelation.Out));
        }

        public static PointVectorRelation GetPointVectorRelation<T>(T sightlyPoint, T startVertex, T endVertex) where T : IPoint, new()
        {
            double tempValue = (startVertex.X - sightlyPoint.X) * (endVertex.Y - sightlyPoint.Y) -
                                (endVertex.X - sightlyPoint.X) * (startVertex.Y - sightlyPoint.Y);

            if (Math.Abs(tempValue) < double.Epsilon)
            {
                return PointVectorRelation.LiesOnTheLine;
            }
            else if (tempValue > 0)
            {
                return PointVectorRelation.LiesLeft;
            }
            else if (tempValue < 0)
            {
                return PointVectorRelation.LiesRight;
            }

            return PointVectorRelation.LiesOnTheLine;
        }

        public static PointLineSegementRelation GetPointLineSegmentRelation<T>(T sightlyPoint, T startVertex, T endVertex) where T : IPoint, new()
        {
            if (GetPointVectorRelation(sightlyPoint, startVertex, endVertex) == PointVectorRelation.LiesOnTheLine)
            {
                if ((sightlyPoint.X > startVertex.X && sightlyPoint.X > endVertex.X) ||
                   (sightlyPoint.Y > startVertex.Y && sightlyPoint.Y > endVertex.Y) ||
                   (sightlyPoint.X < startVertex.X && sightlyPoint.X < endVertex.X) ||
                   (sightlyPoint.Y < startVertex.Y && sightlyPoint.Y < endVertex.Y))
                {
                    return PointLineSegementRelation.Out;
                }
                else
                {
                    return PointLineSegementRelation.On;
                }
            }

            return PointLineSegementRelation.Out;
        }


        public static Point CalculateCircumcenterCenterPoint<T>(T firstPoint, T secondPoint, T thirdPoint) where T : IPoint, new()
        {
            double x1 = firstPoint.X; double y1 = firstPoint.Y;

            double x2 = secondPoint.X; double y2 = secondPoint.Y;

            double x3 = thirdPoint.X; double y3 = thirdPoint.Y;

            double temp = 2 * (x1 * (y2 - y3) + x2 * (y3 - y1) + x3 * (y1 - y2));

            double tempX = ((y1 * y1 + x1 * x1) * (y2 - y3) + (y2 * y2 + x2 * x2) * (y3 - y1) + (y3 * y3 + x3 * x3) * (y1 - y2)) / temp;

            double tempY = ((y1 * y1 + x1 * x1) * (x3 - x2) + (y2 * y2 + x2 * x2) * (x1 - x3) + (y3 * y3 + x3 * x3) * (x2 - x1)) / temp;

            return new Point(tempX, tempY);
        }

        public static LineLineRelation CalculateIntersection<T>(T firstLineFirstPoint,
            T firstLineSecondPoint,
            T secondLineFirstPoint,
            T secondLineSecondPoint,
            out T intersection) where T : IPoint, new()
        {
            if (firstLineFirstPoint.Equals(firstLineSecondPoint) || secondLineFirstPoint.Equals(secondLineSecondPoint))
                throw new NotImplementedException();
            // precision issues!
            double firstSlope = SpatialUtility.CalculateSlope(firstLineFirstPoint, firstLineSecondPoint);

            double secondSlope = SpatialUtility.CalculateSlope(secondLineFirstPoint, secondLineSecondPoint);

            if ((firstSlope - secondSlope) == 0 ||
                    (Math.Abs(firstSlope) == double.PositiveInfinity &&
                    Math.Abs(secondSlope) == double.PositiveInfinity))
            {

                if (GetPointVectorRelation(firstLineFirstPoint, secondLineFirstPoint, secondLineSecondPoint) == PointVectorRelation.LiesOnTheLine)
                {
                    intersection = new T() { X = double.NaN, Y = double.NaN };

                    return LineLineRelation.Coinciding;
                }
                else
                {
                    intersection = new T() { X = double.PositiveInfinity, Y = double.PositiveInfinity };

                    return LineLineRelation.Parallel;
                }
            }

            double tempX, tempY;

            if (Math.Abs(firstSlope) == double.PositiveInfinity)
            {
                tempX = firstLineFirstPoint.X;

                tempY = secondLineFirstPoint.Y + secondSlope * (tempX - firstLineFirstPoint.X);
            }
            else if (Math.Abs(secondSlope) == double.PositiveInfinity)
            {
                tempX = secondLineFirstPoint.X;

                tempY = firstLineFirstPoint.Y + firstSlope * (tempX - firstLineFirstPoint.X);
            }
            else
            {
                tempX = (firstLineFirstPoint.Y -
                            secondLineFirstPoint.Y +
                            secondSlope * secondLineFirstPoint.X -
                            firstSlope * firstLineFirstPoint.X) / (secondSlope - firstSlope);

                tempY = secondLineFirstPoint.Y + secondSlope * (tempX - secondLineFirstPoint.X);
            }

            intersection = new T() { X = tempX, Y = tempY };

            return LineLineRelation.Intersect;
        }

        public static bool CalculateIntersection<T>(T firstLinePoint, double firstLineSlope, T secondLinePoint, double secondLineSlope) where T : IPoint, new()
        {
            throw new NotImplementedException();
        }


        //todo: halate montabeg shodan dar nazar gerefte nashode!
        public static bool DoseIntersectDirectionTheLineSegment<T>(
            T startOfDirection,
            T endOfDirection,
            T startOfLine,
            T endOfLine,
            out T intersection) where T : IPoint, new()
        {

            if (IntersectLineWithLineSegment(startOfDirection, endOfDirection, startOfLine, endOfLine, out intersection) == LineLineSegmentRelation.Intersect)
            {

                if (endOfDirection.X != startOfDirection.X)
                {
                    return (intersection.X - startOfDirection.X) / (endOfDirection.X - startOfDirection.X) > 1;
                }
                else if (endOfDirection.Y != startOfDirection.Y)
                {
                    return (intersection.Y - startOfDirection.Y) / (endOfDirection.Y - startOfDirection.Y) > 1;
                }
                else
                {
                    throw new NotImplementedException();
                }

                //}
            }
            else
            {
                return false;
            }
        }

        public static LineLineSegmentRelation IntersectLineWithLineSegment<T>(
            T lineFirstPoint,
            T lineSecondPoint,
            T startOfLineSegment,
            T endOfLineSegment,
            out T intersection) where T : IPoint, new()
        {
            LineLineRelation relation = CalculateIntersection(lineFirstPoint, lineSecondPoint, startOfLineSegment, endOfLineSegment, out intersection);

            if (relation == LineLineRelation.Intersect)
            {
                if ((intersection.X > startOfLineSegment.X && intersection.X > endOfLineSegment.X) ||
                    (intersection.Y > startOfLineSegment.Y && intersection.Y > endOfLineSegment.Y) ||
                    (intersection.X < startOfLineSegment.X && intersection.X < endOfLineSegment.X) ||
                    (intersection.Y < startOfLineSegment.Y && intersection.Y < endOfLineSegment.Y))
                {
                    return LineLineSegmentRelation.Nothing;
                }
                else
                {
                    return LineLineSegmentRelation.Intersect;
                }
            }
            else if (relation == LineLineRelation.Coinciding)
            {
                return LineLineSegmentRelation.Coinciding;
            }
            else if (relation == LineLineRelation.Parallel)
            {
                return LineLineSegmentRelation.Parallel;
            }
            else
            {
                throw new NotImplementedException();
            }
        }





        //
        public static bool IsPointInRing<T>(Geometry<T> ring, T point) where T : IPoint, new()
        {
            if (ring.IsNullOrEmpty() || point is null)
                return false;

            var numberOfPoints = ring.Points.Count;

            if (ring.Type != GeometryType.LineString || numberOfPoints < 3)
                throw new NotImplementedException("SpatialUtility.cs > IsPointInPolygon");

            var boundingBox = ring.GetBoundingBox();

            var doesEncomapss = boundingBox.Encomapss(point);

            if (!doesEncomapss)
                return false;

            double totalAngle = 0.0;

            for (int i = 0; i < numberOfPoints - 1; i++)
            {
                var angle = SpatialUtility.GetAngle(ring.Points[i], point, ring.Points[i + 1]);

                totalAngle += angle;
            }

            totalAngle += SpatialUtility.GetAngle(ring.Points[numberOfPoints - 1], point, ring.Points[0]);

            if (Math.Abs(totalAngle - 2 * Math.PI) < 0.1)
                return true;

            return false;
        }

        public static bool IsPointOnLineString<T>(Geometry<T> lineString, T point) where T : IPoint, new()
        {
            if (lineString.IsNullOrEmpty() || point is null)
                return false;

            var numberOfPoints = lineString.Points.Count;

            if (lineString.Type != GeometryType.LineString || numberOfPoints < 2)
                throw new NotImplementedException("SpatialUtility.cs > IsPointOnLineString");

            var boundingBox = lineString.GetBoundingBox();

            var doesEncomapss = boundingBox.Encomapss(point);

            if (!doesEncomapss)
                return false;

            for (int i = 0; i < numberOfPoints - 1; i++)
            {
                if (GetPointLineSegmentRelation(point, lineString.Points[i], lineString.Points[i + 1]) == PointLineSegementRelation.On)
                    return true;
            }

            return false;
        }

        public static bool DoesLineStringIntersectsLineSegment<T>(Geometry<T> lineString, T lineSegmentStart, T lineSegmentEnd) where T : IPoint, new()
        {
            if (lineString.IsNullOrEmpty() || lineSegmentStart is null || lineSegmentEnd is null)
                return false;

            var numberOfPoints = lineString.Points.Count;

            if (lineString.Type != GeometryType.LineString || numberOfPoints < 2)
                throw new NotImplementedException("SpatialUtility.cs > IsPointOnLineString");

            var boundingBox = lineString.GetBoundingBox();
            
            if (!boundingBox.Intersects(BoundingBox.Create(lineSegmentStart, lineSegmentEnd)))
                return false;

            for (int i = 0; i < numberOfPoints - 1; i++)
            {
                if (DoseIntersectDirectionTheLineSegment(lineString.Points[i], lineString.Points[i + 1], lineSegmentStart, lineSegmentEnd, out _))
                    return true;
            }

            return false;
        }
    }
}
