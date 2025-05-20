using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Media;

using IRI.Sta.Common.Helpers;

namespace IRI.Jab.Common.Helpers;

public static class BrushHelper
{
    public static Brush PickBrush()
    {
        Brush result = Brushes.Transparent;

        Type brushesType = typeof(Brushes);

        PropertyInfo[] properties = brushesType.GetProperties();

        int randomIndex = RandomHelper.Get(properties.Length);

        result = (Brush)properties[randomIndex].GetValue(null, null);

        return new SolidColorBrush(((SolidColorBrush)result).Color);
    }

    public static Brush PickGoodBrush()
    {
        return new SolidColorBrush(ColorHelper.GetRandomWpfColor());
    }

    public static List<SolidColorBrush> GetAllKnownColors()
    {
        Type ColorType = typeof(System.Windows.Media.Colors);

        PropertyInfo[] colors = ColorType.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);

        List<SolidColorBrush> result = new List<SolidColorBrush>(colors.Length);

        foreach (PropertyInfo item in colors)
        {
            result.Add(new SolidColorBrush((Color)item.GetValue(null, null)));
        }

        return result;
    }

    public static Brush CreateFromNameOrHex(string colorNameOrHex)
    {
        var converter = new BrushConverter();

        return converter.ConvertFromString(colorNameOrHex) as SolidColorBrush;
    }

    public static Brush CreateBrush(string hexColor)
    {
        if (hexColor == null)
        {
            return null;
        }

        if (!hexColor.StartsWith("#"))
        {
            hexColor = $"#{hexColor}";
        }

        return (SolidColorBrush)(new BrushConverter().ConvertFrom(hexColor));
    }

    public static Brush CreateBrush(string hexColor, double opacity)
    {
        if (!hexColor.StartsWith("#"))
        {
            hexColor = $"#{hexColor}";
        }

        var color = ColorHelper.ToWpfColor(hexColor);

        var alpha = opacity > 1 ? color.A : (opacity < 0 ? 0 : opacity * color.A);

        return new SolidColorBrush(Color.FromArgb((byte)alpha, r: color.R, g: color.G, b: color.B));
    }

    public static Brush CreateBrush(Color color, double opacity)
    {
        var alpha = opacity > 1 ? color.A : (opacity < 0 ? 0 : opacity * color.A);

        return new SolidColorBrush(Color.FromArgb((byte)alpha, r: color.R, g: color.G, b: color.B));
    }

    public static System.Drawing.Brush CreateGdiBrush(string hexColor)
    {
        return new System.Drawing.SolidBrush(ColorHelper.ToGdiColor(hexColor));
    }

    public static System.Drawing.Brush CreateGdiBrush(string hexColor, double opacity)
    {
        var color = ColorHelper.ToGdiColor(hexColor);

        //var alpha = opacity > 1 ? color.A : (opacity < 0 ? 0 : opacity * color.A);

        //return new System.Drawing.SolidBrush(System.Drawing.Color.FromArgb(alpha: (int)alpha, red: color.R, green: color.G, blue: color.B));

        return CreateGdiBrush(color, opacity);
    }

    public static System.Drawing.Brush CreateGdiBrush(System.Drawing.Color color, double opacity)
    {
        var alpha = opacity > 1 ? color.A : (opacity < 0 ? 0 : opacity * color.A);

        return new System.Drawing.SolidBrush(System.Drawing.Color.FromArgb(alpha: (int)alpha, red: color.R, green: color.G, blue: color.B));
    }

}
