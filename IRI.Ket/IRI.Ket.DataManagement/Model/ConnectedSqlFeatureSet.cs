using IRI.Ket.DataManagement.DataSource;
using IRI.Ket.SqlServerSpatialExtension.Model;
using Microsoft.SqlServer.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Ket.DataManagement.Model
{
    //SqlFeatureSet which is connected to its source. For Add, Remove, Update actions
    //This class was used first for FFSDB editing tool
    public class ConnectedSqlFeatureSet<T> : SqlFeatureSet<T> where T : class, ISqlGeometryAware
    {
        public FeatureDataSource<T> DataSource { get; set; }

        public ConnectedSqlFeatureSet(List<T> features, FeatureDataSource<T> dataSource, string title) : base(features)
        {
            this.DataSource = dataSource;

            this.Title = title;
        }
    }
}
