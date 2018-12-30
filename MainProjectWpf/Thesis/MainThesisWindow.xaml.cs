using IRI.Ket.DataManagement.DataSource;
using IRI.Ket.SpatialExtensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
using System.Windows.Shapes;

namespace IRI.MainProjectWPF.Thesis
{
    /// <summary>
    /// Interaction logic for MainThesisWindow.xaml
    /// </summary>
    public partial class MainThesisWindow : Window
    {
        public MainThesisWindow()
        {
            InitializeComponent();



            //var folder = @"E:\Data\Iran\GSI Categorized\250kJPEGWm";

            //System.IO.DirectoryInfo info = new DirectoryInfo(folder);

            //foreach (var file in info.EnumerateFiles("*.jpg"))
            //{
            //    IRI.Ket.WorldfileFormat.WorldfilePyramid.Create(file.FullName, 0.00026458386250105835);
            //}

        }
        private double GetUnitDistance()
        {
            double? _unitDistance = null;

            if (_unitDistance == null || double.IsNaN(_unitDistance.Value))
            {
                PresentationSource source = PresentationSource.FromVisual(this);

                if (source == null)
                    return 1;

                double dpiX = 96.0 * source.CompositionTarget.TransformToDevice.M11;

                //size of each pixel (in meter)
                _unitDistance = IRI.Msh.Common.Helpers.ConversionHelper.InchToMeterFactor / dpiX;
            }

            return _unitDistance.Value;
        }

        private void openKdTree(object sender, RoutedEventArgs e)
        {
            MainWindow window = new MainWindow();

            window.Show();
        }

        private void openKdTreeSfc(object sender, RoutedEventArgs e)
        {
            KdTreeWindow window = new KdTreeWindow();

            window.Show();
        }

        private void openRTreeSfc(object sender, RoutedEventArgs e)
        {
            RTreeWindow window = new RTreeWindow();

            window.Show();
        }

        private void openSfc(object sender, RoutedEventArgs e)
        {
            RasterSfcWindow window = new RasterSfcWindow();

            window.Show();

        }

        private void pointSorting(object sender, RoutedEventArgs e)
        {
            PointDistributionOrderingWindow window = new PointDistributionOrderingWindow();

            window.Show();
        }

        private void BigDataSetButton_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Stopwatch watch = System.Diagnostics.Stopwatch.StartNew();

            int k = 1;

            for (int i = 1; i < 124; i++)
            {

                int n = i * 1000;

                //read the points
                SqlServerDataSource source = new SqlServerDataSource("data source=.;integrated security=true;initial catalog = IRI.Database", "Earthquakes", "Location");
                var points = source.SelectFeatures("SELECT top(" + n + ") lat,_long FROM Earthquakes").Select(d => new IRI.Msh.Common.Primitives.Point((double)d["lat"], (double)d["_long"])).ToList();
                watch.Stop();
                //Debug.WriteLine($"STEP 1: {watch.ElapsedMilliseconds}");
                //Debug.WriteLine($"STEP 1: {watch.ElapsedMilliseconds}");
                watch.Restart();

                //order the points
                IRI.Ket.Spatial.PointSorting.PointOrdering.GraySorter(points.ToArray());

                watch.Stop();
                Debug.WriteLine($"{n}, {watch.ElapsedMilliseconds}");
                watch.Restart();

                if (i > 40)
                {
                    i += 2;
                }
                if (i > 60)
                {
                    i += 4;
                }
                if (i > 80)
                {
                    i += 5;
                }
                if (i > 100)
                {
                    i += 6;
                }
            }

            //show result
        }
    }
}
