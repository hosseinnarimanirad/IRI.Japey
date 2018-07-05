using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace IRI.Jab.Common.Raster.Model
{

    public class DiscreteRangeColor
    {
        public double MinValue { get; }

        public double MaxValue { get; }

        public List<double> MidValues { get; }

        public List<Color> Colors { get; }

        public DiscreteRangeColor(List<double> values, List<Color> colors)
        {
            if (values.Count - 1 != colors.Count)
            {
                throw new NotImplementedException();
            }

            MidValues = values;

            this.Colors = colors;
        }

        public Color Interpolate(double value)
        {
            var index = MidValues.IndexOf(MidValues.First(v => v >= value));

            return Colors[index == 0 ? 0 : index - 1];
        }
    }

}
