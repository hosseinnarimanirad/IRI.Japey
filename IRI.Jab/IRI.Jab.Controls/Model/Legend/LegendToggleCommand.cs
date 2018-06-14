﻿using IRI.Jab.Cartography;
using IRI.Jab.Cartography.Presenter.Map;
using IRI.Jab.Common;
using IRI.Jab.Common.Assets.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Jab.Controls.Model.Legend
{
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

        private bool _isSelected;

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                RaisePropertyChanged();

                Command.Execute(value);
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

        public ILayer Layer { get; set; }



        public static LegendToggleCommand CreateToggleLayerLabelCommand(MapPresenter map, ILayer layer, LabelParameters labels)
        {
            LegendToggleCommand result = new LegendToggleCommand();

            result.PathMarkup = IRI.Jab.Common.Assets.ShapeStrings.Appbar.appbarTextSerif;

            result.Layer = layer;

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

            layer.Labels = labels;

            result.Command = new RelayCommand(param =>
            {
                if (layer == null)
                    return;

                if (result.IsSelected)
                {
                    result.Layer.Labels.IsOn = true;

                    map.Refresh();
                }
                else
                {
                    result.Layer.Labels.IsOn = false;

                    map.Refresh();
                }
            });

            return result;
        }



    }
}
