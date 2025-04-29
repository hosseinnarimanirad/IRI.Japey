// besmellahe rahmane rahim
// Allahoma ajjel le-valiyek al-faraj

using System;
using IRI.Msh.MeasurementUnit;

namespace IRI.Msh.CoordinateSystem;

public interface IPolarPoint
{
    AngularUnit Angle { get; set; }

    LinearUnit Radius { get; set; }

    AngleMode AngularMode { get; }

    LinearMode LinearMode { get; }

    Cartesian2DPoint<T> ToCartesian<T>() where T : LinearUnit, new();
}
