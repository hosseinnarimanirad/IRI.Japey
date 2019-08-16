using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace IRI.Standards.OGC.SFA
{
    public interface ILinearRing : IEnumerable<IPoint>
    {
        byte[] ToBinary();

        IPointCollection Points { get; }
    }
}
