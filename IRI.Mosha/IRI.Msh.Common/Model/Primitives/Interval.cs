using System;
using System.Collections.Generic;
using System.Text;

namespace IRI.Msh.Common.Model
{
    public class Interval
    {
        private double _min;

        public double Min
        {
            get { return _min; }
            set
            {
                _min = value;
                UpdateMidValue();
            }
        }

        private double _max;

        public double Max
        {
            get { return _max; }
            set
            {
                _max = value;
                UpdateMidValue();
            }
        }
         
        public double Mid { get; private set; }

        private Interval()
        {

        }

        private Interval(double min, double max)
        {
            this.Min = min;

            this.Max = max;
        }

        private void UpdateMidValue()
        {
            this.Mid = (Min + Max) / 2.0;
        }

        public static Interval Create(double min, double max)
        {
            if (min > max)
            {
                return null;
            }

            return new Interval(min, max);
        }

        public override bool Equals(object obj)
        {
            var interval = obj as Interval;

            if (obj == null)
            {
                return false;
            }

            return this.Min == interval.Min && this.Max == interval.Max;
        }
    }
}
