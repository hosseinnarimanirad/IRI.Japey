using IRI.Jab.Common.Assets.Commands;
using IRI.Jab.Common.Presenter.Map;
using IRI.Extensions;
using IRI.Sta.Spatial.Analysis;
using IRI.Sta.Spatial.Primitives;
using IRI.Sta.Common.Primitives;
using System;
using System.Collections.Generic;
using IRI.Sta.Spatial.Model.GeoJsonFormat;
using IRI.Sta.SpatialReferenceSystem.MapProjections;
using System.Windows.Media;
using IRI.Ket.Persistence.DataSources;
using IRI.Sta.Spatial.Mapping;
using System.Windows.Media.Imaging;
using IRI.Sta.ShapefileFormat.ShapeTypes.Abstractions;

namespace IRI.Jab.Common.Model.Legend;

public static class DrawingItemLegendCommands
{
    private const string _removeToolTip = "حذف";

    private const string _editToolTip = "ویرایش";

    private const string _zoomToolTip = "بزرگ‌نمایی";

    private const string _saveAsShapefileToolTip = "ذخیره‌سازی در قالب شیپ‌فایل";
    private const string _saveAsGeoJsonToolTip = "ذخیره‌سازی در قالب ژئوجی‌سان";
    private const string _saveAsPngToolTip = "ذخیره‌سازی در قالب png";


    // ***************** Remove ******************
    // *******************************************
    public static ILegendCommand CreateRemoveDrawingItemLayer(MapPresenter map, DrawingItemLayer layer)
    {
        var result = new LegendCommand()
        {
            PathMarkup = IRI.Jab.Common.Assets.ShapeStrings.Appbar.appbarDelete,
            Layer = layer,
            ToolTip = _removeToolTip,
        };

        result.Command = new RelayCommand(param =>
        {
            map.RemoveDrawingItem(layer);

            //map.Refresh();
        });

        return result;
    }

    // ***************** Edit ********************
    // *******************************************
    public static ILegendCommand CreateEditDrawingItemLayer(MapPresenter map, DrawingItemLayer layer)
    {
        var result = new LegendCommand()
        {
            PathMarkup = IRI.Jab.Common.Assets.ShapeStrings.Appbar.appbarEdit,
            Layer = layer,
            ToolTip = _editToolTip,
        };

        result.Command = new RelayCommand(async param =>
        {
            var editResult = await map.EditAsync(layer.Geometry, map.MapSettings.EditingOptions);

            if (!(editResult.IsCanceled == true))
            {
                map.ClearLayer(layer);
            }

            if (editResult.HasNotNullResult())
            {
                layer.Geometry = editResult.Result;

                //shapeItem.AssociatedLayer = new VectorLayer(shapeItem.Title, new List<SqlGeometry>() { editResult.Result.AsSqlGeometry() }, VisualParameters.GetRandomVisualParameters(), LayerType.Drawing, RenderingApproach.Default, RasterizationApproach.DrawingVisual);

                map.ClearLayer(layer);
                map.AddLayer(layer);
                //map.SetLayer(layer);

                // 1400.03.08- remove highlighted geometry
                layer.IsSelectedInToc = false;
                //map.ClearLayer(layer.HighlightGeometryKey.ToString(), true, true);

                //map.Refresh();

                if (layer.OriginalSource != null)
                {
                    (layer.OriginalSource as IEditableVectorDataSource<Feature<Point>, Point>).Update(new Feature<Point>(editResult.Result) { Id = layer.Id });
                }
            }
        });

        return result;
    }

    // ***************** Export As Shapefile *****
    // *******************************************
    public static ILegendCommand CreateExportDrawingItemLayerAsShapefile(MapPresenter map, DrawingItemLayer layer)
    {
        var result = new LegendCommand()
        {
            PathMarkup = IRI.Jab.Common.Assets.ShapeStrings.Others.shapefile,
            Layer = layer,
            ToolTip = _saveAsShapefileToolTip,
        };

        result.Command = new RelayCommand(async param =>
        {
            try
            {
                var file = map.DialogService.ShowSaveFileDialog("*.shp|*.shp", null, layer.LayerName);

                if (string.IsNullOrWhiteSpace(file))
                    return;

                var esriShape = layer.Geometry.AsSqlGeometry().AsEsriShape();

                IRI.Sta.ShapefileFormat.Shapefile.Save(file, new List<IEsriShape>() { esriShape }, true, true);
            }
            catch (Exception ex)
            {
                await map.DialogService.ShowMessageAsync(ex.Message, null, param);
            }
        });

        return result;
    }

