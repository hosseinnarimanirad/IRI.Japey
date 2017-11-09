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

namespace IRI.Jab.Controls.View.Input
{
    /// <summary>
    /// Interaction logic for GoToView.xaml
    /// </summary>
    public partial class GoToView : UserControl
    {
        public GoToView()
        {
            InitializeComponent();
        }

        private void HamburgerMenu_ItemClick(object sender, MahApps.Metro.Controls.ItemClickEventArgs e)
        {
            this.hamburgerMenuControl.Content = e.ClickedItem;

            this.hamburgerMenuControl.IsPaneOpen = false;
        }
    }
}
