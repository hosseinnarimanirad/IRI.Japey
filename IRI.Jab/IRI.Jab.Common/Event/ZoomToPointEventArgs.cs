using IRI.Ham.SpatialBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Jab.Common
{
    public class ZoomToPointEventArgs : EventArgs
    {
        public double MapScale { get; set; }

        public Point Center { get; set; }

        public ZoomToPointEventArgs(double mapScale, Point center)
        {
            this.MapScale = mapScale;

            this.Center = center;
        }
    }
}
