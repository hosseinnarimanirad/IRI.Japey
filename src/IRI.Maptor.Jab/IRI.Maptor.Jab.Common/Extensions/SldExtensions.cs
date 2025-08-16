using System;
using System.Collections.Generic;
using System.Linq;

using IRI.Maptor.Jab.Common;
using IRI.Maptor.Sta.Ogc.SLD;
using IRI.Maptor.Jab.Common.Cartography.Symbologies;

namespace IRI.Maptor.Extensions;

public static class SldExtensions
{
    public static List<ISymbolizer> ParseToSymbolizers(this StyledLayerDescriptor sld)
    {
        var result = new List<ISymbolizer>();

        var featureTypeStyles = sld.UserLayers.SelectMany(u => u.UserStyles.SelectMany(us => us.FeatureTypeStyles)).ToList();

        if (!featureTypeStyles.IsNullOrEmpty())
        {
            foreach (var item in featureTypeStyles)
            {
                result.AddRange(item.Parse());
            }
        }

        featureTypeStyles = sld.NamedLayers.SelectMany(n => n.UserStyles.SelectMany(us => us.FeatureTypeStyles)).ToList();

        if (!featureTypeStyles.IsNullOrEmpty())
        {
            foreach (var item in featureTypeStyles)
            {
                result.AddRange(item.Parse());
            }
        }

        return result;
    }

    public static List<ISymbolizer> Parse(this FeatureTypeStyle featureTypeStyle)
    {
        var result = new List<ISymbolizer>();

        if (featureTypeStyle is null || featureTypeStyle.Rules.IsNullOrEmpty())
            return result;

        foreach (var rule in featureTypeStyle.Rules)
        {
            foreach (var sldSymbolizer in rule.Symbolizers)
            {
                var symbolizer = ParseSymbolizer(sldSymbolizer);

                symbolizer.MaxScaleDenominator = rule.MaxScaleDenominator;
                symbolizer.MinScaleDenominator = rule.MinScaleDenominator;

                result.Add(symbolizer);
            }
        }

        return result;
    }

    public static ISymbolizer ParseSymbolizer(Symbolizer symbolizer)
    {
        switch (symbolizer)
        {
            case PointSymbolizer pointSymbolizer:
                return Parse(pointSymbolizer);

            case LineSymbolizer lineSymbolizer:
                return Parse(lineSymbolizer);

            case PolygonSymbolizer polygonSymbolizer:
                return Parse(polygonSymbolizer);

            case TextSymbolizer textSymbolizer:
                return Parse(textSymbolizer);

            default:
                throw new NotImplementedException("SldExtensions > ");
        }
    }

    public static ISymbolizer Parse(LineSymbolizer lineSymbolizer)
    {
        var cssParameters = lineSymbolizer.Stroke.CssParameters;

        var stoke = lineSymbolizer.Stroke.GetStroke();
        var strokeThickness = lineSymbolizer.Stroke.GetStrokeWidth();
        var strokeOpacity = lineSymbolizer.Stroke.GetStrokeOpacity();

        return new SimpleSymbolizer(VisualParameters.GetStroke(stoke, strokeThickness, strokeOpacity));
    }

    public static ISymbolizer Parse(PolygonSymbolizer polygonSymbolizer) { return null; }

    public static ISymbolizer Parse(PointSymbolizer pointSymbolizer) { return null; }

    public static ISymbolizer Parse(TextSymbolizer textSymbolizer) { return null; }

}
