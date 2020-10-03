using IRI.Msh.Common.Primitives;
using Ellipsoid = IRI.Msh.CoordinateSystem.Ellipsoid<IRI.Msh.MeasurementUnit.Meter, IRI.Msh.MeasurementUnit.Degree>;

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

        public override TPoint FromGeodetic<TPoint>(TPoint point)
        {
            return MapProjects.GeodeticToMercator(point, this._ellipsoid);
        }

        public override TPoint ToGeodetic<TPoint>(TPoint point)
        {
            return MapProjects.MercatorToGeodetic(point, this._ellipsoid);
        }

        protected override int GetSrid()
        {
            return _srid;
        }
    }
}
