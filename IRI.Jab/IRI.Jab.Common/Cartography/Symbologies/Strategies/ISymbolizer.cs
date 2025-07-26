using IRI.Jab.Common.Cartography.Common;
using IRI.Sta.Common.Primitives;
using IRI.Sta.Spatial.Primitives;
using System;

namespace IRI.Jab.Common.Cartography.Symbologies;

public interface ISymbolizer
{
    SymbologyType Type { get; }

    VisualParameters Get(Feature<Point> feature, double scale);

    Predicate<Feature<Point>> IsFilterPassed { get; }
}
