using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using IRI.Jab.Cartography;
using IRI.Jab.Common;
using IRI.Jab.Common.Model;

namespace IRI.Jab.Controls.View.Map
{
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
}
