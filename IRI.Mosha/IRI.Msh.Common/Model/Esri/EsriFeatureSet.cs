using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRI.Msh.Common.Model.Esri
{
    public class EsriFeatureSet
    {
        public SpatialReference SpatialReference { get; set; }

        public Field[] Fields { get; set; }

        public EsriFeature[] Features { get; set; }

        //public static explicit operator EsriFeatureSet(FeatureSet featureSet)
        //{
        //    var result = new EsriFeatureSet();

        //    result.Fields = featureSet.Fields.ToArray();

        //    result.Features = featureSet.Features.Select(f => new EsriFeature()
        //    {
        //        Attributes = f.Attributes.ToDictionary(i => i.Key, i => i.Value == null ? string.Empty : i.Value.ToString()),
        //        Geometry = f.Geometry.ParseToEsriJsonGeometry()
        //    }).ToArray();

        //    return result;
        //}
    }
}
