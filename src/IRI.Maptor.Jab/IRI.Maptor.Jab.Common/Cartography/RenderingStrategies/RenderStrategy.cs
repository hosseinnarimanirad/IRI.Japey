using System.Windows.Media;
using System.Collections.Generic;

using IRI.Maptor.Sta.Common.Primitives;
using IRI.Maptor.Sta.Spatial.Primitives;
using IRI.Maptor.Jab.Common.Cartography.Symbologies;

namespace IRI.Maptor.Jab.Common.Cartography.RenderingStrategies;

public abstract class RenderStrategy
{
    protected readonly List<ISymbolizer> _symbolizers;

    public RenderStrategy(List<ISymbolizer> symbolizer)
    {
        _symbolizers = symbolizer;
    }

    public abstract ImageBrush? Render(List<Feature<Point>> features, double mapScale, double screenWidth, double screenHeight);
}
