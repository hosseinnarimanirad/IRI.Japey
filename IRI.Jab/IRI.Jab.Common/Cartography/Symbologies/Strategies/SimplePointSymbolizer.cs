using System.Windows.Media;
using IRI.Jab.Common.Cartography.Common;
using IRI.Sta.Common.Primitives;
using IRI.Sta.Spatial.Primitives;

namespace IRI.Jab.Common.Cartography.Symbologies;

public class SimplePointSymbolizer : Notifier, ISymbolizer
{
    public SymbologyType Type { get => SymbologyType.Single; }

    private double _symbolWidth = 16;

    public double SymbolWidth
    {
        get { return _symbolWidth; }
        set
        {
            _symbolWidth = value;
            RaisePropertyChanged();
        }
    }

    private double _symbolHeight = 16;

    public double SymbolHeight
    {
        get { return _symbolHeight; }
        set
        {
            _symbolHeight = value;
            RaisePropertyChanged();
        }
    }


    private Geometry _geometryPointSymbol;

    public Geometry GeometryPointSymbol
    {
        get { return _geometryPointSymbol; }
        set
        {
            _geometryPointSymbol = value;
            RaisePropertyChanged();
        }
    }


    private ImageSource _imagePointSymbol;

    public ImageSource ImagePointSymbol
    {
        get { return _imagePointSymbol; }
        set
        {
            _imagePointSymbol = value;
            RaisePropertyChanged();
        }
    }


    private System.Drawing.Image _imagePointSymbolGdiPlus;

    public System.Drawing.Image ImagePointSymbolGdiPlus
    {
        get { return _imagePointSymbolGdiPlus; }
        set
        {
            _imagePointSymbolGdiPlus = value;
            RaisePropertyChanged();
        }
    }

    public SimplePointSymbolizer()
    {

    }

    public SimplePointSymbolizer(double pointSize)
    {
        SymbolHeight = pointSize;

        SymbolWidth = pointSize;
    }

    public VisualParameters Get(Feature<Point> feature, double scale)
    {
        throw new System.NotImplementedException();
    }
}
