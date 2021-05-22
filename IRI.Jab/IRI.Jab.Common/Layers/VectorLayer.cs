using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

using System.Threading.Tasks;
using Microsoft.SqlServer.Types;

using sb = IRI.Msh.Common.Primitives;

using IRI.Jab.Common.Model;
using IRI.Jab.Common.Extensions;
using IRI.Jab.Common.Convertor;
using IRI.Ket.DataManagement.DataSource;
using IRI.Ket.DataManagement.Model;
using IRI.Msh.Common.Mapping;
using IRI.Msh.Common.Model;
using IRI.Ket.SpatialExtensions;
using IRI.Jab.Common.Model.Symbology;
using IRI.Jab.Common.Helpers;
using IRI.Ket.SqlServerSpatialExtension.Model;

namespace IRI.Jab.Common
{
    public class VectorLayer : BaseLayer
    {
        #region Properties, Fields

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

        public FeatureDataSource DataSource { get; protected set; }

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

        //public Guid Id { get; private set; }

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

        //private SimplePointSymbol _pointSymbol = new SimplePointSymbol() { SymbolWidth = 4, SymbolHeight = 4 };

        //public SimplePointSymbol PointSymbol
        //{
        //    get { return _pointSymbol; }
        //    set
        //    {
        //        _pointSymbol = value;
        //        RaisePropertyChanged();
        //    }
        //}


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

        //public bool IsValid { get; set; }

        //public void Invalidate() => IsValid = false;

        public TileManager TileManager = new TileManager();

        #endregion

        #region Constructors

        internal VectorLayer()
        {

        }

        public VectorLayer(string name, List<SqlGeometry> features, LayerType type, RenderingApproach rendering, RasterizationApproach toRasterTechnique)
            : this(name, features, new VisualParameters(BrushHelper.PickBrush(), BrushHelper.PickBrush(), 1, 1, Visibility.Visible), type, rendering, toRasterTechnique)
        {

        }

        public VectorLayer(string layerName, List<SqlGeometry> features, VisualParameters parameters, LayerType type, RenderingApproach rendering, RasterizationApproach toRasterTechnique)
        {
            if (features == null || features.Count == 0)
                throw new NotImplementedException();

            Initialize(layerName, new MemoryDataSource(features), parameters, type, rendering, toRasterTechnique, ScaleInterval.All, null, null);
        }

        public VectorLayer(string layerName, FeatureDataSource dataSource, VisualParameters parameters, LayerType type, RenderingApproach rendering,
            RasterizationApproach toRasterTechnique, ScaleInterval visibleRange, SimplePointSymbol pointSymbol = null, LabelParameters labeling = null)
        {
            Initialize(layerName, dataSource, parameters, type, rendering, toRasterTechnique, visibleRange, pointSymbol, labeling);
        }

        private void Initialize(string layerName, FeatureDataSource dataSource, VisualParameters parameters, LayerType type, RenderingApproach rendering,
                                    RasterizationApproach toRasterTechnique, ScaleInterval visibleRange,
                                    SimplePointSymbol pointSymbol, LabelParameters labeling)
        {
            this.LayerId = Guid.NewGuid();

            this.DataSource = dataSource;

            this.Rendering = rendering;

            this.ToRasterTechnique = toRasterTechnique;

            var geometries = dataSource.GetGeometries();

            if (geometries?.Count > 0)
            {
                this.Type = type | GetGeometryType(geometries.FirstOrDefault(g => g != null));
            }
            else
            {
                this.Type = type;
            }

            this.Extent = geometries?.GetBoundingBox() ?? sb.BoundingBox.NaN;

            this.LayerName = layerName;

            this.VisualParameters = parameters;

            //this.PointSymbol = pointSymbol ?? new SimplePointSymbol() { SymbolWidth = 4, SymbolHeight = 4 };

            this.Labels = labeling;

            //Check for missing visibleRange
            if (this.Labels != null)
            {
                if (this.Labels.VisibleRange == null)
                {
                    this.Labels.VisibleRange = visibleRange;
                }
            }

            this.VisibleRange = (visibleRange == null) ? ScaleInterval.All : visibleRange;

        }

        #endregion



        #region Default Rendering
        //StreamGeometry Approach
        public Path AsShape(List<SqlGeometry> geometries, double mapScale, sb.BoundingBox exactCurrentExtent, double width,
            double height, TransformGroup viewTransform, TranslateTransform viewTransformForPoints, Func<Point, Point> mapToScreen)
        {
            StreamGeometry geo;

            if (this.Type.HasFlag(LayerType.Point))
            {
                geo = SqlSpatialToStreamGeometry.ParseSqlGeometry(geometries, mapToScreen, this.VisualParameters.PointSymbol.GeometryPointSymbol);

                geo.FillRule = FillRule.Nonzero;

                geo.Transform = viewTransformForPoints;
            }
            else
            {
                geo = SqlSpatialToStreamGeometry.ParseSqlGeometry(geometries, p => p);

                geo.Transform = viewTransform;
            }

            //GeometryDrawing drawing = new GeometryDrawing();

            Path path = new Path()
            {
                StrokeDashArray = VisualParameters.DashType,
                Data = geo,
                Tag = new LayerTag(-1) { Layer = this, IsTiled = false },
                Stroke = VisualParameters.Stroke,
                Fill = VisualParameters.Fill,
                StrokeThickness = VisualParameters.StrokeThickness
            };

            this.Element = path;

            return path;
        }

