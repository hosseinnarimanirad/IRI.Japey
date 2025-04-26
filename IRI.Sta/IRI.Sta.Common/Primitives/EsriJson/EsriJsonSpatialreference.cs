using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Sta.Common.Primitives.Esri
{
    //[DataContract]
    [JsonObject]
    public class EsriJsonSpatialreference
    {
        //[DataMember(Name = "wkid")]
        [JsonProperty("wkid")]
        public int Wkid { get; set; }

        //[DataMember(Name = "latestWkid")]
        [JsonProperty("latestWkid")]
        public int? LatestWkid { get; set; }

        //[DataMember(Name = "vcsWkid")]
        [JsonProperty("vcsWkid")]
        public int? VcsWkid { get; set; }

        //[DataMember(Name = "latestVcsWkid")]
        [JsonProperty("latestVcsWkid")]
        public int? LatestVcsWkid { get; set; }
    }
}
