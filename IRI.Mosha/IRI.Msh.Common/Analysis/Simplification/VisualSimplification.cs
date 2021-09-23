using IRI.Msh.Common.Model.Here;
using IRI.Msh.Common.Primitives;
using IRI.Msh.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace IRI.Msh.Common.Analysis
{
    public static class VisualSimplification
    {
        public static List<T> AdditiveSimplifyByAreaPlus<T>(List<T> points, SimplificationParamters paramters/*double threshold, bool retain3Points = false*/) where T : IPoint
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
                var area = SpatialUtility.GetSignedTriangleArea(points[firstIndex], points[secondIndex], points[thirdIndex]);

                var areaCheck2 = SpatialUtility.GetUnsignedTriangleArea(points[firstIndex], points[(int)((firstIndex + thirdIndex) / 2.0)], points[thirdIndex]);

                tempArea += area;

                if (Math.Abs(tempArea) > paramters.AreaThreshold || areaCheck2 > paramters.AreaThreshold || (area * totalAreaCoef > 0))
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="points"></param>
        /// <param name="threshold">Must be between 0 and 1</param>
        /// <returns></returns>
        public static List<T> SimplifyByAngle<T>(List<T> points, SimplificationParamters paramters /*double threshold, bool retain3Points = false*/) where T : IPoint
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
                var angle = SpatialUtility.GetSquareCosineOfAngle(points[i], points[i + 1], points[i + 2]);

                angles.Add(new Tuple<int, double>(i + 1, angle));
            }

            //When i.Item2 ~ 1 it means points are on a line
            var filter1 = angles.Where(i => i.Item2 < paramters.AngleThreshold).Select(i => i.Item1).ToList();

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
        public static List<T> AdditiveSimplifyByAngle<T>(List<T> points, SimplificationParamters paramters/*double threshold, bool retain3Points = false*/) where T : IPoint
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
                var angle = SpatialUtility.GetSquareCosineOfAngle(points[firstIndex], points[secondIndex], points[thirdIndex]);

                if (angle < 0 || angle < paramters.AngleThreshold)
                {
                    filtered.Add(secondIndex);

                    firstIndex = secondIndex;
                }

                secondIndex = thirdIndex;

                thirdIndex = thirdIndex + 1;
            }

            if (paramters.Retain3Points && filtered.Count == 1)
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
        public static List<T> AdditiveSimplifyByAngleArea<T>(List<T> points, SimplificationParamters paramters /*double angleThreshold, double areaThreshold, bool retain3Points = false*/) where T : IPoint
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
                var angle = SpatialUtility.GetSquareCosineOfAngle(points[firstIndex], points[secondIndex], points[thirdIndex]);

                var area = SpatialUtility.GetUnsignedTriangleArea(points[firstIndex], points[secondIndex], points[thirdIndex]);

                tempArea += area;


                //if (Math.Abs(angle) < threshold)
                //{
                if ((angle < 0 || angle < paramters.AngleThreshold.Value) || tempArea > paramters.AreaThreshold.Value)
                {
                    filtered.Add(secondIndex);

                    firstIndex = secondIndex;

                    tempArea = 0;
                }


                secondIndex = thirdIndex;

                thirdIndex = thirdIndex + 1;
            }

            if (paramters.Retain3Points && filtered.Count == 1)
            {
                filtered.Add(points.Count() / 2);
            }

            filtered.Add(points.Count - 1);

            var result = filtered.Select(i => points[i]).ToList();

            return result;
        }

        public static List<T> AdditiveSimplifyByDistance<T>(List<T> points, SimplificationParamters parameters /*double threshold, bool retain3Points = false*/) where T : IPoint
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
                var semiDistance = SpatialUtility.GetSquareDistance(points[firstIndex], points[secondIndex]);

                temp += semiDistance;

                if (temp > parameters.DistanceThreshold.Value)
                {
                    temp = 0;

                    filtered.Add(secondIndex);

                    firstIndex = secondIndex;
                }

                secondIndex++;
            }

            if (parameters.Retain3Points && filtered.Count == 1)
            {
                filtered.Add(points.Count() / 2);
            }

            filtered.Add(points.Count - 1);

            var result = filtered.Select(i => points[i]).ToList();

            return result;
        }

        public static List<T> SimplifyByArea<T>(List<T> points, SimplificationParamters parameters/*, double threshold, bool retain3Points = false*/) where T : IPoint
        {
            if (points == null || points.Count == 0)
            {
                return null;
            }
            else if (points.Count == 2)
            {
                return points;
            }

            List<T> result = new List<T>();

            result.Add(points.First());

            for (int i = 0; i < points.Count - 2; i++)
            {
                var area = SpatialUtility.GetUnsignedTriangleArea(points[i], points[i + 1], points[i + 2]);

                if (area > parameters.AreaThreshold/* threshold*/)
                {
                    result.Add(points[i + 1]);
                }
            }

            if (parameters.Retain3Points && result.Count == 1)
            {
                result.Add(points[points.Count() / 2]);
            }

            result.Add(points.Last());

            return result;
        }

        public static List<T> AdditiveSimplifyByArea<T>(List<T> points, SimplificationParamters parameters/*, double threshold, bool retain3Points = false*/) where T : IPoint
        {
            if (points == null || points.Count == 0)
            {
                return null;
            }
            else if (points.Count == 2)
            {
                return points;
            }

            List<T> result = new List<T>();

            result.Add(points.First());

            int firstIndex = 0, secondIndex = 1, thirdIndex = 2;

            double tempArea = 0;

            while (thirdIndex < points.Count)
            {
                //1399.07.10
                //var area = SpatialUtility.CalculateSignedTriangleArea(points[firstIndex], points[secondIndex], points[thirdIndex]);
                //tempArea += area;
                //if (Math.Abs(tempArea) > threshold)

                var area = SpatialUtility.GetUnsignedTriangleArea(points[firstIndex], points[secondIndex], points[thirdIndex]);

                tempArea += area;

                if (tempArea > parameters.AreaThreshold /*threshold*/)
                {
                    tempArea = 0;

                    result.Add(points[secondIndex]);

                    firstIndex = secondIndex;
                }

                secondIndex = thirdIndex;

                thirdIndex = thirdIndex + 1;
            }

            if (parameters.Retain3Points && result.Count == 1)
            {
                result.Add(points[points.Count() / 2]);
            }

            result.Add(points.Last());

            //var result = filtered.Select(i => points[i]).ToList();

            return result;
        }

        // ref: https://www.tandfonline.com/doi/abs/10.1179/000870493786962263
        public static List<T> SimplifyByVisvalingam<T>(List<T> pointList, SimplificationParamters parameters, bool isRing/*, double threshold, bool isRing, bool retain3Points = false*/) where T : IPoint
        {
            if (pointList == null || pointList.Count <= 3)
            {
                return pointList;
            }

            var areas = SpatialUtility.GetPrimitiveAreas(pointList, isRing);

            List<T> pList = pointList.ToList();

            //var areas2 = SpatialUtility.GetPrimitiveAreas(pointList, isRing);
            //List<T> pList2 = pointList.ToList();

            while (areas.Count > 0)
            {
                //if (!areas.SequenceEqual(areas2))
                //{

                //}

                //if (!pList.SequenceEqual(pList2))
                //{

                //}

                var minArea = Statistics.Statistics.GetMin(areas);

                if (minArea > parameters.AreaThreshold)
                    break;

                var pointCount = pList.Count;
                var areaCount = areas.Count;

                //double a1=0, a2=0;

                //remove min areas
                for (int i = areas.Count - 1; i >= 0; i--)
                {
                    if (areas[i] == minArea)
                    {
                        if (isRing || i > 0)
                        {
                            // (i-1, i, i+2): i+2 is used insted of i+1 because i+1 is going to be removed
                            var firstIndex = i - 1 < 0 ? pointCount - 1 : i - 1;

                            areas[firstIndex] = SpatialUtility.GetUnsignedTriangleArea(pList[firstIndex], pList[i], pList[(i + 2) % pointCount]);

                            //a1 = areas[firstIndex];
                        }

                        if (isRing || i < areas.Count - 1)
                        {
                            // (i, i+2, i+3): i is used insted of i+1 because i+1 is going to be removed
                            areas[(i + 1) % areaCount] = SpatialUtility.GetUnsignedTriangleArea(pList[i], pList[(i + 2) % pointCount], pList[(i + 3) % pointCount]);

                            //a2 = areas[(i + 1) % areaCount];
                        }

                        ////recalculate adjacent areas
                        //if (i > 0)
                        //{
                        //    areas[i - 1] = SpatialUtility.GetUnsignedTriangleArea(pList[i - 1], pList[i], pList[i + 2]);
                        //}
                        //if (i < areas.Count - 1)
                        //{
                        //    areas[i] = SpatialUtility.GetUnsignedTriangleArea(pList[i], pList[i + 1], pList[i + 2]);
                        //}

                        pList.RemoveAt((i + 1) % pointCount);
                        areas.RemoveAt(i);

                        break;
                    }

                }

                //for (int i = areas2.Count - 1; i >= 0; i--)
                //{
                //    if (areas2[i] == minArea)
                //    {
                //        pList2.RemoveAt((i + 1));
                //        areas2.RemoveAt(i);

                //        ////recalculate adjacent areas
                //        if (i > 0)
                //        {
                //            areas2[i - 1] = SpatialUtility.GetUnsignedTriangleArea(pList2[i - 1], pList2[i], pList2[i + 1]);

                //            if (areas2[i - 1] != a1)
                //            {

                //            }
                //        }
                //        if (i < areas2.Count - 1)
                //        {
                //            areas2[i] = SpatialUtility.GetUnsignedTriangleArea(pList2[i], pList2[i + 1], pList2[i + 2]);

                //            if (areas2[i] != a2)
                //            {

                //            }
                //        }

                //        break;
                //    }

                //}
            }

            if (parameters.Retain3Points && pList.Count == 2)
            {
                pList.Insert(1, pointList[pointList.Count() / 2]);
            }

            return pList.ToList();
        }


        // ref: https://doi.org/10.3138/FM57-6770-U75U-7727
        public static List<T> SimplifyByDouglasPeucker<T>(List<T> pointList, SimplificationParamters parameters/*, double threshold, bool retain3Points = false*/) where T : IPoint
        {
            var result = new List<T>();

            if (pointList == null || pointList.Count < 2)
            {
                return result;
            }

            if (pointList.Count == 2)
            {
                return pointList;
            }

            //to handle lines with the same start and end
            if (pointList.First().Equals(pointList.Last()))
            {
                //return DivideForDouglasPeucker(pointList, threshold, pointList.Count / 2);
                return DivideForDouglasPeucker(pointList, parameters, pointList.Count / 2);
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
            //double effectiveThreshold = threshold * threshold;
            double effectiveThreshold = parameters.DistanceThreshold.Value /** parameters.DistanceThreshold.Value*/;

            for (int i = 1; i < numberOfPoints - 1; i++)
            {
                var semiPerpendicularDistance = SpatialUtility.GetPointToLineSegmentDistance(pointList[0], pointList[numberOfPoints - 1], pointList[i]);

                if (semiPerpendicularDistance > maxSemiPerpendicularDistance)
                {
                    maxIndex = i;
                    maxSemiPerpendicularDistance = semiPerpendicularDistance;
                }
            }

            if (maxSemiPerpendicularDistance > effectiveThreshold)
            {
                return DivideForDouglasPeucker(pointList, parameters, maxIndex);
            }
            else
            {
                return new List<T> { pointList[0], pointList[numberOfPoints - 1] };
            }
        }


        // ref: Lang, T., 1969, Rules for robot draughtsmen. Geographical Magazine, vol.62, No.1, pp.50-51
        // link: 
        // AreaThreshold is used
        public static List<T> SimplifyByLang<T>(List<T> pointList, SimplificationParamters parameters/*, double threshold, int? lookAhead, bool retain3Points = false*/) where T : IPoint
        {
            var result = new List<T>();

            if (pointList == null || pointList.Count < 2)
            {
                return result;
            }

            if (pointList.Count == 2)
            {
                return pointList;
            }

            if (parameters.LookAhead == null)
            {
                parameters.LookAhead = Math.Max(3, pointList.Count / 10);
            }

            var numberOfPoints = pointList.Count;

            //

            //1399.07.09
            //در این جا برای سرعت بیش‌تر مقدار فاصله استفاه 
            //نمی‌شود بلکه توان دوم آن استفاده می‌شود. این 
            //روش باعث می‌شود در محاسبات از تابع جذر استفاده
            //نشود
            //double effectiveThreshold = threshold * threshold;
            double effectiveThreshold = parameters.AreaThreshold.Value /** parameters.AreaThreshold.Value*/;

            int startIndex = 0;

            result.Add(pointList[0]);

            int endIndex = Math.Min(numberOfPoints - 1, parameters.LookAhead.Value);

            while (true)
            {
                if (startIndex == endIndex)
                {
                    break;
                }
                if (endIndex - startIndex == 1)
                {
                    result.Add(pointList[endIndex]);
                    startIndex = endIndex;
                    endIndex = Math.Min(numberOfPoints - 1, endIndex + parameters.LookAhead.Value);

                    continue;
                }

                if (AnyPerpendicularDistanceExceedTolerance(pointList.Skip(startIndex).Take(endIndex - startIndex + 1).ToList(), effectiveThreshold))
                {
                    endIndex--;
                }
                else
                {
                    result.Add(pointList[endIndex]);
                    startIndex = endIndex;
                    endIndex = Math.Min(numberOfPoints - 1, endIndex + parameters.LookAhead.Value);
                }
            }

            if (parameters.Retain3Points && result.Count == 2)
            {
                result.Insert(1, pointList[pointList.Count / 2]);
            }

            return result;
        }

        // http://psimpl.sourceforge.net/reumann-witkam.html
        public static List<T> SimplifyByReumannWitkam<T>(List<T> pointList, SimplificationParamters parameters) where T : IPoint
        {
            var result = new List<T>();

            if (pointList == null || pointList.Count < 2)
            {
                return result;
            }

            if (pointList.Count == 2)
            {
                return pointList;
            }

            var numberOfPoints = pointList.Count;

            //1399.06.23
            //در این جا برای سرعت بیش‌تر مقدار فاصله استفاه 
            //نمی‌شود بلکه توان دوم آن استفاده می‌شود. این 
            //روش باعث می‌شود در محاسبات از تابع جزر استفاده
            //نشود
            //double effectiveThreshold = threshold * threshold;
            double effectiveThreshold = parameters.DistanceThreshold.Value /** parameters.DistanceThreshold.Value*/;

            result.Add(pointList[0]);

            int startIndex = 0;

            int middleIndex = 1;

            for (int i = 2; i < numberOfPoints; i++)
            {
                var semiPerpendicularDistance = SpatialUtility.GetPointToLineSegmentDistance(pointList[startIndex], pointList[middleIndex], pointList[i]);

                if (semiPerpendicularDistance > effectiveThreshold)
                {
                    result.Add(pointList[i - 1]);
                    startIndex = i - 1;
                    middleIndex = i;
                }
            }

            if (parameters.Retain3Points && result.Count == 1)
            {
                result.Add(pointList[pointList.Count() / 2]);
            }

            result.Add(pointList.Last());

            return result;
        }


        // 1400.05.11
        // http://psimpl.sourceforge.net/perpendicular-distance.html
        // Ekdemir, S., Efficient Implementation of Polyline Simplification for Large Datasets and Usability Evaluation.
        // Master’s thesis, Uppsala University, Department of Information Technology, 2011
        public static List<T> SimplifyByPerpendicularDistance<T>(List<T> points, SimplificationParamters parameters/*, double threshold, bool retain3Points = false*/) where T : IPoint
        {
            if (points == null || points.Count == 0)
            {
                return null;
            }
            else if (points.Count == 2)
            {
                return points;
            }

            List<T> result = new List<T>();

            result.Add(points.First());


            // 1400.06.05
            // استفاده از مقدار توان دوم در مقایسه مشکل‌ساز
            // نخواهد بود چون در هر حال تابع توان دوم هم 
            // اکیدا صعودی است و توان دوم هیچ مقداری از توان
            // دوم مقدار کم‌تر از خودش، بیش‌تر نخواهد شد. اما
            // برای این‌که مقایسه سرعت درست انجام شود
            // استفاده از توان دوم متوقف شد.

            // 1400.05.11
            //در این جا برای سرعت بیش‌تر مقدار فاصله استفاه 
            //نمی‌شود بلکه توان دوم آن استفاده می‌شود. این 
            //روش باعث می‌شود در محاسبات از تابع جزر استفاده
            //نشود

            //double effectiveThreshold = threshold * threshold;
            double effectiveThreshold = parameters.DistanceThreshold.Value /** parameters.DistanceThreshold.Value*/;

            for (int i = 0; i < points.Count - 2; i++)
            {
                var perpendicularDistance = SpatialUtility.GetPointToLineSegmentDistance(points[i], points[i + 2], points[i + 1]);

                if (perpendicularDistance > effectiveThreshold)
                    result.Add(points[i + 1]);

                // 1400.06.05
                // نقطه حذف شده به عنوان نقطه شروع استفاده نمی‌شود
                // این روش در بهترین حالت ۵۰ درصد فشرده سازی ایجاد 
                // می کند
                else
                {
                    result.Add(points[i + 2]);
                    i++;
                }
            }

            if (parameters.Retain3Points && result.Count == 1)
            {
                result.Add(points[points.Count() / 2]);
            }

            if (result.Last().DistanceTo(points.Last()) != 0)
            {
                result.Add(points.Last());
            }

            return result;
        }


        // 1400.05.20
        public static List<T> SimplifyByNormalOpeningWindow<T>(List<T> points, SimplificationParamters parameters/*, double threshold, bool retain3Points = false*/) where T : IPoint
        {
            if (points == null || points.Count == 0)
            {
                return null;
            }
            else if (points.Count == 2)
            {
                return points;
            }

            List<T> result = new List<T>();

            result.Add(points.First());

            // 1400.05.20
            //در این جا برای سرعت بیش‌تر مقدار فاصله استفاه 
            //نمی‌شود بلکه توان دوم آن استفاده می‌شود. این 
            //روش باعث می‌شود در محاسبات از تابع جزر استفاده
            //نشود
            double effectiveThreshold = parameters.DistanceThreshold.Value /** parameters.DistanceThreshold.Value*/;

            int startIndex = 0, middleIndex = 1, endIndex = 2;

            while (endIndex < points.Count)
            {
                while (middleIndex < endIndex)
                {
                    var semiDistance = SpatialUtility.GetPointToLineSegmentDistance(points[startIndex], points[endIndex], points[middleIndex]);

                    if (semiDistance > effectiveThreshold)
                    {
                        result.Add(points[middleIndex]);

                        startIndex = middleIndex;
                        middleIndex = startIndex + 1;

                        // after breaking it will increment by 1 6 lines below
                        endIndex = middleIndex;

                        break;
                    }

                    middleIndex++;
                }

                endIndex++;
                middleIndex = startIndex + 1;
            }

            if (parameters.Retain3Points && result.Count == 1)
            {
                result.Add(points[points.Count() / 2]);
            }

            if (result.Last().DistanceTo(points.Last()) != 0)
            {
                result.Add(points.Last());
            }

            return result;
        }

        // 1400.05.20
        public static List<T> SimplifyByBeforeOpeningWindow<T>(List<T> points, SimplificationParamters parameters/*, double threshold, bool retain3Points = false*/) where T : IPoint
        {
            if (points == null || points.Count == 0)
            {
                return null;
            }
            else if (points.Count == 2)
            {
                return points;
            }

            List<T> result = new List<T>();

            result.Add(points.First());

            // 1400.05.20
            //در این جا برای سرعت بیش‌تر مقدار فاصله استفاه 
            //نمی‌شود بلکه توان دوم آن استفاده می‌شود. این 
            //روش باعث می‌شود در محاسبات از تابع جزر استفاده
            //نشود
            double effectiveThreshold = parameters.DistanceThreshold.Value/* * parameters.DistanceThreshold.Value*/;

            int startIndex = 0, middleIndex = 1, endIndex = 2;

            while (endIndex < points.Count)
            {
                while (middleIndex < endIndex)
                {
                    var semiDistance = SpatialUtility.GetPointToLineSegmentDistance(points[startIndex], points[endIndex], points[middleIndex]);

                    if (semiDistance > effectiveThreshold)
                    {
                        result.Add(points[endIndex - 1]);

                        startIndex = endIndex - 1;
                        middleIndex = startIndex + 1;

                        // after breaking it will increment by 1 6 lines below
                        endIndex = middleIndex;

                        break;
                    }

                    middleIndex++;
                }

                endIndex++;
                middleIndex = startIndex + 1;
            }

            if (parameters.Retain3Points && result.Count == 1)
            {
                result.Add(points[points.Count() / 2]);
            }

            if (result.Last().DistanceTo(points.Last()) != 0)
            {
                result.Add(points.Last());
            }

            return result;
        }


        #region Private Methods

        private static List<T> DivideForDouglasPeucker<T>(List<T> pointList, SimplificationParamters paramters, int divideIndex) where T : IPoint
        {
            var result = new List<T>();

            var leftList = pointList.Take(divideIndex + 1).ToList();

            result = SimplifyByDouglasPeucker(leftList, paramters);

            var rightList = pointList.Skip(divideIndex).ToList();

            var rightResult = SimplifyByDouglasPeucker(rightList, paramters);

            result.AddRange(rightResult.Skip(1));

            return result;
        }


        //private static List<T> DivideForDouglasPeucker<T>(List<T> pointList, double threshold, int divideIndex) where T : IPoint
        //{
        //    var result = new List<T>();

        //    var leftList = pointList.Take(divideIndex + 1).ToList();

        //    result = SimplifyByDouglasPeucker(leftList, threshold);

        //    var rightList = pointList.Skip(divideIndex).ToList();

        //    var rightResult = SimplifyByDouglasPeucker(rightList, threshold);

        //    result.AddRange(rightResult.Skip(1));

        //    return result;
        //}

        private static bool AnyPerpendicularDistanceExceedTolerance<T>(List<T> pointList, double threshold) where T : IPoint
        {
            for (int i = 1; i < pointList.Count - 1; i++)
            {
                var semiPerpendicularDistance = SpatialUtility.GetPointToLineSegmentDistance(pointList[0], pointList[pointList.Count - 1], pointList[i]);

                if (semiPerpendicularDistance >= threshold)
                {
                    return true;
                }
            }

            return false;
        }

        #endregion

    }
}
