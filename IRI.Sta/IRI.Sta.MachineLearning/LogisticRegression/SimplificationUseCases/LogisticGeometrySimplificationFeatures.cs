using System;
using System.Collections.Generic;
using System.Text;

namespace IRI.Sta.MachineLearning.LogisticRegressionUseCases
{
    [Flags]
    public enum LogisticGeometrySimplificationFeatures
    {
        VerticalDistance = 1,
        DistanceToPrevious = 2,
        DistanceToNext = 3,
        Area = 4,
        SquareCosineOfAngle = 5,
        CosineOfAngle = 6,

        // فاصله نقطه اول تا سوم
        BaseLength = 7

        //VerticalDistance = 1,
        //DistanceToPrevious = 2,
        //DistanceToNext = 4,
        //Area = 8,
        //SquareCosineOfAngle = 16,
        //CosineOfAngle = 32,

        //// فاصله نقطه اول تا سوم
        //BaseLength = 64
    }
}
