using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRI.Extensions;
using IRI.Msh.CoordinateSystem.MapProjection;
using IRI.Msh.Common.Mapping;
using IRI.Msh.Common.Primitives;
using System.Data;

namespace IRI.Ket.Persistence.DataSources
{
    public class GridDataSource : VectorDataSource<GeodeticSheet, Point>
    {
        public GeodeticIndexType Type { get; protected set; }


        public override GeometryType? GeometryType
        {
            get { return IRI.Msh.Common.Primitives.GeometryType.Polygon; }
            protected set { throw new NotImplementedException("GridDataSource > GeometryType"); }
        }

        public BoundingBox GeodeticWgs84Extent { get; set; }

        //web mercator extent
        public override BoundingBox Extent
        {
            get
            {
                return GeodeticWgs84Extent.Transform(MapProjects.GeodeticWgs84ToWebMercator);
            }

            protected set
            {
                _ = value;
            }
        }

        public override int Srid { get => SridHelper.WebMercator; protected set => _ = value; }

        private GridDataSource()
        {
            //this.ToDataTableMappingFunc = list =>
            //{
            //    DataTable result = new DataTable();

            //    result.Columns.Add("SheetName");
            //    result.Columns.Add("SubTitle");
            //    result.Columns.Add("Longitude Min");
            //    result.Columns.Add("Longitude Max");
            //    result.Columns.Add("Latitude Min");
            //    result.Columns.Add("Latitude Max");
            //    result.Columns.Add("Type");
            //    result.Columns.Add("Geo");

            //    foreach (var item in list)
            //    {
            //        result.Rows.Add(item.SheetName,
            //                          item.SubTitle,
            //                          item.GeodeticExtent.XMin,
            //                          item.GeodeticExtent.XMax,
            //                          item.GeodeticExtent.YMin,
            //                          item.GeodeticExtent.YMax,
            //                          item.Type,
            //                          item.TheGeometry);
            //    }

            //    return result;
            //};

            this.GeodeticWgs84Extent = BoundingBoxes.IranGeodeticWgs84BoundingBox;
        }

        public override string ToString()
        {
            return $"GRID DATASOURCE {Type.GetName()}";
        }

        //public override List<Geometry<Point>> GetGeometries(BoundingBox boundingBox)
        //{
        //    var geographicBoundingBox = boundingBox.Transform(MapProjects.WebMercatorToGeodeticWgs84);

        //    return GeodeticIndexes.GetIndexLines(geographicBoundingBox, this.Type)
        //           .Select(g => g.Transform(MapProjects.GeodeticWgs84ToWebMercator, SridHelper.WebMercator))
        //           .ToList();
        //}


        public static GridDataSource Create(GeodeticIndexType indexType)
        {
            GridDataSource result = new GridDataSource();

            result.Type = indexType;

            return result;
        }

        public override List<GeodeticSheet> GetGeometryAwares(Geometry<Point>? geometry)
        {
            var geographicBoundingBox = geometry?.GetBoundingBox().Transform(MapProjects.WebMercatorToGeodeticWgs84) ?? this.GeodeticWgs84Extent;

            return GeodeticIndexes.FindIndexSheets(geographicBoundingBox, this.Type)
                                    //.Where(s => s.TheGeometry?.Intersects(geometry) == true)
                                    .ToList();
        }



        //public override List<NamedGeometry> GetGeometryLabelPairs(Geometry<Point> geometry)
        //{
        //    var geographicBoundingBox = geometry.GetBoundingBox().Transform(MapProjects.WebMercatorToGeodeticWgs84);

        //    return GeodeticIndexes.FindIndexSheets(geographicBoundingBox, this.Type)
        //                   .Select(sheet => new NamedGeometry(sheet.TheGeometry, sheet.SheetName))
        //                   .Where(s => s.TheGeometry?.Intersects(geometry) == true)
        //                   .ToList();
        //}


        #region CRUD

        //public override void Add(IGeometryAware<Point> newValue)
        //{
        //    Add(newValue as UtmSheet);
        //}

        //public override void Remove(IGeometryAware<Point> newValue)
        //{
        //    Remove(newValue as UtmSheet);
        //}

        //public override void Update(IGeometryAware<Point> newValue)
        //{
        //    Update(newValue as UtmSheet);
        //}

        public override void Add(GeodeticSheet newValue)
        {
            throw new NotImplementedException();
        }

        public override void Remove(GeodeticSheet value)
        {
            throw new NotImplementedException();
        }

        public override void Update(GeodeticSheet newValue)
        {
            throw new NotImplementedException();
        }

        public override void SaveChanges()
        {
            throw new NotImplementedException();
        }

        #endregion

        protected override Feature<Point> ToFeatureMappingFunc(GeodeticSheet geometryAware)
        {
            //this.ToDataTableMappingFunc = list =>
            //{
            //    DataTable result = new DataTable();

            //    result.Columns.Add("SheetName");
            //    result.Columns.Add("SubTitle");
            //    result.Columns.Add("Longitude Min");
            //    result.Columns.Add("Longitude Max");
            //    result.Columns.Add("Latitude Min");
            //    result.Columns.Add("Latitude Max");
            //    result.Columns.Add("Type");
            //    result.Columns.Add("Geo");

            //    foreach (var item in list)
            //    {
            //        result.Rows.Add(item.SheetName,
            //                          item.SubTitle,
            //                          item.GeodeticExtent.XMin,
            //                          item.GeodeticExtent.XMax,
            //                          item.GeodeticExtent.YMin,
            //                          item.GeodeticExtent.YMax,
            //                          item.Type,
            //                          item.TheGeometry);
            //    }

            //    return result;
            //};

            return new Feature<Point>()
            {
                Id = 1,
                LabelAttribute = nameof(geometryAware.SheetName),
                TheGeometry = geometryAware.TheGeometry,
                Attributes = new Dictionary<string, object>()
                {
                    { nameof(geometryAware.SheetName), geometryAware.SheetName},
                    { nameof(geometryAware.SubTitle), geometryAware.SubTitle},
                    { "Longitude Min", geometryAware.GeodeticExtent.XMin},
                    { "Longitude Max", geometryAware.GeodeticExtent.XMax},
                    { "Latitude Min", geometryAware.GeodeticExtent.YMin},
                    { "Latitude Max", geometryAware.GeodeticExtent.YMax},
                    { "Type", geometryAware.Type},
                }

            };
        }

        public override FeatureSet<Point> GetAsFeatureSet(Geometry<Point>? geometry)
        {
            var geographicBoundingBox = geometry?.GetBoundingBox().Transform(MapProjects.WebMercatorToGeodeticWgs84) ?? this.GeodeticWgs84Extent;

            return new FeatureSet<Point>(GeodeticIndexes.FindIndexSheets(geographicBoundingBox, this.Type)
                                    //.Where(s => s.TheGeometry?.Intersects(geometry) == true)
                                    .Select(ToFeatureMappingFunc)
                                    .ToList());
        }

        public override FeatureSet<Point> Search(string searchText)
        {
            throw new NotImplementedException();
        }
    }
}
