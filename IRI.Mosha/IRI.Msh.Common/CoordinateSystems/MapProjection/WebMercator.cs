using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IRI.Msh.Common.Primitives; 

namespace IRI.Sta.CoordinateSystem.MapProjection
{
    public class WebMercator : MapProjectionBase
    {
        protected double _auxiliarySphereType = 0;

        public double AuxiliarySphereType
        {
            get { return _auxiliarySphereType; }
        }

        public override MapProjectionType Type
        {
            get
            {
                return MapProjectionType.WebMercator;
            }
        }

        public WebMercator()
        {
            //This projection actually has no ellipsoid
            this._ellipsoid = Ellipsoids.WGS84;
        }

        public override IPoint FromGeodetic(IPoint geodeticWgs84)
        {
            return MapProjects.GeodeticWgs84ToWebMercator(geodeticWgs84);
        }

        public override IPoint ToGeodetic(IPoint webMercator)
        {
            return MapProjects.WebMercatorToGeodeticWgs84(webMercator);
        }

        protected override int GetSrid()
        {
            return SridHelper.WebMercator;
        }
    }
}
