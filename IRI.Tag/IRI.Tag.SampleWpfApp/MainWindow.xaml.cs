using System;
using System.Globalization;
using System.Windows;
using IRI.Jab.Common;
using IRI.Jab.Common.TileServices;
using IRI.Sta.Common.Primitives;

namespace IRI.Tag.SampleWpfApp;

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

        var presenter = new ViewModel.AppViewModel();

        await this.map.Register(presenter);

        presenter.Initialize(this);

        this.DataContext = presenter;
         
        presenter.ZoomToExtent(BoundingBoxes.WebMercator_Africa, false, isNewExtent: true);

        presenter.SelectedMapProvider = TileMapProviderFactory.GoogleRoadMap;
    }
}
