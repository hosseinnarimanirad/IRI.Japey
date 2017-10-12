using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ellipsoid = IRI.Ham.CoordinateSystem.Ellipsoid<IRI.Ham.MeasurementUnit.Meter, IRI.Ham.MeasurementUnit.Degree>;

namespace IRI.Ham.CoordinateSystem.MapProjection
{
    public abstract class MapProjectionBase : CoordinateReferenceSystemBase
    {
        protected double _latitudeOfOrigin, _standardParallel1, _standardParallel2, _centralMeridian, _falseEasting, _falseNorthing;

        protected double _scaleFactor = 1.0;

        public MapProjectionBase()
        {

        }

        //public MapProjectionBase(Ellipsoid ellipsoid) : base(string.Empty, ellipsoid)
        //{
        //}

        public MapProjectionBase(string title, Ellipsoid ellipsoid) : base(title, ellipsoid)
        {
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
}
