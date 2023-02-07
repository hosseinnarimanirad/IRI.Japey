using IRI.Ket.ShapefileFormat;
using IRI.Ket.SqlServerSpatialExtension.Helpers;
using IRI.Ket.SqlServerSpatialExtension.Model;
using IRI.Msh.Common.Primitives;
using IRI.Msh.CoordinateSystem.MapProjection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Ket.DataManagement.DataSource
{
    public class SqlFeatureDataSource : MemoryDataSource<Feature<Point>>
    {
        private const string _geoColumnName = "Geo";

        private static Func<List<Feature<Point>>, DataTable> sqlFeatureToDataTableMapping = (list) =>
        {
            if (!(list?.Count > 0))
            {
                return null;
            }

            DataTable table = new DataTable();

            foreach (var col in list?.First().Attributes)
            {
                table.Columns.Add(col.Key);
            }

            table.Columns.Add(new DataColumn(_geoColumnName));

            foreach (var item in list)
            {
                var newRow = table.NewRow();

                foreach (var col in list.First().Attributes)
                {
                    newRow[col.Key] = col.Value;
                }

                newRow[_geoColumnName] = item.TheGeometry;

                table.Rows.Add(newRow);
            }

            return table;
        };

        public SqlFeatureDataSource()
        {

        }

        public SqlFeatureDataSource(List<Feature<Point>> features, Func<Feature<Point>, string> labelFunc = null)
        {
            this.ToDataTableMappingFunc = sqlFeatureToDataTableMapping;

            this._features = features;

            this._labelFunc = labelFunc;

            this._mapToFeatureFunc = f => f;
        }

        public static SqlFeatureDataSource CreateFromShapefile(string shpFileName, string label, SrsBase targetSrs = null, bool correctFarsiCharacters = true, Encoding dataEncoding = null, Encoding headerEncoding = null)
        {
            //var features = ShapefileHelper.ReadAsSqlFeature(shpFileName, dataEncoding, targetSrs, headerEncoding, correctFarsiCharacters, label);
            var features = Shapefile.ReadAsFeature(shpFileName, dataEncoding, targetSrs, headerEncoding, correctFarsiCharacters, label);

            var result = new SqlFeatureDataSource(features, f => f.Label);

            //result.ToDataTableMappingFunc = sqlFeatureToDataTableMapping;

            return result;
        }

        public static async Task<SqlFeatureDataSource> CreateFromShapefileAsync(string shpFileName, string label, Encoding dataEncoding = null, SrsBase targetSrs = null, Encoding headerEncoding = null, bool correctFarsiCharacters = true)
        {
            var features = await Shapefile.ReadAsFeatureAsync(shpFileName, dataEncoding, targetSrs, headerEncoding, correctFarsiCharacters, label);

            var result = new SqlFeatureDataSource(features, i => i.Label);

            //result.ToDataTableMappingFunc = sqlFeatureToDataTableMapping;

            return result;
        }

    }
}
