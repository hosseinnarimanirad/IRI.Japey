using System;
using System.Collections.Generic;
using System.Text;

namespace IRI.Msh.Common.Primitives
{
    public interface IGeometryAware
    {
        int Id { get; set; }

        Geometry TheGeometry { get; set; }
    }
}
