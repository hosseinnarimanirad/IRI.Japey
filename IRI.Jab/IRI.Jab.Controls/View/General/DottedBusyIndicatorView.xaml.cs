﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace IRI.Jab.Controls.View;

/// <summary>
/// Interaction logic for DottedBusyIndicator.xaml
/// </summary>
public partial class DottedBusyIndicatorView : UserControl
{
    public DottedBusyIndicatorView()
    {
        InitializeComponent();
    }
     
    public Brush DotBrush
    {
        get { return (Brush)GetValue(DotBrushProperty); }
        set { SetValue(DotBrushProperty, value); }
    }

    // Using a DependencyProperty as the backing store for DotBrush.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty DotBrushProperty =
        DependencyProperty.Register(nameof(DotBrush), typeof(Brush), typeof(DottedBusyIndicatorView), new PropertyMetadata(Brushes.White));




    public int DotSize
    {
        get { return (int)GetValue(DotSizeProperty); }
        set { SetValue(DotSizeProperty, value); }
    }

    // Using a DependencyProperty as the backing store for DotSize.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty DotSizeProperty =
        DependencyProperty.Register(nameof(DotSize), typeof(int), typeof(DottedBusyIndicatorView), new PropertyMetadata(8));
}
