using System;
using System.Collections.Generic;
using System.Text;

namespace IRI.Msh.Common.Primitives
{
    public class Feature : IGeometryAware
    {
        public int Id { get; set; }

        public Geometry TheGeometry { get; set; }

        public Dictionary<string, object> Attributes { get; set; }
    }
}
    