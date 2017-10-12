using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Ket.Common.Model
{
    [DataContract]
    public class GeoJsonGeometry
    {

        [DataMember(Name = "coordinates")]
        public double[][] Points { get; set; }

        private void ShouldSerializePoints()
        {

        }

        //polygon
        [DataMember(Name = "coordinates")]
        public double[][][] Rings { get; set; }

        //polyline
        [DataMember(Name = "coordinates")]
        public double[][][] Paths { get; set; }

    }
}
