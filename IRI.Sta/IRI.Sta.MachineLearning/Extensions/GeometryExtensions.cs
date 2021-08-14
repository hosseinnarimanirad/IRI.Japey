using IRI.Msh.Common.Analysis;
using IRI.Msh.Common.Extensions;
using IRI.Msh.Common.Primitives;
using IRI.Sta.MachineLearning.LogisticRegressionUseCases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRI.Ket.Common.Extensions
{
    public static class GeometryExtensions
    {
        public static Geometry<T> Simplify<T>(
            this Geometry<T> geometry,
            LogisticGeometrySimplification model,
            //int zoomLevel,
            Func<IPoint, IPoint> toScreenMap,
            bool retain3Poins)
            where T : IPoint, new()
        {

            //var newGeometry = geometry.Transform(t => (T)toScreenMap(t));

            Func<List<T>, List<T>> filter = pList => SimplifyByLogisticRegression(pList, model, /*zoomLevel, *//*toScreenMap*/null, retain3Poins);

            return geometry.FilterPoints(filter);
            //return newGeometry.FilterPoints(filter);
        }

        public static List<T> SimplifyByLogisticRegression<T>(
            this List<T> points,
            LogisticGeometrySimplification model,
            //int zoomLevel,
            Func<IPoint, IPoint> toScreenMap,
            bool retain3Points = false) where T : IPoint
        {
            if (points.IsNullOrEmpty())
                return points;

            return model.SimplifyByLogisticRegression(points, /*zoomLevel,*/ toScreenMap, retain3Points);
        }

    }
}
