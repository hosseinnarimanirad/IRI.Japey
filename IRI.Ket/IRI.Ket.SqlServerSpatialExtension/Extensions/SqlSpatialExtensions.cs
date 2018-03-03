using IRI.Ham.SpatialBase;
using IRI.Ham.SpatialBase.Primitives;
using Microsoft.SqlServer.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRI.Ket.SpatialExtensions;
using IRI.Ham.Common.Helpers;
using IRI.Ket.Common.Model.Esri;
using IRI.Ham.SpatialBase.CoordinateSystems.MapProjection;

namespace IRI.Ket.SpatialExtensions
{
    public static class SqlSpatialExtensions
    {
        public static bool IsNullOrEmpty(this SqlGeography geography)
        {
            return geography == null || geography.IsNull || geography.STIsEmpty().IsTrue;
        }

        public static bool IsNullOrEmpty(this SqlGeometry geometry)
        {
            return geometry == null || geometry.IsNull || geometry.STIsEmpty().IsTrue;
        }

        public static bool IsNotValidOrEmpty(this SqlGeometry geometry)
        {
            return geometry.IsNullOrEmpty() || geometry.STIsValid().IsFalse;
        }

        public static int GetSrid(this SqlGeometry geometry)
        {
            if (geometry == null)
                return 0;

            var srid = geometry.STSrid;

            if (srid.IsNull)
            {
                return 0;
            }
            else
            {
                return geometry.STSrid.Value;
            }
        }

        public static double GetAreaInSquareKilometers(this SqlGeometry geometry, Func<IPoint, Point> toWgs84)
        {

            if (geometry == null)
            {
                return 0;
            }

            try
            {
                return geometry.Transform(i => Ham.CoordinateSystem.MapProjection.MapProjects.GeodeticToCylindricalEqualArea(toWgs84((Point)i)))
                                .STArea()
                                .Value / 1000000.0;
            }
            catch (Exception ex)
            {
                return 0;
            }

        }

        public static SqlGeometry Parse(string wkt)
        {
            return SqlGeometry.Parse(new System.Data.SqlTypes.SqlString(wkt));
        }

        public static SqlGeometry ParseFromEsriJson(string esriGeometryJson, string type)
        {
            return Parse(EsriJsonGeometry.Parse(esriGeometryJson, EnumHelper.Parse<EsriJsonGeometryType>(type)).AsWkt());
        }

        public static void AddEmptySqlGeometry(SqlGeometryBuilder builder, GeometryType type)
        {
            switch (type)
            {
                case GeometryType.LineString:
                    builder.BeginGeometry(OpenGisGeometryType.LineString);
                    break;

                case GeometryType.MultiLineString:
                    builder.BeginGeometry(OpenGisGeometryType.MultiLineString);
                    break;

                case GeometryType.MultiPoint:
                    builder.BeginGeometry(OpenGisGeometryType.MultiPoint);
                    break;

                case GeometryType.MultiPolygon:
                    builder.BeginGeometry(OpenGisGeometryType.MultiPolygon);
                    break;

                case GeometryType.Point:
                    builder.BeginGeometry(OpenGisGeometryType.Point);
                    break;

                case GeometryType.Polygon:
                    builder.BeginGeometry(OpenGisGeometryType.Polygon);
                    break;

                case GeometryType.GeometryCollection:
                    builder.BeginGeometry(OpenGisGeometryType.GeometryCollection);
                    break;

                case GeometryType.CircularString:
                    builder.BeginGeometry(OpenGisGeometryType.CircularString);
                    break;

                case GeometryType.CompoundCurve:
                    builder.BeginGeometry(OpenGisGeometryType.CompoundCurve);
                    break;

                case GeometryType.CurvePolygon:
                    builder.BeginGeometry(OpenGisGeometryType.CurvePolygon);
                    break;

                default:
                    throw new NotImplementedException();
            }

            builder.EndGeometry();
        }

        public static SqlGeometry CreateEmptySqlGeometry(GeometryType type, int srid)
        {
            switch (type)
            {
                case GeometryType.LineString:
                    return CreateEmptyLineString(srid);

                case GeometryType.MultiLineString:
                    return CreateEmptyMultiLineString(srid);

                case GeometryType.MultiPoint:
                    return CreateEmptyMultipoint(srid);

                case GeometryType.MultiPolygon:
                    return CreateEmptyMultiPolygon(srid);

                case GeometryType.Point:
                    return CreateEmptyPoint(srid);

                case GeometryType.Polygon:
                    return CreateEmptyPolygon(srid);

                case GeometryType.GeometryCollection:
                    return CreateEmptyGeometryCollection(srid);

                case GeometryType.CircularString:
                case GeometryType.CompoundCurve:
                case GeometryType.CurvePolygon:

                default:
                    throw new NotImplementedException();
            }

        }

        public static SqlGeometry CreateEmptyPoint(int srid)
        {
            return SqlGeometry.STGeomFromText(new System.Data.SqlTypes.SqlChars("POINT EMPTY"), srid);
        }

        public static SqlGeometry CreateEmptyLineString(int srid)
        {
            return SqlGeometry.STGeomFromText(new System.Data.SqlTypes.SqlChars("LINESTRING EMPTY"), srid);
        }

        public static SqlGeometry CreateEmptyPolygon(int srid)
        {
            return SqlGeometry.STGeomFromText(new System.Data.SqlTypes.SqlChars("POLYGON EMPTY"), srid);
        }

