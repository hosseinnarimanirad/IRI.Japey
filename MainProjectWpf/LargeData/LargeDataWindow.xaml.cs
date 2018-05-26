using IRI.Jab.Cartography;
using IRI.Ket.ShapefileFormat.EsriType;
using IRI.MainProjectWPF.LargeData.Model;
using IRI.MainProjectWPF.LargeData.Utilities;
using IRI.Sta.Common.Mapping;
using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using IRI.Ket.SpatialExtensions;

namespace IRI.MainProjectWPF.LargeData
{
    /// <summary>
    /// Interaction logic for LargeDataWindow.xaml
    /// </summary>
    public partial class LargeDataWindow : Window
    {
        ViewModel.ApplicationPresenter Presenter { get { return this.DataContext as ViewModel.ApplicationPresenter; } }

        public string TodayDirectory
        {
            get
            {
                System.Globalization.PersianCalendar calendar = new System.Globalization.PersianCalendar();

                return string.Format("E:\\Data\\0. Test\\Large Data\\{0}.{1}.{2}\\", calendar.GetYear(DateTime.Now), calendar.GetMonth(DateTime.Now), calendar.GetDayOfMonth(DateTime.Now));
            }
        }

        public LargeDataWindow()
        {
            InitializeComponent();

            this.DataContext = new ViewModel.ApplicationPresenter();

            if (!System.IO.Directory.Exists(TodayDirectory))
            {
                System.IO.Directory.CreateDirectory(TodayDirectory);
            }
        }

        private void Load_Click(object sender, RoutedEventArgs e)
        {
            //string file = @"E:\Data\0. Test\Large Data\TehranBlocks.shp";
            //string file = @"E:\Data\0. Test\Large Data\Countor_500Mercator.shp";

            ////string file = @"E:\Data\0. Test\Large Data\94.04.11\TehranBlocksWiseSlim12.shp";

            ////WiseSlim(file, @"E:\Data\0. Test\Large Data\94.04.12\TehranBlocks11NewTransformWithoutIterationMoreWise.shp", 1.0 / 288896);

            //WiseSlim(file, @"E:\Data\0. Test\Large Data\94.04.12\Countor_500Mercator11NewTransformWithoutIterationMoreWise.shp", 1.0 / 9244667.0);
            ////WiseSlim(file, @"E:\Data\0. Test\Large Data\94.04.11\TehranBlocksWiseSlim12.shp", 1.0 / 144448.0);
            ////WiseSlim(file, @"E:\Data\0. Test\Large Data\94.04.11\TehranBlocksWiseSlim14.shp", 1.0 / 36112.0);


            //string file = @"E:\Data\0. Test\Large Data\94.04.12\Countor_500Mercator11NewTransformWithoutIterationMoreWise.shp";

            //string file = @"E:\Research\Large Data\1394.04.11\errorRectangleShape.shp";
            //string file = @"E:\Data\0. Test\Large Data\94.04.16\errorCountor500.shp";


            //string file = @"E:\Data\0. Test\Large Data\94.04.18\TehranBlocksAddiArea13AfterAddiAngle.shp";
            var file = @"E:\Research\Large Data\ErrorPolygonRemoval.shp";

            var shapes = IRI.Ket.ShapefileFormat.Shapefile.Read(file);

            this.map.EnableZoomingOnMouseWheel();

            this.map.Pan();

            AddShapefile(shapes, System.IO.Path.GetFileNameWithoutExtension(file));

        }

        private void AddShapefile(IEsriShapeCollection shapes, string title)
        {
            this.Presenter.ShapeCollections.Add(new ShapeCollection(shapes, title));

            var geometries = shapes.Select(i => i.AsSqlGeometry(0)).ToList();

            var vertexes = shapes.ExtractPoints().Select(i => (((Sta.Common.Primitives.IPoint)i).AsSqlGeometry(0))).ToList();

            this.map.DrawGeometries(geometries, Guid.NewGuid().ToString(), VisualParameters.CreateNew(.7));

            this.map.DrawGeometries(vertexes, Guid.NewGuid().ToString(), VisualParameters.CreateNew(.9));
        }

        private void FullExtent_Click(object sender, RoutedEventArgs e)
        {
            this.map.FullExtent();
        }

        private void clear_Click(object sender, RoutedEventArgs e)
        {
            this.map.RemoveGeometries();

            this.Presenter.ShapeCollections.Clear();
        }

        private void ApplyArea_Click(object sender, RoutedEventArgs e)
        {
            if (Presenter.CurrentShapeCollection == null)
            {
                return;
            }

            var result = SimplifyAlgorithms.SimplifyByArea(Presenter.CurrentShapeCollection.Shapes, Presenter.AreaThreshold);

            AddShapefile(result, Presenter.CurrentShapeCollection.Title + "Area" + Presenter.AreaThreshold.ToString("#.0"));
        }

