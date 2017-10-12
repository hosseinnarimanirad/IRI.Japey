using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hnr.Spatial.Version04.Primitives;

namespace Hnr.Spatial.Version04
{
    //public delegate Direction BasicTransform(Direction direction);

    //public delegate BasicTransform Transform(BasicTransform firstTransform, BasicTransform secondTransform);



    public static class SpaceFillingCurves
    {
        private static List<Point2D> DoBasicMove(Point2D current, BasicPath path)
        {
            List<Point2D> result = new List<Point2D>();

            Point2D temp = current;

            result.Add(temp);

            for (int i = 0; i < path.NumberOfSteps - 1; i++)
            {
                temp = path[i](temp, 1);

                result.Add(temp);
            }

            return result;
        }

        public static List<Point2D> Construct(Point2D startPoint, int height, int width, BasicPath path, Jump jumpFunc)
        {
            List<Point2D> result = new List<Point2D>();

            if (height * width == path.NumberOfSteps)
            {
                return DoBasicMove(startPoint, path);
            }

            int newHeight = height / path.Height;

            int newWidth = width / path.Width;

            Point2D tempPoint = startPoint;

            for (int i = 0; i < path.NumberOfSteps; i++)
            {
                result.AddRange(Construct(tempPoint, newHeight, newWidth, path.DoTransform(i), jumpFunc));

                tempPoint = jumpFunc(result[result.Count - 1], i, height, width, path);
            }

            return result;
        }
    }

    public class SpaceFillingCurve
    {
        private BasicPath basicPath;

        //private Jump jump;

        List<Point2D> orderedPoints;

        public int NumberOfColumns { get { return this.basicPath.Width; } }

        public int NumberOfRows { get { return this.basicPath.Height; } }

        public SpaceFillingCurve(BasicPath basicPath)
        {
            this.basicPath = basicPath;
            //string s;
            //s.
            //this.jump = jump;

            this.orderedPoints = new List<Point2D>();

            Point2D tempPoint = GetStartPoint(basicPath);

            orderedPoints.Add(tempPoint);

            for (int i = 0; i < this.basicPath.NumberOfSteps - 1; i++)
            {
                tempPoint = basicPath[i](tempPoint, 1);

                orderedPoints.Add(tempPoint);
            }

        }

        //public static List<Point2D> GetOrderedPoints

        public int ComparePoints(Point2D first, Point2D second)
        {
            int firstIndex = this.orderedPoints.IndexOf(first);

            int secondIndex = this.orderedPoints.IndexOf(second);

            if (firstIndex < 0 || secondIndex < 0)
            {
                throw new NotImplementedException();
            }

            return firstIndex.CompareTo(secondIndex);
        }

        public int GetSubRegionIndex(Point2D point)
        {
            return this.orderedPoints.IndexOf(point);
        }

        public Point2D StartPoint
        {
            get { return this.orderedPoints[0]; }
        }

        public BasicPath GetSubRegionBasicPath(int subregionIndex)
        {
            return basicPath.DoTransform(subregionIndex);
        }

        public static Point2D GetStartPoint(BasicPath path)
        {
            Point2D point = new Point2D(0, 0);

            int deltaX = 0, deltaY = 0;

            for (int i = 0; i < path.NumberOfSteps - 1; i++)
            {
                point = path[i](point, 1);

                deltaX = Math.Min(point.X, deltaX);

                deltaY = Math.Min(point.Y, deltaY);
            }

            return new Point2D(-deltaX, -deltaY);
        }
    }
}
