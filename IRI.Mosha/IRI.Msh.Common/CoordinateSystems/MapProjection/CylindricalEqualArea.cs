using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IRI.Msh.Common.Primitives;
using IRI.Msh.Common.Primitives;
using Ellipsoid = IRI.Sta.CoordinateSystem.Ellipsoid<IRI.Sta.MeasurementUnit.Meter, IRI.Sta.MeasurementUnit.Degree>;

namespace IRI.Sta.CoordinateSystem.MapProjection
{
    public class CylindricalEqualArea : MapProjectionBase
    {
        public override MapProjectionType Type
        {
            get
            {
                return MapProjectionType.CylindricalEqualArea;
            }
        }

        public CylindricalEqualArea() : this(Ellipsoids.WGS84)
        {
        }

        public CylindricalEqualArea(Ellipsoid ellipsoid) : this(string.Empty, ellipsoid)
        {
        }

        public CylindricalEqualArea(string title, Ellipsoid ellipsoid, int srid = 0) : base(title, ellipsoid, srid)
        {

        }

        public override IPoint FromGeodetic(IPoint point)
        {
            return MapProjects.GeodeticToCylindricalEqualArea(point, this._ellipsoid);
        }

        public override IPoint ToGeodetic(IPoint point)
        {
            return MapProjects.CylindricalEqualAreaToGeodetic(point, this._ellipsoid);
        }

        protected override int GetSrid()
        {
            return _srid;
        }
    }
}
