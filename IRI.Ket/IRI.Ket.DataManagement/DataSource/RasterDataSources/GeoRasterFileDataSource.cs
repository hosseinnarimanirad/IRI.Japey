using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRI.Ham.SpatialBase;
using Microsoft.SqlServer.Types;
using IRI.Ket.DataManagement.Model;

namespace IRI.Ket.DataManagement.DataSource
{
    public class GeoRasterFileDataSource : IDataSource
    {
        //string imageFileName;
        GeoReferencedImage geoRaster;

        public BoundingBox Extent { get; private set; }

        public GeoRasterFileDataSource(string imageFileName)
        {
            if (!System.IO.File.Exists(imageFileName))
            {
                throw new NotImplementedException();
            }
            
            try
            {
                this.geoRaster = IRI.Ket.WorldfileFormat.WorldfileManager.ReadWorldfile(imageFileName);

                this.Extent = geoRaster.GeodeticWgs84BoundingBox.Transform(i => IRI.Ham.CoordinateSystem.MapProjection.MapProjects.GeodeticWgs84ToWebMercator(i));
            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
            }
        }

        public GeoReferencedImage Get(BoundingBox boundingBox)//, Func<string, GeoReferencedImage> func)
        {
            if (this.Extent.Intersects(boundingBox))
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

            //    this.Extent = result.GeodeticWgs84BoundingBox.Transform(i => IRI.Ham.CoordinateSystem.Projection.GeodeticToMercator(i));

            //    return result;
            //}
            //catch (Exception ex)
            //{
            //    throw new NotImplementedException();
            //}
        }

    }
}
