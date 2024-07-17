using System;
using System.Collections.Generic;
using System.Text;

namespace IRI.Sta.MachineLearning.LogisticRegressionUseCases
{

    /// <summary>
    /// Logistic Regression Simplification Features
    /// </summary>
    public enum LRSimplificationFeatures
    {
        VerticalDistance = 1,
        DistanceToPrevious = 2,
        DistanceToNext = 3,
        Area = 4,
        SquareCosineOfAngle = 5,
        CosineOfAngle = 6,

        // فاصله نقطه اول تا سوم
        BaseLength = 7
    }
}
