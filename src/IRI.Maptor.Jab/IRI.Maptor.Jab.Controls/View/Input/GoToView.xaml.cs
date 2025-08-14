using System.Windows;
using System.Windows.Controls;

namespace IRI.Maptor.Jab.Controls.View;

/// <summary>
/// Interaction logic for GoToView.xaml
/// </summary>
public partial class GoToView : UserControl
{
    public Presenter.GoToPresenter Presenter { get => this.DataContext as Presenter.GoToPresenter; }

    public GoToView()
    {
        InitializeComponent(); 
    }
}
