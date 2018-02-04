using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using IRI.Ham.SpatialBase;
using Microsoft.SqlServer.Types;
using System.Windows.Media.Imaging;
using IRI.Jab.Common;
using System.Windows.Data;
using System.Windows.Shapes;
using IRI.Jab.Cartography.Model;
using System.Threading.Tasks;
using IRI.Jab.Common.Extensions;
using IRI.Ket.DataManagement.DataSource;
using IRI.Ket.DataManagement.Model;
using IRI.Jab.Common.Model;

namespace IRI.Jab.Cartography
{
    public class RasterLayer : BaseLayer
    {
        RasterLayer _parent;

        //private ScaleInterval _visibleRange;

        //public ScaleInterval VisibleRange
        //{
        //    get { return _visibleRange; }
        //    set
        //    {
        //        _visibleRange = value;
        //        RaisePropertyChanged();
        //    }
        //}

        public IDataSource DataSource { get; private set; }

        private FrameworkElement frameworkElement;

        public FrameworkElement Element
        {
            get { return this.frameworkElement; }

            set
            {
                this.frameworkElement = value;

                this.BindWithFrameworkElement(value);

                RaisePropertyChanged();
            }
        }

        //public Guid Id { get; private set; }

        //private string _layerName;

        //public string LayerName
        //{
        //    get { return _layerName; }
        //    set
        //    {
        //        _layerName = value;
        //        RaisePropertyChanged();
        //    }
        //}

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

        //public int ZIndex { get; set; }

        //private VisualParameters _visualParameters;

        //public VisualParameters VisualParameters
        //{
        //    get { return _visualParameters; }
        //    set
        //    {
        //        _visualParameters = value;
        //        RaisePropertyChanged();
        //    }
        //}

        private BoundingBox _extent;

        public override BoundingBox Extent
        {
            get { return _extent; }
            protected set
            {
                _extent = value;
                RaisePropertyChanged();
            }
        }

        public override RenderingApproach Rendering { get; protected set; }

        //public RasterizationApproach ToRasterTechnique { get { return RasterizationApproach.None; } }


        //public bool IsValid { get; set; }

        //public void Invalidate() => IsValid = false;

        public BitmapImage Image { get; set; }

        public void BindWithFrameworkElement(FrameworkElement element)
        {
            if (element is Path || element is Rectangle)
            {
                Binding binding1 = new Binding() { Source = this._parent, Path = new PropertyPath("VisualParameters.Stroke"), Mode = BindingMode.TwoWay };
                element.SetBinding(Path.StrokeProperty, binding1);

                //If uncomment no DrawingBrush will work any more for raster layers
                //Binding binding2 = new Binding() { Source = this._parent, Path = new PropertyPath("VisualParameters.Fill"), Mode = BindingMode.TwoWay };
                //element.SetBinding(Path.FillProperty, binding2);

                Binding binding3 = new Binding() { Source = this._parent, Path = new PropertyPath("VisualParameters.StrokeThickness"), Mode = BindingMode.TwoWay };
                element.SetBinding(Path.StrokeThicknessProperty, binding3);

                Binding binding4 = new Binding() { Source = this._parent, Path = new PropertyPath("VisualParameters.Visibility"), Mode = BindingMode.TwoWay };
                element.SetBinding(Path.VisibilityProperty, binding4);

                Binding binding5 = new Binding() { Source = this._parent, Path = new PropertyPath("VisualParameters.Opacity"), Mode = BindingMode.TwoWay };
                element.SetBinding(Path.OpacityProperty, binding5);
            }
            //else if (element is System.Windows.Controls.Image)
            //{
            //    Binding binding4 = new Binding() { Source = this._parent, Path = new PropertyPath("VisualParameters.Visibility"), Mode = BindingMode.TwoWay };
            //    element.SetBinding(Path.VisibilityProperty, binding4);

            //    Binding binding5 = new Binding() { Source = this._parent, Path = new PropertyPath("VisualParameters.Opacity"), Mode = BindingMode.TwoWay };
            //    element.SetBinding(Path.OpacityProperty, binding5);
            //}
            else
            {
                throw new NotImplementedException();
            }
        }

