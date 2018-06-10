using IRI.Jab.Cartography;
using IRI.Jab.Controls.Model.Legend;
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

namespace IRI.Jab.Controls.View.Map
{
    /// <summary>
    /// Interaction logic for MapLegendItemWithOptions.xaml
    /// </summary>
    public partial class MapLegendItemWithOptions : UserControl
    {
        public MapLegendItemWithOptions()
        {
            InitializeComponent();
        }



        public string LayerName
        {
            get { return (string)GetValue(LayerNameProperty); }
            set { SetValue(LayerNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LayerName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LayerNameProperty =
            DependencyProperty.Register(nameof(LayerName), typeof(string), typeof(MapLegendItemWithOptions), new PropertyMetadata(string.Empty));


        public VisualParameters Symbology
        {
            get { return (VisualParameters)GetValue(SymbologyProperty); }
            set { SetValue(SymbologyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Symbology.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SymbologyProperty =
            DependencyProperty.Register("Symbology", typeof(VisualParameters), typeof(MapLegendItemWithOptions), new PropertyMetadata(null));




        public bool IsChecked
        {
            get { return (bool)GetValue(IsCheckedProperty); }
            set { SetValue(IsCheckedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsChecked.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsCheckedProperty =
            DependencyProperty.Register("IsChecked", typeof(bool), typeof(MapLegendItemWithOptions), new PropertyMetadata(false));



        public IEnumerable<LegendCommand> Commands
        {
            get { return (IEnumerable<LegendCommand>)GetValue(CommandsProperty); }
            set { SetValue(CommandsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Commands.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandsProperty =
            DependencyProperty.Register("Commands", typeof(IEnumerable<LegendCommand>), typeof(MapLegendItemWithOptions), new PropertyMetadata(null));




    }
}
