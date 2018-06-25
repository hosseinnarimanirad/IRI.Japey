using IRI.Msh.Common.Extensions;
using IRI.Msh.Common.Primitives;
using IRI.Msh.CoordinateSystem.MapProjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRI.Msh.Common.Primitives
{
    public class Geometry
    {
        private static readonly Geometry _null = new Geometry();

        public static Geometry Null { get { return _null; } }

        //public static readonly Geometry EmptyPoint = new Geometry(new IPoint[0], GeometryType.Point);
        //public static readonly Geometry EmptyLineString = new Geometry(new IPoint[0], GeometryType.LineString);
        //public static readonly Geometry EmptyPolygon = new Geometry(new IPoint[0], GeometryType.Polygon);
        //public static readonly Geometry EmptyMultiPoint = new Geometry(new Geometry[0], GeometryType.MultiPoint);
        //public static readonly Geometry EmptyMultiLineString = new Geometry(new Geometry[0], GeometryType.MultiLineString);
        //public static readonly Geometry EmptyMultiPolygon = new Geometry(new Geometry[0], GeometryType.MultiPolygon);

        public static Geometry CreateEmpty(GeometryType type, int srid)
        {
            switch (type)
            {
                case GeometryType.GeometryCollection:
                    return new Geometry(new Geometry[0], GeometryType.GeometryCollection) { Srid = srid };

                case GeometryType.LineString:
                    return new Geometry(new Point[0], GeometryType.LineString) { Srid = srid };

                case GeometryType.MultiLineString:
                    return new Geometry(new Geometry[0], GeometryType.MultiLineString) { Srid = srid };

                case GeometryType.MultiPoint:
                    return new Geometry(new Geometry[0], GeometryType.MultiPoint) { Srid = srid };

                case GeometryType.MultiPolygon:
                    return new Geometry(new Geometry[0], GeometryType.MultiPolygon) { Srid = srid };

                case GeometryType.Point:
                    return new Geometry(new Point[0], GeometryType.Point) { Srid = srid };

                case GeometryType.Polygon:
                    return new Geometry(new Geometry[0], GeometryType.Polygon) { Srid = srid };

                case GeometryType.CircularString:
                case GeometryType.CompoundCurve:
                case GeometryType.CurvePolygon:
                default:
                    throw new NotImplementedException();
            }
        }

        public bool IsRingBase()
        {
            return this.Type == GeometryType.Polygon || this.Type == GeometryType.MultiPolygon || this.Type == GeometryType.CurvePolygon;
        }

        public GeometryType Type { get; set; }

        public bool IsNullOrEmpty()
        {
            return (this.Points == null || this.Points?.Length == 0) &&
                    (this.Geometries == null || this.Geometries?.Length == 0);
        }

        private IPoint[] _points;

        public IPoint[] Points
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

        private Geometry()
        {

        }

        private Geometry[] _geometries;

        public Geometry[] Geometries
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

        public int NumberOfPoints { get { return Points?.Length ?? 0; } }

        public int Srid { get; set; }

        public Geometry(IPoint[] points, GeometryType type, int srid = 0) : this(points, type, false, srid)
        {

        }

        public Geometry(IPoint[] points, GeometryType type, bool isClosed, int srid = 0)
        {
            if (type == GeometryType.LineString || type == GeometryType.Point)
            {
                if (type == GeometryType.LineString && isClosed && points?.Length > 0)
                {
                    var lastPoint = points[points.Length - 1];

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

        public Geometry(Geometry[] geometries, GeometryType type, int srid = 0)
        {
            if (type != GeometryType.MultiLineString && type != GeometryType.MultiPoint && type != GeometryType.MultiPolygon && type != GeometryType.Polygon && type != GeometryType.GeometryCollection)
            {
                throw new NotImplementedException();
            }

            this.Geometries = geometries;

            this.Type = type;

            this.Srid = srid;

            if (geometries.Length > 0)
            {
                var tempSubType = geometries.First().Type;

                if (geometries.Any(i => i.Type != tempSubType))
                {
                    this.Type = GeometryType.GeometryCollection;
                }
            }
        }

        public Geometry(Geometry geometry, GeometryType type, int srid) : this(new Geometry[] { geometry }, type, srid)
        {

        }


        public static Geometry Create(double x, double y, int srid = 0)
        {
            return new Geometry(new Point[] { new Point(x, y) }, GeometryType.Point, srid);
        }

        /// <summary>
        /// For Polygons do not repeat first point in the last point
        /// </summary>
        /// <param name="points"></param>
        /// <param name="type"></param>
        /// <param name="srid"></param>
        /// <returns></returns>
        public static Geometry Create(IPoint[] points, GeometryType type, int srid = 0)
        {
            if (points == null)
            {
                CreateEmpty(type, srid);
            }

            switch (type)
            {
                case GeometryType.Point:
                    return new Geometry(points, GeometryType.Point) { Srid = srid };
                //return CreatePointOrLineString(points, srid); //do not use this method: what if making a new Geometry LineString with first point

                case GeometryType.LineString:
                    return new Geometry(points, GeometryType.LineString) { Srid = srid };
                //return CreatePointOrLineString(points, srid);

                case GeometryType.Polygon:
                    return new Geometry(new Geometry(points, GeometryType.LineString), GeometryType.Polygon, srid);

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

        /// <summary>
        /// Does not filter point and multipoint features
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public Geometry SelectPoints(Func<IPoint[], IPoint[]> filter)
        {
            switch (this.Type)
            {
                case GeometryType.CircularString:
                case GeometryType.CompoundCurve:
                case GeometryType.CurvePolygon:
                    return Geometry.Null;

                case GeometryType.GeometryCollection:
                    return new Geometry(this.Geometries.Select(i => i.SelectPoints(filter)).ToArray(), GeometryType.GeometryCollection) { Srid = this.Srid };

                case GeometryType.MultiPoint:
                    return new Geometry(this.Geometries, GeometryType.MultiPoint) { Srid = this.Srid };

                case GeometryType.Point:
                    return new Geometry(this.Points, GeometryType.Point) { Srid = this.Srid };

                case GeometryType.LineString:
                    //return new Geometry(filter(this.Points), GeometryType.LineString) { Srid = this.Srid };
                    return CreatePointOrLineString(filter(this.Points), this.Srid);

                case GeometryType.MultiLineString:
                    return new Geometry(this.Geometries.Select(i => i.SelectPoints(filter)).ToArray(), GeometryType.MultiLineString) { Srid = this.Srid };

                case GeometryType.Polygon:
                    return new Geometry(this.Geometries.Select(i => i.SelectPoints(filter)).ToArray(), GeometryType.Polygon) { Srid = this.Srid };

                case GeometryType.MultiPolygon:
                    return new Geometry(this.Geometries.Select(i => i.SelectPoints(filter)).ToArray(), GeometryType.MultiPolygon) { Srid = this.Srid };

                default:
                    throw new NotImplementedException();
            }
        }

        public static Geometry CreatePointOrLineString(IPoint[] points, int srid)
        {
            if (points.Length == 1)
            {
                return new Geometry(points, GeometryType.Point) { Srid = srid };
            }
            else
            {
                return new Geometry(points, GeometryType.LineString) { Srid = srid };
            }
        }



        public Geometry Transform(Func<IPoint, IPoint> transform, int newSrid = 0)
        {
            switch (this.Type)
            {
                case GeometryType.CircularString:
                case GeometryType.CompoundCurve:
                case GeometryType.CurvePolygon:
                case GeometryType.GeometryCollection:
                    System.Diagnostics.Debug.WriteLine($"****WARNNING: Geometry.cs -> Filter method invalid geometry type");
                    return Geometry.Null;

                case GeometryType.MultiPoint:
                    return new Geometry(this.Geometries.Select(i => i.Transform(transform)).ToArray(), GeometryType.MultiPoint) { Srid = newSrid };

                case GeometryType.Point:
                    return new Geometry(this.Points.Select(i => transform(i)).ToArray(), GeometryType.Point) { Srid = newSrid };

                case GeometryType.LineString:
                    return new Geometry(this.Points.Select(i => transform(i)).ToArray(), GeometryType.LineString) { Srid = newSrid };

                case GeometryType.MultiLineString:
                    return new Geometry(this.Geometries.Select(i => i.Transform(transform)).ToArray(), GeometryType.MultiLineString) { Srid = newSrid };

                case GeometryType.MultiPolygon:
                    return new Geometry(this.Geometries.Select(i => i.Transform(transform)).ToArray(), GeometryType.MultiPolygon) { Srid = newSrid };

                case GeometryType.Polygon:
                    return new Geometry(this.Geometries.Select(i => i.Transform(transform)).ToArray(), GeometryType.Polygon) { Srid = newSrid };

                default:
                    throw new NotImplementedException();
            }
        }

        public IPoint[] GetAllPoints()
        {
            if (Points != null)
            {
                return Points;
            }
            else if (Geometries != null)
            {
                return Geometries.SelectMany(i => i.GetAllPoints()).ToArray();
            }
            else
            {
                return null;
            }
        }

        public bool HasAnyPoint()
        {
            if (Points != null)
            {
                return Points.Length > 0;
            }
            else if (Geometries != null)
            {
                return Geometries.Any(g => g.HasAnyPoint());
            }
            else
            {
                return false;
            }
        }

        public IPoint GetLastPoint()
        {
            if (Points?.Any() == true)
            {
                return Points.Last();
            }
            else if (Geometries?.Length > 0)
            {
                return Geometries.Last().GetLastPoint();
            }
            else
            {
                return null;
            }
        }

        public override string ToString()
        {
            return $"{Type} - #Points: {NumberOfPoints} - #Parts: {Geometries?.Count()}";
        }

        public BoundingBox GetBoundingBox()
        {
            return BoundingBox.CalculateBoundingBox(GetAllPoints());
        }

        public IPoint[] GetLastPart()
        {
            if (this.Points == null && this.Geometries == null)
                return null;

            if (this.Points == null)
            {
                return this.Geometries.Last().GetLastPart();
            }
            else
            {
                return this.Points;
            }
        }

        public bool TryFindPoint(IPoint point, out int pointIndex, out int geometryIndex, out int subGeometryIndex)
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
                for (int g = 0; g < this.Geometries.Length; g++)
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
                        for (int subG = 0; subG < this.Geometries[g].Geometries.Length; subG++)
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

        private bool TryFind(IPoint[] points, IPoint point, out int pointIndex)
        {
            pointIndex = -1;

            if (points == null)
                return false;

            for (int i = 0; i < points.Length; i++)
            {
                if (points[i] == point)
                {
                    pointIndex = i;

                    return true;
                }
            }

            return false; ;
        }

        private bool TryFind(IPoint[] points, double x, double y, out int pointIndex)
        {
            pointIndex = -1;

            if (points == null)
                return false;

            for (int i = 0; i < points.Length; i++)
            {
                if (points[i].X == x && points[i].Y == y)
                {
                    pointIndex = i;

                    return true;
                }
            }

            return false; ;
        }

        public void InsertPoint(IPoint newPoint, int index)
        {
            var points = this.Points.ToList();

            points.Insert(index, newPoint);

            this.Points = points.ToArray();
        }

        public void Remove(IPoint point)
        {
            var list = this.Points.ToList();

            list.Remove(point);

            this.Points = list.ToArray();
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

            this.Points = list.ToArray();
        }

        //This method can better using Array.Resize()
        public void AddLastPoint(IPoint newPoint)
        {
            if (this.Points != null)
            {
                var points = this.Points.ToList();

                points.Add(newPoint);

                this.Points = points.ToArray();
            }
            else
            {
                this.Geometries.Last().AddLastPoint(newPoint);
            }
        }

        public void AddGeometry(IPoint startPoint, GeometryType type)
        {
            if (this.Geometries == null)
                throw new NotImplementedException();

            var geometries = this.Geometries.ToList();

            geometries.Add(new Geometry(new IPoint[] { startPoint }, type));

            this.Geometries = geometries.ToArray();
        }

        public Geometry Clone()
        {
            if (this.Points != null)
            {
                return Geometry.Create((IPoint[])this.Points.Clone(), this.Type, this.Srid);
            }
            if (this.Geometries != null)
            {
                return new Geometry(this.Geometries.Select(g => g.Clone()).ToArray(), this.Type, this.Srid);
            }

            return new Geometry(null, this.Type, false, this.Srid);
        }

        public List<LineSegment> GetLineSegments()
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

        private List<LineSegment> GetLineSegments(bool isClosed)
        {
            List<LineSegment> result = new List<LineSegment>();

            if (Points != null)
            {
                for (int i = 0; i < this.Points.Length - 1; i++)
                {
                    result.Add(new LineSegment(Points[i], Points[i + 1]));
                }

                if (isClosed)
                {
                    //prevent returning segment when polygon has one point
                    if (Points.Length > 1)
                    {
                        result.Add(new LineSegment(Points[Points.Length - 1], Points[0]));
                    }
                }
            }

            return result;
        }

        public Point GetMeanPoint()
        {
            var allPoints = GetAllPoints();

            return new Point(allPoints.Sum(i => i.X) / allPoints.Length, allPoints.Sum(i => i.Y) / allPoints.Length);
        }

        //Returns last geometry ring or point
        public Geometry GetLastGeometryPart()
        {
            if (this.Points != null)
            {
                return this;
            }
            else if (this.Geometries != null)
            {
                if (this.Geometries.Length > 0)
                {
                    return this.Geometries.Last().GetLastGeometryPart();
                }
            }

            return null;
        }

        public void AddNewPart()
        {
            var currentGeometry = this.Clone();

            switch (this.Type)
            {
                case GeometryType.Point:
                    this.Geometries = new Geometry[2];
                    this.Geometries[0] = currentGeometry;
                    this.Geometries[1] = new Geometry(new IPoint[0], GeometryType.Point);
                    this.Points = null;
                    this.Type = GeometryType.MultiPoint;
                    break;

                case GeometryType.LineString:
                    this.Points = null;
                    this.Geometries = new Geometry[2];
                    this.Geometries[0] = currentGeometry;
                    this.Geometries[1] = new Geometry(new IPoint[0], GeometryType.LineString);

                    this.Type = GeometryType.MultiLineString;
                    break;

                case GeometryType.Polygon:
                    this.Geometries = new Geometry[2];
                    this.Geometries[0] = currentGeometry;
                    this.Geometries[1] = new Geometry(new Geometry[] { new Geometry(new IPoint[0], GeometryType.LineString) }, GeometryType.Polygon);
                    this.Type = GeometryType.MultiPolygon;
                    break;

                case GeometryType.MultiPoint:
                    var points = this.Geometries.ToList();
                    points.Add(new Geometry(new IPoint[0], GeometryType.Point));
                    this.Geometries = points.ToArray();
                    break;

                case GeometryType.MultiLineString:
                    var lines = this.Geometries.ToList();
                    lines.Add(new Geometry(new IPoint[0], GeometryType.LineString));
                    this.Geometries = lines.ToArray();
                    break;

                case GeometryType.MultiPolygon:
                    var polygons = this.Geometries.ToList();
                    polygons.Add(new Geometry(new Geometry[] { new Geometry(new IPoint[0], GeometryType.LineString) }, GeometryType.Polygon));
                    this.Geometries = polygons.ToArray();
                    break;



                case GeometryType.GeometryCollection:
                case GeometryType.CircularString:
                case GeometryType.CompoundCurve:
                case GeometryType.CurvePolygon:
                default:
                    throw new NotImplementedException();
            }
        }

        public void ClearEmptyGeometries()
        {
            if (Geometries == null)
            {
                return;
            }

            for (int i = Geometries.Length; i >= 0; i--)
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

        public Geometry GetRingOrLineStringPassingPoint(double x, double y)
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
                    for (int i = 0; i < this.Geometries.Length; i++)
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

        public bool TryRemovePart(Geometry geometry)
        {
            if (this.Geometries?.Length > 0)
            {
                for (int i = 0; i < this.Geometries.Length; i++)
                {
                    if (this.Geometries[i] == geometry)
                    {
                        var temp = this.Geometries.ToList();

                        temp.Remove(geometry);

                        this.Geometries = temp.ToArray();

                        return true;
                    }
                }
            }

            return false;
        }

        public bool TryRemoveEntireRingOrLineString(double x, double y)
        {
            var part = this.GetRingOrLineStringPassingPoint(x, y);

            if (this.Geometries?.Length > 0)
            {
                for (int i = 0; i < this.Geometries.Length; i++)
                {
                    switch (Geometries[i].Type)
                    {
                        case GeometryType.LineString:
                            if (this.TryRemovePart(part))
                                return true;
                            break;

                        case GeometryType.Polygon:
                            for (int g = Geometries[i].Geometries.Length - 1; g >= 0; g--)
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

        public SrsBase GetSrs()
        {
            return SridHelper.AsSrsBase(Srid);
        }

        public Geometry Project(SrsBase targetSrs)
        {
            var sourceSrs = GetSrs();

            return Project(sourceSrs, targetSrs);
        }

        public Geometry Project(SrsBase sourceSrs, SrsBase targetSrs)
        {
            if (sourceSrs.Ellipsoid.AreTheSame(targetSrs.Ellipsoid))
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


        #region Static Methods


        // To do: provide sample input and expected output for this method  
        public static Geometry ParseToGeodeticGeometry(double[][] geoCoordinates, GeometryType geometryType, bool isLongitudeFirst = false)
        {
            return new Geometry(geoCoordinates.Select(p => ParseLineStringToGeodeticGeometry(p, isLongitudeFirst)).ToArray(), geometryType, SridHelper.GeodeticWGS84);
        }

        // To do: provide sample input and expected output for this method
        private static Geometry ParseLineStringToGeodeticGeometry(double[] values, bool isLongitudeFirst)
        {
            if (values == null || values.Count() % 2 != 0)
            {
                throw new NotImplementedException();
            }

            List<Point> result = new List<Point>(values.Length / 2);

            if (isLongitudeFirst)
            {
                for (int i = 0; i < values.Length - 1; i += 2)
                {
                    result.Add(new Point(values[i], values[i + 1]));
                }
            }
            else
            {
                for (int i = 0; i < values.Length - 1; i += 2)
                {
                    result.Add(new Point(values[i + 1], values[i]));
                }
            }

            return new Geometry(result.ToArray(), GeometryType.LineString, SridHelper.GeodeticWGS84);

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

        public static Geometry ParsePointToGeometry(double[] xy, bool isLongitudeFirst, int srid = SridHelper.GeodeticWGS84)
        {
            return new Geometry(new Point[] { Point.Parse(xy, isLongitudeFirst) }, GeometryType.Point) { Srid = srid };
        }

        public static Geometry ParseLineStringToGeometry(double[][] geoCoordinates, GeometryType geometryType, bool isLongitudeFirst = false, int srid = SridHelper.GeodeticWGS84)
        {
            return new Geometry(geoCoordinates.Select(p => Point.Parse(p, isLongitudeFirst)).ToArray(), geometryType, srid);
        }

        public static Geometry ParsePolygonToGeometry(double[][][] rings, GeometryType geometryType, bool isLongFirst, int srid = SridHelper.GeodeticWGS84)
        {
            return new Geometry(rings.Select(p => ParseLineStringToGeometry(p, GeometryType.LineString, isLongFirst)).ToArray(), geometryType, srid);
        }

        #endregion
    }
}
