using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using IRI.Maptor.Extensions;
using IRI.Maptor.Jab.Common.Events;
using IRI.Maptor.Jab.Common.Helpers; 
using IRI.Maptor.Sta.Spatial.Primitives;
using IRI.Maptor.Jab.Common.Cartography.Symbologies;

namespace IRI.Maptor.Jab.Common;

public partial class VisualParameters : DependencyObject, INotifyPropertyChanged
{
    private event EventHandler<CustomEventArgs<Visibility>> _onVisibilityChanged;

    public event EventHandler<CustomEventArgs<Visibility>> OnVisibilityChanged
    {
        remove { this._onVisibilityChanged -= value; }
        add
        {
            if (this._onVisibilityChanged == null)
            {
                this._onVisibilityChanged += value;
            }
        }
    }

    public event EventHandler<CustomEventArgs<VisualParameters>> OnChanged;

    // ************************************* Fill *********************************************
    public Brush Fill
    {
        get { return (Brush)GetValue(FillProperty); }
        set { SetValue(FillProperty, value); }
    }

    public static readonly DependencyProperty FillProperty =
        DependencyProperty.Register(nameof(Fill), typeof(Brush), typeof(VisualParameters), new PropertyMetadata());


    // ************************************* Stroke ********************************************
    public Brush Stroke
    {
        get { return (Brush)GetValue(StrokeProperty); }
        set { SetValue(StrokeProperty, value); }
    }

    public static readonly DependencyProperty StrokeProperty =
        DependencyProperty.Register(nameof(Stroke), typeof(Brush), typeof(VisualParameters), new PropertyMetadata(null));


    // ************************************* Opacity********************************************
    public double Opacity
    {
        get { return (double)GetValue(OpacityProperty); }
        set { SetValue(OpacityProperty, value); }
    }

    public static readonly DependencyProperty OpacityProperty =
        DependencyProperty.Register(nameof(Opacity), typeof(double), typeof(VisualParameters));


    // ************************************* StrokeThickness ********************************************
    public double StrokeThickness
    {
        get { return (double)GetValue(StrokeThicknessProperty); }
        set { SetValue(StrokeThicknessProperty, value); }
    }

    public static readonly DependencyProperty StrokeThicknessProperty =
        DependencyProperty.Register(nameof(StrokeThickness), typeof(double), typeof(VisualParameters));


    // ************************************* Visibility *********************************************
    public Visibility Visibility
    {
        get { return (Visibility)GetValue(VisibilityProperty); }
        set { SetValue(VisibilityProperty, value); }
    }

    public static readonly DependencyProperty VisibilityProperty =
        DependencyProperty.Register(nameof(Visibility),
                                    typeof(Visibility),
                                    typeof(VisualParameters),
                                    new PropertyMetadata(Visibility.Visible, new PropertyChangedCallback((dp, dpE) =>
                                    {
                                        var obj = dp as VisualParameters;

                                        var newVisibility = (Visibility)dpE.NewValue;

                                        var oldVisibility = (Visibility)dpE.OldValue;

                                        if (newVisibility == oldVisibility) { }
                                        else
                                        {
                                            obj._onVisibilityChanged.SafeInvoke(obj, new CustomEventArgs<Visibility>(newVisibility));
                                        }
                                    })));


    // ************************************* Order **********************************************
    public int Order
    {
        get { return (int)GetValue(OrderProperty); }
        set { SetValue(OrderProperty, value); }
    }

    // Using a DependencyProperty as the backing store for Order.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty OrderProperty =
        DependencyProperty.Register(nameof(Order), typeof(int), typeof(VisualParameters));


    //// ************************************* DashType *********************************************
    //public DoubleCollection DashType
    //{
    //    get { return (DoubleCollection)GetValue(DashTypeProperty); }
    //    set { SetValue(DashTypeProperty, value); }
    //}

    //public static readonly DependencyProperty DashTypeProperty =
    //    DependencyProperty.Register(nameof(DashType), typeof(DoubleCollection), typeof(VisualParameters));


    // ************************************* DashStyle *********************************************
    public DashStyle DashStyle
    {
        get { return (DashStyle)GetValue(DashStyleProperty); }
        set { SetValue(DashStyleProperty, value); }
    }

    public static readonly DependencyProperty DashStyleProperty =
        DependencyProperty.Register(nameof(DashStyle), typeof(DashStyle), typeof(VisualParameters), new PropertyMetadata(null));


    public PenLineCap PenLineCap { get; set; }
    public PenLineJoin PenLineJoin { get; set; }

    // ************************************* IsInScaleRange *******************************************
    public bool IsInScaleRange
    {
        get { return (bool)GetValue(IsInScaleRangeProperty); }
        set { SetValue(IsInScaleRangeProperty, value); }
    }

    public static readonly DependencyProperty IsInScaleRangeProperty =
        DependencyProperty.Register(nameof(IsInScaleRange), typeof(bool), typeof(VisualParameters), new PropertyMetadata(true, new PropertyChangedCallback((dp, dpE) =>
        {
            var obj = dp as VisualParameters;

            var newIsInScaleRange = ((bool)dpE.NewValue);

            var oldIsInScaleRange = ((bool)dpE.OldValue);

            if (newIsInScaleRange != oldIsInScaleRange)
            {
                obj.OnChanged?.Invoke(obj, new CustomEventArgs<VisualParameters>(obj));
            }
        })));




