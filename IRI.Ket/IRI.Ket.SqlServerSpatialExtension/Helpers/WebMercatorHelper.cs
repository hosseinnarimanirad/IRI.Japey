using System.Text;

using IRI.Extensions;
using IRI.Sta.Spatial.Model;
using IRI.Sta.Spatial.Helpers;
using IRI.Sta.Common.Primitives;
using IRI.Sta.ShapefileFormat.Dbf;
using IRI.Sta.SpatialReferenceSystem;
using IRI.Sta.SpatialReferenceSystem.MapProjections;

namespace IRI.Ket.SqlServerSpatialExtension.Helpers;

public static class WebMercatorHelper
{
    public static void WriteGeodeticBoundingBoxToGoogleTileRegionsAsShapefile(string fileName, BoundingBox geodeticBoundingBox, int zoomLevel)
    {
        var tiles = WebMercatorUtility.GeodeticBoundingBoxToGoogleTileRegions(geodeticBoundingBox, zoomLevel);

        IRI.Sta.ShapefileFormat.Shapefile.Save<TileInfo>(
            fileName,
            tiles,
            t => t.GeocentricExtent.AsEsriShape(SridHelper.GeodeticWGS84),
            new List<IRI.Sta.ShapefileFormat.Model.ObjectToDbfTypeMap<TileInfo>>()
                {
                    new IRI.Sta.ShapefileFormat.Model.ObjectToDbfTypeMap<TileInfo>(DbfFieldDescriptors.GetIntegerField("Google Zoom"), t => t.ZoomLevel),
                    new IRI.Sta.ShapefileFormat.Model.ObjectToDbfTypeMap<TileInfo>(DbfFieldDescriptors.GetIntegerField("Row Number"), t => t.RowNumber),
                    new IRI.Sta.ShapefileFormat.Model.ObjectToDbfTypeMap<TileInfo>(DbfFieldDescriptors.GetIntegerField("Column Number"), t => t.ColumnNumber)
                },
            Encoding.ASCII,
            new NoProjection(),
            true);

        //IRI.Sta.ShapefileFormat.Shapefile.SaveAsShapefile(
        //    fileName,
        //    tiles.Select(r => (IEsriShape)r.GeodeticExtent.AsEsriShape()),
        //    false,
        //    new NoProjection(),
        //    true);

        //IRI.Sta.ShapefileFormat.Dbf.DbfFile.Write<TileInfo>(
        //    IRI.Sta.ShapefileFormat.Shapefile.GetDbfFileName(fileName),
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
