using System;
using System.Collections.Generic;
using System.Text;

namespace IRI.Sta.MachineLearning.LogisticRegressionUseCases
{
    [Flags]
    public enum LogisticGeometrySimplificationFeatures
    {
        VerticalSquareDistance = 1,
        SquareDistanceToPrevious = 2,
        SquareDistanceToNext = 4,
        Area = 8,
        SquareCosineOfAngle = 16,
        CosineOfAngle = 32,
    }
}
