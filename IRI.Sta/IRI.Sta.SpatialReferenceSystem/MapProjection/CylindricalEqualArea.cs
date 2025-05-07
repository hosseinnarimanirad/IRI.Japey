using Ellipsoid = IRI.Sta.CoordinateSystems.Ellipsoid<IRI.Sta.Metrics.Meter, IRI.Sta.Metrics.Degree>;

namespace IRI.Sta.CoordinateSystems.MapProjection;

public class CylindricalEqualArea : MapProjectionBase
{
    public override SpatialReferenceType Type
    {
        get
        {
            return SpatialReferenceType.CylindricalEqualArea;
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
