// besmellahe rahmane rahim
// Allahoma ajjel le-valiyek al-faraj

using System;
using IRI.Msh.MeasurementUnit;
using System.Collections.Generic;
using IRI.Msh.Algebra;

namespace IRI.Msh.CoordinateSystem;

public interface IEllipsoidal : IEnumerable<IEllipsoidalPoint>
{
    AngleMode AngularMode { get; }
    IEllipsoid Datum { get; }
    int Dimension { get; }
    AxisType Handedness { get; }
    string Name { get; }
    int NumberOfPoints { get; }
    AngleRange HorizontalRange { get; set; }
    IAngularCollection HorizontalAngle { get; }
    IAngularCollection VerticalAngle { get; }

    IEllipsoidal RotateAboutX(IRI.Msh.MeasurementUnit.AngularUnit value, RotateDirection direction);
    IEllipsoidal RotateAboutY(IRI.Msh.MeasurementUnit.AngularUnit value, RotateDirection direction);
    IEllipsoidal RotateAboutZ(IRI.Msh.MeasurementUnit.AngularUnit value, RotateDirection direction);
    IEllipsoidal Shift(ISphericalPoint newBase);
    IEllipsoidalPoint this[int index] { get; set; }
    Cartesian3D<T> ToCartesian<T>() where T : LinearUnit, new();
    Ellipsoidal<TNewLinear, TNewAngular> ChangeTo<TNewLinear, TNewAngular>()
        where TNewLinear : LinearUnit, new()
        where TNewAngular : AngularUnit, new();
    
    Matrix ToMatrix();
    IEllipsoidal Clone();

    
}
