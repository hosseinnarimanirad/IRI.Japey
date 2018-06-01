using System;
using System.Collections.Generic;
using System.Text;

namespace IRI.Sta.Common.Primitives
{
    public interface IGeometryAware
    {
        Geometry Geometry { get; set; }
    }
}