        private void ApplyAreaAdditive_Click(object sender, RoutedEventArgs e)
        {
            if (Presenter.CurrentShapeCollection == null)
            {
                return;
            }

            var result = SimplifyAlgorithms.AdditiveSimplifyByArea(Presenter.CurrentShapeCollection.Shapes, Presenter.AreaThreshold);

            AddShapefile(result, Presenter.CurrentShapeCollection.Title + "Area Adit." + Presenter.AreaThreshold.ToString("#.0"));
        }

        private void AngleApply_Click(object sender, RoutedEventArgs e)
        {
            if (Presenter.CurrentShapeCollection == null)
            {
                return;
            }

            var result = SimplifyAlgorithms.SimplifyByAngle(Presenter.CurrentShapeCollection.Shapes, Presenter.AngleThreshold);

            AddShapefile(result, Presenter.CurrentShapeCollection.Title + "Angle" + Presenter.AngleThreshold.ToString("#.00"));
        }

        private void ApplyAdditiveAngle_Click(object sender, RoutedEventArgs e)
        {
            if (Presenter.CurrentShapeCollection == null)
            {
                return;
            }

            var result = SimplifyAlgorithms.AdditiveSimplifyByAngle(Presenter.CurrentShapeCollection.Shapes, Presenter.AngleThreshold);

            AddShapefile(result, Presenter.CurrentShapeCollection.Title + "Adit. Angle" + Presenter.AngleThreshold.ToString("#.00"));
        }
        private void ApplyAdditiveDistance_Click(object sender, RoutedEventArgs e)
        {
            if (Presenter.CurrentShapeCollection == null)
            {
                return;
            }

            var result = SimplifyAlgorithms.AdditiveSimplifyByDistance(Presenter.CurrentShapeCollection.Shapes, Presenter.AreaThreshold / 4.0);

            AddShapefile(result, Presenter.CurrentShapeCollection.Title + "Adit. Dist." + (Presenter.AreaThreshold / 4.0).ToString("#.00"));

        }

        private void ApplyAreaAdditivePlus_Click(object sender, RoutedEventArgs e)
        {
            if (Presenter.CurrentShapeCollection == null)
            {
                return;
            }

            var result = SimplifyAlgorithms.AdditiveSimplifyByAreaPlus(Presenter.CurrentShapeCollection.Shapes, Presenter.AreaThreshold);

            AddShapefile(result, Presenter.CurrentShapeCollection.Title + "Adit. Area Plus" + (Presenter.AreaThreshold).ToString("#.00"));

        }

        //private void additiveTransform_Click(object sender, RoutedEventArgs e)
        //{
        //    string file = @"E:\Data\0. Test\Large Data\TehranBlocks.shp";
        //    string output = @"E:\Data\0. Test\Large Data\94.04.18\TehranBlocksAdditiveAngle.shp";

        //    //var result = Utilities.SimplifyAlgorithms.AdditiveAreaSimplify(IRI.Ket.ShapefileFormat.Shapefile.Read(file), 13);
        //    var result = Utilities.SimplifyAlgorithms.AdditiveSimplifyByAngle(IRI.Ket.ShapefileFormat.Shapefile.Read(file), .98);

        //    IRI.Ket.ShapefileFormat.Writer.ShpWriter.Write(output, result, true);

        //    result = Utilities.SimplifyAlgorithms.AdditiveSimplifyByArea(result, 13);
        //    output = @"E:\Data\0. Test\Large Data\94.04.18\TehranBlocksAddiArea13AfterAddiAngle.shp";
        //    IRI.Ket.ShapefileFormat.Writer.ShpWriter.Write(output, result, true);

        //}

        private void GeneralizeByAreaSimple_Click(object sender, RoutedEventArgs e)
        {
            var fileName = this.SelectShapefile();

            if (string.IsNullOrEmpty(fileName))
                return;

            var shapes = GetShapes(fileName);

            var result = Utilities.SimplifyAlgorithms.SimplifyByArea(shapes, Presenter.AreaThreshold);

            var unitDistance = WebMercatorUtility.CalculateGroundResolution(Presenter.ZoomLevel, 35);

            string output;

            if (unitDistance * unitDistance - Presenter.AreaThreshold < .001)
            {
                output = TodayDirectory + System.IO.Path.GetFileNameWithoutExtension(fileName) + "AreaSimple" + Presenter.ZoomLevel + ".shp";
            }
            else
            {
                output = TodayDirectory + System.IO.Path.GetFileNameWithoutExtension(fileName) + "AreaSimple" + Presenter.AreaThreshold + ".shp";
            }

            IRI.Ket.ShapefileFormat.Shapefile.Save(output, result, true);
        }

