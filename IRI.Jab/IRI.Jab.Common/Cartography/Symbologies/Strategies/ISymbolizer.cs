using IRI.Jab.Common.Cartography.Common;
using IRI.Sta.Common.Primitives;
using IRI.Sta.Spatial.Primitives;

namespace IRI.Jab.Common.Cartography.Symbologies;

public interface ISymbolizer
{
    SymbologyType Type { get; }

    VisualParameters Get(Feature<Point> feature, double scale);
}