        public static SqlGeometry CreateEmptyMultipoint(int srid)
        {
            return SqlGeometry.STGeomFromText(new System.Data.SqlTypes.SqlChars("MULTIPOINT EMPTY"), srid);
        }

        public static SqlGeometry CreateEmptyMultiLineString(int srid)
        {
            return SqlGeometry.STGeomFromText(new System.Data.SqlTypes.SqlChars("MULTILINESTRING EMPTY"), srid);
        }

        public static SqlGeometry CreateEmptyMultiPolygon(int srid)
        {
            return SqlGeometry.STGeomFromText(new System.Data.SqlTypes.SqlChars("MULTIPOLYGON EMPTY"), srid);
        }

        public static SqlGeometry CreateEmptyGeometryCollection(int srid)
        {
            return SqlGeometry.STGeomFromText(new System.Data.SqlTypes.SqlChars("GEOMETRYCOLLECTION EMPTY"), srid);
        }

        public static string AsWkt(this SqlGeometry geometry)
        {
            return new string(geometry.STAsText().Value);
        }

        public static string AsWktZM(this SqlGeometry geometry)
        {
            return new string(geometry.AsTextZM().Value);
        }

        public static byte[] AsWkb(this SqlGeometry geometry)
        {
            return geometry.STAsBinary().Value;
        }

        public static byte[] AsWkbZm(this SqlGeometry geometry)
        {
            return geometry.AsBinaryZM().Value;
        }

        public static List<SqlGeometry> GetGeometries(this SqlGeometry geometry)
        {
            if (geometry.IsNullOrEmpty() || geometry.STNumGeometries().IsNull)
            {
                return new List<SqlGeometry>();
            }

            int numberOfGeometries = geometry.STNumGeometries().Value;

            var result = new List<SqlGeometry>(numberOfGeometries);

            for (int i = 0; i < numberOfGeometries; i++)
            {
                result.Add(geometry.STGeometryN(i + 1));
            }

            return result;
        }

        public static BoundingBox GetBoundingBox(this SqlGeometry geometry)
        {
            if (geometry.IsNullOrEmpty())
            {
                return new BoundingBox(double.NaN, double.NaN, double.NaN, double.NaN);
            }
            else if (geometry.STIsValid().IsFalse)
            {
                return new BoundingBox(double.NaN, double.NaN, double.NaN, double.NaN);
            }

            var envelope = geometry.STEnvelope();

            var point1 = envelope.STPointN(1);
            var point2 = envelope.STPointN(2);
            var point3 = envelope.STPointN(3);
            var point4 = envelope.STPointN(4);

            var xArray = new double[] { point1.STX.Value, point2.STX.Value, point3.STX.Value, point4.STX.Value };

            var yArray = new double[] { point1.STY.Value, point2.STY.Value, point3.STY.Value, point4.STY.Value };

            return new BoundingBox(xArray.Min(), yArray.Min(), xArray.Max(), yArray.Max());
        }

        public static BoundingBox GetBoundingBox(this List<SqlGeometry> spatialFeatures)
        {
            if (spatialFeatures == null || spatialFeatures.Count < 1)
            {
                return new BoundingBox(double.NaN, double.NaN, double.NaN, double.NaN);
            }

            var envelopes = spatialFeatures.Select(i => i?.STEnvelope()).Where(i => !i.IsNullOrEmpty()).ToList();


            //var xValues = envelopes.SelectMany(i => new double[] { i.STPointN(1).STX.Value, i.STPointN(2).STX.Value, i.STPointN(3).STX.Value, i.STPointN(4).STX.Value });

            //var yValues = envelopes.SelectMany(i => new double[] { i.STPointN(1).STY.Value, i.STPointN(2).STY.Value, i.STPointN(3).STY.Value, i.STPointN(4).STY.Value });

            //return new BoundingBox(xValues.Min(), yValues.Min(), xValues.Max(), yValues.Max());

            return GetBoundingBoxFromEnvelopes(envelopes);

            //Method 0
            //return BoundingBox.GetMergedBoundingBox(spatialFeatures.Select(i => i.GetBoundingBox()).Where(i => !i.IsNaN()));
        }

        public static BoundingBox GetBoundingBoxFromEnvelopes(List<SqlGeometry> envelopes)
        {
            if (envelopes == null)
            {
                return BoundingBox.NaN;
            }

            var validEnvelopes = envelopes.Where(i => !i.IsNotValidOrEmpty()).ToList();

            if (validEnvelopes.Count == 0)
            {
                return BoundingBox.NaN;
            }

            var xValues = validEnvelopes.SelectMany(i => new double[] { i.STPointN(1).STX.Value, i.STPointN(2).STX.Value, i.STPointN(3).STX.Value, i.STPointN(4).STX.Value });

            var yValues = validEnvelopes.SelectMany(i => new double[] { i.STPointN(1).STY.Value, i.STPointN(2).STY.Value, i.STPointN(3).STY.Value, i.STPointN(4).STY.Value });

            return new BoundingBox(xValues.Min(), yValues.Min(), xValues.Max(), yValues.Max());
        }



