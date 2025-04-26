using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IRI.Sta.Common.Primitives;
using Ellipsoid = IRI.Msh.CoordinateSystem.Ellipsoid<IRI.Msh.MeasurementUnit.Meter, IRI.Msh.MeasurementUnit.Degree>;

namespace IRI.Msh.CoordinateSystem.MapProjection
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

        public override TPoint FromGeodetic<TPoint>(TPoint point)
        {
            return MapProjects.GeodeticToCylindricalEqualArea(point, this._ellipsoid);
        }

        public override TPoint ToGeodetic<TPoint>(TPoint point) 
        {
            return MapProjects.CylindricalEqualAreaToGeodetic(point, this._ellipsoid);
        }

        protected override int GetSrid()
        {
            return _srid;
        }
    }
}
