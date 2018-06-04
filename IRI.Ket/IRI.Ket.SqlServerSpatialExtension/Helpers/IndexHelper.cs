using IRI.Ket.SpatialExtensions;
using IRI.Ket.SqlServerSpatialExtension.Model;
using IRI.Msh.Common.Model.Mapping;
using IRI.Msh.Common.Primitives;
using Microsoft.SqlServer.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Ket.SqlServerSpatialExtension.Helpers
{
    public static class IndexHelper
    {
        public static List<SqlGeometry> Get250kIndex(SqlGeometry webMercatorGeometry)
        {
            return GetIndexes(webMercatorGeometry, IRI.Msh.Common.Mapping.MapIndexes.Find250kIndexes);
        }

        public static List<SqlGeometry> Get100kIndex(SqlGeometry webMercatorGeometry)
        {
            return GetIndexes(webMercatorGeometry, IRI.Msh.Common.Mapping.MapIndexes.Find100kIndexes);
        }

        public static List<SqlGeometry> Get50kIndex(SqlGeometry webMercatorGeometry)
        {
            return GetIndexes(webMercatorGeometry, IRI.Msh.Common.Mapping.MapIndexes.Find50kIndexes);
        }

        public static List<SqlGeometry> Get25kIndex(SqlGeometry webMercatorGeometry)
        {
            return GetIndexes(webMercatorGeometry, IRI.Msh.Common.Mapping.MapIndexes.Find25kIndexes);
        }


        public static List<SqlFeature> Get100kIndexSheets(SqlGeometry webMercatorGeometry)
        {
            return GetIndexeSheets(webMercatorGeometry, IRI.Msh.Common.Mapping.MapIndexes.FindNcc100kIndexes);
        }

        public static List<SqlFeature> Get50kIndexSheets(SqlGeometry webMercatorGeometry)
        {
            return GetIndexeSheets(webMercatorGeometry, IRI.Msh.Common.Mapping.MapIndexes.FindNcc50kIndexes);
        }

        public static List<SqlFeature> Get25kIndexSheets(SqlGeometry webMercatorGeometry)
        {
            return GetIndexeSheets(webMercatorGeometry, IRI.Msh.Common.Mapping.MapIndexes.FindNcc25kIndexes);
        }

        public static List<SqlGeometry> GetIndexes(SqlGeometry webMercatorRegion, Func<BoundingBox, List<BoundingBox>> indexFunc)
        {
            var geographicGeometry = webMercatorRegion.Transform(IRI.Sta.CoordinateSystem.MapProjection.MapProjects.WebMercatorToGeodeticWgs84, 4326);

            var geographicBoundingBox = geographicGeometry.GetBoundingBox();

            return indexFunc(geographicBoundingBox)
                        .Where(b => b.Intersects(geographicGeometry))
                        .Select(b => b.Transform(IRI.Sta.CoordinateSystem.MapProjection.MapProjects.GeodeticWgs84ToWebMercator).AsSqlGeometry())
                        .ToList();
        }

        public static List<SqlFeature> GetIndexeSheets(SqlGeometry webMercatorRegion, Func<BoundingBox, List<IndexSheet>> indexFunc)
        {
            var geographicGeometry = webMercatorRegion.Transform(IRI.Sta.CoordinateSystem.MapProjection.MapProjects.WebMercatorToGeodeticWgs84, 4326);

            var geographicBoundingBox = geographicGeometry.GetBoundingBox();

            return indexFunc(geographicBoundingBox)
                        .Where(b => b.Extent.Intersects(geographicGeometry))
                        .Select(b => new SqlFeature()
                        {
                            Geometry = b.Extent.Transform(IRI.Sta.CoordinateSystem.MapProjection.MapProjects.GeodeticWgs84ToWebMercator).AsSqlGeometry(),
                            Attributes = new Dictionary<string, object>() { { nameof(b.SheetName), b.SheetName } }
                        })
                        .ToList();
        }
    }
}
