using System.Windows.Media;
using System.Windows.Controls;
using IRI.Maptor.Jab.Common.Abstractions;

namespace IRI.Maptor.Jab.Common.View.MapMarkers;

/// <summary>
/// Interaction logic for CircleImageMarker.xaml
/// </summary>
public partial class CircleImageMarker : UserControl, IMapMarker
{
    public CircleImageMarker()
    {
        InitializeComponent();
    }

    public CircleImageMarker(ImageSource image, string tooltip = null)
    {
        InitializeComponent();

        this.image.Source = image;

        if (tooltip != null)
        {
            this.image.ToolTip = tooltip;
        }
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
