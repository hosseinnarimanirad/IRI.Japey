using IRI.Ket.DataManagement.DataSource; 
using IRI.Msh.Common.Primitives; 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Ket.DataManagement.Model
{
    //SqlFeatureSet which is connected to its source. For Add, Remove, Update actions
    //This class was used first for FFSDB editing tool
    public class ConnectedSqlFeatureSet<T> : FeatureSet<T> where T : class, IGeometryAware<Point>
    {
        public FeatureDataSource<T> DataSource { get; set; }

        public ConnectedSqlFeatureSet(List<T> features, FeatureDataSource<T> dataSource, string title) : base(features)
        {
            this.DataSource = dataSource;

            this.Title = title;
        }
    }
}
