// besmellahe rahmane rahim
// Allahomma ajjel le-valiyek al-faraj

using System;
using System.Collections.Generic;
using System.Text;
using IRI.Ket.DataStructure;
using IRI.Sta.Algebra;

namespace IRI.Ket.Geometry
{
    public static class ComputationalGeometry
    {
        public static PointCircleRelation GetPointCircleRelation(Point sightlyPoint, Point point1, Point point2, Point point3)
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

        public static PointVectorRelation GetPointVectorRelation(Point sightlyPoint, Point startVertex, Point endVertex)
        {
            double tempValue = ((startVertex.X - sightlyPoint.X) * (endVertex.Y - sightlyPoint.Y) -
                                (endVertex.X - sightlyPoint.X) * (startVertex.Y - sightlyPoint.Y));

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

        public static PointLineSegementRelation GetPointLineSegmentRelation(Point sightlyPoint, Point startVertex, Point endVertex)
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

        public static PoinTriangleRelation GetPointTriangleRelation(Point sightlyPoint, Point firstVertex, Point secondVertex, Point thirdVertex)
        {
            int firstRelation = (int)GetPointVectorRelation(sightlyPoint, firstVertex, secondVertex);

            int secondRelation = (int)GetPointVectorRelation(sightlyPoint, secondVertex, thirdVertex);

            int thirdRelation = (int)GetPointVectorRelation(sightlyPoint, thirdVertex, firstVertex);

            return (PoinTriangleRelation)(firstRelation * QuasiTriangle.firstEdgeWeight +
                                            secondRelation * QuasiTriangle.secondEdgeWeight +
                                            thirdRelation * QuasiTriangle.thirdEdgeWeight);
        }

        public static PointCollection CreateConvexHull(PointCollection points)
        {
            int leftBoundIndex = points.LowerBoundIndex;

            IRI.Ket.Geometry.Point initialPoint = points[leftBoundIndex];

            int length = points.Count;

            IndexValue<double>[] unsortedPoints = new IndexValue<double>[length - 1];

            int counter = 0;

            for (int i = 0; i < length; i++)
            {
                if (i == leftBoundIndex)
                    continue;

                unsortedPoints[counter] = new IndexValue<double>(i,
                                                                Math.Atan2(points[i].Y - initialPoint.Y,
                                                                points[i].X - initialPoint.X));
                counter++;
            }

            IndexValue<double>[] sortedPoints = SortAlgorithm.Heapsort<IndexValue<double>>(unsortedPoints, SortDirection.Descending);

            IRI.Ket.Geometry.PointCollection result = new IRI.Ket.Geometry.PointCollection();

            result.Add(points[leftBoundIndex]);

            counter = 0;

            while (counter < sortedPoints.Length)
            {
                IRI.Ket.Geometry.Point tempPoint = points[sortedPoints[counter].Index];

                if (result.Count < 2)
                {
                    result.Add(tempPoint);

                    counter++;

                    continue;
                }

                PointVectorRelation pointSituation = GetPointVectorRelation(tempPoint, result[result.Count - 2], result[result.Count - 1]);

                if (pointSituation == PointVectorRelation.LiesLeft)
                {
                    result.Add(tempPoint);

                    counter++;
                }
                else if (pointSituation == PointVectorRelation.LiesRight)
                {
                    result.RemoveAt(result.Count - 1);
                }
                else
                {
                    if (sortedPoints[counter].Value == sortedPoints[0].Value)
                    {
                        if (CalculateDistance(initialPoint, tempPoint) > CalculateDistance(initialPoint, result[result.Count - 1]))
                        {
                            result.Add(tempPoint);
                        }

                        counter++;
                    }
                    else if (sortedPoints[counter].Value == sortedPoints[length - 2].Value)
                    {
                        if (CalculateDistance(initialPoint, tempPoint) < CalculateDistance(initialPoint, result[result.Count - 1]))
                        {
                            result.Add(tempPoint);
                        }

                        counter++;
                    }
                    else
                    {
                        result.RemoveAt(result.Count - 1);
                    }
                }
            }

            return result;
        }

        public static List<int> GetConvexHullVertexes(PointCollection points)
        {
            int leftBoundIndex = points.LowerBoundIndex;

            IRI.Ket.Geometry.Point initialPoint = points[leftBoundIndex];

            int length = points.Count;

            IndexValue<double>[] list = new IndexValue<double>[length - 1];

            int counter = 0;

            for (int i = 0; i < length; i++)
            {
                if (i == leftBoundIndex)
                    continue;

                list[counter] = new IndexValue<double>(i,
                                                                Math.Atan2(points[i].Y - initialPoint.Y,
                                                                points[i].X - initialPoint.X));
                counter++;
            }

            list = SortAlgorithm.Heapsort<IndexValue<double>>(list, SortDirection.Descending);

            List<int> result = new List<int>();

            result.Add(leftBoundIndex);

            counter = 0;

            while (counter < list.Length)
            {
                IRI.Ket.Geometry.Point tempPoint = points[list[counter].Index];

                if (result.Count < 2)
                {
                    result.Add(list[counter].Index);

                    counter++;

                    continue;
                }

                PointVectorRelation pointSituation = GetPointVectorRelation(tempPoint,
                                                                                        points[result[result.Count - 2]],
                                                                                        points[result[result.Count - 1]]);

                if (pointSituation == PointVectorRelation.LiesLeft)
                {
                    result.Add(list[counter].Index);

                    counter++;
                }
                else if (pointSituation == PointVectorRelation.LiesRight)
                {
                    result.RemoveAt(result.Count - 1);
                }
                else
                {
                    if (list[counter].Value == list[0].Value &&
                        CalculateDistance(initialPoint, tempPoint) > CalculateDistance(initialPoint, points[result[result.Count - 1]]))
                    {
                        result.Add(list[counter].Index);

                        counter++;
                    }
                    else if (list[counter].Value == list[length - 2].Value &&
                        CalculateDistance(initialPoint, tempPoint) < CalculateDistance(initialPoint, points[result[result.Count - 1]]))
                    {
                        result.Add(list[counter].Index);

                        counter++;
                    }
                    else
                    {
                        result.RemoveAt(result.Count - 1);
                    }
                }
            }

            return result;
        }

        public static double CalculateDistance(Point firstPoint, Point secondPoint)
        {
            double dx = firstPoint.X - secondPoint.X;

            double dy = firstPoint.Y - secondPoint.Y;

            return Math.Sqrt(dx * dx + dy * dy);
        }

        public static Point CalculateCircumcenterCenterPoint(Point firstPoint, Point secondPoint, Point thirdPoint)
        {
            double x1 = firstPoint.X; double y1 = firstPoint.Y;

            double x2 = secondPoint.X; double y2 = secondPoint.Y;

            double x3 = thirdPoint.X; double y3 = thirdPoint.Y;

            double temp = 2 * (x1 * (y2 - y3) + x2 * (y3 - y1) + x3 * (y1 - y2));

            double tempX = ((y1 * y1 + x1 * x1) * (y2 - y3) + (y2 * y2 + x2 * x2) * (y3 - y1) + (y3 * y3 + x3 * x3) * (y1 - y2)) / temp;

            double tempY = ((y1 * y1 + x1 * x1) * (x3 - x2) + (y2 * y2 + x2 * x2) * (x1 - x3) + (y3 * y3 + x3 * x3) * (x2 - x1)) / temp;

            return new Point(tempX, tempY);
        }

        public static LineLineRelation CalculateIntersection(Point firstLineFirstPoint,
                                                    Point firstLineSecondPoint,
                                                    Point secondLineFirstPoint,
                                                    Point secondLineSecondPoint,
                                                    out Point intersection)
        {
            if (firstLineFirstPoint.Equals(firstLineSecondPoint) || secondLineFirstPoint.Equals(secondLineSecondPoint))
            {
                throw new NotImplementedException();
            }
            // precision issues!
            double firstSlope = CalculateSlope(firstLineFirstPoint, firstLineSecondPoint);

            double secondSlope = CalculateSlope(secondLineFirstPoint, secondLineSecondPoint);

            if ((firstSlope - secondSlope) == 0 ||
                    (Math.Abs(firstSlope) == double.PositiveInfinity &&
                    Math.Abs(secondSlope) == double.PositiveInfinity))
            {

                if (GetPointVectorRelation(firstLineFirstPoint, secondLineFirstPoint, secondLineSecondPoint) == PointVectorRelation.LiesOnTheLine)
                {
                    intersection = new Point(double.NaN, double.NaN);

                    return LineLineRelation.Coinciding;
                }
                else
                {
                    intersection = new Point(double.PositiveInfinity, double.PositiveInfinity);

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

            intersection = new Point(tempX, tempY);

            return LineLineRelation.Intersect;
        }

        public static bool CalculateIntersection(Point firstLinePoint, double firstLineSlope, Point secondLinePoint, double secondLineSlope)
        {
            throw new NotImplementedException();
        }


        //### halate montabeg shodan dar nazar gerefte nashode!
        public static bool DoseIntersectDirectionTheLineSegment(Point startOfDirection, Point endOfDirection, Point startOfLine, Point endOfLine, out Point intersection)
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

        public static LineLineSegmentRelation IntersectLineWithLineSegment(Point lineFirstPoint, Point lineSecondPoint, Point startOfLineSegment, Point endOfLineSegment, out Point intersection)
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


        public static Point CalculateMidPoint(Point firstPoint, Point secondPoint)
        {
            return new Point((firstPoint.X + secondPoint.X) / 2, (firstPoint.Y + secondPoint.Y) / 2);
        }

        public static double CalculateSlope(Point firstPoint, Point secondPoint)
        {
            return (secondPoint.Y - firstPoint.Y) / (secondPoint.X - firstPoint.X);
        }
    }
}
