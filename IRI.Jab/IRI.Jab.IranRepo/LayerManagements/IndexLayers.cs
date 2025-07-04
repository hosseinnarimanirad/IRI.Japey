using System.Windows.Media;

using IRI.Extensions;
using IRI.Jab.Common;
using IRI.Jab.Common.Enums;
using IRI.Jab.Common.Model;
using IRI.Sta.Common.Helpers;
using IRI.Sta.Spatial.Primitives;
using IRI.Sta.Common.Primitives;
using IRI.Sta.Spatial.MapIndexes;
using IRI.Jab.Common.Model.Legend;
using IRI.Jab.Common.Presenter.Map;
using IRI.Sta.SpatialReferenceSystem;
using IRI.Sta.Persistence.DataSources;
using IRI.Sta.SpatialReferenceSystem.MapProjections;

namespace IRI.Jab.IranRepo;


public static class IndexLayers
{
    public static VectorLayer GetLayerFromShapefile(string layerName, string filePath, string color)
    {
        Func<Point, Point> mapFunc = MapProjects.GeodeticWgs84ToWebMercator;

        var features = ShapefileDataSourceFactory.Create(filePath, new WebMercator());

        var geo = features.GetGeometries();

        //ShapefileDataSource<SqlFeature> shapes = new ShapefileDataSource<SqlFeature>(filePath, "Geo", SridHelper.GeodeticWGS84, Encoding.UTF8, true, null);

        //var geo = features.GetGeometries().Select(g => g.Transform(mapFunc, SridHelper.WebMercator)).ToList();

        return new VectorLayer(layerName, geo, new VisualParameters(null, color, 1, 1), LayerType.VectorLayer, RenderingApproach.Default, RasterizationApproach.DrawingVisual);
    }

    public static VectorLayer GetIndex250kLayer()
    {
        var jsonString = ZipFileHelper.OpenAndReadAsString("iriRepo.dll", "IriIndex250k");

        var source = OrdinaryJsonListSource<Index250k>.CreateFromJsonString(jsonString, i => i.AsFeature(), i => i.SheetNameEn);
         
        VisualParameters parameters = new VisualParameters(null, "#FFEA4333", 5, .9) { Visibility = System.Windows.Visibility.Collapsed };

        return new VectorLayer("اندکس ۲۵۰ هزار", source, parameters, LayerType.VectorLayer, RenderingApproach.Default, RasterizationApproach.DrawingVisual, ScaleInterval.Create(4))
        {
            ShowInToc = false,
            CanUserDelete = false
        };

    }

    public static VectorLayer GetIndex100kLayer()
    {
        var jsonString = ZipFileHelper.OpenAndReadAsString("iriRepo.dll", "IriIndex100k");

        var source = OrdinaryJsonListSource<Index100k>.CreateFromJsonString(jsonString, i => i.AsFeature(), i => i.SheetNameEn);
         
        VisualParameters parameters = new VisualParameters(null, "#FFEA4333", 3, .9) { Visibility = System.Windows.Visibility.Collapsed };

        return new VectorLayer("اندکس ۱۰۰ هزار", source, parameters, LayerType.VectorLayer, RenderingApproach.Default, RasterizationApproach.GdiPlus, ScaleInterval.Create(5))
        {
            ShowInToc = false,
            CanUserDelete = false
        };

    }

    public static VectorLayer GetIndex50kLayer()
    {
        var jsonString = ZipFileHelper.OpenAndReadAsString("iriRepo.dll", "IriIndex50k");

        var source = OrdinaryJsonListSource<Index50k>.CreateFromJsonString(jsonString, i => i.AsFeature(), i => i.SheetNumber);
         
        VisualParameters parameters = new VisualParameters(null, "#88EA4333", 2, .8) { Visibility = System.Windows.Visibility.Collapsed };

        return new VectorLayer("اندکس ۵۰ هزار", source, parameters, LayerType.VectorLayer, RenderingApproach.Default, RasterizationApproach.GdiPlus, ScaleInterval.Create(9))
        {
            ShowInToc = false,
            CanUserDelete = false
        };

    }

