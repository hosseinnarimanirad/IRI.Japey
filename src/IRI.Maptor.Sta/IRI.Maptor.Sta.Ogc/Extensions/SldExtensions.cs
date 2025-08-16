using IRI.Maptor.Sta.Metrics;
using IRI.Maptor.Sta.Ogc.SLD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SldHelper = IRI.Maptor.Sta.Ogc.SLD.SldHelper;

namespace IRI.Maptor.Extensions;

public static class SldExtensions
{
    private const double _defaultFontSize = 10;
    private const double _defaultFillOpacity = 1;
    private const double _defaultStrokeWidth = 1;
    private const double _defaultStrokeDashOffset = 0;

    private static string? GetValue(this List<CssParameter> parameters, string key)
    {
        if (parameters.IsNullOrEmpty())
            return null;

        return parameters.FirstOrDefault(p => p.Name.EqualsIgnoreCase(key))?.Value ?? null;
    }

    #region Font

    public static string? GetFontFamily(this Font font)
    {
        return font.CssParameters.GetValue(SldHelper.CssParameter_FontFamily);
    }

    public static double GetFontSize(this Font font)
    {
        double result;

        if (!double.TryParse(font.CssParameters.GetValue(SldHelper.CssParameter_FontSize), out result))
            result = _defaultFontSize;

        return result;
    }

    public static string? GetFontStyle(this Font font)
    {
        return font.CssParameters.GetValue(SldHelper.CssParameter_FontStyle);
    }

    public static string? GetFontWeight(this Font font)
    {
        return font.CssParameters.GetValue(SldHelper.CssParameter_FontWeight);
    }

    #endregion


    #region Fill

    public static string? GetFill(this Fill fill)
    {
        return fill.CssParameters.GetValue(SldHelper.CssParameter_Fill);
    }

    public static double GetFillOpacity(this Fill fill)
    {
        double result;

        if (!double.TryParse(fill.CssParameters.GetValue(SldHelper.CssParameter_FillOpacity), out result))
            result = _defaultFillOpacity;

        return result;
    }

    #endregion


    #region Stroke


    public static string? GetStroke(this Stroke stoke)
    {
        return stoke.CssParameters.GetValue(SldHelper.CssParameter_Stroke);
    }

    public static double GetStrokeWidth(this Stroke stoke)
    {
        double result;

        if (!double.TryParse(stoke.CssParameters.GetValue(SldHelper.CssParameter_StrokeWidth), out result))
            result = _defaultStrokeWidth;

        return result;
    }

    public static double GetStrokeOpacity(this Stroke stoke)
    {
        double result;

        if (!double.TryParse(stoke.CssParameters.GetValue(SldHelper.CssParameter_StrokeOpacity), out result))
            result = _defaultFillOpacity;

        return result;
    }

    public static string? GetStrokeLinejoin(this Stroke stoke)
    {
        return stoke.CssParameters.GetValue(SldHelper.CssParameter_StrokeLinejoin);
    }

    public static string? GetStrokeLineCap(this Stroke stoke)
    {
        return stoke.CssParameters.GetValue(SldHelper.CssParameter_StrokeLineCap);
    }

    public static int[]? GetStrokeStrokeDashArray(this Stroke stoke)
    {
        var dashArray = stoke.CssParameters.GetValue(SldHelper.CssParameter_StrokeDashArray);

        if (dashArray.IsNullOrEmpty())
            return null;

        return dashArray.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
    }

    public static double GetStrokeDashOffset(this Stroke stoke)
    {
        double result;

        if (!double.TryParse(stoke.CssParameters.GetValue(SldHelper.CssParameter_StrokeDashOffset), out result))
            result = _defaultStrokeDashOffset;

        return result;
    }
     
    #endregion

}
