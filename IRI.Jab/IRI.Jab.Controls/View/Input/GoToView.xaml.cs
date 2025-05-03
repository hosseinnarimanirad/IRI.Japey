using System.Windows;
using System.Windows.Controls;

namespace IRI.Jab.Controls.View.Input
{
    /// <summary>
    /// Interaction logic for GoToView.xaml
    /// </summary>
    public partial class GoToView : UserControl
    {
        public Presenter.GoToPresenter Presenter { get => this.DataContext as Presenter.GoToPresenter; }

        public GoToView()
        {
            InitializeComponent();

            //this.hamburgerMenuControl.ItemsSource = new List<HamburgerGoToMenuItem>()
            //{
            //    new HamburgerGoToMenuItem(new GoToGeodeticView()){ Title = "Geodetic", SubTitle=string.Empty, Tooltip="Geodetic", Icon= IRI.Jab.Common.Assets.ShapeStrings.Appbar.appbarGlobe},
            //    new HamburgerGoToMenuItem(new GoToMapProjectView()){ Title = "UTM", SubTitle="UTM", Tooltip="UTM", Icon= IRI.Jab.Common.Assets.ShapeStrings.Appbar.appbarMapTreasure}
            //};
        }

        //public GoToView(GoToPresenter presenter) : this()
        //{
        //    //this.hamburgerMenuControl.ItemsSource = new List<HamburgerGoToMenuItem>()
        //    //{
        //    //    new HamburgerGoToMenuItem(new GoToGeodeticView()){ Title = "Geodetic", SubTitle=string.Empty, Tooltip="Geodetic", Icon= IRI.Jab.Common.Assets.ShapeStrings.Appbar.appbarGlobe},
        //    //    new HamburgerGoToMenuItem(new GoToMapProjectView()){ Title = "UTM", SubTitle="UTM", Tooltip="UTM", Icon= IRI.Jab.Common.Assets.ShapeStrings.Appbar.appbarMapTreasure}
        //    //};


        //    //this.hamburgerMenuControl.OptionsItemsSource = ;
        //}

        private void HamburgerMenu_ItemClick(object sender, MahApps.Metro.Controls.ItemClickEventArgs e)
        {
            //var menuItem = e.ClickedItem as HamburgerGoToMenuItem;

            //this.hamburgerMenuControl.Content = menuItem.Content;

            //this.hamburgerMenuControl.IsPaneOpen = false;


            //contentFrame.Navigate(menuItem.PageType);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //this.Presenter.IsPaneOpen = false;
        }
    }
}
