using IRI.Ket.SqlServerSpatialExtension.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Ket.DataManagement.DataSource
{
    public class SqlFeatureDataSource : MemoryDataSource<SqlFeature>
    {
        private const string _geoColumnName = "Geo";

        private static Func<List<SqlFeature>, DataTable> sqlFeatureToDataTableMapping = (list) =>
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

                newRow[_geoColumnName] = item.TheSqlGeometry;

                table.Rows.Add(newRow);
            }

            return table;
        };

        public SqlFeatureDataSource()
        {

        }

        public SqlFeatureDataSource(List<SqlFeature> features, Func<SqlFeature, string> labelFunc = null)
        {
            this.ToDataTableMappingFunc = sqlFeatureToDataTableMapping;

            this._features = features;

            this._labelFunc = labelFunc;
        }

    }
}
