//using IRI.Ket.Persistence.DataSources;
//using IRI.Sta.Common.Primitives;
//using IRI.Sta.Common.Helpers;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace IRI.Ket.WebApiPersistence;

//public class WebApiDataSource : VectorDataSource<Feature<Point>, Point>
//{
//    public override int Srid { get => throw new NotImplementedException(); protected set => throw new NotImplementedException(); }

//    private readonly string _url;

//    private MemoryDataSource _memoryDataSource;

//    protected MemoryDataSource MemoryDataSource
//    {
//        get
//        {
//            if (_memoryDataSource == null)
//            {
//                _memoryDataSource = GetData();
//            }

//            return _memoryDataSource;
//        }
//    }

//    public WebApiDataSource(string url)
//    {
//        _url = url;
//    }

//    private void GetData()
//    {
//        NetHelper.HttpGet(_url);
//    }

//    public override FeatureSet<Point> GetAsFeatureSetOfPoint(BoundingBox boundingBox)
//    {
//        throw new NotImplementedException();
//    }

//    public override FeatureSet<Point> GetAsFeatureSetOfPoint(Geometry<Point>? geometry)
//    {
//        throw new NotImplementedException();
//    }

//    public override List<Feature<Point>> GetGeometryAwares(BoundingBox boundingBox)
//    {
//        throw new NotImplementedException();
//    }

//    public override List<Feature<Point>> GetGeometryAwares(Geometry<Point>? geometry)
//    {
//        return GetAsFeatureSetOfPoint(geometry).Features;
//    }

//    public override FeatureSet<Point> Search(string searchText)
//    {
//        throw new NotImplementedException();
//    }

//    protected override Feature<Point> ToFeatureMappingFunc(Feature<Point> geometryAware)
//    {
//        return geometryAware;
//    }
//}
