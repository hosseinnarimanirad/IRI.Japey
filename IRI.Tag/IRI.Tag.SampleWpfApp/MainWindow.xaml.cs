using IRI.Msh.Common.Helpers;
using IRI.Msh.Common.Primitives;
using IRI.Sta.Common.Helpers;
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

namespace IRI.Tag.SampleWpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                SqlServerTypes.Utilities.LoadNativeAssembliesv14(Environment.CurrentDirectory);
            }
            catch (Exception ex)
            {
                MessageBox.Show("problem #1000");
            }

            var presenter = new ViewModel.AppPresenter();

            await this.map.Register(presenter);

            presenter.Initialize(this);

            this.DataContext = presenter;


            //presenter.RemoveAllProviders();
            //add a map provider to default providers
            presenter.MapProviders.Add(Jab.Common.TileServices.TileMapProviderFactory.WazeStreet);

            presenter.ZoomToExtent(BoundingBoxes.IranWebMercatorBoundingBox, false);

        }
    }
}
