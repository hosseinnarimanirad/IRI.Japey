using System.Windows;
using System.ComponentModel;
using System.Windows.Controls;
using System.Runtime.CompilerServices;

namespace IRI.Maptor.Jab.Controls.View;

/// <summary>
/// Interaction logic for MapDrawingLegendView.xaml
/// </summary>
public partial class MapDrawingLegendView : UserControl, INotifyPropertyChanged
{
    public MapDrawingLegendView()
    {
        InitializeComponent();
    }


    public string GroupName
    {
        get { return (string)GetValue(GroupNameProperty); }
        set
        {
            SetValue(GroupNameProperty, value);
            RaisePropertyChanged(nameof(ShowTools));
        }
    }

    // Using a DependencyProperty as the backing store for GroupName.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty GroupNameProperty =
        DependencyProperty.Register(nameof(GroupName), typeof(string), typeof(MapDrawingLegendView), new PropertyMetadata("D"));


    public double TitleFontSize
    {
        get { return (double)GetValue(TitleFontSizeProperty); }
        set { SetValue(TitleFontSizeProperty, value); }
    }

    // Using a DependencyProperty as the backing store for FontSize.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty TitleFontSizeProperty =
        DependencyProperty.Register(nameof(TitleFontSize), typeof(double), typeof(MapDrawingLegendView), new PropertyMetadata(13.0));


    public bool ShowTools
    {
        get { return (bool)GetValue(ShowToolsProperty); }
        set
        {
            SetValue(ShowToolsProperty, value);
            RaisePropertyChanged(nameof(ShowTools));
        }
    }

    // Using a DependencyProperty as the backing store for ShowTools.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ShowToolsProperty =
        DependencyProperty.Register(nameof(ShowTools), typeof(bool), typeof(MapDrawingLegendView), new PropertyMetadata(true));

    public event PropertyChangedEventHandler PropertyChanged;

    protected void RaisePropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
