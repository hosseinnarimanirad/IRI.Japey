using System;
using System.Collections.Generic;
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

namespace IRI.Jab.CoordinatePanel
{
    /// <summary>
    /// Interaction logic for FlatCoordinateWindow.xaml
    /// </summary>
    public partial class FlatCoordinateWindow : UserControl
    {
        public FlatCoordinateWindow()
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

                this.x1.Content = string.Format("{0:D3}° {1:D2}' {2:00.0}''", longitude.DegreePart, longitude.MinutePart, longitude.SecondPart);

                this.x2.Content = string.Format("{0:D3}° {1:D2}' {2:00.0}''", latitude.DegreePart, latitude.MinutePart, latitude.SecondPart);
            }
            else if (geodeticDd.IsChecked == true)
            {
                this.x1.Content = string.Format("{0:F5}", geodeticPoint.X.ToString("#,#.#####"));

                this.x2.Content = string.Format("{0:F5}", geodeticPoint.Y.ToString("#,#.#####"));
            }
            else if (utm.IsChecked == true)
            {
                Point tempUtm = GeodeticToUTM(geodeticPoint);

                this.x1.Content = string.Format("{0:F5}", tempUtm.X.ToString("#,#.#####"));

                this.x2.Content = string.Format("{0:F5}", tempUtm.Y.ToString("#,#.#####"));

                this.zone.Visibility = System.Windows.Visibility.Visible;

                this.region.Visibility = System.Windows.Visibility.Visible;

                this.zone.Content = IRI.Ham.CoordinateSystem.MapProjection.MapProjects.FindZone(geodeticPoint.X);
            }
            else if (mercator.IsChecked == true)
            {
                Point tempUtm = GeodeticToMercator(geodeticPoint);

                this.x1.Content = string.Format("{0:F5}", tempUtm.X.ToString("#,#.#####"));

                this.x2.Content = string.Format("{0:F5}", tempUtm.Y.ToString("#,#.#####"));
            }
            else if (cea.IsChecked == true)
            {
                Point tempUtm = GeodeticToCylindricalEqualArea(geodeticPoint);

                this.x1.Content = string.Format("{0:F5}", tempUtm.X.ToString("#,#.#####"));

                this.x2.Content = string.Format("{0:F5}", tempUtm.Y.ToString("#,#.#####"));
            }
            else if (tm.IsChecked == true)
            {
                Point tempUtm = GeodeticToTransverseMercator(geodeticPoint);

                this.x1.Content = string.Format("{0:F5}", tempUtm.X.ToString("#,#.#####"));

                this.x2.Content = string.Format("{0:F5}", tempUtm.Y.ToString("#,#.#####"));
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

    }
}