        private async Task<List<RasterLayer>> GetRasterLayer(BoundingBox region, double mapScale, double unitDistance)
        {
            List<RasterLayer> result = new List<RasterLayer>();

            if (this.DataSource.GetType() == typeof(OfflineGoogleMapDataSource<object>))
            {
                var googleDataSource = this.DataSource as OfflineGoogleMapDataSource<object>;

                var tiles = googleDataSource.GetTiles(region.Transform(Ham.CoordinateSystem.MapProjection.MapProjects.GeodeticWgs84ToWebMercator), mapScale);

                foreach (var item in tiles)
                {
                    var boundingBox = item.GeodeticWgs84BoundingBox.Transform(Ham.CoordinateSystem.MapProjection.MapProjects.GeodeticWgs84ToWebMercator);

                    //94.12.16
                    //int width = (int)(boundingBox.Width * mapScale / unitDistance);

                    //int height = (int)(boundingBox.Height * mapScale / unitDistance);

                    RasterLayer layer =
                        new RasterLayer(this, this.LayerName, Common.Helpers.ImageUtility.ToImage(item.Image), this.VisualParameters.Opacity,
                            boundingBox, this.Type == LayerType.BaseMap, this.Type == LayerType.ImagePyramid);

                    result.Add(layer);
                }
            }
            else if (this.DataSource.GetType() == typeof(ZippedImagePyramidDataSource))
            {
                var pyramidDataSource = this.DataSource as ZippedImagePyramidDataSource;

                var tiles = pyramidDataSource.GetTiles(region.Transform(Ham.CoordinateSystem.MapProjection.MapProjects.WebMercatorToGeodeticWgs84), mapScale);

                foreach (var item in tiles)
                {
                    var boundingBox = item.GeodeticWgs84BoundingBox.Transform(Ham.CoordinateSystem.MapProjection.MapProjects.GeodeticWgs84ToWebMercator);

                    RasterLayer layer =
                        new RasterLayer(this, this.LayerName, Common.Helpers.ImageUtility.ToImage(item.Image), this.VisualParameters.Opacity,
                            boundingBox, this.Type == LayerType.BaseMap, this.Type == LayerType.ImagePyramid);

                    result.Add(layer);
                }
            }
            else if (this.DataSource.GetType() == typeof(OnlineGoogleMapDataSource<object>))
            {
                var googleDataSource = this.DataSource as OnlineGoogleMapDataSource<object>;

                var tiles = await googleDataSource.GetTiles(region, mapScale);

                foreach (var item in tiles)
                {
                    var boundingBox = item.Item2.GeodeticWgs84BoundingBox.Transform(Ham.CoordinateSystem.MapProjection.MapProjects.GeodeticWgs84ToWebMercator);

                    //94.12.16
                    //int width = (int)(boundingBox.Width * mapScale / unitDistance);

                    //int height = (int)(boundingBox.Height * mapScale / unitDistance);

                    RasterLayer layer =
                        new RasterLayer(this, this.LayerName, Common.Helpers.ImageUtility.ToImage(item.Item2.Image), this.VisualParameters.Opacity,
                            boundingBox, true);

                    result.Add(layer);
                }

            }
            else if (this.DataSource.GetType() == typeof(GeoRasterFileDataSource))
            {
                //95.01.18
                //var geodeticBoundingBox = region.Transform(i => Ham.CoordinateSystem.Projection.MercatorToGeodetic(i));

                //var geo = (this.DataSource as GeoRasterFileDataSource).Get(geodeticBoundingBox);

                var geo = (this.DataSource as GeoRasterFileDataSource).Get(region);

                if (geo != null)
                {
                    var boundingBox = geo.GeodeticWgs84BoundingBox.Transform(Ham.CoordinateSystem.MapProjection.MapProjects.GeodeticWgs84ToWebMercator);

                    //94.12.16
                    //int width = (int)(boundingBox.Width * mapScale / unitDistance);

                    //int height = (int)(boundingBox.Height * mapScale / unitDistance);

                    RasterLayer layer = new RasterLayer(this,
                                                this.LayerName, Common.Helpers.ImageUtility.ToImage(geo.Image),
                                                this.VisualParameters.Opacity, boundingBox, false);

                    result.Add(layer);
                }
            }
            else
            {
                string whereClouse =
                  string.Format(System.Globalization.CultureInfo.InvariantCulture, " ImageMinX < {0} AND ImageMaxX > {2} AND ImageMinY < {1} AND ImageMaxY > {3}",
                      region.XMax,
                      region.YMax,
                      region.XMin,
                      region.YMin);

                //??
                var table = await ((IRI.Ket.DataManagement.Model.IFeatureDataSource)this.DataSource).GetEntireFeatureAsync(whereClouse);

                foreach (System.Data.DataRow item in table.Rows)
                {
                    if (item["Image"] == DBNull.Value)
                    {
                        continue;
                    }

                    var boundingBox = new BoundingBox(xMin: double.Parse(item["ImageMinX"].ToString()),
                                                        yMin: double.Parse(item["ImageMinY"].ToString()),
                                                        xMax: double.Parse(item["ImageMaxX"].ToString()),
                                                        yMax: double.Parse(item["ImageMaxY"].ToString()));

                    //94.12.16
                    //int width = (int)(boundingBox.Width * mapScale / unitDistance);

                    //int height = (int)(boundingBox.Width * mapScale / unitDistance);

                    RasterLayer layer =
                        new RasterLayer(this, this.LayerName,
                                            Common.Helpers.ImageUtility.ToImage((byte[])item["Image"]),
                                            this.VisualParameters.Opacity,
                                            boundingBox,
                                            this.Type == LayerType.BaseMap);

                    result.Add(layer);
                }
            }

            return result;

        }

