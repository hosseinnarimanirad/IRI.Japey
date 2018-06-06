using IRI.Sta.MeasurementUnit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRI.Sta.CoordinateSystem.MapProjection
{
    public static class DefaultMapProjections
    {
        public static LambertConformalConic LccNiocWithWgs84
        {
            get; private set;
            //get
            //{
            //    double phi0 = 32.5;
            //    double phi1 = 29.65508274166;
            //    double phi2 = 35.31468809166;
            //    double lambda0 = 45.0;
            //    return new LambertConformalConic(Ellipsoids.WGS84, phi1, phi2, lambda0, phi0, 1500000.0, 1166200.0, 0.9987864078);
            //}         
        }

        public static LambertConformalConic LccNiocWithClarcke1880Rgs
        {
            get; private set;
            //get
            //{
            //    double phi0 = 32.5; 
            //    double phi1 = 29.655083333333333333333333333333;
            //    double phi2 = 35.314694444444444444444444444444;
            //    double lambda0 = 45.0;
            //    return new LambertConformalConic(Ellipsoids.Clarke1880Rgs, phi1, phi2, lambda0, phi0, 1500000.0, 1166200.0, 1);
            //}
        }

        public static LambertConformalConic LccNahrawan
        {
            get; private set;
            //get
            //{
            //    double phi0 = 32.5;
            //    //double phi1 = 29.6551;
            //    //double phi2 = 35.3197;
            //    double phi1 = 29.655083333333333333333333333333;
            //    double phi2 = 35.314694444444444444444444444444;
            //    double lambda0 = 45.0;
            //    return new LambertConformalConic(Ellipsoids.Nahrawan, phi1, phi2, lambda0, phi0, 1500000.0, 1166200.0, 1);
            //}
        }

        public static LambertConformalConic LccNahrawanIraq
        {
            get; private set;
            //get
            //{
            //    double phi0 = 32.5;
            //    double phi1 = 29.655083333333333333333333333333;
            //    double phi2 = 35.314694444444444444444444444444;
            //    double lambda0 = 45.0;
            //    return new LambertConformalConic(Ellipsoids.NahrawanIraq, phi1, phi2, lambda0, phi0, 1500000.0, 1166200.0, 1);
            //}
        }

        //https://epsg.io/3200
        public static LambertConformalConic LccFd58
        {
            get; private set;
            //get
            //{
            //    double phi0 = 32.5;
            //    double phi1 = 29.655083333333333333333333333333;
            //    double phi2 = 35.314694444444444444444444444444;
            //    double lambda0 = 45.0;
            //    return new LambertConformalConic(Ellipsoids.FD58, phi1, phi2, lambda0, phi0, 1500000.0, 1166200.0, 1);
            //}
        }

        public static NoProjection GeodeticWgs84
        {
            get; private set;
        }

        static DefaultMapProjections()
        {
            GeodeticWgs84 = new NoProjection();

            //https://epsg.io/3200
            LccFd58 = new LambertConformalConic(
               ellipsoid: Ellipsoids.FD58,
               standardParallel1: 29.655083333333333333333333333333,
               standardParallel2: 35.314694444444444444444444444444,
               centralMeridian: 45.0,
               latitudeOfOrigin: 32.5,
               falseEasting: 1500000.0,
               falseNorthing: 1166200.0,
               scaleFactor: 0.9987864078,
               srid: 3200);

            LccNahrawanIraq = new LambertConformalConic(
               ellipsoid: Ellipsoids.NahrawanIraq,
               standardParallel1: 29.655083333333333333333333333333,
               standardParallel2: 35.314694444444444444444444444444,
               centralMeridian: 45.0,
               latitudeOfOrigin: 32.5,
               falseEasting: 1500000.0,
               falseNorthing: 1166200.0,
               scaleFactor: 1);

            LccNahrawan = new LambertConformalConic(
               ellipsoid: Ellipsoids.Nahrawan,
               standardParallel1: 29.655083333333333333333333333333,
               standardParallel2: 35.314694444444444444444444444444,
               centralMeridian: 45.0,
               latitudeOfOrigin: 32.5,
               falseEasting: 1500000.0,
               falseNorthing: 1166200.0,
               scaleFactor: 1);

            LccNiocWithClarcke1880Rgs = new LambertConformalConic(
                ellipsoid: Ellipsoids.Clarke1880Rgs,
                standardParallel1: 29.655083333333333333333333333333,
                standardParallel2: 35.314694444444444444444444444444,
                centralMeridian: 45.0,
                latitudeOfOrigin: 32.5,
                falseEasting: 1500000.0,
                falseNorthing: 1166200.0,
                scaleFactor: 0.9987864078);

            LccNiocWithWgs84 = new LambertConformalConic(
                ellipsoid: Ellipsoids.WGS84,
                standardParallel1: 29.65508274166,
                standardParallel2: 35.31468809166,
                centralMeridian: 45.0,
                latitudeOfOrigin: 32.5,
                falseEasting: 1500000.0,
                falseNorthing: 1166200.0,
                scaleFactor: 0.9987864078);


        }
    }
}
