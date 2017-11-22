using IRI.Ham.CoordinateSystem.MapProjection;
using IRI.Jab.Common.Model.Globalization;
using IRI.Ket.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Jab.Controls.Model.CoordinatePanel
{
    public static class SpatialReferenceItems
    {
        static readonly Func<double, string> toStringForGeodetic = d => d.ToString("#,#.#####");
        static readonly Func<double, string> toStringForGeodeticDms = d => DegreeHelper.ToDms(d, true);
        static readonly Func<double, string> toStringForDefault = d => d.ToString("#,#.###");

        static readonly PersianEnglishItem geodetic = new PersianEnglishItem("ژئودتیک (اعشار درجه)", "Geodetic (decimal degree)", Common.Model.LanguageMode.Persian);
        static readonly PersianEnglishItem geodeticDms = new PersianEnglishItem("ژئودتیک (درجه، دقیقه، ثانیه)", "Geodetic (dms)", Common.Model.LanguageMode.Persian);
        static readonly PersianEnglishItem utm = new PersianEnglishItem("سیستم تصویر UTM", "UTM", Common.Model.LanguageMode.Persian);
        static readonly PersianEnglishItem mercator = new PersianEnglishItem("سیستم تصویر Mercator", "Mercator", Common.Model.LanguageMode.Persian);
        static readonly PersianEnglishItem tm = new PersianEnglishItem("سیستم تصویر Transeverse Mercator", "Transeverse Mercator", Common.Model.LanguageMode.Persian);
        static readonly PersianEnglishItem cea = new PersianEnglishItem("سیستم تصویر Cylindrical Equal Area", "Cylindrical Equal Area", Common.Model.LanguageMode.Persian);

        //static readonly PersianEnglishItem _defaultZone = new PersianEnglishItem("ناحیه", "Zone", Common.Model.LanguageMode.Persian);
        static readonly PersianEnglishItem _defaultX = new PersianEnglishItem("X", "X", Common.Model.LanguageMode.Persian);
        static readonly PersianEnglishItem _defaultY = new PersianEnglishItem("Y", "Y", Common.Model.LanguageMode.Persian);
        static readonly PersianEnglishItem _defaultLatitude = new PersianEnglishItem("عرض جغرافیایی", "Latitude", Common.Model.LanguageMode.Persian);
        static readonly PersianEnglishItem _defaultLongitude = new PersianEnglishItem("طول جغرافیایی", "Longitude", Common.Model.LanguageMode.Persian);

        static readonly SpatialReferenceItem _geodeticWgs84 = new SpatialReferenceItem(p => p, toStringForGeodetic, geodetic, _defaultLongitude, _defaultLatitude);
        static readonly SpatialReferenceItem _geodeticDmsWgs84 = new SpatialReferenceItem(p => p, toStringForGeodeticDms, geodeticDms, _defaultLongitude, _defaultLatitude);
        static readonly SpatialReferenceItem _utmWgs84 = new SpatialReferenceItem(p => MapProjects.GeodeticToUTM(p, p.Y > 0), toStringForDefault, utm, _defaultX, _defaultY) { IsZoneVisible = true };
        static readonly SpatialReferenceItem _mercatorWgs84 = new SpatialReferenceItem(p => MapProjects.GeodeticToMercator(p), toStringForDefault, mercator, _defaultX, _defaultY);
        static readonly SpatialReferenceItem _tmWgs84 = new SpatialReferenceItem(p => MapProjects.GeodeticToTransverseMercator(p), toStringForDefault, tm, _defaultX, _defaultY);
        static readonly SpatialReferenceItem _cylindricalEqualAreaWgs84 = new SpatialReferenceItem(p => MapProjects.GeodeticToCylindricalEqualArea(p), toStringForDefault, cea, _defaultX, _defaultY);

        public static SpatialReferenceItem GeodeticWgs84
        {
            get { return _geodeticWgs84; }
        }

        public static SpatialReferenceItem GeodeticDmsWgs84
        {
            get { return _geodeticDmsWgs84; }
        }

        public static SpatialReferenceItem UtmWgs84
        {
            get { return _utmWgs84; }
        }

        public static SpatialReferenceItem MercatorWgs84
        {
            get { return _mercatorWgs84; }
        }

        public static SpatialReferenceItem TmWgs84
        {
            get { return _tmWgs84; }
        }

        public static SpatialReferenceItem CylindricalEqualAreaWgs84
        {
            get { return _cylindricalEqualAreaWgs84; }
        }

    }
}
