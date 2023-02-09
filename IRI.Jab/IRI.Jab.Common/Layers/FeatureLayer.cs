using IRI.Jab.Common.Convertor;
using IRI.Extensions;
using IRI.Jab.Common.Model;
using IRI.Ket.DataManagement.DataSource;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using sb = IRI.Msh.Common.Primitives;

namespace IRI.Jab.Common
{
    public class FeatureLayer : BaseLayer
    {
        public SqlFeatureDataSource DataSource { get; protected set; }

        public Func<sb.Feature<sb.Point>, VisualParameters> SymbologyRule { get; set; }

        private LayerType _type;

        public override LayerType Type
        {
            get { return _type; }
            protected set
            {
                _type = value;
                RaisePropertyChanged();
            }
        }

        private FrameworkElement _element;

        public FrameworkElement Element
        {
            get { return this._element; }

            set
            {
                this._element = value;

                BindWithFrameworkElement(value);

                RaisePropertyChanged();
            }
        }

        private sb.BoundingBox _extent;

        public override sb.BoundingBox Extent
        {
            get { return _extent; }
            protected set
            {
                _extent = value;
                RaisePropertyChanged();
            }
        }

        public override RenderingApproach Rendering { get; protected set; }

        public override RasterizationApproach ToRasterTechnique { get; protected set; }

        public void BindWithFrameworkElement(FrameworkElement element)
        {
            Binding binding4 = new Binding() { Source = this, Path = new PropertyPath("VisualParameters.Visibility"), Mode = BindingMode.TwoWay };
            //Binding binding4 = new Binding() { Source = this, Path = new PropertyPath(nameof(VisualParameters.Visibility)), Mode = BindingMode.TwoWay }; try using this line insted of above
            element.SetBinding(Path.VisibilityProperty, binding4);

            Binding binding5 = new Binding() { Source = this, Path = new PropertyPath("VisualParameters.Opacity"), Mode = BindingMode.TwoWay };
            element.SetBinding(Path.OpacityProperty, binding5);
        }

        public Path AsBitmapUsingGdiPlus(List<sb.Feature<sb.Point>> features, List<string> labels, double mapScale, sb.BoundingBox boundingBox, double width, double height, Func<Point, Point> mapToScreen, RectangleGeometry area)
        {
            if (features == null)
                return null;
             
            var image = SqlFeatureToGdiBitmap.ParseSqlGeometry(
                features,
                width,
                height,
                mapToScreen,
                this.SymbologyRule);


            if (image == null)
                return null;
             
            BitmapImage bitmapImage = Helpers.ImageUtility.AsBitmapImage(image, System.Drawing.Imaging.ImageFormat.Png);

            image.Dispose();

            Path path = new Path()
            {
                Data = area,
                Tag = new Model.LayerTag(mapScale) { Layer = this, Tile = null, IsDrawn = true, IsNew = true }
            };

            this.Element = path;

            path.Fill = new ImageBrush(bitmapImage);

            bitmapImage.Freeze();

            return path;
        }

        #region Constructors

        internal FeatureLayer()
        {

        }

        public FeatureLayer(string name, List<sb.Feature<sb.Point>> features, Func<sb.Feature<sb.Point>, VisualParameters> symbologyRule,
                            RenderingApproach rendering, RasterizationApproach toRasterTechnique, ScaleInterval visibleRange)
        {
            if (features == null || features.Count == 0)
                throw new NotImplementedException();

            Initialize(name, new SqlFeatureDataSource(features), rendering, toRasterTechnique, visibleRange, symbologyRule);
        }

        public FeatureLayer(string layerName, SqlFeatureDataSource dataSource, RenderingApproach rendering,
                            RasterizationApproach toRasterTechnique, Func<sb.Feature<sb.Point>, VisualParameters> symbologyRule, ScaleInterval visibleRange)
        {
            Initialize(layerName, dataSource, rendering, toRasterTechnique, visibleRange, symbologyRule);
        }

        private void Initialize(string layerName, SqlFeatureDataSource dataSource, RenderingApproach rendering,
            RasterizationApproach toRasterTechnique, ScaleInterval visibleRange, Func<sb.Feature<sb.Point>, VisualParameters> symbologyRule)
        {
            this.LayerId = Guid.NewGuid();

            this.DataSource = dataSource;

            this.Rendering = rendering;

            this.ToRasterTechnique = toRasterTechnique;

            var geometries = dataSource.GetGeometries();

            this.Type = LayerType.Feature | LayerType.FeatureLayer;

            //if (geometries?.Count > 0)
            //{
            //    this.Type = type | GetGeometryType(geometries.FirstOrDefault(g => g != null));
            //}
            //else
            //{
            //    this.Type = type;
            //}

            this.Extent = DataSource?.GetGeometries()?.GetBoundingBox() ?? sb.BoundingBox.NaN;

            this.LayerName = layerName;

            this.VisualParameters = new VisualParameters(null, null, 0, 1, Visibility.Visible);

            this.SymbologyRule = symbologyRule;

            //this.PointSymbol = pointSymbol ?? new SimplePointSymbol() { SymbolWidth = 4, SymbolHeight = 4 };

            //this.Labels = labeling;

            ////Check for missing visibleRange
            //if (this.Labels != null)
            //{
            //    if (this.Labels.VisibleRange == null)
            //    {
            //        this.Labels.VisibleRange = visibleRange;
            //    }
            //}

            this.VisibleRange = (visibleRange == null) ? ScaleInterval.All : visibleRange;

        }

        #endregion
    }
}
