using System;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

using IRI.Maptor.Extensions;
using IRI.Maptor.Jab.Common.Models;
using IRI.Maptor.Jab.Common.Helpers;
using IRI.Maptor.Sta.Spatial.Helpers;
using IRI.Maptor.Sta.Spatial.Primitives;
using IRI.Maptor.Sta.Persistence.DataSources;

using WpfPoint = System.Windows.Point;
using Point = IRI.Maptor.Sta.Common.Primitives.Point;
using IRI.Maptor.Sta.Common.Primitives;
using IRI.Maptor.Jab.Common.Enums;
using IRI.Maptor.Sta.Persistence.Abstractions;
using IRI.Maptor.Jab.Common.Cartography.Symbologies;
using IRI.Maptor.Jab.Common.Cartography.Rendering;
using IRI.Maptor.Extensions;

namespace IRI.Maptor.Jab.Common;

public class VectorLayer : BaseLayer
{
    #region Properties, Fields

    public IVectorDataSource DataSource { get; protected set; }

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

    public override RasterizationApproach ToRasterTechnique { get; protected set; }

    //public bool IsValid { get; set; }

    //public void Invalidate() => IsValid = false;

    public TileManager TileManager = new TileManager();

    public List<ISymbolizer> Symbolizers { get; protected set; } = new List<ISymbolizer>();

    #endregion

    #region Constructors

    internal VectorLayer()
    {

    }

    public VectorLayer(string name, List<Geometry<Point>> features, LayerType type, RenderingApproach rendering, RasterizationApproach toRasterTechnique)
        : this(name, features, new VisualParameters(BrushHelper.PickBrush(), BrushHelper.PickBrush(), 1, 1, Visibility.Visible), type, rendering, toRasterTechnique)
    {

    }

    public VectorLayer(string layerName, List<Geometry<Point>> features, VisualParameters parameters, LayerType type, RenderingApproach rendering, RasterizationApproach toRasterTechnique)
    {
        if (features == null || features.Count == 0)
            throw new NotImplementedException();

        Initialize(layerName, new MemoryDataSource(features), parameters, type, rendering, toRasterTechnique, ScaleInterval.All, null, null);
    }

    public VectorLayer(string layerName, IVectorDataSource dataSource, VisualParameters parameters, LayerType type, RenderingApproach rendering,
        RasterizationApproach toRasterTechnique, ScaleInterval visibleRange, SimplePointSymbolizer pointSymbol = null, LabelParameters labeling = null)
    {
        Initialize(layerName, dataSource, parameters, type, rendering, toRasterTechnique, visibleRange, pointSymbol, labeling);
    }

    private void Initialize(string layerName, IVectorDataSource dataSource, VisualParameters parameters, LayerType type, RenderingApproach rendering,
                                RasterizationApproach toRasterTechnique, ScaleInterval visibleRange,
                                SimplePointSymbolizer pointSymbol, LabelParameters labeling)
    {
        this.LayerId = Guid.NewGuid();

        this.DataSource = dataSource;

        this.Rendering = rendering;

        this.ToRasterTechnique = toRasterTechnique;

        this.Type = type;

        //var geometries = dataSource.GetGeometries();

        if (dataSource.GeometryType.AsLayerType() is not null)
        {
            this.Type = type | dataSource.GeometryType.AsLayerType().Value; /*GetGeometryType(geometries.FirstOrDefault(g => g != null))*/;
        }
        else
        {
            this.Type = type;
        }

        //this.Extent = geometries?.GetBoundingBox() ?? BoundingBox.NaN;
        this.Extent = dataSource.WebMercatorExtent;

        this.LayerName = layerName;

        this.VisualParameters = parameters;

        //this.PointSymbol = pointSymbol ?? new SimplePointSymbol() { SymbolWidth = 4, SymbolHeight = 4 };

        //if (layerName == "مدار" || layerName == "تکه مسیر خط")
        //{
        //    var p1 = parameters.Clone();
        //    var p2 = parameters.Clone();
        //    var p3 = parameters.Clone();

        //    p1.Stroke = Brushes.Purple; p1.StrokeThickness = 8;
        //    p2.Stroke = Brushes.Red; p2.StrokeThickness = 5;
        //    p3.Stroke = Brushes.Blue; p3.StrokeThickness = 2;

        //    this.Symbolizers.Add(new SimpleSymbolizer(f => f.Attributes["denomi_vol"].ToString() == "400", p1));
        //    this.Symbolizers.Add(new SimpleSymbolizer(f => f.Attributes["denomi_vol"].ToString() == "230", p2));
        //    this.Symbolizers.Add(new SimpleSymbolizer(f => f.Attributes["denomi_vol"].ToString() == "63", p3));
        //}
        //else
        //{
            this.Symbolizers.Add(new SimpleSymbolizer(parameters));
        //}

        this.Labels = labeling;

        //Check for missing visibleRange
        if (this.Labels != null)
        {
            if (this.Labels.VisibleRange == null)
            {
                this.Labels.VisibleRange = visibleRange;
            }

            this.Symbolizers.Add(new LabelSymbolizer(labeling));
        }

        this.VisibleRange = (visibleRange == null) ? ScaleInterval.All : visibleRange;

    }

    #endregion

    public override string ToString()
    {
        return $"{Enum.GetName(this.Type)} - {this.DataSource.ToString()}";
    }

    #region Rendering


