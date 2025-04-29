using Ellipsoid = IRI.Msh.CoordinateSystem.Ellipsoid<IRI.Msh.MeasurementUnit.Meter, IRI.Msh.MeasurementUnit.Degree>;

namespace IRI.Msh.CoordinateSystem.MapProjection;

public abstract class MapProjectionBase : SrsBase
{
    protected double _latitudeOfOrigin, _standardParallel1, _standardParallel2, _centralMeridian, _falseEasting, _falseNorthing;

    protected double _scaleFactor = 1.0;

    protected int _srid = 0;

    public MapProjectionBase()
    {

    }

    //public MapProjectionBase(Ellipsoid ellipsoid) : base(string.Empty, ellipsoid)
    //{
    //}

    public MapProjectionBase(string title, Ellipsoid ellipsoid, int srid) : base(title, ellipsoid)
    {
        _srid = srid;
    }

    public double FalseEasting
    {
        get { return _falseEasting; }
    }

    public double FalseNorthing
    {
        get { return _falseNorthing; }
    }

    public double CentralMeridian
    {
        get { return _centralMeridian; }
    }

    public double ScaleFactor
    {
        get { return _scaleFactor; }
    }

    public double LatitudeOfOrigin
    {
        get { return _latitudeOfOrigin; }
    }

    public double StandardParallel1
    {
        get { return _standardParallel1; }
    }

    public double StandardParallel2
    {
        get { return _standardParallel2; }
    }

}
