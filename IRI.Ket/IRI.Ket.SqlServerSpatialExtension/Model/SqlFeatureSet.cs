using IRI.Msh.Common.Model;
using Microsoft.SqlServer.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Ket.SqlServerSpatialExtension.Model
{
    public class SqlFeatureSet
    {
        public string Title { get; set; }

        public int Srid { get; set; }

        public List<Field> Fields { get; set; }

        public List<SqlFeature> Features { get; set; }

        public SqlFeatureSet(IEnumerable<SqlGeometry> features)
        {
            this.Features = features.Select(i => new SqlFeature(i)).ToList();

            this.Fields = new List<Field>();
        }

        public SqlFeatureSet()
        {

        }
    }

    public class SqlFeatureSet<T> where T : ISqlGeometryAware
    {
        public string Title { get; set; }

        public int Srid { get; set; }

        public List<Field> Fields { get; set; }

        public List<T> Features { get; set; }

        public SqlFeatureSet(List<T> features)
        {
            this.Features = features;

            this.Fields = new List<Field>();
        }

        public SqlFeatureSet()
        {

        }
    }
}
