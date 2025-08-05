using System.Windows.Data;
using System.Windows.Controls;

using IRI.Jab.Common;
using IRI.Jab.Common.Model;

namespace IRI.Jab.Controls.View;

/// <summary>
/// Interaction logic for UserControl1.xaml
/// </summary>
public partial class MapLegendView : UserControl
{
    public MapLegendView()
    {
        InitializeComponent();
    }

    private void CollectionViewSource_Filter(object sender, FilterEventArgs e)
    {
        var item = e.Item as ILayer;

        e.Accepted =
            item.ShowInToc && (
            item.Type.HasFlag(LayerType.VectorLayer) ||
            item.Type.HasFlag(LayerType.Raster) ||
            item.Type.HasFlag(LayerType.ImagePyramid));
    }

}
