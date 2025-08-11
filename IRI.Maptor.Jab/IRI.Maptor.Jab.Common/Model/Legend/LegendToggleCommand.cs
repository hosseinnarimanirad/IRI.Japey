using System;

using IRI.Maptor.Jab.Common.Presenter.Map;
using IRI.Maptor.Jab.Common.Assets.Commands;
using IRI.Maptor.Jab.Common.Localization;
using MahApps.Metro.IconPacks;

namespace IRI.Maptor.Jab.Common.Model.Legend;

public class LegendToggleCommand : Notifier, ILegendCommand
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

    private string _notCheckedPathMarkup;
    public string NotCheckedPathMarkup
    {
        get { return _notCheckedPathMarkup; }
        set
        {
            _notCheckedPathMarkup = value;
            RaisePropertyChanged();
        }
    }


    private bool _isSelected;
    public bool IsSelected
    {
        get { return _isSelected; }
        set
        {
            _isSelected = value;
            RaisePropertyChanged();

            Command?.Execute(value);
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

    //private string _toolTip;
    //public string ToolTip
    //{
    //    get { return _toolTip; }
    //    set
    //    {
    //        _toolTip = value;
    //        RaisePropertyChanged();
    //    }
    //}

    private string ToolTipResourceKey { get; set; }
    public string ToolTip => LocalizationManager.Instance[ToolTipResourceKey];


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


    public LegendToggleCommand()
    {
        Localization.LocalizationManager.Instance.LanguageChanged += Instance_LanguageChanged;
    }

    private void Instance_LanguageChanged()
    {
        RaisePropertyChanged(nameof(ToolTip));
    }

    public static LegendToggleCommand CreateToggleLayerLabelCommand(MapPresenter map, ILayer layer/*, LabelParameters labels*/)
    {
        LegendToggleCommand result = new LegendToggleCommand();

        result.PathMarkup = new PackIconModern() { Kind = PackIconModernKind.TextSerif }.Data;// IRI.Maptor.Jab.Common.Assets.ShapeStrings.Appbar.appbarTextSerif;
        result.NotCheckedPathMarkup = IRI.Maptor.Jab.Common.Assets.ShapeStrings.AppbarExtension.appbarTextSerifNone;
        //result.ToolTip = "نمایش برچسب عوارض";
        result.ToolTipResourceKey = LocalizationResourceKeys.cmd_legend_toggleLayerLabel.ToString();
        result.Layer = layer;
        result.IsSelected = layer.Labels?.IsOn == true;

        EventHandler<CustomEventArgs<LabelParameters>> labels_IsInScaleRangeChanged = (sender, e) =>
        {
            if (e.Arg != null)
            {
                result.IsEnabled = e.Arg.IsInScaleRange;
            }
        };

        EventHandler<CustomEventArgs<LabelParameters>> layer_OnLabelChanged = (sender, e) =>
        {
            if (e.Arg != null)
            {
                e.Arg.OnIsInScaleRangeChanged -= labels_IsInScaleRangeChanged;
                e.Arg.OnIsInScaleRangeChanged += labels_IsInScaleRangeChanged;
            }
        };

        layer.OnLabelChanged -= layer_OnLabelChanged;
        layer.OnLabelChanged += layer_OnLabelChanged;

        //layer.Labels = labels;

        result.Command = new RelayCommand(param =>
        {
            if (layer == null)
                return;

            if (result.IsSelected)
            {
                result.Layer.Labels.IsOn = true;

                //map.Refresh();
            }
            else
            {
                result.Layer.Labels.IsOn = false;

                //map.Refresh();
            }

            map.RefreshLayerVisibility(result.Layer);
        });

        return result;
    }



}
