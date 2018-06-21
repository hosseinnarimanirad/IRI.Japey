//using IRI.Jab.Common;
//using IRI.Jab.Common.Presenter.Map;
//using IRI.Jab.Common;
//using IRI.Jab.Common.Assets.Commands;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace IRI.Jab.Common.Model.Legend
//{
//    public class MapLegendItemWithOptionsModel : Notifier
//    {
//        ILayer _layer;

//        public ILayer Layer { get { return _layer; } }

//        public MapLegendItemWithOptionsModel(ILayer layer)
//        {
//            this._layer = layer;
//            RaisePropertyChanged(nameof(LayerName));
//            RaisePropertyChanged(nameof(Symbology));


//        }

//        public Guid Id { get { return _layer.Id; } }

//        public string LayerName
//        {
//            get { return _layer.LayerName; }
//        }

//        public VisualParameters Symbology
//        {
//            get { return _layer.VisualParameters; }
//        }

//        public LabelParameters Label
//        {
//            get { return _layer.Labels; }
//        }

//        private List<ILegendCommand> _commands;

//        public List<ILegendCommand> Commands
//        {
//            get { return _commands; }
//            set
//            {
//                _commands = value;
//                RaisePropertyChanged();
//            }
//        }

//        public ILayer GetLayer()
//        {
//            return _layer;
//        }

//    }
//}
