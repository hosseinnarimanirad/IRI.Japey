﻿using IRI.Msh.Common.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace IRI.Msh.Common.Primitives;

public interface IFeatureSet<TGeometryAware, TPoint>
    where TGeometryAware : IGeometryAware<TPoint>
    where TPoint : IPoint, new()
{
    public string Title { get; set; }

    public int Srid { get; set; }

    public List<Field> Fields { get; set; }

    public List<TGeometryAware> Features { get; set; }
}