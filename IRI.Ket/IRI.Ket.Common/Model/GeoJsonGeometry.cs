   
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Ket.Common.Model
{
    [JsonObject]
    public class GeoJsonGeometry
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("coordinates")]
        public Newtonsoft.Json.Linq.JArray Coordinates { get; set; }

        public GeoJsonType GeometryType
        {
            get
            {
                GeoJsonType result;

                if (Enum.TryParse<GeoJsonType>(Type, out result))
                    return result;

                else
                    throw new NotImplementedException();
            }
        }

        [JsonIgnore()]
        public double[][] Points
        {
            get { return Coordinates.ToObject<double[][]>(); }
            set { Coordinates = new Newtonsoft.Json.Linq.JArray(value); }
        }

        //polygon 
        [JsonIgnore()]
        public double[][][] Rings
        {
            get { return Coordinates.ToObject<double[][][]>(); }
            set { Coordinates = new Newtonsoft.Json.Linq.JArray(value); }
        }

        //polyline 
        [JsonIgnore()]
        public double[][][] Paths
        {
            get { return Coordinates.ToObject<double[][][]>(); }
            set { Coordinates = new Newtonsoft.Json.Linq.JArray(value); }
        }

        //multipolygon
        [JsonIgnore()]
        public double[][][][] Polygons
        {
            get { return Coordinates.ToObject<double[][][][]>(); }
            set { Coordinates = new Newtonsoft.Json.Linq.JArray(value); }
        }
 
    }
}
