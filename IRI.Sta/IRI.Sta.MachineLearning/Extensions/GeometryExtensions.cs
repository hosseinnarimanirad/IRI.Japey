using IRI.Ket.MachineLearning.Regressions;
using IRI.Msh.Common.Analysis;
using IRI.Msh.Common.Primitives;
using IRI.Sta.MachineLearning.LogisticRegressionUseCases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRI.Sta.MachineLearning.Extensions
{
    public static class GeometryExtensions
    {
        public static Geometry<T> Simplify<T>(this Geometry<T> geometry, LogisticGeometrySimplification model, bool retain3Poins)
            where T : IPoint, new()
        {
            Func<List<T>, List<T>> filter = pList => SimplifyByLogisticRegression(pList, model, retain3Poins);

            return geometry.FilterPoints(filter);
        }

        public static List<T> SimplifyByLogisticRegression<T>(List<T> points, LogisticGeometrySimplification model, bool retain3Points = false) where T : IPoint
        {
            if (points == null || points.Count == 0)
            {
                return null;
            }
            else if (points.Count == 2)
            {
                return points;
            }

            //List<(int index, double area)> areas = new List<(int index, double area)>();
            List<T> result = new List<T>();

            result.Add(points.First());

            int firstIndex = 0, middleIndex = 1, lastIndex = 2;

            for (int i = 2; i < points.Count; i++)
            {
                lastIndex = i;

                var parameters = new LogisticGeometrySimplificationParameters(points[firstIndex], points[middleIndex], points[lastIndex]);

                if (model.IsRetained(parameters) == true)
                {
                    result.Add(points[middleIndex]);

                    firstIndex = middleIndex;

                    middleIndex = lastIndex;
                }
                else
                {
                    middleIndex = lastIndex;
                }
            }

            if (retain3Points && result.Count == 1)
            {
                result.Add(points[points.Count() / 2]);
            }

            result.Add(points.Last());

            //var filteredIndexes = areas.Where(i => i.area > threshold).Select(i => i.index).ToList();

            //filteredIndexes.Insert(0, 0);

            //if (!points[0].Equals(points[points.Count - 1]))
            //{
            //    filteredIndexes.Add(points.Count - 1);
            //}

            //var result = filteredIndexes.Select(i => points[i]).ToList();            

            return result;
        }

    }
}