        public static OpenGisGeometryType GetOpenGisType(this SqlGeometry geometry)
        {
            if (geometry == null)
            {
                return OpenGisGeometryType.GeometryCollection;
            }
            else
            {
                return (OpenGisGeometryType)Enum.Parse(typeof(OpenGisGeometryType), geometry.STGeometryType().Value, true);
            }

            //if (geometry.IsNullOrEmpty())
            //{
            //    return OpenGisGeometryType.GeometryCollection;
            //}
            //else
            //{
            //    return (OpenGisGeometryType)Enum.Parse(typeof(OpenGisGeometryType), geometry.STGeometryType().Value, true);
            //}
        }

        public static Point AsPoint(this SqlGeography point)
        {
            if (point.IsNullOrEmpty() || point.Long.IsNull || point.Lat.IsNull)
            {
                return new Point(double.NaN, double.NaN);
            }
            else
            {
                return new Point(point.Long.Value, point.Lat.Value);
            }
        }

        public static IPoint AsPoint(this SqlGeometry point)
        {
            if (point.IsNullOrEmpty() || point.STX.IsNull || point.STY.IsNull)
            {
                return new Point(double.NaN, double.NaN);
            }
            else
            {
                return new Point(point.STX.Value, point.STY.Value);
            }
        }

        public static SqlGeometry GetCentroidPlus(this SqlGeometry geometry)
        {
            if (geometry.IsNotValidOrEmpty())
            {
                return SqlGeometry.Null;
            }

            return geometry.STContains(geometry.STCentroid()).Value ? geometry.STCentroid() : geometry.STPointOnSurface();
        }






        #region Projection (SqlGeography)


        public static SqlGeometry Project(this SqlGeography geography, Func<IPoint, IPoint> mapFunction, int newSrid = 0)
        {

            SqlGeometryBuilder builder = new SqlGeometryBuilder();

            builder.SetSrid(newSrid);

            geography = geography.MakeValid();

            OpenGisGeometryType geometryType = (OpenGisGeometryType)Enum.Parse(typeof(OpenGisGeometryType), geography.STGeometryType().Value, true);

            builder.BeginGeometry(geometryType);

            switch (geometryType)
            {
                case OpenGisGeometryType.CircularString:
                case OpenGisGeometryType.CompoundCurve:
                case OpenGisGeometryType.CurvePolygon:
                case OpenGisGeometryType.GeometryCollection:
                    throw new NotImplementedException();

                case OpenGisGeometryType.LineString:
                    ProjectLineString(builder, geography, mapFunction);
                    break;

                case OpenGisGeometryType.MultiLineString:
                    ProjectMultiLineSring(builder, geography, mapFunction);
                    break;

                case OpenGisGeometryType.MultiPoint:
                    ProjectMultiPoint(builder, geography, mapFunction);
                    break;

                case OpenGisGeometryType.MultiPolygon:
                    ProjectMultiPolygon(builder, geography, mapFunction);
                    break;

                case OpenGisGeometryType.Point:
                    ProjectPoint(builder, geography, mapFunction);
                    break;

                case OpenGisGeometryType.Polygon:
                    ProjectPolygon(builder, geography, mapFunction);
                    break;

                default:
                    throw new NotImplementedException();
            }

            builder.EndGeometry();

            return builder.ConstructedGeometry.STIsValid().IsTrue ? builder.ConstructedGeometry : builder.ConstructedGeometry.MakeValid();
        }

        public static SqlGeometry Transform(this SqlGeometry geometry, Func<IPoint, IPoint> mapFunction, int newSrid = 0)
        {
            return geometry.ExtractPoints().Transform(mapFunction, newSrid).AsSqlGeometry();
        }

        //Not supporting Z and M values
        public static void ProjectMultiPolygon(SqlGeometryBuilder builder, SqlGeography multiPolygon, Func<IPoint, IPoint> mapFunction)
        {
            int numberOfGeometries = multiPolygon.STNumGeometries().Value;

            for (int i = 1; i <= numberOfGeometries; i++)
            {
                builder.BeginGeometry(OpenGisGeometryType.Polygon);

                ProjectPolygon(builder, multiPolygon.STGeometryN(i), mapFunction);

                builder.EndGeometry();
            }
        }

        //Not supporting Z and M values
        public static void ProjectPolygon(SqlGeometryBuilder builder, SqlGeography geometry, Func<IPoint, IPoint> mapFunction)
        {
            //ProjectRing(builder, geometry.STExteriorRing(), mapFunction);

            //int numberOfInteriorRings = geometry.STNumInteriorRing().Value;
            int numberOfRings = geometry.NumRings().Value;

            for (int i = 1; i <= numberOfRings; i++)
            {
                //ProjectRing(builder, geometry.STInteriorRingN(i), mapFunction);
                ProjectLineString(builder, geometry.RingN(i), mapFunction);
            }
        }

        //Not supporting Z and M values
        public static void ProjectMultiLineSring(SqlGeometryBuilder builder, SqlGeography multiLineString, Func<IPoint, IPoint> mapFunction)
        {
            int numberOfGeometries = multiLineString.STNumGeometries().Value;

            for (int i = 1; i <= numberOfGeometries; i++)
            {
                builder.BeginGeometry(OpenGisGeometryType.LineString);

                ProjectLineString(builder, multiLineString.STGeometryN(i), mapFunction);

                builder.EndGeometry();
            }
        }

