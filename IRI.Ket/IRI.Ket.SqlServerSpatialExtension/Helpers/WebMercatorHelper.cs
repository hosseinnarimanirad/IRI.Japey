using IRI.Sta.CoordinateSystem.MapProjection;
using IRI.Msh.Common.Primitives;
using IRI.Msh.Common.Mapping;
using IRI.Msh.Common.Model;
using IRI.Ket.ShapefileFormat.Dbf;
using IRI.Ket.ShapefileFormat.EsriType;
using IRI.Ket.SpatialExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Ket.SqlServerSpatialExtension.Helpers
{
    public static class WebMercatorHelper
    {
        public static void WriteGeodeticBoundingBoxToGoogleTileRegionsAsShapefile(string fileName, BoundingBox geodeticBoundingBox, int zoomLevel)
        {
            var tiles = WebMercatorUtility.GeodeticBoundingBoxToGoogleTileRegions(geodeticBoundingBox, zoomLevel);

            IRI.Ket.ShapefileFormat.Shapefile.Save<TileInfo>(
                fileName,
                tiles,
                t => t.GeocentricExtent.AsEsriShape(SridHelper.GeodeticWGS84),
                new List<ShapefileFormat.Model.ObjectToDbfTypeMap<TileInfo>>()
                    {
                        new ShapefileFormat.Model.ObjectToDbfTypeMap<TileInfo>(DbfFieldDescriptors.GetIntegerField("Google Zoom"), t => t.ZoomLevel),
                        new ShapefileFormat.Model.ObjectToDbfTypeMap<TileInfo>(DbfFieldDescriptors.GetIntegerField("Row Number"), t => t.RowNumber),
                        new ShapefileFormat.Model.ObjectToDbfTypeMap<TileInfo>(DbfFieldDescriptors.GetIntegerField("Column Number"), t => t.ColumnNumber)
                    },
                Encoding.ASCII,
                new NoProjection(),
                true);

            //IRI.Ket.ShapefileFormat.Shapefile.SaveAsShapefile(
            //    fileName,
            //    tiles.Select(r => (IEsriShape)r.GeodeticExtent.AsEsriShape()),
            //    false,
            //    new NoProjection(),
            //    true);

            //IRI.Ket.ShapefileFormat.Dbf.DbfFile.Write<TileInfo>(
            //    IRI.Ket.ShapefileFormat.Shapefile.GetDbfFileName(fileName),
            //    tiles,
            //    new List<Func<TileInfo, object>>()
            //    {
            //        new Func<TileInfo, object>(t=>t.ZoomLevel),
            //        new Func<TileInfo, object>(t=>t.RowNumber),
            //        new Func<TileInfo, object>(t=>t.ColumnNumber)
            //    },
            //    new List<ShapefileFormat.Dbf.DbfFieldDescriptor>()
            //    {
            //        ShapefileFormat.Dbf.DbfFieldDescriptors.GetIntegerField("Google Zoom"),
            //        ShapefileFormat.Dbf.DbfFieldDescriptors.GetIntegerField("Row Number"),
            //        ShapefileFormat.Dbf.DbfFieldDescriptors.GetIntegerField("Column Number")
            //    },
            //    Encoding.ASCII,
            //    true);
        }

        public static void WriteWebMercatorBoundingBoxToGoogleTileRegionsAsShapefile(string fileName, BoundingBox webMercatorBoundingBox, int zoomLevel)
        {
            var geographicBoundingBox = webMercatorBoundingBox.Transform(i => MapProjects.WebMercatorToGeodeticWgs84(i));

            WriteGeodeticBoundingBoxToGoogleTileRegionsAsShapefile(fileName, geographicBoundingBox, zoomLevel);
        }

    }
}
