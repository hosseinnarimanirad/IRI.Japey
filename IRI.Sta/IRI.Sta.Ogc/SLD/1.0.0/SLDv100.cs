using System.Collections.Generic;
using System.Xml.Serialization;

namespace IRI.Sta.Ogc.SLD;

// Main SLD classes
[XmlRoot("StyledLayerDescriptor", Namespace = "http://www.opengis.net/sld", IsNullable = false)]
public class StyledLayerDescriptor
{
    public const string defaultNamespace = "http://www.opengis.net/sld";

    [XmlElement("Name")]
    public string Name { get; set; }

    [XmlElement("Title")]
    public string Title { get; set; }

    [XmlElement("Abstract")]
    public string Abstract { get; set; }

    [XmlElement("NamedLayer")]
    public List<NamedLayer> NamedLayers { get; set; }

    [XmlElement("UserLayer")]
    public List<UserLayer> UserLayers { get; set; }

    [XmlAttribute("version")]
    public string Version { get; set; } = "1.0.0";

    // Correct schemaLocation attribute declaration
    [XmlAttribute("schemaLocation", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
    public string SchemaLocation { get; set; } = "http://www.opengis.net/sld StyledLayerDescriptor.xsd";

    // Add these namespace declarations for proper serialization
    [XmlNamespaceDeclarations]
    public XmlSerializerNamespaces Xmlns { get; set; }

    public StyledLayerDescriptor()
    {
        Xmlns = new XmlSerializerNamespaces();
        Xmlns.Add("", defaultNamespace);
        Xmlns.Add("ogc", "http://www.opengis.net/ogc");
        Xmlns.Add("xlink", "http://www.w3.org/1999/xlink");
        Xmlns.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");
    }
}

public class NamedLayer
{
    [XmlElement("Name")]
    public string Name { get; set; }

    [XmlElement("LayerFeatureConstraints")]
    public LayerFeatureConstraints LayerFeatureConstraints { get; set; }

    [XmlElement("NamedStyle")]
    public List<NamedStyle> NamedStyles { get; set; }

    [XmlElement("UserStyle")]
    public List<UserStyle> UserStyles { get; set; }
}

public class NamedStyle
{
    [XmlElement("Name")]
    public string Name { get; set; }
}

public class UserLayer
{
    [XmlElement("Name")]
    public string Name { get; set; }

    [XmlElement("RemoteOWS")]
    public RemoteOWS RemoteOWS { get; set; }

    [XmlElement("LayerFeatureConstraints")]
    public LayerFeatureConstraints LayerFeatureConstraints { get; set; }

    [XmlElement("UserStyle")]
    public List<UserStyle> UserStyles { get; set; }
}

public class RemoteOWS
{
    [XmlElement("Service")]
    public ServiceType Service { get; set; }

    [XmlElement("OnlineResource")]
    public OnlineResource OnlineResource { get; set; }
}

public enum ServiceType
{
    WFS,
    WCS
}

//[XmlType(Namespace = "http://www.w3.org/1999/xlink")]
public class OnlineResource
{
    [XmlAttribute("type", Namespace = "http://www.w3.org/1999/xlink")]
    public string Type { get; set; } = "simple";

    [XmlAttribute("href", Namespace = "http://www.w3.org/1999/xlink")]
    public string Href { get; set; }
}

public class LayerFeatureConstraints
{
    [XmlElement("FeatureTypeConstraint")]
    public List<FeatureTypeConstraint> FeatureTypeConstraints { get; set; }
}

public class FeatureTypeConstraint
{
    [XmlElement("FeatureTypeName")]
    public string FeatureTypeName { get; set; }

    [XmlElement("Filter", Namespace = "http://www.opengis.net/ogc")]
    public FilterType Filter { get; set; }

    [XmlElement("Extent")]
    public List<Extent> Extents { get; set; }
}

public class Extent
{
    [XmlElement("Name")]
    public string Name { get; set; }

    [XmlElement("Value")]
    public string Value { get; set; }
}

// UserStyle and related classes
public class UserStyle
{
    [XmlElement("Name")]
    public string Name { get; set; }

    [XmlElement("Title")]
    public string Title { get; set; }

    [XmlElement("Abstract")]
    public string Abstract { get; set; }

    [XmlElement("IsDefault")]
    public bool? IsDefault { get; set; }

    [XmlElement("FeatureTypeStyle")]
    public List<FeatureTypeStyle> FeatureTypeStyles { get; set; }


    // This pattern prevents serialization when null
    public bool ShouldSerializeIsDefault() => IsDefault.HasValue;
}

public class FeatureTypeStyle
{
    [XmlElement("Name")]
    public string Name { get; set; }

    [XmlElement("Title")]
    public string Title { get; set; }

    [XmlElement("Abstract")]
    public string Abstract { get; set; }

    [XmlElement("FeatureTypeName")]
    public string FeatureTypeName { get; set; }

    [XmlElement("SemanticTypeIdentifier")]
    public List<string> SemanticTypeIdentifiers { get; set; }

    [XmlElement("Rule")]
    public List<Rule> Rules { get; set; }
}

public class Rule
{
    [XmlElement("Name")]
    public string Name { get; set; }

    [XmlElement("Title")]
    public string Title { get; set; }

    [XmlElement("Abstract")]
    public string Abstract { get; set; }

    [XmlElement("LegendGraphic")]
    public LegendGraphic LegendGraphic { get; set; }

    [XmlElement("Filter", Namespace = "http://www.opengis.net/ogc")]
    public FilterType Filter { get; set; }

    [XmlElement("ElseFilter")]
    public ElseFilter ElseFilter { get; set; }

    [XmlElement("MinScaleDenominator")]
    public double? MinScaleDenominator { get; set; }

    [XmlElement("MaxScaleDenominator")]
    public double? MaxScaleDenominator { get; set; }

    [XmlElement("Symbolizer")]
    [XmlElement("PolygonSymbolizer", Type = typeof(PolygonSymbolizer))]
    [XmlElement("LineSymbolizer", Type = typeof(LineSymbolizer))]
    [XmlElement("PointSymbolizer", Type = typeof(PointSymbolizer))]
    [XmlElement("TextSymbolizer", Type = typeof(TextSymbolizer))]
    //[XmlElement("RasterSymbolizer", Type = typeof(RasterSymbolizer))]
    public List<Symbolizer> Symbolizers { get; set; }



    // This pattern prevents serialization when null
    public bool ShouldSerializeMinScaleDenominator() => MinScaleDenominator.HasValue;
    public bool ShouldSerializeMaxScaleDenominator() => MaxScaleDenominator.HasValue;


}

public class LegendGraphic
{
    [XmlElement("Graphic")]
    public Graphic Graphic { get; set; }
}

public class ElseFilter { }

// Symbolizer hierarchy
[XmlInclude(typeof(LineSymbolizer))]
[XmlInclude(typeof(PolygonSymbolizer))]
[XmlInclude(typeof(PointSymbolizer))]
[XmlInclude(typeof(TextSymbolizer))]
//[XmlInclude(typeof(RasterSymbolizer))]
public abstract class Symbolizer
{
    [XmlElement("Geometry")]
    public Geometry Geometry { get; set; }
}

public class LineSymbolizer : Symbolizer
{
    [XmlElement("Stroke")]
    public Stroke Stroke { get; set; }
}

//[System.Serializable]
//[XmlType(AnonymousType = true, Namespace = StyledLayerDescriptor.defaultNamespace)]
public class PolygonSymbolizer : Symbolizer
{
    [XmlElement("Fill")]
    public Fill Fill { get; set; }

    [XmlElement("Stroke")]
    public Stroke Stroke { get; set; }
}

public class PointSymbolizer : Symbolizer
{
    [XmlElement("Graphic")]
    public Graphic Graphic { get; set; }
}

public class TextSymbolizer : Symbolizer
{
    [XmlElement("Label")]
    public Label Label { get; set; }

    [XmlElement("Font")]
    public Font Font { get; set; }

    [XmlElement("LabelPlacement")]
    public LabelPlacement LabelPlacement { get; set; }

    [XmlElement("Halo")]
    public Halo Halo { get; set; }

    [XmlElement("Fill")]
    public Fill Fill { get; set; }
}

public class RasterSymbolizer : Symbolizer
{
    [XmlElement("Opacity")]
    public double? Opacity { get; set; }

    [XmlElement("ChannelSelection")]
    public ChannelSelection ChannelSelection { get; set; }

    [XmlElement("OverlapBehavior")]
    public OverlapBehavior OverlapBehavior { get; set; }

    [XmlElement("ColorMap")]
    public ColorMap? ColorMap { get; set; }

    [XmlElement("ContrastEnhancement")]
    public ContrastEnhancement ContrastEnhancement { get; set; }

    [XmlElement("ShadedRelief")]
    public ShadedRelief ShadedRelief { get; set; }

    [XmlElement("ImageOutline")]
    public ImageOutline ImageOutline { get; set; }


    // This pattern prevents serialization when null
    public bool ShouldSerializeOpacity() => Opacity.HasValue;
}

public class Label
{
    [XmlElement("PropertyName", Namespace = "http://www.opengis.net/ogc")]
    public string PropertyName { get; set; }
}


// Supporting classes for symbolizers
public class Geometry
{
    [XmlElement("PropertyName", Namespace = "http://www.opengis.net/ogc")]
    public string PropertyName { get; set; }
}

public class Stroke
{
    [XmlElement("GraphicFill")]
    public GraphicFill GraphicFill { get; set; }

    [XmlElement("GraphicStroke")]
    public GraphicStroke GraphicStroke { get; set; }

    [XmlElement("CssParameter")]
    public List<CssParameter> CssParameters { get; set; }
}

public class Fill
{
    [XmlElement("GraphicFill")]
    public GraphicFill GraphicFill { get; set; }

    [XmlElement("CssParameter")]
    public List<CssParameter> CssParameters { get; set; }
}

public class CssParameter
{
    [XmlAttribute("name")]
    public string Name { get; set; }

    [XmlText]
    public string Value { get; set; }
}

[XmlInclude(typeof(ExternalGraphic))]
[XmlInclude(typeof(Mark))]
public class Graphic
{
    [XmlElement("ExternalGraphic")]
    public List<ExternalGraphic> ExternalGraphics { get; set; }

    [XmlElement("Mark")]
    public List<Mark> Marks { get; set; }

    [XmlElement("Opacity")]
    public double? Opacity { get; set; }

    [XmlElement("Size")]
    public int Size { get; set; }
    //public ParameterValueType Size { get; set; }

    [XmlElement("Rotation")]
    public double? Rotation { get; set; }
    //public ParameterValueType Rotation { get; set; }


    // This pattern prevents serialization when null
    public bool ShouldSerializeOpacity() => Opacity.HasValue;
    public bool ShouldSerializeRotation() => Rotation.HasValue;
}

public class GraphicFill
{
    [XmlElement("Graphic")]
    public Graphic Graphic { get; set; }
}

public class GraphicStroke
{
    [XmlElement("Graphic")]
    public Graphic Graphic { get; set; }
}

public class ExternalGraphic
{
    [XmlElement("OnlineResource")]
    public OnlineResource OnlineResource { get; set; }

    [XmlElement("Format")]
    public string Format { get; set; }
}

public class Mark
{
    [XmlElement("WellKnownName")]
    public string WellKnownName { get; set; }

    [XmlElement("Fill")]
    public Fill Fill { get; set; }

    [XmlElement("Stroke")]
    public Stroke Stroke { get; set; }
}

public class ParameterValueType
{
    [XmlElement("expression", Namespace = "http://www.opengis.net/ogc")]
    public List<string> Expressions { get; set; }

    [XmlText]
    public string Value { get; set; }
}

// Text symbolizer supporting classes
public class Font
{
    [XmlElement("CssParameter")]
    public List<CssParameter> CssParameters { get; set; }
}

public class LabelPlacement
{
    [XmlElement("PointPlacement")]
    public PointPlacement PointPlacement { get; set; }

    [XmlElement("LinePlacement")]
    public LinePlacement LinePlacement { get; set; }
}

public class PointPlacement
{
    [XmlElement("AnchorPoint")]
    public AnchorPoint AnchorPoint { get; set; }

    [XmlElement("Displacement")]
    public Displacement Displacement { get; set; }

    [XmlElement("Rotation")]
    public ParameterValueType Rotation { get; set; }
}

public class AnchorPoint
{
    [XmlElement("AnchorPointX")]
    public ParameterValueType AnchorPointX { get; set; }

    [XmlElement("AnchorPointY")]
    public ParameterValueType AnchorPointY { get; set; }
}

public class Displacement
{
    [XmlElement("DisplacementX")]
    public ParameterValueType DisplacementX { get; set; }

    [XmlElement("DisplacementY")]
    public ParameterValueType DisplacementY { get; set; }
}

public class LinePlacement
{
    [XmlElement("PerpendicularOffset")]
    public ParameterValueType PerpendicularOffset { get; set; }
}

public class Halo
{
    [XmlElement("Radius")]
    public ParameterValueType Radius { get; set; }

    [XmlElement("Fill")]
    public Fill Fill { get; set; }
}

// Raster symbolizer supporting classes
public class ChannelSelection
{
    [XmlElement("RedChannel")]
    public SelectedChannelType RedChannel { get; set; }

    [XmlElement("GreenChannel")]
    public SelectedChannelType GreenChannel { get; set; }

    [XmlElement("BlueChannel")]
    public SelectedChannelType BlueChannel { get; set; }

    [XmlElement("GrayChannel")]
    public SelectedChannelType GrayChannel { get; set; }
}

public class SelectedChannelType
{
    [XmlElement("SourceChannelName")]
    public string SourceChannelName { get; set; }

    [XmlElement("ContrastEnhancement")]
    public ContrastEnhancement ContrastEnhancement { get; set; }
}

public class OverlapBehavior
{
    [XmlElement("LATEST_ON_TOP")]
    public LatestOnTop LatestOnTop { get; set; }

    [XmlElement("EARLIEST_ON_TOP")]
    public EarliestOnTop EarliestOnTop { get; set; }

    [XmlElement("AVERAGE")]
    public Average Average { get; set; }

    [XmlElement("RANDOM")]
    public Random Random { get; set; }
}

public class LatestOnTop { }
public class EarliestOnTop { }
public class Average { }
public class Random { }

public class ColorMap
{
    [XmlElement("ColorMapEntry")]
    public List<ColorMapEntry>? ColorMapEntries { get; set; }
}

public class ColorMapEntry
{
    [XmlAttribute("color")]
    public string Color { get; set; }

    [XmlAttribute("opacity")]
    public double? Opacity { get; set; }

    [XmlAttribute("quantity")]
    public double? Quantity { get; set; }

    [XmlAttribute("label")]
    public string Label { get; set; }


    // This pattern prevents serialization when null
    public bool ShouldSerializeOpacity() => Opacity.HasValue;
}

public class ContrastEnhancement
{
    [XmlElement("Normalize")]
    public Normalize Normalize { get; set; }

    [XmlElement("Histogram")]
    public Histogram Histogram { get; set; }

    [XmlElement("GammaValue")]
    public double? GammaValue { get; set; }
}

public class Normalize { }
public class Histogram { }

public class ShadedRelief
{
    [XmlElement("BrightnessOnly")]
    public bool? BrightnessOnly { get; set; }

    [XmlElement("ReliefFactor")]
    public double? ReliefFactor { get; set; }
}

public class ImageOutline
{
    [XmlElement("LineSymbolizer")]
    public LineSymbolizer LineSymbolizer { get; set; }

    [XmlElement("PolygonSymbolizer")]
    public PolygonSymbolizer PolygonSymbolizer { get; set; }
}

// OGC Filter types (simplified)
public class FilterType
{
    // This would be expanded with actual filter capabilities
    [XmlElement("PropertyIsEqualTo", Namespace = "http://www.opengis.net/ogc")]
    public PropertyFilterBase PropertyIsEqualTo { get; set; }


    [XmlElement("PropertyIsLessThan", Namespace = "http://www.opengis.net/ogc")]
    public PropertyFilterBase PropertyIsLessThan { get; set; }


    [XmlElement("PropertyIsNotEqualTo", Namespace = "http://www.opengis.net/ogc")]
    public PropertyFilterBase PropertyIsNotEqualTo { get; set; }


    [XmlElement("PropertyIsLessThanOrEqualTo", Namespace = "http://www.opengis.net/ogc")]
    public PropertyFilterBase PropertyIsLessThanOrEqualTo { get; set; }


    [XmlElement("PropertyIsGreaterThan", Namespace = "http://www.opengis.net/ogc")]
    public PropertyFilterBase PropertyIsGreaterThan { get; set; }


    [XmlElement("PropertyIsGreaterThanOrEqualTo", Namespace = "http://www.opengis.net/ogc")]
    public PropertyFilterBase PropertyIsGreaterThanOrEqualTo { get; set; }

    // Add other filter types as needed



    //<PropertyIsLike>

    //<PropertyIsNull>

    //<PropertyIsBetween>



}

public class PropertyFilterBase
{
    [XmlElement("PropertyName", Namespace = "http://www.opengis.net/ogc")]
    public string PropertyName { get; set; }

    [XmlElement("Literal", Namespace = "http://www.opengis.net/ogc")]
    public string Literal { get; set; }
}
