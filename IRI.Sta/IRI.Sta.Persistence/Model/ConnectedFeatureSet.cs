//using System;
//using System.Collections.Generic;

//using IRI.Sta.Common.Primitives;
//using IRI.Sta.Spatial.Primitives;
//using IRI.Sta.Persistence.DataSources;


//namespace IRI.Sta.Persistence.Model;

////SqlFeatureSet which is connected to its source. For Add, Remove, Update actions
////This class was used first for FFSDB editing tool
//public class ConnectedFeatureSet<TGeometryAware> : IFeatureSet<TGeometryAware, Point> where TGeometryAware : class, IGeometryAware<Point>
//{
//    public VectorDataSource/*<TGeometryAware>*/ DataSource { get; set; }
//    public string Title { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
//    public int Srid { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
//    public List<Field> Fields { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
//    public List<TGeometryAware> Features { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

//    public ConnectedFeatureSet(List<TGeometryAware> features, VectorDataSource/*<TGeometryAware>*/ dataSource, string title)
//    {
//        Features = features;

//        DataSource = dataSource;

//        Title = title;
//    }
//}