        //Not supporting Z and M values
        public static void ProjectLineString(SqlGeometryBuilder builder, SqlGeography lineString, Func<IPoint, IPoint> mapFunction)
        {
            int numberOfPoints = lineString.STNumPoints().Value;

            //Point startPoint = mapFunction(new Point(lineString.STStartPoint().Long.Value, lineString.STStartPoint().Lat.Value));
            var startPoint = mapFunction(lineString.STStartPoint().AsPoint());

            builder.BeginFigure(startPoint.X, startPoint.Y);

            for (int i = 2; i <= numberOfPoints; i++)
            {
                var point = mapFunction(GetPoint(lineString, i));

                builder.AddLine(point.X, point.Y);
            }

            builder.EndFigure();
        }

        //Not supporting Z and M values
        //?? possible bug: start value for i
        public static void ProjectMultiPoint(SqlGeometryBuilder builder, SqlGeography multiPoint, Func<IPoint, IPoint> mapFunction)
        {
            int numberOfGeometries = multiPoint.STNumGeometries().Value;

            for (int i = 1; i <= numberOfGeometries; i++)
            {
                builder.BeginGeometry(OpenGisGeometryType.Point);

                ProjectPoint(builder, multiPoint.STGeometryN(i), mapFunction);

                builder.EndGeometry();
            }
        }

        //Not supporting Z and M values
        public static void ProjectPoint(SqlGeometryBuilder builder, SqlGeography point, Func<IPoint, IPoint> mapFunction)
        {
            //Point thePoint = mapFunction(new Point(point.Long.Value, point.Lat.Value));
            var thePoint = mapFunction(point.AsPoint());

            builder.BeginFigure(thePoint.X, thePoint.Y);

            builder.EndFigure();
        }

        private static Point GetPoint(SqlGeography geography, int index)
        {
            return geography.STPointN(index).AsPoint();

            //return new IRI.Ham.SpatialBase.Point(temp.Long.Value, temp.Lat.Value);
        }

        public static SqlGeometry GeodeticToMercator(this SqlGeography geometry)
        {
            return Project(geometry, point => IRI.Ham.CoordinateSystem.MapProjection.MapProjects.GeodeticToMercator(point, IRI.Ham.CoordinateSystem.Ellipsoids.WGS84));
        }

        public static SqlGeometry GeodeticWgs84ToWebMercator(this SqlGeography geometry)
        {
            return Project(geometry, point => IRI.Ham.CoordinateSystem.MapProjection.MapProjects.GeodeticWgs84ToWebMercator(point), SridHelper.WebMercator);
        }

        public static SqlGeometry GeodeticToCylindricalEqualArea(this SqlGeography geometry)
        {
            return Project(geometry, point => IRI.Ham.CoordinateSystem.MapProjection.MapProjects.GeodeticToCylindricalEqualArea(point, IRI.Ham.CoordinateSystem.Ellipsoids.WGS84));
        }

        #endregion


        #region Projection (SqlGeometry)

        public static SqlGeography Project(this SqlGeometry geometry, Func<IPoint, IPoint> mapFunction, int srid)
        {
            var builder = new SqlGeographyBuilder();

            builder.SetSrid(srid);

            geometry = geometry.MakeValid();

            OpenGisGeographyType geographyType = (OpenGisGeographyType)Enum.Parse(typeof(OpenGisGeographyType), geometry.STGeometryType().Value, true);

            builder.BeginGeography(geographyType);

            switch (geographyType)
            {
                case OpenGisGeographyType.Point:
                    ProjectPoint(builder, geometry, mapFunction);
                    break;

                case OpenGisGeographyType.LineString:
                    ProjectLineString(builder, geometry, mapFunction);
                    break;

                case OpenGisGeographyType.Polygon:
                    ProjectPolygon(builder, geometry, mapFunction);
                    break;

                case OpenGisGeographyType.MultiPoint:
                    ProjectMultiPoint(builder, geometry, mapFunction);
                    break;

                case OpenGisGeographyType.MultiLineString:
                    ProjectMultiLineSring(builder, geometry, mapFunction);
                    break;

                case OpenGisGeographyType.MultiPolygon:
                    ProjectMultiPolygon(builder, geometry, mapFunction);
                    break;

                case OpenGisGeographyType.GeometryCollection:
                case OpenGisGeographyType.CircularString:
                case OpenGisGeographyType.CompoundCurve:
                case OpenGisGeographyType.CurvePolygon:
                case OpenGisGeographyType.FullGlobe:
                default:
                    throw new NotImplementedException();
            }

            builder.EndGeography();

            var result = builder.ConstructedGeography.STIsValid().IsTrue ? builder.ConstructedGeography : builder.ConstructedGeography.MakeValid();

            return result.EnvelopeAngle().Value == 180 ? result.ReorientObject() : result;


        }

        //Not supporting Z and M values
        public static void ProjectMultiPolygon(SqlGeographyBuilder builder, SqlGeometry multiPolygon, Func<IPoint, IPoint> mapFunction)
        {
            int numberOfGeometries = multiPolygon.STNumGeometries().Value;

            for (int i = 1; i <= numberOfGeometries; i++)
            {
                builder.BeginGeography(OpenGisGeographyType.Polygon);

                ProjectPolygon(builder, multiPolygon.STGeometryN(i), mapFunction);

                builder.EndGeography();
            }
        }