    public static VectorLayer GetIndex25kLayer()
    {
        var jsonString = ZipFileHelper.OpenAndReadAsString("iriRepo.dll", "IriIndex25k");

        var source = OrdinaryJsonListSource<Index25k>.CreateFromJsonString(jsonString, i => i.AsFeature(), i => i.SheetNumber);
         
        VisualParameters parameters = new VisualParameters(null, "#88FF8130", 2, .8) { Visibility = System.Windows.Visibility.Collapsed, DashStyle = DashStyles.Dot };

        return new VectorLayer("اندکس ۲۵ هزار", source, parameters, LayerType.VectorLayer, RenderingApproach.Default, RasterizationApproach.GdiPlus, ScaleInterval.Create(10))
        {
            ShowInToc = false,
            CanUserDelete = false
        };

    }

    public static List<ILayer> GetLayers(MapPresenter map)
    {
        var index250k = IndexLayers.GetIndex250kLayer();
         
        var fontFamily = new FontFamily("Times New Roman");

        var index250kLabels = new LabelParameters(ScaleInterval.Create(7), 12, index250k.VisualParameters.Stroke, fontFamily, i => i.GetCentroidPlus()) { IsRtl = false };

        index250k.Commands = GetCommands<Index250k>(map, index250k, index250kLabels);

        //100k
        var index100k = IndexLayers.GetIndex100kLayer();

        //var index100kLegend = new MapLegendItemWithOptionsModel(index100k);

        var index100kLabels = new LabelParameters(ScaleInterval.Create(9), 12, index250k.VisualParameters.Stroke, fontFamily, i => i.GetCentroidPlus()) { IsRtl = false };

        index100k.Commands = GetCommands<Index100k>(map, index100k, index100kLabels);

        return new List<ILayer>() { index250k, index100k };
         
    }

    private static List<ILegendCommand> GetCommands<T>(MapPresenter map, VectorLayer layer, LabelParameters label)
        where T : class, IGeometryAware<Point>
    {
        return new List<ILegendCommand>()
        {
            LegendCommand.CreateZoomToExtentCommand(map, layer),
            LegendCommand.CreateShowAttributeTable/*<T>*/(map,layer),
            LegendCommand.CreateSelectByDrawing/*<T>*/(map,layer),
            LegendCommand.CreateClearSelected(map,layer),
            LegendToggleCommand.CreateToggleLayerLabelCommand(map, layer, label)
        };
    }



    public static List<Index250k> GetIndex250kSource(Point geodeticPoint)
    {
        var geometry = MapProjects.GeodeticWgs84ToWebMercator(geodeticPoint).AsGeometry(SridHelper.WebMercator);

        var jsonString = ZipFileHelper.OpenAndReadAsString("iriRepo.dll", "IriIndex250k");

        return OrdinaryJsonListSource<Index250k>.CreateFromJsonString(jsonString, i => i.AsFeature(), i => i.SheetNameEn).GetGeometryAwares(geometry);
    }

    public static List<Index100k> GetIndex100kSource(Point geodeticPoint)
    {
        var geometry = MapProjects.GeodeticWgs84ToWebMercator(geodeticPoint).AsGeometry(SridHelper.WebMercator);

        var jsonString = ZipFileHelper.OpenAndReadAsString("iriRepo.dll", "IriIndex100k");

        return OrdinaryJsonListSource<Index100k>.CreateFromJsonString(jsonString, i => i.AsFeature(), i => i.SheetNameEn).GetGeometryAwares(geometry);
    }

    public static List<Index50k> GetIndex50kSource(Point geodeticPoint)
    {
        var geometry = MapProjects.GeodeticWgs84ToWebMercator(geodeticPoint).AsGeometry(SridHelper.WebMercator);

        var jsonString = ZipFileHelper.OpenAndReadAsString("iriRepo.dll", "IriIndex50k");

        return OrdinaryJsonListSource<Index50k>.CreateFromJsonString(jsonString, i => i.AsFeature(), i => i.SheetNameEn).GetGeometryAwares(geometry);
    }



    //
    public static List<ILayer> Get2kAndLowerIndexLayers(MapPresenter map, int utmZone)
    {
        return new List<ILayer>() { Get2kDynamicIndexBlock(map, utmZone), Get2kDynamicIndexSheet(map, utmZone), Get1kDynamicIndex(map, utmZone), Get500DynamicIndex(map, utmZone) };
    }

