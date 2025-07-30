using IRI.Jab.Common.Cartography.Symbologies;
using IRI.Sta.Common.Primitives;
using IRI.Sta.Spatial.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace IRI.Jab.Common.Cartography.Rendering;

public abstract class RenderStrategy
{
    protected readonly List<ISymbolizer> _symbolizers;

    public RenderStrategy(List<ISymbolizer> symbolizer)
    {
        this._symbolizers = symbolizer;
    }

    public abstract ImageBrush? Render(List<Feature<Point>> features, double mapScale, double screenWidth, double screenHeight);
}
