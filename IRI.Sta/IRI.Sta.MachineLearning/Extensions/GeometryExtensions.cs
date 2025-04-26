using IRI.Sta.Common.Analysis;
using IRI.Extensions;
using IRI.Sta.Common.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IRI.Sta.MachineLearning;

namespace IRI.Extensions;

public static class GeometryExtensions
{
    public static Geometry<T> Simplify<T>(
        this Geometry<T> geometry,
        LogisticSimplification<T> model,
        Func<T, T> toScreenMap,
        bool retain3Poins) where T : IPoint, new()
    {
        Func<List<T>, List<T>> filter = pList => SimplifyByLogisticRegression(pList, model, toScreenMap, retain3Poins);

        var temp = geometry.FilterPoints(filter);

        if (temp.NumberOfPoints > geometry.NumberOfPoints)
        {

        }

        return temp;
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
        //return model.SimplifyByLogisticRegression_Fast_O_n(points, toScreenMap, retain3Points);
    }

}
