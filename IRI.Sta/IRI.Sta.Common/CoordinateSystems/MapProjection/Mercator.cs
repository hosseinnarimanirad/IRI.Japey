using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IRI.Ham.MeasurementUnit;
using IRI.Ham.SpatialBase;
using Ellipsoid = IRI.Ham.CoordinateSystem.Ellipsoid<IRI.Ham.MeasurementUnit.Meter, IRI.Ham.MeasurementUnit.Degree>;

namespace IRI.Ham.CoordinateSystem.MapProjection
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

        public Mercator(Ellipsoid ellipsoid) : base(string.Empty, ellipsoid)
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

    }
}
