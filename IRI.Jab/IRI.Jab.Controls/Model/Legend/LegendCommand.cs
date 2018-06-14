using IRI.Jab.Cartography;
using IRI.Jab.Cartography.Presenter.Map;
using IRI.Jab.Common;
using IRI.Jab.Common.Assets.Commands;
using IRI.Ket.SqlServerSpatialExtension.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Jab.Controls.Model.Legend
{
    public class LegendCommand : Notifier, ILegendCommand
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

        public static LegendCommand CreateZoomToExtentCommand(MapPresenter map, ILayer layer)
        {
            var result = new LegendCommand()
            {
                PathMarkup = IRI.Jab.Common.Assets.ShapeStrings.Appbar.appbarMagnify,
                Layer = layer
            };

            result.Command = new RelayCommand((param) =>
            {
                if (layer == null || map == null)
                    return;

                map.ZoomToExtent(result.Layer.Extent);
            });

            return result;
        }

        public static LegendCommand CreateShowAttributeTable(MapPresenter map, ILayer layer)
        {
            var result = new LegendCommand()
            {
                PathMarkup = IRI.Jab.Common.Assets.ShapeStrings.Appbar.appbarPageText,
                Layer = layer
            };

            result.Command = new RelayCommand((param) =>
            {
                if (layer == null || map == null)
                    return;

                var dataTable = (layer as VectorLayer).GetEntireFeature();

                var list = new List<SqlIndex250k>() {
                    new SqlIndex250k(new Msh.Common.Mapping.Index250k()),
                    new SqlIndex250k(new Msh.Common.Mapping.Index250k()),
                    new SqlIndex250k(new Msh.Common.Mapping.Index250k()),
                    new SqlIndex250k(new Msh.Common.Mapping.Index250k()),
                    new SqlIndex250k(new Msh.Common.Mapping.Index250k())};

                //map.SelectedLayers.Add(new Cartography.Model.Map.SelectedLayer() { LayerName = layer.LayerName, Features = dataTable });
                map.SelectedLayers.Add(new Cartography.Model.Map.SelectedLayer() { LayerName = layer.LayerName, Features = list });
            });

            return result;
        }

        public static LegendCommand Create(ILayer layer, Action action, string markup)
        {
            var result = new LegendCommand()
            {
                PathMarkup = markup,
                Command = new RelayCommand(param => action())
            };

            result.Command = new RelayCommand(param => action());

            return result;
        }
    }
}
