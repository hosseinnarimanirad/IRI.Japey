using System;
using System.Collections.Generic; 

namespace IRI.Maptor.Jab.Common;

public class PointArrayEventArgs : EventArgs
{
    public List<IRI.Maptor.Sta.Common.Primitives.Point> Coordinates { get; set; }

    public bool IsClosed { get; set; }

    public PointArrayEventArgs(List<IRI.Maptor.Sta.Common.Primitives.Point> coordinates, bool isClosed)
    {
        this.Coordinates = coordinates;

        this.IsClosed = isClosed;
    }
}
