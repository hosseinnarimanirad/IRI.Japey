using IRI.Jab.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRI.Ham.SpatialBase;
using IRI.Jab.Cartography.Model;
using System.Windows;
using IRI.Jab.Common.Model;

namespace IRI.Jab.Cartography
{
    public abstract class BaseLayer : Notifier, ILayer
    {
        public BaseLayer()
        {
            this.Id = Guid.NewGuid();

            this.VisibleRange = ScaleInterval.All;

            this.VisualParameters = VisualParameters.CreateNew(1);
        }

        public abstract LayerType Type { get; protected set; }

        public abstract BoundingBox Extent { get; protected set; }

        public abstract RenderingApproach Rendering { get; protected set; }

        public virtual RasterizationApproach ToRasterTechnique
        {
            get { return RasterizationApproach.None; }

            protected set { }
        }

        public virtual void Invalidate() => IsValid = false;

        public Guid Id { get; protected set; }

        public bool IsValid { get; set; }

        public int ZIndex { get; set; }


        private ScaleInterval _visibleRange;

        public ScaleInterval VisibleRange
        {
            get { return _visibleRange; }
            set
            {
                _visibleRange = value;
                RaisePropertyChanged();
            }
        }

        private string _layerName;

        public string LayerName
        {
            get { return _layerName; }
            set
            {
                _layerName = value;
                RaisePropertyChanged();
            }
        }


        private VisualParameters _visualParameters;

        public VisualParameters VisualParameters
        {
            get { return _visualParameters; }
            set
            {
                _visualParameters = value;

                RaisePropertyChanged();

                if (_visualParameters != null)
                {
                    _visualParameters.OnVisibilityChanged -= RaiseVisibilityChanged;
                    _visualParameters.OnVisibilityChanged += RaiseVisibilityChanged;
                }

            }
        }


        //public event EventHandler<CustomEventArgs<bool>> OnVisibilityChanged;
        private event EventHandler<CustomEventArgs<Visibility>> _onVisibilityChanged;

        public event EventHandler<CustomEventArgs<Visibility>> OnVisibilityChanged
        {
            remove { this._onVisibilityChanged -= value; }
            add
            {
                if (this._onVisibilityChanged == null)
                {
                    this._onVisibilityChanged += value;
                }
            }
        }

        private void RaiseVisibilityChanged(object sender, CustomEventArgs<Visibility> e)
        {
            this._onVisibilityChanged.SafeInvoke(sender, e);
        }
    }
}
