﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRI.Standards.OGC.SFA
{
    public interface IOgcGeometryCollection : IEnumerable<IOgcGeometry>
    {
        IOgcGeometry this[int index] { get; set; }

        int Count { get; }
    }
}
