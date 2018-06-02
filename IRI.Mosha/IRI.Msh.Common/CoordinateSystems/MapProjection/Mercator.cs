using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IRI.Sta.MeasurementUnit;
using IRI.Msh.Common.Primitives;
using Ellipsoid = IRI.Sta.CoordinateSystem.Ellipsoid<IRI.Sta.MeasurementUnit.Meter, IRI.Sta.MeasurementUnit.Degree>;
using IRI.Msh.Common.Primitives;

namespace IRI.Sta.CoordinateSystem.MapProjection
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
