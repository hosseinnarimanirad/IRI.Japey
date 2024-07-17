using IRI.Msh.Common.Analysis;
using IRI.Extensions;
using IRI.Msh.Common.Primitives;
using IRI.Sta.MachineLearning.LogisticRegressionUseCases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRI.Extensions
{
    public static class GeometryExtensions
    {
        public static Geometry<T> Simplify<T>(
            this Geometry<T> geometry,
            LogisticSimplification<T> model,
            Func<T, T> toScreenMap,
            bool retain3Poins) where T : IPoint, new()
        {
            Func<List<T>, List<T>> filter = pList => SimplifyByLogisticRegression(pList, model, toScreenMap, retain3Poins);

            return geometry.FilterPoints(filter);
        }

        public static List<T> SimplifyByLogisticRegression<T>(
            this List<T> points,
            LogisticSimplification<T> model,
            Func<T, T> toScreenMap,
            bool retain3Points = false) where T : IPoint, new()
        {
            if (points.IsNullOrEmpty())
                return points;

            return model.SimplifyByLogisticRegression(points, toScreenMap, retain3Points);
        }

    }
}
