﻿// besmellahe rahmane rahim
// Allahoma ajjel le-valiyek al-faraj

using System;
using IRI.Msh.MeasurementUnit;
using System.Collections.Generic;

namespace IRI.Msh.CoordinateSystem
{
    public interface IEllipsoidalPoint
    {
        IEllipsoid Datum { get; }

        AngleMode AngularMode { get; }

        AngularUnit VerticalAngle { get; set; }

        AngularUnit HorizontalAngle { get; set; }

        AngleRange HorizontalRange { get; set; }

        Cartesian3DPoint<T> ToCartesian<T>() where T : LinearUnit, new();
    }
}
