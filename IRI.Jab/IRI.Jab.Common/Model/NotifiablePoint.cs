using IRI.Ham.SpatialBase;
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

        public NotifiablePoint(double x, double y)
        {
            this.X = x;

            this.Y = y;
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
    }
}
