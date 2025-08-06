//using IRI.Maptor.Sta.Common.Primitives.Esri;
//

//namespace IRI.Extensions
//{
//    public static class EsriExtensions
//    {
//        public static EsriFeatureSet AsEsriFeatureSet(this SqlFeatureSet featureSet)
//        {
//            var result = new EsriFeatureSet();

//            result.Fields = featureSet.Fields.ToArray();

//            result.Features = featureSet.Features.Select(f => new EsriFeature()
//            {
//                Attributes = f.Attributes.ToDictionary(i => i.Key, i => i.Value == null ? string.Empty : i.Value.ToString()),
//                Geometry = f.TheSqlGeometry.AsEsriJsonGeometry()
//            }).ToArray();

//            return result;
//        }
//    }
//}
