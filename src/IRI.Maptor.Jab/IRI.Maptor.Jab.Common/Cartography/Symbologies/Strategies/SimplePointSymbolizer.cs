using System;
using System.Collections.Generic;
using System.Windows.Media;
using IRI.Maptor.Sta.Common.Primitives;
using IRI.Maptor.Sta.Spatial.Primitives;

namespace IRI.Maptor.Jab.Common.Cartography.Symbologies;

public class SimplePointSymbolizer : SymbolizerBase
{
    public override SymbologyType Type { get => SymbologyType.Single; }

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

    public Predicate<Feature<Point>> IsFilterPassed => throw new NotImplementedException();

    public SimplePointSymbolizer()
    {

    }

    public SimplePointSymbolizer(double pointSize)
    {
        SymbolHeight = pointSize;

        SymbolWidth = pointSize;
    }

    //public override VisualParameters Get()
    //{
    //    throw new System.NotImplementedException();
    //}

    public ImageBrush Render(List<Feature<Point>> features, double mapScale, double screenWidth, double screenHeight)
    {
        throw new NotImplementedException();
    }

}
