using IRI.Jab.Common.Assets.Commands;
using IRI.Jab.Common.Presenter.Map;
using IRI.Ket.SpatialExtensions;
using IRI.Ket.SqlServerSpatialExtension.Model;
using IRI.Msh.Common.Analysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRI.Msh.Common.Extensions;
using IRI.Msh.Common.Model.GeoJson;
using IRI.Msh.CoordinateSystem.MapProjection;

namespace IRI.Jab.Common.Model.Legend
{
    public static class DrawingItemLegendCommands
    {
        private const string _removeToolTip = "حذف";

        private const string _editToolTip = "ویرایش";

        private const string _zoomToolTip = "بزرگ‌نمایی";

        private const string _saveAsShapefileToolTip = "ذخیره‌سازی در قالب شیپ‌فایل";
        private const string _saveAsGeoJsonToolTip = "ذخیره‌سازی در قالب ژئوجی‌سان";


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

                    //map.Refresh();

                    if (layer.OriginalSource != null)
                    {
                        layer.OriginalSource.Update(new SqlFeature(editResult.Result.AsSqlGeometry()) { Id = layer.Id });
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
                    var file = map.SaveFile("*.shp|*.shp");

                    if (string.IsNullOrWhiteSpace(file))
                        return;

                    var esriShape = layer.Geometry.AsSqlGeometry().AsEsriShape();

                    IRI.Ket.ShapefileFormat.Shapefile.Save(file, new List<Ket.ShapefileFormat.EsriType.IEsriShape>() { esriShape }, true, true);
                }
                catch (Exception ex)
                {
                    await map.ShowMessageAsync(null, ex.Message);
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
                    var file = map.SaveFile("*.json|*.json");

                    if (string.IsNullOrWhiteSpace(file))
                        return;

                    var feature = GeoJsonFeature.Create(layer.Geometry.Project(SrsBases.GeodeticWgs84).AsGeoJson());

                    GeoJsonFeatureSet featureSet = new GeoJsonFeatureSet() { Features = new List<GeoJsonFeature>() { feature }, TotalFeatures = 1 };

                    featureSet.Save(file, false, false);
                }
                catch (Exception ex)
                {
                    await map.ShowMessageAsync(null, ex.Message);
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
                    await map.ShowMessageAsync(null, ex.Message);
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
                    await map.ShowMessageAsync(null, ex.Message);
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
                    await map.ShowMessageAsync(null, ex.Message);
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
                    await map.ShowMessageAsync(null, ex.Message);
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
                    await map.ShowMessageAsync(null, ex.Message);
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
                    await map.ShowMessageAsync(null, ex.Message);
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
                    var simplified = layer.Geometry.Simplify(SimplificationType.AdditiveByAngle, new SimplificationParamters() { AngleThreshold = 0.98, Retain3Points = true });
                    //VisualSimplification.sim layer.Geometry.Simplify()
                    map.AddDrawingItem(simplified, $"{layer.Title} simplified-{map.CurrentZoomLevel}");

                }
                catch (Exception ex)
                {
                    await map.ShowMessageAsync(null, ex.Message);
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
                    var simplified = layer.Geometry.Simplify(SimplificationType.AdditiveByArea, map.CurrentZoomLevel, new SimplificationParamters() { Retain3Points = true });
                    //VisualSimplification.sim layer.Geometry.Simplify()
                    map.AddDrawingItem(simplified, $"{layer.Title} simplified-{map.CurrentZoomLevel}");

                }
                catch (Exception ex)
                {
                    await map.ShowMessageAsync(null, ex.Message);
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

                    map.AddDrawingItem(cloned, $"{layer.Title} cloned-{map.CurrentZoomLevel}");

                }
                catch (Exception ex)
                {
                    await map.ShowMessageAsync(null, ex.Message);
                }
            });

            return result;
        }

    }
}
