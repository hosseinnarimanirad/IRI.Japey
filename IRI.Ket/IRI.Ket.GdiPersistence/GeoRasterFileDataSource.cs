﻿using IRI.Msh.Common.Primitives;
using IRI.Msh.Common.Model;
using IRI.Msh.CoordinateSystem.MapProjection;

namespace IRI.Ket.Persistence.DataSources
{
    public class GeoRasterFileDataSource : IDataSource
    {
        //string imageFileName;
        GeoReferencedImage geoRaster;

        public BoundingBox WebMercatorExtent { get; private set; }

        public int Srid => SridHelper.WebMercator;

        private GeoRasterFileDataSource(string imageFileName, int srid)
        {
            this.geoRaster = IRI.Ket.WorldfileFormat.WorldfileManager.ReadWorldfile(imageFileName, srid);

            this.WebMercatorExtent = geoRaster.GeodeticWgs84BoundingBox.Transform(i => IRI.Msh.CoordinateSystem.MapProjection.MapProjects.GeodeticWgs84ToWebMercator(i));
        }

        public GeoRasterFileDataSource(GeoReferencedImage image)
        {
            this.geoRaster = image;
            
            this.WebMercatorExtent = geoRaster.GeodeticWgs84BoundingBox.Transform(i => IRI.Msh.CoordinateSystem.MapProjection.MapProjects.GeodeticWgs84ToWebMercator(i));
        }

        public static GeoRasterFileDataSource Create(string imageFileName, int srid)
        {
            try
            {
                return new GeoRasterFileDataSource(imageFileName, srid);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public GeoReferencedImage Get(BoundingBox boundingBox)//, Func<string, GeoReferencedImage> func)
        {
            if (this.WebMercatorExtent.Intersects(boundingBox))
            {
                return geoRaster;
            }
            else
            {
                return null;
            }

            //try
            //{
            //    var result = IRI.Ket.WorldfileFormat.WorldfileManager.ReadWorldfile(this.imageFileName);

            //    this.Extent = result.GeodeticWgs84BoundingBox.Transform(i => IRI.Msh.CoordinateSystem.Projection.GeodeticToMercator(i));

            //    return result;
            //}
            //catch (Exception ex)
            //{
            //    throw new NotImplementedException();
            //}
        }

    }
}
