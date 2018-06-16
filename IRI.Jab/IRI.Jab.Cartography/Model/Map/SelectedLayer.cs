using IRI.Jab.Common;
using IRI.Ket.SqlServerSpatialExtension.Model;
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
    public class SelectedLayer<T> : Notifier, ISelectedLayer where T : ISqlGeometryAware
    {
        public string LayerName { get; set; }

        private ObservableCollection<T> _features;

        public ObservableCollection<T> Features
        {
            get { return _features; }
            set
            {
                _features = value;
                RaisePropertyChanged();
            }
        }


        //private DataTable _features;

        //public DataTable Features
        //{
        //    get { return _features; }
        //    set
        //    {
        //        _features = value;
        //        RaisePropertyChanged();
        //    }
        //}



        private IEnumerable<T> _highlightedFeatures;

        public IEnumerable<T> HighlightedFeatures
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
