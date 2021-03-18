using IRI.Ket.MachineLearning.Regressions;
using IRI.Msh.Algebra;
using IRI.Msh.Common.Extensions;
using IRI.Msh.Common.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRI.Sta.MachineLearning.LogisticRegressionUseCases
{
    public class LogisticGeometrySimplification
    {
        private LogisticRegression _regression;

        private LogisticGeometrySimplification()
        {

        }

        public static LogisticGeometrySimplification Create(LogisticGeometrySimplificationTrainingData trainingData)
        {
            if (trainingData == null || trainingData.Records.IsNullOrEmpty())
            {
                return null;
            }

            Matrix xValues = new Matrix(trainingData.Records.Count, 6);

            double[] yValues = new double[trainingData.Records.Count];

            for (int i = 0; i < trainingData.Records.Count; i++)
            {
                xValues[i, 0] = trainingData.Records[i].ZoomLevel;
                xValues[i, 1] = trainingData.Records[i].SemiDistanceToNext;
                xValues[i, 2] = trainingData.Records[i].SemiDistanceToPrevious;
                xValues[i, 3] = trainingData.Records[i].SemiArea;
                xValues[i, 4] = trainingData.Records[i].SemiCosineOfAngle;
                xValues[i, 5] = trainingData.Records[i].SemiVerticalDistance;

                yValues[i] = trainingData.Records[i].IsRetained ? 1 : 0;
            }

            var result = new LogisticGeometrySimplification();

            result._regression = new LogisticRegression();

            result._regression.Fit(xValues, yValues);

            return result;
        }

        public static LogisticGeometrySimplification Create<T>(List<T> originalPoints, List<T> simplifiedPoints, int zoomLevel) where T : IPoint
        {
            var parameters = GenerateTrainingData(originalPoints, simplifiedPoints, zoomLevel);

            return Create(parameters);
        }

        // ورودی این متد بایستی نقاط تمیز شده باشد
        public static LogisticGeometrySimplificationTrainingData GenerateTrainingData<T>(List<T> originalPoints, List<T> simplifiedPoints, int zoomLevel) where T : IPoint
        {
            if (originalPoints.IsNullOrEmpty() || simplifiedPoints.IsNullOrEmpty())
            {
                return new LogisticGeometrySimplificationTrainingData();
            }

            List<LogisticGeometrySimplificationParameters> parameters = new List<LogisticGeometrySimplificationParameters>();

            // ایجاد رکوردهای مثبت برای فایل ساده شده
            for (int i = 0; i < simplifiedPoints.Count - 2; i++)
            {
                parameters.Add(new LogisticGeometrySimplificationParameters(simplifiedPoints[i], simplifiedPoints[i + 1], simplifiedPoints[i + 2], zoomLevel)
                {
                    IsRetained = true
                });
            }

            // تعیین ارتباط بین اندکس نقطه در لیست 
            // اصلی و اندکس نقطه در لیست ساده شده
            Dictionary<int, int> indexMap = new Dictionary<int, int>();
            for (int simplifiedIndex = 0; simplifiedIndex < simplifiedPoints.Count; simplifiedIndex++)
            {
                var currentPoint = simplifiedPoints[simplifiedIndex];

                for (int originalIndex = simplifiedIndex; originalIndex < originalPoints.Count; originalIndex++)
                {
                    if (currentPoint.X == originalPoints[originalIndex].X && currentPoint.Y == originalPoints[originalIndex].Y)
                    {
                        indexMap.Add(simplifiedIndex, originalIndex);

                        break;
                    }
                }
            }

            // ایجاد رکوردهای منفی برای فایل اصلی 
            // پیمایش نقاط میانی و یافتن نقاط مانده
            // ابتدا و انتها
            for (int i = 1; i < originalPoints.Count - 1; i++)
            {
                if (indexMap.ContainsValue(i))
                    continue;

                var prevRetainedPoint = indexMap.Where(index => index.Value < i).Last();

                var nextRetainedPoint = indexMap.First(index => index.Value > i);

                parameters.Add(new LogisticGeometrySimplificationParameters(
                    originalPoints[prevRetainedPoint.Value],
                    originalPoints[i],
                    originalPoints[nextRetainedPoint.Value],
                    zoomLevel)
                {
                    IsRetained = false
                });
            }

            return new LogisticGeometrySimplificationTrainingData() { Records = parameters };
        }

        private bool? IsRetained(LogisticGeometrySimplificationParameters parameters)
        {
            double[] xValues = new double[]
            {
                1,
                parameters.ZoomLevel,
                parameters.SemiDistanceToNext,
                parameters.SemiDistanceToPrevious,
                parameters.SemiArea,
                parameters.SemiCosineOfAngle,
                parameters.SemiVerticalDistance,
            };

            var result = _regression.Predict(xValues);

            if (result.HasValue)
            {
                return Math.Round(result.Value) == 1;
            }
            else
            {
                return null;
            }
        }

        public List<T> SimplifyByLogisticRegression<T>(List<T> points, int zoomLevel, bool retain3Points = false) where T : IPoint
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

            int firstIndex = 0, middleIndex = 1, lastIndex = 2;

            for (int i = 2; i < points.Count; i++)
            {
                lastIndex = i;

                var parameters = new LogisticGeometrySimplificationParameters(points[firstIndex], points[middleIndex], points[lastIndex], zoomLevel);

                if (IsRetained(parameters) == true)
                {
                    result.Add(points[middleIndex]);

                    firstIndex = middleIndex;

                    middleIndex = lastIndex;
                }
                else
                {
                    middleIndex = lastIndex;
                }
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
