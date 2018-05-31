using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Types;

namespace IRI.Ket.SqlServerSpatialExtension.Model
{
    public class SqlFeature : ISqlGeometryAware
    {
        private SqlGeometry _geometry;

        public SqlGeometry Geometry
        {
            get
            {
                return _geometry;
            }

            set
            {
                _geometry = value;
            }
        }

        private Dictionary<string, object> _attributes;

        public Dictionary<string, object> Attributes
        {
            get { return _attributes; }
            set { _attributes = value; }
        }

        public override string ToString()
        {
            return $"Geometry: {Geometry?.STGeometryType()}, Attributes: {Attributes?.Count}";
        }
    }
}
