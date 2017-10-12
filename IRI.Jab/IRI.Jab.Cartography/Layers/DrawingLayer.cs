using IRI.Jab.Cartography.Model;
using IRI.Jab.Common;
using System;
using System.Collections.Generic;
using System.Windows.Media;
using sb = IRI.Ham.SpatialBase;
using IRI.Jab.Common.Model;

namespace IRI.Jab.Cartography
{
    public class DrawingLayer : BaseLayer
    {
        DrawMode _mode;

        //Path drawingPath;

        //List<sb.Point> geoPoints;

        //CancellationTokenSource drawingCancellationToken;

        //TaskCompletionSource<PointArrayEventArgs> taskCompletionSource;

        //bool displayDrawingPath = false;

        EditableFeatureLayer _editableFeatureLayer;

        //public EditableFeatureLayer TemporaryLayer
        //{
        //    get { return _editableFeatureLayer; }
        //}

        public override LayerType Type
        {
            get
            {
                return LayerType.EditableItem;
            }

            protected set
            {
                throw new NotImplementedException();
            }
        }

        public override sb.BoundingBox Extent
        {
            get
            {
                return this._editableFeatureLayer.Extent;
            }

            protected set
            {
                throw new NotImplementedException();
            }
        }

        public override RenderingApproach Rendering
        {
            get
            {
                return RenderingApproach.Default;
            }

            protected set
            {
                throw new NotImplementedException();
            }
        }

        public DrawingLayer(DrawMode mode, Transform toScreen, sb.Point startMercatorPoint)
        {
            this._mode = mode;

            sb.Primitives.GeometryType type;

            switch (mode)
            {
                case DrawMode.Point:
                    type = sb.Primitives.GeometryType.Point;
                    break;
                case DrawMode.Polyline:
                    type = sb.Primitives.GeometryType.LineString;
                    break;
                case DrawMode.Polygon:
                    type = sb.Primitives.GeometryType.Polygon;
                    break;
                case DrawMode.Rectange:
                case DrawMode.Freehand:
                default:
                    throw new NotImplementedException();
            }

            this._editableFeatureLayer = new EditableFeatureLayer("edit", new List<sb.Point>() { startMercatorPoint }, toScreen, type, true);

            this._editableFeatureLayer.OnRequestFinishDrawing += (sender, e) => { this.OnRequestFinishDrawing.SafeInvoke(this); };

            this._editableFeatureLayer.RequestFinishEditing = g =>
            {
                this.RequestFinishEditing?.Invoke(g);
            };

            this.VisibleRange = ScaleInterval.All;

            this.VisualParameters = new VisualParameters(mode == DrawMode.Polygon ? new SolidColorBrush(Colors.YellowGreen) : null, new SolidColorBrush(Colors.Blue), 3, 1);

        }

        public Action<sb.Primitives.Geometry> RequestFinishEditing;

        public event EventHandler OnRequestFinishDrawing;

        public EditableFeatureLayer GetLayer()
        {
            return this._editableFeatureLayer;
        }

        public void AddVertex(sb.Point mercatorPoint)
        {
            this._editableFeatureLayer.AddVertex(mercatorPoint);
        }

        public void UpdateLastVertexLocation(sb.Point point)
        {
            this._editableFeatureLayer.UpdateLastSemiVertexLocation(point);
        }

        public void AddSemiVertex(sb.Point mercatorPoint)
        {
            this._editableFeatureLayer.AddSemiVertex(mercatorPoint);
        }

        public sb.Primitives.Geometry GetFinalGeometry()
        {
            return this._editableFeatureLayer.GetFinalGeometry();
        }
    }
}
