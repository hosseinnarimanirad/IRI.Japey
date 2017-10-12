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

namespace IRI.Jab.LegendControl
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class MapLegend : UserControl
    {
        //public delegate void layerOpacityValueChangedEventHandler(object sender, RoutedPropertyChangedEventArgs<double> e);

        public event IRI.Jab.LegendControl.MapLegendItem.layerOpacityValueChangedEventHandler LayerOpacityValueChanged;

        //public delegate void layerVisibilityChangedEventHandler(object sender, RoutedEventArgs e);

        public event IRI.Jab.LegendControl.MapLegendItem.layerVisibilityChangedEventHandler LayerVisibilityChanged;

        //public delegate void layerStyleChangedEventHandler(object sender, MouseButtonEventArgs e);

        public event IRI.Jab.LegendControl.MapLegendItem.layerStyleChangedEventHandler LayerStyleChanged;

        public MapLegend()
        {
            InitializeComponent();
        }

        public void AddLayer(string layerName, VisualParameters parameters, LayerType type)
        {
            MapLegendItem item = new MapLegendItem(layerName, parameters, type);

            item.LayerOpacityValueChanged += new MapLegendItem.layerOpacityValueChangedEventHandler(item_LayerOpacityValueChanged);

            item.LayerVisibilityChanged += new MapLegendItem.layerVisibilityChangedEventHandler(item_LayerVisibilityChanged);

            item.LayerStyleChanged += new MapLegendItem.layerStyleChangedEventHandler(item_LayerStyleChanged);

            this.mapItems.Items.Add(item);
        }

        void item_LayerStyleChanged(object sender, MouseButtonEventArgs e)
        {
            if (this.LayerStyleChanged != null)
            {
                this.LayerStyleChanged(sender, e);
            }
        }

        void item_LayerVisibilityChanged(object sender, RoutedEventArgs e)
        {
            if (this.LayerVisibilityChanged != null)
            {
                this.LayerVisibilityChanged(sender, e);
            }
        }

        void item_LayerOpacityValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (this.LayerOpacityValueChanged != null)
            {
                this.LayerOpacityValueChanged(sender, e);
            }
        }

        public void RemoveLayer(string layerName)
        {
            for (int i = 0; i < this.mapItems.Items.Count; i++)
            {
                if (((MapLegendItem)this.mapItems.Items[i]).layerName.Text.Equals(layerName))
                {
                    this.mapItems.Items.Remove(this.mapItems.Items[i]);

                    break;
                }
            }
        }

    }
}
