using IRI.Ket.SpatialExtensions;
using IRI.Ket.SqlServerSpatialExtension.Model;
using IRI.Msh.Common.Mapping;
using IRI.Msh.Common.Primitives;
using Microsoft.SqlServer.Types;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Ket.SqlServerSpatialExtension.Mapping
{

    public class SqlIndex250k : Index250k, ISqlGeometryAware
    {
        [JsonIgnore]
        SqlGeometry ISqlGeometryAware.TheSqlGeometry { get => TheGeometry.AsSqlGeometry(); set => throw new NotImplementedException(); }

        public SqlIndex250k()
        {

        }

        public SqlIndex250k(Index250k index)
        {
            this.MinLatitude = index.MinLatitude;

            this.MinLongitude = index.MinLongitude;

            this.SheetNameEn = index.SheetNameEn;

            this.SheetNameFa = index.SheetNameFa;

            this.SheetNumber = index.SheetNumber;
        }

        public SqlFeature AsSqlFeature()
        {
            return new SqlFeature()
            {
                TheSqlGeometry = (this as ISqlGeometryAware).TheSqlGeometry,
                LabelAttribute = nameof(this.SheetNameEn),
                Attributes = new Dictionary<string, object>()
                {
                    {nameof(this.Height), this.Height },
                    {nameof(this.Id), this.Id},
                    {nameof(this.MinLatitude), this.MinLatitude},
                    {nameof(this.MinLongitude), this.MinLongitude },
                    {nameof(this.SheetNameEn), this.SheetNameEn },
                    {nameof(this.SheetNameFa), this.SheetNameFa },
                    {nameof(this.SheetNumber), this.SheetNumber },
                    {nameof(this.Width), this.Width }
                },
            };
        }
    }
}