        //Not supporting Z and M values
        public static void ProjectPolygon(SqlGeographyBuilder builder, SqlGeometry polygon, Func<IPoint, IPoint> mapFunction)
        {
            ProjectLineString(builder, polygon.STExteriorRing(), mapFunction);

            int numberOfInteriorRings = polygon.STNumInteriorRing().Value;
            //int numberOfRings = polygon.STNumCurves().Value;

            for (int i = 1; i <= numberOfInteriorRings; i++)
            {
                //ProjectRing(builder, geometry.STInteriorRingN(i), mapFunction);
                ProjectLineString(builder, polygon.STInteriorRingN(i), mapFunction);
            }
        }

        //Not supporting Z and M values
        public static void ProjectMultiLineSring(SqlGeographyBuilder builder, SqlGeometry multiLineString, Func<IPoint, IPoint> mapFunction)
        {
            int numberOfGeometries = multiLineString.STNumGeometries().Value;

            for (int i = 1; i <= numberOfGeometries; i++)
            {
                builder.BeginGeography(OpenGisGeographyType.LineString);

                ProjectLineString(builder, multiLineString.STGeometryN(i), mapFunction);

                builder.EndGeography();
            }
        }

        //Not supporting Z and M values
        public static void ProjectLineString(SqlGeographyBuilder builder, SqlGeometry lineString, Func<IPoint, IPoint> mapFunction)
        {
            if (lineString.IsNull)
                return;

            int numberOfPoints = lineString.STNumPoints().Value;

            //Point startPoint = mapFunction(new Point(lineString.STStartPoint().Long.Value, lineString.STStartPoint().Lat.Value));
            var startPoint = mapFunction(lineString.STStartPoint().AsPoint());

            builder.BeginFigure(startPoint.Y, startPoint.X);

            for (int i = 2; i <= numberOfPoints; i++)
            {
                var point = mapFunction(GetPoint(lineString, i));

                builder.AddLine(point.Y, point.X);
            }

            builder.EndFigure();
        }

        //Not supporting Z and M values
        //?? possible bug: start value for i
        public static void ProjectMultiPoint(SqlGeographyBuilder builder, SqlGeometry multiPoint, Func<IPoint, IPoint> mapFunction)
        {
            int numberOfGeometries = multiPoint.STNumGeometries().Value;

            for (int i = 1; i <= numberOfGeometries; i++)
            {
                builder.BeginGeography(OpenGisGeographyType.Point);

                ProjectPoint(builder, multiPoint.STGeometryN(i), mapFunction);

                builder.EndGeography();
            }
        }

        //Not supporting Z and M values
        public static void ProjectPoint(SqlGeographyBuilder builder, SqlGeometry point, Func<IPoint, IPoint> mapFunction)
        {
            //Point thePoint = mapFunction(new Point(point.Long.Value, point.Lat.Value));
            var thePoint = mapFunction(point.AsPoint());

            builder.BeginFigure(thePoint.Y, thePoint.X);

            builder.EndFigure();
        }

        private static IPoint GetPoint(SqlGeometry geometry, int index)
        {
            return geometry.STPointN(index).AsPoint();

            //return new IRI.Ham.SpatialBase.Point(temp.Long.Value, temp.Lat.Value);
        }

        public static SqlGeography WebMercatorToGeographic(this SqlGeometry geometry)
        {
            return geometry.Project(Ham.CoordinateSystem.MapProjection.MapProjects.WebMercatorToGeodeticWgs84, SridHelper.GeodeticWGS84);
        }

        public static SqlGeography MercatorToGeographic(this SqlGeometry geometry)
        {
            return geometry.Project(Ham.CoordinateSystem.MapProjection.MapProjects.WebMercatorToGeodeticWgs84, SridHelper.GeodeticWGS84);
        }

        public static SqlGeography UTMToGeographic(this SqlGeometry geometry, int utmZone)
        {
            return geometry.Project(i => Ham.CoordinateSystem.MapProjection.MapProjects.UTMToGeodetic(i, utmZone), SridHelper.GeodeticWGS84);
        }

        #endregion


        #region Extract Points

        //public static Geometry CreateEmpty

        public static Geometry ExtractPoints(this SqlGeometry geometry)
        {
            //This check is not required bacause it is already checked at evey ExtractXXXX function
            //this is specially for MultiXXX types. What is multipolygon was not Empty but first GeometryN of it was empty
            //if (geometry.IsNullOrEmpty())
            //{
            //    return Geometry.CreateEmpty(geometry.GetOpenGisType().ToGeometryType(), geometry.GetSrid());
            //}

            Geometry result;

            switch (geometry.GetOpenGisType())
            {
                case OpenGisGeometryType.Point:
                    result = ExtractPointsFromPoint(geometry);
                    break;

                case OpenGisGeometryType.MultiPoint:
                    result = ExtractPointsFromMultiPoint(geometry);
                    break;

                case OpenGisGeometryType.LineString:
                    result = ExtractPointsFromLineString(geometry, false);
                    break;

                case OpenGisGeometryType.MultiLineString:
                    result = ExtractPointsFromMultiLineString(geometry);
                    break;

                case OpenGisGeometryType.Polygon:
                    result = ExtractPointsFromPolygon(geometry);
                    break;

                case OpenGisGeometryType.MultiPolygon:
                    result = ExtractPointsFromMultiPolygon(geometry);
                    break;

                case OpenGisGeometryType.GeometryCollection:
                    result = ExtractPointsFromGeometryCollection(geometry);
                    break;

                case OpenGisGeometryType.CircularString:
                case OpenGisGeometryType.CompoundCurve:
                case OpenGisGeometryType.CurvePolygon:
                default:
                    throw new NotImplementedException();
            }

            result.Srid = geometry.GetSrid();

            return result;
        }

