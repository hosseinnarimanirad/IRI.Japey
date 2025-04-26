using System;
using System.Collections.Generic;
using System.Text;

namespace IRI.Sta.Common.Primitives;

public class INamedGeometry<T> : IGeometryAware<T> where T : IPoint, new()
{
    public int Id { get; set; }

    public Geometry<T> TheGeometry { get; set; }

    public string Label { get; set; }

}
