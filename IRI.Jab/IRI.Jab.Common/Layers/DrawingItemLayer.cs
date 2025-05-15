using System;
using System.Collections.Generic;

using IRI.Sta.Spatial.Primitives; using IRI.Sta.Common.Primitives;
using IRI.Ket.Persistence.DataSources;
using IRI.Jab.Common.Model;
using IRI.Jab.Common.Model.Legend;

using Geometry = IRI.Sta.Spatial.Primitives.Geometry<IRI.Sta.Common.Primitives.Point>;
using System.Linq;
using IRI.Sta.SpatialReferenceSystem.MapProjections;

namespace IRI.Jab.Common
{
    public class DrawingItemLayer : VectorLayer, IIdentifiable
    {
        public Action<DrawingItemLayer>? RequestHighlightGeometry;

        public Action<DrawingItemLayer>? RequestChangeVisibility;


        public int Id { get; set; }

        public Guid HighlightGeometryKey { get; private set; }

        private Geometry? _geometry;
        public Geometry? Geometry
        {
            get { return _geometry; }
            set
            {
                _geometry = value;
                RaisePropertyChanged();

                this.DataSource = new MemoryDataSource(new List<Geometry<Point>>() { value });
                this.Extent = value.GetBoundingBox();
            }
        }

        public VectorDataSource<Feature<Point>, Point>? OriginalSource { get; set; }

        public SpecialPointLayer? SpecialPointLayer { get; set; }


        private DrawingItemLayer(string layerName, int id, RasterizationApproach rasterizationApproach)
        {
            this.Id = id;
            this.LayerName = layerName;
            this.Rendering = RenderingApproach.Default;
            this.ToRasterTechnique = rasterizationApproach;
            this.ZIndex = int.MaxValue;
            this.HighlightGeometryKey = Guid.NewGuid();
        }

        //internal DrawingItemLayer(
        //    string layerName,
        //    Geometry geometry,
        //    VisualParameters? visualParameters = null,
        //    int id = int.MinValue,
        //    VectorDataSource<Feature<Point>, Point>? source = null) : this(layerName, id, RasterizationApproach.DrawingVisual)
        //{
        //    this.Extent = geometry.GetBoundingBox();

        //    this.VisualParameters = visualParameters ?? VisualParameters.GetDefaultForDrawingItems();

        //    this.OriginalSource = source;

        //    this.Geometry = geometry;

        //    var featureType =
        //        (geometry.Type == GeometryType.Point || geometry.Type == GeometryType.MultiPoint) ? LayerType.Point :
        //        ((geometry.Type == GeometryType.LineString || geometry.Type == GeometryType.MultiLineString) ? LayerType.Polyline :
        //        (geometry.Type == GeometryType.Polygon || geometry.Type == GeometryType.MultiPolygon) ? LayerType.Polygon : LayerType.None);

        //    this.Type = LayerType.Drawing | featureType;

        //    this.Commands = new List<ILegendCommand>();

        //    this.OnIsSelectedInTocChanged += (sender, e) =>
        //    {
        //        this.RequestHighlightGeometry?.Invoke(this);
        //    };

        //    this.OnVisibilityChanged += (sender, e) =>
        //    {
        //        this.RequestChangeVisibility?.Invoke(this);
        //    };

        //    //HighlightGeometryKey = Guid.NewGuid();

        //    //this.Rendering = RenderingApproach.Default;

        //    //this.ToRasterTechnique = RasterizationApproach.GdiPlus;

        //    //this.Id = id;

        //    //this.ZIndex = int.MaxValue;

        //    //this.LayerName = layerName;
        //}

        public bool IsSpecialLayer() => SpecialPointLayer != null;

        public bool CanShowHighlightGeometry() => this.IsSelectedInToc && VisualParameters?.Visibility == System.Windows.Visibility.Visible;

        public static DrawingItemLayer CreateGeometryLayer(
            string layerName,
            Geometry geometry,
            VisualParameters? visualParameters = null,
            int id = int.MinValue,
            VectorDataSource<Feature<Point>, Point>? source = null)
        {
            DrawingItemLayer result = new DrawingItemLayer(layerName, id, RasterizationApproach.DrawingVisual);

            result.Extent = geometry.GetBoundingBox();

            result.VisualParameters = visualParameters ?? VisualParameters.GetDefaultForDrawingItems();

            result.OriginalSource = source;

            result.Geometry = geometry;

            var featureType =
                (geometry.Type == GeometryType.Point || geometry.Type == GeometryType.MultiPoint) ? LayerType.Point :
                ((geometry.Type == GeometryType.LineString || geometry.Type == GeometryType.MultiLineString) ? LayerType.Polyline :
                (geometry.Type == GeometryType.Polygon || geometry.Type == GeometryType.MultiPolygon) ? LayerType.Polygon : LayerType.None);

            result.Type = LayerType.Drawing | featureType;

            result.Commands = new List<ILegendCommand>();

            result.OnIsSelectedInTocChanged += (sender, e) =>
            {
                result.RequestHighlightGeometry?.Invoke(result);
            };

            result.OnVisibilityChanged += (sender, e) =>
            {
                result.RequestChangeVisibility?.Invoke(result);
            };

            return result;
        }

        public static DrawingItemLayer CreateSpecialLayer(
            string layerName,
            List<Locateable> locateables,
            int id = int.MinValue)
        {
            DrawingItemLayer result = new DrawingItemLayer(layerName, id, RasterizationApproach.DrawingVisual)
            {
                Extent = BoundingBox.CalculateBoundingBox(locateables.Select(l => new Point() { X = l.Location.X, Y = l.Location.Y })),

                VisualParameters = VisualParameters.GetDefaultForDrawingItems(),

                Type = LayerType.MoveableItem,
            };

            result.SpecialPointLayer = new SpecialPointLayer(layerName, locateables, .8, null, LayerType.MoveableItem) { ParentLayerId = result.LayerId };

            return result;
        }
    }
}
