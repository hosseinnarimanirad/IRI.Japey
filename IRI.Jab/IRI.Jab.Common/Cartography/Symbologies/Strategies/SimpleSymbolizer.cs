using System;
using IRI.Sta.Common.Primitives;
using IRI.Sta.Spatial.Primitives;
using IRI.Jab.Common.Cartography.Common;
using System.Windows.Media;
using System.Collections.Generic;

namespace IRI.Jab.Common.Cartography.Symbologies;

public class SimpleSymbolizer : Notifier, ISymbolizer
{
    public SymbologyType Type { get => SymbologyType.Single; }

    private VisualParameters _param;

    public VisualParameters Param
    {
        get { return _param; }
        set
        {
            _param = value;
            RaisePropertyChanged();
        }
    }

    public SimpleSymbolizer(VisualParameters visualParameters)
    {
        Param = visualParameters;
    }


    public VisualParameters Get(Feature<Point> feature, double scale) => Param;

    public ImageBrush Render(List<Feature<Point>> features, double mapScale, double screenWidth, double screenHeight)
    {
        throw new NotImplementedException();
    }

    public Predicate<Feature<Point>> IsFilterPassed => feature => true;

}