    public static VectorLayer Get2kDynamicIndexBlock(MapPresenter map, int utmZone)
    {
        var fontFamily = new FontFamily("Times New Roman");

        UtmGridDataSource source = UtmGridDataSource.Create(UtmIndexType.Ncc2kBlock, utmZone);

        var label = new LabelParameters(ScaleInterval.Create(8), 14, Brushes.Red, fontFamily, i => i?.GetCentroidPlus()/*?.STCentroid()*/) { IsRtl = false };

        VisualParameters parameters = new VisualParameters(null, "#88EA4333", 2, .8) { Visibility = System.Windows.Visibility.Collapsed };

        var layer =
            new VectorLayer(
                "بلوک‌های ۲ هزار",
                source,
                parameters,
                LayerType.VectorLayer,
                RenderingApproach.Default,
                RasterizationApproach.DrawingVisual,
                ScaleInterval.Create(6),
                null,
                null)
            {
                ShowInToc = false,
                CanUserDelete = false
            };

        layer.Commands = GetCommands<UtmSheet>(map, layer, label);

        return layer;
    }

    public static VectorLayer Get2kDynamicIndexSheet(MapPresenter map, int utmZone)
    {
        var fontFamily = new FontFamily("Times New Roman");

        UtmGridDataSource source = UtmGridDataSource.Create(UtmIndexType.Ncc2kSheet, utmZone);

        var label = new LabelParameters(ScaleInterval.Create(11), 13, Brushes.Red, fontFamily, i => i?.GetCentroidPlus()) { IsRtl = false };

        VisualParameters parameters = new VisualParameters(null, "#88EA4333", 2, .8) { Visibility = System.Windows.Visibility.Collapsed };

        var layer =
            new VectorLayer(
                "اندکس ۲ هزار",
                source,
                parameters,
                LayerType.VectorLayer,
                RenderingApproach.Default,
                RasterizationApproach.DrawingVisual,
                ScaleInterval.Create(11),
                null,
                null)
            {
                ShowInToc = false,
                CanUserDelete = false
            };

        layer.Commands = GetCommands<UtmSheet>(map, layer, label);

        return layer;
    }

    public static VectorLayer Get1kDynamicIndex(MapPresenter map, int utmZone)
    {
        var fontFamily = new FontFamily("Times New Roman");

        UtmGridDataSource source = UtmGridDataSource.Create(UtmIndexType.Ncc1k, utmZone);

        var label = new LabelParameters(ScaleInterval.Create(14), 14, Brushes.Red, fontFamily, i => i?.GetCentroidPlus()) { IsRtl = false };

        VisualParameters parameters = new VisualParameters(null, "#88EA4333", 2, .8) { Visibility = System.Windows.Visibility.Collapsed };

        var layer =
            new VectorLayer(
                "اندکس ۱ هزار",
                source,
                parameters,
                LayerType.VectorLayer,
                RenderingApproach.Default,
                RasterizationApproach.DrawingVisual,
                ScaleInterval.Create(13),
                null,
                null)
            {
                ShowInToc = false,
                CanUserDelete = false
            };

        layer.Commands = GetCommands<UtmSheet>(map, layer, label);

        return layer;
    }

    public static VectorLayer Get500DynamicIndex(MapPresenter map, int utmZone)
    {
        var fontFamily = new FontFamily("Times New Roman");

        UtmGridDataSource source = UtmGridDataSource.Create(UtmIndexType.Ncc500, utmZone);

        var label = new LabelParameters(ScaleInterval.Create(15), 14, Brushes.Red, fontFamily, i => i?.GetCentroidPlus()) { IsRtl = false };

        VisualParameters parameters = new VisualParameters(null, "#88EA4333", 2, .8) { Visibility = System.Windows.Visibility.Collapsed };

        var layer =
            new VectorLayer(
                "اندکس ۵۰۰",
                source,
                parameters,
                LayerType.VectorLayer,
                RenderingApproach.Default,
                RasterizationApproach.DrawingVisual,
                ScaleInterval.Create(14),
                null,
                null)
            {
                ShowInToc = false,
                CanUserDelete = false
            };

        layer.Commands = GetCommands<UtmSheet>(map, layer, label);

        return layer;
    }



