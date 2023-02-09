using IRI.Extensions;
using IRI.Ket.SqlServerSpatialExtension.Model;
using IRI.Msh.Common.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

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
