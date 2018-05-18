using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRI.Ham.SpatialBase.Model
{
    public class ZoomScale
    {
        public int ZoomLevel { get; private set; }

        public double Scale { get { return 1.0 / InverseScale; } }

        public double InverseScale { get; private set; }

        public ZoomScale(int zoomLevel, double inverseScale)
        {
            this.ZoomLevel = zoomLevel;

            this.InverseScale = inverseScale;
        }

        public override string ToString() => $"Level: {ZoomLevel}; InverseScale: {InverseScale}";
    }
}
