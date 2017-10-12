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
    /// Interaction logic for ImageMarker.xaml
    /// </summary>
    public partial class PhotoMarker : UserControl
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
    }
}
