﻿using IRI.Maptor.Jab.Common;
using IRI.Maptor.Jab.Common.Models.Legend;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace IRI.Maptor.Jab.Controls.View;

/// <summary>
/// Interaction logic for MapLegendItemWithOptions.xaml
/// </summary>
public partial class MapLegendItemWithOptionsView : UserControl
{
    public MapLegendItemWithOptionsView()
    {
        InitializeComponent();
    }

    #region DependencyProperties

    public string Title
    {
        get { return (string)GetValue(TitleProperty); }
        set { SetValue(TitleProperty, value); }
    }

    // Using a DependencyProperty as the backing store for LayerName.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty TitleProperty =
        DependencyProperty.Register(nameof(Title), typeof(string), typeof(MapLegendItemWithOptionsView), new PropertyMetadata(new PropertyChangedCallback((d, dp) =>
        {
            try
            {
                ((MapLegendItemWithOptionsView)d).UpdateTitle((string)dp.NewValue);
            }
            catch (Exception ex)
            {
                return;
            }
        })));

    public double TitleFontSize
    {
        get { return (double)GetValue(TitleFontSizeProperty); }
        set { SetValue(TitleFontSizeProperty, value); }
    }

    // Using a DependencyProperty as the backing store for FontSize.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty TitleFontSizeProperty =
        DependencyProperty.Register(nameof(TitleFontSize), typeof(double), typeof(MapLegendItemWithOptionsView), new PropertyMetadata(13.0));


    public bool IsEditable
    {
        get { return (bool)GetValue(IsEditableProperty); }
        set { SetValue(IsEditableProperty, value); }
    }

    // Using a DependencyProperty as the backing store for IsEditable.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty IsEditableProperty =
        DependencyProperty.Register(nameof(IsEditable), typeof(bool), typeof(MapLegendItemWithOptionsView), new PropertyMetadata(false));


    public VisualParameters Symbology
    {
        get { return (VisualParameters)GetValue(SymbologyProperty); }
        set { SetValue(SymbologyProperty, value); }
    }

    // Using a DependencyProperty as the backing store for Symbology.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty SymbologyProperty =
        DependencyProperty.Register(nameof(Symbology), typeof(VisualParameters), typeof(MapLegendItemWithOptionsView), new PropertyMetadata(null));



    public bool IsChecked
    {
        get { return (bool)GetValue(IsCheckedProperty); }
        set { SetValue(IsCheckedProperty, value); }
    }

    // Using a DependencyProperty as the backing store for IsChecked.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty IsCheckedProperty =
        DependencyProperty.Register(nameof(IsChecked), typeof(bool), typeof(MapLegendItemWithOptionsView), new PropertyMetadata(false));



    public IEnumerable<ILegendCommand> Commands
    {
        get { return (IEnumerable<ILegendCommand>)GetValue(CommandsProperty); }
        set { SetValue(CommandsProperty, value); }
    }

    // Using a DependencyProperty as the backing store for Commands.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty CommandsProperty =
        DependencyProperty.Register(nameof(Commands), typeof(IEnumerable<ILegendCommand>), typeof(MapLegendItemWithOptionsView), new PropertyMetadata(null));

    #endregion





    private void UpdateTitle(string newValue)
    {
        var layer = (this.DataContext as DrawingItemLayer);

        if (layer != null)
        {
            layer.LayerName = newValue;
        } 
    }


}
