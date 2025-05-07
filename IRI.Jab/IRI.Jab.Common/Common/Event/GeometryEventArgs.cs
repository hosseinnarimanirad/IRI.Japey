using IRI.Sta.Spatial.Primitives; using IRI.Sta.Common.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geometry = IRI.Sta.Spatial.Primitives.Geometry<IRI.Sta.Common.Primitives.Point>;


namespace IRI.Jab.Common;

public class GeometryEventArgs : EventArgs
{
    public Geometry Geometry { get; set; }

    public GeometryEventArgs(Geometry geometry)
    {
        this.Geometry = geometry;
    }
}
