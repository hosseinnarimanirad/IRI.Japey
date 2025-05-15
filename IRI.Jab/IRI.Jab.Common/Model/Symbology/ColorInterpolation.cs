using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace IRI.Jab.Common.Model.Symbology;

public class ColorInterpolation
{
    Color _minColor, _maxColor;

    int rangeR, rangeB, rangeG;

    public ColorInterpolation(Color minColor, Color maxColor)
    {
        _minColor = minColor;

        _maxColor = maxColor;

        rangeR = maxColor.R - minColor.R;
        rangeG = maxColor.G - minColor.G;
        rangeB = maxColor.B - minColor.B;

    }

    public Color Interpolate(double value, double minValue, double maxValue)
    {
        var r = (int)(_minColor.R + rangeR / (maxValue - minValue) * (value - minValue));
        var g = (int)(_minColor.G + rangeG / (maxValue - minValue) * (value - minValue));
        var b = (int)(_minColor.B + rangeB / (maxValue - minValue) * (value - minValue));

        return Color.FromArgb(r, g, b);

    }

}
