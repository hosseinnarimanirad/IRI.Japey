using IRI.Msh.Common.Analysis;
using IRI.Msh.Common.Primitives;
using System;
using System.Collections.Generic;
using System.Text;

namespace IRI.Sta.MachineLearning.LogisticRegressionUseCases
{
    public class LogisticGeometrySimplificationParameters
    {
        public double SemiDistanceToNext { get; set; }

        public double SemiDistanceToPrevious { get; set; }

        public double SemiArea { get; set; }

        public double SemiCosineOfAngle { get; set; }

        public double SemiVerticalDistance { get; set; }

        public int ZoomLevel { get; set; }

        public bool IsRetained { get; set; }

        public LogisticGeometrySimplificationParameters(IPoint first, IPoint middle, IPoint last, int zoomLevel)
        {             
            this.SemiDistanceToNext = SpatialUtility.GetSemiDistance(middle, last);
            
            this.SemiDistanceToPrevious = SpatialUtility.GetSemiDistance(middle, first);

            this.SemiArea = SpatialUtility.CalculateUnsignedTriangleArea(first, middle, last);

            this.SemiCosineOfAngle = SpatialUtility.CalculateSemiCosineOfAngle(first, middle, last);

            this.SemiVerticalDistance = SpatialUtility.SemiPointToLineSegmentDistance(first, middle, last);

            this.ZoomLevel = zoomLevel;
        }        
    }
}
