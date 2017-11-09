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
    /// Interaction logic for GoToMapProjectView.xaml
    /// </summary>
    public partial class GoToMapProjectView : UserControl
    {
        public GoToMapProjectView()
        {
            InitializeComponent();
        }


        public string Note { get => "UTM"; }

    }
}