        public RasterLayer(RasterLayer parent, string name, BitmapImage image, double opacity, BoundingBox boundingBox, bool isBaseMap, bool isPyramid = false, RenderingApproach rendering = RenderingApproach.Default)
        {

            this.Id = Guid.NewGuid();

            this._parent = parent;

            this._type = isBaseMap ? LayerType.BaseMap : (isPyramid ? LayerType.ImagePyramid : LayerType.Raster);

            this.Rendering = rendering;

            this.LayerName = name;

            this._extent = boundingBox;

            this.Image = image;

            //this.VisualParameters = new VisualParameters(new ImageBrush(image), isBaseMap ? null : Brushes.Black, isBaseMap ? 0 : 1, opacity);
        }

        public RasterLayer(IDataSource dataSource, string layerName, ScaleInterval visibleRange, bool isBaseMap, bool isPyramid, Visibility visibility, double opacity, RenderingApproach rendering = RenderingApproach.Default)
        {
            this.Id = Guid.NewGuid();

            this._type = isBaseMap ? LayerType.BaseMap : (isPyramid ? LayerType.ImagePyramid : LayerType.Raster);

            this.DataSource = dataSource;

            this.LayerName = layerName;

            this.VisibleRange = visibleRange;

            //this.VisualParameters = new VisualParameters(null, isBaseMap ? null : Brushes.Black, isBaseMap ? 0 : 1, opacity);
            this.VisualParameters = new VisualParameters(null, null, 0, opacity, visibility);
        }


        public async Task<List<Path>> ParseToPath(BoundingBox boundingBox, Transform viewTransform, double mapScale, double unitDistance)
        {
            List<RasterLayer> layers = await GetRasterLayer(boundingBox, mapScale, unitDistance);

            var result = new List<Path>();

            foreach (var item in layers)
            {
                System.Windows.Point topLeft = item.Extent.TopLeft.AsWpfPoint();

                System.Windows.Point bottomRigth = item.Extent.BottomRight.AsWpfPoint();

                RectangleGeometry geometry = new RectangleGeometry(new Rect(topLeft, bottomRigth), 0, 0);

                geometry.Transform = viewTransform;

                Path path = new Path()
                {
                    Fill = new ImageBrush(item.Image),
                    Data = geometry,
                    Tag = new LayerTag(mapScale) { Layer = item, IsDrawn = true, BoundingBox = item.Extent }
                };

                item.Element = path;

                result.Add(path);
            }

            return result;
        }

        //public void CloseConnection()
        //{
        //    this.DataSource.CloseConnection();
        //}
    }
}
