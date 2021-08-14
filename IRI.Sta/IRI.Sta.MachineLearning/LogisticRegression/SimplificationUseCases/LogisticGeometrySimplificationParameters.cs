using IRI.Msh.Common.Analysis;
using IRI.Msh.Common.Primitives;
using System;
using System.Collections.Generic;
using System.Text;

namespace IRI.Sta.MachineLearning.LogisticRegressionUseCases
{
    public class LogisticGeometrySimplificationParameters
    {
        //public const int NumberOfFeatures = 4;

        public const double MinScreenAreaThreshold = 0.001;

        public double SquareDistanceToNext { get; set; }

        public double SquareDistanceToPrevious { get; set; }

        public double Area { get; set; }

        public double SquareCosineOfAngle { get; set; }

        public double CosineOfAngle { get; set; }

        public double VerticalSquareDistance { get; set; }

        //public int ZoomLevel { get; set; }

        public List<double> GetSelectedFeatures()
        {
            return new List<double>()
            {
                SquareDistanceToNext,
                SquareDistanceToPrevious,
                Area,
                SquareCosineOfAngle,
                CosineOfAngle,
                VerticalSquareDistance
            };
        }

        public bool IsRetained { get; set; }

        public LogisticGeometrySimplificationParameters()
        {

        }

        public LogisticGeometrySimplificationParameters(IPoint first, IPoint middle, IPoint last, /*int zoomLevel, */Func<IPoint, IPoint> toScreenMap = null)
        {
            var firstScreenPoint = toScreenMap == null ? first : toScreenMap(first);
            var middleScreenPoint = toScreenMap == null ? middle : toScreenMap(middle);
            var lastScreenPoint = toScreenMap == null ? last : toScreenMap(last);

            //this.SemiDistanceToNext = SpatialUtility.GetSemiDistance(middle, last);

            //this.SemiDistanceToPrevious = SpatialUtility.GetSemiDistance(middle, first);

            //this.SemiArea = SpatialUtility.CalculateUnsignedTriangleArea(first, middle, last);

            //this.SemiCosineOfAngle = SpatialUtility.CalculateSemiCosineOfAngle(first, middle, last);

            //this.SemiVerticalDistance = SpatialUtility.SemiPointToLineSegmentDistance(first, middle, last);

            //this.ZoomLevel = zoomLevel;

            this.Area = SpatialUtility.GetUnsignedTriangleArea(firstScreenPoint, middleScreenPoint, lastScreenPoint);

            // 1400.05.23
            // save performance
            if (Area < MinScreenAreaThreshold)
                return;

            this.SquareDistanceToNext = SpatialUtility.GetSquareDistance(middleScreenPoint, lastScreenPoint);

            this.SquareDistanceToPrevious = SpatialUtility.GetSquareDistance(middleScreenPoint, firstScreenPoint);

            this.SquareCosineOfAngle = SpatialUtility.GetSquareCosineOfAngle(firstScreenPoint, middleScreenPoint, lastScreenPoint);

            this.CosineOfAngle = SpatialUtility.GetCosineOfAngle(firstScreenPoint, middleScreenPoint, lastScreenPoint);

            this.VerticalSquareDistance = SpatialUtility.GetPointToLineSegmentSquareDistance(firstScreenPoint, lastScreenPoint, middleScreenPoint);
        }

        public override string ToString()
        {
            return $"SemiDistanceToNext: {SquareDistanceToNext} \n SemiDistanceToPrevious: {SquareDistanceToPrevious} \n SemiArea: {Area} \n SquareCosineOfAngle: {SquareCosineOfAngle} \n CosineOfAngle: {CosineOfAngle} \n SemiVerticalDistance: {VerticalSquareDistance}";
        }
    }
}
