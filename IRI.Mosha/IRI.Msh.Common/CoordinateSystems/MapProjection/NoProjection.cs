using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IRI.Msh.Common.Primitives;
using Ellipsoid = IRI.Sta.CoordinateSystem.Ellipsoid<IRI.Sta.MeasurementUnit.Meter, IRI.Sta.MeasurementUnit.Degree>;

namespace IRI.Sta.CoordinateSystem.MapProjection
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

        public NoProjection(string title, Ellipsoid ellipsoid) : base(title, ellipsoid, ellipsoid.Srid)
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

        protected override int GetSrid()
        {
            return Ellipsoid.Srid;
        }
    }
}
