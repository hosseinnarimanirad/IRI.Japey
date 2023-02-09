using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRI.Ket.SqlServerSpatialExtension.Model;
using Microsoft.SqlServer.Types;
using IRI.Ket.SqlServerSpatialExtension;
using IRI.Extensions;
using IRI.Msh.CoordinateSystem.MapProjection;
using IRI.Msh.Common.Mapping;
using IRI.Msh.Common.Primitives;
using System.Data;

namespace IRI.Ket.DataManagement.DataSource.MemorySources
{
    public class GridDataSource : FeatureDataSource<GeodeticSheet>
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

        public override List<Geometry<Point>> GetGeometries()
        {
            return null;
        }

        public override List<Geometry<Point>> GetGeometries(Geometry<Point> geometry)
        {
            //var geographicBoundingBox = geometry.GetBoundingBox().Transform(MapProjects.WebMercatorToGeodeticWgs84);

            //return MapIndexes.GetIndexLines(geographicBoundingBox, this.Type)
            //        .Select(g => g.AsSqlGeometry().Transform(MapProjects.GeodeticWgs84ToWebMercator, SridHelper.WebMercator))
            //        .ToList();

            return GetGeometries(geometry.GetBoundingBox());
        }

        public override List<Geometry<Point>> GetGeometries(BoundingBox boundingBox)
        {
            var geographicBoundingBox = boundingBox.Transform(MapProjects.WebMercatorToGeodeticWgs84);

            return GeodeticIndexes.GetIndexLines(geographicBoundingBox, this.Type)
                   .Select(g => g.Transform(MapProjects.GeodeticWgs84ToWebMercator, SridHelper.WebMercator))
                   .ToList();
        }

        public override List<Geometry<Point>> GetGeometriesForDisplay(double mapScale, BoundingBox boundingBox)
        {
            var geographicBoundingBox = boundingBox.Transform(MapProjects.WebMercatorToGeodeticWgs84);

            return GeodeticIndexes.GetIndexLines(geographicBoundingBox, this.Type)
                   .Select(g => g.Transform(MapProjects.GeodeticWgs84ToWebMercator, SridHelper.WebMercator))
                   .ToList();
        }

        public static GridDataSource Create(GeodeticIndexType indexType)
        {
            GridDataSource result = new GridDataSource();

            result.Type = indexType;

            return result;
        }

        public override List<GeodeticSheet> GetFeatures()
        {
            return GetFeatures(null);
        }

        public override List<GeodeticSheet> GetFeatures(Geometry<Point> geometry)
        {
            var geographicBoundingBox = geometry?.GetBoundingBox().Transform(MapProjects.WebMercatorToGeodeticWgs84) ?? this.GeodeticWgs84Extent;

            return GeodeticIndexes.FindIndexSheets(geographicBoundingBox, this.Type)
                                    .Where(s => s.TheGeometry?.Intersects(geometry) == true)
                                    .ToList();
        }

        public override FeatureSet GetSqlFeatures()
        {
            throw new NotImplementedException();
        }

        public override FeatureSet GetSqlFeatures(Geometry<Point> geometry)
        {
            throw new NotImplementedException();
        }

        public override List<NamedGeometry<Point>> GetGeometryLabelPairs(Geometry<Point> geometry)
        {
            var geographicBoundingBox = geometry.GetBoundingBox().Transform(MapProjects.WebMercatorToGeodeticWgs84);

            return GeodeticIndexes.FindIndexSheets(geographicBoundingBox, this.Type)
                           .Select(sheet => new NamedGeometry<Point>(sheet.TheGeometry, sheet.SheetName))
                           .Where(s => s.TheGeometry?.Intersects(geometry) == true)
                           .ToList();
        }

        public override void Add(IGeometryAware<Point> newValue)
        {
            throw new NotImplementedException();
        }

        public override void Remove(IGeometryAware<Point> value)
        {
            throw new NotImplementedException();
        }

        public override void Update(IGeometryAware<Point> newValue)
        {
            throw new NotImplementedException();
        }

        public override void UpdateFeature(IGeometryAware<Point> feature)
        {
            throw new NotImplementedException();
        }

        public override void SaveChanges()
        {
            throw new NotImplementedException();
        }
    }
}