    //DrawingVisual Approach
    //public ImageBrush? RenderUsingDrawingVisual(List<Feature<Point>> features, double mapScale, double screenWidth, double screenHeight/*,*/ /*Func<WpfPoint, WpfPoint> mapToScreen,*/ /*RectangleGeometry area*/)
    //{
    //    if (features.IsNullOrEmpty())
    //        return null;

    //    //var pen = this.VisualParameters.GetWpfPen();

    //    //Brush brush = this.VisualParameters.Fill;

    //    //DrawingVisual drawingVisual = new DrawingVisualRenderer().ParseGeometry(features, /*mapToScreen,*/ pen, brush, this.VisualParameters.PointSymbol);

    //    //if (drawingVisual is null)
    //    //    return null;

    //    //RenderTargetBitmap image = new RenderTargetBitmap((int)screenWidth, (int)screenHeight, 96, 96, PixelFormats.Pbgra32);

    //    //image.Render(drawingVisual);

    //    //if (this.CanRenderLabels(mapScale))
    //    //{
    //    //    var renderedLabels = this.DrawLabels(features/*, image*//*, mapToScreen*/);

    //    //    if (renderedLabels is not null)
    //    //        image.Render(renderedLabels);
    //    //}

    //    //image.Freeze();

    //    //var drawingVisuals = AsDrawingVisual(features, mapScale);
    //    var drawingVisuals = new DrawingVisualRenderStrategy(Symbolizers).AsDrawingVisual(features, mapScale);

    //    var image = ImageUtility.Render(drawingVisuals, (int)screenWidth, (int)screenHeight);

    //    return new ImageBrush(image);

    //    //Path path = new Path()
    //    //{
    //    //    //Data = area,
    //    //    Tag = new LayerTag(mapScale) { Layer = this, IsTiled = false },
    //    //};

    //    //this.Element = path;

    //    //path.Fill = new ImageBrush(image);

    //    //return path;
    //}

    //Gdi+
    //public ImageBrush? RenderUsingGdiPlus(List<Feature<Point>> features, double mapScale, double screenWidth, double screenHeight/*,*/ /*Func<WpfPoint, WpfPoint> mapToScreen,*/ /*RectangleGeometry area*/)
    //{
    //    //if (features.IsNullOrEmpty())
    //    //    return null;

    //    ////var borderBrush = this.VisualParameters.Stroke.AsGdiBrush();

    //    ////var pen = this.VisualParameters.GetGdiPlusPen();

    //    //var bitmap = GdiBitmapRenderer.ParseSqlGeometry(
    //    //    features,
    //    //    screenWidth,
    //    //    screenHeight,
    //    //    //mapToScreen,
    //    //    this.VisualParameters.GetGdiPlusPen(),
    //    //    this.VisualParameters.Fill.AsGdiBrush(),
    //    //    this.VisualParameters.PointSymbol);

    //    //if (bitmap == null)
    //    //    return null;

    //    //if (CanRenderLabels(mapScale))
    //    //{
    //    //    GdiBitmapRenderer.DrawLabels(features, bitmap, /*mapToScreen,*/ this.Labels);
    //    //}

    //    var bitmap = AsGdiBitmap(features, screenWidth, screenHeight, mapScale);

    //    if (bitmap is null)
    //        return null;

    //    BitmapImage image = ImageUtility.AsBitmapImage(bitmap, System.Drawing.Imaging.ImageFormat.Png);

    //    bitmap.Dispose();

    //    image.Freeze();

    //    return new ImageBrush(image);

    //    //Path path = new Path()
    //    //{
    //    //    //Data = area,
    //    //    Tag = new Model.LayerTag(mapScale) { Layer = this, Tile = null, IsDrawn = true, IsNew = true }
    //    //};

    //    //this.Element = path;

    //    //path.Fill = new ImageBrush(bitmapImage);

    //    //return path;
    //}

    //Consider Labels
    //public ImageBrush? RenderUsingWriteableBitmap(List<Feature<Point>> features, double mapScale, double screenWidth, double screenHeight/*,*/ /*Func<WpfPoint, WpfPoint> mapToScreen, *//*RectangleGeometry area*/)
    //{
    //    if (features.IsNullOrEmpty())
    //        return null;

    //    var image = new WriteableBitmapRenderStrategy(this.Symbolizers).ParseSqlGeometry(
    //                        features,
    //                        //mapToScreen,
    //                        (int)screenWidth,
    //                        (int)screenHeight,
    //                        this.VisualParameters.Stroke.AsSolidColor().Value,
    //                        this.VisualParameters.Fill.AsSolidColor().Value);

    //    if (image == null)
    //        return null;

    //    if (CanRenderLabels(mapScale))
    //    {
    //        //this.DrawLabel(labels, geometries, image, transform);
    //    }

    //    image.Freeze();

    //    return new ImageBrush(image);

    //    ////Try #3
    //    //Path path = new Path()
    //    //{
    //    //    //Data = area,
    //    //    Tag = new LayerTag(mapScale) { Layer = this, Tile = null, IsDrawn = true, IsNew = true }
    //    //};

    //    //this.Element = path;

    //    //path.Fill = new ImageBrush(image);

    //    //return path;
    //}

    //StreamGeometry Approach
    //public Path AsShape(List<Feature<Point>> features, TransformGroup viewTransform, TranslateTransform viewTransformForPoints/*, Func<WpfPoint, WpfPoint> mapToScreen*/)
    //{
    //    StreamGeometry geo;

