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
        const string _defaultLabelAttributeName = "Label";

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

        public SqlFeature(SqlGeometry geometry)
        {
            this.Geometry = geometry;

            this.Attributes = new Dictionary<string, object>();
        }

        public SqlFeature(SqlGeometry geometry, string label)
        {
            this.Geometry = geometry;

            this.Attributes = new Dictionary<string, object>() { { _defaultLabelAttributeName, label } };
        }

        public SqlFeature()
        {

        }

        public string LabelAttribute { get; set; } = _defaultLabelAttributeName;

        public string Label
        {
            get
            {
                if (Attributes?.Keys.Any(k => k == LabelAttribute) == true)
                {
                    return Attributes[LabelAttribute]?.ToString();
                }

                return string.Empty;
            }
        }
    }
}