    // ***************** Export As GeoJson *******
    // *******************************************
    public static ILegendCommand CreateExportDrawingItemLayerAsGeoJson(MapPresenter map, DrawingItemLayer layer)
    {
        var result = new LegendCommand()
        {
            PathMarkup = IRI.Jab.Common.Assets.ShapeStrings.Others.json,
            Layer = layer,
            ToolTip = _saveAsGeoJsonToolTip,
        };

        result.Command = new RelayCommand(async param =>
        {
            try
            {
                var file = map.DialogService.ShowSaveFileDialog("*.json|*.json", null, layer.LayerName);

                if (string.IsNullOrWhiteSpace(file))
                    return;

                var feature = GeoJsonFeature.Create(layer.Geometry.Project(SrsBases.GeodeticWgs84).AsGeoJson());

                GeoJsonFeatureSet featureSet = new GeoJsonFeatureSet() { Features = new List<GeoJsonFeature>() { feature }, TotalFeatures = 1 };

                featureSet.Save(file, false, false);
            }
            catch (Exception ex)
            {
                await map.DialogService.ShowMessageAsync(ex.Message, null, param);
            }
        });

        return result;
    }

    public static ILegendCommand CreateExportDrawingItemLayerAsPng(MapPresenter map, DrawingItemLayer layer)
    {
        var result = new LegendCommand()
        {
            PathMarkup = IRI.Jab.Common.Assets.ShapeStrings.Appbar.appbarImage,
            Layer = layer,
            ToolTip = _saveAsPngToolTip,
        };

        result.Command = new RelayCommand(async param =>
        {
            try
            {
                var fileName = map.DialogService.ShowSaveFileDialog("*.png|*.png", null, layer.LayerName);

                if (string.IsNullOrWhiteSpace(fileName))
                    return;

                var groundBoundingBox = layer.Geometry.GetBoundingBox().Expand(1.1);

                var currentScreenSize = WebMercatorUtility.ToScreenSize(map.CurrentZoomLevel, groundBoundingBox);

                var scale = WebMercatorUtility.GetGoogleMapScale(map.CurrentZoomLevel);

                var drawingVisual = await layer.AsDrawingVisual(groundBoundingBox, currentScreenSize.Width, currentScreenSize.Height, scale);

                RenderTargetBitmap image = new RenderTargetBitmap(currentScreenSize.Width, currentScreenSize.Height, 96, 96, PixelFormats.Pbgra32);

                image.Render(drawingVisual);

                var frame = BitmapFrame.Create(image);

                PngBitmapEncoder pngImage = new PngBitmapEncoder();

                pngImage.Frames.Add(frame);

                using (System.IO.Stream stream = System.IO.File.Create(fileName))
                {
                    pngImage.Save(stream);
                }
            }
            catch (Exception ex)
            {
                await map.DialogService.ShowMessageAsync(ex.Message, null, param);
            }
        });

        return result;
    }

    // ***************** Exterior Ring ***********
    // *******************************************
    public static ILegendCommand CreateGetExteriorRingCommand(MapPresenter map, DrawingItemLayer layer)
    {
        var result = new LegendCommand()
        {
            PathMarkup = IRI.Jab.Common.Assets.ShapeStrings.SegoePrint.exteriorRing,
            Layer = layer,
            ToolTip = "حلقه خارجی",
        };

        result.Command = new RelayCommand(async param =>
        {
            try
            {
                var geometry = layer.Geometry.AsSqlGeometry().STExteriorRing();

                map.AddDrawingItem(geometry.AsGeometry(), $"{layer.LayerName}-ExteriorRing");
            }
            catch (Exception ex)
            {
                await map.DialogService.ShowMessageAsync(ex.Message, null, param);
            }
        });

        return result;
    }

    // ***************** Envelope ****************
    // *******************************************
    public static ILegendCommand CreateGetEnvelopeCommand(MapPresenter map, DrawingItemLayer layer)
    {
        var result = new LegendCommand()
        {
            PathMarkup = IRI.Jab.Common.Assets.ShapeStrings.SegoePrint.envelope,
            Layer = layer,
            ToolTip = "مستطیل دربرگیرنده",
        };

        result.Command = new RelayCommand(async param =>
        {
            try
            {
                var geometry = layer.Geometry.AsSqlGeometry().STEnvelope();

                map.AddDrawingItem(geometry.AsGeometry(), $"{layer.LayerName}-Envelope");
            }
            catch (Exception ex)
            {
                await map.DialogService.ShowMessageAsync(ex.Message, null, param);
            }
        });

        return result;
    }

