// besmellahe rahmane rahim
// Allahoma ajjel le-valiyek al-faraj

using System;
using IRI.Sta.Metrics;

namespace IRI.Sta.SpatialReferenceSystem;

public interface ISphericalPoint
{
    AngleMode AngularMode { get; }

    AngularUnit HorizontalAngle { get; set; }

    AngleRange HorizontalRange { get; set; }

    LinearMode LinearMode { get; }

    LinearUnit Radius { get; set; }

    Cartesian3DPoint<T> ToCartesian<T>() where T : LinearUnit, new();

    AngularUnit VerticalAngle { get; set; }
}
