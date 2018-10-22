using Microsoft.SqlServer.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Ket.SqlServerSpatialExtension.Model
{
    public interface ISqlGeometryAware
    {
        int Id { get; set; }

        SqlGeometry TheSqlGeometry { get; set; }
    }
}
