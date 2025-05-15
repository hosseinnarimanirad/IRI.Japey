using System.Windows;
using System.Windows.Controls;
using IRI.Jab.Common.Abstractions;

namespace IRI.Jab.Common.View.MapMarkers;
/// <summary>
/// Interaction logic for TextboxMarker.xaml
/// </summary>
public partial class TextboxMarker : UserControl, IMapMarker
{
    public TextboxMarker()
    {
        InitializeComponent();
    }

    public string LabelValue
    {
        get { return (string)GetValue(LabelValueProperty); }
        set { SetValue(LabelValueProperty, value); }
    }

    // Using a DependencyProperty as the backing store for LabelValue.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty LabelValueProperty =
        DependencyProperty.Register(nameof(LabelValue), typeof(string), typeof(TextboxMarker), new PropertyMetadata(string.Empty));




    public string TooltipValue
    {
        get { return (string)GetValue(TooltipValueProperty); }
        set { SetValue(TooltipValueProperty, value); }
    }

    // Using a DependencyProperty as the backing store for TooltipValue.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty TooltipValueProperty =
        DependencyProperty.Register("TooltipValue", typeof(string), typeof(TextboxMarker), new PropertyMetadata(string.Empty));


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
