using IRI.Jab.Common.Model;
using IRI.Jab.Controls.Presenter;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using IRI.Jab.Common.Extensions;
using IRI.Jab.Common.Model.CoordinatePanel;
using IRI.Jab.Common.Presenter;

namespace IRI.Jab.Controls.View.Map
{
    /// <summary>
    /// Interaction logic for CoordinatePanelView.xaml
    /// </summary>
    public partial class CoordinatePanelView : UserControl, INotifyPropertyChanged
    {
        public CoordinatePanelPresenter Presenter { get { return this.DataContext as CoordinatePanelPresenter; } }
        //const string persianLongitudeLabel = "طول جغرافیایی";
        //const string persianLatitudeLabel = "عرض جغرافیایی";

        //const string englishLongitudeLabel = "Longitude";
        //const string englishLatitudeLabel = "Latitude";

        //const string persianZoneLabel = "ناحیه";
        //const string englishZoneLabel = "Zone";

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        public CoordinatePanelView()
        {
            InitializeComponent(); 
        }




        private void options_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.optionsRow.Height = new GridLength(1, GridUnitType.Auto);

            this.optionsRow2.Height = new GridLength(1, GridUnitType.Auto);

            this.Opacity = 1;
        }

        //private void coordinateChanged_Checked(object sender, RoutedEventArgs e)
        //{
        //    //this.optionsRow.Height = new GridLength(0, GridUnitType.Pixel);

        //    this.system.Content = ((RadioButton)sender).Tag.ToString();

        //    //this.Opacity = .8;
        //    if (geodeticDms.IsChecked == true || geodeticDd.IsChecked == true)
        //    {
        //        this.first.Content = "طول جغرافیایی:";

        //        this.second.Content = "عرض جغرافیایی:";
        //    }
        //    else// if (utm.IsChecked == true)
        //    {
        //        this.first.Content = "X:";

        //        this.second.Content = "Y:";
        //    }

        //}

        //private string _xLabel;

        //public string XLabel
        //{
        //    get { return _xLabel; }
        //    set
        //    {
        //        _xLabel = value;
        //        RaisePropertyChanged();
        //    }
        //}

        //private string _yLabel;

        //public string YLabel
        //{
        //    get { return _yLabel; }
        //    set
        //    {
        //        _yLabel = value;
        //        RaisePropertyChanged();
        //    }
        //}



        //private string _xValue;

        //public string XValue
        //{
        //    get { return _xValue; }
        //    set
        //    {
        //        _xValue = value.LatinNumbersToFarsiNumbers();
        //        RaisePropertyChanged();
        //    }
        //}

        //private string _yValue;

        //public string YValue
        //{
        //    get { return _yValue; }
        //    set
        //    {
        //        _yValue = value.LatinNumbersToFarsiNumbers();
        //        RaisePropertyChanged();
        //    }
        //}



        /// <summary>
        /// Sets the x,y coordinates from elipsoidal mercatar. elipsoid: WGS84
        /// </summary>
        /// <param name="mecatorX"></param>
        /// <param name="mercatorY"></param>
        public void SetCoordinates(Point geodeticPoint)
        {
            Presenter.SelectedItem?.Update(geodeticPoint.AsPoint());


        }

        //private Point GeodeticToUTM(Point geodeticPoint)
        //{
        //    try
        //    {
        //        IRI.Msh.Common.Primitives.Point result =
        //            IRI.Msh.CoordinateSystem.MapProjection.MapProjects.GeodeticToUTM(
        //                    new IRI.Msh.Common.Primitives.Point(geodeticPoint.X, geodeticPoint.Y),
        //                    IRI.Msh.CoordinateSystem.Ellipsoids.WGS84);

        //        return new Point(result.X, result.Y);
        //    }
        //    catch (Exception)
        //    {
        //        return new Point(double.NaN, double.NaN);
        //    }
        //}

        //public Point GeodeticToMercator(Point point)
        //{
        //    try
        //    {
        //        double[][] result = IRI.Msh.CoordinateSystem.MapProjection.MapProjects.GeodeticToMercator(
        //                                                                new double[] { point.X },
        //                                                                new double[] { point.Y },
        //                                                                IRI.Msh.CoordinateSystem.Ellipsoids.WGS84);

        //        return new Point(result[0][0], result[1][0]);
        //    }
        //    catch (Exception)
        //    {
        //        return new Point(double.NaN, double.NaN);
        //    }
        //}

        //public Point GeodeticToTransverseMercator(Point point)
        //{
        //    try
        //    {
        //        double[][] result = IRI.Msh.CoordinateSystem.MapProjection.MapProjects.GeodeticToTransverseMercator(
        //                                                                new double[] { point.X },
        //                                                                new double[] { point.Y },
        //                                                                IRI.Msh.CoordinateSystem.Ellipsoids.WGS84);

