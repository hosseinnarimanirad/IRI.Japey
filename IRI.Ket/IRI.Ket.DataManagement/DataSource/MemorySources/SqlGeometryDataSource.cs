using IRI.Ket.SpatialExtensions;
using IRI.Ket.SqlServerSpatialExtension.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Ket.DataManagement.DataSource.MemorySources
{
    public class SqlGeometryDataSource : MemoryDataSource<NamedSqlGeometry>
    {
        public SqlGeometryDataSource(List<NamedSqlGeometry> geometries)
        {
            this._features = geometries;

            this._labelFunc = nsg => nsg.Label;

            this.Extent = GetGeometries().GetBoundingBox();
        }

    }
}
