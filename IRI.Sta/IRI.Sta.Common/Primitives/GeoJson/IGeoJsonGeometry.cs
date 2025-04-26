
using IRI.Sta.Common.Primitives;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Sta.Common.Model.GeoJson
{
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
}
