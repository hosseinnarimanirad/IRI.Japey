using System;
using System.Windows;
using IRI.Sta.Common.Primitives;

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
            catch
            {
                MessageBox.Show("error!");
            }

            var presenter = new ViewModel.AppPresenter();

            await this.map.Register(presenter);

            presenter.Initialize(this);

            this.DataContext = presenter;


            //presenter.RemoveAllProviders();
            //add a map provider to default providers
            presenter.MapProviders.Add(Jab.Common.TileServices.TileMapProviderFactory.WazeStreet);

            presenter.ZoomToExtent(BoundingBoxes.IranWebMercatorBoundingBox, false, isNewExtent: true);

        }
    }
}
