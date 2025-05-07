using IRI.Sta.Common.Primitives;
using System.Collections.Generic;
using System;

namespace IRI.Sta.Spatial.Analysis;

public class DisplacementComparer<T> : IComparer<Tuple<double, int, int, int, int, T, int>> where T : IPoint
{
    public int Compare(Tuple<double, int, int, int, int, T, int> x, Tuple<double, int, int, int, int, T, int> y)
    {
        return x.Item1.CompareTo(y.Item1);
    }
}