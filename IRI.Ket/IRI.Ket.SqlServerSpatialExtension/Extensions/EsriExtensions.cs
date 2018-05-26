using IRI.Sta.Common.Model.Esri;
using IRI.Ket.SpatialExtensions;
using IRI.Ket.SqlServerSpatialExtension.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Ket.SqlServerSpatialExtension.Extensions
{
    public static class EsriExtensions
    {
        public static EsriFeatureSet Parse(this FeatureSet featureSet)
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
}
