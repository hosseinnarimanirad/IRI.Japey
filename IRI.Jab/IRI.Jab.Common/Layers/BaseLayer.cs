using System;
using System.Windows;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using IRI.Extensions;
using IRI.Jab.Common.Model;
using IRI.Sta.Common.Primitives;
using IRI.Jab.Common.Model.Legend;
using IRI.Jab.Common.Assets.Commands;
using IRI.Jab.Common.Enums;

namespace IRI.Jab.Common;

public abstract class BaseLayer : Notifier, ILayer
{
    public BaseLayer()
    {
        this.LayerId = Guid.NewGuid();

        this.VisibleRange = ScaleInterval.All;

        this.VisualParameters = VisualParameters.CreateNew(1);

        this.ParentLayerId = Guid.Empty;
    }

    public int AuxilaryId { get; set; }

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

    // use for identify tool
    public bool IsSearchable { get; set; } = false;

    private bool _isSelectedInToc;

    public bool IsSelectedInToc
    {
        get { return _isSelectedInToc; }
        set
        {
            if (value && _isSelectedInToc == value)
            {
                value = false;
            }

            _isSelectedInToc = value;
            RaisePropertyChanged();
            RaisePropertyChanged(nameof(ShowOptions));
            //ChangeSymbologyCommand?.CanExecute(null);

            OnIsSelectedInTocChanged?.Invoke(this, new CustomEventArgs<BaseLayer>(this));
        }
    }

    private bool _isExpandedInToc;

    public bool IsExpandedInToc
    {
        get { return _isExpandedInToc; }
        set
        {
            if (value && _isExpandedInToc == value)
            {
                value = false;
            }

            _isExpandedInToc = value;
            RaisePropertyChanged();
            //RaisePropertyChanged(nameof(ShowOptions));
            //ChangeSymbologyCommand?.CanExecute(null);

            //OnIsSelectedInTocChanged?.Invoke(this, new CustomEventArgs<BaseLayer>(this));

            //if (this.IsGroupLayer && !this.SubLayers.IsNullOrEmpty())
            //{
            //    foreach (var subLayer in SubLayers)
            //    {
            //        subLayer.IsExpandedInToc = value;
            //    }
            //}
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

    private List<ILegendCommand>? _commands;
    public List<ILegendCommand>? Commands
    {
        get { return _commands; }
        set
        {
            _commands = value;
            RaisePropertyChanged();
            RaisePropertyChanged(nameof(ShowOptions));
        }
    }


    public void TurnOff()
    {
        SetVisibility(Visibility.Collapsed);
    }

    public void TurnOn()
    {
        SetVisibility(Visibility.Visible);
    }

    public void SetVisibility(Visibility visibility)
    {
        this.VisualParameters.Visibility = visibility;

        if (!SubLayers.IsNullOrEmpty())
        {
            foreach (var item in SubLayers)
            {
                item.SetVisibility(visibility);
            }
        }
    }

    public void ToggleVisibility()
    {
        if (this.VisualParameters.Visibility == Visibility.Visible)
        {
            TurnOff();
        }
        else
        {
            TurnOn();
        }
    }

    public bool CanRenderLayer(double mapScale)
    {
        return this.VisualParameters?.Visibility == Visibility.Visible && this.VisibleRange.IsInRange(1.0 / mapScale);
    }

    public bool CanRenderLabels(double mapScale)
    {
        return this.Labels?.IsLabeled(1.0 / mapScale) == true;
    }

    private List<IFeatureTableCommand> _featureTableCommands;
    public List<IFeatureTableCommand> FeatureTableCommands
    {
        get { return _featureTableCommands; }
        set
        {
            _featureTableCommands = value;
            RaisePropertyChanged();
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



    private RelayCommand _toggleExpandCommand;
    public RelayCommand ToggleExpandCommand
    {
        get
        {
            if (_toggleExpandCommand == null)
            {
                //_changeSymbologyCommand = new RelayCommand(param => { this.RequestChangeSymbology?.Invoke(this); }, param => IsSelectedInToc);
                _toggleExpandCommand = new RelayCommand(param => { this.IsExpandedInToc = !this.IsExpandedInToc; });
            }

            return _toggleExpandCommand;
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

            this._onLayerNameChanged?.Invoke(this, new CustomEventArgs<string>(value));
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


    private event EventHandler<CustomEventArgs<string>> _onLayerNameChanged;

    public event EventHandler<CustomEventArgs<string>> OnLayerNameChanged
    {
        remove { this._onLayerNameChanged -= value; }
        add
        {
            if (this._onLayerNameChanged == null)
            {
                this._onLayerNameChanged += value;
            }
        }
    }

    private void RaiseVisibilityChanged(object sender, CustomEventArgs<Visibility> e)
    {
        this._onVisibilityChanged.SafeInvoke(this, e);
    }

    public event EventHandler<CustomEventArgs<LabelParameters>> OnLabelChanged;

    public event EventHandler<CustomEventArgs<BaseLayer>> OnIsSelectedInTocChanged;
}
