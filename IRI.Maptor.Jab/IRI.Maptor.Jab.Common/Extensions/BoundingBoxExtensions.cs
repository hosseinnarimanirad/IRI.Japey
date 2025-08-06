using System.Windows;
using IRI.Maptor.Sta.Common.Primitives;

namespace IRI.Extensions;

public static class BoundingBoxExtensions
{
    public static Rect AsRect(this BoundingBox boundingBox)
    {
        return new Rect(boundingBox.BottomRight.AsWpfPoint(), boundingBox.TopLeft.AsWpfPoint());
    }
}
