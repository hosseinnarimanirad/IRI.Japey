using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace IRI.Sta.Ogc.SFA
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct OgcPointM : IOgcPoint
    {
        double x;

        double y;

        double m;

        public double X { get { return this.x; } }

        public double Y { get { return this.y; } }

        public double M { get { return this.m; } }

        public OgcPointM(double x, double y, double m)
        {
            this.x = x;

            this.y = y;

            this.m = m;
        }
    }
}
