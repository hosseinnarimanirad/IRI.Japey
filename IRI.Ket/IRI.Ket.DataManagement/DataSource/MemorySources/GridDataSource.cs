using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRI.Ket.SqlServerSpatialExtension.Model;
using Microsoft.SqlServer.Types;
using IRI.Ket.SqlServerSpatialExtension;
using IRI.Ket.SpatialExtensions;
using IRI.Msh.CoordinateSystem.MapProjection;
using IRI.Msh.Common.Mapping;
using IRI.Msh.Common.Primitives;

namespace IRI.Ket.DataManagement.DataSource.MemorySources
{
    public class GridDataSource : MemoryDataSource
    {
        //In degree
        public double GridWidth { get; private set; }

        //In degree
        public double GridHeight { get; private set; }

        public Indexes Type { get; private set; }

        public GridDataSource()
        {
        }

        public GridDataSource(List<SqlGeometry> geometries) : base(geometries)
        {
        }

        public static GridDataSource Create(Indexes indexType)
        {
            GridDataSource result = new GridDataSource();

            result.Type = indexType;

            return result;
        }

        public override List<SqlGeometry> GetGeometries()
        {
            return new List<SqlGeometry>();
        }

        public override List<SqlGeometry> GetGeometries(SqlGeometry geometry)
        {
            //var geographicBoundingBox = geometry.GetBoundingBox().Transform(MapProjects.WebMercatorToGeodeticWgs84);

            //return MapIndexes.GetIndexLines(geographicBoundingBox, this.Type)
            //        .Select(g => g.AsSqlGeometry().Transform(MapProjects.GeodeticWgs84ToWebMercator, SridHelper.WebMercator))
            //        .ToList();

            return GetGeometries(geometry.GetBoundingBox());
        }

        public override List<SqlGeometry> GetGeometries(BoundingBox boundingBox)
        {
            var geographicBoundingBox = boundingBox.Transform(MapProjects.WebMercatorToGeodeticWgs84);

            return MapIndexes.GetIndexLines(geographicBoundingBox, this.Type)
                   .Select(g => g.AsSqlGeometry().Transform(MapProjects.GeodeticWgs84ToWebMercator, SridHelper.WebMercator))
                   .ToList();
        }

    }
}
