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

namespace IRI.Jab.Common.View.MapMarkers
{
    /// <summary>
    /// Interaction logic for MapMarker.xaml
    /// </summary>
    public partial class MapMarker : UserControl
    {
        public MapMarker(int number)
        {
            InitializeComponent();

            this.numberBox.Text = number.ToString();
        }

        public MapMarker(string content)
        {
            InitializeComponent();

            this.numberBox.Text = content;
        }
    }
}
