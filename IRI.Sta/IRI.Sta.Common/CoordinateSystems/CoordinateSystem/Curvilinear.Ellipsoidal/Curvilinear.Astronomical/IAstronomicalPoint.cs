// besmellahe rahmane rahim
// Allahoma ajjel le-valiyek al-faraj

using System;
using IRI.Sta.Metrics;

namespace IRI.Sta.CoordinateSystems;

public interface IAstronomicalPoint
{
    AngleMode AngularMode { get; }

    AngularUnit HorizontalAngle { get; set; }

    AngleRange HorizontalRange { get; set; }

    Cartesian3DPoint<T> ToCartesian<T>() where T : LinearUnit, new();

    AngularUnit VerticalAngle { get; set; }
}
