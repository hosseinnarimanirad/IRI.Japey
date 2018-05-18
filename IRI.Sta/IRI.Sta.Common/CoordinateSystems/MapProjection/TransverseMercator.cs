using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IRI.Ham.SpatialBase;
using Ellipsoid = IRI.Ham.CoordinateSystem.Ellipsoid<IRI.Ham.MeasurementUnit.Meter, IRI.Ham.MeasurementUnit.Degree>;

namespace IRI.Ham.CoordinateSystem.MapProjection
{
    public class TransverseMercator : MapProjectionBase
    {
        //readonly double _falseEasting, _falseNorthing, _centralMeridian, _scaleFactor = 1.0, _latitude_Of_Origin;

        public override MapProjectionType Type
        {
            get
            {
                return MapProjectionType.TransverseMercator;
            }
        }

        public TransverseMercator() : this(Ellipsoids.WGS84)
        {
        }

        public TransverseMercator(Ellipsoid ellipsoid) : this(ellipsoid, 0, 0, 0, 0, 1)
        {
        }

        public TransverseMercator(Ellipsoid ellipsoid, double centralMeridian, double latitudeOfOrigin, double falseEasting, double falseNorthing, double scaleFactor) : base(string.Empty, ellipsoid)
        {
            this._falseEasting = falseEasting;

            this._falseNorthing = falseNorthing;

            this._centralMeridian = centralMeridian;

            this._scaleFactor = scaleFactor;

            this._latitudeOfOrigin = latitudeOfOrigin;
        }

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
