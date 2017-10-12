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
    /// Interaction logic for CountableShapeMarker.xaml
    /// </summary>
    public partial class CountableShapeMarker : UserControl
    {
        public CountableShapeMarker(Geometry shape, string count)
        {
            InitializeComponent();

            //this.image.Data = Geometry.Parse(shape);
            this.image.Data = shape;

            this.labelBox.Text = count;
        }
    }
}

