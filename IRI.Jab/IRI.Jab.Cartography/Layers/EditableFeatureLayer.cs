using System;
using System.Collections.Generic;
using System.Linq;
using IRI.Jab.Common;
using System.Windows.Shapes;
using System.Windows.Media;
using IRI.Jab.Common.Extensions;
using IRI.Ham.CoordinateSystem;
using IRI.Ham.SpatialBase;
using System.Windows;
using System.Windows.Input;
using IRI.Jab.Cartography.Model.DataStructure;
using WpfPoint = System.Windows.Point;
using WpfGeometry = System.Windows.Media.Geometry;
using Geometry = IRI.Ham.SpatialBase.Primitives.Geometry;
using Point = IRI.Ham.SpatialBase.Point;
using IRI.Ham.SpatialBase.Primitives;
using IRI.Jab.Common.Model;
using IRI.Ham.CoordinateSystem.MapProjection;

namespace IRI.Jab.Cartography
{
    public class EditableFeatureLayer : BaseLayer
    {
        static readonly Brush _stroke = BrushHelper.FromHex("#FF1CA1E2");
        static readonly Brush _fill = BrushHelper.FromHex("#661CA1E2");
        string _delete = "حذف";
        string _copy = "کپی";
        string _move = "انتقال";

        //private bool _isClosed;

        private Geometry _mercatorGeometry;

        private Path _feature;

        private PathGeometry _pathGeometry;

        RecursiveCollection<Locateable> _vertices;

        RecursiveCollection<Locateable> _midVertices;


        private SpecialPointLayer _primaryVerticesLayer;

        private SpecialPointLayer _midVerticesLayer;

        Transform _toScreen;

        bool _isNewDrawingMode = false;

        bool IsRingBase()
        {
            return this._mercatorGeometry.Type == GeometryType.Polygon ||
                this._mercatorGeometry.Type == GeometryType.MultiPolygon ||
                this._mercatorGeometry.Type == GeometryType.CurvePolygon;

        }

        public Action<FrameworkElement, MouseButtonEventArgs, ILocateable> RequestRightClickOptions;

        public Action RequestRemoveRightClickOptions;

        public Action<EditableFeatureLayer> Refresh;

        public event EventHandler OnRequestFinishDrawing;

        public Action<Geometry> RequestFinishEditing;