        private static Geometry ExtractPointsFromPoint(SqlGeometry geometry)
        {
            var srid = geometry.GetSrid();

            //This check is required
            if (geometry.IsNullOrEmpty())
                return Geometry.CreateEmpty(GeometryType.Point, srid);

            return new Geometry(new IPoint[] { geometry.AsPoint() }, GeometryType.Point) { Srid = srid };
        }

        private static Geometry ExtractPointsFromMultiPoint(SqlGeometry geometry)
        {
            var srid = geometry.GetSrid();

            //This check is required
            if (geometry.IsNullOrEmpty())
                return Geometry.CreateEmpty(GeometryType.MultiPoint, srid);

            var numberOfGeometries = geometry.STNumGeometries().Value;

            Geometry[] result = new Geometry[numberOfGeometries];

            for (int i = 1; i <= numberOfGeometries; i++)
            {
                result[i - 1] = ExtractPointsFromPoint(geometry.STGeometryN(i));
            }

            return new Geometry(result, GeometryType.MultiPoint) { Srid = srid };
        }

        private static Geometry ExtractPointsFromLineString(SqlGeometry geometry, bool isRing)
        {
            var srid = geometry.GetSrid();

            //This check is required
            if (geometry.IsNullOrEmpty())
                return Geometry.CreateEmpty(GeometryType.LineString, srid);

            int numberOfPoints = isRing ? geometry.STNumPoints().Value - 1 : geometry.STNumPoints().Value;

            Point[] result = new Point[numberOfPoints];

            for (int i = 1; i <= numberOfPoints; i++)
            {
                result[i - 1] = new Point(geometry.STPointN(i).STX.Value, geometry.STPointN(i).STY.Value);
            }

            return new Geometry(result, GeometryType.LineString) { Srid = srid };
        }

        private static Geometry ExtractPointsFromMultiLineString(SqlGeometry geometry)
        {
            var srid = geometry.GetSrid();

            //This check is required
            if (geometry.IsNullOrEmpty())
                return Geometry.CreateEmpty(GeometryType.MultiLineString, srid);

            int numberOfParts = geometry.STNumGeometries().Value;

            Geometry[] result = new Geometry[numberOfParts];

            for (int i = 1; i <= numberOfParts; i++)
            {
                result[i - 1] = ExtractPointsFromLineString(geometry.STGeometryN(i), false);
            }

            return new Geometry(result, GeometryType.MultiLineString) { Srid = srid };
        }

        private static Geometry ExtractPointsFromPolygon(SqlGeometry geometry)
        {
            var srid = geometry.GetSrid();

            //This check is required
            if (geometry.IsNullOrEmpty())
                return Geometry.CreateEmpty(GeometryType.Polygon, srid);

            var numberOfInteriorRings = geometry.STNumInteriorRing().Value;

            Geometry exteriorRing = ExtractPointsFromLineString(geometry.STExteriorRing(), true);

            Geometry[] result = new Geometry[numberOfInteriorRings + 1];

            result[0] = exteriorRing;

            for (int i = 1; i <= numberOfInteriorRings; i++)
            {
                //The first result contains the exterior ring so start at i 
                result[i] = ExtractPointsFromLineString(geometry.STInteriorRingN(i), true);
            }

            return new Geometry(result, GeometryType.Polygon) { Srid = srid };
        }

        private static Geometry ExtractPointsFromMultiPolygon(SqlGeometry geometry)
        {
            var srid = geometry.GetSrid();

            //This check is required
            if (geometry.IsNullOrEmpty())
                return Geometry.CreateEmpty(GeometryType.MultiPolygon, srid);

            var numberOfGeometries = geometry.STNumGeometries().Value;

            Geometry[] result = new Geometry[numberOfGeometries];

            for (int i = 0; i < numberOfGeometries; i++)
            {
                result[i] = ExtractPointsFromPolygon(geometry.STGeometryN(i + 1));
            }

            return new Geometry(result, GeometryType.MultiPolygon) { Srid = srid };
        }

        private static Geometry ExtractPointsFromGeometryCollection(SqlGeometry geometry)
        {
            var srid = geometry.GetSrid();

            //This check is required
            if (geometry.IsNullOrEmpty())
                return Geometry.CreateEmpty(GeometryType.GeometryCollection, srid);

            var numberOfGeometries = geometry.STNumGeometries().Value;

            Geometry[] result = new Geometry[numberOfGeometries];

            for (int i = 1; i <= numberOfGeometries; i++)
            {
                result[i - 1] = geometry.STGeometryN(i).ExtractPoints();
            }

            return new Geometry(result, GeometryType.GeometryCollection) { Srid = srid };
        }

        #endregion



        #region Esri Json Geometry

        private static double?[] GetEsriJsonObjectPoint(SqlGeometry point)
        {
            if (point.IsNullOrEmpty())
                return new double?[0];

            return new double?[] { point.STX.Value, point.STY.Value };
        }

        private static double?[][] GetLineStringOrRing(SqlGeometry lineStringOrRing)
        {
            if (lineStringOrRing.IsNullOrEmpty())
                return new double?[0][];

            int numberOfPoints = lineStringOrRing.STNumPoints().Value;

            double?[][] result = new double?[numberOfPoints][];

            for (int i = 1; i <= numberOfPoints; i++)
            {
                result[i - 1] = GetEsriJsonObjectPoint(lineStringOrRing.STPointN(i));
            }

            return result;
        }

