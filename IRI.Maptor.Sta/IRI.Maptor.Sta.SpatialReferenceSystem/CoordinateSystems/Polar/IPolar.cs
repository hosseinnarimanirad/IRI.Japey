// besmellahe rahmane rahim
// Allahoma ajjel le-valiyek al-faraj

using System;
using IRI.Maptor.Sta.Metrics;
using System.Collections.Generic;
using IRI.Maptor.Sta.Mathematics;
using IRI.Maptor.Sta.SpatialReferenceSystem;

namespace IRI.Maptor.Sta.SpatialReferenceSystem;

public interface IPolar : IEnumerable<IPolarPoint>
{

    int Dimension { get; }
    AxisType Handedness { get; }
    LinearMode LinearMode { get; }
    string Name { get; }
    int NumberOfPoints { get; }
    AngleMode AngularMode { get; }
    AngleRange AngularRange { get; set; }
    ILinearCollection Radius { get; }
    IAngularCollection Angle { get; }

    IPolarPoint this[int index] { get; set; }
    
    IPolar Rotate(AngularUnit value, RotateDirection direction);
    IPolar Shift(IPolarPoint newBase);
    Matrix ToMatrix();
    IPolar Clone();

    Cartesian2D<T> ToCartesian<T>() where T : LinearUnit, new();
    Polar<TNewLinear, TNewAngular> ChangeTo<TNewLinear, TNewAngular>()
        where TNewLinear : LinearUnit, new()
        where TNewAngular : AngularUnit, new();
}
