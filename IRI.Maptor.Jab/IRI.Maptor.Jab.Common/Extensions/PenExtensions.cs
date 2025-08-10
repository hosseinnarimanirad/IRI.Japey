using System.Windows.Media;
using IRI.Maptor.Jab.Common.Helpers;

namespace IRI.Maptor.Extensions;

public static class PenExtensions
{
    public static System.Drawing.Pen AsGdiPen(this Pen pen)
    {
        var brush = pen.Brush as SolidColorBrush;
        brush = brush ?? Brushes.Transparent;
        System.Drawing.Color color = brush.Color.AsGdiColor();
        var gdiPen = new System.Drawing.Pen(color, (float)pen.Thickness);

        return gdiPen;
    }


    public static System.Drawing.Pen AsGdiPen(string hexColor, float thickness)
    {
        var color = ColorHelper.ToGdiColor(hexColor);

        return new System.Drawing.Pen(color, thickness);
    }

    public static System.Drawing.Pen AsGdiPen(string hexColor, float thickness, double opacity)
    {
        var color = ColorHelper.ToGdiColor(hexColor);

        var alpha = opacity > 1 ? 255 : opacity < 0 ? 0 : opacity * 255;

        return new System.Drawing.Pen(System.Drawing.Color.FromArgb((int)alpha, color.R, color.G, color.B), thickness);
    }
}
