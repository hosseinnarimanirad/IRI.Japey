using IRI.Ket.SpatialExtensions;
using IRI.Ket.SqlServerSpatialExtension.Model;
using IRI.Msh.Common.Mapping;
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
        public static List<SqlGeometry> GetIndexes(SqlGeometry webMercatorGeometry, NccIndexType type)
        {
            switch (type)
            {
                case NccIndexType.Ncc250k:
                    return GetIndexes(webMercatorGeometry, IRI.Msh.Common.Mapping.MapIndexes.Find250kIndexMbbs);

                case NccIndexType.Ncc100k:
                    return GetIndexes(webMercatorGeometry, IRI.Msh.Common.Mapping.MapIndexes.Find100kIndexMbbs);

                case NccIndexType.Ncc50k:
                    return GetIndexes(webMercatorGeometry, IRI.Msh.Common.Mapping.MapIndexes.Find50kIndexMbbs);

                case NccIndexType.Ncc25k:
                    return GetIndexes(webMercatorGeometry, IRI.Msh.Common.Mapping.MapIndexes.Find25kIndexMbbs);

                case NccIndexType.Ncc10k:
                    return GetIndexes(webMercatorGeometry, IRI.Msh.Common.Mapping.MapIndexes.Find10kIndexMbbs);

                case NccIndexType.Ncc5k:
                    return GetIndexes(webMercatorGeometry, IRI.Msh.Common.Mapping.MapIndexes.Find5kIndexMbbs);

                case NccIndexType.NccUtmBased2kSheet:
                case NccIndexType.NccUtmBased1k:
                case NccIndexType.NccUtmBased500:
                default:
                    throw new NotImplementedException();
            }
        }

        public static List<SqlFeature> GetIndexSheets(SqlGeometry webMercatorGeometry, NccIndexType type)
        {
            switch (type)
            {
                case NccIndexType.Ncc250k:
                    return GetIndexeSheets(webMercatorGeometry, IRI.Msh.Common.Mapping.MapIndexes.Find250kIndexSheets);

                case NccIndexType.Ncc100k:
                    return GetIndexeSheets(webMercatorGeometry, IRI.Msh.Common.Mapping.MapIndexes.Find100kIndexSheets);

                case NccIndexType.Ncc50k:
                    return GetIndexeSheets(webMercatorGeometry, IRI.Msh.Common.Mapping.MapIndexes.Find50kIndexSheets);

                case NccIndexType.Ncc25k:
                    return GetIndexeSheets(webMercatorGeometry, IRI.Msh.Common.Mapping.MapIndexes.Find25kIndexSheets);

                case NccIndexType.Ncc10k:
                    return GetIndexeSheets(webMercatorGeometry, IRI.Msh.Common.Mapping.MapIndexes.Find10kIndexSheets);

                case NccIndexType.Ncc5k:
                    return GetIndexeSheets(webMercatorGeometry, IRI.Msh.Common.Mapping.MapIndexes.Find5kIndexSheets);


                case NccIndexType.NccUtmBased2kSheet:
                case NccIndexType.NccUtmBased1k:
                case NccIndexType.NccUtmBased500:
                default:
                    throw new NotImplementedException();
            }
        }

        public static List<SqlGeometry> GetIndexes(SqlGeometry webMercatorRegion, Func<BoundingBox, List<BoundingBox>> indexFunc)
        {
            var geographicGeometry = webMercatorRegion.Transform(IRI.Msh.CoordinateSystem.MapProjection.MapProjects.WebMercatorToGeodeticWgs84, 4326);

            var geographicBoundingBox = geographicGeometry.GetBoundingBox();

            return indexFunc(geographicBoundingBox)
                        .Where(b => b.Intersects(geographicGeometry))
                        .Select(b => b.Transform(IRI.Msh.CoordinateSystem.MapProjection.MapProjects.GeodeticWgs84ToWebMercator).AsSqlGeometry())
                        .ToList();
        }

        public static List<SqlFeature> GetIndexeSheets(SqlGeometry webMercatorRegion, Func<BoundingBox, List<IndexSheet>> indexFunc)
        {
            var geographicGeometry = webMercatorRegion.Transform(IRI.Msh.CoordinateSystem.MapProjection.MapProjects.WebMercatorToGeodeticWgs84, 4326);

            var geographicBoundingBox = geographicGeometry.GetBoundingBox();

            return indexFunc(geographicBoundingBox)
                        .Where(b => b.Extent.Intersects(geographicGeometry))
                        .Select(b => new SqlFeature(b.Extent.Transform(IRI.Msh.CoordinateSystem.MapProjection.MapProjects.GeodeticWgs84ToWebMercator).AsSqlGeometry())
                        {
                            Attributes = new Dictionary<string, object>() { { nameof(b.SheetName), b.SheetName } }
                        })
                        .ToList();
        }
    }
}
