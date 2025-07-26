using IRI.Sta.Common.Primitives;
using IRI.Sta.Spatial.Primitives;
using System.Windows.Media;

namespace IRI.Jab.Common.Cartography;

public interface IStyle
{
    VisualParameters Get(Feature<Point> feature, double scale);
} 