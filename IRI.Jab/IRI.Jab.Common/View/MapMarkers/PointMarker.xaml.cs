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
using IRI.Jab.Common.Model.MapMarkers;

namespace IRI.Jab.Common
{
    /// <summary>
    /// Interaction logic for PointMarker.xaml
    /// </summary>
    public partial class PointMarker : UserControl, IMapMarker
    {
        public PointMarker(string label)
        {
            InitializeComponent();

            this.labelBox.Text = label;
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
