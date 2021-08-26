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

        public double SquareDistanceToPrevious { get; set; }

        public double SquareDistanceToNext { get; set; }

        public double Area { get; set; }

        public double SquareCosineOfAngle { get; set; }

        public double CosineOfAngle { get; set; }

        public double VerticalSquareDistance { get; set; }

        [JsonIgnore]
        public LogisticGeometrySimplificationFeatures Features { get; private set; }

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

            if (Features.HasFlag(LogisticGeometrySimplificationFeatures.Area))
                result.Add(this.Area);

            if (Features.HasFlag(LogisticGeometrySimplificationFeatures.SquareDistanceToNext))
                result.Add(this.SquareDistanceToNext);

            if (Features.HasFlag(LogisticGeometrySimplificationFeatures.SquareDistanceToPrevious))
                result.Add(this.SquareDistanceToPrevious);

            if (Features.HasFlag(LogisticGeometrySimplificationFeatures.CosineOfAngle))
                result.Add(this.CosineOfAngle);

            if (Features.HasFlag(LogisticGeometrySimplificationFeatures.SquareCosineOfAngle))
                result.Add(this.SquareCosineOfAngle);

            if (Features.HasFlag(LogisticGeometrySimplificationFeatures.VerticalSquareDistance))
                result.Add(this.VerticalSquareDistance);

            _featureValues = result;
        }

        public bool IsRetained { get; set; }

        public LogisticGeometrySimplificationParameters()
        {
            Features =
                LogisticGeometrySimplificationFeatures.VerticalSquareDistance |
                LogisticGeometrySimplificationFeatures.SquareDistanceToNext |
                LogisticGeometrySimplificationFeatures.SquareDistanceToPrevious;//|
                //LogisticGeometrySimplificationFeatures.Area;
            //LogisticGeometrySimplificationFeatures.CosineOfAngle,
            //LogisticGeometrySimplificationFeatures.SquareCosineOfAngle

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

        public LogisticGeometrySimplificationParameters(T first, T middle, T last, /*int zoomLevel, */Func<T, T> toScreenMap = null) : this()
        {
            var firstScreenPoint = toScreenMap == null ? first : toScreenMap(first);
            var middleScreenPoint = toScreenMap == null ? middle : toScreenMap(middle);
            var lastScreenPoint = toScreenMap == null ? last : toScreenMap(last);

            if (Features.HasFlag(LogisticGeometrySimplificationFeatures.Area))
                this.Area = SpatialUtility.GetUnsignedTriangleArea(firstScreenPoint, middleScreenPoint, lastScreenPoint);

            if (Features.HasFlag(LogisticGeometrySimplificationFeatures.SquareDistanceToNext))
                this.SquareDistanceToNext = SpatialUtility.GetSquareDistance(middleScreenPoint, lastScreenPoint);

            if (Features.HasFlag(LogisticGeometrySimplificationFeatures.SquareDistanceToPrevious))
                this.SquareDistanceToPrevious = SpatialUtility.GetSquareDistance(middleScreenPoint, firstScreenPoint);

            if (Features.HasFlag(LogisticGeometrySimplificationFeatures.CosineOfAngle))
                this.CosineOfAngle = SpatialUtility.GetCosineOfAngle(firstScreenPoint, middleScreenPoint, lastScreenPoint);


            if (Features.HasFlag(LogisticGeometrySimplificationFeatures.SquareCosineOfAngle))
                //this.SquareCosineOfAngle = SpatialUtility.GetSquareCosineOfAngle(firstScreenPoint, middleScreenPoint, lastScreenPoint);
                this.SquareCosineOfAngle = CosineOfAngle * CosineOfAngle;

            if (Features.HasFlag(LogisticGeometrySimplificationFeatures.VerticalSquareDistance))
                this.VerticalSquareDistance = SpatialUtility.GetPointToLineSegmentDistance(firstScreenPoint, lastScreenPoint, middleScreenPoint);
        }

        public override string ToString()
        {
            return $"SemiDistanceToNext: {SquareDistanceToNext} \n SemiDistanceToPrevious: {SquareDistanceToPrevious} \n SemiArea: {Area} \n SquareCosineOfAngle: {SquareCosineOfAngle} \n CosineOfAngle: {CosineOfAngle} \n SemiVerticalDistance: {VerticalSquareDistance}";
        }
    }
}
