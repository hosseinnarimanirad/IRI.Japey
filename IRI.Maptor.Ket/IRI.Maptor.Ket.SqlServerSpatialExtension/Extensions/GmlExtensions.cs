using Microsoft.SqlServer.Types;
using System.Data.SqlTypes;
using System.Xml;
using System.Xml.Linq;

namespace IRI.Maptor.Extensions;

public static class GmlExtensions
{ 
    public static string AsGml3(this SqlGeometry geometry, bool writeSrid = false)
    {
        var gml = geometry?.AsGml();

        if (gml == null || gml.IsNull)
        {
            return string.Empty;
        }

        var doc = XDocument.Parse(geometry.AsGml().Value);

        doc.Root.SetAttributeValue(XNamespace.Xmlns + "gml", "http://www.opengis.net/gml");
        doc.Root.SetAttributeValue("xmlns", null);

        if (writeSrid)
        {
            doc.Root.SetAttributeValue("srsName", $"http://www.opengis.net/gml/srs/epsg.xml#{geometry.STSrid.Value}");
        }

        //doc.Declaration = null;

        return doc.ToString(SaveOptions.OmitDuplicateNamespaces);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="gmlString">should not contain srsName attribute</param>
    /// <param name="srid"></param>
    /// <returns></returns>
    public static SqlGeometry ParseGML3(string gmlString, int srid)
    {
        var reader = XmlReader.Create(new StringReader(gmlString));

        SqlXml sqlXml = new SqlXml(reader);

        return SqlGeometry.GeomFromGml(sqlXml, srid);
    }

    //        #region GML

    //        private static double?[] GetEsriJsonObjectPoint(SqlGeometry point)
    //        {
    //            if (point.IsNullOrEmpty())
    //                return new double?[0];

    //            return new double?[] { point.STX.Value, point.STY.Value };
    //        }

    //        private static double?[][] GetLineStringOrRing(SqlGeometry lineStringOrRing)
    //        {
    //            if (lineStringOrRing.IsNullOrEmpty())
    //                return new double?[0][];

    //            int numberOfPoints = lineStringOrRing.STNumPoints().Value;

    //            double?[][] result = new double?[numberOfPoints][];

    //            for (int i = 1; i <= numberOfPoints; i++)
    //            {
    //                result[i - 1] = GetEsriJsonObjectPoint(lineStringOrRing.STPointN(i));
    //            }

    //            return result;
    //        }

    //        //Not supportig Z and M Values
    //        private static EsriJsonGeometry SqlPointToEsriJsonPoint(this SqlGeometry geometry)
    //        {
    //            //This check is required
    //            if (geometry.IsNullOrEmpty())
    //                return new EsriJsonGeometry()
    //                {
    //                    type = EsriJsonGeometryType.point,
    //                };

    //            return new EsriJsonGeometry()
    //            {
    //                x = geometry.STX.Value,
    //                y = geometry.STY.Value,
    //                type = EsriJsonGeometryType.point,
    //                spatialReference = new EsriJsonSpatialreference() { wkid = geometry.GetSrid() }
    //            };
    //        }

    //        //Not supportig Z and M Values
    //        private static EsriJsonGeometry SqlMultiPointToEsriJsonMultiPoint(this SqlGeometry geometry)
    //        {
    //            //This check is required
    //            if (geometry.IsNullOrEmpty())
    //                return new EsriJsonGeometry()
    //                {
    //                    type = EsriJsonGeometryType.multipoint,
    //                    points = new double?[0][],
    //                };

    //            var numberOfGeometries = geometry.STNumGeometries().Value;

    //            double?[][] points = new double?[numberOfGeometries][];

    //            for (int i = 1; i <= numberOfGeometries; i++)
    //            {
    //                points[i - 1] = GetEsriJsonObjectPoint(geometry.STGeometryN(i));
    //            }

    //            return new EsriJsonGeometry()
    //            {
    //                points = points,
    //                type = EsriJsonGeometryType.multipoint,
    //                spatialReference = new EsriJsonSpatialreference() { wkid = geometry.GetSrid() }
    //            };
    //        }

    //        //Not supportig Z and M Values
    //        private static EsriJsonGeometry SqlLineStringToEsriJsonPolyline(this SqlGeometry geometry)
    //        {
    //            //This check is required
    //            if (geometry.IsNullOrEmpty())
    //                return new EsriJsonGeometry()
    //                {
    //                    type = EsriJsonGeometryType.polyline,
    //                    paths = new double?[0][][],
    //                };

    //            double?[][][] paths = new double?[1][][] { GetLineStringOrRing(geometry) };

    //            return new EsriJsonGeometry()
    //            {
    //                paths = paths,
    //                type = EsriJsonGeometryType.polyline,
    //                spatialReference = new EsriJsonSpatialreference() { wkid = geometry.GetSrid() }
    //            };
    //        }

    //        //Not supportig Z and M Values
    //        private static EsriJsonGeometry SqlMultiLineStringToEsriJsonPolyline(this SqlGeometry geometry)
    //        {
    //            //This check is required
    //            if (geometry.IsNullOrEmpty())
    //                return new EsriJsonGeometry()
    //                {
    //                    type = EsriJsonGeometryType.polyline,
    //                    paths = new double?[0][][],
    //                };

    //            int numberOfParts = geometry.STNumGeometries().Value;

    //            double?[][][] result = new double?[numberOfParts][][];

    //            for (int i = 1; i <= numberOfParts; i++)
    //            {
    //                result[i - 1] = GetLineStringOrRing(geometry.STGeometryN(i));
    //            }

    //            return new EsriJsonGeometry()
    //            {
    //                paths = result,
    //                type = EsriJsonGeometryType.polyline,
    //                spatialReference = new EsriJsonSpatialreference() { wkid = geometry.GetSrid() }
    //            };
    //        }

    //        //Not supportig Z and M Values
    //        private static EsriJsonGeometry SqlPolygonToEsriJsonPolygon(this SqlGeometry geometry)
    //        {
    //            //This check is required
    //            if (geometry.IsNullOrEmpty())
    //                return new EsriJsonGeometry()
    //                {
    //                    type = EsriJsonGeometryType.polygon,
    //                    rings = new double?[0][][],
    //                };

    //            double?[][][] rings = new double?[1][][] { GetLineStringOrRing(geometry) };

    //            return new EsriJsonGeometry()
    //            {
    //                paths = rings,
    //                type = EsriJsonGeometryType.polygon,
    //                spatialReference = new EsriJsonSpatialreference() { wkid = geometry.GetSrid() }
    //            };
    //        }

    //        //Not supportig Z and M Values
    //        private static EsriJsonGeometry SqlMultiPolygonToEsriJsonMultiPolygon(this SqlGeometry geometry)
    //        {
    //            //This check is required
    //            if (geometry.IsNullOrEmpty())
    //                return new EsriJsonGeometry()
    //                {
    //                    type = EsriJsonGeometryType.polygon,
    //                    rings = new double?[0][][],
    //                };

    //            int numberOfParts = geometry.STNumGeometries().Value;

    //            double?[][][] rings = new double?[numberOfParts][][];

    //            for (int i = 1; i <= numberOfParts; i++)
    //            {
    //                rings[i - 1] = GetLineStringOrRing(geometry.STGeometryN(i));
    //            }

    //            return new EsriJsonGeometry()
    //            {
    //                paths = rings,
    //                type = EsriJsonGeometryType.polygon,
    //                spatialReference = new EsriJsonSpatialreference() { wkid = geometry.GetSrid() }
    //            };
    //        }

    //        public static IRI.Maptor.Sta.Ogc.GML.v212.AbstractGeometryType ParseToEsriJsonGeometry(this SqlGeometry geometry)
    //        { 
    //            OpenGisGeometryType geometryType = geometry.GetOpenGisType();

    //            switch (geometryType)
    //            {
    //                case OpenGisGeometryType.CircularString:
    //                case OpenGisGeometryType.CompoundCurve:
    //                case OpenGisGeometryType.CurvePolygon:
    //                case OpenGisGeometryType.GeometryCollection:
    //                default:
    //                    throw new NotImplementedException();

    //                case OpenGisGeometryType.Point:
    //                    return geometry.SqlPointToEsriJsonPoint();

    //                case OpenGisGeometryType.MultiPoint:
    //                    return SqlMultiPointToEsriJsonMultiPoint(geometry);

    //                case OpenGisGeometryType.LineString:
    //                    return SqlLineStringToEsriJsonPolyline(geometry);

    //                case OpenGisGeometryType.MultiLineString:
    //                    return SqlMultiLineStringToEsriJsonPolyline(geometry);

    //                case OpenGisGeometryType.Polygon:
    //                    return SqlPolygonToEsriJsonPolygon(geometry);

    //                case OpenGisGeometryType.MultiPolygon:
    //                    return SqlMultiPolygonToEsriJsonMultiPolygon(geometry);
    //            }
    //        }

    //        #endregion
}
