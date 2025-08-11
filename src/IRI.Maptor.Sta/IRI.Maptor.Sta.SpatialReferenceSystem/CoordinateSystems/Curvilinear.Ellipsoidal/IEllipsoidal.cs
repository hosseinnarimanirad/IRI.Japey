﻿// besmellahe rahmane rahim
// Allahoma ajjel le-valiyek al-faraj

using System;
using IRI.Maptor.Sta.Metrics;
using System.Collections.Generic;
using IRI.Maptor.Sta.Mathematics;

namespace IRI.Maptor.Sta.SpatialReferenceSystem;

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

    IEllipsoidal RotateAboutX(IRI.Maptor.Sta.Metrics.AngularUnit value, RotateDirection direction);
    IEllipsoidal RotateAboutY(IRI.Maptor.Sta.Metrics.AngularUnit value, RotateDirection direction);
    IEllipsoidal RotateAboutZ(IRI.Maptor.Sta.Metrics.AngularUnit value, RotateDirection direction);
    IEllipsoidal Shift(ISphericalPoint newBase);
    IEllipsoidalPoint this[int index] { get; set; }
    Cartesian3D<T> ToCartesian<T>() where T : LinearUnit, new();
    Ellipsoidal<TNewLinear, TNewAngular> ChangeTo<TNewLinear, TNewAngular>()
        where TNewLinear : LinearUnit, new()
        where TNewAngular : AngularUnit, new();
    
    Matrix ToMatrix();
    IEllipsoidal Clone();

    
}
