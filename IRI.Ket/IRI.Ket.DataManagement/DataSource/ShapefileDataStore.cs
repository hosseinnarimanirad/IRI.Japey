using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRI.Ham.SpatialBase;
using Microsoft.SqlServer.Types;
using IRI.Ket.ShapefileFormat;
using IRI.Ket.DataManagement.Model;

namespace IRI.Ket.DataManagement.DataSource
{
    public class ShapefileDataStore : IDataSource
    {
        string _shpFileName, _spatialColumnName, _labelColumnName;

        int _srid;

        public BoundingBox Extent { get { throw new NotImplementedException(); } }

        public async static Task<ShapefileDataStore> Create(string shpFileName, string spatialColumnName, int srid, Encoding encoding, string labelColumnName = null)
        {
            var result = new ShapefileDataStore(shpFileName, spatialColumnName, srid, encoding, labelColumnName);

            await result.MakeIndex();

            return result;
        }

        private ShapefileDataStore()
        {

        }

        private ShapefileDataStore(string shpFileName, string spatialColumnName, int srid, Encoding encoding, string labelColumnName = null)
        {
            if (!System.IO.File.Exists(shpFileName))
            {
                throw new NotImplementedException();
            }

            this._shpFileName = shpFileName;

            this._spatialColumnName = spatialColumnName;

            this._labelColumnName = labelColumnName;

            this._srid = srid;


        }

        public async Task MakeIndex(bool overwrite = true)
        {
            var indexFileName = IRI.Ket.ShapefileFormat.Shapefile.GetIndexFileName(_shpFileName);

            if (overwrite && System.IO.File.Exists(indexFileName))
            {
                System.IO.File.Delete(indexFileName);
            }

            await Shapefile.CreateIndex(_shpFileName);
        }
         
    }
}
