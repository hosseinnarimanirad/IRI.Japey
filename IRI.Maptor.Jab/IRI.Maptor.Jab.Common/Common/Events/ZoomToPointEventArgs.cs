using System;

using IRI.Maptor.Sta.Common.Primitives;

namespace IRI.Maptor.Jab.Common;

public class ZoomToPointEventArgs : EventArgs
{
    public double MapScale { get; set; }

    public Point Center { get; set; }

    public ZoomToPointEventArgs(double mapScale, Point center)
    {
        this.MapScale = mapScale;

        this.Center = center;
    }
}
