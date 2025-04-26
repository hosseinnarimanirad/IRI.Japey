using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRI.Extensions;
using IRI.Msh.CoordinateSystem.MapProjection;
using IRI.Sta.Common.Mapping;
using IRI.Sta.Common.Primitives;
using System.Data;

namespace IRI.Ket.Persistence.DataSources
{
    public class GridDataSource : VectorDataSource<GeodeticSheet, Point>
    {
        public GeodeticIndexType Type { get; protected set; }

        public BoundingBox GeodeticWgs84Extent { get; set; }

        public override BoundingBox WebMercatorExtent
        {
            get => GeodeticWgs84Extent.Transform(MapProjects.GeodeticWgs84ToWebMercator);
            protected set => _ = value;
        }

        public override int Srid { get => SridHelper.WebMercator; protected set => _ = value; }

        public override GeometryType? GeometryType
        {
            get => IRI.Sta.Common.Primitives.GeometryType.Polygon;
            protected set => _ = value;
        }

        private GridDataSource()
        {
            this.GeodeticWgs84Extent = BoundingBoxes.IranGeodeticWgs84BoundingBox;
        }

        public override string ToString()
        {
            return $"GridDataSource {Type.GetName()}";
        }


        // Get GeometryAwares [GENERIC]
        public override List<GeodeticSheet> GetGeometryAwares(BoundingBox boundingBox)
        {
            var geographicBoundingBox = boundingBox.Transform(MapProjects.WebMercatorToGeodeticWgs84);

            return GeodeticIndexes.FindIndexSheets(geographicBoundingBox, this.Type);
        }

        public override List<GeodeticSheet> GetGeometryAwares(Geometry<Point>? geometry)
        {
            var geographicBoundingBox = geometry?.GetBoundingBox().Transform(MapProjects.WebMercatorToGeodeticWgs84) ?? this.GeodeticWgs84Extent;

            return GeodeticIndexes.FindIndexSheets(geographicBoundingBox, this.Type)
                                    .Where(s => s.TheGeometry?.Intersects(geometry) == true)
                                    .ToList();
        }

        // Get as FeatureSet of Point
        public override FeatureSet<Point> GetAsFeatureSetOfPoint(BoundingBox boundingBox)
        {
            var geographicBoundingBox = boundingBox.Transform(MapProjects.WebMercatorToGeodeticWgs84);

            return new FeatureSet<Point>(GeodeticIndexes.FindIndexSheets(geographicBoundingBox, this.Type)
                                    .Select(ToFeatureMappingFunc)
                                    .ToList());
        }

        public override FeatureSet<Point> GetAsFeatureSetOfPoint(Geometry<Point>? geometry)
        {
            var geographicBoundingBox = geometry?.GetBoundingBox().Transform(MapProjects.WebMercatorToGeodeticWgs84) ?? this.GeodeticWgs84Extent;

            return new FeatureSet<Point>(GeodeticIndexes.FindIndexSheets(geographicBoundingBox, this.Type)
                                    .Where(s => s.TheGeometry?.Intersects(geometry) == true)
                                    .Select(ToFeatureMappingFunc)
                                    .ToList());
        }


        public override FeatureSet<Point> Search(string searchText)
        {
            throw new NotImplementedException();
        }

        protected override Feature<Point> ToFeatureMappingFunc(GeodeticSheet geometryAware)
        {
            return new Feature<Point>()
            {
                Id = geometryAware.Id,
                LabelAttribute = nameof(geometryAware.SheetName),
                TheGeometry = geometryAware.TheGeometry,
                Attributes = new Dictionary<string, object>()
                {
                    {nameof(geometryAware.Id), geometryAware.Id},
                    { nameof(geometryAware.SheetName), geometryAware.SheetName},
                    { nameof(geometryAware.SubTitle), geometryAware.SubTitle},
                    { nameof(geometryAware.Type), geometryAware.Type},
                    { "Min Longitude", geometryAware.GeodeticExtent.XMin},
                    { "Max Longitude", geometryAware.GeodeticExtent.XMax},
                    { "Min Latitude", geometryAware.GeodeticExtent.YMin},
                    { "Max Latitude", geometryAware.GeodeticExtent.YMax},
                }
            };
        }

        public static GridDataSource Create(GeodeticIndexType indexType)
        {
            GridDataSource result = new GridDataSource();

            result.Type = indexType;

            return result;
        }

    }
}