        //Not supportig Z and M Values
        private static EsriJsonGeometry SqlPointToEsriJsonPoint(this SqlGeometry geometry)
        {
            //This check is required
            if (geometry.IsNullOrEmpty())
                return new EsriJsonGeometry()
                {
                    Type = EsriJsonGeometryType.point,
                };

            return new EsriJsonGeometry()
            {
                X = geometry.STX.Value,
                Y = geometry.STY.Value,
                Type = EsriJsonGeometryType.point,
                SpatialReference = new EsriJsonSpatialreference() { Wkid = geometry.GetSrid() }
            };
        }

        //Not supportig Z and M Values
        private static EsriJsonGeometry SqlMultiPointToEsriJsonMultiPoint(this SqlGeometry geometry)
        {
            //This check is required
            if (geometry.IsNullOrEmpty())
                return new EsriJsonGeometry()
                {
                    Type = EsriJsonGeometryType.multipoint,
                    Points = new double?[0][],
                };

            var numberOfGeometries = geometry.STNumGeometries().Value;

            double?[][] points = new double?[numberOfGeometries][];

            for (int i = 1; i <= numberOfGeometries; i++)
            {
                points[i - 1] = GetEsriJsonObjectPoint(geometry.STGeometryN(i));
            }

            return new EsriJsonGeometry()
            {
                Points = points,
                Type = EsriJsonGeometryType.multipoint,
                SpatialReference = new EsriJsonSpatialreference() { Wkid = geometry.GetSrid() }
            };
        }

        //Not supportig Z and M Values
        private static EsriJsonGeometry SqlLineStringToEsriJsonPolyline(this SqlGeometry geometry)
        {
            //This check is required
            if (geometry.IsNullOrEmpty())
                return new EsriJsonGeometry()
                {
                    Type = EsriJsonGeometryType.polyline,
                    Paths = new double?[0][][],
                };

            double?[][][] paths = new double?[1][][] { GetLineStringOrRing(geometry) };

            return new EsriJsonGeometry()
            {
                Paths = paths,
                Type = EsriJsonGeometryType.polyline,
                SpatialReference = new EsriJsonSpatialreference() { Wkid = geometry.GetSrid() }
            };
        }

        //Not supportig Z and M Values
        private static EsriJsonGeometry SqlMultiLineStringToEsriJsonPolyline(this SqlGeometry geometry)
        {
            //This check is required
            if (geometry.IsNullOrEmpty())
                return new EsriJsonGeometry()
                {
                    Type = EsriJsonGeometryType.polyline,
                    Paths = new double?[0][][],
                };

            int numberOfParts = geometry.STNumGeometries().Value;

            double?[][][] result = new double?[numberOfParts][][];

            for (int i = 1; i <= numberOfParts; i++)
            {
                result[i - 1] = GetLineStringOrRing(geometry.STGeometryN(i));
            }

            return new EsriJsonGeometry()
            {
                Paths = result,
                Type = EsriJsonGeometryType.polyline,
                SpatialReference = new EsriJsonSpatialreference() { Wkid = geometry.GetSrid() }
            };
        }

        //Not supportig Z and M Values
        private static EsriJsonGeometry SqlPolygonToEsriJsonPolygon(this SqlGeometry geometry)
        {
            //This check is required
            if (geometry.IsNullOrEmpty())
                return new EsriJsonGeometry()
                {
                    Type = EsriJsonGeometryType.polygon,
                    Rings = new double?[0][][],
                };

            double?[][][] rings = new double?[1][][] { GetLineStringOrRing(geometry) };

            return new EsriJsonGeometry()
            {
                Paths = rings,
                Type = EsriJsonGeometryType.polygon,
                SpatialReference = new EsriJsonSpatialreference() { Wkid = geometry.GetSrid() }
            };
        }

        //Not supportig Z and M Values
        private static EsriJsonGeometry SqlMultiPolygonToEsriJsonMultiPolygon(this SqlGeometry geometry)
        {
            //This check is required
            if (geometry.IsNullOrEmpty())
                return new EsriJsonGeometry()
                {
                    Type = EsriJsonGeometryType.polygon,
                    Rings = new double?[0][][],
                };

            int numberOfParts = geometry.STNumGeometries().Value;

            double?[][][] rings = new double?[numberOfParts][][];

            for (int i = 1; i <= numberOfParts; i++)
            {
                rings[i - 1] = GetLineStringOrRing(geometry.STGeometryN(i));
            }

            return new EsriJsonGeometry()
            {
                Paths = rings,
                Type = EsriJsonGeometryType.polygon,
                SpatialReference = new EsriJsonSpatialreference() { Wkid = geometry.GetSrid() }
            };
        }

