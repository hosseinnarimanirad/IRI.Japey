using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace IRI.Jab.NavigationControl
{


    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class NavigationControl : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        private static readonly double panStep = 40;

        //Dictionary<double, double> zoomLevels;

        //IRI.MapViewer.MapViewer mapViewer;

        public event MouseButtonEventHandler OnMoveLeft;

        public event MouseButtonEventHandler OnMoveRight;

        public event MouseButtonEventHandler OnMoveUp;

        public event MouseButtonEventHandler OnMoveDown;

        public event MouseButtonEventHandler OnZoomIn;

        public event MouseButtonEventHandler OnZoomOut;


        //public event IRI.Jab.Common.ZoomEventHandler OnZoomChanged;

        //public event IRI.Jab.Common.PanEventHandler OnPanTriggered;

        public event EventHandler<IRI.Jab.Common.ZoomEventArgs> OnZoomChanged;

        public event EventHandler<IRI.Jab.Common.PanEventArgs> OnPanTriggered;



        public NavigationControl()
        {
            InitializeComponent();

            //Binding binding = new Binding("Value");
            //binding.Source = zoom;
            //binding.Mode = BindingMode.TwoWay;
            //this.SetBinding(ZoomLevelProperty, binding);
        }

        private void moveRight_MouseDown(object sender, MouseButtonEventArgs e)
        {
            PanMap(-panStep, 0);

            if (this.OnMoveRight != null)
            {
                this.OnMoveRight(sender, e);
            }
        }

        private void moveDown_MouseDown(object sender, MouseButtonEventArgs e)
        {
            PanMap(0, -panStep);

            if (this.OnMoveDown != null)
            {
                this.OnMoveDown(sender, e);
            }
        }

        private void moveLeft_MouseDown(object sender, MouseButtonEventArgs e)
        {
            PanMap(panStep, 0);

            if (this.OnMoveLeft != null)
            {
                this.OnMoveLeft(sender, e);
            }
        }

        private void moveUp_MouseDown(object sender, MouseButtonEventArgs e)
        {
            PanMap(0, panStep);

            if (this.OnMoveUp != null)
            {
                this.OnMoveUp(sender, e);
            }
        }

        private void zoomIn_MouseDown(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Trace.WriteLine(new StackTrace().GetFrame(0).GetMethod().Name + " " + this.zoom.Value);

            if (this.zoom.Value + 1 <= this.zoom.Maximum)
            {
                this.ZoomLevel += 1;
            }

            if (this.OnZoomIn != null)
            {
                this.OnZoomIn(sender, e);
            }
        }

        private void zoomOut_MouseDown(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Trace.WriteLine(new StackTrace().GetFrame(0).GetMethod().Name + " " + this.zoom.Value);

            if (this.zoom.Value - 1 >= this.zoom.Minimum)
            {
                //this.zoom.Value -= 1;
                this.ZoomLevel -= 1;
            }

            if (this.OnZoomOut != null)
            {
                this.OnZoomOut(sender, e);
            }
        }

        private void zoom_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this.ZoomLevel = this.zoom.Value;

            this.OnZoomChanged?.Invoke(sender, new Common.ZoomEventArgs(e.NewValue, double.NaN));
        }

        private void PanMap(double xOffset, double yOffset)
        {
            if (this.OnPanTriggered != null)
            {
                this.OnPanTriggered(this, new IRI.Jab.Common.PanEventArgs(new Point(xOffset, yOffset)));
            }
        }

        public void UpdateZoomLevel(double zoomLevel)
        {
            this.zoom.ValueChanged -= zoom_ValueChanged;

            this.zoom.Value = zoomLevel;

            this.zoom.ValueChanged += zoom_ValueChanged;
        }

        public void ZoomIn()
        {
            //this.zoom.Value++;
            this.ZoomLevel = this.ZoomLevel + 1;
        }

        public void ZoomOut()
        {
            //this.zoom.Value--;
            this.ZoomLevel = this.ZoomLevel - 1;
        }

        //public double ZoomLevel
        //{
        //    get { return this.zoom.Value; }
        //}

        public double ZoomLevel
        {
            get { return (double)GetValue(ZoomLevelProperty); }
            set
            {
                System.Diagnostics.Trace.WriteLine("NAV0 " + new StackTrace().GetFrame(0).GetMethod().Name + " " + value.ToString());
                System.Diagnostics.Trace.WriteLine("\t\tNAV1 " + new StackTrace().GetFrame(1).GetMethod().Name + " " + value.ToString());

                if (this.zoom.Maximum < value || this.zoom.Minimum > value)
                    return;

                UpdateZoomLevel(value);

                SetValue(ZoomLevelProperty, value);

                OnPropertyChanged("ZoomLevel");
                //this.zoom.Value = value;
            }
        }

        // Using a DependencyProperty as the backing store for Fill.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ZoomLevelProperty =
            DependencyProperty.Register("ZoomLevel",
                typeof(double),
                typeof(NavigationControl),
                new PropertyMetadata(new PropertyChangedCallback((d, dp) =>
                {
                    ((NavigationControl)d).UpdateZoomLevel((double)dp.NewValue);
                })));
    }
}
