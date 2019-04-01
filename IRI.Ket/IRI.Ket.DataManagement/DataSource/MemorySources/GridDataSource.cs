using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRI.Ket.SqlServerSpatialExtension.Model;
using Microsoft.SqlServer.Types;
using IRI.Ket.SqlServerSpatialExtension;
using IRI.Ket.SpatialExtensions;
using IRI.Msh.CoordinateSystem.MapProjection;
using IRI.Msh.Common.Mapping;
using IRI.Msh.Common.Primitives;
using IRI.Ket.SqlServerSpatialExtension.Mapping;
using System.Data;

namespace IRI.Ket.DataManagement.DataSource.MemorySources
{
    public class GridDataSource : FeatureDataSource<SqlGeodeticSheet>
    {
        ////In degree
        //public double GridWidth { get; private set; }

        ////In degree
        //public double GridHeight { get; private set; }

        public GeodeticIndexType Type { get; protected set; }

        public BoundingBox GeodeticWgs84Extent { get; set; }

        //web mercator extent
        public override BoundingBox Extent
        {
            get { return GeodeticWgs84Extent.Transform(MapProjects.GeodeticWgs84ToWebMercator); }

            protected set
            {
                throw new NotImplementedException();
            }
        }

        private GridDataSource()
        {
            this.ToDataTableMappingFunc = list =>
            {
                DataTable result = new DataTable();

                result.Columns.Add("SheetName");
                result.Columns.Add("SubTitle");
                result.Columns.Add("Longitude Min");
                result.Columns.Add("Longitude Max");
                result.Columns.Add("Latitude Min");
                result.Columns.Add("Latitude Max");
                result.Columns.Add("Type");
                result.Columns.Add("Geo");

                foreach (var item in list)
                {
                    result.Rows.Add(item.SheetName,
                                      item.SubTitle,
                                      item.GeodeticExtent.XMin,
                                      item.GeodeticExtent.XMax,
                                      item.GeodeticExtent.YMin,
                                      item.GeodeticExtent.YMax,
                                      item.Type,
                                      item.TheGeometry);
                }

                return result;
            };

            this.GeodeticWgs84Extent = BoundingBoxes.IranGeodeticWgs84BoundingBox;
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

            return GeodeticIndexes.GetIndexLines(geographicBoundingBox, this.Type)
                   .Select(g => g.AsSqlGeometry().Transform(MapProjects.GeodeticWgs84ToWebMercator, SridHelper.WebMercator))
                   .ToList();
        }

        public override List<SqlGeometry> GetGeometriesForDisplay(double mapScale, BoundingBox boundingBox)
        {
            var geographicBoundingBox = boundingBox.Transform(MapProjects.WebMercatorToGeodeticWgs84);

            return GeodeticIndexes.GetIndexLines(geographicBoundingBox, this.Type)
                   .Select(g => g.AsSqlGeometry().Transform(MapProjects.GeodeticWgs84ToWebMercator, SridHelper.WebMercator))
                   .ToList();
        }

        public static GridDataSource Create(GeodeticIndexType indexType)
        {
            GridDataSource result = new GridDataSource();

            result.Type = indexType;

            return result;
        }

        public override List<SqlGeodeticSheet> GetFeatures()
        {
            return GetFeatures(null);
        }

        public override List<SqlGeodeticSheet> GetFeatures(SqlGeometry geometry)
        {
            var geographicBoundingBox = geometry?.GetBoundingBox().Transform(MapProjects.WebMercatorToGeodeticWgs84) ?? this.GeodeticWgs84Extent;

            return GeodeticIndexes.FindIndexSheets(geographicBoundingBox, this.Type)
                                    .Select(s => new SqlGeodeticSheet(s))
                                    .Where(s => s.TheSqlGeometry?.STIntersects(geometry).IsTrue == true)
                                    .ToList();
        }

        public override List<NamedSqlGeometry> GetGeometryLabelPairs(SqlGeometry geometry)
        {
            var geographicBoundingBox = geometry.GetBoundingBox().Transform(MapProjects.WebMercatorToGeodeticWgs84);

            return GeodeticIndexes.FindIndexSheets(geographicBoundingBox, this.Type)
                           .Select(sheet => new NamedSqlGeometry(sheet.TheGeometry.AsSqlGeometry(), sheet.SheetName))
                           .Where(s => s.TheSqlGeometry?.STIntersects(geometry).IsTrue == true)
                           .ToList();
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
