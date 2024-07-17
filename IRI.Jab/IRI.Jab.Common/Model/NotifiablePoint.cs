using IRI.Msh.Common.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Jab.Common.Model
{
    public class NotifiablePoint : Notifier, IPoint
    {
        private double _x;

        public double X
        {
            get { return _x; }
            set
            {
                _x = value;
                RaisePropertyChanged();

                if (IsRaiseCoordinateChangeEnabled)
                {
                    RaiseCoordinateChangedAction?.Invoke(this);
                }

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

                if (IsRaiseCoordinateChangeEnabled)
                {
                    RaiseCoordinateChangedAction?.Invoke(this);
                }

            }
        }

        public NotifiablePoint()
        {

        }

        public NotifiablePoint(double x, double y, Action<NotifiablePoint> changeAction)
        {
            this.X = x;

            this.Y = y;

            this.RaiseCoordinateChangedAction = changeAction;
        }

        public bool AreExactlyTheSame(object obj)
        {
            throw new NotImplementedException();
        }

        public double DistanceTo(IPoint point)
        {
            throw new NotImplementedException();
        }

        public byte[] AsWkb()
        {
            throw new NotImplementedException();
        }

        public Action<NotifiablePoint> RaiseCoordinateChangedAction { get; set; }

        private bool _isRaiseCoordinateChangeEnabled = true;

        public bool IsRaiseCoordinateChangeEnabled
        {
            get { return _isRaiseCoordinateChangeEnabled; }
            set { _isRaiseCoordinateChangeEnabled = value; }
        }


        public bool IsNaN()
        {
            return double.IsNaN(X) || double.IsNaN(Y);
        }

    }
}
