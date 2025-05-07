using IRI.Sta.Common.Primitives;
using IRI.Sta.Spatial.Primitives; 
using Newtonsoft.Json; 

namespace IRI.Sta.Spatial.Model.GeoJson;

[JsonConverter(typeof(GeoJsonGeometryConverter))]
public interface IGeoJsonGeometry
{
    string Type { get; set; }

    GeometryType GeometryType { get; }

    Geometry<Point> Parse(bool isLongitudeFirst = true, int srid = 0);

    bool IsNullOrEmpty();

    string Serialize(bool indented, bool removeSpaces = false);

    int NumberOfGeometries();

    int NumberOfPoints();

    Geometry<Point> TransformToWeMercator(bool isLongitudeFirst = true);
     
}
