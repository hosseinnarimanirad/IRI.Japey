using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IRI.Ham.SpatialBase;
using Ellipsoid = IRI.Ham.CoordinateSystem.Ellipsoid<IRI.Ham.MeasurementUnit.Meter, IRI.Ham.MeasurementUnit.Degree>;

namespace IRI.Ham.CoordinateSystem.MapProjection
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

        public CylindricalEqualArea(string title, Ellipsoid ellipsoid) : base(title, ellipsoid)
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
    }
}
