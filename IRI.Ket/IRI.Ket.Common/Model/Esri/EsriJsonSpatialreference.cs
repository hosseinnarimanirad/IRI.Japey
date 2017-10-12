using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Ket.Common.Model.Esri
{
    [DataContract]
    public class EsriJsonSpatialreference
    {
        [DataMember(Name = "wkid")]
        public int Wkid { get; set; }

        [DataMember(Name = "latestWkid")]
        public int? LatestWkid { get; set; }

        [DataMember(Name = "vcsWkid")]
        public int? VcsWkid { get; set; }

        [DataMember(Name = "latestVcsWkid")]
        public int? LatestVcsWkid { get; set; }
    }
}
