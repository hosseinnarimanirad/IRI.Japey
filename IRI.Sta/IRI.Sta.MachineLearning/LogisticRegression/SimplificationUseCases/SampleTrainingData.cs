using System;
using System.Collections.Generic;
using System.Text;

namespace IRI.Sta.MachineLearning.LogisticRegressionUseCases
{
    public class SampleTrainingData
    {
        List<LogisticGeometrySimplificationParameters> zeroAgnleSamples = new List<LogisticGeometrySimplificationParameters>();

        public SampleTrainingData()
        {
            zeroAgnleSamples = new List<LogisticGeometrySimplificationParameters>()
            {
                new LogisticGeometrySimplificationParameters()
                {
                    CosineOfAngle = 0,
                    Area = 100,
                    SquareDistanceToNext = 100,
                    SquareDistanceToPrevious = 100,
                    VerticalSquareDistance = 10,
                    SquareCosineOfAngle = 0,
                    IsRetained = false
                }};
        }
    }
}