        private void GeneralizeByAreaSimpleIterative_Click(object sender, RoutedEventArgs e)
        {

        }

        private void GeneralizeByAreaAdditive_Click(object sender, RoutedEventArgs e)
        {
            var fileName = this.SelectShapefile();

            if (string.IsNullOrEmpty(fileName))
                return;

            var shapes = GetShapes(fileName);

            var result = Utilities.SimplifyAlgorithms.AdditiveSimplifyByArea(shapes, Presenter.AreaThreshold);

            var unitDistance = WebMercatorUtility.CalculateGroundResolution(Presenter.ZoomLevel, 35);

            string output;

            if (unitDistance * unitDistance - Presenter.AreaThreshold < .001)
            {
                output = TodayDirectory + System.IO.Path.GetFileNameWithoutExtension(fileName) + "AreaAdditive" + Presenter.ZoomLevel + ".shp";
            }
            else
            {
                output = TodayDirectory + System.IO.Path.GetFileNameWithoutExtension(fileName) + "AreaAdditive" + Presenter.AreaThreshold + ".shp";
            }

            IRI.Ket.ShapefileFormat.Shapefile.Save(output, result, true);
        }

        private void GeneralizeByAreaAdditivePlus_Click(object sender, RoutedEventArgs e)
        {
            var fileName = this.SelectShapefile();

            if (string.IsNullOrEmpty(fileName))
                return;

            var shapes = GetShapes(fileName);

            var result = Utilities.SimplifyAlgorithms.AdditiveSimplifyByAreaPlus(shapes, Presenter.AreaThreshold);

            var unitDistance = WebMercatorUtility.CalculateGroundResolution(Presenter.ZoomLevel, 35);

            string output;

            if (unitDistance * unitDistance - Presenter.AreaThreshold < .001)
            {
                output = TodayDirectory + System.IO.Path.GetFileNameWithoutExtension(fileName) + "AreaAdditivePlus" + Presenter.ZoomLevel + ".shp";
            }
            else
            {
                output = TodayDirectory + System.IO.Path.GetFileNameWithoutExtension(fileName) + "AreaAdditivePlus" + Presenter.AreaThreshold + ".shp";
            }

            IRI.Ket.ShapefileFormat.Shapefile.Save(output, result, true);
        }

        private void GeneralizeByAngleSimple_Click(object sender, RoutedEventArgs e)
        {
            var fileName = this.SelectShapefile();

            if (string.IsNullOrEmpty(fileName))
                return;

            var shapes = GetShapes(fileName);

            double threshold = Presenter.AngleThreshold;

            var result = Utilities.SimplifyAlgorithms.SimplifyByAngle(shapes, threshold);

            var output = TodayDirectory + System.IO.Path.GetFileNameWithoutExtension(fileName) + "AngleSimple" + threshold + ".shp";

            IRI.Ket.ShapefileFormat.Shapefile.Save(output, result, true);
        }

        private void GeneralizeByAngleAdditive_Click(object sender, RoutedEventArgs e)
        {
            var fileName = this.SelectShapefile();

            if (string.IsNullOrEmpty(fileName))
                return;

            var shapes = GetShapes(fileName);

            double threshold = Presenter.AngleThreshold;

            var result = Utilities.SimplifyAlgorithms.AdditiveSimplifyByAngle(shapes, threshold);

            var output = TodayDirectory + System.IO.Path.GetFileNameWithoutExtension(fileName) + "AngleAdditive" + threshold + ".shp";

            IRI.Ket.ShapefileFormat.Shapefile.Save(output, result, true);
        }


        private string SelectShapefile()
        {
            var dialog = new Microsoft.Win32.OpenFileDialog() { Filter = "*.shp|*.shp", Multiselect = false };

            if (dialog.ShowDialog() == true)
            {
                return dialog.FileName;
            }
            else
            {
                return string.Empty;
            }
        }

        private IEsriShapeCollection GetShapes(string fileName)
        {
            var shapes = IRI.Ket.ShapefileFormat.Shapefile.Read(fileName);

            var width = Math.Max(shapes.MainHeader.MinimumBoundingBox.Width, shapes.MainHeader.MinimumBoundingBox.Height);

            //this.Presenter.EstimatedScale = WebMercatorUtility.EstimateMapScale(width, 30, 900);

            var z1 = WebMercatorUtility.GetZoomLevel(width, 30, 900);

            var z2 = WebMercatorUtility.GetZoomLevel(width, 30, 900);

            Trace.WriteLine($"Z1: {z1}");

            Trace.WriteLine($"Z2: {z2}");

            return shapes;
        }

        private void GetFileScale_Click(object sender, RoutedEventArgs e)
        {
            var fileName = SelectShapefile();

            if (string.IsNullOrEmpty(fileName))
                return;

            GetShapes(fileName);


        }

        private void sTEnvelope_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
