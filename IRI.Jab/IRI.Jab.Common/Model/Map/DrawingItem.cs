using IRI.Msh.Common.Primitives;
using IRI.Jab.Common;
using IRI.Jab.Common.Assets.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRI.Jab.Common.Model.Legend;
using System.Collections.ObjectModel;
using IRI.Jab.Common.Assets.ShapeStrings;

namespace IRI.Jab.Common.Model.Map
{
    public class DrawingItem : IRI.Jab.Common.Model.EditCommandAwareItem
    {
        private const string _removeToolTip = "حذف";

        private const string _editToolTip = "ویرایش";

        private const string _zoomToolTip = "بزرگ‌نمایی";

        private string _id;

        public string Id
        {
            get { return AssociatedLayer?.Id.ToString() ?? Guid.NewGuid().ToString(); }
        }


        private Geometry _shape;

        public Geometry Geometry
        {
            get { return _shape; }
            set
            {
                _shape = value;
                RaisePropertyChanged();
            }
        }

        private ILayer _associatedLayer;

        public ILayer AssociatedLayer
        {
            get { return _associatedLayer; }
            set
            {
                _associatedLayer = value;
                RaisePropertyChanged();
            }
        }

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

        private bool _isSelected;

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (_isSelected == value)
                {
                    return;
                }

                _isSelected = value;
                RaisePropertyChanged();

                this.RequestHighlightGeometry?.Invoke(this);
            }
        }

        public Action<DrawingItem> RequestZoomToGeometry;

        public Action<DrawingItem> RequestHighlightGeometry;


        //public Action<DrawingItem> RequestDownload;

        private ObservableCollection<ILegendCommand> _commands;

        public ObservableCollection<ILegendCommand> Commands
        {
            get { return _commands; }
            set
            {
                _commands = value;
                RaisePropertyChanged();
            }
        }

        public DrawingItem(string title, Geometry geometry)
        {
            //this._id = Guid.NewGuid().ToString();

            this.Title = title;

            this.Geometry = geometry;

            this.Commands = new ObservableCollection<ILegendCommand>();

            //RequestRemoveAction = removeAction;

            //RequestEditAction = editAction;

            //RequestZoomToGeometry = zoomToAction;

            Commands.Add(new LegendCommand() { Command = RemoveCommand, Layer = AssociatedLayer, PathMarkup = Appbar.appbarDelete, ToolTip = _removeToolTip });

            Commands.Add(new LegendCommand() { Command = EditCommand, Layer = AssociatedLayer, PathMarkup = Appbar.appbarEdit, ToolTip = _editToolTip });

            Commands.Add(new LegendCommand() { Command = ZoomCommand, Layer = AssociatedLayer, PathMarkup = Appbar.appbarMagnify, ToolTip = _zoomToolTip });

            //shapeItem.RequestZoomToGeometry = (g) => { this.ZoomToExtent(g.Geometry.GetBoundingBox(), false); };

            //shapeItem.RequestDownload = (s) =>
            //{
            //    //this.OnRequestShowDownloadDialog?.Invoke(s);
            //};

            //shapeItem.AssociatedLayer = new VectorLayer(shapeItem.Title, new List<SqlGeometry>() { drawing.AsSqlGeometry() }, VisualParameters.GetRandomVisualParameters(), LayerType.Drawing, RenderingApproach.Default, RasterizationApproach.DrawingVisual);

        }



        #region Commands

        private RelayCommand _zoomCommand;

        public RelayCommand ZoomCommand
        {
            get
            {
                if (_zoomCommand == null)
                {
                    _zoomCommand = new RelayCommand(param => this.RequestZoomToGeometry?.Invoke(this));
                }

                return _zoomCommand;
            }
        }

        private RelayCommand _highlightCommand;

        public RelayCommand HighlightCommand
        {
            get
            {
                if (_highlightCommand == null)
                {
                    _highlightCommand = new RelayCommand(param =>
                    {
                        this.RequestHighlightGeometry?.Invoke(this);
                    });
                }

                return _highlightCommand;
            }
        }


        #endregion
    }
}
