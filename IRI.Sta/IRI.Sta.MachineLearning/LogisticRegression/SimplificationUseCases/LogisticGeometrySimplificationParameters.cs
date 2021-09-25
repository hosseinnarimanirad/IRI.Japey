using IRI.Msh.Common.Analysis;
using IRI.Msh.Common.Extensions;
using IRI.Msh.Common.Primitives;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace IRI.Sta.MachineLearning.LogisticRegressionUseCases
{
    public class LogisticGeometrySimplificationParameters<T> where T : IPoint
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

        public List<LogisticGeometrySimplificationFeatures> Features { get; set; }

        [JsonIgnore]
        private List<double> _featureValues;

        [JsonIgnore]
        public List<double> FeatureValues
        {
            get
            {
                if (_featureValues.IsNullOrEmpty())
                    InitFeatureValues();

                return _featureValues;
            }
        }

        private void InitFeatureValues()
        {
            List<double> result = new List<double>();

            if (Features.Contains(LogisticGeometrySimplificationFeatures.Area))
                result.Add(this.Area);

            if (Features.Contains(LogisticGeometrySimplificationFeatures.DistanceToNext))
                result.Add(this.DistanceToNext);

            if (Features.Contains(LogisticGeometrySimplificationFeatures.DistanceToPrevious))
                result.Add(this.DistanceToPrevious);

            if (Features.Contains(LogisticGeometrySimplificationFeatures.CosineOfAngle))
                result.Add(this.CosineOfAngle);

            if (Features.Contains(LogisticGeometrySimplificationFeatures.SquareCosineOfAngle))
                result.Add(this.SquareCosineOfAngle);

            if (Features.Contains(LogisticGeometrySimplificationFeatures.VerticalDistance))
                result.Add(this.VerticalDistance);

            if (Features.Contains(LogisticGeometrySimplificationFeatures.BaseLength))
                result.Add(this.BaseLength);

            _featureValues = result;
        }

        public bool IsRetained { get; set; }

        public LogisticGeometrySimplificationParameters()
        {
            if (Features.IsNullOrEmpty())
            {
                Features = new List<LogisticGeometrySimplificationFeatures>()
                {
                   //LogisticGeometrySimplificationFeatures.VerticalDistance ,
                   //LogisticGeometrySimplificationFeatures.DistanceToNext ,
                   //LogisticGeometrySimplificationFeatures.DistanceToPrevious ,
                   //LogisticGeometrySimplificationFeatures.Area ,
                   //LogisticGeometrySimplificationFeatures.CosineOfAngle ,
                   //LogisticGeometrySimplificationFeatures.SquareCosineOfAngle,
                   //LogisticGeometrySimplificationFeatures.BaseLength
                };
            }

            //FeatureValues = new List<double>()
            //{
            //    VerticalSquareDistance,
            //    SquareDistanceToNext,
            //    SquareDistanceToPrevious,
            //    //Area,
            //    //CosineOfAngle,
            //    //SquareCosineOfAngle,
            //};
        }

        public LogisticGeometrySimplificationParameters(T first, T middle, T last, /*int zoomLevel, */List<LogisticGeometrySimplificationFeatures> features, Func<T, T> toScreenMap = null) : this()
        {
            var firstScreenPoint = toScreenMap == null ? first : toScreenMap(first);
            var middleScreenPoint = toScreenMap == null ? middle : toScreenMap(middle);
            var lastScreenPoint = toScreenMap == null ? last : toScreenMap(last);

            this.Features = features;

            if (Features.Contains(LogisticGeometrySimplificationFeatures.Area))
                this.Area = SpatialUtility.GetUnsignedTriangleArea(firstScreenPoint, middleScreenPoint, lastScreenPoint);

            if (Features.Contains(LogisticGeometrySimplificationFeatures.DistanceToNext))
                this.DistanceToNext = SpatialUtility.GetDistance(middleScreenPoint, lastScreenPoint);

            if (Features.Contains(LogisticGeometrySimplificationFeatures.DistanceToPrevious))
                this.DistanceToPrevious = SpatialUtility.GetDistance(middleScreenPoint, firstScreenPoint);

            if (Features.Contains(LogisticGeometrySimplificationFeatures.CosineOfAngle))
                this.CosineOfAngle = SpatialUtility.GetCosineOfAngle(firstScreenPoint, middleScreenPoint, lastScreenPoint);
             
            if (Features.Contains(LogisticGeometrySimplificationFeatures.SquareCosineOfAngle))
                //this.SquareCosineOfAngle = SpatialUtility.GetSquareCosineOfAngle(firstScreenPoint, middleScreenPoint, lastScreenPoint);
                this.SquareCosineOfAngle = CosineOfAngle * CosineOfAngle;

            if (Features.Contains(LogisticGeometrySimplificationFeatures.VerticalDistance))
                this.VerticalDistance = SpatialUtility.GetPointToLineSegmentDistance(firstScreenPoint, lastScreenPoint, middleScreenPoint);

            if (Features.Contains(LogisticGeometrySimplificationFeatures.BaseLength))
                this.BaseLength = SpatialUtility.GetDistance(firstScreenPoint, lastScreenPoint);

        }

        public override string ToString()
        {
            return $"DistanceToNext: {DistanceToNext} \n DistanceToPrevious: {DistanceToPrevious} \n Area: {Area} \n SquareCosineOfAngle: {SquareCosineOfAngle} \n CosineOfAngle: {CosineOfAngle} \n SemiVerticalDistance: {VerticalDistance} \n BaseLength: {BaseLength}";
        }
    }
}