    //    if (this.Type.HasFlag(LayerType.Point))
    //    {
    //        geo = StreamGeometryRenderer.ParseSqlGeometry(features, /*mapToScreen,*/ this.VisualParameters.PointSymbol.GeometryPointSymbol);

    //        geo.FillRule = FillRule.Nonzero;

    //        geo.Transform = viewTransformForPoints;
    //    }
    //    else
    //    {
    //        geo = StreamGeometryRenderer.ParseSqlGeometry(features/*, p => p*/);

    //        geo.Transform = viewTransform;
    //    }

    //    //GeometryDrawing drawing = new GeometryDrawing();

    //    Path path = new Path()
    //    {
    //        StrokeDashArray = VisualParameters.DashType,
    //        Data = geo,
    //        Tag = new LayerTag(-1) { Layer = this, IsTiled = false },
    //        Stroke = VisualParameters.Stroke,
    //        Fill = VisualParameters.Fill,
    //        StrokeThickness = VisualParameters.StrokeThickness
    //    };

    //    this.Element = path;

    //    return path;
    //}
    //OpenTK
    //public Path AsBitmapUsingOpenTK(List<Geometry<Point>> geometries, List<string> labels, double mapScale, BoundingBox boundingBox, double width, double height, Func<WpfPoint, WpfPoint> mapToScreen, RectangleGeometry area)
    //{
    //    if (geometries == null)
    //        return null;

    //    //Pen pen = new Pen(this.VisualParameters.Stroke, this.VisualParameters.StrokeThickness);
    //    var pen = this.VisualParameters.GetGdiPlusPen();

    //    Brush brush = this.VisualParameters.Fill;

    //    //var color = ((SolidColorBrush)this.VisualParameters.Stroke)?.Color ?? ((SolidColorBrush)this.VisualParameters.Fill).Color;

    //    //var image = new SqlSpatialToOpenTKBitmap().ParseSqlGeometry(
    //    //    geometries,
    //    //    width,
    //    //    height,
    //    //    mapToScreen,
    //    //    new System.Drawing.Pen(System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B), (int)this.VisualParameters.StrokeThickness),
    //    //    System.Drawing.Brushes.SkyBlue);

    //    var image = new SqlSpatialToOpenTKBitmap().ParseSqlGeometry(
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
    //        SqlSpatialToGdiBitmap.DrawLabels(labels, geometries, image, mapToScreen, Labels);
    //    }

    //    BitmapImage bitmapImage = IRI.Maptor.Jab.Common.Helpers.ImageUtility.AsBitmapImage(image, System.Drawing.Imaging.ImageFormat.Png);

    //    //Try #3
    //    Path path = new Path()
    //    {
    //        Data = area,
    //        Tag = new Model.LayerTag(mapScale) { Layer = this, Tile = null, IsDrawn = true, IsNew = true }
    //    };

    //    this.Element = path;

    //    path.Fill = new ImageBrush(bitmapImage);


    //    return path;
    //}

    #endregion


    #region Tile Rendering, Old methods

    //DrawingVisual Approach
    //public ImageBrush? AsTileUsingDrawingVisual(List<Feature<Point>> features, double mapScale, /*TileInfo region,*/ double tileWidth, double tileHeight/*, RectangleGeometry area*//*,*/ /*Func<WpfPoint, WpfPoint> viewTransform, *//*BoundingBox totalExtent*/)
    //{
    //    if (features is null)
    //        return null;

    //    //var shiftX = region.WebMercatorExtent.Center.X - totalExtent.TopLeft.X - region.WebMercatorExtent.Width / 2.0;
    //    //var shiftY = region.WebMercatorExtent.Center.Y - totalExtent.TopLeft.Y + region.WebMercatorExtent.Height / 2.0;

    //    var drawingVisual = new DrawingVisualRenderer().ParseGeometry(
    //                            features,
    //                            //p => viewTransform(new WpfPoint(p.X - shiftX, p.Y - shiftY)),
    //                            this.VisualParameters.GetWpfPen(),
    //                            this.VisualParameters.Fill,
    //                            this.VisualParameters.PointSymbol);

    //    if (drawingVisual is null)
    //        return null;

    //    RenderTargetBitmap image = new RenderTargetBitmap((int)tileWidth, (int)tileHeight, 96, 96, PixelFormats.Pbgra32);

    //    image.Render(drawingVisual);

    //    if (this.CanRenderLabels(mapScale))
    //    {
    //        this.DrawLabels(features, image/*, p => viewTransform(new WpfPoint(p.X - shiftX, p.Y - shiftY))*/);
    //    }

    //    image.Freeze();

    //    return new ImageBrush(image);

    //    //Path path = new Path()
    //    //{
    //    //    //Data = area,
    //    //    Tag = new LayerTag(mapScale) { Layer = this, IsTiled = true, Tile = new TileInfo(region.RowNumber, region.ColumnNumber, region.ZoomLevel), IsDrawn = true, IsNew = true }
    //    //};

    //    //this.Element = path;

    //    //path.Fill = new ImageBrush(image);

    //    //return path;
    //}

    ////Gdi+ Approach
    //public ImageBrush? AsTileUsingGdiPlusAsync(List<Feature<Point>> features, double mapScale, /*TileInfo region, */double tileWidth, double tileHeight/*, RectangleGeometry area*//*,*/ /*Func<WpfPoint, WpfPoint> viewTransform, *//*BoundingBox totalExtent*/)
    //{
    //    if (features.IsNullOrEmpty())
    //        return null;

