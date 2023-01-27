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

namespace IRI.Jab.Common.View.MapMarkers
{
    /// <summary>
    /// Interaction logic for MapMarker.xaml
    /// </summary>
    public partial class MapMarker : UserControl, IMapMarker
    {
        public MapMarker(int number)
        {
            InitializeComponent();

            this.Value = number.ToString();

            //this.numberBox.Text = number.ToString();
        }

        public MapMarker(string content)
        {
            InitializeComponent();

            this.Value = content;

            //this.numberBox.Text = content;
        }



        public string Value
        {
            get { return (string)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Value.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(nameof(Value), typeof(string), typeof(MapMarker), new PropertyMetadata(string.Empty));


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
