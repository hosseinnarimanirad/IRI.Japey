using IRI.Msh.Common.Primitives;
using Microsoft.SqlServer.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Ket.DataManagement.DataSource
{
    public interface IScaleDependentDataSource : IDataSource
    {
        List<SqlGeometry> GetGeometries(double mapScale);

        List<SqlGeometry> GetGeometries(double mapScale, BoundingBox boundingBox);

        Task<List<SqlGeometry>> GetGeometriesAsync(double mapScale);

        Task<List<SqlGeometry>> GetGeometriesAsync(double mapScale, BoundingBox boundingBox);
         
    }
}
