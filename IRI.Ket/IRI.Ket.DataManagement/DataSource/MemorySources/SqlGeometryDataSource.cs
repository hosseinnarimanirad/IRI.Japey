using IRI.Ket.SpatialExtensions;
using IRI.Ket.SqlServerSpatialExtension.Model;
using IRI.Msh.Common.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRI.Msh.Common.Extensions;

namespace IRI.Ket.DataManagement.DataSource.MemorySources
{
    public class SqlGeometryDataSource : MemoryDataSource<NamedGeometry<Point>>
    {
        public SqlGeometryDataSource(List<NamedGeometry<Point>> geometries)
        {
            this._features = geometries;

            this._labelFunc = nsg => nsg.Label;

            this.Extent = GetGeometries().GetBoundingBox();
        }

    }
}
