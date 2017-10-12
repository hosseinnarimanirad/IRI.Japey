using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace IRI.Standards.OGC.SFA
{
    public interface IPoint
    {
        double X { get; }

        double Y { get; }
    }
}
