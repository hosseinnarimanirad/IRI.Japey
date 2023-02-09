using IRI.Extensions;
using IRI.Msh.Common.Model;
using IRI.Msh.CoordinateSystem.MapProjection;
using Microsoft.SqlServer.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Ket.SqlServerSpatialExtension.Model
{
    public class SqlFeatureSet
    {
        public string Title { get; set; }

        public int Srid { get; set; }

        public List<Field> Fields { get; set; }

        public List<SqlFeature> Features { get; set; }

        public SqlFeatureSet(IEnumerable<SqlGeometry> features) : this(features?.FirstOrDefault().GetSrid() ?? 0)
        {
            this.Features = features.Select(i => new SqlFeature(i)).ToList();

        }

        public SqlFeatureSet(int srid)
        {
            this.Srid = srid;

            this.Fields = new List<Field>();
        }

        public void SaveAsShapefile(string shpFileName, Encoding encoding, SrsBase srs, bool overwrite = false)
        {
            //SaveAsShapefile(shpFileName, values.Select(v => geometryMap(v)), false, srs, overwrite);

            //DbfFile.Write(GetDbfFileName(shpFileName), values, attributeMappings, encoding, overwrite);

            ShapefileFormat.Shapefile.SaveAsShapefile(shpFileName, Features, f => f.TheSqlGeometry.AsEsriShape(), false, srs, overwrite);

            ShapefileFormat.Dbf.DbfFile.Write(ShapefileFormat.Shapefile.GetDbfFileName(shpFileName), Features.Select(f => f.Attributes).ToList(), encoding, overwrite);
        }

        public void SaveAsGeoJson(string geoJsonFileName, bool isLongitudeFirst)
        {
            var srsBase = SridHelper.AsSrsBase(this.Srid);

            var features = Features.Select(f => f.AsGeoJsonFeature(p => srsBase.ToWgs84Geodetic(p), isLongitudeFirst)).ToList();

            Msh.Common.Model.GeoJson.GeoJsonFeatureSet featureSet = new Msh.Common.Model.GeoJson.GeoJsonFeatureSet()
            {
                Features = features,
                TotalFeatures = features.Count,
            };

            featureSet.Save(geoJsonFileName, false, true);
        }
    }

    public class SqlFeatureSet<T> where T : ISqlGeometryAware
    {
        public string Title { get; set; }

        public int Srid { get; set; }

        public List<Field> Fields { get; set; }

        public List<T> Features { get; set; }

        public SqlFeatureSet(List<T> features)
        {
            this.Features = features;

            this.Fields = new List<Field>();
        }

        public SqlFeatureSet()
        {

        }
    }
}
