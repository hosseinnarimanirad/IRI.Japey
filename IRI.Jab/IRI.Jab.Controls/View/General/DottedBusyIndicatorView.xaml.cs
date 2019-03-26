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

namespace IRI.Jab.Controls.View
{
    /// <summary>
    /// Interaction logic for DottedBusyIndicator.xaml
    /// </summary>
    public partial class DottedBusyIndicatorView : UserControl
    {
        public DottedBusyIndicatorView()
        {
            InitializeComponent();
        }



        public Brush DotBrush
        {
            get { return (Brush)GetValue(DotBrushProperty); }
            set { SetValue(DotBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DotBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DotBrushProperty =
            DependencyProperty.Register(nameof(DotBrush), typeof(Brush), typeof(DottedBusyIndicatorView), new PropertyMetadata(Brushes.White));




        public int DotSize
        {
            get { return (int)GetValue(DotSizeProperty); }
            set { SetValue(DotSizeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DotSize.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DotSizeProperty =
            DependencyProperty.Register(nameof(DotSize), typeof(int), typeof(DottedBusyIndicatorView), new PropertyMetadata(8));


    }
}
