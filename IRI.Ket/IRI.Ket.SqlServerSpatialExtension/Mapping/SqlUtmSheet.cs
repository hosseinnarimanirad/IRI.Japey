using IRI.Ket.SpatialExtensions;
using IRI.Ket.SqlServerSpatialExtension.Model;
using IRI.Msh.Common.Mapping;
using Microsoft.SqlServer.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Ket.SqlServerSpatialExtension.Mapping
{
    public class SqlUtmSheet : UtmSheet, ISqlGeometryAware
    {
        public SqlGeometry TheSqlGeometry { get => TheGeometry.AsSqlGeometry(); set => throw new NotImplementedException(); }

        public SqlUtmSheet(UtmSheet sheet)
        {
            this.Column = sheet.Column;

            this.Row = sheet.Row;

            this.SheetName = sheet.SheetName;

            this.Type = sheet.Type;

            this.UtmExtent = sheet.UtmExtent;

            this.UtmZone = sheet.UtmZone;
        }
    }
}
