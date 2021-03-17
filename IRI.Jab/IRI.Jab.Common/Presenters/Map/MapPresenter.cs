using IRI.Msh.Common.Primitives;
using IRI.Jab.Common.Model;
using IRI.Jab.Common.Model.Map;
using IRI.Jab.Common.TileServices;
using IRI.Jab.Common.Assets.Commands;
using IRI.Jab.Common.Extensions;
using IRI.Jab.Common.Helpers;
using IRI.Jab.Common.Model.Common;
using IRI.Jab.Common.Model.Spatialable;
using IRI.Jab.Common.Presenters;
using IRI.Ket.DataManagement.DataSource;
using IRI.Ket.SpatialExtensions;
using Microsoft.SqlServer.Types;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using static System.Windows.Media.Colors;
using WpfPoint = System.Windows.Point;
using IRI.Ket.SqlServerSpatialExtension.Model;
using IRI.Jab.Common.Model.Legend;
using IRI.Msh.CoordinateSystem.MapProjection;
using IRI.Ket.DataManagement.Model;
using IRI.Msh.Common.Model;
using IRI.Ket.Common.Service;
using IRI.Msh.Common.Helpers;
using System.Windows.Threading;
using Geometry = IRI.Msh.Common.Primitives.Geometry<IRI.Msh.Common.Primitives.Point>;

namespace IRI.Jab.Common.Presenter.Map
{
    public abstract class MapPresenter : BasePresenter
    {
        #region Properties

        private ProxySettingsModel _proxy;
        public ProxySettingsModel Proxy
        {
            get { return _proxy; }
            set
            {
                _proxy = value;
                value.FireProxyChanged = p => this.SetProxy(p.GetProxy());
                RaisePropertyChanged();
            }
        }


        private MapSettingsModel _mapSettings = new MapSettingsModel();
        public MapSettingsModel MapSettings
        {
            get { return _mapSettings; }
            private set
            {
                _mapSettings = value;
                RaisePropertyChanged();
            }
        }


        private MapPanelPresenter _mapPanel;
        public MapPanelPresenter MapPanel
        {
            get { return _mapPanel; }
            set
            {
                _mapPanel = value;
                RaisePropertyChanged();
            }
        }


        private CoordinatePanelPresenter _coordinatePanel;
        public CoordinatePanelPresenter CoordinatePanel
        {
            get { return _coordinatePanel; }
            set
            {
                _coordinatePanel = value;
                RaisePropertyChanged();
            }
        }


        private List<EnvelopeMarkupLabelTriple> _ostanha;
        public List<EnvelopeMarkupLabelTriple> Ostanha
        {
            get { return _ostanha; }
            set
            {
                _ostanha = value;
                RaisePropertyChanged();
            }
        }


        private EditableFeatureLayer _currentEditingLayer;
        public EditableFeatureLayer CurrentEditingLayer
        {
            get { return _currentEditingLayer; }
            set
            {
                _currentEditingLayer = value;
                RaisePropertyChanged();

                if (_currentEditingLayer != null)
                {
                    _currentEditingLayer.RequestSelectedLocatableChanged = (l) =>
                    {
                        this.UpdateCurrentEditingPoint(new IRI.Msh.Common.Primitives.Point(l.X, l.Y));
                    };

                    _currentEditingLayer.RequestZoomToPoint = (p) =>
                    {
                        this.Zoom(IRI.Msh.Common.Mapping.WebMercatorUtility.GetGoogleMapScale(14), p);
                    };

                    _currentEditingLayer.RequestZoomToGeometry = g =>
                    {
                        this.ZoomToExtent(g.GetBoundingBox(), false);
                    };
                }

            }
        }


        private ObservableCollection<ISelectedLayer> _selectedLayers = new ObservableCollection<ISelectedLayer>();
        public ObservableCollection<ISelectedLayer> SelectedLayers
        {
            get { return _selectedLayers; }
            set
            {
                _selectedLayers = value;
                RaisePropertyChanged();
            }
        }


        private ISelectedLayer _currentLayer;
        public ISelectedLayer CurrentLayer
        {
            get { return _currentLayer; }
            set
            {
                _currentLayer = value;
                RaisePropertyChanged();

                if (value?.ShowSelectedOnMap == true)
                {
                    ShowSelectedFeatures(value?.GetSelectedFeatures());
                }
            }
        }


        private ObservableCollection<ILayer> _layers;
        public ObservableCollection<ILayer> Layers
        {
            get { return _layers; }
            set
            {
                _layers = value;
                RaisePropertyChanged();

                //if (_layers == null)
                //    return;

                //_layers.CollectionChanged += (sender, e) =>
                //{
                //    switch (e.Action)
                //    {
                //        case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                //            foreach (var item in e.NewItems.Cast<ILayer>())
                //            {
                //                if (item.Type == LayerType.BaseMap)
                //                {
                //                    return;
                //                }

                //                var layer = item;

                //                var model = new Model.Legend.MapLegendItemWithOptionsModel(layer);
                //                model.Commands = new List<Model.Legend.ILegendCommand>()
                //                {
                //                    LegendCommand.CreateZoomToExtentCommand(this, layer),
                //                };

                //                LegendLayers.Add(model);
                //            }
                //            break;
                //        case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                //            foreach (var item in e.OldItems.Cast<ILayer>())
                //            {
                //                if (item.Type == LayerType.BaseMap)
                //                {
                //                    return;
                //                }

                //                LegendLayers.Remove(LegendLayers.First(ll => ll.Id == item.Id));
                //            }
                //            break;
                //        case System.Collections.Specialized.NotifyCollectionChangedAction.Replace:
                //            break;
                //        case System.Collections.Specialized.NotifyCollectionChangedAction.Move:
                //            break;
                //        case System.Collections.Specialized.NotifyCollectionChangedAction.Reset:
                //            this.LegendLayers.Clear();
                //            break;
                //        default:
                //            break;
                //    }
                //};
            }
        }


        //LegendCommand.CreateZoomToExtentCommand(this, layer),
        //                LegendCommand.CreateSelectByDrawing<T>(this, (VectorLayer) layer),
        //                LegendCommand.CreateShowAttributeTable<T>(this, (VectorLayer) layer),
        //                LegendCommand.CreateClearSelected(this, (VectorLayer) layer),
        //                LegendCommand.CreateRemoveLayer(this, layer),

        private List<Func<MapPresenter, IFeatureTableCommand>> _defaultVectorLayerFeatureTableCommands = FeatureTableCommands.GetDefaultVectorLayerCommands<SqlFeature>();
        public List<Func<MapPresenter, IFeatureTableCommand>> DefaultVectorLayerFeatureTableCommands
        {
            get { return _defaultVectorLayerFeatureTableCommands; }
            set
            {
                _defaultVectorLayerFeatureTableCommands = value;
                RaisePropertyChanged();
            }
        }


        private List<Func<MapPresenter, ILayer, ILegendCommand>> _defaultVectorLayerCommands = LegendCommand.GetDefaultVectorLayerCommands<SqlFeature>();

        public List<Func<MapPresenter, ILayer, ILegendCommand>> DefaultVectorLayerCommands
        {
            get { return _defaultVectorLayerCommands; }
            set { value = _defaultVectorLayerCommands; }
        }

        public ILayer GetSelectedLayerInToc()
        {
            return this.Layers.SingleOrDefault(l => l.IsSelectedInToc);
        }

        private ObservableCollection<DrawingItemLayer> _drawingItems = new ObservableCollection<DrawingItemLayer>();
        public ObservableCollection<DrawingItemLayer> DrawingItems
        {
            get { return _drawingItems; }
            set
            {
                _drawingItems = value;
                RaisePropertyChanged();
            }
        }


        private IRI.Msh.Common.Primitives.Point _currentPoint;
        public IRI.Msh.Common.Primitives.Point CurrentPoint
        {
            get { return _currentPoint; }
            set
            {
                _currentPoint = value;
                RaisePropertyChanged();
            }
        }


        //private Dictionary<string, Func<TileType, IMapProvider>> _mapProviders;
        //public Dictionary<string, Func<TileType, IMapProvider>> MapProviders
        //{
        //    get { return _mapProviders; }
        //    set
        //    {
        //        _mapProviders = value;
        //        RaisePropertyChanged();
        //    }
        //}
        private List<TileMapProvider> _mapProviders;

        public List<TileMapProvider> MapProviders
        {
            get { return _mapProviders; }
            set
            {
                _mapProviders = value;
                RaisePropertyChanged();
            }
        }

        private TileMapProvider _selectedMapProvider;

        public TileMapProvider SelectedMapProvider
        {
            get { return _selectedMapProvider; }
            set
            {
                _selectedMapProvider = value;
                RaisePropertyChanged();
            }
        }


