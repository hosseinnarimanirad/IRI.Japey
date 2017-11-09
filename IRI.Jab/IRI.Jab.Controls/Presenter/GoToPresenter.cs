using IRI.Jab.Common;
using IRI.Jab.Common.Assets.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Jab.Controls.Presenter
{
    public class GoToPresenter : Notifier
    {
        private double _x;

        public double X
        {
            get { return _x; }
            set
            {
                _x = value;
                RaisePropertyChanged();
            }
        }

        private double _y;

        public double Y
        {
            get { return _y; }
            set
            {
                _y = value;
                RaisePropertyChanged();
            }
        }

        private Model.DegreeMinuteSecondModel _longitudeDms;

        public Model.DegreeMinuteSecondModel LongitudeDms
        {
            get { return _longitudeDms; }
            set
            {
                _longitudeDms = value;
                RaisePropertyChanged();
            }
        }

        private Model.DegreeMinuteSecondModel _latitudeDms;

        public Model.DegreeMinuteSecondModel LatitudeDms
        {
            get { return _latitudeDms; }
            set
            {
                _latitudeDms = value;
                RaisePropertyChanged();
            }
        }


        private RelayCommand _goToCommand;

        public RelayCommand GoToCommand
        {
            get
            {
                if (_goToCommand == null)
                {
                    _goToCommand = new RelayCommand(param => { this.GoTo(); });
                }

                return _goToCommand;
            }
        }

        public void GoTo()
        {
            this.RequestGoTo?.Invoke(new IRI.Ham.SpatialBase.Point(X, Y));
        }

        public Action<IRI.Ham.SpatialBase.Point> RequestGoTo;

        public GoToPresenter(Action<IRI.Ham.SpatialBase.Point> requestGoto)
        {
            this.RequestGoTo = requestGoto;
        }
    }
}
