//using IRI.Ket.SpatialExtensions;
//using IRI.Ket.SqlServerSpatialExtension.Model;
//using IRI.Msh.Common.Mapping;
//using Microsoft.SqlServer.Types;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace IRI.Ket.SqlServerSpatialExtension.Mapping
//{
//    public class SqlGeodeticSheet : GeodeticSheet, ISqlGeometryAware
//    {
//        public SqlGeometry TheSqlGeometry { get => this.TheGeometry.AsSqlGeometry(); set => throw new NotImplementedException(); }

//        public SqlGeodeticSheet(GeodeticSheet sheet) : base(sheet.GeodeticExtent, sheet.Type)
//        {
//            this.SheetName = sheet.SheetName;            
//        }



//    }
//}
