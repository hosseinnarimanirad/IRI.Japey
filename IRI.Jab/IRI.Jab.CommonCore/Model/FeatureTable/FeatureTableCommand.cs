using IRI.Jab.Common.Assets.Commands;
using IRI.Jab.Common.Model.Map;
using IRI.Jab.Common.Presenter.Map;
using IRI.Ket.SpatialExtensions;
using IRI.Ket.SqlServerSpatialExtension.Model;
using IRI.Msh.Common.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Jab.Common.Model
{
    public class FeatureTableCommand : Notifier, IFeatureTableCommand
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


        private string _toolTip;
        public string ToolTip
        {
            get { return _toolTip; }
            set
            {
                _toolTip = value;
                RaisePropertyChanged();
            }
        }


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


        //public ILayer Layer { get; set; }


      
    }
     

}