        //DrawingVisual Approach
        public Path AsDrawingVisual(List<SqlGeometry> geometries, List<string> labels, double mapScale, sb.BoundingBox exactCurrentExtent, double width, double height, Func<Point, Point> mapToScreen, RectangleGeometry area)
        {
            if (geometries == null)
                return null;

            //Pen pen = new Pen(this.VisualParameters.Stroke, this.VisualParameters.StrokeThickness);

            //pen.DashStyle = this.VisualParameters.DashStyle;

            var pen = this.VisualParameters.GetWpfPen();

            Brush brush = this.VisualParameters.Fill;

            DrawingVisual drawingVisual = new SqlSpatialToDrawingVisual().ParseSqlGeometry(geometries, i => mapToScreen(i), pen, brush, this.VisualParameters.PointSymbol);

            RenderTargetBitmap image = new RenderTargetBitmap((int)width, (int)height, 96, 96, PixelFormats.Pbgra32);

            image.Render(drawingVisual);

            if (this.IsLabeled(mapScale))
            {
                this.DrawLabels(labels, geometries, image, mapToScreen);
            }

            image.Freeze();

            Path path = new Path()
            {
                Data = area,
                Tag = new LayerTag(mapScale) { Layer = this, IsTiled = false },
            };

            this.Element = path;

            path.Fill = new ImageBrush(image);

            return path;
        }

