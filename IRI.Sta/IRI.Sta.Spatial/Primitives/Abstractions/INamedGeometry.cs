using IRI.Sta.Common.Primitives;
using IRI.Sta.Common.Abstrations;
using IRI.Sta.Spatial.Primitives;
using System;
using System.Collections.Generic;
using System.Text;

namespace IRI.Sta.Spatial.Primitives;

public class INamedGeometry<T> : IGeometryAware<T> where T : IPoint, new()
{
    public int Id { get; set; }

    public Geometry<T> TheGeometry { get; set; }

    public string Label { get; set; }

}
