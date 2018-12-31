using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace IRI.Standards.OGC.SFA
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct PointZM : IPoint
    {
        double x;

        double y;

        double z;

        double m;

        public double X { get { return this.x; } }

        public double Y { get { return this.y; } }

        public double Z { get { return this.z; } }

        public double M { get { return this.m; } }

        public PointZM(double x, double y, double z, double m)
        {
            this.x = x;

            this.y = y;

            this.z = z;

            this.m = m;
        }
    }
}
