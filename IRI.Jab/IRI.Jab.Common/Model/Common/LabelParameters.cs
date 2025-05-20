using System;
using System.Windows.Media;
using IRI.Sta.Spatial.Primitives;
using IRI.Sta.Common.Primitives;
using IRI.Jab.Common.Model;

namespace IRI.Jab.Common;

public class LabelParameters : Notifier
{
    public event EventHandler<CustomEventArgs<LabelParameters>> OnIsInScaleRangeChanged;

    public event EventHandler<CustomEventArgs<LabelParameters>> OnIsOnChanged;


    private ScaleInterval _visibleRange;
    public ScaleInterval VisibleRange
    {
        get { return _visibleRange; }
        set
        {
            _visibleRange = value;
            RaisePropertyChanged();
        }
    }


    private int _fontSize;
    public int FontSize
    {
        get { return _fontSize; }
        set
        {
            _fontSize = value;
            RaisePropertyChanged();
        }
    }


    private Brush _foreground;
    public Brush Foreground
    {
        get { return _foreground; }
        set
        {
            _foreground = value;
            RaisePropertyChanged();
        }
    }


    private FontFamily _fontFamily;
    public FontFamily FontFamily
    {
        get { return _fontFamily; }
        set
        {
            _fontFamily = value;
            RaisePropertyChanged();
        }
    }


    private bool _isInScaleRange = true;
    public bool IsInScaleRange
    {
        get
        {
            return _isInScaleRange;
        }
        set
        {
            if (_isInScaleRange == value)
            {
                return;
            }

            _isInScaleRange = value;
            RaisePropertyChanged();
            this.OnIsInScaleRangeChanged?.Invoke(this, new CustomEventArgs<LabelParameters>(this));
        }
    }


    private bool _isRtl = true;
    public bool IsRtl
    {
        get { return _isRtl; }
        set
        {
            _isRtl = value;
            RaisePropertyChanged();
        }
    }


    private bool _isOn;
    public bool IsOn
    {
        get { return _isOn; }
        set
        {
            _isOn = value;
            RaisePropertyChanged();
            this.OnIsOnChanged?.Invoke(this, new CustomEventArgs<LabelParameters>(this));
        }
    }


    public bool IsLabeled(double inverseMapScale)
    {
        return VisibleRange.IsInRange(inverseMapScale) && this.IsOn;
    }

    public Func<Geometry<Point>, Geometry<Point>> PositionFunc { get; set; }

    public LabelParameters(ScaleInterval visibleRange, int fontSize, Brush foreground, FontFamily fontFamily, Func<Geometry<Point>, Geometry<Point>> positionFunc)
    {
        this.VisibleRange = visibleRange;

        this.FontSize = fontSize;

        this.Foreground = foreground ?? Brushes.Black;

        this.FontFamily = fontFamily;

        this.PositionFunc = positionFunc;
    }

    public LabelParameters(ScaleInterval visibleRange, int fontSize, Color foreground, FontFamily fontFamily, Func<Geometry<Point>, Geometry<Point>> positionFunc)
        : this(visibleRange, fontSize, new SolidColorBrush(foreground), fontFamily, positionFunc)
    {

    }
}
