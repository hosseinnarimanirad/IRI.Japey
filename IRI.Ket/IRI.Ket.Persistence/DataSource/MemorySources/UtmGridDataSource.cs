using IRI.Extensions;
using IRI.Msh.Common.Mapping;
using IRI.Msh.Common.Primitives;
using IRI.Msh.CoordinateSystem.MapProjection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Ket.DataManagement.DataSource.MemorySources
{
    public class UtmGridDataSource : VectorDataSource<UtmSheet, Point>
    {
        public int UtmZone { get; set; }

        public UtmIndexType Type { get; protected set; }

        public override BoundingBox Extent { get => throw new NotImplementedException(); protected set => throw new NotImplementedException(); }

        private UtmGridDataSource()
        {
            //this.ToDataTableMappingFunc = list =>
            //{
            //    DataTable result = new DataTable();
            //    result.Columns.Add("Geo");
            //    result.Columns.Add("SheetName");
            //    result.Columns.Add("UtmZone");
            //    result.Columns.Add("XMin (UTM)");
            //    result.Columns.Add("XMax (UTM)");
            //    result.Columns.Add("YMin (UTM)");
            //    result.Columns.Add("YMax (UTM)");
            //    result.Columns.Add("Type");
            //    result.Columns.Add("Row");
            //    result.Columns.Add("Column");

            //    foreach (var item in list)
            //    {
            //        result.Rows.Add(item.TheGeometry,
            //                          item.SheetName,
            //                          item.UtmZone,
            //                          item.UtmExtent.XMin,
            //                          item.UtmExtent.XMax,
            //                          item.UtmExtent.YMin,
            //                          item.UtmExtent.YMax,
            //                          item.Type,
            //                          item.Row,
            //                          item.Column);
            //    }

            //    return result;
            //};
        }

        public override int Srid { get => SridHelper.WebMercator; protected set => _ = value; }

        //public override int GetSrid()
        //{
        //    return SridHelper.WebMercator;
        //}


        //public override List<Geometry<Point>> GetGeometries(Geometry<Point> geometry)
        //{
        //    //var geographicBoundingBox = geometry.GetBoundingBox().Transform(MapProjects.WebMercatorToGeodeticWgs84);

        //    //return MapIndexes.GetIndexLines(geographicBoundingBox, this.Type)
        //    //        .Select(g => g.AsSqlGeometry().Transform(MapProjects.GeodeticWgs84ToWebMercator, SridHelper.WebMercator))
        //    //        .ToList();

        //    return GetGeometries(geometry.GetBoundingBox());
        //}

        //public override List<Geometry<Point>> GetGeometries(BoundingBox boundingBox)
        //{
        //    var geographicBoundingBox = boundingBox.Transform(MapProjects.WebMercatorToGeodeticWgs84);

        //    return UtmIndexes.GetIndexLines(geographicBoundingBox, this.Type, UtmZone)
        //           .Select(g => g.Transform(MapProjects.GeodeticWgs84ToWebMercator, SridHelper.WebMercator))
        //           .ToList();
        //}

        //public override List<Geometry<Point>> GetGeometriesForDisplay(double mapScale, BoundingBox boundingBox)
        //{
        //    var geographicBoundingBox = boundingBox.Transform(MapProjects.WebMercatorToGeodeticWgs84);

        //    return UtmIndexes.GetIndexLines(geographicBoundingBox, this.Type, UtmZone)
        //           .Select(g => g.Transform(MapProjects.GeodeticWgs84ToWebMercator, SridHelper.WebMercator))
        //           .ToList();
        //}

        //public override List<NamedGeometry> GetGeometryLabelPairs(Geometry<Point> geometry)
        //{
        //    var geographicBoundingBox = geometry.GetBoundingBox().Transform(MapProjects.WebMercatorToGeodeticWgs84);

        //    return UtmIndexes.GetIndexSheets(geographicBoundingBox, this.Type, UtmZone)
        //                   .Select(sheet => new NamedGeometry(sheet.TheGeometry, sheet.SheetName))
        //                    .Where(s => s.TheGeometry?.Intersects(geometry) == true)
        //                   .ToList();
        //}

        //public override List<UtmSheet> GetFeatures(Geometry<Point> geometry)
        //{

        //}


        //public override FeatureSet<Point> GetSqlFeatures(Geometry<Point> geometry)
        //{
        //    var geographicBoundingBox = geometry.GetBoundingBox().Transform(MapProjects.WebMercatorToGeodeticWgs84);

        //    var features = UtmIndexes.GetIndexSheets(geographicBoundingBox, this.Type, UtmZone)
        //                        .Where(s => s.TheGeometry?.Intersects(geometry) == true)
        //                        .Select(s => new Feature()
        //                        {
        //                            TheGeometry = s.TheGeometry,
        //                            LabelAttribute = nameof(s.SheetName),
        //                            Id = s.Id,
        //                            Attributes = new Dictionary<string, object>()
        //                            {
        //                                {nameof(s.Column), s.Column},
        //                                {nameof(s.Id), s.Id},
        //                                {nameof(s.Row), s.Row},
        //                                {nameof(s.SheetName), s.SheetName},
        //                                {nameof(s.UtmZone), s.UtmZone},
        //                                {nameof(s.Type), s.Type},
        //                                {nameof(s.Row), s.Row},
        //                            }
        //                        })
        //                        .ToList();

        //    return new FeatureSet<Point>(features.Cast<Feature<Point>>().ToList());
        //}



        protected override Feature<Point> ToFeatureMappingFunc(UtmSheet geometryAware)
        {
            return new Feature<Point>()
            {
                TheGeometry = geometryAware.TheGeometry,
                LabelAttribute = nameof(geometryAware.SheetName),
                Id = geometryAware.Id,
                Attributes = new Dictionary<string, object>()
                                    {
                                        {nameof(geometryAware.Column), geometryAware.Column},
                                        {nameof(geometryAware.Id), geometryAware.Id},
                                        {nameof(geometryAware.Row), geometryAware.Row},
                                        {nameof(geometryAware.SheetName), geometryAware.SheetName},
                                        {nameof(geometryAware.UtmZone), geometryAware.UtmZone},
                                        {nameof(geometryAware.Type), geometryAware.Type},
                                        {nameof(geometryAware.Row), geometryAware.Row},
                                    }
            };
        }

        public override List<UtmSheet> GetGeometryAwares(Geometry<Point>? geometry)
        {
            var geographicBoundingBox = geometry.GetBoundingBox().Transform(MapProjects.WebMercatorToGeodeticWgs84);

            return UtmIndexes.GetIndexSheets(geographicBoundingBox, this.Type, UtmZone)
                                .Where(s => s.TheGeometry?.Intersects(geometry) == true)
                                .ToList();
        }


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


        public override void Remove(UtmSheet value)
        {
            throw new NotImplementedException();
        }

        public override void Update(UtmSheet newValue)
        {
            throw new NotImplementedException();
        }

        public override void Add(UtmSheet newValue)
        {
            throw new NotImplementedException();
        }


        public override void SaveChanges()
        {
            throw new NotImplementedException();
        }

        #endregion

        public static UtmGridDataSource Create(UtmIndexType indexType, int utmZone)
        {

            UtmGridDataSource result = new UtmGridDataSource();

            result.Type = indexType;

            result.UtmZone = utmZone;

            return result;
        }

        public override FeatureSet<Point> GetAsFeatureSet(Geometry<Point>? geometry)
        {
            if (geometry.IsNullOrEmpty())
            {
                return null;
            }

            var geographicBoundingBox = geometry.GetBoundingBox().Transform(MapProjects.WebMercatorToGeodeticWgs84);

            var features = UtmIndexes.GetIndexSheets(geographicBoundingBox, this.Type, UtmZone)
                                .Where(s => s.TheGeometry?.Intersects(geometry) == true)
                                .Select(s => ToFeatureMappingFunc(s))
                                .ToList();

            return new FeatureSet<Point>(features);
        }
    }
}
