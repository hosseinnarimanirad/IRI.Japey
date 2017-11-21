using IRI.Ket.Common.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace IRI.Jab.Controls.View.Map
{
    /// <summary>
    /// Interaction logic for CoordinatePanelView.xaml
    /// </summary>
    public partial class CoordinatePanelView : UserControl, INotifyPropertyChanged
    {
        public CoordinatePanelView()
        {
            InitializeComponent();
        }

        private void options_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.optionsRow.Height = new GridLength(1, GridUnitType.Auto);

            this.Opacity = 1;
        }

        private void coordinateChanged_Checked(object sender, RoutedEventArgs e)
        {
            //this.optionsRow.Height = new GridLength(0, GridUnitType.Pixel);

            this.system.Content = ((RadioButton)sender).Tag.ToString();

            //this.Opacity = .8;
            if (geodeticDms.IsChecked == true || geodeticDd.IsChecked == true)
            {
                this.first.Content = "طول جغرافیایی:";

                this.second.Content = "عرض جغرافیایی:";
            }
            else// if (utm.IsChecked == true)
            {
                this.first.Content = "X:";

                this.second.Content = "Y:";
            }

        }

        private string _xLabel;

        public string XLabel
        {
            get { return _xLabel; }
            set
            {
                _xLabel = value.LatinNumbersToFarsiNumbers();
                RaisePropertyChanged();
            }
        }

        private string _yLabel;

        public string YLabel
        {
            get { return _yLabel; }
            set
            {
                _yLabel = value.LatinNumbersToFarsiNumbers();
                RaisePropertyChanged();
            }
        }



        /// <summary>
        /// Sets the x,y coordinates from elipsoidal mercatar. elipsoid: WGS84
        /// </summary>
        /// <param name="mecatorX"></param>
        /// <param name="mercatorY"></param>
        public void SetCoordinates(Point geodeticPoint)
        {

            this.zone.Visibility = System.Windows.Visibility.Collapsed;

            this.region.Visibility = System.Windows.Visibility.Collapsed;

            if (geodeticDms.IsChecked == true)
            {
                IRI.Ham.MeasurementUnit.Degree longitude = new IRI.Ham.MeasurementUnit.Degree(geodeticPoint.X);

                IRI.Ham.MeasurementUnit.Degree latitude = new IRI.Ham.MeasurementUnit.Degree(geodeticPoint.Y);

                this.XLabel = DegreeHelper.ToDms(geodeticPoint.X, true);

                //string.Format("{0:D3}° {1:D2} {2:00.0}''", longitude.DegreePart, longitude.MinutePart, longitude.SecondPart);

                //this.YLabel = string.Format("{0:D3}° {1:D2}' {2:00.0}''", latitude.DegreePart, latitude.MinutePart, latitude.SecondPart);

                this.YLabel = DegreeHelper.ToDms(geodeticPoint.Y, true);
            }
            else if (geodeticDd.IsChecked == true)
            {
                this.XLabel = string.Format(CultureInfo.InvariantCulture, "{0:F5}", geodeticPoint.X.ToString("#,#.#####"));

                this.YLabel = string.Format(CultureInfo.InvariantCulture, "{0:F5}", geodeticPoint.Y.ToString("#,#.#####"));
            }
            else if (utm.IsChecked == true)
            {
                Point tempUtm = GeodeticToUTM(geodeticPoint);

                this.XLabel = string.Format(CultureInfo.InvariantCulture, "{0:F5}", tempUtm.X.ToString("#,#.#####"));

                this.YLabel = string.Format(CultureInfo.InvariantCulture, "{0:F5}", tempUtm.Y.ToString("#,#.#####"));

                this.zone.Visibility = System.Windows.Visibility.Visible;

                this.region.Visibility = System.Windows.Visibility.Visible;

                this.zone.Content = IRI.Ham.CoordinateSystem.MapProjection.MapProjects.FindZone(geodeticPoint.X);
            }
            else if (mercator.IsChecked == true)
            {
                Point tempUtm = GeodeticToMercator(geodeticPoint);

                this.XLabel = string.Format(CultureInfo.InvariantCulture, "{0:F5}", tempUtm.X.ToString("#,#.#####"));

                this.YLabel = string.Format(CultureInfo.InvariantCulture, "{0:F5}", tempUtm.Y.ToString("#,#.#####"));
            }
            else if (cea.IsChecked == true)
            {
                Point tempUtm = GeodeticToCylindricalEqualArea(geodeticPoint);

                this.XLabel = string.Format(CultureInfo.InvariantCulture, "{0:F5}", tempUtm.X.ToString("#,#.#####"));

                this.YLabel = string.Format(CultureInfo.InvariantCulture, "{0:F5}", tempUtm.Y.ToString("#,#.#####"));
            }
            else if (tm.IsChecked == true)
            {
                Point tempUtm = GeodeticToTransverseMercator(geodeticPoint);

                this.XLabel = string.Format(CultureInfo.InvariantCulture, "{0:F5}", tempUtm.X.ToString("#,#.#####"));

                this.YLabel = string.Format(CultureInfo.InvariantCulture, "{0:F5}", tempUtm.Y.ToString("#,#.#####"));
            }

        }

        //private Point MercatorToDecimalGeodetic(Point mercatorPoint)
        //{
        //    double[][] result = IRI.Ham.CoordinateSystem.Projection.MercatorToGeodetic(
        //                                                            new double[] { mercatorPoint.X },
        //                                                            new double[] { mercatorPoint.Y },
        //                                                            IRI.Ham.CoordinateSystem.Ellipsoids.WGS84);

        //    return new Point(result[0][0], result[1][0]);
        //}

        private Point GeodeticToUTM(Point geodeticPoint)
        {
            try
            {
                IRI.Ham.SpatialBase.Point result =
                    IRI.Ham.CoordinateSystem.MapProjection.MapProjects.GeodeticToUTM(
                            new IRI.Ham.SpatialBase.Point(geodeticPoint.X, geodeticPoint.Y),
                            IRI.Ham.CoordinateSystem.Ellipsoids.WGS84);

                return new Point(result.X, result.Y);
            }
            catch (Exception)
            {
                return new Point(double.NaN, double.NaN);
            }
        }

        public Point GeodeticToMercator(Point point)
        {
            try
            {
                double[][] result = IRI.Ham.CoordinateSystem.MapProjection.MapProjects.GeodeticToMercator(
                                                                        new double[] { point.X },
                                                                        new double[] { point.Y },
                                                                        IRI.Ham.CoordinateSystem.Ellipsoids.WGS84);

                return new Point(result[0][0], result[1][0]);
            }
            catch (Exception)
            {
                return new Point(double.NaN, double.NaN);
            }
        }

        public Point GeodeticToTransverseMercator(Point point)
        {
            try
            {
                double[][] result = IRI.Ham.CoordinateSystem.MapProjection.MapProjects.GeodeticToTransverseMercator(
                                                                        new double[] { point.X },
                                                                        new double[] { point.Y },
                                                                        IRI.Ham.CoordinateSystem.Ellipsoids.WGS84);

                return new Point(result[0][0], result[1][0]);
            }
            catch (Exception)
            {
                return new Point(double.NaN, double.NaN);
            }
        }

        private Point GeodeticToCylindricalEqualArea(Point point)
        {
            try
            {
                double[][] result =
                    IRI.Ham.CoordinateSystem.MapProjection.MapProjects.GeodeticToCylindricalEqualArea(
                        new double[] { point.X },
                        new double[] { point.Y }, IRI.Ham.CoordinateSystem.Ellipsoids.WGS84,
                        0, 0);

                return new Point(result[0][0], result[1][0]);
            }
            catch (Exception)
            {
                return new Point(double.NaN, double.NaN);
            }

        }

        private void close_Click(object sender, RoutedEventArgs e)
        {
            this.optionsRow.Height = new GridLength(0, GridUnitType.Pixel);

            this.Opacity = .8;
        }

        private void UserControl_MouseLeave(object sender, MouseEventArgs e)
        {
            this.optionsRow.Height = new GridLength(0, GridUnitType.Pixel);

            this.Opacity = .8;
        }

        //private Point MercatorToDmsGeodetic(Point mercatorPoint)
        //{
        //    Point geodetic = MercatorToDecimalGeodetic(mercatorPoint);

        //    IRI.Ham.MeasurementUnit.Degree longitude = new IRI.Ham.MeasurementUnit.Degree(geodetic.X);

        //    IRI.Ham.MeasurementUnit.Degree latitude = new IRI.Ham.MeasurementUnit.Degree(geodetic.Y);


        //}



        public Point Position
        {
            get { return (Point)GetValue(PositionProperty); }
            set { SetValue(PositionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Position.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PositionProperty =
            DependencyProperty.Register("Position", typeof(Point), typeof(CoordinatePanelView), new PropertyMetadata(new PropertyChangedCallback((d, dp) =>
            {
                try
                {
                    ((CoordinatePanelView)d).SetCoordinates((Point)dp.NewValue);
                }
                catch (Exception ex)
                {
                    return;
                }
            })));



        public bool ShowGeodetic
        {
            get { return (bool)GetValue(ShowGeodeticProperty); }
            set { SetValue(ShowGeodeticProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShowGeodetic.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowGeodeticProperty =
            DependencyProperty.Register("ShowGeodetic", typeof(bool), typeof(CoordinatePanelView), new PropertyMetadata(true));


        public bool ShowUTM
        {
            get { return (bool)GetValue(ShowUTMProperty); }
            set { SetValue(ShowUTMProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShowUTM.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowUTMProperty =
            DependencyProperty.Register("ShowUTM", typeof(bool), typeof(CoordinatePanelView), new PropertyMetadata(true));


        public bool ShowMercator
        {
            get { return (bool)GetValue(ShowMercatorProperty); }
            set { SetValue(ShowMercatorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShowMercator.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowMercatorProperty =
            DependencyProperty.Register("ShowMercator", typeof(bool), typeof(CoordinatePanelView), new PropertyMetadata(true));


        public bool ShowTM
        {
            get { return (bool)GetValue(ShowTMProperty); }
            set { SetValue(ShowTMProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShowTM.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowTMProperty =
            DependencyProperty.Register("ShowTM", typeof(bool), typeof(CoordinatePanelView), new PropertyMetadata(true));



        public bool ShowCylindricalEqualArea
        {
            get { return (bool)GetValue(ShowCylindricalEqualAreaProperty); }
            set { SetValue(ShowCylindricalEqualAreaProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShowCylindricalEqualArea.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowCylindricalEqualAreaProperty =
            DependencyProperty.Register("ShowCylindricalEqualArea", typeof(bool), typeof(CoordinatePanelView), new PropertyMetadata(true));


        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
