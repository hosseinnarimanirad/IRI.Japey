using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IRI.Msh.Common.Model;
using IRI.Msh.Common.Primitives;
using IRI.Ket.Persistence.DataSources;


namespace IRI.Ket.Persistence.Model
{
    //SqlFeatureSet which is connected to its source. For Add, Remove, Update actions
    //This class was used first for FFSDB editing tool
    public class ConnectedSqlFeatureSet<TGeometryAware> : IFeatureSet<TGeometryAware, Point> where TGeometryAware : class, IGeometryAware<Point>
    {
        public VectorDataSource<TGeometryAware, Point> DataSource { get; set; }
        public string Title { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int Srid { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public List<Field> Fields { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public List<TGeometryAware> Features { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public ConnectedSqlFeatureSet(List<TGeometryAware> features, VectorDataSource<TGeometryAware, Point> dataSource, string title)  
        {
            this.Features = features;
            
            this.DataSource = dataSource;

            this.Title = title;
        }
    }
}
