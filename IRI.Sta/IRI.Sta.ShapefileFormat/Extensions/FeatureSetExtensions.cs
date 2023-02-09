using IRI.Ket.ShapefileFormat;
using IRI.Msh.CoordinateSystem.MapProjection;
using System;
using System.Collections.Generic;
using System.Text;
using IRI.Ket.SpatialExtensions;
using IRI.Ket.ShapefileFormat.Dbf;
using System.Linq;
using IRI.Msh.Common.Extensions;
using IRI.Msh.Common.Model.GeoJson;
using IRI.Msh.Common.Primitives;

namespace IRI.Sta.Common.Extensions
{
    public static class FeatureSetExtensions
    {
        public static void SaveAsShapefile(this FeatureSet featureSet, string shpFileName, Encoding encoding, SrsBase srs, bool overwrite = false)
        {
            Shapefile.SaveAsShapefile(shpFileName, featureSet.Features, f => f.TheGeometry.AsEsriShape(f.TheGeometry.Srid), false, srs, overwrite);

            DbfFile.Write(Shapefile.GetDbfFileName(shpFileName), featureSet.Features.Select(f => f.Attributes).ToList(), encoding, overwrite);
        }

        public static void SaveAsGeoJson(this FeatureSet featureSet, string geoJsonFileName, bool isLongitudeFirst)
        {
            var srsBase = SridHelper.AsSrsBase(featureSet.Srid);

            var features = featureSet.Features.Select(f => f.AsGeoJsonFeature(p => srsBase.ToWgs84Geodetic(p), isLongitudeFirst)).ToList();

            GeoJsonFeatureSet jsonFeatureSet = new GeoJsonFeatureSet()
            {
                Features = features,
                TotalFeatures = features.Count,
            };

            jsonFeatureSet.Save(geoJsonFileName, false, true);
        }
    }
}
