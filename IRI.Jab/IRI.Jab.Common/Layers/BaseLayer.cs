using System;
using System.Collections.Generic;
using IRI.Msh.Common.Primitives;
using IRI.Jab.Common.Model;
using System.Windows;
using IRI.Jab.Common.Model.Legend;
using IRI.Jab.Common.Assets.Commands;
using System.Collections.ObjectModel;

namespace IRI.Jab.Common
{
    public abstract class BaseLayer : Notifier, ILayer
    {
        public BaseLayer()
        {
            this.LayerId = Guid.NewGuid();

            this.VisibleRange = ScaleInterval.All;

            this.VisualParameters = VisualParameters.CreateNew(1);

            this.ParentLayerId = Guid.Empty;
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

        public Guid LayerId { get; protected set; }

        private bool _isGroupLayer;

        public bool IsGroupLayer
        {
            get { return _isGroupLayer; }
            set
            {
                _isGroupLayer = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(ShowOptions));
            }
        }


        public Guid ParentLayerId { get; set; }

        public ObservableCollection<ILayer> SubLayers { get; set; }

        public bool IsValid { get; set; } = true;

        public int ZIndex { get; set; }

        private bool _isSelectedInToc;

        public bool IsSelectedInToc
        {
            get { return _isSelectedInToc; }
            set
            {
                if (_isSelectedInToc == value)
                {
                    return;
                }

                _isSelectedInToc = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(ShowOptions));
                //ChangeSymbologyCommand?.CanExecute(null);

                OnIsSelectedInTocChanged?.Invoke(this, new CustomEventArgs<BaseLayer>(this));
            }
        }

        public bool ShowOptions
        {
            get { return IsSelectedInToc && Commands?.Count > 0 && !IsGroupLayer; }
        }

        private bool _showInToc = true;

        public bool ShowInToc
        {
            get { return _showInToc; }
            set
            {
                _showInToc = value;
                RaisePropertyChanged();
            }
        }

        private bool _canUserDelete = true;

        public bool CanUserDelete
        {
            get { return Type != LayerType.BaseMap && _canUserDelete; }
            set
            {
                _canUserDelete = value;
                RaisePropertyChanged();
            }
        }

        private int _numberOfSelectedFeatures;

        public bool HasSelectedFeatures
        {
            get { return NumberOfSelectedFeatures > 0; }
        }

        public int NumberOfSelectedFeatures
        {
            get { return _numberOfSelectedFeatures; }
            set
            {
                _numberOfSelectedFeatures = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(HasSelectedFeatures));

                this.OnSelectedFeaturesChanged?.Invoke(this, new CustomEventArgs<BaseLayer>(this));
            }
        }


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

        private List<ILegendCommand> _commands;

        public List<ILegendCommand> Commands
        {
            get { return _commands; }
            set
            {
                _commands = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(ShowOptions));
            }
        }

        private RelayCommand _changeSymbologyCommand;

        public RelayCommand ChangeSymbologyCommand
        {
            get
            {
                if (_changeSymbologyCommand == null)
                {
                    //_changeSymbologyCommand = new RelayCommand(param => { this.RequestChangeSymbology?.Invoke(this); }, param => IsSelectedInToc);
                    _changeSymbologyCommand = new RelayCommand(param => { this.RequestChangeSymbology?.Invoke(this); });
                }

                return _changeSymbologyCommand;
            }
        }


        private LabelParameters _labels;

        public LabelParameters Labels
        {
            get { return _labels; }
            set
            {
                _labels = value;
                RaisePropertyChanged();

                this.OnLabelChanged?.Invoke(this, new CustomEventArgs<LabelParameters>(value));
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

        public Action<ILayer> RequestChangeSymbology;

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

        public event EventHandler<CustomEventArgs<BaseLayer>> OnSelectedFeaturesChanged;

        private void RaiseVisibilityChanged(object sender, CustomEventArgs<Visibility> e)
        {
            this._onVisibilityChanged.SafeInvoke(this, e);
        }

        public event EventHandler<CustomEventArgs<LabelParameters>> OnLabelChanged;

        public event EventHandler<CustomEventArgs<BaseLayer>> OnIsSelectedInTocChanged;
    }
}
