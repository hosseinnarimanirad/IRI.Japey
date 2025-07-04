using IRI.Extensions;
using IRI.Sta.Common.Abstrations;
using IRI.Sta.Spatial.GeoJsonFormat;
using IRI.Sta.SpatialReferenceSystem;
using IRI.Sta.Common.PropertyDescriptors;
using IRI.Sta.SpatialReferenceSystem.MapProjections;
using System.ComponentModel;

namespace IRI.Sta.Spatial.Primitives;


public class Feature<T> : IGeometryAware<T>, ICustomTypeDescriptor where T : IPoint, new()
{
    protected const string _defaultLabelAttributeName = "Label";

    public int Id { get; set; }

    public Geometry<T> TheGeometry { get; set; }

    public Dictionary<string, object> Attributes { get; set; }

    public string LabelAttribute { get; set; } = _defaultLabelAttributeName;

    public string Label
    {
        get
        {
            if (Attributes?.ContainsKey(LabelAttribute) == true)
                return Attributes[LabelAttribute]?.ToString() ?? string.Empty;

            return string.Empty;
        }
    }

    public Feature(Geometry<T> geometry) : this(geometry, new Dictionary<string, object>())
    {

    }

    public Feature(Geometry<T> geometry, string label) : this(geometry, new Dictionary<string, object>() { { _defaultLabelAttributeName, label } })
    {

    }

    public Feature(Geometry<T> geometry, Dictionary<string, object> attributes)
    {
        this.TheGeometry = geometry;

        this.Attributes = attributes;
    }

    public Feature()
    {

    }

    public GeoJsonFeature AsGeoJsonFeature()
    {
        return new GeoJsonFeature() { Geometry = TheGeometry.Project(SrsBases.GeodeticWgs84).AsGeoJson(), Id = Id.ToString(), Properties = Attributes };
    }

    public GeoJsonFeature AsGeoJsonFeature(Func<T, T> toWgs84Func, bool isLongitudeFirst)
    {
        return new GeoJsonFeature()
        {
            Geometry = this.TheGeometry.Transform(toWgs84Func, SridHelper.GeodeticWGS84).AsGeoJson(isLongitudeFirst),
            Id = this.Id.ToString(),
            Properties = this.Attributes/*.ToDictionary(k => k.Key, k => k.Value)*/,

        };
    }

    public override string ToString()
    {
        return $"Geometry: {TheGeometry?.Type}, Attributes: {Attributes?.Count}";
    }


    #region ICustomTypeDescriptor


    public string GetComponentName()
    {
        return TypeDescriptor.GetComponentName(this, true);
    }

    public EventDescriptor GetDefaultEvent()
    {
        return TypeDescriptor.GetDefaultEvent(this, true);
    }

    public string GetClassName()
    {
        return TypeDescriptor.GetClassName(this, true);
    }

    public EventDescriptorCollection GetEvents(Attribute[] attributes)
    {
        return TypeDescriptor.GetEvents(this, attributes, true);
    }

    EventDescriptorCollection System.ComponentModel.ICustomTypeDescriptor.GetEvents()
    {
        return TypeDescriptor.GetEvents(this, true);
    }

    public TypeConverter GetConverter()
    {
        return TypeDescriptor.GetConverter(this, true);
    }

    public object GetPropertyOwner(PropertyDescriptor pd)
    {
        // return the dictionary containing attributes
        return this.Attributes;
    }

    public AttributeCollection GetAttributes()
    {
        return TypeDescriptor.GetAttributes(this, true);
    }

    public object GetEditor(Type editorBaseType)
    {
        return TypeDescriptor.GetEditor(this, editorBaseType, true);
    }

    public PropertyDescriptor GetDefaultProperty()
    {
        return null;
    }

    PropertyDescriptorCollection System.ComponentModel.ICustomTypeDescriptor.GetProperties()
    {
        return ((ICustomTypeDescriptor)this).GetProperties(new Attribute[0]);
    }

    public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
    {
        System.Collections.ArrayList properties = new System.Collections.ArrayList();

        foreach (var e in this.Attributes)
        {
            properties.Add(new DictionaryPropertyDescriptor(this.Attributes, e.Key));
        }

        PropertyDescriptor[] props = (PropertyDescriptor[])properties.ToArray(typeof(PropertyDescriptor));

        return new PropertyDescriptorCollection(props);
    }

    #endregion
}


