using IRI.Jab.Common.Model.MapMarkers;
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
}
