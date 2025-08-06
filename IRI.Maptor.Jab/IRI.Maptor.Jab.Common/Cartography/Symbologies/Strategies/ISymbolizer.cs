using System; 

using IRI.Maptor.Sta.Common.Primitives;
using IRI.Maptor.Sta.Spatial.Primitives;
using IRI.Maptor.Jab.Common.Cartography.Common;

namespace IRI.Maptor.Jab.Common.Cartography.Symbologies;

public interface ISymbolizer
{

    SymbologyType Type { get; }

    double? MinScaleDenominator { get; set; }

    double? MaxScaleDenominator { get; set; }

    //VisualParameters Get();

    Func<Feature<Point>, bool> IsFilterPassed { get; }

    Predicate<double> ScalePredicate { get; }

    bool IsInScaleRange(double scale);
    //ImageBrush Render(List<Feature<Point>> features, double mapScale, double screenWidth, double screenHeight);
}
