using Ellipsoid = IRI.Msh.CoordinateSystem.Ellipsoid<IRI.Msh.MeasurementUnit.Meter, IRI.Msh.MeasurementUnit.Degree>;

namespace IRI.Msh.CoordinateSystem.MapProjection;

public class NoProjection : MapProjectionBase
{
    public override SpatialReferenceType Type
    {
        get
        {
            return SpatialReferenceType.None;
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

    public override TPoint FromGeodetic<TPoint>(TPoint point)
    {
        return point;
    }

    public override TPoint ToGeodetic<TPoint>(TPoint point)
    {
        return point;
    }

    protected override int GetSrid()
    {
        return Ellipsoid.Srid;
    }
}
