using IRI.Jab.Cartography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using IRI.Jab.Common.Model;

namespace IRI.Jab.LegendControl
{
    /// <summary>
    /// Interaction logic for Legend.xaml
    /// </summary>
    public partial class Legend : UserControl
    {
        public Legend()
        {
            InitializeComponent();
        }

        private void CollectionViewSource_Filter(object sender, FilterEventArgs e)
        {
            var item = e.Item as ILayer;

            e.Accepted =
                (item.Type.HasFlag(LayerType.VectorLayer) ||
                item.Type.HasFlag(LayerType.Raster)) &&
                !item.Type.HasFlag(LayerType.Drawing) &&
                !item.Type.HasFlag(LayerType.EditableItem);
        }
    }
}
