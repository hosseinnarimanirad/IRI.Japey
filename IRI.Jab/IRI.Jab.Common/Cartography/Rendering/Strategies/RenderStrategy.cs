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
    public abstract ImageBrush? Render(List<Feature<Point>> features, double mapScale, double screenWidth, double screenHeight);
}