    public static List<ILayer> Get50kAndHigherIndexLayers(MapPresenter map)
    {
        return new List<ILayer>() { Get50kDynamicIndex(map), Get25kDynamicIndex(map), Get10kDynamicIndex(map), Get5kDynamicIndex(map) };
    }

    public static VectorLayer Get50kDynamicIndex(MapPresenter map)
    {
        var fontFamily = new FontFamily("Times New Roman");

        GridDataSource source50k = GridDataSource.Create(GeodeticIndexType.Ncc50k);

        var label = new LabelParameters(ScaleInterval.Create(9), 14, Brushes.Red, fontFamily, i => i?.GetCentroidPlus()) { IsRtl = false };

        VisualParameters parameters = new VisualParameters(null, "#88EA4333", 2, .8) { Visibility = System.Windows.Visibility.Collapsed };

        var layer50k =
            new VectorLayer(
                "اندکس ۵۰ هزار",
                source50k,
                parameters,
                LayerType.VectorLayer,
                RenderingApproach.Default,
                RasterizationApproach.DrawingVisual,
                ScaleInterval.Create(7),
                null,
                null)
            {
                ShowInToc = false,
                CanUserDelete = false
            };

        layer50k.Commands = GetCommands<GeodeticSheet>(map, layer50k, label);

        return layer50k;
    }

    public static VectorLayer Get25kDynamicIndex(MapPresenter map)
    {
        var fontFamily = new FontFamily("Times New Roman");

        GridDataSource source25k = GridDataSource.Create(GeodeticIndexType.Ncc25k);

        var label = new LabelParameters(ScaleInterval.Create(10, 19), 14, Brushes.Red, fontFamily, i => i?.GetCentroidPlus()) { IsRtl = false };

        VisualParameters parameters = new VisualParameters(null, "#88EA4333", 1, .8) { Visibility = System.Windows.Visibility.Collapsed };

        var layer25k =
            new VectorLayer(
                "اندکس ۲۵ هزار",
                source25k,
                parameters,
                LayerType.VectorLayer,
                RenderingApproach.Default,
                RasterizationApproach.DrawingVisual,
                ScaleInterval.Create(8),
                null,
                null)
            {
                ShowInToc = false,
                CanUserDelete = false
            };

        layer25k.Commands = GetCommands<GeodeticSheet>(map, layer25k, label);

        return layer25k;
    }

    public static VectorLayer Get10kDynamicIndex(MapPresenter map)
    {
        var fontFamily = new FontFamily("Times New Roman");

        var label = new LabelParameters(ScaleInterval.Create(11, 19), 14, Brushes.Red, fontFamily, i => i?.GetCentroidPlus()) { IsRtl = false };

        VisualParameters parameters = new VisualParameters(null, "#88EA4333", 1, .8) { Visibility = System.Windows.Visibility.Collapsed };

        var layer10k =
            new VectorLayer(
                "اندکس ۱۰ هزار",
                GridDataSource.Create(GeodeticIndexType.Ncc10k),
                parameters,
                LayerType.VectorLayer,
                RenderingApproach.Default,
                RasterizationApproach.DrawingVisual,
                ScaleInterval.Create(9),
                null,
                null)
            {
                ShowInToc = false,
                CanUserDelete = false
            };

        layer10k.Commands = GetCommands<GeodeticSheet>(map, layer10k, label);

        return layer10k;
    }

    public static VectorLayer Get5kDynamicIndex(MapPresenter map)
    {
        var fontFamily = new FontFamily("Times New Roman");

        var label = new LabelParameters(ScaleInterval.Create(12, 19), 14, Brushes.Red, fontFamily, i => i?.GetCentroidPlus()) { IsRtl = false };

        VisualParameters parameters = new VisualParameters(null, "#88EA4333", 1, .8) { Visibility = System.Windows.Visibility.Collapsed };

        var layer5k =
            new VectorLayer(
                "اندکس ۵ هزار",
                GridDataSource.Create(GeodeticIndexType.Ncc5k),
                parameters,
                LayerType.VectorLayer,
                RenderingApproach.Default,
                RasterizationApproach.DrawingVisual,
                ScaleInterval.Create(10),
                null,
                null)
            {
                ShowInToc = false,
                CanUserDelete = false
            };

        layer5k.Commands = GetCommands<GeodeticSheet>(map, layer5k, label);

        return layer5k;
    }
}
