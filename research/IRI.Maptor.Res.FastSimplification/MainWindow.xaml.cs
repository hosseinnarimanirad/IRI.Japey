using System.Windows;

namespace IRI.Maptor.Res.FastSimplification;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private async void Button_Click(object sender, RoutedEventArgs e)
    {
        await SimplificationHelper.GeneralTest();
    }
}