    // ***************** Convex Hull *************
    // *******************************************
    public static ILegendCommand CreateGetConvexHullCommand(MapPresenter map, DrawingItemLayer layer)
    {
        var result = new LegendCommand()
        {
            PathMarkup = IRI.Jab.Common.Assets.ShapeStrings.SegoePrint.convexHull,
            Layer = layer,
            ToolTip = "پوش محدب",
        };

        result.Command = new RelayCommand(async param =>
        {
            try
            {
                var geometry = layer.Geometry.AsSqlGeometry().STConvexHull();

                map.AddDrawingItem(geometry.AsGeometry(), $"{layer.LayerName}-ConvexHull");
            }
            catch (Exception ex)
            {
                await map.DialogService.ShowMessageAsync(ex.Message, null, param);
            }
        });

        return result;
    }

    // ***************** Boundary ****************
    // *******************************************
    public static ILegendCommand CreateGetBoundaryCommand(MapPresenter map, DrawingItemLayer layer)
    {
        var result = new LegendCommand()
        {
            PathMarkup = IRI.Jab.Common.Assets.ShapeStrings.SegoePrint.boundary,
            Layer = layer,
            ToolTip = "مرز",
        };

        result.Command = new RelayCommand(async param =>
        {
            try
            {
                var geometry = layer.Geometry.AsSqlGeometry().STBoundary();

                map.AddDrawingItem(geometry.AsGeometry(), $"{layer.LayerName}-Boundary");
            }
            catch (Exception ex)
            {
                await map.DialogService.ShowMessageAsync(ex.Message, null, param);
            }
        });

        return result;
    }

    // ***************** Break into geometries ***
    // *******************************************
    public static ILegendCommand CreateBreakIntoGeometriesCommand(MapPresenter map, DrawingItemLayer layer)
    {
        var result = new LegendCommand()
        {
            PathMarkup = IRI.Jab.Common.Assets.ShapeStrings.SegoePrint.extractGeometries,
            Layer = layer,
            ToolTip = "تفکیک به هندسه",
        };

        result.Command = new RelayCommand(async param =>
        {
            try
            {
                var geometries = layer.Geometry.AsSqlGeometry().GetGeometries();

                var counter = 0;

                foreach (var geo in geometries)
                {
                    map.AddDrawingItem(geo.AsGeometry(), $"{layer.LayerName} Geometry #{counter++}");
                }
            }
            catch (Exception ex)
            {
                await map.DialogService.ShowMessageAsync(ex.Message, null, param);
            }
        });

        return result;
    }

    // ***************** Extract points **********
    // *******************************************
    public static ILegendCommand CreateBreakIntoPointsCommand(MapPresenter map, DrawingItemLayer layer)
    {
        var result = new LegendCommand()
        {
            PathMarkup = IRI.Jab.Common.Assets.ShapeStrings.SegoePrint.extractPoints,
            Layer = layer,
            ToolTip = "تفکیک به نقاط",
        };

        result.Command = new RelayCommand(async param =>
        {
            try
            {
                var pointCollection = IRI.Ket.SqlServerSpatialExtension.SqlSpatialUtility.MakePointCollection(layer.Geometry.GetAllPoints());

                map.AddDrawingItem(pointCollection.AsGeometry(), $"{layer.LayerName} Points");

            }
            catch (Exception ex)
            {
                await map.DialogService.ShowMessageAsync(ex.Message, null, param);
            }
        });

        return result;
    }

    // ***************** Simplify by Angle *******
    // *******************************************
    public static ILegendCommand CreateSimplifyByAngleCommand(MapPresenter map, DrawingItemLayer layer)
    {
        var result = new LegendCommand()
        {
            PathMarkup = IRI.Jab.Common.Assets.ShapeStrings.Appbar.appbarFlag,
            Layer = layer,
            ToolTip = "ساده‌سازی روش زاویه",
        };

        result.Command = new RelayCommand(async param =>
        {
            try
            {
                var simplified = layer.Geometry.Simplify(SimplificationType.CumulativeAngle, new SimplificationParamters() { AngleThreshold = 0.99, Retain3Points = true });
                //VisualSimplification.sim layer.Geometry.Simplify()
                map.AddDrawingItem(simplified, $"{layer.LayerName} simplified-{map.CurrentZoomLevel}");

            }
            catch (Exception ex)
            {
                await map.DialogService.ShowMessageAsync(ex.Message, null, param);
            }
        });

        return result;
    }

