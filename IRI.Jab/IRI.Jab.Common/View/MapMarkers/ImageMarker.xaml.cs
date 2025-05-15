﻿using System.Windows.Media;
using System.Windows.Controls;
using IRI.Jab.Common.Abstractions;

namespace IRI.Jab.Common.View.MapMarkers;

/// <summary>
/// Interaction logic for ImageSymbolMarker.xaml
/// </summary>
public partial class ImageMarker : UserControl, IMapMarker
{
    public ImageMarker(ImageSource symbol, double width = 16, double height = 16)
    {
        InitializeComponent();

        this.image.Source = symbol;

        this.root.Width = width;

        this.root.Height = height;

        //this.viewbox.Width = width;

        //this.viewbox.Height = height;
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
