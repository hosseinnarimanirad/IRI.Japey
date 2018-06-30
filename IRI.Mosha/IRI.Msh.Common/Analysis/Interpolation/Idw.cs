using IRI.Msh.Common.Primitives;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace IRI.Msh.Common.Analysis.Interpolation
{
    public static class Idw
    {
        public static double Calculate(IEnumerable<Point3D> points, IPoint measurePoint)
        {
            //var weights = new List<double>();

            double weightedSum = 0;

            double sum = 0;

            foreach (var item in points)
            {
                double distance = measurePoint.DistanceTo(item);

                double weight = 1.0 / (distance * distance);

                //weights.Add(weight);

                weightedSum += weight * item.Z;

                sum += weight;
            }

            return (weightedSum / sum);
        }
    }
}
