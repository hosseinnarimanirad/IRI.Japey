﻿using System;
using System.Collections.Generic;
using System.Text;

namespace IRI.Msh.Common.Primitives
{
    public interface IGeometryAware
    {
        Geometry TheGeometry { get; set; }
    }
}
