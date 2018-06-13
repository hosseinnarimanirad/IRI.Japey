﻿using System;
namespace IRI.Msh.Common.Primitives
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