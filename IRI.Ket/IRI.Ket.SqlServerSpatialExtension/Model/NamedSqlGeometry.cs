using Microsoft.SqlServer.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Ket.SqlServerSpatialExtension.Model
{
    public class NamedSqlGeometry
    {
        public SqlGeometry Geometry { get; set; }

        public string Label { get; set; }

        public NamedSqlGeometry(SqlGeometry geometry, string label)
        {
            this.Geometry = geometry;

            this.Label = label;
        }
    }
}
