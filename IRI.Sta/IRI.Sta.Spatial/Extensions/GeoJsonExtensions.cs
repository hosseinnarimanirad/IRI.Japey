using IRI.Sta.Spatial.GeoJsonFormat;

namespace IRI.Extensions;

public static class GeoJsonExtensions
{
    public static GeoJsonFeature AsFeature(this IGeoJsonGeometry geometry)
    {
        return GeoJsonFeature.Create(geometry);
    }

    public static GeoJsonFeatureSet AsFeatureSet(this IGeoJsonGeometry geometry)
    {
        return new GeoJsonFeatureSet() { Features = new List<GeoJsonFeature>() { geometry.AsFeature() }, TotalFeatures = 1 };
    }
}
