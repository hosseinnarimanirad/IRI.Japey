using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IRI.Ham.SpatialBase;
using Ellipsoid = IRI.Ham.CoordinateSystem.Ellipsoid<IRI.Ham.MeasurementUnit.Meter, IRI.Ham.MeasurementUnit.Degree>;

namespace IRI.Ham.CoordinateSystem.MapProjection
{
    public class NoProjection : MapProjectionBase
    {
        public override MapProjectionType Type
        {
            get
            {
                return MapProjectionType.None;
            }
        }

        public NoProjection() : this(Ellipsoids.WGS84)
        {
        }

        public NoProjection(Ellipsoid ellipsoid) : this(string.Empty, ellipsoid)
        {
        }

        public NoProjection(string title, Ellipsoid ellipsoid) : base(title, ellipsoid)
        {

        }

        public override IPoint FromGeodetic(IPoint point)
        {
            return point;
        }

        public override IPoint ToGeodetic(IPoint point)
        {
            return point;
        }
    }
}
