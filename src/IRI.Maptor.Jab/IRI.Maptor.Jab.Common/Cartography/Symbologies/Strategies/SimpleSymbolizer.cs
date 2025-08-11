using System;
using IRI.Maptor.Sta.Common.Primitives;
using IRI.Maptor.Sta.Spatial.Primitives;
using IRI.Maptor.Jab.Common.Cartography.Common;
using System.Windows.Media;
using System.Collections.Generic;

namespace IRI.Maptor.Jab.Common.Cartography.Symbologies;

public class SimpleSymbolizer : SymbolizerBase
{
    public override SymbologyType Type { get => SymbologyType.Single; }

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

    public SimpleSymbolizer(Func<Feature<Point>, bool> filter, VisualParameters visualParameters):this(visualParameters)
    {
        this.IsFilterPassed = filter;
    }

    //public override VisualParameters Get( ) => Param;

    public ImageBrush Render(List<Feature<Point>> features, double mapScale, double screenWidth, double screenHeight)
    {
        throw new NotImplementedException();
    }

    //public override Func<Feature<Point>, bool> IsFilterPassed { get; private set; }

}
