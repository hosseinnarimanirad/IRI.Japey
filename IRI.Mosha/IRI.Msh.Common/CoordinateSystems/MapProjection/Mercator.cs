using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IRI.Msh.MeasurementUnit;
using IRI.Msh.Common.Primitives;
using Ellipsoid = IRI.Msh.CoordinateSystem.Ellipsoid<IRI.Msh.MeasurementUnit.Meter, IRI.Msh.MeasurementUnit.Degree>;
using IRI.Msh.Common.Primitives;

namespace IRI.Msh.CoordinateSystem.MapProjection
{
    public class Mercator : MapProjectionBase
    {
        public override MapProjectionType Type
        {
            get
            {
                return MapProjectionType.Mercator;
            }
        }

        public Mercator() : this(Ellipsoids.WGS84)
        {
        }

        public Mercator(Ellipsoid ellipsoid, int srid = 0) : base(string.Empty, ellipsoid, srid)
        {
        }

        public override IPoint FromGeodetic(IPoint point)
        {
            return MapProjects.GeodeticToMercator(point, this._ellipsoid);
        }

        public override IPoint ToGeodetic(IPoint point)
        {
            return MapProjects.MercatorToGeodetic(point, this._ellipsoid);
        }

        protected override int GetSrid()
        {
            return _srid;
        }
    }
}
