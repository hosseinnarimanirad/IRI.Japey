using Microsoft.SqlServer.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Jab.Common.Model
{
    public class GeometryLabelPairs
    {
        public List<SqlGeometry> Geometries { get; set; }

        public List<string> Labels { get; set; }

        public GeometryLabelPairs(List<SqlGeometry> geometries, List<string> labels)
        {
            this.Geometries = geometries;

            this.Labels = labels;
        }

    }
}