        public override BoundingBox Extent
        {
            get
            {
                return _mercatorGeometry.GetBoundingBox();
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

        private WpfPoint ToScreen(WpfPoint point)
        {
            return _toScreen.Transform(point);
        }


        /// <summary>
        /// For Polygons do not repeat first point in the last point
        /// </summary>
        /// <param name="name"></param>
        /// <param name="mercatorPoints"></param>
        /// <param name="isClosed"></param>
        public EditableFeatureLayer(string name, List<Point> mercatorPoints, Transform toScreen, GeometryType type, bool isNewDrawing = false)
            : this(name, Geometry.Create(mercatorPoints.Cast<IPoint>().ToArray(), type), toScreen, isNewDrawing)
        {

        }

        ///// <summary>
        ///// For Polygons do not repeat first point in the last point
        ///// </summary>
        ///// <param name="name"></param>
        ///// <param name="mercatorPoints"></param>
        ///// <param name="isClosed"></param>
        //public EditableFeatureLayer(string name, List<Point> mercatorPoints, Transform toScreen, GeometryType type, bool isNewDrawing = false)
        //    : this(name, new Geometry(mercatorPoints.Cast<IPoint>().ToArray(), type), toScreen, isNewDrawing)
        //{

        //}

        public EditableFeatureLayer(string name, Geometry mercatorGeometry, Transform toScreen, bool isNewDrawing = false)
        {
            this._isNewDrawingMode = isNewDrawing;

            this.LayerName = name;

            this._mercatorGeometry = mercatorGeometry;

            this.Id = Guid.NewGuid();

            this._toScreen = toScreen;

            this.VisibleRange = ScaleInterval.All;

            this.VisualParameters = new VisualParameters(IsRingBase() ? _fill : null, _stroke, 3, 1);

            //if (  mercatorPoints.Count < 1)
            //    return;

            this._feature = GetDefaultEditingPath();

            this._pathGeometry = new PathGeometry();

            MakePathGeometry();

            this._feature.Data = this._pathGeometry;

            if (!isNewDrawing)
            {
                this._feature.MouseRightButtonDown += (sender, e) => { this.RegisterMapOptionsForPath(e); };
            }

            var layerType = _isNewDrawingMode ? LayerType.EditableItem : LayerType.MoveableItem | LayerType.EditableItem;

            this._primaryVerticesLayer = new SpecialPointLayer("vert", new List<Locateable>(), 1, ScaleInterval.All, layerType) { AlwaysTop = true };

            this._midVerticesLayer = new SpecialPointLayer("int. vert", new List<Locateable>(), .7, ScaleInterval.All, layerType) { AlwaysTop = true };

            ReconstructLocateables();

            if (this._isNewDrawingMode)
            {
                this.AddSemiVertex((Point)(mercatorGeometry.Points == null ? mercatorGeometry.Geometries.Last().Points.Last() : mercatorGeometry.Points.Last()));
            }
        }


        private Path GetDefaultEditingPath()
        {
            var result = new Path()
            {
                Stroke = _stroke,
                StrokeThickness = 4,
                Opacity = .9,
            };

            if (IsRingBase())
            {
                result.Fill = _fill;
            }

            return result;
        }



        private void ReconstructLocateables()
        {
            this._vertices = new RecursiveCollection<Locateable>();

            this._midVertices = new RecursiveCollection<Locateable>();

            MakeLocateables(this._mercatorGeometry, _vertices, _midVertices);

            //this._primaryVerticesLayer = new SpecialPointLayer("vert", this._vertices.GetFlattenCollection(), ScaleInterval.All, layerType, 1);

            //this._midVerticesLayer = new SpecialPointLayer("int. vert", this._midVertices.GetFlattenCollection(), ScaleInterval.All, layerType, .7);

            this._primaryVerticesLayer.Items.Clear();
            this._midVerticesLayer.Items.Clear();

            var primary = _vertices.GetFlattenCollection();

            var mid = _midVertices.GetFlattenCollection();

            foreach (var item in mid)
            {
                _midVerticesLayer.Items.Add(item);
            }

            foreach (var item in primary)
            {
                _primaryVerticesLayer.Items.Add(item);
            }





            //this._vertices = new List<List<Locateable>>();

            //this._midVertices = new List<List<Locateable>>();

            //if (this._mercatorGeometry.Points != null)
            //{
            //    MakeLocateables(this._mercatorGeometry.Points);
            //}
            //else
            //{
            //    foreach (var item in this._mercatorGeometry.Geometries)
            //    {
            //        MakeLocateables(item.Points);
            //    }
            //}

            //var layerType = _isNewDrawingMode ? LayerType.EditableItem : LayerType.MoveableItem | LayerType.EditableItem;

            //this._primaryVerticesLayer = new SpecialPointLayer("vert", this._vertices.SelectMany(i => i), ScaleInterval.All, layerType, 1);

            //this._midVerticesLayer = new SpecialPointLayer("int. vert", this._midVertices.SelectMany(i => i), ScaleInterval.All, layerType, .7);
        }

        private void MakeLocateables(Geometry geometry, RecursiveCollection<Locateable> primaryCollection, RecursiveCollection<Locateable> midCollection)
        {
            if (geometry.Points != null)
            {
                primaryCollection.Values = new List<Locateable>();

                midCollection.Values = new List<Locateable>();

                for (int i = 0; i < geometry.Points.Length; i++)
                {
                    var locateable = ToPrimaryLocateable(geometry.Points[i]);

                    //if (i == 0 && _isNewDrawingMode && IsRingBase())
                    //{
                    //locateable.Element.MouseDown += (sender, e) => { this.OnRequestFinishDrawing.SafeInvoke(this); };
                    //}
                    //else if (i == geometry.Points.Length - 1 && _isNewDrawingMode && !IsRingBase())
                    //{
                    //    locateable.Element.MouseDown += (sender, e) => { this.OnRequestFinishDrawing.SafeInvoke(this); };
                    //}

                    primaryCollection.Values.Add(locateable);

                    //do not make mid points in drawing mode
                    if (_isNewDrawingMode)
                        continue;

                    if (geometry.Type == GeometryType.Point || geometry.Type == GeometryType.MultiPoint)
                        continue;

                    if (i == geometry.Points.Length - 1)
                    {
                        if (IsRingBase())
                        {
                            midCollection.Values.Add(ToSecondaryLocateable(geometry.Points[i], geometry.Points[0]));
                        }
                    }
                    else
                    {
                        midCollection.Values.Add(ToSecondaryLocateable(geometry.Points[i], geometry.Points[i + 1]));
                    }
                }
            }
            else
            {
                primaryCollection.Collections = new List<RecursiveCollection<Locateable>>();

                midCollection.Collections = new List<RecursiveCollection<Locateable>>();

                foreach (var item in geometry.Geometries)
                {
                    RecursiveCollection<Locateable> subPrimaryCollection = new RecursiveCollection<Locateable>();

                    RecursiveCollection<Locateable> subMidCollection = new RecursiveCollection<Locateable>();

                    MakeLocateables(item, subPrimaryCollection, subMidCollection);

                    primaryCollection.Collections.Add(subPrimaryCollection);

                    midCollection.Collections.Add(subMidCollection);
                }
            }
        }

        private void MakePathGeometry()
        {
            _pathGeometry.Figures.Clear();

            MakePathGeometry(this._mercatorGeometry);
        }

        private void MakePathGeometry(Geometry geometry)
        {
            if (geometry.Points != null)
            {
                PathFigure pathFigure = new PathFigure() { IsClosed = IsRingBase(), StartPoint = ToScreen(geometry.Points.First().AsWpfPoint()) };

                for (int i = 1; i < geometry.Points.Length; i++)
                {
                    var segment = new LineSegment(ToScreen(geometry.Points[i].AsWpfPoint()), true);

                    pathFigure.Segments.Add(segment);
                }

                this._pathGeometry.Figures.Add(pathFigure);
            }
            else if (geometry.Geometries != null)
            {
                foreach (var g in geometry.Geometries)
                {
                    MakePathGeometry(g);
                }
            }
            else
            {
                return;
            }
        }

        private Locateable ToPrimaryLocateable(IPoint point)
        {
            var mercatorPoint = point;

            var element = new Common.View.MapMarkers.Circle(1);

            var locateable = new Locateable(Model.AncherFunctionHandlers.CenterCenter) { Element = element, X = mercatorPoint.X, Y = mercatorPoint.Y };

            if (_isNewDrawingMode)
            {
                locateable.Element.MouseDown += (sender, e) => { this.OnRequestFinishDrawing.SafeInvoke(this); };
            }
            else
            {
                element.MouseRightButtonDown += (sender, e) =>
                {
                    RegisterMapOptionsForVertices(e, mercatorPoint);
                };
            }

            locateable.OnPositionChanged += (sender, e) =>
            {
                UpdateLineSegment(mercatorPoint as Point, new Point(locateable.X, locateable.Y));

                mercatorPoint.X = locateable.X;
                mercatorPoint.Y = locateable.Y;
            };

            return locateable;
        }

        private Locateable ToSecondaryLocateable(IPoint first, IPoint second)
        {
            var mercatorPoint = new Point((first.X + second.X) / 2.0, (first.Y + second.Y) / 2.0);

            var element = new Common.View.MapMarkers.Circle(.6);

            var locateable = new Locateable(Model.AncherFunctionHandlers.CenterCenter) { Element = element, X = mercatorPoint.X, Y = mercatorPoint.Y };

            element.MouseLeftButtonDown += (sender, e) =>
            {
                mercatorPoint.X = locateable.X;

                mercatorPoint.Y = locateable.Y;

                if (!TryInsertPoint(mercatorPoint, first, second, this._mercatorGeometry))
                    throw new NotImplementedException();

                Refresh?.Invoke(this);
            };

            return locateable;
        }


        public Path GetPath(Transform transform)
        {
            this._toScreen = transform;

            MakePathGeometry();

            return this._feature;
        }

        public SpecialPointLayer GetVertices()
        {
            //return new SpecialPointLayer("vert", this._vertices.SelectMany(i => i), ScaleInterval.All, LayerType.MoveableItem | LayerType.EditableItem, 1);
            return _primaryVerticesLayer;
        }

        public SpecialPointLayer GetMidVertices()
        {
            //return new SpecialPointLayer("int. vert", this._midVertices.SelectMany(i => i), ScaleInterval.All, LayerType.MoveableItem | LayerType.EditableItem, .7);
            return _midVerticesLayer;
        }

        public Geometry GetFinalGeometry()
        {
            return this._mercatorGeometry;
        }


        private void RegisterMapOptionsForVertices(MouseButtonEventArgs e, IPoint point)
        {
            var presenter = new Jab.Common.Presenters.MapOptions.MapOptionsPresenter(
                rightToolTip: _copy,
                leftToolTip: _move,
                middleToolTip: _delete,

                rightSymbol: IRI.Jab.Common.Assets.ShapeStrings.Appbar.appbarClipboard,
                leftSymbol: IRI.Jab.Common.Assets.ShapeStrings.Appbar.appbarCheck,
                middleSymbol: IRI.Jab.Common.Assets.ShapeStrings.Appbar.appbarDelete);

            presenter.RightCommandAction = i =>
            {
                var geodetic = MapProjects.MercatorToGeodetic(presenter.Location);

                Clipboard.SetDataObject($"{geodetic.X.ToString("n4")},{geodetic.Y.ToString("n4")}");

                this.RemoveMapOptions();
            };

            presenter.LeftCommandAction = i =>
            {
                RequestFinishEditing?.Invoke(this._mercatorGeometry);

                this.RemoveMapOptions();
            };

            presenter.MiddleCommandAction = i =>
            {
                TryDeleteVertex(point, this._mercatorGeometry, _mercatorGeometry.Type == GeometryType.Polygon);

                Refresh?.Invoke(this);

                this.RemoveMapOptions();
            };

            RequestRightClickOptions?.Invoke(new Common.View.MapOptions.MapThreeOptions(), e, presenter);

        }

        private void RegisterMapOptionsForPath(MouseButtonEventArgs e)
        {
            var presenter = new Jab.Common.Presenters.MapOptions.MapOptionsPresenter(
                rightToolTip: string.Empty,
                leftToolTip: string.Empty,
                middleToolTip: string.Empty,

                leftSymbol: IRI.Jab.Common.Assets.ShapeStrings.Appbar.appbarClose,
                rightSymbol: IRI.Jab.Common.Assets.ShapeStrings.Appbar.appbarCheck,
                middleSymbol: null);

            presenter.RightCommandAction = i =>
            {
                RequestFinishEditing?.Invoke(this._mercatorGeometry);

                this.RemoveMapOptions();
            };

            presenter.LeftCommandAction = i =>
            {
                RequestFinishEditing?.Invoke(this._mercatorGeometry);

                this.RemoveMapOptions();
            };

            RequestRightClickOptions?.Invoke(new Common.View.MapOptions.MapTwoOptions(false), e, presenter);

        }

        private void RemoveMapOptions()
        {
            this.RequestRemoveRightClickOptions?.Invoke();
        }


        private int GetLeftMidPointIndex(int primaryIndex, int length)
        {
            int left;

            if (primaryIndex == 0 && IsRingBase())
            {
                left = length - 1;
            }
            else if (primaryIndex == 0 && !IsRingBase())
            {
                left = int.MinValue;
            }
            else
            {
                left = primaryIndex - 1;
            }

            return left;
        }

        private int GetRightMidPointIndex(int primaryIndex, int length)
        {
            int right;

            if (primaryIndex == length && !IsRingBase())
            {
                right = int.MinValue;
            }
            else
            {
                right = primaryIndex;
            }

            return right;
        }


        private bool TryInsertPoint(Point mercatorPoint, IPoint startLineSegment, IPoint endLineSegment, Geometry geometry)
        {
            var point = this.ToScreen(mercatorPoint.AsWpfPoint());

            if (geometry.Points != null)
            {
                for (int i = 0; i < geometry.Points.Length; i++)
                {
                    if (geometry.Points[i] == startLineSegment)
                    {
                        //this._pathGeometry.Figures.First().Segments.Insert(i, new LineSegment(new System.Windows.Point(point.X, point.Y), true));

                        //var points = this._mercatorGeometry.Points.ToList();
                        //points.Insert(i + 1, mercatorPoint);
                        //this._mercatorGeometry.Points = points.ToArray();
                        geometry.InsertPoint(mercatorPoint, i + 1);

                        ReconstructLocateables();

                        //MakePathGeometry();

                        return true;
                    }
                }
            }
            else
            {
                for (int g = 0; g < geometry.Geometries.Length; g++)
                {
                    if (TryInsertPoint(mercatorPoint, startLineSegment, endLineSegment, geometry.Geometries[g]))
                        return true;

                }
            }

            return false;
        }

        private bool TryDeleteVertex(IPoint point, Geometry geometry, bool isRing)
        {
            if (geometry.Points != null)
            {
                var minimumPoints = isRing ? 3 : 2;

                if (geometry.Points.Count() <= minimumPoints)
                    return false;

                for (int i = 0; i < geometry.Points.Length; i++)
                {
                    if (geometry.Points[i] == point)
                    {
                        geometry.Remove(geometry.Points[i]);

                        MakePathGeometry();

                        ReconstructLocateables();

                        return true;
                    }
                }
            }
            else
            {
                for (int g = 0; g < geometry.Geometries.Length; g++)
                {
                    if (TryDeleteVertex(point, geometry.Geometries[g], geometry.Type == GeometryType.Polygon))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private void UpdateLineSegment(Point point, Point newValue)
        {
            var oldPoint = ToScreen(point.AsWpfPoint());

            var screenPoint = ToScreen(newValue.AsWpfPoint());

            for (int f = 0; f < this._pathGeometry.Figures.Count; f++)
            {
                var dif = this._pathGeometry.Figures[f].StartPoint - oldPoint;

                if (this._pathGeometry.Figures[f].StartPoint.AsPoint().AreExactlyTheSame(oldPoint.AsPoint()))
                {
                    this._pathGeometry.Figures[f].StartPoint = screenPoint;
                }
                else
                {
                    for (int s = 0; s < this._pathGeometry.Figures[f].Segments.Count; s++)
                    {
                        var lineSegment = (this._pathGeometry.Figures[f].Segments[s] as LineSegment);

                        dif = lineSegment.Point - oldPoint;

                        if (lineSegment.Point.AsPoint().AreExactlyTheSame(oldPoint.AsPoint()))
                        {
                            lineSegment.Point = screenPoint;
                        }
                    }
                }
            }

            var matched = UpdateLineSegment(point, newValue, this._mercatorGeometry, this._midVertices);

            if (!matched)
            {
                throw new NotImplementedException();
            }
        }

        private bool UpdateLineSegment(Point point, Point newPoint, Geometry geometry, RecursiveCollection<Locateable> midCollection)
        {
            if (geometry.Points != null)
            {
                for (int i = 0; i < geometry.Points.Length; i++)
                {
                    if (geometry.Points[i] == point)
                    {
                        UpdateMidPoints(point, newPoint, i, midCollection);

                        return true;
                    }
                }
            }
            else
            {
                for (int g = 0; g < geometry.Geometries.Length; g++)
                {
                    var matched = UpdateLineSegment(point, newPoint, geometry.Geometries[g], midCollection.Collections[g]);

                    if (matched)
                        return true;
                }
            }

            return false;
        }

        private void UpdateMidPoints(Point point, Point newPoint, int pointIndex, RecursiveCollection<Locateable> midCollection)
        {
            try
            {
                var displacement = new Point((newPoint.X - point.X) / 2.0, (newPoint.Y - point.Y) / 2.0);

                var length = midCollection.Values.Count;

                var leftIndex = GetLeftMidPointIndex(pointIndex, length);

                var rightIndex = GetRightMidPointIndex(pointIndex, length);

                Locateable left = leftIndex == int.MinValue ? null : midCollection.Values[leftIndex];

                if (left != null)
                {
                    left.X = left.X + displacement.X;
                    left.Y = left.Y + displacement.Y;
                }

                Locateable right = rightIndex == int.MinValue ? null : midCollection.Values[rightIndex];

                if (right != null)
                {
                    right.X = right.X + displacement.X;
                    right.Y = right.Y + displacement.Y;
                }

            }
            catch (Exception ex)
            {

            }
        }


        public void AddVertex(Point mercatorPoint)
        {
            AddVertex(mercatorPoint, this._mercatorGeometry, this._vertices);
        }

        //probably this method can be better
        private void AddVertex(Point mercatorPoint, Geometry geometry, RecursiveCollection<Locateable> primaryCollection)
        {
            if (geometry.Points != null)
            {
                var point = this.ToScreen(mercatorPoint.AsWpfPoint());

                var locateable = ToPrimaryLocateable(mercatorPoint);

                if (geometry.Points.Last().AreExactlyTheSame(mercatorPoint))
                    return;

                geometry.AddLastPoint(mercatorPoint);

                primaryCollection.Values.Add(locateable);

                this._primaryVerticesLayer.Items.Add(locateable);

                //this._pathGeometry.Figures.Last().Segments.RemoveAt(this._pathGeometry.Figures.Last().Segments.Count - 1);

                //this._pathGeometry.Figures.Last().Segments.Add(new LineSegment(new WpfPoint(point.X, point.Y), true));
            }
            else
            {
                AddVertex(mercatorPoint, geometry.Geometries.Last(), primaryCollection.Collections.Last());
            }
        }


        public void AddSemiVertex(Point mercatorPoint)
        {
            var point = this.ToScreen(mercatorPoint.AsWpfPoint());

            this._pathGeometry.Figures.Last().Segments.Add(new LineSegment(new WpfPoint(point.X, point.Y), true));
        }

        public void UpdateLastSemiVertexLocation(Point newMercatorPoint)
        {
            if (_pathGeometry.Figures?.Last()?.Segments?.Count() < 1)
                AddSemiVertex(newMercatorPoint);

            if (_pathGeometry.Figures.Last().Segments.Count < this._mercatorGeometry.GetLastPart().Length)
            {
                AddSemiVertex(newMercatorPoint);
            }

            var newPoint = this.ToScreen(newMercatorPoint.AsWpfPoint());

            var lastSegment = ((LineSegment)_pathGeometry.Figures.Last().Segments.Last()).Point = new WpfPoint(newPoint.X, newPoint.Y);
        }

        public void FinishEditing()
        {
            this.RequestFinishEditing?.Invoke(this._mercatorGeometry);
        }
    }
}
