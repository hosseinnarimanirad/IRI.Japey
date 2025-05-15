﻿using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Input;
using IRI.Jab.Common.Abstractions;
using IRI.Sta.Common.Primitives;
using IRI.Sta.SpatialReferenceSystem;

namespace IRI.Jab.Common.View.MapMarkers;

/// <summary>
/// Interaction logic for ShapeWithLabelMarker.xaml
/// </summary>
public partial class CoordinateMarker : UserControl, INotifyPropertyChanged, IMapMarker
{
    //public double X
    //{
    //    get { return (double)GetValue(XProperty); }
    //    set
    //    {
    //        SetValue(XProperty, value);
    //        UpdateCoordinates();
    //    }
    //}

    //// Using a DependencyProperty as the backing store for X.  This enables animation, styling, binding, etc...
    //public static readonly DependencyProperty XProperty =
    //    DependencyProperty.Register(nameof(X), typeof(double), typeof(CoordinateMarker), new PropertyMetadata(0.0));


    //public double Y
    //{
    //    get { return (double)GetValue(YProperty); }
    //    set
    //    {
    //        SetValue(YProperty, value);
    //        UpdateCoordinates();
    //    }
    //}

    //// Using a DependencyProperty as the backing store for Y.  This enables animation, styling, binding, etc...
    //public static readonly DependencyProperty YProperty =
    //    DependencyProperty.Register(nameof(Y), typeof(double), typeof(CoordinateMarker), new PropertyMetadata(0.0));


    public bool ChangeToDms { get; }

    private string _xLabel;

    public string XLabel
    {
        get { return _xLabel; }
        set
        {
            _xLabel = value;
            RaisePropertyChanged();
        }
    }

    private string _yLabel;

    public string YLabel
    {
        get { return _yLabel; }
        set
        {
            _yLabel = value;
            RaisePropertyChanged();
        }
    }



    private coordinates _current;

    private Point _mercatorLocation;

    public Point MercatorLocation
    {
        get { return _mercatorLocation; }
        set
        {
            _mercatorLocation = value;
            RaisePropertyChanged();
            UpdateCoordinates();
        }
    }


    public CoordinateMarker(double mercatorX, double mercatorY, bool changeToDms = false)
    {
        InitializeComponent();

        this._current = coordinates.Geodetic;

        this.ChangeToDms = changeToDms;

        this.MercatorLocation = new Point(mercatorX, mercatorY);

        //this.X = mercatorX;

        //this.Y = mercatorY;

    }

    private void changeCoordinate(object sender, MouseButtonEventArgs e)
    {
        _current = (coordinates)((int)(_current + 1) % 3);

        UpdateCoordinates();
    }

    public void UpdateCoordinates()
    {
        var value = MapProjects.WebMercatorToGeodeticWgs84(MercatorLocation);

        if (_current == coordinates.Utm)
        {
            value = MapProjects.GeodeticToUTM(value);
        }

        if (_current == coordinates.GeodeticDms)
        {
            XLabel = IRI.Sta.Common.Helpers.DegreeHelper.ToDms(value.X, true); YLabel = IRI.Sta.Common.Helpers.DegreeHelper.ToDms(value.Y, true);
        }
        else
        {
            var decimals = 2;

            if (_current == coordinates.Geodetic)
                decimals = 5;

            XLabel = value.X.ToString($"N{decimals}"); YLabel = value.Y.ToString($"N{decimals}");
        }

    }

    enum coordinates
    {
        Utm = 0,
        Geodetic = 1,
        GeodeticDms = 2
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void RaisePropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private bool _isSelected;

    public bool IsSelected
    {
        get { return _isSelected; }
        set
        {
            _isSelected = value;
        }
    }
}