        public static EsriJsonGeometry ParseToEsriJsonGeometry(this SqlGeometry geometry)
        {
            //if (geometry.IsNotValidOrEmpty())
            //{
            //    throw new NotImplementedException();
            //}

            OpenGisGeometryType geometryType = geometry.GetOpenGisType();

            switch (geometryType)
            {
                case OpenGisGeometryType.CircularString:
                case OpenGisGeometryType.CompoundCurve:
                case OpenGisGeometryType.CurvePolygon:
                case OpenGisGeometryType.GeometryCollection:
                default:
                    throw new NotImplementedException();

                case OpenGisGeometryType.Point:
                    return geometry.SqlPointToEsriJsonPoint();

                case OpenGisGeometryType.MultiPoint:
                    return SqlMultiPointToEsriJsonMultiPoint(geometry);

                case OpenGisGeometryType.LineString:
                    return SqlLineStringToEsriJsonPolyline(geometry);

                case OpenGisGeometryType.MultiLineString:
                    return SqlMultiLineStringToEsriJsonPolyline(geometry);

                case OpenGisGeometryType.Polygon:
                    return SqlPolygonToEsriJsonPolygon(geometry);

                case OpenGisGeometryType.MultiPolygon:
                    return SqlMultiPolygonToEsriJsonMultiPolygon(geometry);
            }
        }

        #endregion


        #region Path Markup

        private static string LineStringOrRingToPathMarkup(SqlGeometry lineStringOrRing, double decimals)
        {
            if (lineStringOrRing.IsNullOrEmpty())
                return string.Empty;

            int numberOfPoints = lineStringOrRing.STNumPoints().Value;

            StringBuilder result = new StringBuilder(" M ");

            if (double.IsNaN(decimals))
            {
                for (int i = 1; i <= numberOfPoints; i++)
                {
                    var p = lineStringOrRing.STPointN(i);

                    result.Append($"{p.STX.Value},{-p.STY.Value}L ");
                }
            }
            else
            {
                int digits = (int)decimals;

                for (int i = 1; i <= numberOfPoints; i++)
                {
                    var p = lineStringOrRing.STPointN(i);

                    result.Append($"{Math.Round(p.STX.Value, digits)},{Math.Round(-p.STY.Value, digits)}L ");
                }
            }


            result.Remove(result.Length - 2, 1);

            result.Append(" Z ");

            return result.ToString();
        }


        //Not supportig Z and M Values
        private static string SqlLineStringToPathMarkup(this SqlGeometry geometry, double decimals)
        {
            //This check is required
            if (geometry.IsNullOrEmpty())
                return string.Empty;

            return LineStringOrRingToPathMarkup(geometry, decimals);
        }

        //Not supportig Z and M Values
        private static string SqlMultiLineStringToPathMarkup(this SqlGeometry geometry, double decimals)
        {
            //This check is required
            if (geometry.IsNullOrEmpty())
                return string.Empty;

            int numberOfParts = geometry.STNumGeometries().Value;

            StringBuilder result = new StringBuilder();

            for (int i = 1; i <= numberOfParts; i++)
            {
                result.Append(LineStringOrRingToPathMarkup(geometry.STGeometryN(i), decimals));
            }

            return result.ToString();
        }

        //Not supportig Z and M Values
        private static string SqlPolygonToPathMarkup(this SqlGeometry geometry, double decimals)
        {
            //This check is required
            if (geometry.IsNullOrEmpty())
                return string.Empty;

            var numberOfInteriorRings = geometry.STNumInteriorRing().Value;

            StringBuilder result = new StringBuilder(LineStringOrRingToPathMarkup(geometry.STExteriorRing(), decimals));

            for (int i = 1; i <= numberOfInteriorRings; i++)
            {
                //The first result contains the exterior ring so start at i 
                result.Append(LineStringOrRingToPathMarkup(geometry.STInteriorRingN(i), decimals));
            }

            return result.ToString();
        }

        //Not supportig Z and M Values
        private static string SqlMultiPolygonToPathMarkup(this SqlGeometry geometry, double decimals)
        {
            //This check is required
            if (geometry.IsNullOrEmpty())
                return string.Empty;

            int numberOfParts = geometry.STNumGeometries().Value;

            StringBuilder result = new StringBuilder();

            for (int i = 1; i <= numberOfParts; i++)
            {
                result.Append(SqlPolygonToPathMarkup(geometry.STGeometryN(i), decimals));
            }

            return result.ToString();
        }

        public static string ParseToPathMarkup(this SqlGeometry geometry, double decimals = double.NaN)
        {
            //if (geometry.IsNotValidOrEmpty())
            //{
            //    throw new NotImplementedException();
            //}



            OpenGisGeometryType geometryType = geometry.GetOpenGisType();

            switch (geometryType)
            {
                case OpenGisGeometryType.CircularString:
                case OpenGisGeometryType.CompoundCurve:
                case OpenGisGeometryType.CurvePolygon:
                case OpenGisGeometryType.GeometryCollection:
                default:
                    throw new NotImplementedException();

                case OpenGisGeometryType.Point:
                case OpenGisGeometryType.MultiPoint:
                    throw new NotImplementedException();

                case OpenGisGeometryType.LineString:
                    return SqlLineStringToPathMarkup(geometry, decimals);

                case OpenGisGeometryType.MultiLineString:
                    return SqlMultiLineStringToPathMarkup(geometry, decimals);

                case OpenGisGeometryType.Polygon:
                    return SqlPolygonToPathMarkup(geometry, decimals);

                case OpenGisGeometryType.MultiPolygon:
                    return SqlMultiPolygonToPathMarkup(geometry, decimals);
            }
        }

        #endregion

        #region GeoJson



        #endregion



        #region Model

        public static SqlGeometry AsSqlGeometry(this IRI.Ham.SpatialBase.Model.TileInfo tile)
        {
            return tile.WebMercatorExtent.AsSqlGeometry();
        }

        #endregion
    }
}