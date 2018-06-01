﻿using System;
using System.Collections.Generic;
using System.Text;

namespace IRI.Sta.Common.Primitives
{
    public class Feature : IGeometryAware
    {
        public Geometry Geometry { get; set; }

        public Dictionary<string, object> Attributes { get; set; }
    }
}
