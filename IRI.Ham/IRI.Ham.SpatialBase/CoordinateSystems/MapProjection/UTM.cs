using IRI.Ham.SpatialBase;
using Ellipsoid = IRI.Ham.CoordinateSystem.Ellipsoid<IRI.Ham.MeasurementUnit.Meter, IRI.Ham.MeasurementUnit.Degree>;

namespace IRI.Ham.CoordinateSystem.MapProjection
{
    public class UTM : MapProjectionBase
    {
        //double _centralLongitude;
        //readonly double _latitude0, _latitude1, _latitude2, _longitude0, _falseEasting, _falseNorthing;

        public override MapProjectionType Type
        {
            get
            {
                return MapProjectionType.UTM;
            }
        }

        public UTM(double centralLongitude) : this(Ellipsoids.WGS84, centralLongitude)
        {
        }

        public UTM(Ellipsoid ellipsoid, double centralLongitude) : base(string.Empty, ellipsoid)
        {
            this._centralMeridian = centralLongitude;

            this._latitudeOfOrigin = 0;

            this._falseEasting = 500000;

            this._scaleFactor = 0.9996;
        }

        //public override Point FromGeodetic(Point point)
        //{
        //    return MapProjects.GeodeticToUTM(point, this._ellipsoid);
        //}

        //public override Point ToGeodetic(Point point)
        //{
        //    return MapProjects.UTMToGeodetic(point, this._ellipsoid, this._centralMeridian);
        //}

        public override IPoint FromGeodetic(IPoint point)
        {
            var tempLongitude = point.X - _centralMeridian;

            var tempLatitude = point.Y - _latitudeOfOrigin;

            var result = MapProjects.GeodeticToTransverseMercator(new Point(tempLongitude, tempLatitude), this._ellipsoid);

            return new Point(result.X * _scaleFactor + _falseEasting, result.Y * _scaleFactor + _falseNorthing);
        }

        public override IPoint ToGeodetic(IPoint point)
        {
            //return MapProjects.TransverseMercatorToGeodetic(point, this._ellipsoid);

            double tempX = (point.X - _falseEasting) / _scaleFactor;

            double tempY = (point.Y - _falseNorthing) / _scaleFactor;

            Point result = MapProjects.TransverseMercatorToGeodetic(new Point(tempX, tempY), _ellipsoid);

            result.X = result.X + _centralMeridian;

            result.Y = result.Y + _latitudeOfOrigin;

            return result;

        }
    }
}
