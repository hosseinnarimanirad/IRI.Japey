//using IRI.Extensions;
//using IRI.Ket.SqlServerSpatialExtension.Model;
//using IRI.Msh.Common.Mapping;
//using IRI.Msh.Common.Primitives;
//using Microsoft.SqlServer.Types;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace IRI.Ket.SqlServerSpatialExtension.Helpers
//{
//    public static class IndexHelper
//    {
//        public static List<SqlGeometry> GetIndexes(SqlGeometry webMercatorGeometry, GeodeticIndexType type)
//        {
//            switch (type)
//            {
//                case GeodeticIndexType.Ncc250k:
//                    return GetIndexes(webMercatorGeometry, IRI.Msh.Common.Mapping.GeodeticIndexes.Find250kIndexMbbs);

//                case GeodeticIndexType.Ncc100k:
//                    return GetIndexes(webMercatorGeometry, IRI.Msh.Common.Mapping.GeodeticIndexes.Find100kIndexMbbs);

//                case GeodeticIndexType.Ncc50k:
//                    return GetIndexes(webMercatorGeometry, IRI.Msh.Common.Mapping.GeodeticIndexes.Find50kIndexMbbs);

//                case GeodeticIndexType.Ncc25k:
//                    return GetIndexes(webMercatorGeometry, IRI.Msh.Common.Mapping.GeodeticIndexes.Find25kIndexMbbs);

//                case GeodeticIndexType.Ncc10k:
//                    return GetIndexes(webMercatorGeometry, IRI.Msh.Common.Mapping.GeodeticIndexes.Find10kIndexMbbs);

//                case GeodeticIndexType.Ncc5k:
//                    return GetIndexes(webMercatorGeometry, IRI.Msh.Common.Mapping.GeodeticIndexes.Find5kIndexMbbs);
                     
//                default:
//                    throw new NotImplementedException();
//            }
//        }

//        public static List<SqlFeature> GetIndexSheets(SqlGeometry webMercatorGeometry, GeodeticIndexType type)
//        { 
//            var geographicGeometry = webMercatorGeometry.Transform(IRI.Msh.CoordinateSystem.MapProjection.MapProjects.WebMercatorToGeodeticWgs84, 4326);

//            var geographicBoundingBox = geographicGeometry.GetBoundingBox();
            
//            return GeodeticIndexes.FindIndexSheets(geographicBoundingBox, type)
//                        .Where(b => b.GeodeticExtent.Intersects(geographicGeometry))
//                        .Select(b => new SqlFeature(b.GeodeticExtent.Transform(IRI.Msh.CoordinateSystem.MapProjection.MapProjects.GeodeticWgs84ToWebMercator).AsSqlGeometry())
//                        {
//                            Attributes = new Dictionary<string, object>() { { nameof(b.SheetName), b.SheetName } }
//                        })
//                        .ToList();

//            //switch (type)
//            //{
//            //    case GeodeticIndexType.Ncc250k:
//            //        return GetIndexeSheets(webMercatorGeometry, IRI.Msh.Common.Mapping.GeodeticIndexes.Find250kIndexSheets);

//            //    case GeodeticIndexType.Ncc100k:
//            //        return GetIndexeSheets(webMercatorGeometry, IRI.Msh.Common.Mapping.GeodeticIndexes.Find100kIndexSheets);

//            //    case GeodeticIndexType.Ncc50k:
//            //        return GetIndexeSheets(webMercatorGeometry, IRI.Msh.Common.Mapping.GeodeticIndexes.Find50kIndexSheets);

//            //    case GeodeticIndexType.Ncc25k:
//            //        return GetIndexeSheets(webMercatorGeometry, IRI.Msh.Common.Mapping.GeodeticIndexes.Find25kIndexSheets);

//            //    case GeodeticIndexType.Ncc10k:
//            //        return GetIndexeSheets(webMercatorGeometry, IRI.Msh.Common.Mapping.GeodeticIndexes.Find10kIndexSheets);

//            //    case GeodeticIndexType.Ncc5k:
//            //        return GetIndexeSheets(webMercatorGeometry, IRI.Msh.Common.Mapping.GeodeticIndexes.Find5kIndexSheets);

//            //    default:
//            //        throw new NotImplementedException();
//            //}
//        }

        

//        public static List<SqlGeometry> GetIndexes(SqlGeometry webMercatorRegion, Func<BoundingBox, List<BoundingBox>> indexFunc)
//        {
//            var geographicGeometry = webMercatorRegion.Transform(IRI.Msh.CoordinateSystem.MapProjection.MapProjects.WebMercatorToGeodeticWgs84, 4326);

//            var geographicBoundingBox = geographicGeometry.GetBoundingBox();

//            return indexFunc(geographicBoundingBox)
//                        .Where(b => b.Intersects(geographicGeometry))
//                        .Select(b => b.Transform(IRI.Msh.CoordinateSystem.MapProjection.MapProjects.GeodeticWgs84ToWebMercator).AsSqlGeometry())
//                        .ToList();
//        }
         
//    }
//}
