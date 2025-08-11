//using IRI.Maptor.Sta.Common.Primitives;

//namespace IRI.Maptor.Sta.Spatial.Primitives;

//public class Feature : Feature<Point>
//{
//    public Feature(Geometry<Point> geometry) : this(geometry, new Dictionary<string, object>())
//    {

//    }

//    public Feature(Geometry<Point> geometry, string label) : this(geometry, new Dictionary<string, object>() { { _defaultLabelAttributeName, label } })
//    {

//    }

//    public Feature(Geometry<Point> geometry, Dictionary<string, object> attributes) : base(geometry, attributes)
//    {
//    }

//    public Feature()
//    {

//    }

//    //public GeoJsonFeature AsGeoJsonFeature(Func<Point, Point> toWgs84Func, bool isLongitudeFirst)
//    //{
//    //    return new GeoJsonFeature()
//    //    {
//    //        Geometry = this.TheGeometry.Transform(toWgs84Func, SridHelper.GeodeticWGS84).AsGeoJson(isLongitudeFirst),
//    //        Id = this.Id.ToString(),
//    //        Properties = this.Attributes/*.ToDictionary(k => k.Key, k => k.Value)*/,

//    //    };
//    //}
//}