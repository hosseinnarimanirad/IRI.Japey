using IRI.Msh.Common.Primitives; 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ellipsoid = IRI.Msh.CoordinateSystem.Ellipsoid<IRI.Msh.MeasurementUnit.Meter, IRI.Msh.MeasurementUnit.Degree>;

namespace IRI.Msh.CoordinateSystem.MapProjection
{
    public abstract class SrsBase
    {
        private string _title;

        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        public int Srid
        {
            get { return GetSrid(); }
        }

        protected abstract int GetSrid();

        public string DatumName { get; set; }

        protected Ellipsoid _ellipsoid;

        public Ellipsoid Ellipsoid
        {
            get { return _ellipsoid; }
        }

        public abstract MapProjectionType Type { get; }

        public SrsBase()
        {

        }

        //public CoordinateReferenceSystemBase() : this(Ellipsoids.WGS84)
        //{

        //}
        //public CoordinateReferenceSystemBase(Ellipsoid ellipsoid) : this(string.Empty, ellipsoid)
        //{

        //}

        public SrsBase(string title, Ellipsoid ellipsoid)
        {
            this.Title = title;

            this._ellipsoid = ellipsoid;
        }


        public abstract IPoint ToGeodetic(IPoint point);

        public IPoint ToWgs84Geodetic(IPoint point)
        {
            return ToGeodetic(point, Ellipsoids.WGS84);
        }

        //must be tested
        public IPoint ToLocalGeodetic(IPoint point)
        {
            var geocentricGeodeticPoint = ToGeodetic(point);

            return Transformation.ChangeDatum(geocentricGeodeticPoint, this.Ellipsoid.GetGeocentricVersion(0), this.Ellipsoid);
        }

        public IPoint ToGeodetic(IPoint point, Ellipsoid targetEllipsoid)
        {
            var temp = ToGeodetic(point);

            return ToTargetEllipsoid(temp, targetEllipsoid);
        }


        public abstract IPoint FromGeodetic(IPoint point);

        public IPoint FromWgs84Geodetic(IPoint point)
        {
            return FromGeodetic(point, Ellipsoids.WGS84);
        }

        //must be tested
        public IPoint FromLocalGeodetic(IPoint point)
        {
            var geocentricGeodeticPoint = Transformation.ChangeDatum(point, this.Ellipsoid, this.Ellipsoid.GetGeocentricVersion(0));

            return FromGeodetic(geocentricGeodeticPoint);
        }

        public IPoint FromGeodetic(IPoint point, Ellipsoid sourceEllipsoid)
        {
            var temp = FromSourceEllipsoid(point, sourceEllipsoid);

            return FromGeodetic(temp);
        }


        protected IPoint ToTargetEllipsoid(IPoint point, Ellipsoid targetEllipsoid)
        {
            if (targetEllipsoid != this.Ellipsoid ||
                targetEllipsoid.SemiMajorAxis.Value != this.Ellipsoid.SemiMajorAxis.Value ||
                targetEllipsoid.InverseFlattening != this.Ellipsoid.InverseFlattening)
            {
                return Transformation.ChangeDatumSimple(point, this._ellipsoid, targetEllipsoid);
            }
            else
            {
                return point;
            }
        }

        protected IPoint FromSourceEllipsoid(IPoint point, Ellipsoid sourceEllipsoid)
        {
            if (sourceEllipsoid != this.Ellipsoid ||
                sourceEllipsoid.SemiMajorAxis.Value != this.Ellipsoid.SemiMajorAxis.Value ||
                sourceEllipsoid.InverseFlattening != this.Ellipsoid.InverseFlattening)
            {
                return Transformation.ChangeDatumSimple(point, sourceEllipsoid, this._ellipsoid);
            }
            else
            {
                return point;
            }
        }


        //private int GetSrid()
        //{
        //    switch (this.Type)
        //    {
        //        case MapProjectionType.None:
        //            return Ellipsoid.Srid;

        //        case MapProjectionType.AlbersEqualAreaConic:
        //            break;
        //        case MapProjectionType.AzimuthalEquidistant:
        //            break;
        //        case MapProjectionType.CylindricalEqualArea:
        //            break;
        //        case MapProjectionType.LambertConformalConic:
        //            break;
        //        case MapProjectionType.Mercator:
        //            break;

        //        case MapProjectionType.TransverseMercator:
        //            break;

        //        case MapProjectionType.UTM:
        //            return (this as UTM).GetSrid(true);
                    
        //        case MapProjectionType.WebMercator:
        //            return SridHelper.WebMercator;

        //        default:
        //            break;
        //    }

        //    return 0;
        //}
    }
}
