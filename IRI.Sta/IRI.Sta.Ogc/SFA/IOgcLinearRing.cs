﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace IRI.Sta.Ogc.SFA;

public interface IOgcLinearRing : IEnumerable<IOgcPoint>
{
    byte[] ToBinary();

    IOgcPointCollection Points { get; }
}
