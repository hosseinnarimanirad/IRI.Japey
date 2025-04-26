using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IRI.Sta.Common.Primitives; 

namespace IRI.Msh.CoordinateSystem.MapProjection
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

        public override TPoint FromGeodetic<TPoint>(TPoint geodeticWgs84)
        {
            return MapProjects.GeodeticWgs84ToWebMercator(geodeticWgs84);
        }

        public override TPoint ToGeodetic<TPoint>(TPoint webMercator)
        {
            return MapProjects.WebMercatorToGeodeticWgs84(webMercator);
        }

        protected override int GetSrid()
        {
            return SridHelper.WebMercator;
        }
    }
}
