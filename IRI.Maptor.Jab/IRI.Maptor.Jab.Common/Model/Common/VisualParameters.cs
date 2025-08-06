using System;
using System.Windows;
using System.Windows.Media;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using IRI.Extensions;
using IRI.Maptor.Jab.Common.Helpers;
using IRI.Maptor.Jab.Common.Cartography.Symbologies.Strategies;
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


    // ************************************* DashType *********************************************
    public DoubleCollection DashType
    {
        get { return (DoubleCollection)GetValue(DashTypeProperty); }
        set { SetValue(DashTypeProperty, value); }
    }

    // Using a DependencyProperty as the backing store for DashType.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty DashTypeProperty =
        DependencyProperty.Register(nameof(DashType), typeof(DoubleCollection), typeof(VisualParameters));


    // ************************************* DashStyle *********************************************
    public DashStyle DashStyle
    {
        get { return (DashStyle)GetValue(DashStyleProperty); }
        set { SetValue(DashStyleProperty, value); }
    }

    // Using a DependencyProperty as the backing store for DashStyle.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty DashStyleProperty =
        DependencyProperty.Register(nameof(DashStyle), typeof(DashStyle), typeof(VisualParameters), new PropertyMetadata(null));




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

    //public VisualParameters(Color fill, Color? stroke, double strokeThickness, double opacity, Visibility visibility = Visibility.Visible)
    //    : this(new SolidColorBrush(fill), stroke.HasValue ? new SolidColorBrush(stroke.Value) : null, strokeThickness, opacity, visibility)
    //{

    //}

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

    public static VisualParameters CreateNew(double opacity = 1, double strokeThickness = 1, bool withoutFill = false)
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
}
