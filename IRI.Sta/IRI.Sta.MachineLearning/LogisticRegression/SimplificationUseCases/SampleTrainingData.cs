using IRI.Msh.Common.Primitives;
using System;
using System.Collections.Generic;
using System.Text;

namespace IRI.Sta.MachineLearning.LogisticRegressionUseCases
{
    public class SampleTrainingData
    {
        List<LogisticGeometrySimplificationParameters<Point>> zeroAgnleSamples = new List<LogisticGeometrySimplificationParameters<Point>>();

        public SampleTrainingData()
        {
            zeroAgnleSamples = new List<LogisticGeometrySimplificationParameters<Point>>()
            {
                new LogisticGeometrySimplificationParameters<Point>()
                {
                    CosineOfAngle = 0,
                    Area = 100,
                    DistanceToNext = 100,
                    DistanceToPrevious = 100,
                    VerticalDistance = 10,
                    SquareCosineOfAngle = 0,
                    IsRetained = false
                }};
        }

        public IEnumerable<LogisticGeometrySimplificationParameters<Point>> Get()
        {
            //"100 100, 200 100.5, 300 100"
            yield return new LogisticGeometrySimplificationParameters<Point>() {   };
        }
    }
}