        //        return new Point(result[0][0], result[1][0]);
        //    }
        //    catch (Exception)
        //    {
        //        return new Point(double.NaN, double.NaN);
        //    }
        //}

        //private Point GeodeticToCylindricalEqualArea(Point point)
        //{
        //    try
        //    {
        //        double[][] result =
        //            IRI.Msh.CoordinateSystem.MapProjection.MapProjects.GeodeticToCylindricalEqualArea(
        //                new double[] { point.X },
        //                new double[] { point.Y }, IRI.Msh.CoordinateSystem.Ellipsoids.WGS84,
        //                0, 0);

        //        return new Point(result[0][0], result[1][0]);
        //    }
        //    catch (Exception)
        //    {
        //        return new Point(double.NaN, double.NaN);
        //    }

        //}

        //private void close_Click(object sender, RoutedEventArgs e)
        //{
        //    this.optionsRow.Height = new GridLength(0, GridUnitType.Pixel);

        //    this.Opacity = .8;
        //}

        private void UserControl_MouseLeave(object sender, MouseEventArgs e)
        {
            this.optionsRow.Height = new GridLength(0, GridUnitType.Pixel);

            this.optionsRow2.Height = new GridLength(0, GridUnitType.Pixel);

            this.Opacity = .8;
        }

        public Point Position
        {
            get { return (Point)GetValue(PositionProperty); }
            set { SetValue(PositionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Position.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PositionProperty =
            DependencyProperty.Register(nameof(Position), typeof(Point), typeof(CoordinatePanelView), new PropertyMetadata(new PropertyChangedCallback((d, dp) =>
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



        //public bool ShowGeodetic
        //{
        //    get { return (bool)GetValue(ShowGeodeticProperty); }
        //    set { SetValue(ShowGeodeticProperty, value); }
        //}

        //// Using a DependencyProperty as the backing store for ShowGeodetic.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty ShowGeodeticProperty =
        //    DependencyProperty.Register(nameof(ShowGeodetic), typeof(bool), typeof(CoordinatePanelView), new PropertyMetadata(true));


        //public bool ShowUTM
        //{
        //    get { return (bool)GetValue(ShowUTMProperty); }
        //    set { SetValue(ShowUTMProperty, value); }
        //}

        //// Using a DependencyProperty as the backing store for ShowUTM.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty ShowUTMProperty =
        //    DependencyProperty.Register(nameof(ShowUTM), typeof(bool), typeof(CoordinatePanelView), new PropertyMetadata(true));


        //public bool ShowMercator
        //{
        //    get { return (bool)GetValue(ShowMercatorProperty); }
        //    set { SetValue(ShowMercatorProperty, value); }
        //}

        //// Using a DependencyProperty as the backing store for ShowMercator.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty ShowMercatorProperty =
        //    DependencyProperty.Register(nameof(ShowMercator), typeof(bool), typeof(CoordinatePanelView), new PropertyMetadata(true));


        //public bool ShowTM
        //{
        //    get { return (bool)GetValue(ShowTMProperty); }
        //    set { SetValue(ShowTMProperty, value); }
        //}

        //// Using a DependencyProperty as the backing store for ShowTM.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty ShowTMProperty =
        //    DependencyProperty.Register(nameof(ShowTM), typeof(bool), typeof(CoordinatePanelView), new PropertyMetadata(true));



        //public bool ShowCylindricalEqualArea
        //{
        //    get { return (bool)GetValue(ShowCylindricalEqualAreaProperty); }
        //    set { SetValue(ShowCylindricalEqualAreaProperty, value); }
        //}

        //// Using a DependencyProperty as the backing store for ShowCylindricalEqualArea.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty ShowCylindricalEqualAreaProperty =
        //    DependencyProperty.Register(nameof(ShowCylindricalEqualArea), typeof(bool), typeof(CoordinatePanelView), new PropertyMetadata(true));



        public LanguageMode UILanguage
        {
            get { return (LanguageMode)GetValue(UILanguageProperty); }
            set
            {
                SetValue(UILanguageProperty, value);
                SetLanguage(value);
            }
        }

        // Using a DependencyProperty as the backing store for UILanguage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UILanguageProperty =
            DependencyProperty.Register(
                nameof(UILanguage),
                typeof(LanguageMode),
                typeof(CoordinatePanelView),
                new PropertyMetadata(LanguageMode.Persian, (d, dp) =>
                {
                    try
                    {
                        ((CoordinatePanelView)d).SetLanguage((LanguageMode)dp.NewValue);
                    }
                    catch (Exception ex)
                    {
                        return;
                    }
                }));

        private void SetLanguage(LanguageMode value)
        {
            if (this.Presenter != null)
            {
                this.Presenter.SetLanguage(value);
            }
        }

         

    }
}
