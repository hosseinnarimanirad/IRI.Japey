using System;
using System.Collections.Generic;
using System.Text;

namespace IRI.Msh.Common.Primitives
{
    public class Feature<T> : IGeometryAware<T> where T : IPoint, new()
    {
        public int Id { get; set; }

        public Geometry<T> TheGeometry { get; set; }

        public Dictionary<string, object> Attributes { get; set; }
    }
}
