using IRI.Jab.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Jab.Controls.Model
{
    public class DegreeMinuteSecondModel : Notifier
    {
        private int _degree;

        public int Degree
        {
            get { return _degree; }
            set
            {
                _degree = value;
                RaisePropertyChanged();
            }
        }

        private int _minute;

        public int Minute
        {
            get { return _minute; }
            set
            {
                _minute = value;
                RaisePropertyChanged();
            }
        }

        private double _second;

        public double Second
        {
            get { return _second; }
            set
            {
                _second = value;
                RaisePropertyChanged();
            }
        }

        public DegreeMinuteSecondModel(double deciamlDegree)
        {
            int degree, minute;

            double second;

            IRI.Ket.Common.Helpers.DegreeHelper.ToDms(deciamlDegree, true, out degree, out minute, out second);
        }

        public double GetDegreeValue()
        {
            return Degree + Minute / 60.0 + Second / 3600.0;
        }
    }
}
