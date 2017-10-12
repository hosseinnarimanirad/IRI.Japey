using IRI.Ham.MeasurementUnit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRI.Ham.CoordinateSystem.MapProjection
{
    public static class MapProjections
    {
        public static LambertConformalConic LccNiocWithWgs84
        {
            get
            {
                double phi0 = 32.5;
                double phi1 = 29.65508274166;
                double phi2 = 35.31468809166;
                double lambda0 = 45.0;
                return new LambertConformalConic(Ellipsoids.WGS84, phi1, phi2, lambda0, phi0, 1500000.0, 1166200.0, 0.9987864078);
            }
        }

        public static LambertConformalConic LccNiocWithClarcke1880Rgs
        {
            get
            {
                double phi0 = 32.5;
                //double phi1 = 29.65508274166;
                //double phi2 = 35.31468809166;
                double phi1 = 29.655083333333333333333333333333;
                double phi2 = 35.314694444444444444444444444444;
                double lambda0 = 45.0;
                //return new LambertConformalConic(Ellipsoids.Clarke1880Rgs, phi1, phi2, lambda0, phi0, 1500000.0, 1166200.0, 0.9987864078);
                return new LambertConformalConic(Ellipsoids.Clarke1880Rgs, phi1, phi2, lambda0, phi0, 1500000.0, 1166200.0, 1);
            }
        }

        public static LambertConformalConic LccNahrawan
        {
            get
            {
                double phi0 = 32.5;
                //double phi1 = 29.6551;
                //double phi2 = 35.3197;
                double phi1 = 29.655083333333333333333333333333;
                double phi2 = 35.314694444444444444444444444444;
                double lambda0 = 45.0;
                return new LambertConformalConic(Ellipsoids.Nahrawan, phi1, phi2, lambda0, phi0, 1500000.0, 1166200.0, 1);
            }
        }

        public static LambertConformalConic LccNahrawanIraq
        {
            get
            {
                double phi0 = 32.5;
                double phi1 = 29.655083333333333333333333333333;
                double phi2 = 35.314694444444444444444444444444;
                double lambda0 = 45.0;
                return new LambertConformalConic(Ellipsoids.NahrawanIraq, phi1, phi2, lambda0, phi0, 1500000.0, 1166200.0, 1);
            }
        }
         
        public static LambertConformalConic LccFd58
        {
            get
            {
                double phi0 = 32.5;
                double phi1 = 29.655083333333333333333333333333;
                double phi2 = 35.314694444444444444444444444444;
                double lambda0 = 45.0;
                return new LambertConformalConic(Ellipsoids.FD58, phi1, phi2, lambda0, phi0, 1500000.0, 1166200.0, 1);
            }
        }


    }
}
