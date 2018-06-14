using IRI.Jab.Common;
using IRI.Msh.Common.Primitives;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Jab.Cartography.Model.Map
{
    public class SelectedLayer : Notifier
    {
        public string LayerName { get; set; }

        //private ObservableCollection<IGeometryAware> _features;

        //public ObservableCollection<IGeometryAware> Features
        //{
        //    get { return _features; }
        //    set
        //    {
        //        _features = value;
        //        RaisePropertyChanged();
        //    }
        //}


        private IEnumerable<object> _features;

        public IEnumerable<object> Features
        {
            get { return _features; }
            set
            {
                _features = value;
                RaisePropertyChanged();
            }
        }



        private IEnumerable<IGeometryAware> _highlightedFeatures;

        public IEnumerable<IGeometryAware> HighlightedFeatures
        {
            get { return _highlightedFeatures; }
            set
            {
                _highlightedFeatures = value;
                RaisePropertyChanged();
            }
        }


    }
}