    // ***************** Simplify by Area ********
    // *******************************************
    public static ILegendCommand CreateSimplifyByAreaCommand(MapPresenter map, DrawingItemLayer layer)
    {
        var result = new LegendCommand()
        {
            PathMarkup = IRI.Jab.Common.Assets.ShapeStrings.Appbar.appbarFlag,
            Layer = layer,
            ToolTip = "ساده‌سازی روش مساحت",
        };

        result.Command = new RelayCommand(async param =>
        {
            try
            {
                var simplified = layer.Geometry.Simplify(SimplificationType.CumulativeTriangleRoutine, map.CurrentZoomLevel, new SimplificationParamters() { Retain3Points = true });
                //VisualSimplification.sim layer.Geometry.Simplify()
                map.AddDrawingItem(simplified, $"{layer.LayerName} simplified-{map.CurrentZoomLevel}");

            }
            catch (Exception ex)
            {
                await map.DialogService.ShowMessageAsync(ex.Message, null, param);
            }
        });

        return result;
    }

    // ***************** Duplicate ***************
    // *******************************************
    public static ILegendCommand CreateCloneDrawingItemCommand(MapPresenter map, DrawingItemLayer layer)
    {
        var result = new LegendCommand()
        {
            PathMarkup = IRI.Jab.Common.Assets.ShapeStrings.Appbar.appbarPageCopy,
            Layer = layer,
            ToolTip = "ایجاد کپی از عارضه",
        };

        result.Command = new RelayCommand(async param =>
        {
            try
            {
                var cloned = layer.Geometry.Clone();

                map.AddDrawingItem(cloned, $"{layer.LayerName} cloned-{map.CurrentZoomLevel}");

            }
            catch (Exception ex)
            {
                await map.DialogService.ShowMessageAsync(ex.Message, null, param);
            }
        });

        return result;
    }

    // ***************** Simplifications **********
    // *******************************************
    public static ILegendCommand CreateSimplifyByVWCommand(MapPresenter map, DrawingItemLayer layer)
    {
        var result = new LegendCommand()
        {
            PathMarkup = IRI.Jab.Common.Assets.ShapeStrings.Appbar.appbarFlag,
            Layer = layer,
            ToolTip = "ساده‌سازی روش ویزوال",
        };

        result.Command = new RelayCommand(async param =>
        {
            try
            {
                var simplified = layer.Geometry.Simplify(SimplificationType.VisvalingamWhyatt, map.CurrentZoomLevel, new SimplificationParamters() { Retain3Points = true });
                //VisualSimplification.sim layer.Geometry.Simplify()
                map.AddDrawingItem(simplified, $"{layer.LayerName} simplified-VW-{map.CurrentZoomLevel}");

            }
            catch (Exception ex)
            {
                await map.DialogService.ShowMessageAsync(ex.Message, null, param);
            }
        });

        return result;
    }

    public static ILegendCommand CreateSimplifyByRDPCommand(MapPresenter map, DrawingItemLayer layer)
    {
        var result = new LegendCommand()
        {
            PathMarkup = IRI.Jab.Common.Assets.ShapeStrings.Appbar.appbarFlag,
            Layer = layer,
            ToolTip = "ساده‌سازی روش داگلاس",
        };

        result.Command = new RelayCommand(async param =>
        {
            try
            {
                var simplified = layer.Geometry.Simplify(SimplificationType.RamerDouglasPeucker, map.CurrentZoomLevel, new SimplificationParamters() { Retain3Points = true });
                //VisualSimplification.sim layer.Geometry.Simplify()
                map.AddDrawingItem(simplified, $"{layer.LayerName} simplified-RDP-{map.CurrentZoomLevel}");

            }
            catch (Exception ex)
            {
                await map.DialogService.ShowMessageAsync(ex.Message, null, param);
            }
        });

        return result;
    }

    internal static List<Func<MapPresenter, DrawingItemLayer, ILegendCommand>> GetDefaultTextLayerCommands()
    {
        return new List<Func<MapPresenter, DrawingItemLayer, ILegendCommand>>()
        {
            CreateRemoveDrawingItemLayer,
            (p,l)=>LegendCommand. CreateZoomToExtentCommandFunc(p,l)
        };
    }
}