        //private TileType _baseMapType = TileType.None;
        //public TileType BaseMapType
        //{
        //    get { return _baseMapType; }
        //    set
        //    {
        //        if (_baseMapType == value)
        //            return;

        //        _baseMapType = value;
        //        RaisePropertyChanged();

        //        //SetTileService(ProviderType, value, MapSettings.GetFileName);
        //        UpdateBaseMap();
        //    }
        //}


        private string _providerTypeFullName = string.Empty;
        public string ProviderTypeFullName
        {
            get { return _providerTypeFullName; }
        }

        public async Task SetTileBaseMap(string tileMapFullName)
        {
            if (string.IsNullOrEmpty(tileMapFullName))
            {
                RemoveAllTileServices();

                return;
            }

            var tileMapFullNameToUpper = tileMapFullName?.ToUpper();

            if (_providerTypeFullName == tileMapFullNameToUpper)
            {
                return;
            }

            _providerTypeFullName = tileMapFullNameToUpper;

            RaisePropertyChanged(nameof(ProviderTypeFullName));

            var provider = this.MapProviders.SingleOrDefault(m => m.Name?.EqualsIgnoreCase(ProviderTypeFullName) == true);

            if (provider == null)
            {
                return;
            }

            this.SelectedMapProvider = provider;

            await SetTileService(provider, MapSettings.GetLocalFileName);//, MapSettings.GetFileName);             
        }


        private bool _doNotCheckInternet = false;
        public bool DoNotCheckInternet
        {
            get { return _doNotCheckInternet; }
            set
            {
                _doNotCheckInternet = value;
                RaisePropertyChanged();
            }
        }


        private bool? _isConnected = null;
        public bool IsConnected
        {
            get { return _isConnected.HasValue && _isConnected.Value; }
            set
            {
                if (_isConnected == value)
                    return;

                _isConnected = value;
                RaisePropertyChanged();

                this.RequestSetConnectedState?.Invoke(value);
            }
        }


        private MapStatus _mapStatus;
        public MapStatus MapStatus
        {
            get { return _mapStatus; }
            set
            {
                if (_mapStatus == value)
                {
                    return;
                }

                _mapStatus = value;
                RaisePropertyChanged();

                switch (_mapStatus)
                {
                    case MapStatus.Drawing:
                        this.IsDrawMode = true;
                        break;
                    case MapStatus.Editing:
                        this.IsEditMode = true;
                        break;
                    //case MapStatus.Measuring:
                    //    this.IsMeasureMode = true;
                    //    break;
                    case MapStatus.Idle:
                        this.IsDrawMode = false;
                        this.IsEditMode = false;
                        break;
                    default:
                        break;
                }

            }
        }


        private MapAction _mapAction;
        public MapAction MapAction
        {
            get { return _mapAction; }
            set
            {
                _mapAction = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(IsPanMode));
                RaisePropertyChanged(nameof(IsZoomInMode));
                RaisePropertyChanged(nameof(IsZoomOutMode));
            }
        }