    //    //var shiftX = region.WebMercatorExtent.Center.X - totalExtent.TopLeft.X - region.WebMercatorExtent.Width / 2.0;
    //    //var shiftY = region.WebMercatorExtent.Center.Y - totalExtent.TopLeft.Y + region.WebMercatorExtent.Height / 2.0;

    //    var bitmap = GdiBitmapRenderer.ParseSqlGeometry(
    //                    features,
    //                    tileWidth,
    //                    tileHeight,
    //                    //p => viewTransform(new WpfPoint(p.X - shiftX, p.Y - shiftY)),
    //                    this.VisualParameters.GetGdiPlusPen(),
    //                    this.VisualParameters.Fill.AsGdiBrush(),
    //                    this.VisualParameters.PointSymbol);

    //    if (bitmap is null)
    //        return null;

    //    if (this.CanRenderLabels(mapScale))
    //    {
    //        //96.05.19
    //        //SqlSpatialToGdiBitmap.DrawLabels(labels, geometries, image, transform, Labels);
    //    }

    //    var image = ImageUtility.AsBitmapImage(bitmap, System.Drawing.Imaging.ImageFormat.Png);

    //    bitmap.Dispose();

    //    image.Freeze();

    //    return new ImageBrush(image);

    //    //Path path = new Path()
    //    //{
    //    //    //Data = area,
    //    //    Tag = new LayerTag(mapScale) { Layer = this, IsTiled = true, Tile = new TileInfo(region.RowNumber, region.ColumnNumber, region.ZoomLevel), IsDrawn = true, IsNew = true }
    //    //};

    //    //this.Element = path;

    //    //path.Fill = new ImageBrush(bitmapImage);

    //    //return path;
    //}

    //Writeable Bitmap Approach
    //Consider Labeling
    //public ImageBrush? AsTileUsingWriteableBitmap(List<Feature<Point>> features, double mapScale, /*TileInfo region, */double tileWidth, double tileHeight/*, RectangleGeometry area*//*,*/ /*Func<WpfPoint, WpfPoint> viewTransform, *//*BoundingBox totalExtent*/)
    //{
    //    if (features.IsNullOrEmpty())
    //        return null;

    //    //var transform = MapToTileScreenWpf(totalExtent, region.WebMercatorExtent, viewTransform);

    //    var image = new WriteableBitmapRenderer().ParseSqlGeometry(
    //        features,
    //        //transform,
    //        (int)tileWidth,
    //        (int)tileHeight,
    //        this.VisualParameters.Stroke.AsSolidColor().Value,
    //        this.VisualParameters.Fill.AsSolidColor().Value);

    //    if (image is null)
    //        return null;

    //    image.Freeze();

    //    return new ImageBrush(image);

    //    //Path path = new Path()
    //    //{
    //    //    //Data = area,
    //    //    Tag = new LayerTag(mapScale) { Layer = this, IsTiled = true, Tile = region, IsDrawn = true, IsNew = true }
    //    //};

    //    //this.Element = path;

    //    //path.Fill = new ImageBrush(image);

    //    //return path;
    //}

    ////OpenTK Approach
    //public Path AsTileUsinOpenTK(List<Geometry<Point>> geometries, List<string> labels, double mapScale, TileInfo region, double tileWidth, double tileHeight, RectangleGeometry area, Func<WpfPoint, WpfPoint> viewTransform, BoundingBox totalExtent)
    //{
    //    if (geometries == null)
    //        return null;

    //    //Brush brush = this.VisualParameters.Fill;

    //    //var color = ((SolidColorBrush)this.VisualParameters.Stroke)?.Color ?? ((SolidColorBrush)this.VisualParameters.Fill).Color;

    //    //var pen = new System.Drawing.Pen(System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B), (int)this.VisualParameters.StrokeThickness);

    //    //if (this.VisualParameters.DashStyle != null)
    //    //{
    //    //    pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
    //    //}
    //    var pen = this.VisualParameters.GetGdiPlusPen();

    //    var brush = this.VisualParameters.GetGdiPlusFillBrush();

    //    var transform = MapToTileScreenWpf(totalExtent, region.WebMercatorExtent, viewTransform);

    //    var image = new SqlSpatialToOpenTKBitmap().ParseSqlGeometry(
    //                    geometries,
    //                    tileWidth,
    //                    tileHeight,
    //                    transform,
    //                    pen,
    //                    brush);

    //    if (image == null)
    //        return null;

    //    if (labels != null)
    //    {
    //        SqlSpatialToGdiBitmap.DrawLabels(labels, geometries, image, transform, this.Labels);
    //    }

    //    BitmapImage bitmapImage = IRI.Maptor.Jab.Common.Helpers.ImageUtility.AsBitmapImage(image, System.Drawing.Imaging.ImageFormat.Png);

    //    Path path = new Path()
    //    {
    //        Data = area,
    //        Tag = new LayerTag(mapScale) { Layer = this, IsTiled = true, Tile = region, IsDrawn = true, IsNew = true }
    //    };

    //    this.Element = path;

    //    path.Fill = new ImageBrush(bitmapImage);

    //    return path;
    //}

