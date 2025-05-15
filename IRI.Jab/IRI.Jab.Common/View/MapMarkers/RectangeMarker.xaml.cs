using System.Windows.Controls;
using IRI.Jab.Common.Abstractions;

namespace IRI.Jab.Common.View.MapMarkers;

/// <summary>
/// Interaction logic for RectangeMarker.xaml
/// </summary>
public partial class RectangeMarker : UserControl, IMapMarker
{
    public RectangeMarker()
    {
        InitializeComponent();
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
