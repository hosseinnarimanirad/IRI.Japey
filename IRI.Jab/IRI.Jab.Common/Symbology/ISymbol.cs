using IRI.Sta.Common.Primitives;
using IRI.Sta.Spatial.Primitives;
using System.Windows.Media;

namespace IRI.Jab.Common.Symbology;

public interface ISymbol
{
    VisualParameters Get(Feature<Point> feature);
}