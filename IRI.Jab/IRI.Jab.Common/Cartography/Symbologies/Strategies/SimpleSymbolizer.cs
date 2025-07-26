using System.Windows.Media;
using IRI.Jab.Common.Cartography.Common;
using IRI.Sta.Common.Primitives;
using IRI.Sta.Spatial.Primitives;

namespace IRI.Jab.Common.Cartography.Symbologies;

public class SimpleSymbolizer : Notifier, ISymbolizer
{
    public SymbologyType Type { get => SymbologyType.Single; }

    public VisualParameters Get(Feature<Point> feature, double scale) => Param;

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
}