    // ************************************* PointSymbol ********************************************
    private SimplePointSymbolizer _pointSymbol = new SimplePointSymbolizer() { SymbolWidth = 4, SymbolHeight = 4 };

    public SimplePointSymbolizer PointSymbol
    {
        get { return _pointSymbol; }
        set
        {
            _pointSymbol = value;
            RaisePropertyChanged();
        }
    }


    private VisualParameters(double opacity) : this(BrushHelper.PickBrush(), BrushHelper.PickBrush(), 1, opacity)
    {
    }

    public VisualParameters(Brush fill, Brush stroke, double strokeThickness, double opacity, Visibility visibility = Visibility.Visible)
    {
        this.Fill = fill;

        this.Stroke = stroke;

        this.StrokeThickness = strokeThickness;

        this.Opacity = opacity;

        this.Visibility = visibility;

        //this.DashType = dashType;
    }

    public VisualParameters(Color fill, Color? stroke = null, double strokeThickness = 1, double opacity = 1)
        : this(new SolidColorBrush(fill), stroke.HasValue ? new SolidColorBrush(stroke.Value) : null, strokeThickness, opacity)
    {

    }

    public VisualParameters(string hexFill, string hexStroke, double strokeThickness = 1, double opacity = 1) :
        this(BrushHelper.CreateBrush(hexFill), BrushHelper.CreateBrush(hexStroke), strokeThickness, opacity)
    {

    }


    public VisualParameters Clone()
    {
        return new VisualParameters(Fill, Stroke, StrokeThickness, Opacity, Visibility);
    }

    public Pen? GetWpfPen()
    {
        var result = Stroke != null ? new Pen(Stroke, StrokeThickness) : null;

        if (result != null && DashStyle != null)
            result.DashStyle = DashStyle;

        if (result != null)
            result.LineJoin = PenLineJoin.Round;

        return result;
    }

    public System.Drawing.Pen? GetGdiPlusPen(double? opacity = null)
    {
        var result = Stroke != null ? new System.Drawing.Pen(Stroke.AsGdiBrush(opacity), (int)StrokeThickness) : null;

        if (result != null && DashStyle != null)
            result.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;

        if (result != null)
            result.LineJoin = System.Drawing.Drawing2D.LineJoin.Round;

        return result;
    }

    public System.Drawing.Brush GetGdiPlusFillBrush(double? opacity = null)
    {
        return Fill.AsGdiBrush(opacity);
    }


    /// <summary>
    /// Returns a random VisualParameters
    /// </summary>
    /// <param name="opacity"></param>
    /// <returns></returns>
    public static VisualParameters CreateNew(double opacity = 1)
    {
        return new VisualParameters(opacity);
    }

    public static VisualParameters CreateNew(double opacity, double strokeThickness = 1, bool withoutFill = false)
    {
        Brush fill = null;

        if (!withoutFill)
        {
            fill = BrushHelper.PickGoodBrush();
        }

        return new VisualParameters(fill, BrushHelper.PickBrush(), strokeThickness, opacity);
    }


    #region INotifyPropertyChanged

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void RaisePropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    #endregion


    #region Builders

    public void Build(Sta.Ogc.SLD.Stroke stroke)
    {
        var strokeValue = stroke.StrokeValue;
        var strokeThickness = stroke.StrokeThicknessValue;
        var strokeOpacity = stroke.StrokeOpacityValue;
        var strokeLineJoin = stroke.StrokeLineJoinValue;
        var strokeLineCap = stroke.StrokeLineCapValue;
        var strokeDashArray = stroke.StrokeDashArrayValue;
        var strokeDashOffset = stroke.StrokeDashOffsetValue;

        this.Stroke = BrushHelper.CreateBrush(strokeValue, strokeOpacity);
        this.StrokeThickness = strokeThickness;

        this.PenLineJoin = strokeLineJoin.Parse();
        this.PenLineCap = strokeLineCap.Parse();

        if (strokeDashArray is not null)
            this.DashStyle = new System.Windows.Media.DashStyle(strokeDashArray.ToList(), strokeDashOffset);

        return;
    }

    public void Build(Sta.Ogc.SLD.Fill fill)
    {

        var fillValue = fill.FillValue;
        var fillOpacity = fill.FillOpacityValue;

        this.Fill = BrushHelper.CreateBrush(fillValue, fillOpacity);

    }

    #endregion

    #region Static 

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

    public static VisualParameters Get(string? hexFill, string? hexStroke, double strokeThickness, double fillOpacity = 1, double strokeOpacity = 1)
    {
        var fill = ColorHelper.ToWpfColor(hexFill, fillOpacity);

        var stroke = ColorHelper.ToWpfColor(hexStroke, strokeOpacity);

        return new VisualParameters(new SolidColorBrush(fill), new SolidColorBrush(stroke), strokeThickness, 1);
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


    public static VisualParameters GetDefaultForHighlight(Feature<IRI.Maptor.Sta.Common.Primitives.Point> sqlGeometryAware)
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

    #endregion
}
