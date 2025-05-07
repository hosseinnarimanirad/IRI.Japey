using IRI.Extensions;
using IRI.Sta.Common.Primitives;
using IRI.Sta.Spatial.Primitives;

namespace IRI.Ket.Persistence.DataSources;

public class OrdinaryJsonListSource<TGeometryAware> : MemoryDataSource<TGeometryAware, Point> where TGeometryAware : class, IGeometryAware<Point>
{
    public override GeometryType? GeometryType
    {
        get; protected set;
    }

    private OrdinaryJsonListSource(List<TGeometryAware> features, Func<TGeometryAware, Feature<Point>> mapToFeatureFunc) : this(features, mapToFeatureFunc, null)
    {

    }

    private OrdinaryJsonListSource(List<TGeometryAware> features, Func<TGeometryAware, Feature<Point>> mapToFeatureFunc, Func<TGeometryAware, string>? labelFunc)
    {
        this._labelFunc = labelFunc;

        this._features = features;

        this.WebMercatorExtent = features.Select(f => f.TheGeometry).GetBoundingBox();

        this._mapToFeatureFunc = mapToFeatureFunc;

        // 1401.11.29
        this.GeometryType = features.First().TheGeometry.Type;
    }

    public override string ToString()
    {
        return $"OrdinaryJsonListSource";
    }

    public static OrdinaryJsonListSource<TGeometryAware> CreateFromJsonString(string jsonString, Func<TGeometryAware, Feature<Point>> mapToFeatureFunc, Func<TGeometryAware, string> labelFunc = null)
    {
        var values = Newtonsoft.Json.JsonConvert.DeserializeObject<List<TGeometryAware>>(jsonString);

        return new OrdinaryJsonListSource<TGeometryAware>(values, mapToFeatureFunc, labelFunc);
    }

    public static OrdinaryJsonListSource<TGeometryAware> CreateFromFile(string fileName, Func<TGeometryAware, Feature<Point>> mapToFeatureFunc, Func<TGeometryAware, string> labelFunc = null)
    {
        var jsonString = System.IO.File.ReadAllText(fileName);

        return CreateFromJsonString(jsonString, mapToFeatureFunc, labelFunc);
    }

    //public static GeoJsonSource<SqlFeature> Create(List<SqlFeature> features)
    //{
    //    return new GeoJsonSource<SqlFeature>(features, f => f);
    //}
}
