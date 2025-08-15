using System.Windows.Media;
using IRI.Maptor.Jab.Common.Cartography.Symbologies;
using IRI.Maptor.Jab.Common.Helpers;
using IRI.Maptor.Sta.Common.Primitives;
using IRI.Maptor.Sta.Spatial.Primitives;

namespace IRI.Maptor.Jab.Common;

public partial class VisualParameters
{
    public static VisualParameters GetFill(Color fill, double opacity = 1)
    {
        return new VisualParameters(new SolidColorBrush(fill), null, 0, opacity);
    }

    public static VisualParameters GetStroke(Color stroke, double strokeThickness = 1, double opacity = 1)
    {
        return new VisualParameters(null, new SolidColorBrush(stroke), strokeThickness, opacity);
    }

    public static VisualParameters GetFill(string? hexFill, double opacity = 1)
    {            
        return new VisualParameters(new SolidColorBrush(ColorHelper.ToWpfColor(hexFill)), null, 0, opacity);
    }

    public static VisualParameters GetStroke(string? hexStroke, double strokeThickness = 1, double opacity = 1)
    {
        return new VisualParameters(null, new SolidColorBrush(ColorHelper.ToWpfColor(hexStroke)), strokeThickness, opacity);
    }

    public static VisualParameters Get(Color fill, Color stroke, double strokeThickness, double opacity = 1)
    {
        return new VisualParameters(new SolidColorBrush(fill), new SolidColorBrush(stroke), strokeThickness, opacity);
    }


    public static VisualParameters GetDefaultForDrawing(DrawMode mode)
    {
        var result = new VisualParameters(mode == DrawMode.Polygon ? DefaultDrawingFill : null, DefaultDrawingStroke, 2, .7);

        return result;
    }

    public static VisualParameters GetDefaultForSelection()
    {
        return new VisualParameters(DefaultSelectionFill, DefaultSelectionStroke, 2, 0.9);
    }

    public static VisualParameters GetDefaultForRoutingPoints()
    {
        return new VisualParameters(DefaultRoutingPointFill, DefaultRoutingPointStroke, 2, 0.9);
    }

    public static VisualParameters GetDefaultForRoutingPolylineThin()
    {
        return new VisualParameters(DefaultRoutingLineStrokeThin, DefaultRoutingLineStrokeThin, 4, 1);
    }

    public static VisualParameters GetDefaultForRoutingPolylineThick()
    {
        return new VisualParameters(DefaultRoutingLineThick, DefaultRoutingLineThick, 6, 1);
    }

    public static VisualParameters GetDefaultForHighlight()
    {
        return new VisualParameters(DefaultHighlightFill, DefaultHighlightStroke, 2, .8);
    }


    public static VisualParameters GetDefaultForHighlight(IGeometryAware<Point> sqlGeometryAware)
    {
        VisualParameters result;

        if (sqlGeometryAware?.TheGeometry?.IsPointOrMultiPoint() == true)
        {
            result = new VisualParameters(DefaultHighlightStroke, DefaultHighlightFill, 3, .9) { PointSymbol = new SimplePointSymbolizer(10) };
        }
        else
        {
            result = GetDefaultForHighlight();
        }

        return result;

    }


    public static SolidColorBrush DefaultHighlightStroke = new SolidColorBrush(Colors.Yellow);

    public static SolidColorBrush DefaultHighlightFill = new SolidColorBrush(new Color() { B = 0, G = 255, R = 255, A = 50 });


    public static SolidColorBrush DefaultSelectionStroke = new SolidColorBrush(Colors.Cyan);

    public static SolidColorBrush DefaultSelectionFill = new SolidColorBrush(new Color() { B = 255, G = 255, R = 0, A = 160 });


    public static SolidColorBrush DefaultDrawingStroke = new SolidColorBrush(new Color() { R = 255, G = 200, B = 0, A = 250 });

    public static SolidColorBrush DefaultDrawingFill = new SolidColorBrush(new Color() { R = 255, G = 200, B = 0, A = 160 });


    public static SolidColorBrush DefaultRoutingPointFill = new SolidColorBrush(new Color() { R = 255, G = 228, B = 225, A = 255 });

    public static SolidColorBrush DefaultRoutingPointStroke = new SolidColorBrush(new Color() { R = 220, G = 20, B = 60, A = 255 });


    public static SolidColorBrush DefaultRoutingLineStrokeThin = new SolidColorBrush(new Color() { R = 32, G = 108, B = 213, A = 255 });//#256FD7
    
    public static SolidColorBrush DefaultRoutingLineThick = new SolidColorBrush(new Color() { R = 102, G = 157, B = 246, A = 255 });//#256FD7

    public static DashStyle GetDefaultDashStyleForMeasurements()
    {
        return new DashStyle(new double[] { 2, 1 }, 0);
    }

    public static VisualParameters GetDefaultForMeasurements()
    {

        return new VisualParameters(
            BrushHelper.CreateBrush(ColorHelper.ToWpfColor("#FBB03B"), 0.3),
            BrushHelper.CreateBrush("#FBB03B"),
            3,
            1,
            System.Windows.Visibility.Visible)
        {
            DashStyle = VisualParameters.GetDefaultDashStyleForMeasurements()
        };
    }

    public static VisualParameters GetDefaultForDrawingItems()
    {
        return new VisualParameters(null, BrushHelper.PickGoodBrush(), 2, 1);
    }

}
