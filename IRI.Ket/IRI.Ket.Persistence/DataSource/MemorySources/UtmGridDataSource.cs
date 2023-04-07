﻿using IRI.Extensions;
using IRI.Msh.Common.Mapping;
using IRI.Msh.Common.Primitives;
using IRI.Msh.CoordinateSystem.MapProjection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Ket.Persistence.DataSources
{
    public class UtmGridDataSource : VectorDataSource<UtmSheet, Point>
    {
        public int UtmZone { get; set; }

        public UtmIndexType Type { get; protected set; }

        public BoundingBox GeodeticWgs84Extent { get; set; }

        public override BoundingBox WebMercatorExtent
        {
            get => GeodeticWgs84Extent.Transform(MapProjects.GeodeticWgs84ToWebMercator);
            protected set => _ = value;
        }

        public override int Srid { get => SridHelper.WebMercator; protected set => _ = value; }

        public override GeometryType? GeometryType
        {
            get => IRI.Msh.Common.Primitives.GeometryType.Polygon;
            protected set => _ = value;
        }

        private UtmGridDataSource()
        {
            this.GeodeticWgs84Extent = BoundingBoxes.IranGeodeticWgs84BoundingBox;
        }

        public override string ToString()
        {
            return $"UtmGridDataSource {Type.GetName()}";
        }


        // Get GeometryAwares [GENERIC]
        public override List<UtmSheet> GetGeometryAwares(BoundingBox boundingBox)
        {
            var geographicBoundingBox = boundingBox.Transform(MapProjects.WebMercatorToGeodeticWgs84);

            return UtmIndexes.GetIndexSheets(geographicBoundingBox, this.Type, UtmZone);
        }

        public override List<UtmSheet> GetGeometryAwares(Geometry<Point>? geometry)
        {
            var geographicBoundingBox = geometry?.GetBoundingBox().Transform(MapProjects.WebMercatorToGeodeticWgs84) ?? this.GeodeticWgs84Extent;

            return UtmIndexes.GetIndexSheets(geographicBoundingBox, this.Type, UtmZone)
                                .Where(s => s.TheGeometry?.Intersects(geometry) == true)
                                .ToList();
        }


        // Get as FeatureSet of Point
        public override FeatureSet<Point> GetAsFeatureSetOfPoint(BoundingBox boundingBox)
        {
            var geographicBoundingBox = boundingBox.Transform(MapProjects.WebMercatorToMercatorWgs84);

            var features = UtmIndexes.GetIndexSheets(geographicBoundingBox, this.Type, UtmZone)
                                //.Where(s => s.TheGeometry?.Intersects(boundingBox) == true)
                                .Select(ToFeatureMappingFunc)
                                .ToList();

            return new FeatureSet<Point>(features);
        }

        public override FeatureSet<Point> GetAsFeatureSetOfPoint(Geometry<Point>? geometry)
        {
            var geographicBoundingBox = geometry?.GetBoundingBox().Transform(MapProjects.WebMercatorToGeodeticWgs84) ?? this.GeodeticWgs84Extent;

            var features = UtmIndexes.GetIndexSheets(geographicBoundingBox, this.Type, UtmZone)
                                .Where(s => s.TheGeometry?.Intersects(geometry) == true)
                                .Select(ToFeatureMappingFunc)
                                .ToList();

            return new FeatureSet<Point>(features);
        }


        public override FeatureSet<Point> Search(string searchText)
        {
            throw new NotImplementedException();
        }

        protected override Feature<Point> ToFeatureMappingFunc(UtmSheet geometryAware)
        {
            return new Feature<Point>()
            {
                Id = geometryAware.Id,
                LabelAttribute = nameof(geometryAware.SheetName),
                TheGeometry = geometryAware.TheGeometry,
                Attributes = new Dictionary<string, object>()
                                    {
                                        {nameof(geometryAware.Id), geometryAware.Id},
                                        {nameof(geometryAware.SheetName), geometryAware.SheetName},
                                        {nameof(geometryAware.UtmZone), geometryAware.UtmZone},
                                        {nameof(geometryAware.Type), geometryAware.Type},
                                        {nameof(geometryAware.Row), geometryAware.Row},
                                        {nameof(geometryAware.Column), geometryAware.Column},
                                    }
            };
        }

        public static UtmGridDataSource Create(UtmIndexType indexType, int utmZone)
        {
            UtmGridDataSource result = new UtmGridDataSource();

            result.Type = indexType;

            result.UtmZone = utmZone;

            return result;
        }

        #region Old code

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


        #endregion
    }
}
