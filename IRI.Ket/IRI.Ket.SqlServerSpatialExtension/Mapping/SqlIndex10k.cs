using IRI.Ket.SpatialExtensions;
using IRI.Ket.SqlServerSpatialExtension.Model;
using IRI.Msh.Common.Mapping;
using Microsoft.SqlServer.Types;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Ket.SqlServerSpatialExtension.Mapping
{
    public class SqlIndex10k : Index10k, ISqlGeometryAware
    {
        [JsonIgnore]
        SqlGeometry ISqlGeometryAware.TheSqlGeometry { get => TheGeometry.AsSqlGeometry(); set => throw new NotImplementedException(); }

        public SqlIndex10k()
        {

        }

        public SqlIndex10k(Index10k index)
        {
            this.MinLatitude = index.MinLatitude;

            this.MinLongitude = index.MinLongitude;

            this.SheetNameEn = index.SheetNameEn;

            this.SheetNameFa = index.SheetNameFa;

            this.SheetNumber = index.SheetNumber; 
        }
    }
}
