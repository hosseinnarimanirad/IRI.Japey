using IRI.Jab.Common.Presenter.Map;
using IRI.Jab.Common.Assets.Commands;
using IRI.Extensions;
using System;
using System.Windows;
using System.Collections.Generic;
using IRI.Extensions;
using sb = IRI.Sta.Common.Primitives;
using IRI.Jab.Common.Model.Map;

namespace IRI.Jab.Common.Model.Legend
{
    public class LegendCommand : Notifier, ILegendCommand
    {
        private const string _exportAsBitmapToolTip = "خروجی عکسی";

        private const string _exportAsShapefileToolTip = "خروجی در قالب شیپ‌فایل";

        private const string _exportAsGeoJsonToolTip = "خروجی در قالب GeoJson";

        private RelayCommand _command;

        public RelayCommand Command
        {
            get { return _command; }
            set
            {
                _command = value;
                RaisePropertyChanged();
            }
        }

        private string _pathMarkup;

        public string PathMarkup
        {
            get { return _pathMarkup; }
            set
            {
                _pathMarkup = value;
                RaisePropertyChanged();
            }
        }

        private bool _isEnabled = true;

        public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                _isEnabled = value;
                RaisePropertyChanged();
            }
        }

        private string _toolTip;

        public string ToolTip
        {
            get { return _toolTip; }
            set
            {
                _toolTip = value;
                RaisePropertyChanged();
            }
        }

        private bool _isCommandVisible = true;

        public bool IsCommandVisible
        {
            get { return _isCommandVisible; }
            set
            {
                _isCommandVisible = value;
                RaisePropertyChanged();
            }
        }


        public ILayer Layer { get; set; }


        #region Defaults for ILayer

        public static LegendCommand Create(ILayer layer, Action action, string markup, string tooltip)
        {
            var result = new LegendCommand()
            {
                PathMarkup = markup,
                Command = new RelayCommand(param => action()),
                ToolTip = tooltip,
                Layer = layer
            };

            result.Command = new RelayCommand(param => action());

            return result;
        }


        public static Func<MapPresenter, ILayer, LegendCommand> CreateZoomToExtentCommandFunc = CreateZoomToExtentCommand;
        public static LegendCommand CreateZoomToExtentCommand(MapPresenter map, ILayer layer)
        {
            var result = new LegendCommand()
            {
                PathMarkup = IRI.Jab.Common.Assets.ShapeStrings.Appbar.appbarMagnify,
                Layer = layer,
                ToolTip = "محدودهٔ لایه"
            };

            result.Command = new RelayCommand((param) =>
            {
                if (layer == null || map == null)
                    return;

                map.ZoomToExtent(result.Layer.Extent, false);
            });

            return result;
        }


        public static Func<MapPresenter, ILayer, ILegendCommand> CreateRemoveLayerFunc = CreateRemoveLayer;
        public static ILegendCommand CreateRemoveLayer(MapPresenter map, ILayer layer)
        {
            var result = new LegendCommand()
            {
                PathMarkup = IRI.Jab.Common.Assets.ShapeStrings.Appbar.appbarDelete,
                Layer = layer,
                ToolTip = "حذف لایه",
            };

            result.Command = new RelayCommand(param =>
            {
                map.ClearLayer(layer, true);
            });

            return result;
        }

        public static ILegendCommand CreateShowSymbologyView(ILayer layer, Action showSymbologyViewAction)
        {
            return Create(layer, showSymbologyViewAction, Assets.ShapeStrings.Appbar.appbarCart, "سمبل‌گذاری");
        }

        #endregion


        #region Defaults for VectorLayer

        public static Func<MapPresenter, ILayer, LegendCommand> CreateShowAttributeTableFunc<T>() where T : class, sb.IGeometryAware<sb.Point>
        {
            return (presenter, layer) => CreateShowAttributeTable<T>(presenter, layer as VectorLayer);
        }
        public static LegendCommand CreateShowAttributeTable<T>(MapPresenter map, VectorLayer layer) where T : class, sb.IGeometryAware<sb.Point>
        {
            var result = new LegendCommand()
            {
                PathMarkup = IRI.Jab.Common.Assets.ShapeStrings.Appbar.appbarPageText,
                Layer = layer,
                ToolTip = "اطلاعات توصیفی",
            };

            result.Command = new RelayCommand((param) =>
            {
                if (layer == null || map == null)
                    return;

                var features = layer.GetFeatures<T>();

                var newLayer = new Map.SelectedLayer<T>(layer, layer.GetFields());

                //newLayer.RequestSave = l =>
                //{
                //    layer.sou
                //};

                if (features == null)
                {
                    newLayer.Features = new System.Collections.ObjectModel.ObservableCollection<T>();
                }
                else
                {
                    newLayer.Features = new System.Collections.ObjectModel.ObservableCollection<T>(features);
                }


                map.AddSelectedLayer(newLayer);
            });

            return result;
        }


        public static Func<MapPresenter, ILayer, ILegendCommand> CreateSelectByDrawingFunc<T>() where T : class, sb.IGeometryAware<sb.Point>
        {
            return (presenter, layer) => CreateSelectByDrawing<T>(presenter, layer as VectorLayer);
        }
        public static ILegendCommand CreateSelectByDrawing<T>(MapPresenter map, VectorLayer layer) where T : class, sb.IGeometryAware<sb.Point>
        {
            var result = new LegendCommand()
            {
                PathMarkup = IRI.Jab.Common.Assets.ShapeStrings.Appbar.appbarVectorPenConvert,
                Layer = layer,
                ToolTip = "انتخاب عوارض محدودهٔ ترسیم",
            };

            result.Command = new RelayCommand(async param =>
            {
                var options = EditableFeatureLayerOptions.CreateDefaultForDrawing(false, false);

                options.IsOptionsAvailable = false;

                var drawingResult = await map.GetDrawingAsync(Model.DrawMode.Polygon, options);

                if (!drawingResult.HasNotNullResult())
                    return;

                var features = layer.GetFeatures<T>(drawingResult.Result);

                if (features == null)
                {
                    return;
                }

                var newLayer = new SelectedLayer<T>(layer, layer.GetFields())
                {
                    ShowSelectedOnMap = true
                };

                if (features != null)
                {
                    newLayer.Features = new System.Collections.ObjectModel.ObservableCollection<T>(features);
                }

                map.AddSelectedLayer(newLayer);
            });

            return result;
        }


        public static Func<MapPresenter, ILayer, ILegendCommand> CreateClearSelectedFunc = (presenter, layer) => CreateClearSelected(presenter, layer as VectorLayer);
        public static ILegendCommand CreateClearSelected(MapPresenter map, VectorLayer layer)
        {
            var result = new LegendCommand()
            {
                PathMarkup = IRI.Jab.Common.Assets.ShapeStrings.Appbar.appbarClose,
                Layer = layer,
                ToolTip = "پاک کردن عوارض انتخابی",
                IsCommandVisible = false,
            };

            result.Command = new RelayCommand(param =>
            {
                map.RemoveSelectedLayer(layer);
            });

            layer.OnSelectedFeaturesChanged += (sender, e) => { result.IsCommandVisible = e.Arg.HasSelectedFeatures; };

            return result;
        }

        public static Func<MapPresenter, ILayer, ILegendCommand> CreateExportAsPngFunc = (presenter, layer) => CreateExportAsPng(presenter, layer as VectorLayer);
        public static ILegendCommand CreateExportAsPng(MapPresenter map, VectorLayer layer)
        {
            var result = new LegendCommand()
            {
                PathMarkup = IRI.Jab.Common.Assets.ShapeStrings.Appbar.appbarImage,
                Layer = layer,
                ToolTip = _exportAsBitmapToolTip,
            };

            result.Command = new RelayCommand(async param =>
            {
                try
                {
                    var fileName = await map.DialogService.ShowSaveFileDialogAsync("*.png|*.png", null, layer.LayerName);

                    if (string.IsNullOrWhiteSpace(fileName))
                        return;

                    layer.SaveAsPng(fileName, map.CurrentExtent, map.ActualWidth, map.ActualHeight, map.MapScale);
                }
                catch (Exception ex)
                {
                    await map.DialogService.ShowMessageAsync(ex.Message, null, param);
                }
            });

            return result;
        }


        public static Func<MapPresenter, ILayer, ILegendCommand> CreateExportAsShapefileFunc = (presenter, layer) => CreateExportAsShapefile(presenter, layer as VectorLayer);
        public static ILegendCommand CreateExportAsShapefile(MapPresenter map, VectorLayer layer)
        {
            var result = new LegendCommand()
            {
                PathMarkup = IRI.Jab.Common.Assets.ShapeStrings.Appbar.appbarDownload,
                Layer = layer,
                ToolTip = _exportAsShapefileToolTip,
            };

            result.Command = new RelayCommand(async param =>
            {
                try
                {
                    var fileName = await map.DialogService.ShowSaveFileDialogAsync("*.shp|*.shp", null, layer.LayerName);

                    if (string.IsNullOrWhiteSpace(fileName))
                        return;

                    layer.ExportAsShapefile(fileName);
                }
                catch (Exception ex)
                {
                    await map.DialogService.ShowMessageAsync(ex.Message, null, param);
                }
            });

            return result;
        }

        public static Func<MapPresenter, ILayer, ILegendCommand> CreateExportAsGeoJsonFunc = (presenter, layer) => CreateExportAsGeoJson(presenter, layer as VectorLayer);
        public static ILegendCommand CreateExportAsGeoJson(MapPresenter map, VectorLayer layer)
        {
            var result = new LegendCommand()
            {
                PathMarkup = IRI.Jab.Common.Assets.ShapeStrings.Appbar.appbarDownload,
                Layer = layer,
                ToolTip = _exportAsGeoJsonToolTip,
            };

            result.Command = new RelayCommand(async param =>
            {
                try
                {
                    var fileName = await map.DialogService.ShowSaveFileDialogAsync("*.json|*.json", null, layer.LayerName);

                    if (string.IsNullOrWhiteSpace(fileName))
                        return;

                    // 1400.02.31
                    // به خاطر خروجی برنامه البرز نگار
                    // چون در سایت ژئوجی‌سان دات آی‌او
                    // بارگذاری می شه خروجی‌ها
                    layer.ExportAsGeoJson(fileName, true);
                }
                catch (Exception ex)
                {
                    await map.DialogService.ShowMessageAsync(ex.Message, null, param);
                }
            });

            return result;
        }

        internal static List<Func<MapPresenter, ILayer, ILegendCommand>> GetDefaultVectorLayerCommands<T>() where T : class, sb.IGeometryAware<sb.Point>
        {
            return new List<Func<MapPresenter, ILayer, ILegendCommand>>()
            {
                CreateSelectByDrawingFunc<T>(),
                CreateShowAttributeTableFunc<T>(),
                CreateClearSelectedFunc,
                CreateRemoveLayerFunc,
                CreateExportAsPngFunc,
                CreateZoomToExtentCommandFunc
            };
        }

       

        #endregion

    }
}
