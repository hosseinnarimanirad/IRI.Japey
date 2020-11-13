using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRI.Msh.Common.Model.GeoJson
{
    public static class GeoJson
    {
        public static IEnumerable<GeoJsonFeature> ReadFeatures(string fileName)
        {
            var geoJsonString = System.IO.File.ReadAllText(fileName);

            var parsedObject = Newtonsoft.Json.Linq.JObject.Parse(geoJsonString);

            return parsedObject["features"].Select(f => JsonConvert.DeserializeObject<GeoJsonFeature>(f.ToString()));
        }

        public static GeoJsonFeatureSet ParseToGeoJsonFeatureCollection(string geoJsonFeatures)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<GeoJsonFeatureSet>(geoJsonFeatures);
        }

        public static IEnumerable<GeoJsonFeature> ParseToGeoJsonFeatures(string geoJsonFeature)
        {
            return ParseToGeoJsonFeatureCollection(geoJsonFeature).features;
        }

        public const string Point = "Point";
        public const string MultiPoint = "MultiPoint";
        public const string LineString = "LineString";
        public const string MultiLineString = "MultiLineString";
        public const string Polygon = "Polygon";
        public const string MultiPolygon = "MultiPolygon";

        internal static string Serialize(IGeoJsonGeometry geoJson, bool indented, bool removeSpaces = false)
        {
            var result = Newtonsoft.Json.JsonConvert.SerializeObject(geoJson, indented ? Formatting.Indented : Formatting.None);

            return removeSpaces ? result.Replace(" ", string.Empty) : result;
        }
    }
}
