using IRI.Jab.Common.Presenter.Map;
using IRI.Jab.Common.Assets.Commands;
using IRI.Ket.SpatialExtensions;
using IRI.Ket.SqlServerSpatialExtension.Model;
using System;

namespace IRI.Jab.Common.Model.Legend
{
    public class LegendCommand : Notifier, ILegendCommand
    {
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

        public static LegendCommand CreateShowAttributeTable<T>(MapPresenter map, ILayer layer) where T : ISqlGeometryAware
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

                //System.Diagnostics.Stopwatch watch = System.Diagnostics.Stopwatch.StartNew();

                //System.Diagnostics.Debug.WriteLine($"Command start");

                var features = (layer as VectorLayer).GetFeatures<T>();

                //var list = new List<SqlIndex250k>() {
                //    new SqlIndex250k(new Msh.Common.Mapping.Index250k()),
                //    new SqlIndex250k(new Msh.Common.Mapping.Index250k()),
                //    new SqlIndex250k(new Msh.Common.Mapping.Index250k()),
                //    new SqlIndex250k(new Msh.Common.Mapping.Index250k()),
                //    new SqlIndex250k(new Msh.Common.Mapping.Index250k())};

                //watch.Stop();
                //System.Diagnostics.Debug.WriteLine($"Get Features finished {watch.ElapsedMilliseconds}");
                //watch.Restart();

                var newLayer = new Model.Map.SelectedLayer<T>(layer)
                {
                    Features = new System.Collections.ObjectModel.ObservableCollection<T>(features)
                };

                map.AddSelectedLayer(newLayer);


                //map.SelectedLayers.Add(newLayer);

                //map.CurrentLayer = newLayer;               

                //watch.Stop();
                //System.Diagnostics.Debug.WriteLine($"map.SelectedLayers.Add {watch.ElapsedMilliseconds}");

            });

            return result;
        }

        public static ILegendCommand CreateSelectByDrawing<T>(MapPresenter map, VectorLayer layer) where T : ISqlGeometryAware
        {
            var result = new LegendCommand()
            {
                PathMarkup = IRI.Jab.Common.Assets.ShapeStrings.Appbar.appbarVectorPenConvert,
                Layer = layer,
                ToolTip = "انتخاب عوارض محدودهٔ ترسیم",
            };

            result.Command = new RelayCommand(async param =>
            {
                var drawing = await map.GetDrawingAsync(Model.DrawMode.Polygon);

                var features = layer.GetFeatures<T>(drawing.AsSqlGeometry());

                var newLayer = new Model.Map.SelectedLayer<T>(layer)
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

        public static LegendCommand Create(ILayer layer, Action action, string markup)
        {
            var result = new LegendCommand()
            {
                PathMarkup = markup,
                Command = new RelayCommand(param => action())
            };

            result.Command = new RelayCommand(param => action());

            return result;
        }

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
    }
}