    //private static Func<WpfPoint, WpfPoint> MapToTileScreenWpf(BoundingBox totalExtent, BoundingBox mapBoundingBoxOfTile, Func<WpfPoint, WpfPoint> viewTransform)
    //{
    //    return p => { return viewTransform(new WpfPoint(p.X - mapBoundingBoxOfTile.TopLeft.X + totalExtent.TopLeft.X, p.Y - mapBoundingBoxOfTile.BottomRight.Y + totalExtent.BottomRight.Y)); };
    //}

    //private static Func<WpfPoint, WpfPoint> OldMapToTileScreenWpf(BoundingBox totalExtent, BoundingBox mapBoundingBoxOfTile, Transform viewTransform)
    //{
    //    var mapShift = (mapBoundingBoxOfTile.Center - new Point(totalExtent.TopLeft.X + mapBoundingBoxOfTile.Width / 2.0, totalExtent.TopLeft.Y - mapBoundingBoxOfTile.Height / 2.0)).AsWpfPoint();

    //    return p => { return viewTransform.Transform(new WpfPoint(p.X - mapShift.X, p.Y - mapShift.Y)); };
    //}

    //private static Func<Point, Point> MapToTileScreen(BoundingBox totalExtent, BoundingBox mapBoundingBoxOfTile, Transform viewTransform)
    //{
    //    var mapShift = (mapBoundingBoxOfTile.Center - new Point(totalExtent.TopLeft.X + mapBoundingBoxOfTile.Width / 2.0, totalExtent.TopLeft.Y - mapBoundingBoxOfTile.Height / 2.0)).AsWpfPoint();

    //    return p => { return viewTransform.Transform(new WpfPoint(p.X - mapShift.X, p.Y - mapShift.Y)).AsPoint(); };
    //}

