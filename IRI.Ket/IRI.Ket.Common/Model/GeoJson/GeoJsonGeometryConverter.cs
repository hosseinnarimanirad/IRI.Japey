using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Ket.Common.Model.GeoJson
{
    public class GeoJsonGeometryConverter : JsonConverter
    {
        private const string typeKey = "type";

        private const string coordinateKey = "coordinates";

        public override bool CanConvert(Type objectType)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jObject = JObject.Load(reader);

            var type = jObject[typeKey]?.ToString();

            GeoJsonType geometryType;

            if (!Enum.TryParse<GeoJsonType>(type, out geometryType))
            {
                return null;
            }

            switch (geometryType)
            {
                case GeoJsonType.Point:
                    return new GeoJsonPoint() { Type = type, Coordinates = jObject[coordinateKey].ToObject<double[]>() };  

                case GeoJsonType.MultiPoint:
                    return new GeoJsonMultiPoint() { Type = type, Coordinates = jObject[coordinateKey].ToObject<double[][]>() };   

                case GeoJsonType.LineString:
                    return new GeoJsonLineString() { Type = type, Coordinates = jObject[coordinateKey].ToObject<double[][]>() };

                case GeoJsonType.MultiLineString:
                    return new GeoJsonMultiLineString() { Type = type, Coordinates = jObject[coordinateKey].ToObject<double[][][]>() };                    

                case GeoJsonType.Polygon:
                    return new GeoJsonPolygon() { Type = type, Coordinates = jObject[coordinateKey].ToObject<double[][][]>() }; 

                case GeoJsonType.MultiPolygon:
                    return new GeoJsonMultiPolygon() { Type = type, Coordinates = jObject[coordinateKey].ToObject<double[][][][]>() };
                   
                default:
                    throw new NotImplementedException();
            }

        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
