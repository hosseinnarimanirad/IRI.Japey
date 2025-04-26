﻿using IRI.Sta.Common.Primitives;
using IRI.Sta.Ogc.WMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Extensions
{
    public static class BoundingBoxExtensions
    {
        public static EX_GeographicBoundingBox AsEXGeographicBoundingBox(this BoundingBox value)
        {
            return new EX_GeographicBoundingBox()
            {
                eastBoundLongitude = value.XMax,
                westBoundLongitude = value.XMin,
                northBoundLatitude = value.YMax,
                southBoundLatitude = value.YMin
            };
        }

        public static Boundingbox AsBoundingbox(this BoundingBox value, string crs = WmsConstants.Epsg4326)
        {
            if (crs == WmsConstants.Crs84)
            {
                return new Boundingbox() { crs = crs, minx = value.XMin, maxx = value.XMax, miny = value.YMin, maxy = value.YMax };
            }
            else
            {
                //This is strange but wms 1.3.0 is this way!
                return new Boundingbox() { crs = crs, minx = value.YMin, maxx = value.YMax, miny = value.XMin, maxy = value.XMax };
            }
        }
    }
}
