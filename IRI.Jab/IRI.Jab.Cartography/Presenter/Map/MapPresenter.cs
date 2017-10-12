using IRI.Ham.SpatialBase.Primitives;
using IRI.Jab.Cartography.Model;
using IRI.Jab.Cartography.TileServices;
using IRI.Jab.Common;
using IRI.Jab.Common.Assets.Commands;
using IRI.Jab.Common.Extensions;
using IRI.Jab.Common.Model;
using IRI.Ket.SpatialExtensions;
using Microsoft.SqlServer.Types;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace IRI.Jab.Cartography.Presenter.Map
{
    public class MapPresenter : BasePresenter
    {
        #region Properties

        private TileType _baseMapType = TileType.Hybrid;

        public TileType BaseMapType
        {
            get { return _baseMapType; }
            set
            {
                if (_baseMapType == value)
                    return;

                _baseMapType = value;
                RaisePropertyChanged();

                SetBaseMap(value);
            }
        }

        private async void SetBaseMap(TileType type)
        {
            await CheckInternetAccess();

            this.RequestChangeBaseMap?.Invoke(type);
        }

        public string GooglePath { get; set; }

        private bool _isConnected = false;

        public bool IsConnected
        {
            get { return _isConnected; }
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
                _mapStatus = value;
                RaisePropertyChanged();
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

        private ObservableCollection<ILayer> _layers;

        public ObservableCollection<ILayer> Layers
        {
            get { return _layers; }
            set
            {
                _layers = value;
                RaisePropertyChanged();
            }
        }

        public double MapScale
        {
            get
            {
                return this.RequestMapScale?.Invoke() ?? 1;
            }
        }

        public int CurrentZoomLevel { get { return this.RequestCurrentZoomLevel.Invoke(); } }

        public IRI.Ham.SpatialBase.BoundingBox CurrentExtent
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
                RaisePropertyChanged(nameof(IsNewDrawOrEditMode));
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
                RaisePropertyChanged(nameof(IsNewDrawOrEditMode));
            }
        }

        public bool IsNewDrawOrEditMode
        {
            get { return IsEditMode || IsDrawMode; }
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

        #region Actions & Funcs

        public Action<System.Net.WebProxy> RequestSetProxy;

        public Action<MapAction, Cursor> RequestSetDefaultCursor;

        public Action<Cursor> RequestSetCursor;

        public Func<double> RequestGetActualWidth;

        public Func<double> RequestGetActualHeight;

        public Action<MapPresenter> RegisterAction;

        public Action<bool> RequestSetConnectedState;

        public Action<TileType> RequestChangeBaseMap;

        public Func<double> RequestMapScale;

        public Func<int> RequestCurrentZoomLevel;

        public Func<Ham.SpatialBase.BoundingBox> RequestCurrentExtent;

        public Action RequestRefresh;

        public Action RequestIranExtent;

        public Action RequestFullExtent;

        public Action<bool> RequestEnableZoomOnMouseWheel;

        public Action<double> RequestZoomToScale;

        public Action<Ham.SpatialBase.Point, double> RequestZoomToPoint;

        public Action<Ham.SpatialBase.BoundingBox, Action> RequestZoomToExtent;

        public Action<SqlGeometry> RequestZoomToFeature;

        public Action RequestEnableRectangleZoom;

        public Action RequestEnableZoomOut;

        public Action RequestPan;

        public Action<Point, Action> RequestPanTo;


        public Action<MapOptionsEventArgs<FrameworkElement>> RequestRegisterMapOptions;

        public Action RequestUnregisterMapOptions;

        public Action RequestRemoveMapOptions;


        public Action RequestCopyCurrentLocationToClipboard;


        public Action<ILayer> RequestRemoveLayer;

        public Action<string> RequestRemoveLayerByName;

        public Action<LayerType, bool> RequestClearLayer;

        public Action<string, bool> RequestClearLayerByName;

        public Action RequestRemovePolyBezierLayers;



        public Action<List<Point>> RequestFlashPoints;

        public Action<Point> RequestFlashPoint;


        public Action<List<SqlGeometry>, string, VisualParameters> RequestAddGeometries;

        public Action<SpecialPointLayer> RequestAddSpecialPointLayer;

        public Action<ILayer> RequestSetLayer;

        public Action<VectorLayer> RequestAddLayer;

        public Action<string, List<Ham.SpatialBase.Point>, System.Windows.Media.Geometry, bool, VisualParameters> RequestAddPolyBezier;


        public Func<DrawMode, bool, Task<Geometry>> RequestGetGeometry;

        public Action RequestClearMap;

        public Action RequestCancelGetDrawing;

        public Action RequestCancelEditGeometry;

        public Action RequestFinishEditGeometry;

        public Func<Geometry, Task<Geometry>> RequestEdit;

        public Func<System.Windows.Media.Geometry, VisualParameters, Task<PolyBezierLayer>> RequestGetBezier;


        public Func<SqlGeometry, ObservableCollection<System.Data.DataTable>> RequestIdentify;

        public Func<Task<IRI.Ham.SpatialBase.Point>> RequestGetPoint;

        #endregion

        #region Methods

        public void SetProxy(System.Net.WebProxy proxy)
        {
            this.RequestSetProxy?.Invoke(proxy);
        }

        public void SetDefaultCursor(MapAction action, Cursor cursor)
        {
            this.RequestSetDefaultCursor?.Invoke(action, cursor);
        }

        public void SetCursor(Cursor cursor)
        {
            this.RequestSetCursor?.Invoke(cursor);
        }

        public void Pan()
        {
            this.RequestPan?.Invoke();
        }

        public void EnableZoomOnMouseWheel(bool isEnabled)
        {
            this.RequestEnableZoomOnMouseWheel?.Invoke(isEnabled);
        }

        public void Refresh()
        {
            this.RequestRefresh?.Invoke();
        }

        public async Task CheckInternetAccess()
        {
            this.IsConnected = await IRI.Ket.Common.Helpers.NetHelper.IsConnectedToInternet();
        }

        public void DrawGeometries(List<SqlGeometry> geometry, string name, VisualParameters parameters)
        {
            this.RequestAddGeometries?.Invoke(geometry, name, parameters);
        }

        public void DrawGeometry(SqlGeometry geometry, string name, VisualParameters parameters)
        {
            DrawGeometries(new List<SqlGeometry> { geometry }, name, parameters);
        }

        public void AddLayer(SpecialPointLayer layer)
        {
            this.RequestAddSpecialPointLayer?.Invoke(layer);
        }

        public void SetLayer(ILayer layer)
        {
            this.RequestSetLayer?.Invoke(layer);
        }

        public void AddLayer(VectorLayer layer)
        {
            this.RequestAddLayer?.Invoke(layer);
        }

        public void AddPolyBezierLayer(string name, List<Ham.SpatialBase.Point> bezierPoints, System.Windows.Media.Geometry symbol, VisualParameters decorationVisuals, bool showSymbolOnly)
        {
            this.RequestAddPolyBezier?.Invoke(name, bezierPoints, symbol, showSymbolOnly, decorationVisuals);
        }

        public void RemovePolyBezierLayers()
        {
            this.RequestRemovePolyBezierLayers?.Invoke();
        }

        public void ClearLayer(LayerType type, bool remove)
        {
            RequestClearLayer?.Invoke(type, remove);
        }

        public void ClearLayer(string layerName, bool remove)
        {
            this.RequestClearLayerByName?.Invoke(layerName, remove);
        }

        public void RemoveLayer(string layerName)
        {
            this.RequestRemoveLayerByName?.Invoke(layerName);
        }

        public void RemoveLayer(ILayer layer)
        {
            this.RequestRemoveLayer?.Invoke(layer);
        }

        public void FlashPoints(List<Point> points)
        {
            this.RequestFlashPoints?.Invoke(points);
        }

        public void FlashPoint(Point point)
        {
            this.RequestFlashPoint?.Invoke(point);
        }

        public void PanTo(Point point, Action callback)
        {
            this.RequestPanTo?.Invoke(point, callback);
        }

        public void PanToGeographicPoint(IRI.Ham.SpatialBase.IPoint point, Action callback = null)
        {
            var mercatorPoint = IRI.Ham.CoordinateSystem.MapProjection.MapProjects.GeodeticToMercator(point).AsWpfPoint();

            this.PanTo(mercatorPoint, callback);
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

        public void ClearMap()
        {
            //for (int i = Layers.Count - 1; i >= 0; i--)
            //{
            //    RemoveLayer(Layers[i]);
            //}
            ClearLayer(LayerType.VectorLayer, true);
            ClearLayer(LayerType.Complex, true);
            ClearLayer(LayerType.Drawing, true);
            ClearLayer(LayerType.Feature, true);
            ClearLayer(LayerType.Selection, true);

        }


        public void Zoom(double mapScale)
        {
            this.RequestZoomToScale?.Invoke(mapScale);
        }

        public void Zoom(double mapScale, Ham.SpatialBase.Point center)
        {
            this.RequestZoomToPoint?.Invoke(center, mapScale);
        }

        public void ZoomToExtent(Ham.SpatialBase.BoundingBox boundingBox, Action callback = null)
        {
            this.RequestZoomToExtent?.Invoke(boundingBox, callback);
        }

        public void Zoom(SqlGeometry geometry)
        {
            if (this.RequestZoomToFeature != null)
            {
                this.RequestZoomToFeature(geometry);
            }
        }

        protected void RegisterRightClickMapOptions(FrameworkElement view, ILocateable dataContext)
        {
            this.RequestRegisterMapOptions?.Invoke(new MapOptionsEventArgs<FrameworkElement>(view, dataContext));
        }

        protected void UnregisterRightClickMapOptions()
        {
            this.RequestUnregisterMapOptions?.Invoke();
        }

        protected async Task<Geometry> GetDrawing(DrawMode mode, bool display = true)
        {
            this.IsDrawMode = true;

            var result = await this.RequestGetGeometry?.Invoke(mode, display);

            this.IsDrawMode = false;

            return result;
            //return tcs.Task;
        }

        protected void CancelGetDrawing()
        {
            this.IsDrawMode = false;

            this.RequestCancelGetDrawing?.Invoke(); //this is called in MapViewer

            this.OnCancelDrawGeometry?.Invoke(null, EventArgs.Empty); //this is called in the apps
        }

        protected void CancelEditGeometry()
        {
            this.IsEditMode = false;

            this.RequestCancelEditGeometry?.Invoke(); //this is called in MapViewer

            this.OnCancelDrawGeometry?.Invoke(null, EventArgs.Empty); //this is called in the apps
        }

        protected void FinishEditGeometry()
        {
            this.IsEditMode = false;

            this.RequestFinishEditGeometry?.Invoke(); //this is called in MapViewer

            this.OnFinishEditGeometry?.Invoke(null, EventArgs.Empty); //this is called in the apps
        }

        protected Task<PolyBezierLayer> GetBezier(System.Windows.Media.Geometry symbol, VisualParameters decorationVisual)
        {
            if (RequestGetBezier != null)
            {
                return RequestGetBezier(symbol, decorationVisual);
            }
            else
            {
                return new Task<PolyBezierLayer>(null);
            }
        }

        public async Task<Geometry> Edit(Geometry geometry)
        {
            this.IsEditMode = true;

            Geometry result = null;

            if (this.RequestEdit != null)
            {
                result = await RequestEdit(geometry);
            }
            else
            {
                result = await new Task<Geometry>(null);
            }

            this.IsEditMode = false;

            return result;
        }

        public Task<Geometry> Edit(List<Ham.SpatialBase.Point> points, bool isClosed)
        {
            if (points == null || points.Count < 1)
            {
                return new Task<Geometry>(null);
            }

            var type = points.Count == 1 ? GeometryType.Point : (isClosed ? GeometryType.Polygon : GeometryType.LineString);

            Geometry geometry = new Geometry(points.ToArray(), type);

            return Edit(geometry);
        }

        public void FireMapStatusChanged(MapStatus status)
        {
            this.MapStatus = status;
        }

        public void FireMapActionChanged(MapAction action)
        {
            this.MapAction = action;
        }

        public void FireExtentChanged(Ham.SpatialBase.BoundingBox currentExtent)
        {
            this.RaisePropertyChanged(nameof(CurrentExtent));

            this.OnExtentChanged?.Invoke(null, EventArgs.Empty);
        }

        public void FireMouseMove(Point currentPoint)
        {
            this.OnMouseMove?.Invoke(this, currentPoint);
        }

        public void FireMapMouseUp(Point currentPoint)
        {
            this.OnMapMouseUp?.Invoke(this, currentPoint);
        }

        public void FireZoomChanged(double mapScale)
        {
            this.RaisePropertyChanged(nameof(this.CurrentZoomLevel));

            this.OnZoomChanged?.Invoke(this, mapScale);
        }

        protected void RemoveMapOptions()
        {
            this.RequestRemoveMapOptions?.Invoke();
        }

        public ObservableCollection<System.Data.DataTable> Identify(IRI.Ham.SpatialBase.Point arg)
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

        public Task<IRI.Ham.SpatialBase.Point> GetPoint()
        {
            if (RequestGetPoint != null)
            {
                return RequestGetPoint();
            }
            else
            {
                return new Task<Ham.SpatialBase.Point>(() => Ham.SpatialBase.Point.NaN);
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

        private RelayCommand _clearMapCommand;

        public RelayCommand ClearMapCommand

        {
            get
            {
                if (_clearMapCommand == null)
                {
                    _clearMapCommand = new RelayCommand(param =>
                    {
                        ClearMap();

                        this.RequestClearMap?.Invoke();
                    });
                }

                return _clearMapCommand;
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


        private RelayCommand _cancelNewDrawingCommand;

        public RelayCommand CancelNewDrawingCommand
        {
            get
            {
                if (_cancelNewDrawingCommand == null)
                {
                    _cancelNewDrawingCommand = new RelayCommand(param => this.CancelGetDrawing());
                }
                return _cancelNewDrawingCommand;
            }
        }

        private RelayCommand _cancelEditDrawingCommand;

        public RelayCommand CancelEditDrawingCommand
        {
            get
            {
                if (_cancelEditDrawingCommand == null)
                {
                    _cancelEditDrawingCommand = new RelayCommand(param =>
                    {
                        this.IsEditMode = false;

                        this.CancelEditGeometry();
                    });
                }
                return _cancelEditDrawingCommand;
            }
        }

        private RelayCommand _finishEditDrawingCommand;

        public RelayCommand FinishEditDrawingCommand
        {
            get
            {
                if (_finishEditDrawingCommand == null)
                {
                    _finishEditDrawingCommand = new RelayCommand(param =>
                    {
                        this.FinishEditGeometry();
                    });
                }
                return _finishEditDrawingCommand;
            }
        }


        #endregion



        #region Events

        public event EventHandler<Point> OnMouseMove;

        public event EventHandler<double> OnZoomChanged;

        public event EventHandler<Point> OnMapMouseUp;

        public event EventHandler OnExtentChanged;

        public event EventHandler OnCancelEditGeometry;

        public event EventHandler OnFinishEditGeometry;

        public event EventHandler OnCancelDrawGeometry;

        #endregion
    }
}
