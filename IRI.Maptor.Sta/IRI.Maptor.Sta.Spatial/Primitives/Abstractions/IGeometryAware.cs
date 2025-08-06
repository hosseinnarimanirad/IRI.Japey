using IRI.Maptor.Sta.Common.Abstrations;
using IRI.Maptor.Sta.Common.Primitives;
using IRI.Maptor.Sta.Spatial.Primitives;
using System;
using System.Collections.Generic;
using System.Text;

namespace IRI.Maptor.Sta.Spatial.Primitives;

public interface IGeometryAware<T> : IIdentifiable where T : IPoint, new()
{
    Geometry<T> TheGeometry { get; set; }
}
