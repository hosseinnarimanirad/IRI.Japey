﻿using IRI.Msh.Algebra;
using IRI.Extensions;
using IRI.Msh.Common.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRI.Sta.MachineLearning.LogisticRegressionUseCases
{
    public class LogisticGeometrySimplification<T> where T : IPoint, new()
    {
        //private const int _numberOfFeatures = 3;

        private List<LogisticGeometrySimplificationFeatures> _features;

        private LogisticRegression _regression;

        private LogisticGeometrySimplification()
        {

        }

        // ورودی این متد بایستی نقاط تمیز شده باشد
        public static LogisticGeometrySimplificationTrainingData<T> GenerateTrainingData(
            List<T> originalPoints,
            List<T> simplifiedPoints,
            bool isRingMode,
            //int zoomLevel,
            List<LogisticGeometrySimplificationFeatures> features,
            Func<T, T> toScreenMap = null)
        {
            if (originalPoints.IsNullOrEmpty() || simplifiedPoints.IsNullOrEmpty())
            {
                return new LogisticGeometrySimplificationTrainingData<T>();
            }

            List<LogisticGeometrySimplificationParameters<T>> parameters = new List<LogisticGeometrySimplificationParameters<T>>();

            // ایجاد رکوردهای مثبت برای فایل ساده شده
            //for (int i = 0; i < simplifiedPoints.Count - 2; i++)
            //{
            //    parameters.Add(new LogisticGeometrySimplificationParameters(simplifiedPoints[i], simplifiedPoints[i + 1], simplifiedPoints[i + 2], /*zoomLevel,*/ toScreenMap)
            //    {
            //        IsRetained = true
            //    });
            //}

            // تعیین ارتباط بین اندکس نقطه در لیست 
            // اصلی و اندکس نقطه در لیست ساده شده
            Dictionary<int, int> indexMap = new Dictionary<int, int>();

            for (int simplifiedIndex = 0; simplifiedIndex < simplifiedPoints.Count; simplifiedIndex++)
            {
                var currentPoint = simplifiedPoints[simplifiedIndex];

                for (int originalIndex = simplifiedIndex; originalIndex < originalPoints.Count; originalIndex++)
                {
                    if (Msh.Common.Analysis.SpatialUtility.GetEuclideanDistance(currentPoint, originalPoints[originalIndex]) < Msh.Common.Analysis.SpatialUtility.EpsilonDistance)
                    {
                        indexMap.Add(simplifiedIndex, originalIndex);

                        break;
                    }
                }
            }

            if (indexMap.Count != simplifiedPoints.Count)
            {
                throw new NotImplementedException("خطا در شناسایی معادل نقطه عارضه ساده شده در عارضه اصلی");
            }

            var startIndex = indexMap.First().Value;
            var middleIndex = originalPoints.Count <= startIndex + 1 ? 0 : startIndex + 1;
            var lastIndex = originalPoints.Count <= middleIndex + 1 ? 0 : middleIndex + 1;

            do
            {
                if (!isRingMode && lastIndex <= startIndex)
                    break;

                // نقطه حفظ شده
                if (indexMap.ContainsValue(middleIndex))
                {
                    parameters.Add(new LogisticGeometrySimplificationParameters<T>(
                                           originalPoints[startIndex],
                                           originalPoints[middleIndex],
                                           originalPoints[lastIndex],
                                           /*zoomLevel,*/
                                           features,
                                           toScreenMap)
                    {
                        IsRetained = true,
                    });

                    startIndex = middleIndex;
                }
                // نقطه حذف نشده
                else
                {
                    parameters.Add(new LogisticGeometrySimplificationParameters<T>(
                                          originalPoints[startIndex],
                                          originalPoints[middleIndex],
                                          originalPoints[lastIndex],
                                          /*zoomLevel,*/
                                          features,
                                          toScreenMap)
                    {
                        IsRetained = false,
                    });
                }

                middleIndex = lastIndex;
                lastIndex = originalPoints.Count <= middleIndex + 1 ? 0 : middleIndex + 1;

            } while (middleIndex != indexMap.First().Value);

            return new LogisticGeometrySimplificationTrainingData<T>() { Records = parameters };
        }

        public static LogisticGeometrySimplification<T> Create(LogisticGeometrySimplificationTrainingData<T> trainingData)
        {
            if (trainingData == null || trainingData.Records.IsNullOrEmpty())
            {
                return null;
            }

            // 1400.03.20
            //Matrix xValues = new Matrix(trainingData.Records.Count, LogisticGeometrySimplificationParameters.NumberOfFeatures);
            var numberOfFeatures = trainingData.GetNumberOfFeatures();

            Matrix xValues = new Matrix(trainingData.Records.Count, numberOfFeatures);

            double[] yValues = new double[trainingData.Records.Count];

            for (int i = 0; i < trainingData.Records.Count; i++)
            {
                // 1400.03.20
                var features = trainingData.Records[i].FeatureValues;
                //
                for (int j = 0; j < numberOfFeatures; j++)
                {
                    xValues[i, j] = features[j];
                }
                //
                //xValues[i, 0] = trainingData.Records[i].SemiDistanceToNext;
                //xValues[i, 1] = trainingData.Records[i].SemiDistanceToPrevious;
                ////xValues[i, 2] = trainingData.Records[i].SemiVerticalDistance;
                //xValues[i, 2] = trainingData.Records[i].SemiArea;
                //xValues[i, 3] = trainingData.Records[i].SquareCosineOfAngle;
                ////xValues[i, 3] = trainingData.Records[i].CosineOfAngle;

                yValues[i] = trainingData.Records[i].IsRetained ? 1 : 0;
            }

            var result = new LogisticGeometrySimplification<T>();

            result._regression = new LogisticRegression(new LogisticRegressionOptions() { RegularizationMethod = RegularizationMethods.L2 });

            result._regression.Fit(xValues, yValues);

            result._features = trainingData.Records.First().Features;

            return result;
        }

        public static LogisticGeometrySimplification<T> Create(
            List<T> originalPoints,
            List<T> simplifiedPoints,
            //int zoomLevel,
            Func<T, T> toScreenMap,
            bool isRingMode,
            List<LogisticGeometrySimplificationFeatures> features)
        {
            var parameters = GenerateTrainingData(originalPoints, simplifiedPoints, /*zoomLevel,*/ isRingMode, features, toScreenMap);

            return Create(parameters);
        }


        //// ورودی این متد بایستی نقاط تمیز شده باشد
        //public static LogisticGeometrySimplificationTrainingData GenerateTrainingData<T>(
        //    List<T> originalPoints,
        //    List<T> simplifiedPoints,
        //    //int zoomLevel,
        //    Func<IPoint, IPoint> toScreenMap,
        //    bool isRingMode) where T : IPoint
        //{
        //    if (originalPoints.IsNullOrEmpty() || simplifiedPoints.IsNullOrEmpty())
        //    {
        //        return new LogisticGeometrySimplificationTrainingData();
        //    }

        //    List<LogisticGeometrySimplificationParameters> parameters = new List<LogisticGeometrySimplificationParameters>();

        //    // ایجاد رکوردهای مثبت برای فایل ساده شده
        //    for (int i = 0; i < simplifiedPoints.Count - 2; i++)
        //    {
        //        parameters.Add(new LogisticGeometrySimplificationParameters(simplifiedPoints[i], simplifiedPoints[i + 1], simplifiedPoints[i + 2], /*zoomLevel,*/ toScreenMap)
        //        {
        //            IsRetained = true
        //        });
        //    }

        //    // تعیین ارتباط بین اندکس نقطه در لیست 
        //    // اصلی و اندکس نقطه در لیست ساده شده
        //    Dictionary<int, int> indexMap = new Dictionary<int, int>();

        //    for (int simplifiedIndex = 0; simplifiedIndex < simplifiedPoints.Count; simplifiedIndex++)
        //    {
        //        var currentPoint = simplifiedPoints[simplifiedIndex];

        //        for (int originalIndex = simplifiedIndex; originalIndex < originalPoints.Count; originalIndex++)
        //        {
        //            if (currentPoint.X == originalPoints[originalIndex].X && currentPoint.Y == originalPoints[originalIndex].Y)
        //            {
        //                indexMap.Add(simplifiedIndex, originalIndex);

        //                break;
        //            }
        //        }
        //    }

        //    // ایجاد رکوردهای منفی برای فایل اصلی 
        //    // پیمایش نقاط میانی و یافتن نقاط مانده
        //    // ابتدا و انتها
        //    // 1400.03.09
        //    // در حالت چندضلعی از نقاط ابتدا و انتهای
        //    // حذف شده در این جا چشم پوشی شده
        //    for (int i = 1; i < originalPoints.Count - 1; i++)
        //    {
        //        if (indexMap.ContainsValue(i))
        //            continue;

        //        // 1400.03.05
        //        // در حال چند رینگ ممکنه نقاط ابتدایی هم حذف
        //        // شوند این برخلاف حالت خط هست که همیشه
        //        // نقطه اول و اخر رو نگه می داریم
        //        var prevPoints = indexMap.Where(index => index.Value < i);

        //        KeyValuePair<int, int> prevRetainedPoint;

        //        if (isRingMode && prevPoints.IsNullOrEmpty())
        //        {
        //            prevRetainedPoint = indexMap.Last();
        //        }
        //        else
        //        {
        //            //prevRetainedPoint = indexMap.Where(index => index.Value < i).Last();
        //            prevRetainedPoint = prevPoints.Last();
        //        }
        //        //var prevRetainedPoint = indexMap.Where(index => index.Value < i).Last();

        //        //var nextRetainedPoint = indexMap.First(index => index.Value > i);
        //        var nextPoints = indexMap.Where(index => index.Value > i);

        //        KeyValuePair<int, int> nextRetainedPoint;

        //        if (isRingMode && nextPoints.IsNullOrEmpty())
        //        {
        //            nextRetainedPoint = indexMap.First();
        //        }
        //        else
        //        {
        //            //nextRetainedPoint = indexMap.First(index => index.Value > i);
        //            nextRetainedPoint = indexMap.First(index => index.Value > i);

        //        }

        //        parameters.Add(new LogisticGeometrySimplificationParameters(
        //            originalPoints[prevRetainedPoint.Value],
        //            originalPoints[i],
        //            originalPoints[nextRetainedPoint.Value],
        //            //zoomLevel,
        //            toScreenMap)
        //        {
        //            IsRetained = false
        //        });
        //    }

        //    return new LogisticGeometrySimplificationTrainingData() { Records = parameters };
        //}

        private bool? IsRetained(LogisticGeometrySimplificationParameters<T> parameters)
        {
            // 1400.03.20
            //List<double> xValues = new List<double>
            //{
            //    //1,
            //    //parameters.ZoomLevel,
            //    parameters.SemiDistanceToNext,
            //    parameters.SemiDistanceToPrevious,
            //    //parameters.SemiVerticalDistance,
            //    parameters.SemiArea,
            //    parameters.SquareCosineOfAngle,
            //    //parameters.CosineOfAngle
            //};

            List<double> xValues = parameters.FeatureValues;

            var result = _regression.Predict(xValues);

            if (result.HasValue)
            {
                if (result.Value > 1 || result.Value < 0)
                {

                }

                //return Math.Round(result.Value) == 1;
                return result.Value > 0.5;
            }
            else
            {
                return null;
            }
        }

        public List<T> SimplifyByLogisticRegression(List<T> points, /*int zoomLevel, */Func<T, T> toScreenMap, bool retain3Points = false)
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

            var screenPoints = points.Select(p => toScreenMap(p)).ToList();

            // add first point automatically
            result.Add(points.First());

            int firstIndex = 0, middleIndex = 1, lastIndex = 2;
               
            while (lastIndex < points.Count)
            {
                middleIndex = lastIndex - 1;

                int secondFloating = 1;

                while (middleIndex > firstIndex)
                {
                    // 1400.06.04
                    // var parameters = new LogisticGeometrySimplificationParameters<T>(points[firstIndex], points[middleIndex], points[lastIndex], toScreenMap);
                    var parameters = new LogisticGeometrySimplificationParameters<T>(screenPoints[firstIndex], screenPoints[middleIndex], screenPoints[lastIndex], _features, null);
                      
                    if (IsRetained(parameters) == true)
                    {
                        result.Add(points[middleIndex]);
                        result.Add(points[lastIndex]);

                        firstIndex = lastIndex;
                        //middleIndex = lastIndex + 1;
                        //lastIndex = lastIndex + 2;

                        break;
                    }
                    else
                    {
                        middleIndex = middleIndex - secondFloating ;
                        secondFloating = secondFloating * 2;
                    }
                }

                lastIndex++;
            }

            if (retain3Points && result.Count == 1)
            {
                result.Add(points[points.Count() / 2]);
            }

            result.Add(points.Last());

            return result;
        }


        public List<T> SimplifyByLogisticRegression_Fast_O_n_CumulativeArea(List<T> points, /*int zoomLevel, */Func<T, T> toScreenMap, bool retain3Points = false)
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

            var screenPoints = points.Select(p => toScreenMap(p)).ToList();

            // add first point automatically
            result.Add(points.First());

            int firstIndex = 0, middleIndex = 1, lastIndex = 2;

            // 1400.03.10
            var tempArea = 0.0;

            //for (int i = 2; i < points.Count; i++)
            //{
            //    lastIndex = i;

            //    var parameters = new LogisticGeometrySimplificationParameters(points[firstIndex], points[middleIndex], points[lastIndex], /*zoomLevel,*/ toScreenMap);

            //    // 1400.03.10
            //    parameters.Area += tempArea;

            //    if (IsRetained(parameters) == true)
            //    {
            //        result.Add(points[middleIndex]);

            //        firstIndex = middleIndex;

            //        middleIndex = lastIndex;

            //        // 1400.03.10
            //        tempArea = 0;
            //    }
            //    else
            //    {
            //        middleIndex = lastIndex;

            //        // 1400.03.10
            //        tempArea = parameters.Area;
            //    }
            //}

            //var steps = 1;

            //var area = 0.0;

            while (lastIndex < points.Count)
            {
                //middleIndex = firstIndex + 1;
                middleIndex = lastIndex - 1;

                //while (middleIndex > firstIndex)
                //{
                // 1400.06.04
                // var parameters = new LogisticGeometrySimplificationParameters<T>(points[firstIndex], points[middleIndex], points[lastIndex], toScreenMap);
                var parameters = new LogisticGeometrySimplificationParameters<T>(screenPoints[firstIndex], screenPoints[middleIndex], screenPoints[lastIndex], _features, null);

                tempArea += parameters.Area;
                parameters.Area = tempArea; ;

                //if (parameters.DistanceToNext < 1 && parameters.DistanceToPrevious < 1 && parameters.BaseLength < 1)
                //{
                //    middleIndex--;
                //    continue;
                //}

                // 1400.06.06
                ////if (
                ////    //parameters.VerticalSquareDistance < LogisticGeometrySimplificationParameters<T>.MinVerticalSquareDistanceThreshold
                ////    //&&
                ////    (lastIndex - middleIndex) > 5
                ////    )
                ////{
                ////    break;

                ////    //middleIndex--;
                ////    //continue;
                ////}

                if (IsRetained(parameters) == true)
                {
                    result.Add(points[middleIndex]);
                    result.Add(points[lastIndex]);

                    firstIndex = lastIndex;
                    //middleIndex = lastIndex + 1;
                    //lastIndex = lastIndex + 2;

                    //break;
                    tempArea = 0;
                }
                else
                {
                    //break; 
                    //middleIndex--;
                }
                //}

                lastIndex++;
            }

            if (retain3Points && result.Count == 1)
            {
                result.Add(points[points.Count() / 2]);
            }

            result.Add(points.Last());

            return result;
        }

        public List<T> SimplifyByLogisticRegression_Fast_O_n(List<T> points, /*int zoomLevel, */Func<T, T> toScreenMap, bool retain3Points = false)
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

            var screenPoints = points.Select(p => toScreenMap(p)).ToList();

            // add first point automatically
            result.Add(points.First());

            int firstIndex = 0, middleIndex = 1, lastIndex = 2;

            while (lastIndex < points.Count)
            {
                //middleIndex = firstIndex + 1;
                middleIndex = lastIndex - 1;

                var parameters = new LogisticGeometrySimplificationParameters<T>(screenPoints[firstIndex], screenPoints[middleIndex], screenPoints[lastIndex], _features, null);

                if (IsRetained(parameters) == true)
                {
                    result.Add(points[middleIndex]);
                    result.Add(points[lastIndex]);

                    firstIndex = lastIndex;
                    lastIndex++;
                }

                lastIndex++;
            }


            if (retain3Points && result.Count == 1)
            {
                result.Add(points[points.Count() / 2]);
            }

            result.Add(points.Last());

            return result;
        }
    }
}