        private bool _isBusy;
        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                //SetIsBusy(value);
                this._isBusy = value;
                RaisePropertyChanged();
            }
        }

        public double InverseMapScale
        {
            get
            {
                return 1.0 / MapScale;
            }
        }

        public double MapScale
        {
            get
            {
                return this.RequestMapScale?.Invoke() ?? 1;
            }
        }

        public double CurrentPointInverseMapScale
        {
            get
            {
                var scale = this.RequestCurrentPointScale?.Invoke() ?? 1;

                return Math.Round(1.0 / scale, 2);
            }
        }

        public double CurrentPointGroundResolution
        {
            get
            {
                return this.RequestCurrentPointGroundResolution?.Invoke() ?? 1;
            }
        }

        public int CurrentZoomLevel { get { return this.RequestCurrentZoomLevel?.Invoke() ?? 1; } }


        public IRI.Msh.Common.Primitives.BoundingBox CurrentExtent
        {
            get
            {
                return this.RequestCurrentExtent?.Invoke() ?? BoundingBoxes.IranMercatorBoundingBox;
            }
        }


        public double ActualWidth
        {
            get { return RequestGetActualWidth?.Invoke() ?? 1; }
        }


        public double ActualHeight
        {
            get { return RequestGetActualHeight?.Invoke() ?? 1; ; }
        }


        private bool _isDrawMode;
        public bool IsDrawMode
        {
            get { return _isDrawMode; }
            set
            {
                _isDrawMode = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(IsDrawEditMeasureMode));
            }
        }


        private bool _isEditMode;
        public bool IsEditMode
        {
            get { return _isEditMode; }
            set
            {
                _isEditMode = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(IsDrawEditMeasureMode));
            }
        }


        public bool IsDrawEditMeasureMode
        {
            get
            {
                return IsEditMode || IsDrawMode;
            }
        }


        public bool IsPanMode
        {
            get { return MapAction == MapAction.Pan; }
            set
            {
                if (value)
                {
                    this.Pan();
                }

                RaisePropertyChanged();
            }
        }


        public bool IsZoomInMode
        {
            get { return MapAction == MapAction.ZoomIn || MapAction == MapAction.ZoomInRectangle; }
            set
            {
                if (value)
                {
                    this.EnableRectangleZoomIn();
                }

                RaisePropertyChanged();
            }
        }


        public bool IsZoomOutMode
        {
            get { return MapAction == MapAction.ZoomOut || MapAction == MapAction.ZoomOutRectangle; }
            set
            {
                if (value)
                {
                    this.EnableZoomOut();
                }

                RaisePropertyChanged();
            }
        }


        #endregion


        public MapPresenter()
        {
            //this.MapProviders = new Dictionary<string, Func<TileType, IMapProvider>>()
            //{
            //    {"GOOGLE", tileType => new GoogleMapProvider(tileType) },
            //    {"BING", tileType => new BingMapProvider(tileType) },
            //    {"NOKIA", tileType => new NokiaMapProvider(tileType) },
            //    {"OPENSTREETMAP", tileType => new OsmMapProvider(tileType) },
            //    {"WAZE", tileType => new WazeMapProvider(tileType) },
            //};
            this.MapProviders = TileMapProviderFactory.GetDefault();

            this.MapPanel = new MapPanelPresenter();

            this.MapPanel.CurrentEditingPoint = new NotifiablePoint(0, 0, param =>
              {
                  if (this.CurrentEditingLayer == null)
                  {
                      Debug.WriteLine($"Exception at map presenter. current editing layer is null!");
                      return;
                  }
                  //this.CurrentEditingLayer.ChangeCurrentEditingPoint(new IRI.Msh.Common.Primitives.Point(param.X, param.Y));
                  this.CurrentEditingLayer.ChangeCurrentEditingPoint(this.MapPanel.CurrentWebMercatorEditingPoint);

              });

            this.CoordinatePanel = new CoordinatePanelPresenter();
        }


        #region Actions & Funcs

        public Action RequestPrint;

        public Action<System.Net.WebProxy> RequestSetProxy;

        public Func<System.Net.WebProxy> RequestGetProxy;

        public Action<MapAction, Cursor> RequestSetDefaultCursor;

        public Action<Cursor> RequestSetCursor;

        public Func<double> RequestGetActualWidth;

        public Func<double> RequestGetActualHeight;

        public Action<MapPresenter> RegisterAction;

        public Action<bool> RequestSetConnectedState;

        public Action RequestRefreshBaseMaps;

        public Action<TileMapProvider, bool, string, bool, Func<TileInfo, string>> RequestSetTileService;

        //public Action RequestRemoveAllTileServices;

        public Func<double> RequestMapScale;

        public Func<double> RequestCurrentPointScale;

        public Func<double> RequestCurrentPointGroundResolution;

        public Func<int> RequestCurrentZoomLevel;

        public Func<IRI.Msh.Common.Primitives.BoundingBox> RequestCurrentExtent;

        public Action RequestRefresh;

        public Action<ILayer> RequestRefreshLayerVisibility;

        public Action RequestIranExtent;

        public Action RequestFullExtent;

        public Action<double> RequestZoomToScale;

        public Action<IRI.Msh.Common.Primitives.Point, double> RequestZoomToPoint;

        public Action<IRI.Msh.Common.Primitives.Point, int, Action> RequestZoomToGoogleScale;

        public Action<IRI.Msh.Common.Primitives.BoundingBox, bool, Action> RequestZoomToExtent;

        public Action<SqlGeometry> RequestZoomToFeature;

        public Action RequestEnableRectangleZoom;

        public Action RequestEnableZoomOut;

        public Action RequestPan;

        public Action<IRI.Msh.Common.Primitives.Point, Action> RequestPanTo;

        public Action<int, IRI.Msh.Common.Primitives.Point, Action, bool> RequestZoomToLevelAndCenter;

        public Action<MapOptionsEventArgs<FrameworkElement>> RequestRegisterMapOptions;

        public Action RequestUnregisterMapOptions;

        public Action RequestRemoveMapOptions;


        public Action RequestCopyCurrentLocationToClipboard;


        //presenter.RequestRemoveLayer = (layer, forceRemove) =>
        //{
        //   this.ClearLayer(layer, true, forceRemove);
        //};

        //presenter.RequestRemoveLayerByName = (i) =>
        //{
        //   this.ClearLayer(i, true);
        //};

        //public Action<ILayer, bool> RequestRemoveLayer;

        //public Action<string> RequestRemoveLayerByName;

        //public Action<LayerType, bool> RequestClearLayerByType;

        //public Action<string, bool> RequestClearLayerByName;

        public Action<ILayer, bool> RequestClearLayer;

        public Action<Predicate<ILayer>, bool, bool> RequestClearLayerByCriteria;

        public Action<Predicate<LayerTag>, bool, bool> RequestClearLayerByTag;

        public Action RequestRemovePolyBezierLayers;



        public Action<List<IRI.Msh.Common.Primitives.Point>> RequestFlashPoints;

        public Action<IRI.Msh.Common.Primitives.Point> RequestFlashPoint;


        public Func<List<SqlGeometry>, VisualParameters, string, System.Windows.Media.Geometry, Task> RequestSelectGeometries;

        public Func<List<SqlGeometry>, string, VisualParameters, Task> RequestAddGeometries;

        public Func<GeometryLabelPairs, string, VisualParameters, LabelParameters, Task> RequestDrawGeometryLablePairs;

        public Action<SpecialPointLayer> RequestAddSpecialPointLayer;

        public Action<ILayer> RequestSetLayer;

        public Action<ILayer> RequestRemoveLayer;

        public Action<string> RequestRemoveLayerByName;

        //public Func<ILayer, Task> RequestAddLayer;
        public Action<ILayer> RequestAddLayer;

        //public Action<string> RequestRemoveLayer;

        public Action<string, List<IRI.Msh.Common.Primitives.Point>, System.Windows.Media.Geometry, bool, VisualParameters> RequestAddPolyBezier;

        public Func<DrawMode, EditableFeatureLayerOptions, bool, Task<Response<Geometry>>> RequestGetDrawingAsync;

        public Action RequestClearAll;

        public Action RequestCancelNewDrawing;

        public Action RequestFinishDrawingPart;

        public Action RequestFinishNewDrawing;

        public Action RequestCancelEdit;

        public Action RequestFinishEdit;

        public Action OnRequestShowAboutMe;

        //public Func<DrawMode, bool, Task<Geometry>> RequestMeasure;
        public Func<DrawMode, EditableFeatureLayerOptions, EditableFeatureLayerOptions, Action, Task<Response<Geometry>>> RequestMeasure;

        //public Action RequestMeasureLength;

        public Action RequestCancelMeasure;

        public Action RequestShowGoToView;

        public Action<ILayer> RequestShowSymbologyView;

        public Action<IPoint> RequestAddPointToNewDrawing;

        public Func<Geometry, EditableFeatureLayerOptions, Task<Response<Geometry>>> RequestEdit;

        public Func<System.Windows.Media.Geometry, VisualParameters, Task<Response<PolyBezierLayer>>> RequestGetBezier;


        public Func<SqlGeometry, ObservableCollection<System.Data.DataTable>> RequestIdentify;

        public Func<Task<Response<IRI.Msh.Common.Primitives.Point>>> RequestGetPoint;

        #endregion


        //*****************************************Map Providers & TileServices***********************************************
        #region Map Providers & TileServices            



        public void RemoveAllTileServices()
        {
            this.Clear(l => l.Type == LayerType.BaseMap, true, true);

            this.RefreshBaseMaps();
        }

        public void RefreshBaseMaps()
        {
            this.RequestRefreshBaseMaps?.Invoke();
        }

        public void AddProvider(TileMapProvider mapProvider)
        {
            //var nameInUpper = mapProviderFullName?.Provider?.EnglishTitle?.ToUpper();

            if (!MapProviders.Any(m => m == mapProvider))
            {
                this.MapProviders.Add(mapProvider);

                //this.MapProviders.Add(nameInUpper, t =>
                //{
                //    mapProvider.TileType = t;

                //    return mapProvider;
                //});
            }
        }

        public void RemoveAllProviders()
        {
            this.MapProviders = new List<TileMapProvider>();

            RemoveAllTileServices();
        }

        //public void RemoveProvider(string name)
        //{
        //    var nameInUpper = name?.ToUpper();

        //    if (MapProviders.ContainsKey(nameInUpper))
        //    {
        //        this.MapProviders.Remove(nameInUpper);
        //    }
        //}


        public async Task SetTileService(TileMapProvider baseMap, Func<TileInfo, string> getLocalFileName)
        {
            //98.01.18
            //if (MapProviders.ContainsKey(baseMap.ProviderName))
            //{
            //    this.MapProviders.Add(baseMap.ProviderName, t => { baseMap.TileType = t; return baseMap; });
            //}


            if (!IsConnected)
            {
                await CheckInternetAccess();
            }

            this.RequestSetTileService?.Invoke(baseMap, MapSettings.IsBaseMapCacheEnabled, MapSettings.BaseMapCacheDirectory, !IsConnected, getLocalFileName);
        }

        //public async Task SetTileService(string provider, TileType tileType, Func<TileInfo, string> getFileName = null)
        //{
        //    provider = provider.ToUpper();

        //    if (!IsConnected)
        //    {
        //        await CheckInternetAccess();
        //    }

        //    if (!MapProviders.ContainsKey(provider))
        //    {
        //        return;
        //    }

        //    var mapProvider = MapProviders[provider](tileType);

        //    this.RequestSetTileService?.Invoke(mapProvider, MapSettings.IsBaseMapCacheEnabled, MapSettings.BaseMapCacheDirectory, !IsConnected, getFileName);

        //}

        #endregion


        //*****************************************Selected Layers & Select Geometries & DrawGeometries & Identify & FlashPoints ******************
        #region Selected Layers & Select/Draw Geometries & Identify & FlashPoints 

        public void AddSelectedLayer(ISelectedLayer selectedLayer)
        {
            if (selectedLayer == null)
            {
                return;
            }

            selectedLayer.AssociatedLayer.NumberOfSelectedFeatures = selectedLayer.CountOfSelectedFeatures();

            var existingLayer = SelectedLayers?.SingleOrDefault(l => l.Id == selectedLayer?.Id);

            if (existingLayer == null)
            {
                selectedLayer.FeaturesChangedAction = ShowSelectedFeatures;

                selectedLayer.HighlightFeaturesChangedAction = ShowHighlightedFeatures;

                selectedLayer.RequestFlashSinglePoint = FlashHighlightedFeatures;

                selectedLayer.RequestZoomTo = (features, callback) =>
                {
                    var extent = BoundingBox.GetMergedBoundingBox(features.Select(f => f.TheSqlGeometry.GetBoundingBox()));

                    this.ZoomToExtent(extent, false, callback);
                };

                selectedLayer.RequestRemove = () => { this.SelectedLayers.Remove(selectedLayer); };

                selectedLayer.RequestEdit = async g =>
                {
                    var editResult = await EditAsync(g.TheSqlGeometry.AsGeometry(), MapSettings.EditingOptions);

                    if (editResult.HasNotNullResult())
                    {
                        SqlFeature f = new SqlFeature(editResult.Result.AsSqlGeometry()) { Id = g.Id };

                        selectedLayer.Update(g, f);
                    }

                    //Referesh
                    if (selectedLayer.ShowSelectedOnMap)
                    {
                        ShowSelectedFeatures(selectedLayer.GetSelectedFeatures());
                    }

                    Refresh();
                };

                //selectedLayer.RequestSave = l =>
                //{
                //    l.Save();
                //};

                this.SelectedLayers.Add(selectedLayer);

                CurrentLayer = selectedLayer;

                if (selectedLayer.ShowSelectedOnMap)
                {
                    ShowSelectedFeatures(selectedLayer.GetSelectedFeatures());
                }
            }
            else
            {
                existingLayer.UpdateSelectedFeatures(selectedLayer.GetSelectedFeatures());

                CurrentLayer = existingLayer;

                if (selectedLayer.ShowSelectedOnMap)
                {
                    ShowSelectedFeatures(selectedLayer.GetSelectedFeatures());
                }
            }
        }

        public void RemoveSelectedLayer(ILayer layer)
        {
            if (layer == null)
            {
                return;
            }

            var selectedLayer = this.SelectedLayers.SingleOrDefault(sl => sl.Id == layer.LayerId);

            layer.NumberOfSelectedFeatures = 0;

            if (selectedLayer != null)
            {
                this.SelectedLayers.Remove(selectedLayer);

                ClearLayer("__$selection", true);
                ClearLayer("__$highlight", true);
            }
        }

        public void RemoveSelectedLayers(Predicate<ILayer> layersToBeRemoved)
        {
            for (int i = this.SelectedLayers.Count - 1; i >= 0; i--)
            {
                if (layersToBeRemoved(this.SelectedLayers[i].AssociatedLayer))
                {
                    RemoveSelectedLayer(this.SelectedLayers[i].AssociatedLayer);
                }
            }
        }

        private async void ShowSelectedFeatures(IEnumerable<ISqlGeometryAware> enumerable)
        {
            ClearLayer("__$selection", true);
            ClearLayer("__$highlight", true);

            if (enumerable == null)
            {
                return;
            }

            await DrawGeometriesAsync(enumerable.Select(i => i.TheSqlGeometry).ToList(), "__$selection", VisualParameters.GetDefaultForSelection());

        }

        private async void ShowHighlightedFeatures(IEnumerable<ISqlGeometryAware> enumerable)
        {
            ClearLayer("__$highlight", true);

            ClearLayer(LayerType.AnimatingItem, true);

            if (enumerable == null || enumerable.Count() == 0)
            {
                return;
            }

            if (enumerable?.Count() < 10 && enumerable.First().TheSqlGeometry.GetOpenGisType() == Microsoft.SqlServer.Types.OpenGisGeometryType.Point)
            {
                FlashPoints(enumerable.Select(e => (IRI.Msh.Common.Primitives.Point)e.TheSqlGeometry.AsPoint()).ToList());
                //FlashSinglePoint?.Invoke(enumerable.First());
            }
            else
            {
                await DrawGeometriesAsync(enumerable.Select(i => i.TheSqlGeometry).ToList(), "__$highlight", VisualParameters.GetDefaultForHighlight(enumerable.FirstOrDefault()));
            }
        }

        public void FlashHighlightedFeatures(ISqlGeometryAware geometry)
        {
            var point = geometry?.TheSqlGeometry?.AsPoint();

            if (point != null)
            {
                FlashPoint(new Msh.Common.Primitives.Point(point.X, point.Y));
            }
        }


        public async Task SelectDrawingItem(DrawingItemLayer layer)
        {
            var highlightGeo = layer.Geometry.AsSqlGeometry();

            var visualParameters = VisualParameters.GetDefaultForSelection();

            //visualParameters.Visibility = layer.VisualParameters.Visibility;

            await SelectGeometryAsync(highlightGeo, visualParameters, layer.HighlightGeometryKey.ToString());
        }

        public async Task SelectGeometryAsync(SqlGeometry geometry)
        {
            await SelectGeometriesAsync(new List<SqlGeometry>() { geometry });
        }

        public async Task SelectGeometryAsync(SqlGeometry geometry, VisualParameters visualParameters, string layerName, System.Windows.Media.Geometry pointSymbol = null)
        {
            await SelectGeometriesAsync(new List<SqlGeometry>() { geometry }, visualParameters, layerName, pointSymbol);
        }

        public async Task SelectGeometriesAsync(List<SqlGeometry> geometries)
        {
            //await this.SelectGeometries(geometries, new VisualParameters(new System.Windows.Media.SolidColorBrush(Aqua), new System.Windows.Media.SolidColorBrush(Aqua), 2, .5));
            await this.SelectGeometriesAsync(geometries, VisualParameters.GetDefaultForSelection(), null);
        }

        public async Task SelectGeometriesAsync(List<SqlGeometry> geometries, VisualParameters visualParameters, string layerName, System.Windows.Media.Geometry pointSymbol = null)
        {
            await this.RequestSelectGeometries?.Invoke(geometries, visualParameters, layerName, pointSymbol);
        }


        public void DrawGeometryLablePairs(GeometryLabelPairs geometries, string name, VisualParameters parameters, LabelParameters labelParameters)
        {
            this.RequestDrawGeometryLablePairs?.Invoke(geometries, name, parameters, labelParameters);
        }

        public async Task DrawGeometriesAsync(List<SqlGeometry> geometry, string name, VisualParameters parameters)
        {
            await this.RequestAddGeometries?.Invoke(geometry, name, parameters);
        }

        public async Task DrawGeometryAsync(SqlGeometry geometry, string name, VisualParameters parameters)
        {
            await DrawGeometriesAsync(new List<SqlGeometry> { geometry }, name, parameters);
        }


        public void FlashPoints(List<IRI.Msh.Common.Primitives.Point> points)
        {
            this.RequestFlashPoints?.Invoke(points);
        }

        public void FlashPoint(IRI.Msh.Common.Primitives.Point point)
        {
            this.RequestFlashPoint?.Invoke(point);
        }


        public ObservableCollection<System.Data.DataTable> Identify(IRI.Msh.Common.Primitives.Point arg)
        {
            if (RequestIdentify != null)
            {
                return RequestIdentify(arg.AsSqlGeometry());
            }
            else
            {
                return null;
            }
        }

        public ObservableCollection<System.Data.DataTable> Identify(SqlGeometry arg)
        {
            if (RequestIdentify != null)
            {
                return RequestIdentify(arg);
            }
            else
            {
                return null;
            }
        }


        public Task<Response<IRI.Msh.Common.Primitives.Point>> GetPoint()
        {
            if (RequestGetPoint != null)
            {
                return RequestGetPoint();
            }
            else
            {
                return new Task<Response<IRI.Msh.Common.Primitives.Point>>(() => new Response<Msh.Common.Primitives.Point>() { Result = IRI.Msh.Common.Primitives.Point.NaN, IsFailed = true });
            }
        }

        #endregion


        //*****************************************Drawing Items*********************************************************
        #region Drawing Items

        List<Func<DrawingItemLayer, ILegendCommand>> drawingItemCommands = null;

        public List<Func<DrawingItemLayer, ILegendCommand>> DrawingItemCommands
        {
            get
            {
                if (drawingItemCommands == null)
                {
                    drawingItemCommands = new List<Func<DrawingItemLayer, ILegendCommand>>()
                    {
                       layer=> LegendCommand.CreateZoomToExtentCommand(this, layer),
                       layer=> LegendCommand.CreateRemoveDrawingItemLayer(this, layer),
                       layer=> LegendCommand.CreateEditDrawingItemLayer(this, layer),
                       layer=> LegendCommand.CreateExportDrawingItemLayerAsShapefile(this, layer),
                    };
                }

                return drawingItemCommands;
            }
            set { drawingItemCommands = value; }
        }

        public void AddDrawingItemCommand(Func<DrawingItemLayer, ILegendCommand> drawingItemCommandFunc)
        {
            this.DrawingItemCommands.Add(drawingItemCommandFunc);
        }


        private async Task DrawAsync(DrawMode mode)
        {
            this.IsPanMode = true;
            //ResetMode(mode);

            var drawingResult = await this.GetDrawingAsync(mode, MapSettings.DrawingOptions, true);

            if (!drawingResult.HasNotNullResult())
            {
                return;
            }

            var shapeItem = MakeShapeItem(drawingResult.Result, $"DRAWING {DrawingItems?.Count}");

            if (shapeItem != null)
            {
                //this.SetLayer(shapeItem.AssociatedLayer);
                AddDrawingItem(shapeItem);
                //this.Refresh();
            }
        }

        public void AddDrawingItem(Geometry<Msh.Common.Primitives.Point> drawing, string name = null, int id = int.MinValue, FeatureDataSource source = null)
        {
            if (drawing.IsNullOrEmpty())
                return;

            var shapeItem = MakeShapeItem(drawing, name ?? $"DRAWING {DrawingItems?.Count}", id, source);

            if (shapeItem != null)
            {
                //this.SetLayer(shapeItem.AssociatedLayer);
                AddDrawingItem(shapeItem);
                //this.Refresh();
            }
        }

        public void AddDrawingItem(DrawingItemLayer item)
        {
            this.DrawingItems.Add(item);

            //this.AddLayer(item.AssociatedLayer);
            this.AddLayer(item);
        }

        public void RemoveDrawingItem(DrawingItemLayer item)
        {
            this.DrawingItems.Remove(item);

            //this.RemoveLayer(item.AssociatedLayer);
            this.ClearLayer(item, true);

            this.ClearLayer(item.HighlightGeometryKey.ToString(), true, true);
        }

        public void RemoveAllDrawingItems()
        {
            for (int i = DrawingItems.Count - 1; i >= 0; i--)
            {
                RemoveDrawingItem(DrawingItems[i]);
            }
        }

        protected DrawingItemLayer MakeShapeItem(Geometry<Msh.Common.Primitives.Point> drawing, string name, int id = int.MinValue, FeatureDataSource source = null)
        {
            var shapeItem = new DrawingItemLayer(name, drawing, id, source);

            shapeItem.Title = name;

            TrySetCommandsForDrawingItemLayer(shapeItem);

            this.IsPanMode = true;

            return shapeItem;
        }

        #endregion


        //*****************************************General***************************************************************
        #region General

        public async Task SetIsBusy(bool isBusy)
        {
            _isBusy = isBusy;
            RaisePropertyChanged(nameof(IsBusy));

            await Wait();
        }

        private async Task Wait()
        {
            await Task.Run(async () =>
            {
                await Task.Delay(1000);
            });
        }

        public async void SetProxy(System.Net.WebProxy proxy)
        {
            this.RequestSetProxy?.Invoke(proxy);

            await CheckInternetAccess();
        }

        public void SetMapCursorSet1()
        {
            var zoomInCursor = new System.Windows.Input.Cursor(Application.GetResourceStream(new Uri("/IRI.Jab.Common;component/Assets/Cursors/MapCursorSet1/MagnifyPlusRightHanded.cur", UriKind.Relative)).Stream, false);
            this.SetDefaultCursor(IRI.Jab.Common.Model.MapAction.ZoomInRectangle, zoomInCursor);
            this.SetDefaultCursor(IRI.Jab.Common.Model.MapAction.ZoomIn, zoomInCursor);

            var zoomOutCursor = new System.Windows.Input.Cursor(Application.GetResourceStream(new Uri("/IRI.Jab.Common;component/Assets/Cursors/MapCursorSet1/MagnifyMinusRightHanded.cur", UriKind.Relative)).Stream, false);
            this.SetDefaultCursor(IRI.Jab.Common.Model.MapAction.ZoomOutRectangle, zoomOutCursor);
            this.SetDefaultCursor(IRI.Jab.Common.Model.MapAction.ZoomOut, zoomOutCursor);
        }

        public void SetDefaultCursor(MapAction action, Cursor cursor)
        {
            this.RequestSetDefaultCursor?.Invoke(action, cursor);
        }

        public void SetCursor(Cursor cursor)
        {
            this.RequestSetCursor?.Invoke(cursor);
        }

        public async Task CheckInternetAccess()
        {
            if (DoNotCheckInternet)
            {
                return;
            }

            var proxy = this.RequestGetProxy?.Invoke();

            this.IsConnected = await IRI.Ket.Common.Helpers.NetHelper.IsConnectedToInternet(proxy);
        }


        public void ClearLayer(ILayer layer, bool remove = true, bool forceRemove = false)
        {
            this.RequestClearLayer?.Invoke(layer, remove);

            this.RemoveSelectedLayers(l => l.LayerId == layer.LayerId);
        }

        public void ClearLayer(LayerType type, bool remove, bool forceRemove = false)
        {
            Clear(tag => tag.LayerType.HasFlag(type), remove, forceRemove);
            //this.RequestClearLayerByType?.Invoke(type, remove);
        }

        public void ClearLayer(string layerName, bool remove = true, bool forceRemove = false)
        {
            Clear(layer => layer.LayerName == layerName, remove, forceRemove);
            //this.RequestClearLayerByName?.Invoke(layerName, remove);
        }

        public void ClearAll()
        {
            this.Clear(new Predicate<ILayer>(l => l.CanUserDelete == true), true);

            this.DrawingItems.Clear();
        }


        private void Clear(Predicate<ILayer> layersToBeRemoved, bool remove, bool forceRemove = false)
        {
            this.RequestClearLayerByCriteria?.Invoke(layersToBeRemoved, remove, forceRemove);

            this.RemoveSelectedLayers(layersToBeRemoved);
        }

        //1397.08.17: potentionally error prone, do not consider removing SelectedLayers associated with the input criteria
        public void Clear(Predicate<LayerTag> criteria, bool remove, bool forceRemove = false)
        {
            this.RequestClearLayerByTag?.Invoke(criteria, remove, forceRemove);
        }

        //public void RemoveLayer(string layerName)
        //{
        //    this.RequestRemoveLayerByName?.Invoke(layerName);
        //}

        //public void RemoveLayer(ILayer layer, bool forceRemove = false)
        //{
        //    this.RequestRemoveLayer?.Invoke(layer, forceRemove);
        //}


        public void FireMapStatusChanged(MapStatus status)
        {
            this.MapStatus = status;
        }

        public void FireMapActionChanged(MapAction action)
        {
            this.MapAction = action;
        }

        public void FireExtentChanged(IRI.Msh.Common.Primitives.BoundingBox currentExtent)
        {
            this.RaisePropertyChanged(nameof(CurrentExtent));

            this.OnExtentChanged?.Invoke(null, EventArgs.Empty);
        }

        public void FireMouseMove(WpfPoint currentPoint)
        {
            this.CurrentPoint = new IRI.Msh.Common.Primitives.Point(currentPoint.X, currentPoint.Y);

            RaisePropertyChanged(nameof(CurrentPointInverseMapScale));
            RaisePropertyChanged(nameof(CurrentPointGroundResolution));

            this.OnMouseMove?.Invoke(this, currentPoint);
        }

        public void FireMapMouseUp(WpfPoint currentPoint)
        {
            this.OnMapMouseUp?.Invoke(this, currentPoint);
        }

        public void FireZoomChanged(double mapScale)
        {
            this.RaisePropertyChanged(nameof(this.CurrentZoomLevel));

            this.OnZoomChanged?.Invoke(this, mapScale);

            RaisePropertyChanged(nameof(InverseMapScale));
        }

        #endregion


        //*****************************************Editing***************************************************************
        #region Editing

        public async Task<Response<Geometry>> EditAsync(Geometry geometry, EditableFeatureLayerOptions options)
        {
            //this.IsEditMode = true;

            Response<Geometry> result = null;

            options = options ?? this.MapSettings.EditingOptions;

            this.MapPanel.Options = options;

            if (this.RequestEdit != null)
            {
                result = await RequestEdit(geometry, options);
            }
            else
            {
                result = await new Task<Response<Geometry>>(() => ResponseFactory.Create<Geometry>(null));
            }

            //this.IsEditMode = false;

            return result;
        }

        public Task<Response<Geometry>> EditAsync(List<IRI.Msh.Common.Primitives.Point> points, bool isClosed, int srid, EditableFeatureLayerOptions options = null)
        {
            if (points == null || points.Count < 1)
            {
                return new Task<Response<Geometry>>(null);
            }

            //1397.08.15.this is already done in EditAsync(geometry,options)
            //options = options ?? this.MapSettings.EditingOptions;
            //this.MapPanel.Options = options;

            var type = points.Count == 1 ? GeometryType.Point : (isClosed ? GeometryType.Polygon : GeometryType.LineString);

            Geometry geometry = new Geometry(points/*.ToArray()*/, type, srid);

            return EditAsync(geometry, options);
        }

        protected void CancelEdit()
        {
            //this.IsEditMode = false;

            this.RequestCancelEdit?.Invoke(); //this is called in MapViewer

            this.OnCancelEdit?.Invoke(null, EventArgs.Empty); //this is called in the apps
        }

        protected void FinishEdit()
        {
            //this.IsEditMode = false;

            this.RequestFinishEdit?.Invoke(); //this is called in MapViewer

            this.OnFinishEdit?.Invoke(null, EventArgs.Empty); //this is called in the apps
        }

        #endregion


        //*****************************************Layer Management******************************************************
        #region Layer Management

        public void RefreshLayerVisibility(ILayer layer)
        {
            this.RequestRefreshLayerVisibility?.Invoke(layer);
        }

        public void AddLayer(SpecialPointLayer layer)
        {
            this.RequestAddSpecialPointLayer?.Invoke(layer);
        }

        public void SetLayer(ILayer layer)
        {
            TrySetCommands(layer);

            this.RequestSetLayer?.Invoke(layer);
        }

        public void UnSetLayer(ILayer layer)
        {
            this.RequestRemoveLayer?.Invoke(layer);
        }

        public void RegisterLayerWidthMap(VectorLayer layer)
        {
            layer.RequestChangeSymbology = l => this.RequestShowSymbologyView?.Invoke(l);
        }

        protected void TrySetCommands(ILayer layer)
        {

            if (layer is VectorLayer)
            {
                if (!(layer?.Commands?.Count > 0))
                {
                    var commands = new List<ILegendCommand>();

                    foreach (var item in DefaultVectorLayerCommands)
                    {
                        commands.Add(item(this, layer));
                    }

                    layer.Commands = commands;

                    //layer.Commands = new List<ILegendCommand>()
                    //{
                    //    LegendCommand.CreateZoomToExtentCommand(this, layer),
                    //    LegendCommand.CreateSelectByDrawing<ISqlGeometryAware>(this, (VectorLayer)layer),
                    //    LegendCommand.CreateShowAttributeTable<ISqlGeometryAware>(this, (VectorLayer)layer),
                    //    LegendCommand.CreateClearSelected(this, (VectorLayer)layer),
                    //    LegendCommand.CreateRemoveLayer(this, layer),
                    //};
                }

                if (!(layer?.FeatureTableCommands?.Count > 0))
                {
                    var commands = new List<IFeatureTableCommand>();

                    foreach (var item in DefaultVectorLayerFeatureTableCommands)
                    {
                        commands.Add(item(this));
                    }

                    layer.FeatureTableCommands = commands;
                }

                if ((layer as VectorLayer).RequestChangeSymbology == null)
                {
                    (layer as VectorLayer).RequestChangeSymbology = l => this.RequestShowSymbologyView?.Invoke(l);
                }
            }
            else if (layer.Type == LayerType.Raster || layer.Type == LayerType.ImagePyramid)
            {
                if (!(layer?.Commands?.Count > 0))
                {
                    layer.Commands = new List<ILegendCommand>()
                    {
                        LegendCommand.CreateZoomToExtentCommand(this, layer),
                        LegendCommand.CreateRemoveLayer(this, layer),
                    };
                }
            }
        }

        protected void TrySetCommandsForDrawingItemLayer(DrawingItemLayer layer)
        {
            layer.Commands = this.DrawingItemCommands.Select(dic => dic(layer))?.ToList();

            //1398.11.14
            //layer.Commands = new List<ILegendCommand>()
            //        {
            //            LegendCommand.CreateZoomToExtentCommand(this, layer),
            //            LegendCommand.CreateRemoveDrawingItemLayer(this, layer),
            //            LegendCommand.CreateEditDrawingItemLayer(this, layer),
            //            LegendCommand.CreateExportDrawingItemLayerAsShapefile(this, layer),
            //        };

            layer.RequestHighlightGeometry = async di =>
             {
                 //this.ClearLayer(layer, true);

                 //this.AddLayer(layer);

                 if (di.CanShowHighlightGeometry())
                 {
                     await SelectDrawingItem(di);
                 }
                 else
                 {
                     // 1399.12.27
                     //ClearLayer(LayerType.Selection, true, true);

                     ClearLayer(di.HighlightGeometryKey.ToString(), true, true);
                 }

                 //var defaultFill = layer.OriginalSymbology.Fill.AsSolidColor();

                 //var defaultStroke = layer.OriginalSymbology.Stroke.AsSolidColor();


                 //if (di.IsSelectedInToc)
                 //{
                 //    if (defaultFill != null)
                 //    {
                 //        layer.VisualParameters.Fill = new System.Windows.Media.SolidColorBrush(VisualParameters.DefaultSelectionFill.Color);
                 //    }

                 //    if (defaultStroke != null)
                 //    {
                 //        layer.VisualParameters.Stroke = new System.Windows.Media.SolidColorBrush(VisualParameters.DefaultSelectionStroke.Color);
                 //    }

                 //    this.ClearLayer(layer, true);

                 //    this.AddLayer(layer);
                 //}
                 //else
                 //{
                 //    if (defaultFill != null)
                 //    {
                 //        layer.VisualParameters.Fill = new System.Windows.Media.SolidColorBrush(defaultFill.Value);
                 //    }

                 //    if (defaultStroke != null)
                 //    {
                 //        layer.VisualParameters.Stroke = new System.Windows.Media.SolidColorBrush(defaultStroke.Value);
                 //    }

                 //    this.ClearLayer(layer, true);

                 //    this.AddLayer(layer);
                 //}

             };

            layer.RequestChangeVisibilityForHighlightGeometry = async di =>
            {
                if (di.CanShowHighlightGeometry())
                {
                    await SelectDrawingItem(di);
                }
                else
                { 
                    ClearLayer(di.HighlightGeometryKey.ToString(), true, true);
                }
            };

            if (layer.RequestChangeSymbology == null)
            {
                layer.RequestChangeSymbology = l => this.RequestShowSymbologyView?.Invoke(l);
            }
        }

        //protected void TrySetCommands<T>(ILayer layer) where T : class, ISqlGeometryAware
        //{
        //    if (layer is VectorLayer)
        //    {
        //        if (!(layer?.Commands?.Count > 0))
        //        {
        //            //1399.06.10
        //            //layer.Commands = new List<ILegendCommand>()
        //            //{
        //            //    LegendCommand.CreateZoomToExtentCommand(this, layer),
        //            //    LegendCommand.CreateSelectByDrawing<T>(this, (VectorLayer)layer),
        //            //    LegendCommand.CreateShowAttributeTable<T>(this, (VectorLayer)layer),
        //            //    LegendCommand.CreateClearSelected(this, (VectorLayer)layer),
        //            //    LegendCommand.CreateRemoveLayer(this, layer),
        //            //};

        //            var commands = new List<ILegendCommand>();

        //            foreach (var item in DefaultVectorLayerCommands)
        //            {
        //                commands.Add(item(this, layer));
        //            }

        //            layer.Commands = commands;
        //        }
        //        if ((layer as VectorLayer).RequestChangeSymbology == null)
        //        {
        //            (layer as VectorLayer).RequestChangeSymbology = l => this.RequestShowSymbologyView?.Invoke(l);
        //        }
        //    }

        //    //this should not happed because source must be FeatureDataSource<T>
        //    else if (layer.Type == LayerType.Raster || layer.Type == LayerType.ImagePyramid)
        //    {
        //        if (!(layer?.Commands?.Count > 0))
        //        {
        //            layer.Commands = new List<ILegendCommand>()
        //            {
        //                LegendCommand.CreateZoomToExtentCommand(this, layer),
        //                LegendCommand.CreateRemoveLayer(this, layer),
        //            };
        //        }
        //    }
        //}


        public void AddLayer(ILayer layer)
        {
            if (!layer.IsGroupLayer)
            {
                TrySetCommands(layer);
            }

            this.RequestAddLayer?.Invoke(layer);
        }

        public void AddLayer<T>(ILayer layer) where T : class, ISqlGeometryAware
        {
            TrySetCommands(layer);

            this.RequestAddLayer?.Invoke(layer);
        }


        public void RemoveLayer(string layerName)
        {
            this.RequestRemoveLayerByName?.Invoke(layerName);
        }

        #endregion


        //*****************************************PolyBezier************************************************************
        #region PolyBezier

        public void AddPolyBezierLayer(string name, List<IRI.Msh.Common.Primitives.Point> bezierPoints, System.Windows.Media.Geometry symbol, VisualParameters decorationVisuals, bool showSymbolOnly)
        {
            this.RequestAddPolyBezier?.Invoke(name, bezierPoints, symbol, showSymbolOnly, decorationVisuals);
        }

        public void RemovePolyBezierLayers()
        {
            this.RequestRemovePolyBezierLayers?.Invoke();
        }

        protected Task<Response<PolyBezierLayer>> GetBezier(System.Windows.Media.Geometry symbol, VisualParameters decorationVisual)
        {
            if (RequestGetBezier != null)
            {
                return RequestGetBezier(symbol, decorationVisual);
            }
            else
            {
                return new Task<Response<PolyBezierLayer>>(() => new Response<PolyBezierLayer>() { IsFailed = true });
            }
        }

        #endregion


        //*****************************************Zoom******************************************************************
        #region Zoom

        public void ZoomToLevelAndCenter(int zoomLevel, IRI.Msh.Common.Primitives.Point centerMapPoint, Action callback = null, bool withAnimation = true)
        {
            this.RequestZoomToLevelAndCenter?.Invoke(zoomLevel, centerMapPoint, callback, withAnimation);
        }

        public void EnableRectangleZoomIn()
        {
            this.RequestEnableRectangleZoom?.Invoke();
        }

        public void EnableZoomOut()
        {
            this.RequestEnableZoomOut?.Invoke();
        }

        public void GoToIranExtent()
        {
            this.RequestIranExtent?.Invoke();
        }

        public void FullExtent()
        {
            this.RequestFullExtent?.Invoke();
        }

        public void Zoom(double mapScale)
        {
            this.RequestZoomToScale?.Invoke(mapScale);
        }

        public void Zoom(double mapScale, IRI.Msh.Common.Primitives.Point center)
        {
            this.RequestZoomToPoint?.Invoke(center, mapScale);
        }

        public void ZoomToGoogleScale(int googleScale, IRI.Msh.Common.Primitives.Point mapCenter, Action callback)
        {
            this.RequestZoomToGoogleScale?.Invoke(mapCenter, googleScale, callback);
        }

        public void ZoomToExtent(IRI.Msh.Common.Primitives.BoundingBox boundingBox, bool isExactExtent, Action callback = null)
        {
            this.RequestZoomToExtent?.Invoke(boundingBox, isExactExtent, callback);
        }

        public void Zoom(SqlGeometry geometry)
        {
            this.RequestZoomToFeature?.Invoke(geometry);
        }

        #endregion


        //*****************************************Pan*******************************************************************
        #region Pan

        public void Pan()
        {
            this.RequestPan?.Invoke();
        }

        public void PanTo(IRI.Msh.Common.Primitives.Point point, Action callback)
        {
            this.RequestPanTo?.Invoke(point, callback);
        }

        public void PanToGeographicPoint(IRI.Msh.Common.Primitives.Point point, Action callback = null)
        {
            var webMercatorPoint = IRI.Msh.CoordinateSystem.MapProjection.MapProjects.GeodeticWgs84ToWebMercator(point);

            this.PanTo(webMercatorPoint, callback);
        }

        #endregion


        //*****************************************Drawing***************************************************************
        #region Drawing

        public async Task<Response<Geometry>> GetDrawingAsync(DrawMode mode, EditableFeatureLayerOptions options = null, bool display = true)
        {
            //this.IsDrawMode = true;

            options = options ?? this.MapSettings.DrawingOptions;

            this.MapPanel.Options = options;

            var result = await this.RequestGetDrawingAsync?.Invoke(mode, options, display);

            //this.IsDrawMode = false;

            return result;
            //return tcs.Task;
        }

        protected void CancelNewDrawing()
        {
            //this.IsDrawMode = false;

            this.RequestCancelNewDrawing?.Invoke(); //this is called in MapViewer

            this.OnCancelNewDrawing?.Invoke(null, EventArgs.Empty); //this is called in the apps
        }

        private void FinishNewDrawing()
        {
            //this.IsDrawMode = false;

            this.RequestFinishNewDrawing?.Invoke(); //this is called in MapViewer

            this.OnFinishNewDrawing?.Invoke(null, EventArgs.Empty); //this is called in the apps
        }

        private void FinishDrawingPart()
        {
            this.RequestFinishDrawingPart?.Invoke();
        }

        protected void DeleteDrawing()
        {
            //this.IsEditMode = false;

            this.RequestCancelEdit?.Invoke(); //this is called in MapViewer

            this.OnDeleteDrawing?.Invoke(null, EventArgs.Empty); //this is called in the apps
        }

        private void AddPointToNewDrawing()
        {
            this.RequestAddPointToNewDrawing?.Invoke(this.MapPanel.CurrentWebMercatorEditingPoint);
        }

        #endregion


        //*****************************************RightClickOptions*****************************************************
        #region RightClickOptions

        protected void RegisterRightClickMapOptions(FrameworkElement view, ILocateable dataContext)
        {
            this.RequestRegisterMapOptions?.Invoke(new MapOptionsEventArgs<FrameworkElement>(view, dataContext));
        }

        protected void UnregisterRightClickMapOptions()
        {
            this.RequestUnregisterMapOptions?.Invoke();
        }

        protected void RemoveMapOptions()
        {
            this.RequestRemoveMapOptions?.Invoke();
        }



        #endregion


        //*****************************************Measure***************************************************************
        #region Measure

        protected async Task<Response<Geometry>> Measure(DrawMode mode, Action action = null)
        {
            //this.IsMeasureMode = true;
            try
            {
                this.MapPanel.Options = MapSettings.DrawingMeasureOptions;

                var result = await this.RequestMeasure?.Invoke(mode, MapSettings.DrawingMeasureOptions, MapSettings.EditingMeasureOptions, action);

                //this.IsMeasureMode = false;

                return result;


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void CancelMeasure()
        {
            //this.IsMeasureMode = false;

            this.RequestCancelMeasure?.Invoke();
        }

        #endregion


        public void UpdateCurrentEditingPoint(IRI.Msh.Common.Primitives.Point webMercatorPoint)
        {
            MapPanel.UpdateCurrentEditingPoint(webMercatorPoint);
        }

        public void Print()
        {
            this.RequestPrint?.Invoke();
        }

        public void Refresh()
        {
            this.RequestRefresh?.Invoke();
        }



        //*****************************************General**************************************************************
        #region Shapefile/Worldfile

        public virtual void AddShapefile()
        {
            this.IsBusy = true;

            var fileName = this.OpenFile("shapefile|*.shp");

            if (!File.Exists(fileName))
            {
                this.IsBusy = false;

                return;
            }

            //FileInfo info = new FileInfo(fileName);

            //if (info.Length / 10000.0 > 1000) //5k
            //{
            //    ShowMessage("حجم فایل انتخابی بیش از حد مجاز است");

            //    return;
            //}

            AddShapefile(fileName);
        }

        public async Task AddShapefile(string fileName)
        {
            try
            {
                var dataSource = await ShapefileDataSourceFactory.CreateAsync(fileName, new WebMercator());

                var vectorLayer = new VectorLayer(Path.GetFileNameWithoutExtension(fileName), dataSource,
                    new VisualParameters(null, BrushHelper.PickBrush(), 3, 1),
                    LayerType.VectorLayer,
                    RenderingApproach.Default,
                    RasterizationApproach.GdiPlus, ScaleInterval.All);

                this.AddLayer<SqlFeature>(vectorLayer);
            }
            catch (Exception ex)
            {
                await ShowMessageAsync(null, ex.Message);
            }
            finally
            {
                this.IsBusy = false;
            }
        }

        public virtual void AddWorldfile()
        {
            this.IsBusy = true;

            var fileName = this.OpenFile("Worldfile|*.bmp;*.jpg;*.jpeg;*.png;*.tif;*.tiff");

            if (!File.Exists(fileName))
            {
                this.IsBusy = false;

                return;
            }

            AddWorldfile(fileName, Msh.CoordinateSystem.MapProjection.SridHelper.GeodeticWGS84);
        }

        public virtual void AddWorldfile(string fileName, int srid)
        {
            try
            {
                var dataSource = GeoRasterFileDataSource.Create(fileName, srid);

                if (dataSource == null)
                {
                    return;
                }

                var rasterLayer = new RasterLayer(dataSource, System.IO.Path.GetFileNameWithoutExtension(fileName), ScaleInterval.All, false, false, Visibility.Visible, .9);

                //this.SetLayer(rasterLayer);

                this.AddLayer(rasterLayer);

                //this.Refresh();

            }
            catch (Exception ex)
            {
                ShowMessageAsync(null, ex.Message);
            }
            finally
            {
                this.IsBusy = false;
            }

        }

        public virtual async Task AddZippedImagePyramid(object owner)
        {
            try
            {
                this.IsBusy = true;

                var fileName = this.OpenFile("Image Pyramid file|*.pyrmd", owner);

                if (!File.Exists(fileName))
                {
                    this.IsBusy = false;

                    return;
                }

                var rasterLayer = new RasterLayer(new ZippedImagePyramidDataSource(fileName),
                    System.IO.Path.GetFileNameWithoutExtension(fileName),
                    ScaleInterval.All,
                    false,
                    true,
                    System.Windows.Visibility.Visible,
                    1);

                this.AddLayer(rasterLayer);
            }
            catch (Exception ex)
            {
                await this.ShowMessageAsync(owner, ex.Message, "خطا");
            }
            finally
            {
                IsBusy = false;
            }

        }

        #endregion


        #region Command

        //
        private RelayCommand _goToIranExtentCommand;

        public RelayCommand GoToIranExtentCommand
        {
            get
            {
                if (_goToIranExtentCommand == null)
                {
                    _goToIranExtentCommand = new RelayCommand(param => GoToIranExtent());
                }

                return _goToIranExtentCommand;
            }
        }

        private RelayCommand _fullExtentCommand;

        public RelayCommand FullExtentCommand
        {
            get
            {
                if (_fullExtentCommand == null)
                {
                    _fullExtentCommand = new RelayCommand(param => FullExtent());
                }

                return _fullExtentCommand;
            }
        }

        private RelayCommand _clearAllCommand;

        public RelayCommand ClearAllCommand

        {
            get
            {
                if (_clearAllCommand == null)
                {
                    _clearAllCommand = new RelayCommand(param =>
                    {
                        this.RequestClearAll?.Invoke();
                    });
                }

                return _clearAllCommand;
            }
        }

        private RelayCommand _rectangleZoomCommand;

        public RelayCommand RectangleZoomCommand
        {
            get
            {
                if (_rectangleZoomCommand == null)
                {
                    _rectangleZoomCommand = new RelayCommand(param => this.EnableRectangleZoomIn());
                }

                return _rectangleZoomCommand;
            }
        }

        private RelayCommand _zoomOutCommand;

        public RelayCommand ZoomOutCommand
        {
            get
            {
                if (_zoomOutCommand == null)
                {
                    _zoomOutCommand = new RelayCommand(param => this.EnableZoomOut());
                }

                return _zoomOutCommand;
            }
        }

        private RelayCommand _panCommand;

        public RelayCommand PanCommand
        {
            get
            {
                if (_panCommand == null)
                {
                    _panCommand = new RelayCommand(param => this.Pan());
                }

                return _panCommand;
            }
        }


        //
        private RelayCommand _checkInternetAccessCommand;

        public RelayCommand CheckInternetAccessCommand
        {
            get
            {
                if (_checkInternetAccessCommand == null)
                {
                    _checkInternetAccessCommand = new RelayCommand(async param => { await CheckInternetAccess(); });
                }

                return _checkInternetAccessCommand;
            }
        }
        //
        private RelayCommand _addShapefileCommand;

        public RelayCommand AddShapefileCommand
        {
            get
            {
                if (_addShapefileCommand == null)
                {
                    _addShapefileCommand = new RelayCommand(param =>
                    {
                        AddShapefile();
                    });
                }
                return _addShapefileCommand;
            }
        }



        private RelayCommand _addWorldfileCommand;

        public RelayCommand AddWorldfileCommand
        {
            get
            {
                if (_addWorldfileCommand == null)
                {
                    _addWorldfileCommand = new RelayCommand(param => { AddWorldfile(); });
                }
                return _addWorldfileCommand;
            }
        }


        private RelayCommand _addZippedImagePyramidCommand;

        public RelayCommand AddZippedImagePyramidCommand
        {
            get
            {
                if (_addZippedImagePyramidCommand == null)
                {
                    _addZippedImagePyramidCommand = new RelayCommand(async param => await AddZippedImagePyramid(param));
                }

                return _addZippedImagePyramidCommand;
            }
        }


        private RelayCommand _cancelNewDrawingCommand;

        public RelayCommand CancelNewDrawingCommand
        {
            get
            {
                if (_cancelNewDrawingCommand == null)
                {
                    _cancelNewDrawingCommand = new RelayCommand(param => this.CancelNewDrawing());
                }
                return _cancelNewDrawingCommand;
            }
        }

        private RelayCommand _finishDrawingPartCommand;

        public RelayCommand FinishDrawingPartCommand
        {
            get
            {
                if (_finishDrawingPartCommand == null)
                {
                    _finishDrawingPartCommand = new RelayCommand(param => this.FinishDrawingPart());
                }
                return _finishDrawingPartCommand;
            }
        }

        private RelayCommand _finishNewDrawingCommand;

        public RelayCommand FinishNewDrawingCommand
        {
            get
            {
                if (_finishNewDrawingCommand == null)
                {
                    _finishNewDrawingCommand = new RelayCommand(param => this.FinishNewDrawing());
                }
                return _finishNewDrawingCommand;
            }
        }

        private RelayCommand _cancelEditDrawingCommand;

        public RelayCommand CancelEditDrawingCommand
        {
            get
            {
                if (_cancelEditDrawingCommand == null)
                {
                    _cancelEditDrawingCommand = new RelayCommand(param => this.CancelEdit());
                }
                return _cancelEditDrawingCommand;
            }
        }

        private RelayCommand _deleteDrawingCommand;

        public RelayCommand DeleteDrawingCommand
        {
            get
            {
                if (_deleteDrawingCommand == null)
                {
                    _deleteDrawingCommand = new RelayCommand(param => this.DeleteDrawing());
                }
                return _deleteDrawingCommand;
            }

        }


        private RelayCommand _finishEditDrawingCommand;

        public RelayCommand FinishEditDrawingCommand
        {
            get
            {

                if (_finishEditDrawingCommand == null)
                {
                    _finishEditDrawingCommand = new RelayCommand(param => this.FinishEdit());
                }
                return _finishEditDrawingCommand;
            }
        }


        private RelayCommand _changeBaseMapCommand;

        public RelayCommand ChangeBaseMapCommand
        {
            get
            {
                if (_changeBaseMapCommand == null)
                {
                    _changeBaseMapCommand = new RelayCommand(async param =>
                    {
                        try
                        {
                            //var args = param.ToString().Split(',');

                            //var provider = args[0];

                            //TileType tileType = (TileType)Enum.Parse(typeof(TileType), args[1]);

                            //this.ProviderTypeFullName = provider;

                            await this.SetTileBaseMap(param?.ToString());

                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine("exception at ChangeBaseMapCommand: " + ex);
                        }
                    });
                }

                return _changeBaseMapCommand;
            }
        }

        //**********Measurements
        private RelayCommand _measureLengthCommand;

        public RelayCommand MeasureLengthCommand
        {
            get
            {
                if (_measureLengthCommand == null)
                {
                    _measureLengthCommand = new RelayCommand(async param =>
                    {
                        //this.MapSettings.DrawingMeasureOptions.IsEdgeLabelVisible = param == null ? true : (bool)param;

                        await this.Measure(DrawMode.Polyline);
                    });
                }

                return _measureLengthCommand;
            }
        }

        private RelayCommand _measureAreaCommand;

        public RelayCommand MeasureAreaCommand
        {
            get
            {
                if (_measureAreaCommand == null)
                {
                    _measureAreaCommand = new RelayCommand(async param =>
                    {
                        //this.MapSettings.DrawingMeasureOptions.IsEdgeLabelVisible = param == null ? true : (bool)param;

                        await this.Measure(DrawMode.Polygon);
                    });
                }

                return _measureAreaCommand;
            }
        }

        private RelayCommand _cancelMeasureCommand;

        public RelayCommand CancelMeasureCommand
        {
            get
            {
                if (_cancelMeasureCommand == null)
                {
                    _cancelMeasureCommand = new RelayCommand(param => this.CancelMeasure());
                }

                return _cancelMeasureCommand;
            }
        }


        private RelayCommand _drawPointCommand;

        public RelayCommand DrawPointCommand
        {
            get
            {
                if (_drawPointCommand == null)
                {
                    _drawPointCommand = new RelayCommand(async param => await DrawAsync(DrawMode.Point));
                }
                return _drawPointCommand;
            }
        }

        private RelayCommand _drawPolygonCommand;

        public RelayCommand DrawPolygonCommand
        {
            get
            {
                if (_drawPolygonCommand == null)
                {
                    _drawPolygonCommand = new RelayCommand(async param => await DrawAsync(DrawMode.Polygon));
                }
                return _drawPolygonCommand;
            }
        }

        private RelayCommand _drawPolylineCommand;

        public RelayCommand DrawPolylineCommand
        {
            get
            {
                if (_drawPolylineCommand == null)
                {
                    _drawPolylineCommand = new RelayCommand(async param => await DrawAsync(DrawMode.Polyline));
                }
                return _drawPolylineCommand;
            }
        }

        private RelayCommand _goToCommand;

        public RelayCommand GoToCommand
        {
            get
            {
                if (_goToCommand == null)
                {
                    _goToCommand = new RelayCommand(param => this.RequestShowGoToView?.Invoke());
                }

                return _goToCommand;
            }
        }

        private RelayCommand _addPointToNewDrawingCommand;

        public RelayCommand AddPointToNewDrawingCommand
        {
            get
            {
                if (_addPointToNewDrawingCommand == null)
                {
                    _addPointToNewDrawingCommand = new RelayCommand(param => AddPointToNewDrawing());
                }

                return _addPointToNewDrawingCommand;
            }
        }

        private RelayCommand _printCommand;

        public RelayCommand PrintCommand
        {
            get
            {
                if (_printCommand == null)
                {
                    _printCommand = new RelayCommand(param => this.Print());
                }

                return _printCommand;
            }
        }

        #endregion


        #region Events

        public event EventHandler<WpfPoint> OnMouseMove;

        public event EventHandler<double> OnZoomChanged;

        public event EventHandler<WpfPoint> OnMapMouseUp;

        public event EventHandler OnExtentChanged;

        public event EventHandler OnCancelEdit;

        public event EventHandler OnFinishEdit;

        public event EventHandler OnCancelNewDrawing;

        public event EventHandler OnFinishNewDrawing;

        public event EventHandler OnDeleteDrawing;

        #endregion

        public virtual Task Initialize()
        {
            return Task.CompletedTask;
        }


        public virtual void RegisterMapOptions()
        {

        }
    }
}
