using IRI.Msh.Common.Analysis;
using IRI.Extensions;
using IRI.Msh.Common.Primitives;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;

namespace IRI.Sta.MachineLearning.LogisticRegressionUseCases
{
    public class LRSimplificationParameters<T> where T : IPoint, new()
    {
        //public const int NumberOfFeatures = 4;

        /// <summary>
        /// in screen scale
        /// </summary>
        public const double MinVerticalSquareDistanceThreshold = 0.1;

        public double DistanceToPrevious { get; set; }

        public double DistanceToNext { get; set; }

        public double Area { get; set; }

        public double SquareCosineOfAngle { get; set; }

        public double CosineOfAngle { get; set; }

        public double VerticalDistance { get; set; }

        public double BaseLength { get; set; }

        public List<LRSimplificationFeatures> Features { get; set; }

        [JsonIgnore]
        private List<double> _featureValues;

        [JsonIgnore]
        public List<double> FeatureValues
        {
            get
            {
                if (_featureValues.IsNullOrEmpty() || _featureValues.Count != Features?.Count)
                    InitFeatureValues();

                return _featureValues;
            }
        }

        private void InitFeatureValues()
        {
            List<double> result = new List<double>();

            if (Features.Contains(LRSimplificationFeatures.DistanceToNext))
                result.Add(this.DistanceToNext);

            if (Features.Contains(LRSimplificationFeatures.DistanceToPrevious))
                result.Add(this.DistanceToPrevious);

            if (Features.Contains(LRSimplificationFeatures.VerticalDistance))
                result.Add(this.VerticalDistance);

            if (Features.Contains(LRSimplificationFeatures.BaseLength))
                result.Add(this.BaseLength);

            if (Features.Contains(LRSimplificationFeatures.CosineOfAngle))
                result.Add(this.CosineOfAngle);

            if (Features.Contains(LRSimplificationFeatures.SquareCosineOfAngle))
                result.Add(this.SquareCosineOfAngle);

            if (Features.Contains(LRSimplificationFeatures.Area))
                result.Add(this.Area);

            _featureValues = result;
        }

        public bool IsRetained { get; set; }

        public LRSimplificationParameters()
        {
            if (Features.IsNullOrEmpty())
            {
                Features = new List<LRSimplificationFeatures>();
            }
        }

        public LRSimplificationParameters(T first, T middle, T last, List<LRSimplificationFeatures> features, Func<T, T> toScreenMap = null) : this()
        {
            var firstScreenPoint = toScreenMap == null ? first : toScreenMap(first);
            var middleScreenPoint = toScreenMap == null ? middle : toScreenMap(middle);
            var lastScreenPoint = toScreenMap == null ? last : toScreenMap(last);

            this.Features = features;

            if (Features.Contains(LRSimplificationFeatures.Area))
                this.Area = SpatialUtility.GetUnsignedTriangleArea(firstScreenPoint, middleScreenPoint, lastScreenPoint);

            if (Features.Contains(LRSimplificationFeatures.DistanceToNext))
                this.DistanceToNext = SpatialUtility.GetEuclideanDistance(middleScreenPoint, lastScreenPoint);

            if (Features.Contains(LRSimplificationFeatures.DistanceToPrevious))
                this.DistanceToPrevious = SpatialUtility.GetEuclideanDistance(middleScreenPoint, firstScreenPoint);

            if (Features.Contains(LRSimplificationFeatures.CosineOfAngle))
                this.CosineOfAngle = SpatialUtility.GetCosineOfOuterAngle(firstScreenPoint, middleScreenPoint, lastScreenPoint);

            if (Features.Contains(LRSimplificationFeatures.SquareCosineOfAngle))
                this.SquareCosineOfAngle = SpatialUtility.GetSquareCosineOfAngle(firstScreenPoint, middleScreenPoint, lastScreenPoint);
            this.SquareCosineOfAngle = CosineOfAngle * CosineOfAngle;

            if (Features.Contains(LRSimplificationFeatures.VerticalDistance))
                this.VerticalDistance = SpatialUtility.GetPointToLineSegmentDistance(firstScreenPoint, lastScreenPoint, middleScreenPoint);

            if (Features.Contains(LRSimplificationFeatures.BaseLength))
                this.BaseLength = SpatialUtility.GetEuclideanDistance(firstScreenPoint, lastScreenPoint);
        }

        public override string ToString()
        {
            return $"DistanceToNext: {DistanceToNext} \n DistanceToPrevious: {DistanceToPrevious} \n Area: {Area} \n SquareCosineOfAngle: {SquareCosineOfAngle} \n CosineOfAngle: {CosineOfAngle} \n VerticalDistance: {VerticalDistance} \n BaseLength: {BaseLength}";
        }
    }
}
