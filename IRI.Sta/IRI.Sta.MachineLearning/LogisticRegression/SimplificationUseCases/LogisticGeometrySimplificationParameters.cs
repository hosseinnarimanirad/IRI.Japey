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

        public double SemiDistanceToNext { get; set; }

        public double SemiDistanceToPrevious { get; set; }

        public double SemiArea { get; set; }

        public double SquareCosineOfAngle { get; set; }

        public double CosineOfAngle { get; set; }

        public double SemiVerticalDistance { get; set; }

        //public int ZoomLevel { get; set; }

        public List<double> GetSelectedFeatures()
        {
            return new List<double>()
            {
                SemiDistanceToNext,
                SemiDistanceToPrevious,
                SemiArea,
                SquareCosineOfAngle,
                CosineOfAngle,
                SemiVerticalDistance
            };
        }

        public bool IsRetained { get; set; }

        public LogisticGeometrySimplificationParameters()
        {

        }

        public LogisticGeometrySimplificationParameters(IPoint first, IPoint middle, IPoint last, /*int zoomLevel, */Func<IPoint, IPoint> toScreenMap)
        {
            var firstScreenPoint = toScreenMap(first);
            var middleScreenPoint = toScreenMap(middle);
            var lastScreenPoint = toScreenMap(last);

            //this.SemiDistanceToNext = SpatialUtility.GetSemiDistance(middle, last);

            //this.SemiDistanceToPrevious = SpatialUtility.GetSemiDistance(middle, first);

            //this.SemiArea = SpatialUtility.CalculateUnsignedTriangleArea(first, middle, last);

            //this.SemiCosineOfAngle = SpatialUtility.CalculateSemiCosineOfAngle(first, middle, last);

            //this.SemiVerticalDistance = SpatialUtility.SemiPointToLineSegmentDistance(first, middle, last);

            //this.ZoomLevel = zoomLevel;


            this.SemiDistanceToNext = SpatialUtility.GetSquareDistance(middleScreenPoint, lastScreenPoint);

            this.SemiDistanceToPrevious = SpatialUtility.GetSquareDistance(middleScreenPoint, firstScreenPoint);

            this.SemiArea = SpatialUtility.GetUnsignedTriangleArea(firstScreenPoint, middleScreenPoint, lastScreenPoint);

            this.SquareCosineOfAngle = SpatialUtility.GetSquareCosineOfAngle(firstScreenPoint, middleScreenPoint, lastScreenPoint);

            this.CosineOfAngle = SpatialUtility.GetCosineOfAngle(firstScreenPoint, middleScreenPoint, lastScreenPoint);

            this.SemiVerticalDistance = SpatialUtility.GetPointToLineSegmentSquareDistance(firstScreenPoint, lastScreenPoint, middleScreenPoint);
        }

        public override string ToString()
        {
            return $"SemiDistanceToNext: {SemiDistanceToNext} \n SemiDistanceToPrevious: {SemiDistanceToPrevious} \n SemiArea: {SemiArea} \n SquareCosineOfAngle: {SquareCosineOfAngle} \n CosineOfAngle: {CosineOfAngle} \n SemiVerticalDistance: {SemiVerticalDistance}";
        }
    }
}
