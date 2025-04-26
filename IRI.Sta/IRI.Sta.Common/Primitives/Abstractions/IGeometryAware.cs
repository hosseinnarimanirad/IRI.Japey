using System;
using System.Collections.Generic;
using System.Text;

namespace IRI.Sta.Common.Primitives;

public interface IGeometryAware<T> : IIdentifiable where T : IPoint, new()
{
    Geometry<T> TheGeometry { get; set; }
}
