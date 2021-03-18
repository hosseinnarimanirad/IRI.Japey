using IRI.Ket.MachineLearning.Regressions;
using IRI.Msh.Common.Analysis;
using IRI.Msh.Common.Extensions;
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
        public static Geometry<T> Simplify<T>(this Geometry<T> geometry, LogisticGeometrySimplification model, int zoomLevel, bool retain3Poins)
            where T : IPoint, new()
        {
            Func<List<T>, List<T>> filter = pList => SimplifyByLogisticRegression(pList, model, zoomLevel, retain3Poins);

            return geometry.FilterPoints(filter);
        }

        public static List<T> SimplifyByLogisticRegression<T>(this List<T> points, LogisticGeometrySimplification model, int zoomLevel, bool retain3Points = false) where T : IPoint
        {
            if (points.IsNullOrEmpty())
                return points;

            return model.SimplifyByLogisticRegression(points, zoomLevel, retain3Points);
        }

    }
}
