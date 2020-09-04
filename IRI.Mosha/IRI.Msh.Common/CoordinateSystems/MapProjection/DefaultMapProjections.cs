
using IRI.Msh.MeasurementUnit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRI.Msh.CoordinateSystem.MapProjection
{
    public static class DefaultMapProjections
    {
        public static LambertConformalConic2P LccNiocWithWgs84
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

        public static LambertConformalConic2P LccNiocWithClarcke1880Rgs
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

        public static LambertConformalConic2P LccNahrawan
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

        public static LambertConformalConic2P LccNahrawanIraq
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
        public static LambertConformalConic1P LccFd58
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

            WebMercator = new WebMercator();

            //https://epsg.io/3200
            //LccFd58 = new LambertConformalConic(
            //   ellipsoid: Ellipsoids.FD58,
            //   standardParallel1: 29.655083333333333333333333333333,
            //   standardParallel2: 35.314694444444444444444444444444,
            //   centralMeridian: 45.0,
            //   latitudeOfOrigin: 32.5,
            //   falseEasting: 1500000.0,
            //   falseNorthing: 1166200.0,
            //   scaleFactor: 0.9987864078,
            //   srid: 3200);
            LccFd58 = new LambertConformalConic1P(
              ellipsoid: Ellipsoids.FD58,
              centralMeridian: 45.0,
              latitudeOfOrigin: 32.5,
              falseEasting: 1500000.0,
              falseNorthing: 1166200.0,
              scaleFactor: 0.9987864078,
              srid: 3200);


            LccNahrawanIraq = new LambertConformalConic2P(
               ellipsoid: Ellipsoids.NahrawanIraq,
               standardParallel1: 29.655083333333333333333333333333,
               standardParallel2: 35.314694444444444444444444444444,
               centralMeridian: 45.0,
               latitudeOfOrigin: 32.5,
               falseEasting: 1500000.0,
               falseNorthing: 1166200.0,
               scaleFactor: 1);


            LccNahrawan = new LambertConformalConic2P(
               ellipsoid: Ellipsoids.Nahrawan,
               standardParallel1: 29.655083333333333333333333333333,
               standardParallel2: 35.314694444444444444444444444444,
               centralMeridian: 45.0,
               latitudeOfOrigin: 32.5,
               falseEasting: 1500000.0,
               falseNorthing: 1166200.0,
               scaleFactor: 1);

            LccNiocWithClarcke1880Rgs = new LambertConformalConic2P(
                ellipsoid: Ellipsoids.Clarke1880Rgs,
                standardParallel1: 29.655083333333333333333333333333,
                standardParallel2: 35.314694444444444444444444444444,
                centralMeridian: 45.0,
                latitudeOfOrigin: 32.5,
                falseEasting: 1500000.0,
                falseNorthing: 1166200.0,
                scaleFactor: 0.9987864078);

            LccNiocWithWgs84 = new LambertConformalConic2P(
                ellipsoid: Ellipsoids.WGS84,
                standardParallel1: 29.65508274166,
                standardParallel2: 35.31468809166,
                centralMeridian: 45.0,
                latitudeOfOrigin: 32.5,
                falseEasting: 1500000.0,
                falseNorthing: 1166200.0,
                scaleFactor: 0.9987864078);


        }

        public static WebMercator WebMercator
        {
            get; private set;
        }


        //  case MapProjectionType.None:
        //        

        //    case MapProjectionType.AlbersEqualAreaConic:
        //    case MapProjectionType.AzimuthalEquidistant:
        //        throw new NotImplementedException();

        //    case MapProjectionType.CylindricalEqualArea:
        //        return new CylindricalEqualArea(this.Title, this.Ellipsoid, Srid) { DatumName = this.Geogcs.Values?.First() };

        //    case MapProjectionType.LambertConformalConic:
        //        return new LambertConformalConic2P(
        //            this.Ellipsoid,
        //            GetParameter(EsriPrjParameterType.StandardParallel_1, double.NaN),
        //            GetParameter(EsriPrjParameterType.StandardParallel_2, double.NaN),
        //            GetParameter(EsriPrjParameterType.CentralMeridian, 0),
        //            GetParameter(EsriPrjParameterType.LatitudeOfOrigin, 0),
        //            GetParameter(EsriPrjParameterType.FalseEasting, 0),
        //            GetParameter(EsriPrjParameterType.FalseNorthing, 0),
        //            GetParameter(EsriPrjParameterType.ScaleFactor, 1),
        //            Srid)
        //        {
        //            Title = this.Title,
        //            DatumName = this.Geogcs.Values?.First()
        //        };

        //    case MapProjectionType.Mercator:
        //        return new Mercator(this.Ellipsoid, Srid)
        //        {
        //            Title = this.Title,
        //            DatumName = this.Geogcs.Values?.First()
        //        };

        //    case MapProjectionType.TransverseMercator:
        //    case MapProjectionType.UTM:
        //        return new TransverseMercator(
        //            this.Ellipsoid,
        //            GetParameter(EsriPrjParameterType.CentralMeridian, 0),
        //            GetParameter(EsriPrjParameterType.LatitudeOfOrigin, 0),
        //            GetParameter(EsriPrjParameterType.FalseEasting, 0),
        //            GetParameter(EsriPrjParameterType.FalseNorthing, 0),
        //            GetParameter(EsriPrjParameterType.ScaleFactor, 1),
        //            Srid)
        //        {
        //            Title = this.Title,
        //            DatumName = this.Geogcs.Values?.First()
        //        };

        //    case MapProjectionType.WebMercator:
        //        return new WebMercator()
        //        {
        //            Title = this.Title,
        //            DatumName = this.Geogcs.Values?.First()
        //        };

        //default:
        //        throw new NotImplementedException();
    }
}

