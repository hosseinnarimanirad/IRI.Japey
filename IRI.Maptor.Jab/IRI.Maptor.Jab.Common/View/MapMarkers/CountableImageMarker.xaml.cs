using System.Windows.Media;
using System.Windows.Controls;
using IRI.Maptor.Jab.Common.Abstractions;

namespace IRI.Maptor.Jab.Common.View.MapMarkers;

/// <summary>
/// Interaction logic for ImageSymbolWithCountMarker.xaml
/// </summary>
public partial class CountableImageMarker : UserControl, IMapMarker
{
    public CountableImageMarker(ImageSource imageSource, string count)
    {
        InitializeComponent();

        this.image.Source = imageSource;

        this.labelBox.Text = count;
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
