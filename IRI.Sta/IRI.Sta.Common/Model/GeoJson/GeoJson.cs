using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRI.Sta.Common.Model.GeoJson
{
    public static class GeoJson
    {
        public static IEnumerable<GeoJsonFeature> ReadFeatures(string fileName)
        {
            var geoJsonString = System.IO.File.ReadAllText(fileName);
             
            var parsedObject = Newtonsoft.Json.Linq.JObject.Parse(geoJsonString);

            return parsedObject["features"].Select(f => JsonConvert.DeserializeObject<GeoJsonFeature>(f.ToString()));
        }

        public static GeoJsonFeatureCollection ParseToGeoJsonFeatureCollection(string geoJsonFeatures)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<GeoJsonFeatureCollection>(geoJsonFeatures);
        }

        public static IEnumerable<GeoJsonFeature> ParseToGeoJsonFeatures(string geoJsonFeature)
        {
            return ParseToGeoJsonFeatureCollection(geoJsonFeature).features;
        }

    }
}
