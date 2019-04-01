using IRI.Ket.SpatialExtensions;
using IRI.Ket.SqlServerSpatialExtension.Mapping;
using IRI.Ket.SqlServerSpatialExtension.Model;
using IRI.Msh.Common.Mapping;
using IRI.Msh.Common.Primitives;
using IRI.Msh.CoordinateSystem.MapProjection;
using Microsoft.SqlServer.Types;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Ket.DataManagement.DataSource.MemorySources
{
    public class UtmGridDataSource : FeatureDataSource<SqlUtmSheet>
    {
        public int UtmZone { get; set; }

        public UtmIndexType Type { get; protected set; }

        public override BoundingBox Extent { get => throw new NotImplementedException(); protected set => throw new NotImplementedException(); }

        private UtmGridDataSource()
        {
            this.ToDataTableMappingFunc = list =>
            {
                DataTable result = new DataTable();
                result.Columns.Add("Geo");
                result.Columns.Add("SheetName");
                result.Columns.Add("UtmZone");
                result.Columns.Add("XMin (UTM)");
                result.Columns.Add("XMax (UTM)");
                result.Columns.Add("YMin (UTM)");
                result.Columns.Add("YMax (UTM)");
                result.Columns.Add("Type");
                result.Columns.Add("Row");
                result.Columns.Add("Column");

                foreach (var item in list)
                {
                    result.Rows.Add(item.TheGeometry,
                                      item.SheetName,
                                      item.UtmZone,
                                      item.UtmExtent.XMin,
                                      item.UtmExtent.XMax,
                                      item.UtmExtent.YMin,
                                      item.UtmExtent.YMax,
                                      item.Type,
                                      item.Row,
                                      item.Column);
                }

                return result;
            };
        }

        public override int GetSrid()
        {
            return SridHelper.WebMercator;
        }


        public override List<SqlGeometry> GetGeometries()
        {
            return null;
        }

        public override List<SqlGeometry> GetGeometries(SqlGeometry geometry)
        {
            //var geographicBoundingBox = geometry.GetBoundingBox().Transform(MapProjects.WebMercatorToGeodeticWgs84);

            //return MapIndexes.GetIndexLines(geographicBoundingBox, this.Type)
            //        .Select(g => g.AsSqlGeometry().Transform(MapProjects.GeodeticWgs84ToWebMercator, SridHelper.WebMercator))
            //        .ToList();

            return GetGeometries(geometry.GetBoundingBox());
        }

        public override List<SqlGeometry> GetGeometries(BoundingBox boundingBox)
        {
            var geographicBoundingBox = boundingBox.Transform(MapProjects.WebMercatorToGeodeticWgs84);

            return UtmIndexes.GetIndexLines(geographicBoundingBox, this.Type, UtmZone)
                   .Select(g => g.AsSqlGeometry().Transform(MapProjects.GeodeticWgs84ToWebMercator, SridHelper.WebMercator))
                   .ToList();
        }

        public override List<SqlGeometry> GetGeometriesForDisplay(double mapScale, BoundingBox boundingBox)
        {
            var geographicBoundingBox = boundingBox.Transform(MapProjects.WebMercatorToGeodeticWgs84);

            return UtmIndexes.GetIndexLines(geographicBoundingBox, this.Type, UtmZone)
                   .Select(g => g.AsSqlGeometry().Transform(MapProjects.GeodeticWgs84ToWebMercator, SridHelper.WebMercator))
                   .ToList();
        }

        public override List<NamedSqlGeometry> GetGeometryLabelPairs(SqlGeometry geometry)
        {
            var geographicBoundingBox = geometry.GetBoundingBox().Transform(MapProjects.WebMercatorToGeodeticWgs84);

            return UtmIndexes.GetIndexSheets(geographicBoundingBox, this.Type, UtmZone)
                           .Select(sheet => new NamedSqlGeometry(sheet.TheGeometry.AsSqlGeometry(), sheet.SheetName))
                            .Where(s => s.TheSqlGeometry?.STIntersects(geometry).IsTrue == true)
                           .ToList();
        }

        public static UtmGridDataSource Create(UtmIndexType indexType, int utmZone)
        {

            UtmGridDataSource result = new UtmGridDataSource();

            result.Type = indexType;

            result.UtmZone = utmZone;

            return result;
        }

        public override List<SqlUtmSheet> GetFeatures(SqlGeometry geometry)
        {
            var geographicBoundingBox = geometry.GetBoundingBox().Transform(MapProjects.WebMercatorToGeodeticWgs84);

            return UtmIndexes.GetIndexSheets(geographicBoundingBox, this.Type, UtmZone)
                                .Select(s => new SqlUtmSheet(s))
                                .Where(s => s.TheSqlGeometry?.STIntersects(geometry).IsTrue == true)
                                .ToList();
        }

        public override DataTable GetEntireFeatures(SqlGeometry geometry)
        {
            throw new NotImplementedException();
        }

        public override void Add(ISqlGeometryAware newValue)
        {
            throw new NotImplementedException();
        }

        public override void Remove(ISqlGeometryAware value)
        {
            throw new NotImplementedException();
        }

        public override void Update(ISqlGeometryAware newValue)
        {
            throw new NotImplementedException();
        }

        public override void UpdateFeature(ISqlGeometryAware feature)
        {
            throw new NotImplementedException();
        }

        public override void SaveChanges()
        {
            throw new NotImplementedException();
        }
    }
}
