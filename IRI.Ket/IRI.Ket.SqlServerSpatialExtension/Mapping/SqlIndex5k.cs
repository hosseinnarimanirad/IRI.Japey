﻿using IRI.Ket.SpatialExtensions;
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
    public class SqlIndex5k : Index5k, ISqlGeometryAware
    {
        [JsonIgnore]
        SqlGeometry ISqlGeometryAware.TheSqlGeometry { get => TheGeometry.AsSqlGeometry(); set => throw new NotImplementedException(); }

        public SqlIndex5k()
        {

        }

        public SqlIndex5k(Index5k index)
        {
            this.MinLatitude = index.MinLatitude;

            this.MinLongitude = index.MinLongitude;

            this.SheetNameEn = index.SheetNameEn;

            this.SheetNameFa = index.SheetNameFa;

            this.SheetNumber = index.SheetNumber;
        }
    }
}