using System.Text;

using IRI.Maptor.Extensions;
using IRI.Maptor.Sta.Spatial.Model;
using IRI.Maptor.Sta.Spatial.Helpers;
using IRI.Maptor.Sta.Common.Primitives;
using IRI.Maptor.Sta.ShapefileFormat.Dbf;
using IRI.Maptor.Sta.SpatialReferenceSystem;
using IRI.Maptor.Sta.SpatialReferenceSystem.MapProjections;
using IRI.Maptor.Extensions;

namespace IRI.Maptor.Ket.SqlServerSpatialExtension.Helpers;

public static class WebMercatorHelper
{
    public static void WriteGeodeticBoundingBoxToGoogleTileRegionsAsShapefile(string fileName, BoundingBox geodeticBoundingBox, int zoomLevel)
    {
        var tiles = WebMercatorUtility.GeodeticBoundingBoxToGoogleTileRegions(geodeticBoundingBox, zoomLevel);

        IRI.Maptor.Sta.ShapefileFormat.Shapefile.Save<TileInfo>(
            fileName,
            tiles,
            t => t.GeocentricExtent.AsEsriShape(SridHelper.GeodeticWGS84),
            new List<IRI.Maptor.Sta.ShapefileFormat.Model.ObjectToDbfTypeMap<TileInfo>>()
                {
                    new IRI.Maptor.Sta.ShapefileFormat.Model.ObjectToDbfTypeMap<TileInfo>(DbfFieldDescriptors.GetIntegerField("Google Zoom"), t => t.ZoomLevel),
                    new IRI.Maptor.Sta.ShapefileFormat.Model.ObjectToDbfTypeMap<TileInfo>(DbfFieldDescriptors.GetIntegerField("Row Number"), t => t.RowNumber),
                    new IRI.Maptor.Sta.ShapefileFormat.Model.ObjectToDbfTypeMap<TileInfo>(DbfFieldDescriptors.GetIntegerField("Column Number"), t => t.ColumnNumber)
                },
            Encoding.ASCII,
            new NoProjection(),
            true);

        //IRI.Maptor.Sta.ShapefileFormat.Shapefile.SaveAsShapefile(
        //    fileName,
        //    tiles.Select(r => (IEsriShape)r.GeodeticExtent.AsEsriShape()),
        //    false,
        //    new NoProjection(),
        //    true);

        //IRI.Maptor.Sta.ShapefileFormat.Dbf.DbfFile.Write<TileInfo>(
        //    IRI.Maptor.Sta.ShapefileFormat.Shapefile.GetDbfFileName(fileName),
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