        //Gdi+
        public Path AsBitmapUsingGdiPlus(List<SqlGeometry> geometries, List<string> labels, double mapScale, sb.BoundingBox boundingBox, double width, double height, Func<Point, Point> mapToScreen, RectangleGeometry area)
        {
            if (geometries == null)
                return null;

            var borderBrush = this.VisualParameters.Stroke.AsGdiBrush();

            var pen = this.VisualParameters.GetGdiPlusPen();

            var image = SqlSpatialToGdiBitmap.ParseSqlGeometry(
                geometries,
                width,
                height,
                mapToScreen,
                pen,
                this.VisualParameters.Fill.AsGdiBrush(),
                this.VisualParameters.PointSymbol);

            if (image == null)
                return null;

            if (labels != null)
            {
                SqlSpatialToGdiBitmap.DrawLabels(labels, geometries, image, mapToScreen, this.Labels);
            }

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

        //Consider Labels
        public Path AsBitmapUsingWriteableBitmap(List<SqlGeometry> geometries, List<string> labels, double mapScale, sb.BoundingBox boundingBox, double width, double height, Func<Point, Point> mapToScreen, RectangleGeometry area)
        {
            if (geometries == null)
                return null;

            var image = new SqlSpatialToWriteableBitmap().ParseSqlGeometry(
                                geometries,
                                mapToScreen,
                                (int)width,
                                (int)height,
                                this.VisualParameters.Stroke.AsSolidColor().Value,
                                this.VisualParameters.Fill.AsSolidColor().Value);

            if (image == null)
                return null;

            if (labels != null)
            {
                //this.DrawLabel(labels, geometries, image, transform);
            }

            //BitmapImage bitmapImage = IRI.Jab.Common.Imaging.ImageUtility.AsBitmapImage(image);

            //Try #3
            Path path = new Path()
            {
                Data = area,
                Tag = new LayerTag(mapScale) { Layer = this, Tile = null, IsDrawn = true, IsNew = true }
            };

            this.Element = path;

            path.Fill = new ImageBrush(image);

            return path;
        }

        //OpenTK
        public Path AsBitmapUsingOpenTK(List<SqlGeometry> geometries, List<string> labels, double mapScale, sb.BoundingBox boundingBox, double width, double height, Func<Point, Point> mapToScreen, RectangleGeometry area)
        {
            if (geometries == null)
                return null;

            //Pen pen = new Pen(this.VisualParameters.Stroke, this.VisualParameters.StrokeThickness);
            var pen = this.VisualParameters.GetGdiPlusPen();

            Brush brush = this.VisualParameters.Fill;

            //var color = ((SolidColorBrush)this.VisualParameters.Stroke)?.Color ?? ((SolidColorBrush)this.VisualParameters.Fill).Color;

            //var image = new SqlSpatialToOpenTKBitmap().ParseSqlGeometry(
            //    geometries,
            //    width,
            //    height,
            //    mapToScreen,
            //    new System.Drawing.Pen(System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B), (int)this.VisualParameters.StrokeThickness),
            //    System.Drawing.Brushes.SkyBlue);

            var image = new SqlSpatialToOpenTKBitmap().ParseSqlGeometry(
                geometries,
                width,
                height,
                mapToScreen,
                pen,
                System.Drawing.Brushes.SkyBlue);

            if (image == null)
                return null;

            if (labels != null)
            {
                SqlSpatialToGdiBitmap.DrawLabels(labels, geometries, image, mapToScreen, Labels);
            }

            BitmapImage bitmapImage = IRI.Jab.Common.Helpers.ImageUtility.AsBitmapImage(image, System.Drawing.Imaging.ImageFormat.Png);

            //Try #3
            Path path = new Path()
            {
                Data = area,
                Tag = new Model.LayerTag(mapScale) { Layer = this, Tile = null, IsDrawn = true, IsNew = true }
            };

            this.Element = path;

            path.Fill = new ImageBrush(bitmapImage);


            return path;
        }

        #endregion


        #region Tile Rendering

        //DrawingVisual Approach
        public Path AsTileUsingDrawingVisual(List<SqlGeometry> geometries, List<string> labels, double mapScale, TileInfo region, double tileWidth, double tileHeight, RectangleGeometry area, Func<Point, Point> viewTransform, sb.BoundingBox totalExtent)
        {
            if (geometries == null)
                return null;

            //Pen pen = new Pen(this.VisualParameters.Stroke, this.VisualParameters.StrokeThickness);

            //if (this.VisualParameters.DashStyle != null)
            //{
            //    pen.DashStyle = this.VisualParameters.DashStyle;
            //}
            var pen = this.VisualParameters.GetWpfPen();

            var shiftX = region.WebMercatorExtent.Center.X - totalExtent.TopLeft.X - region.WebMercatorExtent.Width / 2.0;
            var shiftY = region.WebMercatorExtent.Center.Y - totalExtent.TopLeft.Y + region.WebMercatorExtent.Height / 2.0;

            Brush brush = this.VisualParameters.Fill;

            //var transform = MapToTileScreenWpf(totalExtent, region.WebMercatorExtent, viewTransform);

            var drawingVisual = new SqlSpatialToDrawingVisual().ParseSqlGeometry(
                                    geometries,
                                    p => viewTransform(new Point(p.X - shiftX, p.Y - shiftY)),
                                    pen,
                                    brush,
                                    this.VisualParameters.PointSymbol);

            RenderTargetBitmap image = new RenderTargetBitmap((int)tileWidth, (int)tileHeight, 96, 96, PixelFormats.Pbgra32);

            image.Render(drawingVisual);

            if (labels != null)
            {
                //this.DrawLabels(labels, geometries, image, transform);
                this.DrawLabels(labels, geometries, image, p => viewTransform(new Point(p.X - shiftX, p.Y - shiftY)));
            }

            image.Freeze();

            Path path = new Path()
            {
                Data = area,
                Tag = new LayerTag(mapScale) { Layer = this, IsTiled = true, Tile = new TileInfo(region.RowNumber, region.ColumnNumber, region.ZoomLevel), IsDrawn = true, IsNew = true }
            };

            this.Element = path;

            path.Fill = new ImageBrush(image);

            return path;
        }

        //Gdi+ Approach
        public Path AsTileUsingGdiPlusAsync(List<SqlGeometry> geometries, List<string> labels, double mapScale, TileInfo region, double tileWidth, double tileHeight, RectangleGeometry area, Func<Point, Point> viewTransform, sb.BoundingBox totalExtent)
        {
            if (geometries == null)
                return null;

            //var pen = this.VisualParameters.Stroke != null ? new System.Drawing.Pen(this.VisualParameters.Stroke.AsGdiBrush(), (int)this.VisualParameters.StrokeThickness) : null;

            //if (this.VisualParameters.DashStyle != null)
            //{
            //    pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            //}

            var pen = this.VisualParameters.GetGdiPlusPen();

            var shiftX = region.WebMercatorExtent.Center.X - totalExtent.TopLeft.X - region.WebMercatorExtent.Width / 2.0;
            var shiftY = region.WebMercatorExtent.Center.Y - totalExtent.TopLeft.Y + region.WebMercatorExtent.Height / 2.0;
            //;

            //var transform = MapToTileScreenWpf(totalExtent, region.MercatorExtent, viewTransform);
            //var mapShift = new Point(region.MercatorExtent.Center.X - totalExtent.TopLeft.X, region.MercatorExtent.Center.Y - totalExtent.BottomRight.Y);
            //var mapShift = (mapBoundingBoxOfTile.Center - new sb.Point(totalExtent.TopLeft.X + mapBoundingBoxOfTile.Width / 2.0, totalExtent.TopLeft.Y - mapBoundingBoxOfTile.Height / 2.0)).AsWpfPoint();


            var image = SqlSpatialToGdiBitmap.ParseSqlGeometry(
                            geometries,
                            tileWidth,
                            tileHeight,
                            //transform,
                            p => viewTransform(new Point(p.X - shiftX, p.Y - shiftY)),
                            pen,
                            this.VisualParameters.Fill.AsGdiBrush(),
                            this.VisualParameters.PointSymbol);

            if (image == null)
                return null;

            if (labels != null)
            {
                //96.05.19
                //SqlSpatialToGdiBitmap.DrawLabels(labels, geometries, image, transform, Labels);
            }

            var bitmapImage = IRI.Jab.Common.Helpers.ImageUtility.AsBitmapImage(image, System.Drawing.Imaging.ImageFormat.Png);

            image.Dispose();

            Path path = new Path()
            {
                Data = area,
                Tag = new LayerTag(mapScale) { Layer = this, IsTiled = true, Tile = new TileInfo(region.RowNumber, region.ColumnNumber, region.ZoomLevel), IsDrawn = true, IsNew = true }
            };

            this.Element = path;

            path.Fill = new ImageBrush(bitmapImage);

            return path;
        }

        //Writeable Bitmap Approach
        //Consider Labeling
        public Path AsTileUsingWriteableBitmap(List<SqlGeometry> geometries, List<string> labels, double mapScale, TileInfo region, double tileWidth, double tileHeight, RectangleGeometry area, Func<Point, Point> viewTransform, sb.BoundingBox totalExtent)
        {
            if (geometries == null)
            {
                return null;
            }

            Brush brush = this.VisualParameters.Fill;

            var color = ((SolidColorBrush)this.VisualParameters.Stroke)?.Color ?? ((SolidColorBrush)this.VisualParameters.Fill).Color;

            //var pen = new System.Drawing.Pen(System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B), (int)this.VisualParameters.StrokeThickness);

            //if (this.VisualParameters.DashStyle != null)
            //{
            //    pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            //}

            var transform = MapToTileScreenWpf(totalExtent, region.WebMercatorExtent, viewTransform);

            var image = new SqlSpatialToWriteableBitmap().ParseSqlGeometry(
                geometries,
                transform,
                (int)tileWidth,
                (int)tileHeight,
                this.VisualParameters.Stroke.AsSolidColor().Value,
                this.VisualParameters.Fill.AsSolidColor().Value);

            if (image == null)
                return null;

            Path path = new Path()
            {
                Data = area,
                Tag = new LayerTag(mapScale) { Layer = this, IsTiled = true, Tile = region, IsDrawn = true, IsNew = true }
            };

            this.Element = path;

            path.Fill = new ImageBrush(image);

            return path;
        }

        //OpenTK Approach
        public Path AsTileUsinOpenTK(List<SqlGeometry> geometries, List<string> labels, double mapScale, TileInfo region, double tileWidth, double tileHeight, RectangleGeometry area, Func<Point, Point> viewTransform, sb.BoundingBox totalExtent)
        {
            if (geometries == null)
                return null;

            //Brush brush = this.VisualParameters.Fill;

            //var color = ((SolidColorBrush)this.VisualParameters.Stroke)?.Color ?? ((SolidColorBrush)this.VisualParameters.Fill).Color;

            //var pen = new System.Drawing.Pen(System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B), (int)this.VisualParameters.StrokeThickness);

            //if (this.VisualParameters.DashStyle != null)
            //{
            //    pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            //}
            var pen = this.VisualParameters.GetGdiPlusPen();

            var brush = this.VisualParameters.GetGdiPlusFillBrush();

            var transform = MapToTileScreenWpf(totalExtent, region.WebMercatorExtent, viewTransform);

            var image = new SqlSpatialToOpenTKBitmap().ParseSqlGeometry(
                            geometries,
                            tileWidth,
                            tileHeight,
                            transform,
                            pen,
                            brush);

            if (image == null)
                return null;

            if (labels != null)
            {
                SqlSpatialToGdiBitmap.DrawLabels(labels, geometries, image, transform, this.Labels);
            }

            BitmapImage bitmapImage = IRI.Jab.Common.Helpers.ImageUtility.AsBitmapImage(image, System.Drawing.Imaging.ImageFormat.Png);

            Path path = new Path()
            {
                Data = area,
                Tag = new LayerTag(mapScale) { Layer = this, IsTiled = true, Tile = region, IsDrawn = true, IsNew = true }
            };

            this.Element = path;

            path.Fill = new ImageBrush(bitmapImage);

            return path;
        }

        #endregion


        ////StreamGeometry Approach
        //public Path AsTileUsingStreamGeometry(List<SqlGeometry> geometries, double mapScale, TileInfo region, double tileWidth, double tileHeight, RectangleGeometry area,
        //    Transform viewTransform, sb.BoundingBox totalExtent, TranslateTransform viewTransformForPoints)
        //{
        //    if (geometries == null)
        //        return null;

        //    StreamGeometry geo;

        //    //var transform = MapToTileScreenWpf(totalExtent, region.MercatorExtent, viewTransform);

        //    if (this.Type.HasFlag(LayerType.Point))
        //    {
        //        geo = SqlSpatialToStreamGeometry.ParseSqlGeometry(geometries, p => viewTransform.Transform(p), this.GeometryPointSymbol);

        //        geo.FillRule = FillRule.Nonzero;

        //        //geo.Transform = viewTransform;
        //    }
        //    else
        //    {
        //        geo = SqlSpatialToStreamGeometry.ParseSqlGeometry(geometries, p => viewTransform.Transform(p));

        //        //geo.Transform = viewTransform;
        //    }

        //    geo.Transform = viewTransformForPoints;

        //    GeometryDrawing drawing = new GeometryDrawing();

        //    Path path = new Path() { Data = geo, Tag = new LayerTag(mapScale) { Tile = region, Layer = this, IsTiled = true }, Stroke = VisualParameters.Stroke, Fill = VisualParameters.Fill, StrokeThickness = VisualParameters.StrokeThickness };
        //    //path.RenderTransform = viewTransform;
        //    this.Element = path;

        //    return path;
        //}

        //private static Func<sb.Point, sb.Point> MapToTileScreen(sb.BoundingBox totalExtent, sb.BoundingBox mapBoundingBoxOfTile, Transform viewTransform)
        //{
        //    var mapShift = mapBoundingBoxOfTile.Center - new sb.Point(totalExtent.TopLeft.X + mapBoundingBoxOfTile.Width / 2.0, totalExtent.TopLeft.Y - mapBoundingBoxOfTile.Height / 2.0);

        //    return p => { return viewTransform.Transform(new Point(p.X - mapShift.X, p.Y - mapShift.Y)).AsPoint(); };
        //}
        private static Func<Point, Point> MapToTileScreenWpf(sb.BoundingBox totalExtent, sb.BoundingBox mapBoundingBoxOfTile, Func<Point, Point> viewTransform)
        {
            //var mapShift = (mapBoundingBoxOfTile.TopLeft - new sb.Point(totalExtent.TopLeft.X + mapBoundingBoxOfTile.Width / 2.0, totalExtent.TopLeft.Y - mapBoundingBoxOfTile.Height / 2.0)).AsWpfPoint();

            //var mapShift = new Point(mapBoundingBoxOfTile.TopLeft.X - totalExtent.TopLeft.X, mapBoundingBoxOfTile.BottomRight.Y - totalExtent.BottomRight.Y);

            return p => { return viewTransform(new Point(p.X - mapBoundingBoxOfTile.TopLeft.X + totalExtent.TopLeft.X, p.Y - mapBoundingBoxOfTile.BottomRight.Y + totalExtent.BottomRight.Y)); };
        }

        private static Func<Point, Point> OldMapToTileScreenWpf(sb.BoundingBox totalExtent, sb.BoundingBox mapBoundingBoxOfTile, Transform viewTransform)
        {
            var mapShift = (mapBoundingBoxOfTile.Center - new sb.Point(totalExtent.TopLeft.X + mapBoundingBoxOfTile.Width / 2.0, totalExtent.TopLeft.Y - mapBoundingBoxOfTile.Height / 2.0)).AsWpfPoint();

            return p => { return viewTransform.Transform(new Point(p.X - mapShift.X, p.Y - mapShift.Y)); };
        }

        private static Func<sb.Point, sb.Point> MapToTileScreen(sb.BoundingBox totalExtent, sb.BoundingBox mapBoundingBoxOfTile, Transform viewTransform)
        {
            var mapShift = (mapBoundingBoxOfTile.Center - new sb.Point(totalExtent.TopLeft.X + mapBoundingBoxOfTile.Width / 2.0, totalExtent.TopLeft.Y - mapBoundingBoxOfTile.Height / 2.0)).AsWpfPoint();

            return p => { return viewTransform.Transform(new Point(p.X - mapShift.X, p.Y - mapShift.Y)).AsPoint(); };
        }

        public void BindWithFrameworkElement(FrameworkElement element)
        {
            //if (element is Path)
            //{
            //    Binding binding1 = new Binding() { Source = this, Path = new PropertyPath("VisualParameters.Stroke"), Mode = BindingMode.TwoWay };
            //    element.SetBinding(Path.StrokeProperty, binding1);

            //    Binding binding2 = new Binding() { Source = this, Path = new PropertyPath("VisualParameters.Fill"), Mode = BindingMode.TwoWay };
            //    element.SetBinding(Path.FillProperty, binding2);

            //    Binding binding3 = new Binding() { Source = this, Path = new PropertyPath("VisualParameters.StrokeThickness"), Mode = BindingMode.TwoWay };
            //    element.SetBinding(Path.StrokeThicknessProperty, binding3);

            //    Binding binding4 = new Binding() { Source = this, Path = new PropertyPath("VisualParameters.Visibility"), Mode = BindingMode.TwoWay };
            //    element.SetBinding(Path.VisibilityProperty, binding4);

            //    Binding binding5 = new Binding() { Source = this, Path = new PropertyPath("VisualParameters.Opacity"), Mode = BindingMode.TwoWay };
            //    element.SetBinding(Path.OpacityProperty, binding5);

            //    Binding binding6 = new Binding() { Source = this, Path = new PropertyPath("VisualParameters.DashType"), Mode = BindingMode.TwoWay };
            //    element.SetBinding(Path.StrokeDashArrayProperty, binding5);
            //}
            //else if (element is System.Windows.Controls.Image)
            //{

            Binding binding4 = new Binding() { Source = this, Path = new PropertyPath("VisualParameters.Visibility"), Mode = BindingMode.TwoWay };
            //Binding binding4 = new Binding() { Source = this, Path = new PropertyPath(nameof(VisualParameters.Visibility)), Mode = BindingMode.TwoWay }; try using this line insted of above
            element.SetBinding(Path.VisibilityProperty, binding4);

            Binding binding5 = new Binding() { Source = this, Path = new PropertyPath("VisualParameters.Opacity"), Mode = BindingMode.TwoWay };
            element.SetBinding(Path.OpacityProperty, binding5);
            //}
            //else
            //{
            //    throw new NotImplementedException();
            //}
        }

        private LayerType GetGeometryType(SqlGeometry geometry)
        {
            var geometryType = geometry.GetOpenGisType();

            switch (geometryType)
            {
                case OpenGisGeometryType.Point:
                case OpenGisGeometryType.MultiPoint:
                    return LayerType.Point;

                case OpenGisGeometryType.LineString:
                case OpenGisGeometryType.MultiLineString:
                    return LayerType.Polyline;

                case OpenGisGeometryType.Polygon:
                case OpenGisGeometryType.MultiPolygon:
                    return LayerType.Polygon;

                case OpenGisGeometryType.GeometryCollection:
                case OpenGisGeometryType.CircularString:
                case OpenGisGeometryType.CompoundCurve:
                case OpenGisGeometryType.CurvePolygon:
                default:
                    throw new NotImplementedException();
            }
        }

        #region Raster Save And Export Methods

        public void SaveAsGoogleTiles(string outputFolderPath, int minLevel = 1, int maxLevel = 13)
        {
            if (maxLevel < minLevel)
            {
                throw new NotImplementedException("(ERROR IN VECTOR LAYER): minLevel must be less than maxLevel");
            }

            var zoomLevels = Enumerable.Range(minLevel, maxLevel - minLevel + 1);

            foreach (var zoom in zoomLevels)
            {
                var googleTiles = WebMercatorUtility.WebMercatorBoundingBoxToGoogleTileRegions(this.Extent, zoom);

                var scale = GoogleScale.Scales.Single(i => i.ZoomLevel == zoom).InverseScale;

                var directory = $"{outputFolderPath}\\{zoom}";

                if (!System.IO.Directory.Exists(directory))
                {
                    System.IO.Directory.CreateDirectory(directory);
                }

                foreach (var tile in googleTiles)
                {
                    var geometries = this.GetGeometriesForDisplay(scale, tile.WebMercatorExtent);

                    var transform = IRI.Msh.Common.Mapping.MapUtility.GetMapToScreen(tile.WebMercatorExtent, 256, 256);

                    Func<Point, Point> mapToScreen = p =>
                    {
                        return transform(p.AsPoint()).AsWpfPoint();
                    };

                    var pen = this.VisualParameters.GetGdiPlusPen();
                    pen.Width = 2;
                    var image = SqlSpatialToGdiBitmap.ParseSqlGeometry(
                                    geometries,
                                    256,
                                    256,
                                    mapToScreen,
                                    pen,
                                    this.VisualParameters.Fill.AsGdiBrush(),
                                    this.VisualParameters.PointSymbol);

                    image.Save($"{directory}\\{tile.ZoomLevel}, {tile.RowNumber}, {tile.ColumnNumber}.jpg");
                }
            }
        }

        public void SaveAsPng(string fileName, sb.BoundingBox mapExtent, double imageWidth, double imageHeight, double mapScale)
        {
            switch (this.ToRasterTechnique)
            {

                case RasterizationApproach.StreamGeometry:
                case RasterizationApproach.DrawingVisual:
                    SaveAsPngDrawingVisual(fileName, mapExtent, imageWidth, imageHeight, mapScale);
                    break;

                case RasterizationApproach.WriteableBitmap:
                case RasterizationApproach.OpenTk:
                case RasterizationApproach.GdiPlus:
                case RasterizationApproach.None:
                default:
                    SaveAsPngGdiPlus(fileName, mapExtent, imageWidth, imageHeight, mapScale);
                    break;
            }
        }

        public async Task<System.Drawing.Bitmap> ParseToBitmapImage(sb.BoundingBox mapExtent, double imageWidth, double imageHeight, double mapScale)
        {
            var geoLabledPairs = await this.GetGeometryLabelPairForDisplayAsync(mapScale, mapExtent);

            if (geoLabledPairs.Geometries == null)
                return null;

            var borderBrush = this.VisualParameters.Stroke.AsGdiBrush();

            var pen = this.VisualParameters.GetGdiPlusPen();

            double xScale = imageWidth / mapExtent.Width;
            double yScale = imageHeight / mapExtent.Height;
            double scale = xScale > yScale ? yScale : xScale;

            Func<Point, Point> mapToScreen = new Func<Point, Point>(p => new Point((p.X - mapExtent.XMin) * scale, -(p.Y - mapExtent.YMax) * scale));

            var image = SqlSpatialToGdiBitmap.ParseSqlGeometry(
                geoLabledPairs.Geometries,
                imageWidth,
                imageHeight,
                mapToScreen,
                pen,
                this.VisualParameters.Fill.AsGdiBrush(),
                this.VisualParameters.PointSymbol);

            if (image == null)
                return null;

            if (geoLabledPairs.Labels != null)
            {
                SqlSpatialToGdiBitmap.DrawLabels(geoLabledPairs.Labels, geoLabledPairs.Geometries, image, mapToScreen, this.Labels);
            }

            return image;
        }

        private async void SaveAsPngGdiPlus(string fileName, sb.BoundingBox mapExtent, double imageWidth, double imageHeight, double mapScale)
        {
            var image = await ParseToBitmapImage(mapExtent, imageWidth, imageHeight, mapScale);

            image.Save(fileName);

            image.Dispose();
        }

        private async void SaveAsPngDrawingVisual(string fileName, sb.BoundingBox mapExtent, double imageWidth, double imageHeight, double mapScale)
        {
            var geoLabledPairs = await this.GetGeometryLabelPairForDisplayAsync(mapScale, mapExtent);

            if (geoLabledPairs.Geometries == null)
                return;

            double xScale = imageWidth / mapExtent.Width;
            double yScale = imageHeight / mapExtent.Height;
            double scale = xScale > yScale ? yScale : xScale;

            Func<Point, Point> mapToScreen = new Func<Point, Point>(p => new Point((p.X - mapExtent.XMin) * scale, -(p.Y - mapExtent.YMax) * scale));

            var pen = this.VisualParameters.GetWpfPen();

            Brush brush = this.VisualParameters.Fill;

            DrawingVisual drawingVisual = new SqlSpatialToDrawingVisual().ParseSqlGeometry(geoLabledPairs.Geometries, i => mapToScreen(i), pen, brush, this.VisualParameters.PointSymbol);

            RenderTargetBitmap image = new RenderTargetBitmap((int)imageWidth, (int)imageHeight, 96, 96, PixelFormats.Pbgra32);

            image.Render(drawingVisual);

            if (this.IsLabeled(mapScale))
            {
                this.DrawLabels(geoLabledPairs.Labels, geoLabledPairs.Geometries, image, mapToScreen);
            }

            image.Freeze();

            PngBitmapEncoder pngImage = new PngBitmapEncoder();

            pngImage.Frames.Add(BitmapFrame.Create(image));

            using (System.IO.Stream stream = System.IO.File.Create(fileName))
            {
                pngImage.Save(stream);
            }

        }

        #endregion

        #region Vector Export Methods

        public void ExportAsShapefile(string shpFileName)
        {
            //var features = GetFeatures<T>();
            var features = this.DataSource.GetSqlFeatures();

            features.SaveAsShapefile(shpFileName, System.Text.Encoding.UTF8, null, true);
        }

        public void ExportAsGeoJson(string geoJsonFileName, bool isLongitudeFirst)
        {
            var features = this.DataSource.GetSqlFeatures();

            features.SaveAsGeoJson(geoJsonFileName, isLongitudeFirst);
        }

        //public List<sb.Feature<sb.Point>> GetFeatures<T>(Func<T, sb.Feature<sb.Point>> map, SqlGeometry geometry) where T : class, ISqlGeometryAware
        //{
        //    if (DataSource as FeatureDataSource<T> != null)
        //    {
        //        return (DataSource as FeatureDataSource<T>).GetFeatures(map, geometry);
        //    }

        //    return null;
        //}

        #endregion

        #region GetGeometry & GetFeature Methods

        public async Task<GeometryLabelPairs> GetGeometryLabelPairForDisplayAsync(double mapScale, sb.BoundingBox mapExtent)
        {
            List<SqlGeometry> geometries; List<string> labels = null;

            if (this.IsLabeled(mapScale))
            {
                var geoLabelPairs = await this.DataSource.GetGeometryLabelPairsForDisplayAsync(mapExtent);

                geometries = geoLabelPairs.Select(i => i.TheSqlGeometry).ToList();

                labels = geoLabelPairs.Select(i => i.Label).ToList();
            }
            else
            {
                geometries = await this.GetGeometriesForDisplayAsync(mapScale, mapExtent);
            }

            return new GeometryLabelPairs(geometries, labels);
        }

        public GeometryLabelPairs GetGeometryLabelPairForDisplay(double mapScale, sb.BoundingBox mapExtent)
        {
            List<SqlGeometry> geometries; List<string> labels = null;

            if (this.IsLabeled(mapScale))
            {
                var geoLabelPairs = this.DataSource.GetGeometryLabelPairsForDisplay(mapExtent);

                geometries = geoLabelPairs.Select(i => i.TheSqlGeometry).ToList();

                labels = geoLabelPairs.Select(i => i.Label).ToList();
            }
            else
            {
                geometries = this.GetGeometriesForDisplay(mapScale, mapExtent);
            }

            return new GeometryLabelPairs(geometries, labels);
        }

        public async Task<List<SqlGeometry>> GetGeometriesForDisplayAsync(double mapScale, sb.BoundingBox boundingBox)
        {
            List<SqlGeometry> geometries = new List<SqlGeometry>();

            if (this.DataSource is MemoryScaleDependentDataSource)
            {
                geometries = await ((MemoryScaleDependentDataSource)this.DataSource).GetGeometriesAsync(mapScale, boundingBox);
            }
            else
            {
                geometries = await this.DataSource.GetGeometriesForDisplayAsync(mapScale, boundingBox);
            }

            if (geometries.Count == 0)
                return null;

            return geometries;
        }

        public List<SqlGeometry> GetGeometriesForDisplay(double mapScale, sb.BoundingBox boundingBox)
        {
            List<SqlGeometry> geometries = new List<SqlGeometry>();

            if (this.DataSource is IScaleDependentDataSource)
            {
                geometries = ((IScaleDependentDataSource)this.DataSource).GetGeometries(mapScale, boundingBox);
            }
            else
            {
                geometries = this.DataSource.GetGeometriesForDisplay(mapScale, boundingBox);
            }

            if (geometries.Count == 0)
                return null;

            return geometries;
        }

        public System.Data.DataTable GetEntireFeature()
        {
            return DataSource?.GetEntireFeatures();
        }

        public List<T> GetFeatures<T>() where T : class, ISqlGeometryAware
        {
            return GetFeatures<T>(null);
        }

        public List<T> GetFeatures<T>(SqlGeometry geometry) where T : class, ISqlGeometryAware
        {
            if (DataSource as FeatureDataSource<T> != null)
            {
                return (DataSource as FeatureDataSource<T>).GetFeatures(geometry);
            }

            return null;
        }

        #endregion



        public bool IsLabeled(double mapScale)
        {
            return this.Labels?.IsLabeled(1.0 / mapScale) == true;
        }

        //POTENTIALLY ERROR PROUNE; formattedText is always RTL
        public void DrawLabels(List<string> labels, List<SqlGeometry> geometries, RenderTargetBitmap bmp, Func<Point, Point> mapToScreen)
        {
            if (labels.Count != geometries.Count)
                return;

            var mapCoordinates = geometries.ConvertAll(
                      (g) =>
                      {
                          SqlGeometry point = this.Labels.PositionFunc(g);
                          return new Point(point.STX.Value, point.STY.Value);
                      }).ToList();

            DrawingVisual drawingVisual = new DrawingVisual();

            using (DrawingContext drawingContext = drawingVisual.RenderOpen())
            {
                for (int i = 0; i < labels.Count; i++)
                {
                    FormattedText formattedText =
                        new FormattedText(labels[i] ?? string.Empty, System.Globalization.CultureInfo.CurrentCulture, this.Labels.IsRtl ? FlowDirection.RightToLeft : FlowDirection.LeftToRight,
                        new Typeface(this.Labels.FontFamily, FontStyles.Normal, FontWeights.Normal, FontStretches.Normal),
                        this.Labels.FontSize, this.Labels.Foreground);

                    Point location = mapToScreen(mapCoordinates[i]);

                    drawingContext.DrawText(formattedText, new Point(location.X - formattedText.Width / 2.0, location.Y - formattedText.Height / 2.0));
                }
            }

            bmp.Render(drawingVisual);
        }



        private Image DrawLabels(List<string> labels, List<SqlGeometry> geometries, double width, double height, Func<Point, Point> mapToScreen)
        {
            if (labels.Count != geometries.Count)
                return null;

            List<Point> mapCoordinates = geometries.ConvertAll<Point>(
                      (g) =>
                      {
                          SqlGeometry point = this.Labels.PositionFunc(g);
                          return new Point(point.STX.Value, point.STY.Value);
                      }).ToList();

            DrawingVisual drawingVisual = new DrawingVisual();

            using (DrawingContext drawingContext = drawingVisual.RenderOpen())
            {
                for (int i = 0; i < labels.Count; i++)
                {
                    FormattedText formattedText =
                        new FormattedText(labels[i], System.Globalization.CultureInfo.CurrentCulture, this.Labels.IsRtl ? FlowDirection.RightToLeft : FlowDirection.LeftToRight,
                        new Typeface(this.Labels.FontFamily, FontStyles.Normal, FontWeights.Normal, FontStretches.Normal),
                        this.Labels.FontSize, this.Labels.Foreground);

                    Point location = mapToScreen(mapCoordinates[i]);

                    drawingContext.DrawText(formattedText, new Point(location.X - formattedText.Width / 2.0, location.Y - formattedText.Height / 2.0));
                }
            }

            Image image = Helpers.ImageUtility.Create(width, height, drawingVisual);

            image.Tag = new Model.LayerTag(-1) { Layer = this, IsTiled = false };

            this.BindWithFrameworkElement(image);

            return image;
        }

        private void DrawLabels(List<string> labels, List<SqlGeometry> geometries, System.Drawing.Bitmap image, Func<sb.IPoint, sb.IPoint> mapToScreen)
        {
            if (labels.Count != geometries.Count)
                return;

            var mapCoordinates = geometries.ConvertAll(
                      (g) =>
                      {
                          return this.Labels.PositionFunc(g).AsPoint();
                      }).ToList();

            var font = new System.Drawing.Font(this.Labels.FontFamily.FamilyNames.First().Value, this.Labels.FontSize);

            var graphic = System.Drawing.Graphics.FromImage(image);

            graphic.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            graphic.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

            graphic.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

            graphic.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;

            for (int i = 0; i < labels.Count; i++)
            {
                var location = mapToScreen(mapCoordinates[i]);

                graphic.DrawString(labels[i], font, Labels.Foreground.AsGdiBrush(), (float)location.X, (float)location.Y);
            }

            graphic.Flush();
        }

        private System.Drawing.Bitmap DrawLabel(int width, int height, List<string> labels, List<SqlGeometry> positions, Func<sb.IPoint, sb.IPoint> transform)
        {
            System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(width, height);

            if (labels.Count != positions.Count)
                return null;

            var mapCoordinates = positions.ConvertAll(
                      (g) =>
                      {
                          return this.Labels.PositionFunc(g).AsPoint();
                      }).ToList();

            var font = new System.Drawing.Font(this.Labels.FontFamily.FamilyNames.First().Value, this.Labels.FontSize);

            var graphic = System.Drawing.Graphics.FromImage(bitmap);

            graphic.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            graphic.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

            graphic.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

            graphic.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;

            for (int i = 0; i < labels.Count; i++)
            {
                var location = transform(mapCoordinates[i]);

                graphic.DrawString(labels[i], font, Labels.Foreground.AsGdiBrush(), (float)location.X, (float)location.Y);
            }

            graphic.Flush();

            return bitmap;
        }


        #region Old Codes


        //public async Task<Path> AsTileUsingGdiPlus(List<SqlGeometry> geometries, List<string> labels, double mapScale, TileInfo region, double width, double height, Func<sb.Point, sb.Point> mapToScreen, RectangleGeometry area)
        //{
        //    Brush brush = this.VisualParameters.Fill;

        //    var color = ((SolidColorBrush)this.VisualParameters.Stroke)?.Color ?? ((SolidColorBrush)this.VisualParameters.Fill).Color;

        //    var pen = new System.Drawing.Pen(System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B), (int)this.VisualParameters.StrokeThickness);

        //    if (geometries == null)
        //    {
        //        return null;
        //    }

        //    var image = await new SqlSpatialToGdiBitmap().ParseSqlGeometry(
        //        geometries,
        //        width,
        //        height,
        //        mapToScreen,
        //        pen,
        //        System.Drawing.Brushes.SkyBlue);

        //    if (image == null)
        //        return null;

        //    if (labels != null)
        //    {
        //        this.DrawLabel(labels, geometries, image, mapToScreen);
        //    }

        //    BitmapImage bitmapImage;

        //    using (var memory = new System.IO.MemoryStream())
        //    {
        //        image.Save(memory, System.Drawing.Imaging.ImageFormat.Png);
        //        memory.Position = 0;
        //        bitmapImage = new BitmapImage();
        //        bitmapImage.BeginInit();
        //        bitmapImage.StreamSource = memory;
        //        bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
        //        bitmapImage.EndInit();
        //    }

        //    Path path = new Path()
        //    {
        //        Data = area,
        //        Tag = new Model.LayerTag(mapScale) { Layer = this, IsTiled = true, Tile = region, IsDrawn = true, IsNew = true }
        //    };

        //    this.Element = path;

        //    path.Fill = new ImageBrush(bitmapImage);

        //    return path;


        //}


        //public async Task<Path> AsSingleImage(List<SqlGeometry> geometries, List<string> labels, double width, double height, Func<sb.Point, sb.Point> mapToScreen)
        //{
        //    Path result = new Path();

        //    switch (this.ToRasterTechnique)
        //    {
        //        case RenderingTechnique.StreamGeometry:
        //            var streamGeometry = SqlSpatialToStreamGeometry.ParseSqlGeometry(geometries, mapToScreen, this.GeometryPointSymbol);
        //            result = new Path() { Data = streamGeometry, Tag = this };

        //            break;
        //        case RenderingTechnique.DrawingVisual:
        //            break;
        //        case RenderingTechnique.GdiPlus:
        //            break;
        //        case RenderingTechnique.WriteableBitmap:
        //            break;
        //        case RenderingTechnique.OpenTk:
        //            break;
        //        case RenderingTechnique.None:
        //            break;
        //        default:
        //            break;
        //    }

        //    result.Tag = new LayerTag() { Layer = this, Extent = null, IsDrawn = true, IsNew = true, IsTiled = false };

        //    this.Element = result;

        //    var image = DrawLabel((int)width, (int)height, labels, geometries, mapToScreen);

        //    return Task.Run(() => { return result; });
        //}


        //private static Func<sb.Point, sb.Point> MapToTileScreen(sb.BoundingBox totalExtent, double entireScreenWidth, double entireScreenHeight, sb.BoundingBox mapBoundingBoxOfTile,
        //      Matrix viewTranform, double mapScale, double unitDistance, double tileScreenWidth, double tileScreenHeight)
        //{
        //    //var totalExtent = this.CurrentExtent;

        //    //double width = MapToScreen(mapBoundingBoxOfTile.Width, mapScale, unitDistance);

        //    //double height = MapToScreen(mapBoundingBoxOfTile.Height, mapScale, unitDistance);

        //    var mapShift = (mapBoundingBoxOfTile.Center - new sb.Point(totalExtent.TopLeft.X + mapBoundingBoxOfTile.Width / 2.0, totalExtent.TopLeft.Y - mapBoundingBoxOfTile.Height / 2.0)).AsWpfPoint();

        //    //return p => { return MapToScreen(new Point(p.X - mapShift.X, p.Y - mapShift.Y)).Parse(); };
        //    return p => { return MapToScreen(new Point(p.X - mapShift.X, p.Y - mapShift.Y), viewTranform).AsPoint(); };
        //}

        //public static Point MapToScreen(Point point, Matrix viewTransform)
        //{
        //    return viewTransform.Transform(point);
        //}

        //public static double MapToScreen(double mercatorDistance, double mapScale, double unitDistance)
        //{
        //    return mercatorDistance * mapScale / unitDistance;
        //}

        #endregion
    }
}
