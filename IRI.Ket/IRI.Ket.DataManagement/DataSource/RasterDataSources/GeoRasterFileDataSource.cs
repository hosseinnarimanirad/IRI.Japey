using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRI.Msh.Common.Primitives;
using Microsoft.SqlServer.Types;
using IRI.Ket.DataManagement.Model;
using IRI.Msh.Common.Model;

namespace IRI.Ket.DataManagement.DataSource
{
    public class GeoRasterFileDataSource : IDataSource
    {
        //string imageFileName;
        GeoReferencedImage geoRaster;

        public BoundingBox Extent { get; private set; }

        private GeoRasterFileDataSource(string imageFileName)
        {
            this.geoRaster = IRI.Ket.WorldfileFormat.WorldfileManager.ReadWorldfile(imageFileName);

            this.Extent = geoRaster.GeodeticWgs84BoundingBox.Transform(i => IRI.Msh.CoordinateSystem.MapProjection.MapProjects.GeodeticWgs84ToWebMercator(i));
        }

        public static GeoRasterFileDataSource Create(string imageFileName)
        {
            try
            {
                return new GeoRasterFileDataSource(imageFileName);
            }
            catch (Exception ex)
            {
                return null;
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
