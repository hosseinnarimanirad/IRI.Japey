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
using IRI.Maptor.Jab.Common.Events;
using IRI.Maptor.Jab.Common.Helpers;
using IRI.Maptor.Sta.Spatial.Helpers;
using IRI.Maptor.Sta.Common.Primitives;
using IRI.Maptor.Sta.Spatial.Primitives;
using IRI.Maptor.Sta.Persistence.DataSources;
using IRI.Maptor.Sta.Persistence.Abstractions;
using IRI.Maptor.Jab.Common.Cartography.Symbologies;
using IRI.Maptor.Jab.Common.Cartography.RenderingStrategies;

using WpfPoint = System.Windows.Point;
using Point = IRI.Maptor.Sta.Common.Primitives.Point;

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

    protected LayerType _type;
    public override LayerType Type
    {
        get { return _type; }
        //protected set
        //{
        //    _type = value;
        //    RaisePropertyChanged();
        //}
    }


    //private BoundingBox _extent;
    //public override BoundingBox Extent
    //{
    //    get { return _extent; }
    //    protected set
    //    {
    //        _extent = value;
    //        RaisePropertyChanged();
    //    }
    //}

    private RenderingApproach _rendering = RenderingApproach.Default;
    public override RenderingApproach Rendering { get => _rendering;/* private set;*/ }

    protected RasterizationApproach _rasterizationApproach = RasterizationApproach.DrawingVisual;
    public override RasterizationApproach ToRasterTechnique { get => _rasterizationApproach; /*protected set;*/ }


    private int _numberOfSelectedFeatures;
    public int NumberOfSelectedFeatures
    {
        get { return _numberOfSelectedFeatures; }
        set
        {
            _numberOfSelectedFeatures = value;
            RaisePropertyChanged();
            RaisePropertyChanged(nameof(HasSelectedFeatures));

            this.OnSelectedFeaturesChanged?.Invoke(this, new CustomEventArgs<VectorLayer>(this));
        }
    }

    public bool HasSelectedFeatures
    {
        get { return NumberOfSelectedFeatures > 0; }
    }


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
        RasterizationApproach toRasterTechnique, ScaleInterval visibleRange, SimplePointSymbolizer? pointSymbol = null, LabelParameters? labeling = null)
    {
        Initialize(layerName, dataSource, parameters, type, rendering, toRasterTechnique, visibleRange, pointSymbol, labeling);
    }

    public VectorLayer(string layerName, IVectorDataSource dataSource, List<ISymbolizer> symbolizers, LayerType type, RenderingApproach rendering,
        RasterizationApproach toRasterTechnique)
    {
        this.LayerId = Guid.NewGuid();

        this.DataSource = dataSource;

        this._rendering = rendering;

        this._rasterizationApproach = toRasterTechnique;

        this._type = type;

        if (dataSource.GeometryType.AsLayerType() is not null)
        {
            this._type = type | dataSource.GeometryType.AsLayerType().Value; /*GetGeometryType(geometries.FirstOrDefault(g => g != null))*/;
        }
        else
        {
            this._type = type;
        }

        this.Extent = dataSource.WebMercatorExtent;

        this.LayerName = layerName;

        //this.VisualParameters = parameters;

        this.Symbolizers.AddRange(symbolizers);

        var simpleSymbolizer = symbolizers.FirstOrDefault(s => s is SimpleSymbolizer);

        if (simpleSymbolizer is not null)
            this.VisualParameters = (simpleSymbolizer as SimpleSymbolizer)!.Param;

        var labelSymbolizer = (symbolizers.FirstOrDefault(s => s is LabelSymbolizer) as LabelSymbolizer);

        if (labelSymbolizer is not null)
            this.Labels = (labelSymbolizer as LabelSymbolizer).Labels;

        this.VisibleRange = ScaleInterval.All;

        //this.Labels = labeling;

        //Check for missing visibleRange
        //if (this.Labels != null)
        //{
        //    if (this.Labels.VisibleRange == null)
        //    {
        //        this.Labels.VisibleRange = visibleRange;
        //    }

        //    this.Symbolizers.Add(new LabelSymbolizer(labeling));
        //}

        //this.VisibleRange = (visibleRange == null) ? ScaleInterval.All : visibleRange;
    }

    private void Initialize(string layerName,
                            IVectorDataSource dataSource,
                            VisualParameters parameters,
                            LayerType type,
                            RenderingApproach rendering,
                            RasterizationApproach toRasterTechnique,
                            ScaleInterval visibleRange,
                            SimplePointSymbolizer? pointSymbol,
                            LabelParameters? labeling)
    {
        this.LayerId = Guid.NewGuid();

        this.DataSource = dataSource;

        this._rendering = rendering;

        this._rasterizationApproach = toRasterTechnique;

        this._type = type;

        if (dataSource.GeometryType.AsLayerType() is not null)
        {
            this._type = type | dataSource.GeometryType.AsLayerType().Value; /*GetGeometryType(geometries.FirstOrDefault(g => g != null))*/;
        }
        else
        {
            this._type = type;
        }

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
        return $"{Enum.GetName(this.Type)} - {this.DataSource?.ToString()}";
    }

    public void BindWithFrameworkElement(FrameworkElement element)
    {
        Binding binding4 = new Binding() { Source = this, Path = new PropertyPath("VisualParameters.Visibility"), Mode = BindingMode.TwoWay };
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

    public async Task<List<Feature<Point>>> GetRenderReadyFeatures(BoundingBox mapExtent, double mapScale, double screenWidth, double screenHeight)
    {
        var feature = await this.DataSource.GetAsFeatureSetAsync(mapScale, mapExtent);

        if (feature is null || feature.HasNoGeometry())
            return new List<Feature<Point>>();

        //double xScale = imageWidth / mapExtent.Width;
        //double yScale = imageHeight / mapExtent.Height;
        //double scale = xScale > yScale ? yScale : xScale;
        //Func<Point, Point> mapToScreen = new Func<Point, Point>(p => new Point((p.X - mapExtent.XMin) * scale, -(p.Y - mapExtent.YMax) * scale));
        var mapToScreen = CreateMapToScreenMapFunc(mapExtent, screenWidth, screenHeight);

        return feature.Transform(mapToScreen).Features;
    }


    #region Raster Save And Export Methods

    public async Task<System.Drawing.Bitmap?> AsGdiBitmapAsync(BoundingBox mapExtent, double mapScale, double screenWidth, double screenHeight)
    {
        var features = await GetRenderReadyFeatures(mapExtent, mapScale, screenWidth, screenHeight);

        if (features.IsNullOrEmpty())
            return null;

        return new GdiBitmapRenderStrategy(Symbolizers).AsGdiBitmap(features, mapScale, screenWidth, screenHeight);
    }


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
                var image = await AsGdiBitmapAsync(tile.WebMercatorExtent, scale, 256, 256);

                if (image is null)
                    continue;

                image.Save($"{directory}\\{tile.ZoomLevel}, {tile.RowNumber}, {tile.ColumnNumber}.jpg");
            }
        }
    }

    public async Task SaveAsPng(string fileName, BoundingBox mapExtent, double imageWidth, double imageHeight, double mapScale)
    {
        switch (this.ToRasterTechnique)
        {
            case RasterizationApproach.StreamGeometry:
            case RasterizationApproach.DrawingVisual:
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
                var image = await AsGdiBitmapAsync(mapExtent, mapScale, imageWidth, imageHeight);

                if (image is null)
                    return;

                image.Save(fileName);

                image.Dispose();
                break;
        }
    }

    public async Task<List<DrawingVisual>> AsDrawingVisual(BoundingBox mapExtent, double imageWidth, double imageHeight, double mapScale)
    {
        var features = await GetRenderReadyFeatures(mapExtent, mapScale, imageWidth, imageHeight);

        if (features.IsNullOrEmpty())
            return [];

        return new DrawingVisualRenderStrategy(Symbolizers).AsDrawingVisual(features, mapScale);
    }

    public async Task<RenderTargetBitmap?> AsRenderTargetBitmap(BoundingBox mapExtent, double imageWidth, double imageHeight, double mapScale)
    {
        //var features = await GetRenderReadyFeatures(mapExtent, mapScale, imageWidth, imageHeight);
        //if (features.IsNullOrEmpty())
        //    return null;
        //var drawingVisuals = new DrawingVisualRenderStrategy(Symbolizers).AsDrawingVisual(features, mapScale);

        var drawingVisuals = await AsDrawingVisual(mapExtent, imageWidth, imageHeight, mapScale);

        if (drawingVisuals.IsNullOrEmpty())
            return null;

        return ImageUtility.Render(drawingVisuals, (int)imageWidth, (int)imageHeight);
    }

    #endregion


    #region Vector Export Methods

    public void ExportAsShapefile(string shpFileName)
    {
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

    public FeatureSet<Point>? GetFeatures() => GetFeatures(null);

    public List<Field>? GetFields() => DataSource?.Fields;

    public FeatureSet<Point>? GetFeatures(Geometry<Point>? geometry) => DataSource.GetAsFeatureSet(geometry);

    #endregion


    private Image? DrawLabels(List<Feature<Point>> features, double width, double height)
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

    private void DrawLabels(List<Feature<Point>> features, System.Drawing.Bitmap image)
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

    private System.Drawing.Bitmap? DrawLabel(int width, int height, List<string> labels, List<Geometry<Point>> positions)
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

    #region Events

    public event EventHandler<CustomEventArgs<VectorLayer>> OnSelectedFeaturesChanged;

    #endregion
}
