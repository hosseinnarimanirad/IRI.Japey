using System;
using System.Collections.Generic;
using System.Text;

namespace IRI.Msh.Common.Primitives
{
    public class NamedGeometry<T> : IGeometryAware<T> where T : IPoint, new()
    {
        public int Id { get; set; }

        public Geometry<T> TheGeometry { get; set; }

        public string Label { get; set; }

        public NamedGeometry(Geometry<T> geometry, string label)
        {
            this.TheGeometry = geometry;

            this.Label = label;
        }
    }
}
