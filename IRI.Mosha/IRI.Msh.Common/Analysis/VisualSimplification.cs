using IRI.Msh.Common.Primitives;
using IRI.Msh.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IRI.Msh.Common.Analysis
{
    public class VisualSimplification
    {
        public static List<T> AdditiveSimplifyByArea<T>(List<T> points, double threshold) where T : IPoint
        {
            if (points == null || points.Count == 0)
            {
                return null;
            }
            else if (points.Count == 2)
            {
                return points;
            }

            List<int> filtered = new List<int>();

            filtered.Add(0);

            int firstIndex = 0, secondIndex = 1, thirdIndex = 2;

            double tempArea = 0;

            while (thirdIndex < points.Count)
            {
                var area = SpatialUtility.CalculateSignedTriangleArea(points[firstIndex], points[secondIndex], points[thirdIndex]);

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

            filtered.Add(points.Count - 1);

            var result = filtered.Select(i => points[i]).ToList();

            return result;
        }

        public static List<T> AdditiveSimplifyByAreaPlus<T>(List<T> points, double threshold) where T : IPoint
        {
            if (points == null || points.Count == 0)
            {
                return null;
            }
            else if (points.Count == 2)
            {
                return points;
            }

            List<int> filtered = new List<int>();

            filtered.Add(0);

            int firstIndex = 0, secondIndex = 1, thirdIndex = 2;

            var totalAreaCoef = SpatialUtility.IsClockwise(points) ? 1 : -1;

            double tempArea = 0;

            while (thirdIndex < points.Count)
            {
                var area = SpatialUtility.CalculateSignedTriangleArea(points[firstIndex], points[secondIndex], points[thirdIndex]);

                var areaCheck2 = SpatialUtility.CalculateUnsignedTriangleArea(points[firstIndex], points[(int)((firstIndex + thirdIndex) / 2.0)], points[thirdIndex]);

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

            filtered.Add(points.Count - 1);

            var result = filtered.Select(i => points[i]).ToList();

            return result;
        }

        public static List<T> SimplifyByArea<T>(List<T> points, double threshold) where T : IPoint
        {
            if (points == null || points.Count == 0)
            {
                return null;
            }
            else if (points.Count == 2)
            {
                return points;
            }

            List<Tuple<int, double>> areas = new List<Tuple<int, double>>();

            for (int i = 0; i < points.Count - 2; i++)
            {
                var area = SpatialUtility.CalculateUnsignedTriangleArea(points[i], points[i + 1], points[i + 2]);

                areas.Add(new Tuple<int, double>(i + 1, area));
            }

            var filteredIndexes = areas.Where(i => i.Item2 > threshold).Select(i => i.Item1).ToList();

            filteredIndexes.Insert(0, 0);

            if (!points[0].Equals(points[points.Count - 1]))
            {
                filteredIndexes.Add(points.Count - 1);
            }

            var result = filteredIndexes.Select(i => points[i]).ToList();

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="points"></param>
        /// <param name="threshold">Must be between 0 and 1</param>
        /// <returns></returns>
        public static List<T> SimplifyByAngle<T>(List<T> points, double threshold) where T : IPoint
        {
            if (points == null || points.Count == 0)
            {
                return null;
            }
            else if (points.Count == 2)
            {
                return points;
            }

            List<Tuple<int, double>> angles = new List<Tuple<int, double>>();

            for (int i = 0; i < points.Count - 2; i++)
            {
                var angle = SpatialUtility.CalculateSemiCosineOfAngle(points[i], points[i + 1], points[i + 2]);

                angles.Add(new Tuple<int, double>(i + 1, angle));
            }

            //When i.Item2 ~ 1 it means points are on a line
            var filter1 = angles.Where(i => i.Item2 < threshold).Select(i => i.Item1).ToList();

            filter1.Insert(0, 0);

            filter1.Add(points.Count - 1);

            var result = filter1.Select(i => points[i]).ToList();

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="points"></param>
        /// <param name="threshold">Must be between 0 and 1</param>
        /// <returns></returns>
        public static List<T> AdditiveSimplifyByAngle<T>(List<T> points, double threshold) where T : IPoint
        {
            if (points == null || points.Count == 0)
            {
                return null;
            }
            else if (points.Count == 2)
            {
                return points;
            }

            List<int> filtered = new List<int>();

            filtered.Add(0);

            int firstIndex = 0, secondIndex = 1, thirdIndex = 2;

            while (thirdIndex < points.Count)
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

            filtered.Add(points.Count - 1);

            var result = filtered.Select(i => points[i]).ToList();

            return result;
        }

        /// <summary>
        /// Additive Angle + Area Check
        /// </summary>
        /// <param name="points"></param>
        /// <param name="anglethreshold">Must be between 0 and 1</param>
        /// <returns></returns>
        public static List<T> AdditiveSimplifyByAngleArea<T>(List<T> points, double angleThreshold, double areaThreshold) where T : IPoint
        {
            if (points == null || points.Count == 0)
            {
                return null;
            }
            else if (points.Count == 2)
            {
                return points;
            }

            List<int> filtered = new List<int>();

            filtered.Add(0);

            int firstIndex = 0, secondIndex = 1, thirdIndex = 2;

            double tempArea = 0;

            while (thirdIndex < points.Count)
            {
                var angle = SpatialUtility.CalculateSemiCosineOfAngle(points[firstIndex], points[secondIndex], points[thirdIndex]);

                var area = SpatialUtility.CalculateUnsignedTriangleArea(points[firstIndex], points[secondIndex], points[thirdIndex]);

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

            filtered.Add(points.Count - 1);

            var result = filtered.Select(i => points[i]).ToList();

            return result;
        }

        public static List<T> AdditiveSimplifyByDistance<T>(List<T> points, double threshold) where T : IPoint
        {
            if (points == null || points.Count == 0)
            {
                return null;
            }
            else if (points.Count == 2)
            {
                return points;
            }

            List<int> filtered = new List<int>();

            filtered.Add(0);

            int firstIndex = 0, secondIndex = 1;

            var temp = 0.0;

            while (secondIndex < points.Count)
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

            filtered.Add(points.Count - 1);

            var result = filtered.Select(i => points[i]).ToList();

            return result;
        }

        //ref: https://www.tandfonline.com/doi/abs/10.1179/000870493786962263
        public static List<T> SimplifyByVisvalingham<T>(List<T> pointList, double threshold, bool isRing) where T : IPoint
        {
            var areas = SpatialUtility.GetPrimitiveAreas(pointList, isRing);

            List<T> pList = pointList.ToList();

            List<int> removedPoints = new List<int>();

            while (true)
            {
                var minArea = Statistics.Statistics.GetMin(areas);

                if (minArea < threshold)
                {
                    break;
                }

                //remove min areas
                for (int i = areas.Count - 1; i >= 0; i--)
                {
                    if (areas[i] == minArea)
                    {
                        pList.RemoveAt(i + 1);
                        areas.RemoveAt(i);

                        //update areas
                        if (i > 0)
                        {
                            areas[i - 1] = SpatialUtility.CalculateUnsignedTriangleArea(pList[i - 1], pList[i], pList[i + 1]);
                        }

                        if (i < areas.Count - 1)
                        {
                            areas[i] = SpatialUtility.CalculateUnsignedTriangleArea(pList[i], pList[i + 1], pList[i + 2]);
                        }
                    }
                }

                //recalculate adjacent areas
            }

            return pList.ToList();
        }


        //ref: https://doi.org/10.3138/FM57-6770-U75U-7727
        public static List<T> SimplifyByDouglasPeucker<T>(List<T> pointList, double threshold, bool isRing) where T : IPoint
        {
            var result = new List<T>();

            if (isRing)
            {
                return result;
            }

            if (pointList == null || pointList.Count < 2)
            {
                return result;
            }

            if (pointList.Count == 2)
            {
                return pointList;
            }

            var numberOfPoints = pointList.Count;

            //
            double maxSemiPerpendicularDistance = 0;
            int maxIndex = 0;

            //1399.06.23
            //در این جا برای سرعت بیش‌تر مقدار فاصله استفاه 
            //نمی‌شود بلکه توان دوم آن استفاده می‌شود. این 
            //روش باعث می‌شود در محاسبات از تابع جزر استفاده
            //نشود
            double effectiveThreshold = threshold * threshold;

            for (int i = 1; i < numberOfPoints - 1; i++)
            {
                var semiPerpendicularDistance = SpatialUtility.SemiPointToLineSegmentDistance(pointList[0], pointList[numberOfPoints - 1], pointList[i]);

                if (semiPerpendicularDistance > maxSemiPerpendicularDistance)
                {
                    maxIndex = i;
                    maxSemiPerpendicularDistance = semiPerpendicularDistance;
                }
            }

            if (maxSemiPerpendicularDistance > effectiveThreshold)
            {
                var leftList = pointList.Take(maxIndex + 1).ToList();

                result = SimplifyByDouglasPeucker(leftList, threshold, isRing);

                var rightList = pointList.Skip(maxIndex).ToList();

                var rightResult = SimplifyByDouglasPeucker(rightList, threshold, isRing);

                result.AddRange(rightResult.Skip(1));

                return result;
            }
            else
            {
                return new List<T> { pointList[0], pointList[numberOfPoints - 1] };
            }
        }

    }
}
