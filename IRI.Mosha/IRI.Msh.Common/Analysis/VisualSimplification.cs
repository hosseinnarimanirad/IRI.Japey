using IRI.Msh.Common.Primitives;
using IRI.Msh.Common.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IRI.Msh.Common.Analysis
{
    public class VisualSimplification
    {
        public static IPoint[] AdditiveSimplifyByArea(IPoint[] points, double threshold)
        {
            if (points == null || points.Length == 0)
            {
                return null;
            }
            else if (points.Length == 2)
            {
                return points;
            }

            List<int> filtered = new List<int>();

            filtered.Add(0);

            int firstIndex = 0, secondIndex = 1, thirdIndex = 2;

            double tempArea = 0;

            while (thirdIndex < points.Length)
            {
                var area = SpatialUtility.CalculateTriangleSignedArea(points[firstIndex], points[secondIndex], points[thirdIndex]);

                tempArea += area;

                if (Math.Abs(tempArea) > threshold)
                {
                    tempArea = 0;

                    filtered.Add(secondIndex);

                    firstIndex = secondIndex;
                }

                secondIndex = thirdIndex;

                thirdIndex = thirdIndex + 1;
            }

            if (filtered.Count == 1)
            {
                filtered.Add(points.Count() / 2);
            }

            filtered.Add(points.Length - 1);

            var output1 = filtered.Select(i => points[i]).ToArray();

            return output1;
        }

        public static IPoint[] AdditiveSimplifyByAreaPlus(IPoint[] points, double threshold)
        {
            if (points == null || points.Length == 0)
            {
                return null;
            }
            else if (points.Length == 2)
            {
                return points;
            }

            List<int> filtered = new List<int>();

            filtered.Add(0);

            int firstIndex = 0, secondIndex = 1, thirdIndex = 2;

            var totalAreaCoef = SpatialUtility.IsClockwise(points) ? 1 : -1;

            double tempArea = 0;

            while (thirdIndex < points.Length)
            {
                var area = SpatialUtility.CalculateTriangleSignedArea(points[firstIndex], points[secondIndex], points[thirdIndex]);

                var areaCheck2 = SpatialUtility.CalculateTriangleArea(points[firstIndex], points[(int)((firstIndex + thirdIndex) / 2.0)], points[thirdIndex]);

                tempArea += area;

                if (Math.Abs(tempArea) > threshold || areaCheck2 > threshold || (area * totalAreaCoef > 0))
                {
                    tempArea = 0;

                    filtered.Add(secondIndex);

                    firstIndex = secondIndex;
                }

                secondIndex = thirdIndex;

                thirdIndex = thirdIndex + 1;
            }

            if (filtered.Count == 1)
            {
                filtered.Add(points.Count() / 2);
            }

            filtered.Add(points.Length - 1);

            var output1 = filtered.Select(i => points[i]).ToArray();
            //if (!output1[0].Equals(output1[output1.Length - 1]))
            //{

            //}
            return output1;
        }

        public static IPoint[] SimplifyByArea(IPoint[] points, double threshold)
        {
            if (points == null || points.Length == 0)
            {
                return null;
            }
            else if (points.Length == 2)
            {
                return points;
            }

            List<Tuple<int, double>> areas = new List<Tuple<int, double>>();

            for (int i = 0; i < points.Length - 2; i++)
            {
                var area = SpatialUtility.CalculateTriangleArea(points[i], points[i + 1], points[i + 2]);

                areas.Add(new Tuple<int, double>(i + 1, area));
            }

            var filter1 = areas.Where(i => i.Item2 > threshold).Select(i => i.Item1).ToList();

            filter1.Insert(0, 0);

            filter1.Add(points.Length - 1);

            var output1 = filter1.Select(i => points[i]).ToArray();

            return output1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="points"></param>
        /// <param name="threshold">Must be between 0 and 1</param>
        /// <returns></returns>
        public static IPoint[] SimplifyByAngle(IPoint[] points, double threshold)
        {
            if (points == null || points.Length == 0)
            {
                return null;
            }
            else if (points.Length == 2)
            {
                return points;
            }

            List<Tuple<int, double>> angles = new List<Tuple<int, double>>();

            for (int i = 0; i < points.Length - 2; i++)
            {
                var angle = SpatialUtility.CalculateSemiCosineOfAngle(points[i], points[i + 1], points[i + 2]);

                angles.Add(new Tuple<int, double>(i + 1, angle));
            }

            //When i.Item2 ~ 1 it means points are on a line
            var filter1 = angles.Where(i => i.Item2 < threshold).Select(i => i.Item1).ToList();

            filter1.Insert(0, 0);

            filter1.Add(points.Length - 1);

            var output1 = filter1.Select(i => points[i]).ToArray();

            return output1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="points"></param>
        /// <param name="threshold">Must be between 0 and 1</param>
        /// <returns></returns>
        public static IPoint[] AdditiveSimplifyByAngle(IPoint[] points, double threshold)
        {
            if (points == null || points.Length == 0)
            {
                return null;
            }
            else if (points.Length == 2)
            {
                return points;
            }

            List<int> filtered = new List<int>();

            filtered.Add(0);

            int firstIndex = 0, secondIndex = 1, thirdIndex = 2;

            while (thirdIndex < points.Length)
            {
                var angle = SpatialUtility.CalculateSemiCosineOfAngle(points[firstIndex], points[secondIndex], points[thirdIndex]);

                if (angle < 0 || angle < threshold)
                {
                    filtered.Add(secondIndex);

                    firstIndex = secondIndex;
                }

                secondIndex = thirdIndex;

                thirdIndex = thirdIndex + 1;
            }

            if (filtered.Count == 1)
            {
                filtered.Add(points.Count() / 2);
            }

            filtered.Add(points.Length - 1);

            var output1 = filtered.Select(i => points[i]).ToArray();

            return output1;
        }

        /// <summary>
        /// Additive Angle + Area Check
        /// </summary>
        /// <param name="points"></param>
        /// <param name="anglethreshold">Must be between 0 and 1</param>
        /// <returns></returns>
        public static IPoint[] AdditiveSimplifyByAngleArea(IPoint[] points, double angleThreshold, double areaThreshold)
        {
            if (points == null || points.Length == 0)
            {
                return null;
            }
            else if (points.Length == 2)
            {
                return points;
            }

            List<int> filtered = new List<int>();

            filtered.Add(0);

            int firstIndex = 0, secondIndex = 1, thirdIndex = 2;

            double tempArea = 0;

            while (thirdIndex < points.Length)
            {
                var angle = SpatialUtility.CalculateSemiCosineOfAngle(points[firstIndex], points[secondIndex], points[thirdIndex]);

                var area = SpatialUtility.CalculateTriangleArea(points[firstIndex], points[secondIndex], points[thirdIndex]);

                tempArea += area;


                //if (Math.Abs(angle) < threshold)
                //{
                if ((angle < 0 || angle < angleThreshold) || tempArea > areaThreshold)
                {
                    filtered.Add(secondIndex);

                    firstIndex = secondIndex;

                    tempArea = 0;
                }


                secondIndex = thirdIndex;

                thirdIndex = thirdIndex + 1;
            }

            if (filtered.Count == 1)
            {
                filtered.Add(points.Count() / 2);
            }

            filtered.Add(points.Length - 1);

            var output1 = filtered.Select(i => points[i]).ToArray();

            return output1;
        }

        public static IPoint[] AdditiveSimplifyByDistance(IPoint[] points, double threshold)
        {
            if (points == null || points.Length == 0)
            {
                return null;
            }
            else if (points.Length == 2)
            {
                return points;
            }

            List<int> filtered = new List<int>();

            filtered.Add(0);

            int firstIndex = 0, secondIndex = 1;

            var temp = 0.0;

            while (secondIndex < points.Length)
            {
                var semiDistance = SpatialUtility.GetSemiDistance(points[firstIndex], points[secondIndex]);

                temp += semiDistance;

                if (temp > threshold)
                {
                    temp = 0;

                    filtered.Add(secondIndex);

                    firstIndex = secondIndex;
                }

                secondIndex++;
            }

            if (filtered.Count == 1)
            {
                filtered.Add(points.Count() / 2);
            }

            filtered.Add(points.Length - 1);

            var output1 = filtered.Select(i => points[i]).ToArray();

            return output1;
        }
    }
}
