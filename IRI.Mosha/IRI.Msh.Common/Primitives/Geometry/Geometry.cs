using IRI.Msh.Common.Analysis;
using IRI.Msh.CoordinateSystem.MapProjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IRI.Msh.Common.Primitives
{
    public class Geometry<T> where T : IPoint, new()
    {
        public GeometryType Type { get; set; }

        private List<T> _points;

        public List<T> Points
        {
            get { return _points; }
            set
            {
                if (this.Geometries != null && value != null)
                {
                    throw new NotImplementedException();
                }

                this._points = value;
            }
        }


        private List<Geometry<T>> _geometries;

        public List<Geometry<T>> Geometries
        {
            get { return _geometries; }
            set
            {
                if (this.Points != null && value != null)
                {
                    throw new NotImplementedException();
                }

                this._geometries = value;
            }
        }

        public int NumberOfPoints { get { return Points?.Count ?? 0; } }

        public int NumberOfGeometries { get { return Geometries?.Count ?? 0; } }

        public int TotalNumberOfPoints
        {
            get
            {
                if (this.IsLeafGeometry())
                {
                    return this.NumberOfPoints;
                }
                else
                {
                    return Geometries?.Sum(g => g.TotalNumberOfPoints) ?? 0;
                }

                //if (Points != null)
                //{
                //    return Points.Count;
                //}
                //else
                //{
                //    return Geometries.Sum(g => g.TotalNumberOfPoints);
                //}
            }
        }

        public int Srid { get; set; }

        #region Constructors

        private Geometry()
        {

        }

        //public Geometry(List<Point> points, GeometryType type, int srid) : this(points as List<T>, type, false, srid) { }

        public Geometry(List<T> points, GeometryType type, int srid) : this(points, type, false, srid) { }

        public Geometry(List<T> points, GeometryType type, bool isClosed, int srid)
        {
            if (type == GeometryType.LineString || type == GeometryType.Point)
            {
                if (type == GeometryType.LineString && isClosed && points?.Count > 0)
                {
                    var lastPoint = points[points.Count - 1];

                    var firstPoint = points[0];

                    if (lastPoint.X != firstPoint.X && lastPoint.Y != firstPoint.Y)
                    {
                        throw new ArgumentException("the last point and first point must be the same");
                    }
                }

                this.Points = points;

                this.Type = type;

                this.Srid = srid;
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public Geometry(List<Geometry<T>> geometries, GeometryType type, int srid)
        {
            if (type != GeometryType.MultiLineString && type != GeometryType.MultiPoint && type != GeometryType.MultiPolygon && type != GeometryType.Polygon && type != GeometryType.GeometryCollection)
            {
                throw new NotImplementedException();
            }

            this.Geometries = geometries;

            this.Type = type;

            this.Srid = srid;

            if (geometries.Count > 0)
            {
                var tempSubType = geometries.First().Type;

                if (geometries.Any(i => i.Type != tempSubType))
                {
                    this.Type = GeometryType.GeometryCollection;
                }
            }
        }

        public Geometry(Geometry<T> geometry, GeometryType type, int srid) : this(new List<Geometry<T>>() { geometry }, type, srid) { }

        #endregion

        public override string ToString()
        {
            return $"{Type} - #Points: {NumberOfPoints} - #Parts: {Geometries?.Count()}";
        }

        public bool IsRingBase()
        {
            return this.Type == GeometryType.Polygon || this.Type == GeometryType.MultiPolygon || this.Type == GeometryType.CurvePolygon;
        }

        public bool IsNullOrEmpty()
        {
            return (this.Points == null || this.Points?.Count == 0) &&
                    (this.Geometries == null || this.Geometries?.Count == 0);
        }

        public bool HasAnyPoint()
        {
            //1399.07.17
            //if (Points != null)
            if (this.IsLeafGeometry())
            {
                return Points?.Count > 0;
            }
            else /*if (Geometries != null)*/
            {
                return Geometries?.Any(g => g?.HasAnyPoint() == true) == true;
            }
            //else
            //{
            //    return false;
            //}
        }

        public bool IsValid()
        {
            switch (this.Type)
            {
                case GeometryType.Point:
                    return this.Points?.Count == 1;

                case GeometryType.LineString:
                    return this.Points.Count > 1;

                case GeometryType.Polygon:
                    return this.Geometries?.Count > 0 && this.Geometries?.All(g => g.Points?.Count >= 3) == true;

                case GeometryType.MultiPoint:
                case GeometryType.MultiLineString:
                case GeometryType.MultiPolygon:
                case GeometryType.GeometryCollection:
                    return this.Geometries?.All(g => g.IsValid()) == true;

                case GeometryType.CircularString:
                case GeometryType.CompoundCurve:
                case GeometryType.CurvePolygon:
                default:
                    throw new NotImplementedException();
            }
        }

        public bool IsLeafGeometry()
        {
            switch (Type)
            {
                case GeometryType.Point:
                case GeometryType.LineString:
                    return true;

                case GeometryType.Polygon:
                case GeometryType.MultiPoint:
                case GeometryType.MultiLineString:
                case GeometryType.MultiPolygon:
                case GeometryType.GeometryCollection:
                    return false;

                case GeometryType.CircularString:
                case GeometryType.CompoundCurve:
                case GeometryType.CurvePolygon:
                default:
                    return false;
            }
        }

        public BoundingBox GetBoundingBox()
        {
            return BoundingBox.CalculateBoundingBox(GetAllPoints());
        }



        #region Find

        public bool TryFindPoint(T point, out int pointIndex, out int geometryIndex, out int subGeometryIndex)
        {
            pointIndex = -1;

            geometryIndex = -1;

            subGeometryIndex = -1;

            if (this.Type == GeometryType.GeometryCollection)
            {
                throw new NotImplementedException();
            }

            //LineString, Point cases
            if (this.Points != null)
            {
                return TryFind(this.Points, point, out pointIndex);
            }
            else if (this.Geometries != null)
            {
                for (int g = 0; g < this.Geometries.Count; g++)
                {
                    //MultiPoint, MultiLineString, Polygon cases
                    if (this.Geometries[g].Points != null)
                    {
                        if (TryFind(this.Geometries[g].Points, point, out pointIndex))
                        {
                            geometryIndex = g;

                            return true;
                        }
                    }
                    //MultiPolygon case
                    else if (this.Geometries[g].Geometries != null)
                    {
                        for (int subG = 0; subG < this.Geometries[g].Geometries.Count; subG++)
                        {
                            if (TryFind(this.Geometries[g].Geometries[subG].Points, point, out pointIndex))
                            {
                                geometryIndex = g;

                                subGeometryIndex = subG;

                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }

        private bool TryFind(List<T> points, T point, out int pointIndex)
        {
            pointIndex = -1;

            if (points == null)
                return false;

            for (int i = 0; i < points.Count; i++)
            {
                if (points[i].Equals(point))
                {
                    pointIndex = i;

                    return true;
                }
            }

            return false; ;
        }

        private bool TryFind(List<T> points, double x, double y, out int pointIndex)
        {
            pointIndex = -1;

            if (points == null)
                return false;

            for (int i = 0; i < points.Count; i++)
            {
                if (points[i].X == x && points[i].Y == y)
                {
                    pointIndex = i;

                    return true;
                }
            }

            return false; ;
        }

        #endregion


        #region Analysis

        public Point GetMeanPoint()
        {
            var allPoints = GetAllPoints();

            return new Point(allPoints.Sum(i => i.X) / allPoints.Count, allPoints.Sum(i => i.Y) / allPoints.Count);
        }

        //must be reviewed: 
        public T GetNearestPoint(IPoint point)
        {
            if (this.IsLeafGeometry())
            {
                var minDistance = double.MaxValue;

                T result = new T();

                for (int i = 0; i < this.Points.Count; i++)
                {
                    var distance = this.Points[i].DistanceTo(point);

                    if (minDistance > distance)
                    {
                        result = Points[i];

                        minDistance = distance;
                    }
                }

                return result;
            }
            else
            {
                var nearestPoints = this.Geometries.Select(g => g.GetNearestPoint(point)).ToList();

                //must be reviewed
                return nearestPoints.First();
            }
        }

        /// <summary>
        /// Does not filter point and multipoint features
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public Geometry<T> FilterPoints(Func<List<T>, List<T>> filter)
        {
            switch (this.Type)
            {
                case GeometryType.CircularString:
                case GeometryType.CompoundCurve:
                case GeometryType.CurvePolygon:
                    return Geometry<T>.Null;

                case GeometryType.GeometryCollection:
                    return new Geometry<T>(this.Geometries.Select(i => i.FilterPoints(filter)).ToList(), GeometryType.GeometryCollection, this.Srid);

                case GeometryType.MultiPoint:
                    return new Geometry<T>(this.Geometries, GeometryType.MultiPoint, this.Srid);

                case GeometryType.Point:
                    return new Geometry<T>(this.Points, GeometryType.Point, this.Srid);

                case GeometryType.LineString:
                    //todo: multiple cast consider changing it
                    //return CreatePointOrLineString<T>(filter(this.Points.Select(i => (T)i).ToList()).Select(i => (IPoint)i).ToList(), this.Srid);
                    return CreatePointOrLineString<T>(filter(this.Points), this.Srid);

                case GeometryType.MultiLineString:
                    return new Geometry<T>(this.Geometries.Select(i => i.FilterPoints(filter)).ToList(), GeometryType.MultiLineString, this.Srid);

                case GeometryType.Polygon:
                    return new Geometry<T>(this.Geometries.Select(i => i.FilterPoints(filter)).ToList(), GeometryType.Polygon, this.Srid);

                case GeometryType.MultiPolygon:
                    return new Geometry<T>(this.Geometries.Select(i => i.FilterPoints(filter)).ToList(), GeometryType.MultiPolygon, this.Srid);

                default:
                    throw new NotImplementedException();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="threshold"></param>
        /// <param name="type"></param>
        /// <param name="secondaryParameter">may be area threshold for `AdditiveByAreaAngle` or look ahead parameter for `Lang`</param>
        /// <returns></returns>
        public Geometry<T> Simplify(
            /*double threshold,*/
            SimplificationType type,
            SimplificationParamters paramters
            /*, bool retain3Poins, double secondaryParameter = double.NaN*/)
        {
            Func<List<T>, List<T>> filter;

            switch (type)
            {
                case SimplificationType.ByArea:
                    filter = pList => IRI.Msh.Common.Analysis.VisualSimplification.SimplifyByArea(pList, paramters/* threshold, retain3Poins*/);
                    break;

                case SimplificationType.AdditiveByArea:
                    filter = pList => IRI.Msh.Common.Analysis.VisualSimplification.AdditiveSimplifyByArea(pList, paramters/*threshold, retain3Poins*/);
                    break;

                case SimplificationType.AdditiveByAreaPlus:
                    filter = pList => IRI.Msh.Common.Analysis.VisualSimplification.AdditiveSimplifyByAreaPlus(pList, paramters/* threshold, retain3Poins*/);
                    break;

                case SimplificationType.ByAngle:
                    filter = pList => IRI.Msh.Common.Analysis.VisualSimplification.SimplifyByAngle(pList, paramters/* threshold, retain3Poins*/);
                    break;

                case SimplificationType.AdditiveByAngle:
                    filter = pList => IRI.Msh.Common.Analysis.VisualSimplification.AdditiveSimplifyByAngle(pList, paramters/*threshold, retain3Poins*/);
                    break;

                case SimplificationType.AdditiveByDistance:
                    filter = pList => IRI.Msh.Common.Analysis.VisualSimplification.AdditiveSimplifyByDistance(pList, paramters/*threshold, retain3Poins*/);
                    break;

                case SimplificationType.AdditiveByAreaAngle:
                    filter = pList => IRI.Msh.Common.Analysis.VisualSimplification.AdditiveSimplifyByAngleArea(pList, paramters/*threshold, secondaryParameter, retain3Poins*/);
                    break;

                case SimplificationType.Visvalingam:
                    filter = pList => IRI.Msh.Common.Analysis.VisualSimplification.SimplifyByVisvalingam(pList, paramters, this.IsRingBase()/*threshold, this.IsRingBase(), retain3Poins*/);
                    break;

                case SimplificationType.DouglasPeucker:
                    filter = pList => IRI.Msh.Common.Analysis.VisualSimplification.SimplifyByDouglasPeucker(pList, paramters /*threshold, retain3Poins*/);
                    break;

                case SimplificationType.Lang:
                    filter = pList => IRI.Msh.Common.Analysis.VisualSimplification.SimplifyByLang(
                                        pList,
                                        paramters
                                        //threshold,
                                        //double.IsNaN(secondaryParameter) ? (int?)null : (int)secondaryParameter,
                                        //retain3Poins
                                        );
                    break;

                default:
                    throw new NotImplementedException();
            }

            return this.FilterPoints(filter);
        }

        public Geometry<T> Simplify(SimplificationType type, int zoomLevel, SimplificationParamters paramters)
        {
            var threshold = IRI.Msh.Common.Mapping.WebMercatorUtility.CalculateGroundResolution(zoomLevel, paramters.AverageLatitude ?? 0); //0 seconds!

            paramters.AreaThreshold = threshold * threshold;

            paramters.DistanceThreshold = threshold;

            return Simplify(type, paramters);
        }

        public Geometry<T> Transform(Func<T, T> transform, int newSrid = 0)
        {
            switch (this.Type)
            {
                case GeometryType.CircularString:
                case GeometryType.CompoundCurve:
                case GeometryType.CurvePolygon:
                case GeometryType.GeometryCollection:
                    System.Diagnostics.Debug.WriteLine($"****WARNNING: Geometry.cs -> Filter method invalid geometry type");
                    return Geometry<T>.Null;

                case GeometryType.MultiPoint:
                    return new Geometry<T>(this.Geometries.Select(i => i.Transform(transform)).ToList(), GeometryType.MultiPoint, newSrid);

                case GeometryType.Point:
                    return new Geometry<T>(this.Points.Select(i => transform(i)).ToList(), GeometryType.Point, newSrid);

                case GeometryType.LineString:
                    return new Geometry<T>(this.Points.Select(i => transform(i)).ToList(), GeometryType.LineString, newSrid);

                case GeometryType.MultiLineString:
                    return new Geometry<T>(this.Geometries.Select(i => i.Transform(transform)).ToList(), GeometryType.MultiLineString, newSrid);

                case GeometryType.MultiPolygon:
                    return new Geometry<T>(this.Geometries.Select(i => i.Transform(transform)).ToList(), GeometryType.MultiPolygon, newSrid);

                case GeometryType.Polygon:
                    return new Geometry<T>(this.Geometries.Select(i => i.Transform(transform)).ToList(), GeometryType.Polygon, newSrid);

                default:
                    throw new NotImplementedException();
            }
        }

        #endregion


        #region Geometry Manipulation

        public List<T> GetAllPoints()
        {
            if (Points != null)
            {
                return Points;
            }
            else if (Geometries != null)
            {
                return Geometries.SelectMany(i => i.GetAllPoints()).ToList();
            }
            else
            {
                return null;
            }
        }

        public IPoint GetLastPoint()
        {
            if (Points?.Any() == true)
            {
                return Points.Last();
            }
            else if (Geometries?.Count > 0)
            {
                return Geometries.Last().GetLastPoint();
            }
            else
            {
                return null;
            }
        }

        public void StartNewGeometry(T startPoint, GeometryType type)
        {
            if (this.Geometries == null)
                throw new NotImplementedException();

            var geometries = this.Geometries.ToList();

            geometries.Add(new Geometry<T>(new List<T>() { startPoint }, type, this.Srid));

            this.Geometries = geometries.ToList();
        }

        public Geometry<T> Clone()
        {
            if (this.Points != null)
            {
                List<T> points = new List<T>(this.Points.Count);

                for (int i = 0; i < this.Points.Count; i++)
                {
                    //points[i] = new T() { X = this.Points[i].X, Y = this.Points[i].Y };
                    points.Add(new T() { X = this.Points[i].X, Y = this.Points[i].Y });
                }

                return Geometry<T>.Create(points, this.Type, this.Srid);
            }
            if (this.Geometries != null)
            {
                return new Geometry<T>(this.Geometries.Select(g => g.Clone()).ToList(), this.Type, this.Srid);
            }

            return new Geometry<T>(null, this.Type, false, this.Srid);
        }

        //This method can better using Array.Resize()
        public void InsertLastPoint(T newPoint)
        {
            if (this.Points != null)
            {
                var points = this.Points.ToList();

                points.Add(newPoint);

                this.Points = points.ToList();
            }
            else
            {
                this.Geometries.Last().InsertLastPoint(newPoint);
            }
        }

        public void InsertPoint(T newPoint, int index)
        {
            var points = this.Points.ToList();

            points.Insert(index, newPoint);

            this.Points = points.ToList();
        }

        public void Remove(T point)
        {
            var list = this.Points.ToList();

            list.Remove(point);

            this.Points = list.ToList();
        }

        public void Remove(double x, double y)
        {
            var list = this.Points.ToList();

            for (int i = list.Count - 1; i >= 0; i--)
            {
                if (list[i].X == x && list[i].Y == y)
                {
                    list.Remove(list[i]);

                    break;
                }
            }

            this.Points = list.ToList();
        }

        public List<T> GetLastPart()
        {
            if (this.IsNullOrEmpty())
                return null;

            if (this.IsLeafGeometry())
            {
                return this.Points;
            }
            else
            {
                return this.Geometries.Last().GetLastPart();
            }

            //1399.07.17
            //if (this.Points == null && this.Geometries == null)
            //    return null;

            //if (this.Points == null)
            //{
            //    return this.Geometries.Last().GetLastPart();
            //}
            //else
            //{
            //    return this.Points;
            //}
        }

        //Returns last geometry ring or point
        public Geometry<T> GetLastGeometryPart()
        {
            if (this.Points != null)
            {
                return this;
            }
            else if (this.Geometries != null)
            {
                if (this.Geometries.Count > 0)
                {
                    return this.Geometries.Last().GetLastGeometryPart();
                }
            }

            return null;
        }

        public void ClearEmptyGeometries()
        {
            if (Geometries == null)
            {
                return;
            }

            for (int i = Geometries.Count; i >= 0; i--)
            {
                if (Geometries[i].HasAnyPoint())
                {
                    Geometries[i].ClearEmptyGeometries();
                }
                else
                {
                    this.Geometries.RemoveAt(i);
                }
            }
        }

        public Geometry<T> GetRingOrLineStringPassingPoint(double x, double y)
        {
            int index;

            switch (this.Type)
            {
                case GeometryType.LineString:
                    if (TryFind(this.Points, x, y, out index))
                    {
                        return this;
                    }

                    return null;

                case GeometryType.Polygon:
                case GeometryType.MultiLineString:
                case GeometryType.MultiPolygon:
                    for (int i = 0; i < this.Geometries.Count; i++)
                    {
                        var geo = this.Geometries[i].GetRingOrLineStringPassingPoint(x, y);

                        if (geo != null)
                        {
                            return geo;
                        }
                    }

                    return null;

                case GeometryType.Point:
                case GeometryType.MultiPoint:
                case GeometryType.GeometryCollection:
                case GeometryType.CircularString:
                case GeometryType.CompoundCurve:
                case GeometryType.CurvePolygon:
                default:
                    throw new NotImplementedException();
            }
        }

        public bool TryAddNewPart()
        {
            var currentGeometry = this.Clone();

            //e.g. having a LineString with just one point we cannot convert to MultilineString
            if (!currentGeometry.IsValid())
            {
                return false;
            }

            switch (this.Type)
            {
                case GeometryType.Point:
                    this.Geometries = new List<Geometry<T>>();
                    this.Geometries.Add(currentGeometry);
                    this.Geometries.Add(new Geometry<T>(new List<T>(), GeometryType.Point, this.Srid));
                    this.Points = null;
                    this.Type = GeometryType.MultiPoint;
                    break;
                //this.Geometries = new Geometry[2];
                //this.Geometries[0] = currentGeometry;
                //this.Geometries[1] = new Geometry(new IPoint[0], GeometryType.Point, this.Srid);
                //this.Points = null;
                //this.Type = GeometryType.MultiPoint;
                //break;

                case GeometryType.LineString:
                    this.Points = null;
                    this.Geometries = new List<Geometry<T>>();
                    this.Geometries.Add(currentGeometry);
                    this.Geometries.Add(new Geometry<T>(new List<T>(), GeometryType.LineString, this.Srid));
                    this.Type = GeometryType.MultiLineString;
                    break;

                //this.Points = null;
                //this.Geometries = new Geometry[2];
                //this.Geometries[0] = currentGeometry;
                //this.Geometries[1] = new Geometry(new IPoint[0], GeometryType.LineString, this.Srid);

                //this.Type = GeometryType.MultiLineString;
                //break;

                case GeometryType.Polygon:
                    this.Geometries = new List<Geometry<T>>();
                    this.Geometries.Add(currentGeometry);
                    this.Geometries.Add(new Geometry<T>(new List<Geometry<T>>() { new Geometry<T>(new List<T>(), GeometryType.LineString, this.Srid) }, GeometryType.Polygon, this.Srid));
                    this.Type = GeometryType.MultiPolygon;
                    break;

                //this.Geometries = new Geometry[2];
                //this.Geometries[0] = currentGeometry;
                //this.Geometries[1] = new Geometry(new Geometry[] { new Geometry(new IPoint[0], GeometryType.LineString, this.Srid) }, GeometryType.Polygon, this.Srid);
                //this.Type = GeometryType.MultiPolygon;
                //break;

                case GeometryType.MultiPoint:
                    this.Geometries.Add(new Geometry<T>(new List<T>(), GeometryType.Point, this.Srid));
                    break;
                //var points = this.Geometries.ToList();
                //points.Add(new Geometry(new IPoint[0], GeometryType.Point, this.Srid));
                //this.Geometries = points.ToList();
                //break;

                case GeometryType.MultiLineString:
                    this.Geometries.Add(new Geometry<T>(new List<T>(), GeometryType.LineString, this.Srid));
                    break;

                //var lines = this.Geometries.ToList();
                //lines.Add(new Geometry(new IPoint[0], GeometryType.LineString, this.Srid));
                //this.Geometries = lines.ToList();
                //break;

                case GeometryType.MultiPolygon:
                    this.Geometries.Add(new Geometry<T>(new List<Geometry<T>> { new Geometry<T>(new List<T>(), GeometryType.LineString, this.Srid) }, GeometryType.Polygon, this.Srid));
                    break;

                //var polygons = this.Geometries.ToList();
                //polygons.Add(new Geometry(new Geometry[] { new Geometry(new IPoint[0], GeometryType.LineString, this.Srid) }, GeometryType.Polygon, this.Srid));
                //this.Geometries = polygons.ToList();
                //break;


                case GeometryType.GeometryCollection:
                case GeometryType.CircularString:
                case GeometryType.CompoundCurve:
                case GeometryType.CurvePolygon:
                default:
                    throw new NotImplementedException();
            }

            return true;
        }

        public bool TryRemovePart(Geometry<T> geometry)
        {
            if (this.Geometries?.Count > 0)
            {
                for (int i = 0; i < this.Geometries.Count; i++)
                {
                    if (this.Geometries[i] == geometry)
                    {
                        var temp = this.Geometries.ToList();

                        temp.Remove(geometry);

                        this.Geometries = temp.ToList();

                        return true;
                    }
                }
            }

            return false;
        }

        public bool TryRemoveEntireRingOrLineString(double x, double y)
        {
            var part = this.GetRingOrLineStringPassingPoint(x, y);

            if (this.Geometries?.Count > 0)
            {
                for (int i = 0; i < this.Geometries.Count; i++)
                {
                    switch (Geometries[i].Type)
                    {
                        case GeometryType.LineString:
                            if (this.TryRemovePart(part))
                                return true;
                            break;

                        case GeometryType.Polygon:
                            for (int g = Geometries[i].Geometries.Count - 1; g >= 0; g--)
                            {
                                if (Geometries[i].TryRemovePart(part))
                                {
                                    return true;
                                }
                            }
                            break;

                        case GeometryType.MultiLineString:
                        case GeometryType.MultiPoint:
                        case GeometryType.MultiPolygon:
                        case GeometryType.Point:
                        case GeometryType.GeometryCollection:
                        case GeometryType.CircularString:
                        case GeometryType.CompoundCurve:
                        case GeometryType.CurvePolygon:
                        default:
                            throw new NotImplementedException();
                    }
                }

                return false;
            }

            return false;
        }

        public List<LineSegment<T>> GetLineSegments()
        {
            switch (this.Type)
            {
                case GeometryType.LineString:
                    return GetLineSegments(false);

                case GeometryType.Polygon:
                    return Geometries.SelectMany(i => i.GetLineSegments(true)).ToList();

                case GeometryType.MultiLineString:
                    return Geometries.SelectMany(i => i.GetLineSegments(false)).ToList();

                case GeometryType.MultiPolygon:
                    return Geometries.SelectMany(i => i.GetLineSegments()).ToList();

                case GeometryType.GeometryCollection:
                    return Geometries.SelectMany(i => i.GetLineSegments()).ToList();

                case GeometryType.Point:
                case GeometryType.MultiPoint:
                case GeometryType.CircularString:
                case GeometryType.CompoundCurve:
                case GeometryType.CurvePolygon:
                default:
                    throw new NotImplementedException();
            }
        }

        private List<LineSegment<T>> GetLineSegments(bool isClosed)
        {
            List<LineSegment<T>> result = new List<LineSegment<T>>();

            if (Points != null)
            {
                for (int i = 0; i < this.Points.Count - 1; i++)
                {
                    result.Add(new LineSegment<T>(Points[i], Points[i + 1]));
                }

                if (isClosed)
                {
                    //prevent returning segment when polygon has one point
                    if (Points.Count > 1)
                    {
                        result.Add(new LineSegment<T>(Points[Points.Count - 1], Points[0]));
                    }
                }
            }

            return result;
        }


        private void Reverse()
        {
            switch (this.Type)
            {
                case GeometryType.Point:
                case GeometryType.MultiPoint:
                    break;

                case GeometryType.LineString:
                    this.Points.Reverse();
                    break;

                case GeometryType.Polygon:
                case GeometryType.MultiLineString:
                case GeometryType.MultiPolygon:
                case GeometryType.GeometryCollection:
                    for (int i = 0; i < this.Geometries?.Count; i++)
                    {
                        this.Geometries[i].Reverse();
                    }
                    break;

                case GeometryType.CircularString:
                case GeometryType.CompoundCurve:
                case GeometryType.CurvePolygon:
                default:
                    throw new NotImplementedException("Geometry.cs > Reverse");
            }
        }

        #endregion


        #region Project & Srs

        public SrsBase GetSrs()
        {
            return SridHelper.AsSrsBase(Srid);
        }

        public Geometry<T> Project(SrsBase targetSrs)
        {
            var sourceSrs = GetSrs();

            return Project(sourceSrs, targetSrs);
        }

        public Geometry<T> Project(SrsBase sourceSrs, SrsBase targetSrs)
        {
            if (sourceSrs.Srid == targetSrs.Srid && sourceSrs.Srid != 0)
            {
                return this;
            }
            else if (sourceSrs.Ellipsoid.AreTheSame(targetSrs.Ellipsoid))
            {
                var c1 = this.Transform(p => sourceSrs.ToGeodetic(p), SridHelper.GeodeticWGS84);

                return c1.Transform(p => targetSrs.FromGeodetic(p), targetSrs.Srid);
            }
            else
            {
                var c1 = this.Transform(p => sourceSrs.ToGeodetic(p), SridHelper.GeodeticWGS84);

                return c1.Transform(p => targetSrs.FromGeodetic(p, sourceSrs.Ellipsoid), targetSrs.Srid);
            }
        }

        #endregion



        #region Static Create

        private static readonly Geometry<T> _null = new Geometry<T>();

        public static Geometry<T> Null { get { return _null; } }

        //public static readonly Geometry EmptyPoint = new Geometry(new IPoint[0], GeometryType.Point);
        //public static readonly Geometry EmptyLineString = new Geometry(new IPoint[0], GeometryType.LineString);
        //public static readonly Geometry EmptyPolygon = new Geometry(new IPoint[0], GeometryType.Polygon);
        //public static readonly Geometry EmptyMultiPoint = new Geometry(new Geometry[0], GeometryType.MultiPoint);
        //public static readonly Geometry EmptyMultiLineString = new Geometry(new Geometry[0], GeometryType.MultiLineString);
        //public static readonly Geometry EmptyMultiPolygon = new Geometry(new Geometry[0], GeometryType.MultiPolygon);

        public static Geometry<T> CreateEmpty(GeometryType type, int srid)
        {
            switch (type)
            {
                case GeometryType.GeometryCollection:
                    return new Geometry<T>(new List<Geometry<T>>(), GeometryType.GeometryCollection, srid);

                case GeometryType.LineString:
                    return new Geometry<T>(new List<T>(), GeometryType.LineString, srid);

                case GeometryType.MultiLineString:
                    return new Geometry<T>(new List<Geometry<T>>(), GeometryType.MultiLineString, srid);

                case GeometryType.MultiPoint:
                    return new Geometry<T>(new List<Geometry<T>>(), GeometryType.MultiPoint, srid);

                case GeometryType.MultiPolygon:
                    return new Geometry<T>(new List<Geometry<T>>(), GeometryType.MultiPolygon, srid);

                case GeometryType.Point:
                    return new Geometry<T>(new List<T>(), GeometryType.Point, srid);

                case GeometryType.Polygon:
                    return new Geometry<T>(new List<Geometry<T>>(), GeometryType.Polygon, srid);

                case GeometryType.CircularString:
                case GeometryType.CompoundCurve:
                case GeometryType.CurvePolygon:
                default:
                    throw new NotImplementedException();
            }
        }

        public static Geometry<T> Create(double x, double y, int srid = 0)
        {
            return new Geometry<T>(new List<T> { new T() { X = x, Y = y } }, GeometryType.Point, srid);
        }

        /// <summary>
        /// For Polygons do not repeat first point in the last point
        /// </summary>
        /// <param name="points"></param>
        /// <param name="type"></param>
        /// <param name="srid"></param>
        /// <returns></returns>
        public static Geometry<T> Create(List<T> points, GeometryType type, int srid = 0)
        {
            if (points == null)
            {
                CreateEmpty(type, srid);
            }

            switch (type)
            {
                case GeometryType.Point:
                    return new Geometry<T>(points, GeometryType.Point, srid);
                //return CreatePointOrLineString(points, srid); //do not use this method: what if making a new Geometry LineString with first point

                case GeometryType.LineString:
                    return new Geometry<T>(points, GeometryType.LineString, srid);
                //return CreatePointOrLineString(points, srid);

                case GeometryType.Polygon:
                    return new Geometry<T>(new Geometry<T>(points, GeometryType.LineString, srid), GeometryType.Polygon, srid);

                case GeometryType.MultiPoint:
                case GeometryType.MultiLineString:
                case GeometryType.MultiPolygon:
                case GeometryType.GeometryCollection:
                case GeometryType.CircularString:
                case GeometryType.CompoundCurve:
                case GeometryType.CurvePolygon:
                default:
                    throw new NotImplementedException();
            }
        }

        public static Geometry<T> CreatePointOrLineString<T>(List<T> points, int srid) where T : IPoint, new()
        {
            if (points.Count == 1)
            {
                return new Geometry<T>(points, GeometryType.Point, srid);
            }
            else
            {
                return new Geometry<T>(points, GeometryType.LineString, srid);
            }
        }

        public static Geometry<T> CreatePolygonOrMultiPolygon<T>(List<Geometry<T>> rings, int srid) where T : IPoint, new()
        {
            if (rings == null || rings.Count < 1)
            {
                return Geometry<T>.CreateEmpty(GeometryType.Polygon, srid);
            }

            if (rings.Count == 1)
            {
                return new Geometry<T>(rings, GeometryType.Polygon, srid);
            }

            var orderedRings = rings.Select(p => (area: p.CalculateUnsignedEuclideanArea(), geo: p)).OrderByDescending(i => i.area).ToList();

            var masterPolygons = new List<Geometry<T>>();

            for (int i = 0; i < orderedRings.Count; i++)
            {
                var currentRing = orderedRings[i].geo;

                bool isMasterRing = true;

                for (int p = 0; p < masterPolygons.Count; p++)
                {
                    if (SpatialUtility.IsPointInPolygon(masterPolygons[p].Geometries[0], currentRing.Points.First()))
                    {
                        //not in any of polygon holes
                        if (masterPolygons[p].Geometries.Skip(1).Any(g => SpatialUtility.IsPointInPolygon(g, currentRing.Points.First())))
                            continue;

                        isMasterRing = false;

                        //must be ccw
                        if (SpatialUtility.IsClockwise(currentRing.Points))
                        {
                            currentRing.Reverse();
                        }

                        masterPolygons[p].Geometries.Add(currentRing);
                    }
                }
                if (isMasterRing)
                {
                    //must be cw
                    if (!SpatialUtility.IsClockwise(currentRing.Points))
                    {
                        currentRing.Reverse();
                    }

                    masterPolygons.Add(new Geometry<T>(currentRing, GeometryType.Polygon, srid));
                }
            }

            return masterPolygons.Count == 1 ?
                masterPolygons.First() :
                new Geometry<T>(masterPolygons, GeometryType.MultiPolygon, srid);
        }

        #endregion


        #region Static Methods


        // To do: provide sample input and expected output for this method  
        public static Geometry<T> ParseToGeodeticGeometry(double[][] geoCoordinates, GeometryType geometryType, bool isLongitudeFirst = true)
        {
            return new Geometry<T>(geoCoordinates.Select(p => ParseLineStringToGeodeticGeometry(p.ToList(), isLongitudeFirst)).ToList(), geometryType, SridHelper.GeodeticWGS84);
        }

        // To do: provide sample input and expected output for this method
        private static Geometry<T> ParseLineStringToGeodeticGeometry(List<double> values, bool isLongitudeFirst)
        {
            if (values == null || values.Count() % 2 != 0)
            {
                throw new NotImplementedException();
            }

            List<T> result = new List<T>(values.Count / 2);

            if (isLongitudeFirst)
            {
                for (int i = 0; i < values.Count - 1; i += 2)
                {
                    result.Add(new T() { X = values[i], Y = values[i + 1] });
                }
            }
            else
            {
                for (int i = 0; i < values.Count - 1; i += 2)
                {
                    result.Add(new T() { X = values[i + 1], Y = values[i] });
                }
            }

            return new Geometry<T>(result, GeometryType.LineString, SridHelper.GeodeticWGS84);

        }


        //public static Point ParsePointToGeometry(double[] values, bool isLongitudeFirst)
        //{
        //    if (values == null || values.Count() != 2)
        //    {
        //        throw new NotImplementedException();
        //    }

        //    if (isLongitudeFirst)
        //    {
        //        return new Point(values[0], values[1]);
        //    }
        //    else
        //    {
        //        return new Point(values[1], values[0]);
        //    }
        //}

        public static Geometry<T> ParsePointToGeometry(double[] xy, bool isLongitudeFirst, int srid = SridHelper.GeodeticWGS84)
        {
            return new Geometry<T>(new List<T>() { Point.Parse<T>(xy, isLongitudeFirst) }, GeometryType.Point, srid);
        }

        public static Geometry<T> ParseLineStringToGeometry(
            double[][] geoCoordinates,
            GeometryType geometryType,
            bool isRing,
            bool isLongitudeFirst = true,
            int srid = SridHelper.GeodeticWGS84)
        {
            if (isRing)
            {
                var numberOfPoints = geoCoordinates.Length;

                // skip last point
                return new Geometry<T>(geoCoordinates.Take(numberOfPoints - 1).Select(p => Point.Parse<T>(p, isLongitudeFirst)).ToList(), geometryType, srid);
            }
            else
            {
                return new Geometry<T>(geoCoordinates.Select(p => Point.Parse<T>(p, isLongitudeFirst)).ToList(), geometryType, srid);
            }
        }

        public static Geometry<T> ParsePolygonToGeometry(double[][][] rings, GeometryType geometryType, bool isLongFirst, int srid = SridHelper.GeodeticWGS84)
        {
            return new Geometry<T>(rings.Select(p => ParseLineStringToGeometry(p, GeometryType.LineString, true, isLongFirst, srid)).ToList(), geometryType, srid);
        }


        #endregion


        #region Area


        public double CalculateUnsignedEuclideanArea()
        {
            if (this.IsNullOrEmpty())
                return 0;

            //1399.07.17
            //if (this.Points == null && this.Geometries == null)
            //    return 0;

            switch (this.Type)
            {
                case GeometryType.Point:
                case GeometryType.MultiPoint:
                case GeometryType.LineString:
                case GeometryType.MultiLineString:
                    return 0;

                case GeometryType.Polygon:
                    return CalculateUnsignedEuclideanAreaForPolygon(this);

                case GeometryType.MultiPolygon:
                    return Geometries.Sum(g => g.CalculateUnsignedEuclideanArea());

                case GeometryType.GeometryCollection:
                case GeometryType.CircularString:
                case GeometryType.CompoundCurve:
                case GeometryType.CurvePolygon:
                default:
                    throw new NotImplementedException();
            }


        }

        //1399.06.10
        //یک چندضلعی از یک رینگ بزرگ که ممکنه حفره داشته باشه
        //تشکیل شده. بنابر این بزرگ‌ترین مساحت متعلق به رینگ بزرگ 
        //و باقی همه حفره‌ها هستن
        //این الگوریتم اگه چندضلعی معتبر نباشه درست جواب نمی‌ده
        private static double CalculateUnsignedEuclideanAreaForPolygon<T>(Geometry<T> geometry) where T : IPoint, new()
        {
            if (geometry == null || geometry.Geometries.Count == 0)
            {
                return 0;
            }

            List<double> areas = new List<double>();

            int maxAreaIndex = 0;

            double maxArea = 0;

            for (int i = 0; i < geometry.Geometries.Count; i++)
            {
                var area = SpatialUtility.GetUnsignedRingArea(geometry.Geometries[i].GetAllPoints());

                if (area > maxArea)
                {
                    maxArea = area;
                    maxAreaIndex = i;
                }

                areas.Add(area);
            }

            areas.RemoveAt(maxAreaIndex);

            ////مساحت رینگ اصلی دو مرتبه حساب شده
            ////تا از خودش داخل ارایه کم شود
            //return 2 * outerRingArea - areas.Sum();

            return maxArea - areas.Sum();
        }

        ////1399.06.11
        ////در این جا فرض شده که نقطه اخر چند حلقه تکرار 
        ////نشده
        //private static double CalculateUnsignedEuclideanAreaForRing(List<T> points)
        //{
        //    if (points == null || points.Count < 3)
        //        return 0;

        //    double area = 0;

        //    for (int i = 0; i < points.Count - 1; i++)
        //    {
        //        double temp = points[i].X * points[i + 1].Y - points[i].Y * points[i + 1].X;

        //        area += temp;
        //    }

        //    //1399.06.11
        //    //تکرار نقطه اخر چند ضلعی
        //    //فرض بر این هست که داخل لیست نقطه‌ها
        //    //این نقطه تکرار نشده باشه
        //    area += points[points.Count - 1].X * points[0].Y - points[points.Count - 1].Y * points[0].X;

        //    return Math.Abs(area / 2.0);
        //}

        #endregion


        #region Length

        public double CalculateEuclideanLength()
        {
            if (this.IsNullOrEmpty())
                return 0;

            //1399.07.17
            //if (this.Points == null && this.Geometries == null)
            //    return 0;

            switch (this.Type)
            {
                case GeometryType.Point:
                case GeometryType.MultiPoint:
                    return 0;

                case GeometryType.LineString:
                    return CalculateEuclideanLengthForLineStringOrRing(false);

                case GeometryType.Polygon:
                    return Geometries.Sum(g => g.CalculateEuclideanLengthForLineStringOrRing(true));

                case GeometryType.MultiLineString:
                case GeometryType.MultiPolygon:
                    return Geometries.Sum(g => g.CalculateEuclideanLength());

                case GeometryType.GeometryCollection:
                case GeometryType.CircularString:
                case GeometryType.CompoundCurve:
                case GeometryType.CurvePolygon:
                default:
                    throw new NotImplementedException("Geometry.cs > CalculateEuclideanLength");
            }
        }

        private double CalculateEuclideanLengthForLineStringOrRing(bool isRing)
        {
            if (this.Points == null || this.Points.Count < 2)
            {
                return 0;
            }

            double result = 0;

            for (int i = 0; i < this.Points.Count - 1; i++)
            {
                result += this.Points[i].DistanceTo(this.Points[i + 1]);
            }

            if (isRing)
            {
                result += this.Points[this.Points.Count - 1].DistanceTo(this.Points[0]);
            }

            return result;
        }

        #endregion


        #region Cleansing

        public bool HasDuplicatePoints()
        {
            if (this.IsNullOrEmpty())
            {
                return false;
            }

            if (this.Points?.Count > 0)
            {
                return this.Points.GroupBy(g => new { x = g.X, y = g.Y }).Any(g => g.Count() > 1);
            }
            else
            {
                return this.Geometries?.Any(g => g.HasDuplicatePoints()) == true;
            }
        }

        public void RemoveConsecutiveDuplicatePoints()
        {
            if (this.IsNullOrEmpty())
            {
                return;
            }

            if (this.IsLeafGeometry())
            {
                for (int i = this.Points.Count - 1; i >= 1; i--)
                {
                    if (this.Points[i].Equals(this.Points[i - 1]))
                    {
                        this.Points.RemoveAt(i);
                    }
                }
            }
            else
            {
                for (int i = this.Geometries.Count - 1; i >= 0; i--)
                {
                    if (this.Geometries[i] == null || this.Geometries[i].IsNullOrEmpty())
                    {
                        this.Geometries.RemoveAt(i);
                    }
                }

                for (int i = this.Geometries.Count - 1; i >= 0; i--)
                {
                    this.Geometries[i].RemoveConsecutiveDuplicatePoints();
                }
            }

            this.ReevaluateGeometryType();
        }


        private void ReevaluateGeometryType()
        {
            if (this == null || this.IsNullOrEmpty())
            {
                return;
            }

            var numberOfPoints = this.Points?.Count;

            var numberOfGeometries = this.Geometries?.Count;

            //حالت نقطه یا خط
            if (numberOfPoints > 0)
            {
                if (numberOfPoints == 1)
                {
                    this.Type = GeometryType.Point;
                }
                else
                {
                    this.Type = GeometryType.LineString;
                }
            }
            //سایر حالت‌ها
            else if (numberOfGeometries > 0)
            {
                var types = this.Geometries.Select(g => g.Type).Distinct();

                //حالت ترکیبی
                if (types.Count() > 1)
                {
                    this.Type = GeometryType.GeometryCollection;
                }
                else
                {
                    var subGeometryType = this.Geometries.First().Type;

                    switch (subGeometryType)
                    {
                        //حالت چند نقطه‌ای
                        case GeometryType.Point:
                            this.Type = GeometryType.MultiPoint;
                            break;

                        //حالت چند خطی یا چند ضلعی
                        case GeometryType.LineString:
                            if (this.Type == GeometryType.LineString)
                                this.Type = GeometryType.MultiLineString;
                            else if (this.Type == GeometryType.Polygon)
                                this.Type = GeometryType.MultiPoint;

                            break;

                        //حالت چندضلعی‌های چند تکه‌ای
                        case GeometryType.Polygon:
                            this.Type = GeometryType.MultiPolygon;
                            break;

                        case GeometryType.MultiPoint:
                        case GeometryType.MultiLineString:
                        case GeometryType.MultiPolygon:
                            this.Type = GeometryType.GeometryCollection;
                            break;

                        case GeometryType.GeometryCollection:
                        case GeometryType.CircularString:
                        case GeometryType.CompoundCurve:
                        case GeometryType.CurvePolygon:
                        default:
                            throw new NotImplementedException("Geometry.cs > ReevaluateGeometryType");
                    }
                }
            }

            return;
        }

        #endregion



        #region Conversions

        public Feature<T> AsFeature()
        {
            return new Feature<T>()
            {
                Attributes = new Dictionary<string, object>(),
                Id = 0,
                TheGeometry = this
            };
        }

        #endregion
    }
}
