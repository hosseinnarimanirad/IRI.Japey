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

namespace IRI.Jab.Common.View.MapMarkers
{
    /// <summary>
    /// Interaction logic for ImageSymbolMarker.xaml
    /// </summary>
    public partial class ImageMarker : UserControl
    {
        public ImageMarker(ImageSource symbol, double width = 16, double height = 16)
        {
            InitializeComponent();

            this.image.Source = symbol;

            this.root.Width = width;

            this.root.Height = height;

            //this.viewbox.Width = width;

            //this.viewbox.Height = height;
        }
    }
}
