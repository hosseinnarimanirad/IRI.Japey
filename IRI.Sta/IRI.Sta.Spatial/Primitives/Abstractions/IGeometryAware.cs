using IRI.Sta.Common.Abstrations;
using IRI.Sta.Common.Primitives;
using IRI.Sta.Spatial.Primitives;
using System;
using System.Collections.Generic;
using System.Text;

namespace IRI.Sta.Spatial.Primitives;

public interface IGeometryAware<T> : IIdentifiable where T : IPoint, new()
{
    Geometry<T> TheGeometry { get; set; }
}