    #endregion


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
    }

    public static Func<Point, Point> CreateMapToScreenMapFunc(BoundingBox mapExtent, double screenWidth, double screenHeight)
    {
        double xScale = screenWidth / mapExtent.Width;
        double yScale = screenHeight / mapExtent.Height;
        double scale = xScale > yScale ? yScale : xScale;

        return new Func<Point, Point>(p => new Point((p.X - mapExtent.XMin) * scale, -(p.Y - mapExtent.YMax) * scale));
    }

    public async Task<List<Feature<Point>>> GetRenderReadyFeatures(BoundingBox mapExtent, double imageWidth, double imageHeight, double mapScale)
    {
        //var geoLabledPairs = await this.GetGeometryLabelPairForDisplayAsync(mapScale, mapExtent);
        var feature = await this.DataSource.GetAsFeatureSetAsync(mapScale, mapExtent);

        //if (geoLabledPairs.Geometries == null)
        if (feature is null || feature.HasNoGeometry())
            return new List<Feature<Point>>();

        //double xScale = imageWidth / mapExtent.Width;
        //double yScale = imageHeight / mapExtent.Height;
        //double scale = xScale > yScale ? yScale : xScale;
        //Func<Point, Point> mapToScreen = new Func<Point, Point>(p => new Point((p.X - mapExtent.XMin) * scale, -(p.Y - mapExtent.YMax) * scale));
        var mapToScreen = CreateMapToScreenMapFunc(mapExtent, imageWidth, imageHeight);

        return feature.Transform(mapToScreen).Features;
    }


    #region Raster Save And Export Methods

    public async Task<System.Drawing.Bitmap?> AsGdiBitmapAsync(BoundingBox mapExtent, double imageWidth, double imageHeight, double mapScale)
    {
        ////var geoLabledPairs = await this.GetGeometryLabelPairForDisplayAsync(mapScale, mapExtent);
        //var feature = await this.DataSource.GetAsFeatureSetAsync(mapScale, mapExtent);

        //if (feature is null || feature.HasNoGeometry())
        //    return new System.Drawing.Bitmap((int)imageWidth, (int)imageHeight);

        ////double xScale = imageWidth / mapExtent.Width;
        ////double yScale = imageHeight / mapExtent.Height;
        ////double scale = xScale > yScale ? yScale : xScale;
        ////Func<Point, Point> mapToScreen = new Func<Point, Point>(p => new Point((p.X - mapExtent.XMin) * scale, -(p.Y - mapExtent.YMax) * scale));
        //var mapToScreen = CreateMapToScreenMapFunc(mapExtent, imageWidth, imageHeight);

        //var features = feature.Transform(mapToScreen).Features;
        //var geometries = feature.GetGeometries().Select(g => g.Transform(mapToScreen, g.Srid)).ToList();
        var features = await GetRenderReadyFeatures(mapExtent, imageWidth, imageHeight, mapScale);

        return new GdiBitmapRenderStrategy(Symbolizers).AsGdiBitmap(features, imageWidth, imageHeight, mapScale);
    }

    //public System.Drawing.Bitmap? AsGdiBitmap(List<Feature<Point>> features, double imageWidth, double imageHeight, double mapScale)
    //{
    //    if (features.IsNullOrEmpty())
    //        return null;

    //    //var borderBrush = this.VisualParameters.Stroke.AsGdiBrush();

    //    //var pen = this.VisualParameters.GetGdiPlusPen();

    //    var image = new GdiBitmapRenderStrategy().ParseSqlGeometry(
    //        features,
    //        imageWidth,
    //        imageHeight,
    //        //mapToScreen,
    //        this.VisualParameters.GetGdiPlusPen(),
    //        this.VisualParameters.Fill.AsGdiBrush(),
    //        this.VisualParameters.PointSymbol);

    //    if (image is null)
    //        return null;

    //    if (this.CanRenderLabels(mapScale))
    //    {
    //        new GdiBitmapRenderStrategy().DrawLabels(features, image, /*mapToScreen, */this.Labels);
    //    }

    //    return image;
    //}


    // Todo: this method has been totally changed but not tested!
    public async Task SaveAsGoogleTiles(string outputFolderPath, int minLevel = 1, int maxLevel = 13)
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
                //var geometries = await GetGeometriesForDisplayAsync(scale, tile.WebMercatorExtent);
                //var feature = await this.DataSource.GetAsFeatureSetAsync(scale, tile.WebMercatorExtent);

                //if (feature is null || feature.Features.IsNullOrEmpty())
                //    continue;

                ////var geometries = feature.GetGeometries();

                //var transform = MapUtility.GetMapToScreen(tile.WebMercatorExtent, 256, 256);

                //var features = feature.Transform(transform).Features;

                //Func<WpfPoint, WpfPoint> mapToScreen = p =>  transform(p.AsPoint()).AsWpfPoint(); 

                //var pen = this.VisualParameters.GetGdiPlusPen();
                //pen.Width = 2;
                //var image = GdiBitmapRenderer.ParseSqlGeometry(
                //                features,
                //                256,
                //                256,
                //                //mapToScreen,
                //                pen,
                //                this.VisualParameters.Fill.AsGdiBrush(),
                //                this.VisualParameters.PointSymbol);

                var image = await AsGdiBitmapAsync(tile.WebMercatorExtent, 256, 256, scale);

                if (image is null)
                    continue;

                image.Save($"{directory}\\{tile.ZoomLevel}, {tile.RowNumber}, {tile.ColumnNumber}.jpg");
            }
        }
    }

    public async void SaveAsPng(string fileName, BoundingBox mapExtent, double imageWidth, double imageHeight, double mapScale)
    {
        switch (this.ToRasterTechnique)
        {

            case RasterizationApproach.StreamGeometry:
            case RasterizationApproach.DrawingVisual:
                //SaveAsPngDrawingVisual(fileName, mapExtent, imageWidth, imageHeight, mapScale);
                var renderTargetBitmap = await AsRenderTargetBitmap(mapExtent, imageWidth, imageHeight, mapScale);

                if (renderTargetBitmap is null)
                    return;

                ImageUtility.Save(fileName, renderTargetBitmap);

                break;

            case RasterizationApproach.WriteableBitmap:
            //case RasterizationApproach.OpenTk:
            case RasterizationApproach.GdiPlus:
            case RasterizationApproach.None:
            default:
                //SaveAsPngGdiPlus(fileName, mapExtent, imageWidth, imageHeight, mapScale);
                var image = await AsGdiBitmapAsync(mapExtent, imageWidth, imageHeight, mapScale);

                if (image is null)
                    return;

                image.Save(fileName);

                image.Dispose();
                break;
        }
    }

    //private async void SaveAsPngGdiPlus(string fileName, BoundingBox mapExtent, double imageWidth, double imageHeight, double mapScale)
    //{
    //    var image = await ParseToBitmapImage(mapExtent, imageWidth, imageHeight, mapScale);

    //    image.Save(fileName);

    //    image.Dispose();
    //}

    //// todo: consider using AsDrawingVisual method and removing the duplicate codes
    //private async void SaveAsPngDrawingVisual(string fileName, BoundingBox mapExtent, double imageWidth, double imageHeight, double mapScale)
    //{
    //    ////var geoLabledPairs = await this.GetGeometryLabelPairForDisplayAsync(mapScale, mapExtent);
    //    //var feature = await this.DataSource.GetAsFeatureSetAsync(mapScale, mapExtent);

    //    ////if (geoLabledPairs.Geometries == null)
    //    //if (feature is null || feature.HasNoGeometry())
    //    //    return;

    //    ////double xScale = imageWidth / mapExtent.Width;
    //    ////double yScale = imageHeight / mapExtent.Height;
    //    ////double scale = xScale > yScale ? yScale : xScale;
    //    ////Func<Point, Point> mapToScreen = new Func<Point, Point>(p => new Point((p.X - mapExtent.XMin) * scale, -(p.Y - mapExtent.YMax) * scale));
    //    //var mapToScreen = CreateMapToScreenMapFunc(mapExtent, imageWidth, imageHeight);

    //    //var features = feature.Transform(mapToScreen).Features;

    //    ////var geometries = feature.GetGeometries().Select(g => g.Transform(mapToScreen, g.Srid)).ToList();

    //    //var pen = this.VisualParameters.GetWpfPen();

    //    //Brush brush = this.VisualParameters.Fill;

    //    //DrawingVisual drawingVisual = new DrawingVisualRenderer().ParseGeometry(features, /*i => mapToScreen(i), */pen, brush, this.VisualParameters.PointSymbol);
    //    var drawingVisuals = await AsDrawingVisual(mapExtent, imageWidth, imageHeight, mapScale);

    //    if (drawingVisuals.IsNullOrEmpty())
    //        return;

    //    ImageUtility.Save(fileName, drawingVisuals, (int)imageWidth, (int)imageHeight);

    //    //RenderTargetBitmap image = new RenderTargetBitmap((int)imageWidth, (int)imageHeight, 96, 96, PixelFormats.Pbgra32);

    //    //foreach (var item in drawingVisuals)
    //    //{
    //    //    image.Render(item);
    //    //}

    //    ////if (this.CanRenderLabels(mapScale) && feature.IsLabeled())
    //    ////{
    //    ////    this.DrawLabels(features, image/*, mapToScreen*/);
    //    ////}

    //    //image.Freeze();

    //    //PngBitmapEncoder pngImage = new PngBitmapEncoder();

    //    //pngImage.Frames.Add(BitmapFrame.Create(image));

    //    //using (System.IO.Stream stream = System.IO.File.Create(fileName))
    //    //{
    //    //    pngImage.Save(stream);
    //    //}

    //}

    public async Task<List<DrawingVisual>> AsDrawingVisual(BoundingBox mapExtent, double imageWidth, double imageHeight, double mapScale)
    {
        var features = await GetRenderReadyFeatures(mapExtent, imageWidth, imageHeight, mapScale);

        return new DrawingVisualRenderStrategy(Symbolizers).AsDrawingVisual(features, mapScale);
        //return AsDrawingVisual(features, mapScale);
    }

    //private List<DrawingVisual> AsDrawingVisual(List<Feature<Point>> features, double mapScale)
    //{
    //    var result = new List<DrawingVisual>();

    //    var pen = this.VisualParameters.GetWpfPen();

    //    if (pen is not null)
    //    {
    //        pen.LineJoin = PenLineJoin.Round;
    //        pen.EndLineCap = PenLineCap.Round;
    //        pen.StartLineCap = PenLineCap.Round;
    //    }

    //    Brush brush = this.VisualParameters.Fill;

    //    DrawingVisual drawingVisual = new DrawingVisualRenderStrategy(Symbolizers).ParseGeometry(
    //                                    features,
    //                                    //mapToScreen,
    //                                    pen,
    //                                    brush,
    //                                    VisualParameters.PointSymbol);

    //    drawingVisual.Opacity = this.VisualParameters.Opacity;

    //    result.Add(drawingVisual);

    //    if (this.CanRenderLabels(mapScale) /*&& feature.IsLabeled()*/)
    //    {
    //        var renderedLabels = this.DrawLabels(features/*, image*//*, mapToScreen*/);

    //        if (renderedLabels is not null)
    //        {
    //            renderedLabels.Opacity = this.VisualParameters.Opacity;

    //            result.Add(renderedLabels);
    //        }
    //    }

    //    return result;
    //}

    public async Task<RenderTargetBitmap?> AsRenderTargetBitmap(BoundingBox mapExtent, double imageWidth, double imageHeight, double mapScale)
    {
        var features = await GetRenderReadyFeatures(mapExtent, imageWidth, imageHeight, mapScale);

        if (features.IsNullOrEmpty())
            return null;

        //var drawingVisuals = AsDrawingVisual(features, mapScale);
        var drawingVisuals = new DrawingVisualRenderStrategy(Symbolizers).AsDrawingVisual(features, mapScale);

        if (drawingVisuals.IsNullOrEmpty())
            return null;

        return ImageUtility.Render(drawingVisuals, (int)imageWidth, (int)imageHeight);
    }

    #endregion


    #region Vector Export Methods

    public void ExportAsShapefile(string shpFileName)
    {
        //var features = GetFeatures<T>();
        var features = this.DataSource.GetAsFeatureSet();

        features.SaveAsShapefile(shpFileName, System.Text.Encoding.UTF8, null, true);
    }

    public void ExportAsGeoJson(string geoJsonFileName, bool isLongitudeFirst)
    {
        var features = this.DataSource.GetAsFeatureSet();

        features.SaveAsGeoJson(geoJsonFileName, isLongitudeFirst);
    }

    #endregion


    #region GetGeometry & GetFeature Methods

    //public async Task<FeatureSet<Point>> GetGeometryLabelPairForDisplayAsync(double mapScale, BoundingBox mapExtent)
    //{
    //List<Geometry<Point>>? geometries;

    //List<string>? labels = null;

    //if (this.CanRenderLabels(mapScale))
    //{
    //    var geoLabelPairs = await this.DataSource.GetAsFeatureSetAsync(mapExtent);

    //    geometries = geoLabelPairs.Features.Select(i => i.TheGeometry).ToList();

    //    labels = geoLabelPairs.Features.Select(i => i.Label).ToList();
    //}
    //else
    //{
    //    geometries = await this.GetGeometriesForDisplayAsync(mapScale, mapExtent);
    //}

    //return new GeometryLabelPairs(geometries, labels);
    //}

    //public async Task<List<Geometry<Point>>> GetGeometriesForDisplayAsync(double mapScale, BoundingBox boundingBox)
    //{
    //    //List<Geometry<Point>> geometries = new List<Geometry<Point>>();

    //    //if (this.DataSource is MemoryScaleDependentDataSource)
    //    //{
    //    //    geometries = await ((MemoryScaleDependentDataSource)this.DataSource).GetGeometriesAsync(mapScale, boundingBox);
    //    //}
    //    //else
    //    //{
    //    var features = await this.DataSource.GetAsFeatureSetAsync(mapScale, boundingBox);

    //    if (features == FeatureSet<Point>.Empty)
    //        return new List<Geometry<Point>>();

    //    var geometries = features.GetGeometries();
    //    //}

    //    //if (geometries.Count == 0)
    //    //    return null;

    //    return geometries;
    //}

    public FeatureSet<Point>? GetFeatures/*<T>*/()// where T : class, IGeometryAware<Point>
    {
        return GetFeatures/*<T>*/(null);
    }

    public List<Field>? GetFields()
    {
        return DataSource?.Fields;
    }

    public FeatureSet<Point>? GetFeatures/*<TGeometryAware>*/(Geometry<Point>? geometry) //where TGeometryAware : class, IGeometryAware<Point>
    {
        return DataSource.GetAsFeatureSet(geometry);

        ////if (DataSource as VectorDataSource<TGeometryAware> != null)
        ////{
        ////    //return (DataSource as VectorDataSource<TGeometryAware>)!.GetGeometryAwares(geometry);
        ////    return (DataSource as VectorDataSource<TGeometryAware>)!.GetAsFeatureSet(geometry);
        ////}

        //return null;
    }

    #endregion


    //POTENTIALLY ERROR PROUNE; formattedText is always RTL




    private Image? DrawLabels(List<Feature<Point>> features, double width, double height/*, Func<WpfPoint, WpfPoint> mapToScreen*/)
    {
        if (features.IsNullOrEmpty())
            return null;

        List<WpfPoint> mapCoordinates = features.ConvertAll<WpfPoint>(
                  (g) =>
                  {
                      var point = this.Labels.PositionFunc(g.TheGeometry);
                      return new WpfPoint(point.Points[0].X, point.Points[0].Y);
                  }).ToList();

        DrawingVisual drawingVisual = new DrawingVisual();

        using (DrawingContext drawingContext = drawingVisual.RenderOpen())
        {
            var typeface = new Typeface(this.Labels.FontFamily, FontStyles.Normal, FontWeights.Normal, FontStretches.Normal);

            var flowDirection = this.Labels.IsRtl ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;

            var culture = System.Globalization.CultureInfo.CurrentCulture;

            var backgroundBrush = new SolidColorBrush(Color.FromArgb(50, 255, 255, 255));

            for (int i = 0; i < features.Count; i++)
            {
                var label = features[i].Label;

                if (string.IsNullOrEmpty(label))
                    continue;

                FormattedText formattedText =
                    new FormattedText(label, culture, flowDirection, typeface, this.Labels.FontSize, this.Labels.Foreground, 96);

                WpfPoint location = /*mapToScreen*/(mapCoordinates[i]);

                drawingContext.DrawRectangle(backgroundBrush, null, new Rect(location, new Size(formattedText.Width, formattedText.Height)));
                drawingContext.DrawText(formattedText, new WpfPoint(location.X - formattedText.Width / 2.0, location.Y - formattedText.Height / 2.0));
            }
        }

        Image image = Helpers.ImageUtility.Create(width, height, drawingVisual);

        image.Tag = new LayerTag(-1) { Layer = this, IsTiled = false };

        this.BindWithFrameworkElement(image);

        return image;
    }

    private void DrawLabels(List<Feature<Point>> features, System.Drawing.Bitmap image/*, Func<IPoint, IPoint> mapToScreen*/)
    {
        if (features.IsNullOrEmpty())
            return;

        var mapCoordinates = features.ConvertAll(
                  (g) =>
                  {
                      return this.Labels.PositionFunc(g.TheGeometry).AsPoint();
                  }).ToList();

        var font = new System.Drawing.Font(this.Labels.FontFamily.FamilyNames.First().Value, this.Labels.FontSize);

        var graphic = System.Drawing.Graphics.FromImage(image);

        graphic.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

        graphic.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

        graphic.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

        graphic.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;

        var typeface = new Typeface(this.Labels.FontFamily, FontStyles.Normal, FontWeights.Normal, FontStretches.Normal);

        var flowDirection = this.Labels.IsRtl ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;

        var culture = System.Globalization.CultureInfo.CurrentCulture;

        var backgroundBrush = BrushHelper.CreateGdiBrush(System.Drawing.Color.FromArgb(50, 255, 255, 255), 1);

        for (int i = 0; i < features.Count; i++)
        {
            var label = features[i].Label;

            if (string.IsNullOrEmpty(label))
                continue;

            FormattedText formattedText =
            new FormattedText(label, culture, flowDirection, typeface, this.Labels.FontSize, this.Labels.Foreground, 96);

            var location = /*mapToScreen*/(mapCoordinates[i]);

            graphic.FillRectangle(backgroundBrush, (int)location.X, (int)location.Y, (int)formattedText.Width, (int)formattedText.Height);

            graphic.DrawString(label, font, Labels.Foreground.AsGdiBrush(), (float)location.X, (float)location.Y);
        }

        graphic.Flush();
    }

    private System.Drawing.Bitmap? DrawLabel(int width, int height, List<string> labels, List<Geometry<Point>> positions/*, Func<IPoint, IPoint> transform*/)
    {
        System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(width, height);

        if (labels.Count != positions.Count)
            return null;

        if (labels.IsNullOrEmpty())
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

        var foreground = Labels.Foreground.AsGdiBrush();

        for (int i = 0; i < labels.Count; i++)
        {
            var label = labels[i];

            if (string.IsNullOrEmpty(label))
                continue;

            var location = /*transform*/(mapCoordinates[i]);

            graphic.DrawString(label, font, foreground, (float)location.X, (float)location.Y);
        }

        graphic.Flush();

        return bitmap;
    }
}
