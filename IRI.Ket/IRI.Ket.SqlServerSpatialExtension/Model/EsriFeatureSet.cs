using IRI.Ket.SpatialExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Ket.SqlServerSpatialExtension.Model
{
    public class EsriFeatureSet
    { 
        public SpatialReference SpatialReference { get; set; }
         
        public Field[] Fields { get; set; }

        public EsriFeature[] Features { get; set; }

        public static explicit operator EsriFeatureSet(FeatureSet featureSet)
        {
            var result = new EsriFeatureSet();

            result.Fields = featureSet.Fields.ToArray();

            result.Features = featureSet.Features.Select(f => new EsriFeature()
            {
                Attributes = f.Attributes.ToDictionary(i => i.Key, i => i.Value == null ? string.Empty : i.Value.ToString()),
                Geometry = f.Geometry.ParseToEsriJsonGeometry()
            }).ToArray();

            return result;
        }
    }

    public class SpatialReference
    {
        public int wkid { get; set; }
    }



    public class EsriFeature
    {
        public IRI.Ket.Common.Model.Esri.EsriJsonGeometry Geometry { get; set; }

        public Dictionary<string, string> Attributes { get; set; }
    }
}
