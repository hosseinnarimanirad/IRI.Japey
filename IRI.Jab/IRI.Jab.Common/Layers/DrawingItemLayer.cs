using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRI.Jab.Common.Assets.Commands;
using IRI.Jab.Common.Model;
using IRI.Jab.Common.Model.Legend;
using IRI.Jab.Common.Model.Symbology;
using IRI.Ket.DataManagement.DataSource;
using IRI.Ket.DataManagement.Model;
using IRI.Ket.SpatialExtensions;
using IRI.Msh.Common.Primitives;
using Microsoft.SqlServer.Types;

namespace IRI.Jab.Common
{
    public class DrawingItemLayer : VectorLayer, IIdentifiable
    {
        private const string _removeToolTip = "حذف";

        private const string _editToolTip = "ویرایش";

        private const string _zoomToolTip = "بزرگ‌نمایی";

        private const string _saveToolTip = "ذخیره‌سازی";

        public int Id { get; set; }

        public FeatureDataSource OriginalSource { get; set; }

        public VisualParameters OriginalSymbology { get; set; }

        private Geometry _shape;

        public Geometry Geometry
        {
            get { return _shape; }
            set
            {
                _shape = value;
                RaisePropertyChanged();

                this.DataSource = new MemoryDataSource(new List<SqlGeometry>() { value?.AsSqlGeometry() });
            }
        }

        //private ILayer _associatedLayer;

        //public ILayer AssociatedLayer
        //{
        //    get { return _associatedLayer; }
        //    set
        //    {
        //        _associatedLayer = value;
        //        RaisePropertyChanged();
        //    }
        //}


        //public VisualParameters VisualParameters
        //{
        //    get { return AssociatedLayer?.VisualParameters; }
        //    set
        //    {
        //        if (AssociatedLayer != null)
        //        {
        //            AssociatedLayer.VisualParameters = value;
        //        }

        //        RaisePropertyChanged();
        //    }
        //}


        private string _title;

        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                RaisePropertyChanged();
            }
        }

        //private bool _isSelected;

        //public bool IsSelected
        //{
        //    get { return _isSelected; }
        //    set
        //    {
        //        if (_isSelected == value)
        //        {
        //            return;
        //        }

        //        _isSelected = value;
        //        RaisePropertyChanged();

        //        this.RequestHighlightGeometry?.Invoke(this);
        //    }
        //}

        //public Action<DrawingItemLayer> RequestZoomToGeometry;

        public Action<DrawingItemLayer> RequestHighlightGeometry;

        //public Action<DrawingItemLayer> RequestExportAsShapefile;

        //public Action<DrawingItemLayer> RequestChangeSymbology;

        //private ObservableCollection<ILegendCommand> _commands;

        //public ObservableCollection<ILegendCommand> Commands
        //{
        //    get { return _commands; }
        //    set
        //    {
        //        _commands = value;
        //        RaisePropertyChanged();
        //    }
        //}

        public DrawingItemLayer(string title, Geometry geometry, int id = int.MinValue, FeatureDataSource source = null)
        {
            //this._id = Guid.NewGuid().ToString();

            this.Extent = geometry.GetBoundingBox();

            this.VisualParameters = VisualParameters.GetRandomVisualParameters();

            this.OriginalSymbology = VisualParameters.Clone();

            var featureType =
                (geometry.Type == GeometryType.Point || geometry.Type == GeometryType.MultiPoint) ? LayerType.Point :
                ((geometry.Type == GeometryType.LineString || geometry.Type == GeometryType.MultiLineString) ? LayerType.Polyline :
                (geometry.Type == GeometryType.Polygon || geometry.Type == GeometryType.MultiPolygon) ? LayerType.Polygon : LayerType.None);

            this.Type = LayerType.Drawing | featureType;

            this.Rendering = RenderingApproach.Default;

            this.ToRasterTechnique = RasterizationApproach.DrawingVisual;

            this.Id = id;

            this.OriginalSource = source;

            //this.DataSource = new MemoryDataSource(new List<SqlGeometry>() { geometry.AsSqlGeometry() });

            this.Title = title;

            this.Geometry = geometry;

            this.Commands = new List<ILegendCommand>();

            this.OnIsSelectedInTocChanged += (sender, e) =>
            {
                this.RequestHighlightGeometry?.Invoke(this);
            };

            //Commands.Add(new LegendCommand() { Command = RemoveCommand, Layer = AssociatedLayer, PathMarkup = Appbar.appbarDelete, ToolTip = _removeToolTip });

            //Commands.Add(new LegendCommand() { Command = EditCommand, Layer = AssociatedLayer, PathMarkup = Appbar.appbarEdit, ToolTip = _editToolTip });

            //Commands.Add(new LegendCommand() { Command = ExportAsShapefileCommand, Layer = AssociatedLayer, PathMarkup = Appbar.appbarSave, ToolTip = _saveToolTip });

            //Commands.Add(new LegendCommand() { Command = ZoomCommand, Layer = AssociatedLayer, PathMarkup = Appbar.appbarMagnify, ToolTip = _zoomToolTip });

        }



        #region Commands

        //private RelayCommand _zoomCommand;

        //public RelayCommand ZoomCommand
        //{
        //    get
        //    {
        //        if (_zoomCommand == null)
        //        {
        //            _zoomCommand = new RelayCommand(param => this.RequestZoomToGeometry?.Invoke(this));
        //        }

        //        return _zoomCommand;
        //    }
        //}


        //private RelayCommand _highlightCommand;

        //public RelayCommand HighlightCommand
        //{
        //    get
        //    {
        //        if (_highlightCommand == null)
        //        {
        //            _highlightCommand = new RelayCommand(param =>
        //            {
        //                this.RequestHighlightGeometry?.Invoke(this);
        //            });
        //        }

        //        return _highlightCommand;
        //    }
        //}


        //private RelayCommand _exportAsShapefileCommand;

        //public RelayCommand ExportAsShapefileCommand
        //{
        //    get
        //    {
        //        if (_exportAsShapefileCommand == null)
        //        {
        //            _exportAsShapefileCommand = new RelayCommand(param =>
        //            {
        //                this.RequestExportAsShapefile?.Invoke(this);
        //            });
        //        }

        //        return _exportAsShapefileCommand;
        //    }
        //}


        //private RelayCommand _changeSymbologyCommand;

        //public RelayCommand ChangeSymbologyCommand
        //{
        //    get
        //    {
        //        if (_changeSymbologyCommand == null)
        //        {
        //            _changeSymbologyCommand = new RelayCommand(param =>
        //            {
        //                this.RequestChangeSymbology?.Invoke(this);
        //            });
        //        }

        //        return _changeSymbologyCommand;
        //    }
        //}



        #endregion


    }
}
