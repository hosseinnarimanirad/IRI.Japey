using System;
namespace IRI.Ham.SpatialBase
{
    public interface IPoint
    {
        double X { get; set; }
        double Y { get; set; }

        bool AreExactlyTheSame(object obj);

        double DistanceTo(IPoint point);

        byte[] AsWkb();
    }
}
