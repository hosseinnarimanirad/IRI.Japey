using IRI.Extensions;
using IRI.Msh.Common.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRI.Sta.MachineLearning.LogisticRegressionUseCases
{
    /// <summary>
    /// Logistic Regression Simplification Training Data
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class LRSimplificationTrainingData<T> where T : IPoint, new()
    {
        public int GetNumberOfFeatures()
        {
            //if (Records.IsNullOrEmpty())
            //{
            //    return 0;
            //}

            //var features = Records.First().Features;

            //return features.Count;

            return Features.Count;
        }

        public List<LRSimplificationParameters<T>> Records { get; set; }

        public List<LRSimplificationFeatures> Features { get; set; } = new List<LRSimplificationFeatures>();

        public LRSimplificationTrainingData() : this(new List<LRSimplificationParameters<T>>(), new List<LRSimplificationFeatures>())
        {

        }

        public LRSimplificationTrainingData(List<LRSimplificationParameters<T>> records) : this(records, records.First().Features)
        {
        }

        private LRSimplificationTrainingData(List<LRSimplificationParameters<T>> records, List<LRSimplificationFeatures> features)
        {
            this.Records = records;

            this.Features = features;
        }

        public void Save(string fileName)
        {
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(this);

            System.IO.File.WriteAllText(fileName, jsonString);
        }

        public static LRSimplificationTrainingData<Point> Load(string fileName)
        {
            try
            {
                var jsonString = System.IO.File.ReadAllText(fileName);

                return Newtonsoft.Json.JsonConvert.DeserializeObject<LRSimplificationTrainingData<Point>>(jsonString);
            }
            catch (Exception ex)
            {
                return null;
            }
        }



        // ورودی این متد بایستی نقاط تمیز شده باشد
        public static LRSimplificationTrainingData<T> Create(
            List<T> originalPoints,
            List<T> simplifiedPoints,
            bool isRingMode,
            List<LRSimplificationFeatures> features,
            Func<T, T> toScreenMap = null)
        {
            if (originalPoints.IsNullOrEmpty() || simplifiedPoints.IsNullOrEmpty())
                return new LRSimplificationTrainingData<T>();

            List<LRSimplificationParameters<T>> parameters = new List<LRSimplificationParameters<T>>();

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
                    parameters.Add(new LRSimplificationParameters<T>(
                                           originalPoints[startIndex],
                                           originalPoints[middleIndex],
                                           originalPoints[lastIndex],
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
                    parameters.Add(new LRSimplificationParameters<T>(
                                          originalPoints[startIndex],
                                          originalPoints[middleIndex],
                                          originalPoints[lastIndex],
                                          features,
                                          toScreenMap)
                    {
                        IsRetained = false,
                    });
                }

                middleIndex = lastIndex;
                lastIndex = originalPoints.Count <= middleIndex + 1 ? 0 : middleIndex + 1;

            } while (middleIndex != indexMap.First().Value);

            return new LRSimplificationTrainingData<T>(parameters, features);
        }

    }
}
