using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRI.Jab.Cartography
{
    public class ScaleInterval
    {
        private double _lower;

        public double Lower
        {
            get { return _lower; }
            set { _lower = value; }
        }

        private double _upper;

        public double Upper
        {
            get { return _upper; }
            set { _upper = value; }
        }
         

        public ScaleInterval(double lower, double upper)
        {
            if (lower > upper)
            {
                throw new NotImplementedException();
            }

            this.Lower = lower;
            this.Upper = upper;
        }

        public override bool Equals(object obj)
        {
            if (obj is double) return ((double)obj) > Lower && ((double)obj) < Upper;

            else if (obj is ScaleInterval)
            {
                ScaleInterval temp = obj as ScaleInterval;

                return temp.Lower.Equals(this.Lower) && temp.Upper.Equals(this.Upper);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("Lower = {0}, Upper = {1}", Lower, Upper);
        }

        public static readonly ScaleInterval All = new ScaleInterval(double.NegativeInfinity, double.PositiveInfinity);

        public static ScaleInterval Create(int minGoogleZoomLevel, int maxGoogleZoomLevel = 19)
        {
            if (maxGoogleZoomLevel < minGoogleZoomLevel)
            {
                throw new NotImplementedException();
            }

            var minInverse = 1.0 / IRI.Ham.SpatialBase.Mapping.WebMercatorUtility.GetGoogleMapScale(minGoogleZoomLevel);

            var maxInverse = 1.0 / IRI.Ham.SpatialBase.Mapping.WebMercatorUtility.GetGoogleMapScale(maxGoogleZoomLevel);

            return new ScaleInterval(maxInverse, minInverse);
        }

        public bool IsInRange(double inverseMapScale)
        {
            return (this.Upper >= inverseMapScale && this.Lower < inverseMapScale);
        }
    }
}
