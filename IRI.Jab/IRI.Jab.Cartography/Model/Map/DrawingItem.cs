using IRI.Ham.SpatialBase.Primitives;
using IRI.Jab.Common;
using IRI.Jab.Common.Assets.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Jab.Cartography.Model.Map
{
    public class DrawingItem : IRI.Jab.Common.Model.EditCommandAwareItem
    {
        public DrawingItem()
        {
            this._id = Guid.NewGuid().ToString();
        }

        private string _id;

        public string Id
        {
            get { return _id; }
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

        public ILayer AssociatedLayer { get; set; }

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



        public Action<DrawingItem> RequestZoomToGeometry;

        public Action<DrawingItem> RequestDownload;

         

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

        private RelayCommand _downloadCommand;

        public RelayCommand DownloadCommand
        {
            get
            {
                if (_downloadCommand == null)
                {
                    _downloadCommand = new RelayCommand(param => this.RequestDownload?.Invoke(this));
                }

                return _downloadCommand;
            }
        }

        #endregion
    }
}
