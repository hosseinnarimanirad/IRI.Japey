using IRI.Msh.Common.Primitives;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Msh.Common.Model.GeoJson
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

            GeometryType geometryType;

            if (!Enum.TryParse<GeometryType>(type, out geometryType))
            {
                return null;
            }

            switch (geometryType)
            {
                case GeometryType.Point:
                    return new GeoJsonPoint() { Type = type, Coordinates = jObject[coordinateKey].ToObject<double[]>() };  

                case GeometryType.MultiPoint:
                    return new GeoJsonMultiPoint() { Type = type, Coordinates = jObject[coordinateKey].ToObject<double[][]>() };   

                case GeometryType.LineString:
                    return new GeoJsonLineString() { Type = type, Coordinates = jObject[coordinateKey].ToObject<double[][]>() };

                case GeometryType.MultiLineString:
                    return new GeoJsonMultiLineString() { Type = type, Coordinates = jObject[coordinateKey].ToObject<double[][][]>() };                    

                case GeometryType.Polygon:
                    return new GeoJsonPolygon() { Type = type, Coordinates = jObject[coordinateKey].ToObject<double[][][]>() }; 

                case GeometryType.MultiPolygon:
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
