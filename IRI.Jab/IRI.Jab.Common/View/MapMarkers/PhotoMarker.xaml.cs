using System.Windows.Media;
using System.Windows.Controls;
using IRI.Jab.Common.Abstractions;

namespace IRI.Jab.Common.View.MapMarkers;

/// <summary>
/// Interaction logic for ImageMarker.xaml
/// </summary>
public partial class PhotoMarker : UserControl, IMapMarker
{
    public PhotoMarker()
    {
        InitializeComponent();
    }

    public PhotoMarker(ImageSource imageSource)
    {
        InitializeComponent();

        this.image.Source = imageSource;
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
