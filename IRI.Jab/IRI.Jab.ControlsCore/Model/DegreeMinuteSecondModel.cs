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
        private double _degree;

        public double Degree
        {
            get { return _degree; }
            set
            {
                if (_degree == value)
                    return;

                _degree = value;

                RaisePropertyChanged();

                this.OnValueChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private double _minute;

        public double Minute
        {
            get { return _minute; }
            set
            {
                if (_minute == value)
                    return;

                _minute = value;

                RaisePropertyChanged();

                this.OnValueChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private double _second;

        public double Second
        {
            get { return _second; }
            set
            {
                if (_second == value)
                    return;
                _second = value;

                RaisePropertyChanged();

                this.OnValueChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public DegreeMinuteSecondModel() : this(0)
        {

        }

        public DegreeMinuteSecondModel(double deciamalDegree)
        {
            SetValue(deciamalDegree);
        }

        public double GetDegreeValue()
        {
            return Degree + Minute / 60.0 + Second / 3600.0;
        }

        public event EventHandler OnValueChanged;

        internal void SetValue(double value)
        {
            int degree, minute;

            double second;

            IRI.Ket.Common.Helpers.DegreeHelper.ToDms(value, true, out degree, out minute, out second);

            this.Degree = degree;

            this.Minute = minute;

            this.Second = second;
        }
    }
}
