﻿// besmellahe rahmane rahim
// Allahomma ajjel le-valiyek al-faraj

using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Runtime.Serialization;
using IRI.Sta.Common.Primitives;

namespace IRI.Ket.ShapefileFormat.EsriType
{


    public struct EsriPolyline : IEsriSimplePoints
    {

        /// <summary>
        /// MinX, MinY, MaxX, MaxY
        /// </summary>
        private IRI.Sta.Common.Primitives.BoundingBox boundingBox;

        private EsriPoint[] points;

        public int[] Parts
        {
            get { return this.parts; }
        }

        public int NumberOfPoints
        {
            get { return this.Points == null ? 0 : this.points.Length; }
        }

        /// <summary>
        /// Points for All Parts
        /// </summary>
        public EsriPoint[] Points
        {
            get { return this.points; }
        }


        /// <summary>
        /// Index to First Point in Part
        /// </summary>
        private int[] parts;

        public int NumberOfParts
        {
            get { return this.parts.Length; }
        }

        internal EsriPolyline(IRI.Sta.Common.Primitives.BoundingBox boundingBox, int[] parts, EsriPoint[] points)
        {
            this.boundingBox = boundingBox;

            this.parts = parts;

            this.points = points;
        }

        public EsriPolyline(EsriPoint[] points)
            : this(points, new int[] { 0 })
        {

        }

        public EsriPolyline(EsriPoint[] points, int[] parts)
        {
            //this.boundingBox = new IRI.Sta.Common.Primitives.BoundingBox(xMin: MapStatistics.GetMinX(points),
            //                                 yMin: MapStatistics.GetMinY(points),
            //                                 xMax: MapStatistics.GetMaxX(points),
            //                                 yMax: MapStatistics.GetMaxY(points));
            this.boundingBox = IRI.Sta.Common.Primitives.BoundingBox.CalculateBoundingBox(points.Cast<IRI.Sta.Common.Primitives.IPoint>());

            this.points = points;

            this.parts = parts;
        }

        public EsriPolyline(EsriPoint[][] points)
        {
            this.points = points.Where(i => i.Length > 1).SelectMany(i => i).ToArray();

            this.parts = new int[points.Length];

            for (int i = 1; i < points.Length; i++)
            {
                parts[i] = points.Where((array, index) => index < i).Sum(array => array.Length);
            }

            var boundingBoxes = points.Select(i => IRI.Sta.Common.Primitives.BoundingBox.CalculateBoundingBox(i.Cast<IRI.Sta.Common.Primitives.IPoint>()));

            this.boundingBox = IRI.Sta.Common.Primitives.BoundingBox.GetMergedBoundingBox(boundingBoxes);

        }

        public byte[] WriteContentsToByte()
        {
            System.IO.MemoryStream result = new System.IO.MemoryStream();

            result.Write(System.BitConverter.GetBytes((int)EsriShapeType.EsriPolyLine), 0, ShapeConstants.IntegerSize);

            result.Write(Writer.ShpWriter.WriteBoundingBoxToByte(this), 0, 4 * ShapeConstants.DoubleSize);

            result.Write(System.BitConverter.GetBytes(this.NumberOfParts), 0, ShapeConstants.IntegerSize);

            result.Write(System.BitConverter.GetBytes(this.NumberOfPoints), 0, ShapeConstants.IntegerSize);

            foreach (int item in this.parts)
            {
                result.Write(System.BitConverter.GetBytes(item), 0, ShapeConstants.IntegerSize);
            }

            byte[] tempPoints = Writer.ShpWriter.WritePointsToByte(this.points);

            result.Write(tempPoints, 0, tempPoints.Length);

            return result.ToArray();
        }

        public int ContentLength
        {
            get { return 22 + 2 * NumberOfParts + 8 * NumberOfPoints; }
        }

        public EsriShapeType Type
        {
            get { return EsriShapeType.EsriPolyLine; }
        }

        public EsriPoint[] GetPart(int partNo)
        {
            return ShapeHelper.GetEsriPoints(this, Parts[partNo]);
        }


        public IRI.Sta.Common.Primitives.BoundingBox MinimumBoundingBox
        {
            get { return boundingBox; }
        }

        public string AsSqlServerWkt()
        {
            if (this.NumberOfParts > 1)
            {
                StringBuilder result = new StringBuilder("MULTILINESTRING(");

                for (int i = 0; i < NumberOfParts; i++)
                {
                    result.Append(string.Format("{0},", SqlServerWktMapFunctions.PointGroupElementToWkt(ShapeHelper.GetEsriPoints(this, this.Parts[i]))));
                }

                return result.Remove(result.Length - 1, 1).Append(")").ToString();
            }
            else
            {
                return string.Format("LINESTRING{0}", SqlServerWktMapFunctions.PointGroupElementToWkt(ShapeHelper.GetEsriPoints(this, this.Parts[0])));
            }

        }

        /// <summary>
        /// Changed but not tested. 93.03.21
        /// </summary>
        /// <returns></returns>
        public byte[] AsWkb()
        {
            List<byte> result = new List<byte>();

            if (this.Parts.Count() == 1)
            {
                result.AddRange(OgcWkbMapFunctions.ToWkbLineString(ShapeHelper.GetEsriPoints(this, 0)));
            }
            else
            {
                result.Add((byte)IRI.Standards.OGC.SFA.WkbByteOrder.WkbNdr);

                result.AddRange(BitConverter.GetBytes((uint)IRI.Standards.OGC.SFA.WkbGeometryType.MultiLineString));

                result.AddRange(BitConverter.GetBytes((uint)this.parts.Length));

                for (int i = 0; i < this.parts.Length; i++)
                {
                    result.AddRange(OgcWkbMapFunctions.ToWkbLineString(ShapeHelper.GetEsriPoints(this, this.Parts[i])));
                }
            }

            return result.ToArray();
        }

        public IRI.Ket.KmlFormat.Primitives.PlacemarkType AsPlacemark(Func<IRI.Sta.Common.Primitives.Point, IRI.Sta.Common.Primitives.Point> projectToGeodeticFunc = null, byte[] color = null)
        {
            return AsPlacemark(this, projectToGeodeticFunc, color);
        }

        /// <summary>
        /// Returs Kml representation of the point. Note: Point must be in Lat/Long System
        /// </summary>
        /// <returns></returns>
        static IRI.Ket.KmlFormat.Primitives.PlacemarkType AsPlacemark(EsriPolyline polyline, Func<IRI.Sta.Common.Primitives.Point, IRI.Sta.Common.Primitives.Point> projectToGeodeticFunc = null, byte[] color = null)
        {
            IRI.Ket.KmlFormat.Primitives.PlacemarkType placemark =
                new KmlFormat.Primitives.PlacemarkType();

            List<IRI.Ket.KmlFormat.Primitives.LineStringType> linestrings =
                new List<KmlFormat.Primitives.LineStringType>();

            IRI.Ket.KmlFormat.Primitives.MultiGeometryType multiGeometry =
                new KmlFormat.Primitives.MultiGeometryType();

            IEnumerable<string> coordinates;

            if (projectToGeodeticFunc != null)
            {
                coordinates = polyline.parts
                    .Select(i =>
                        string.Join(" ", ShapeHelper.GetEsriPoints(polyline, i)
                        .Select(j =>
                        {
                            var temp = projectToGeodeticFunc(new IRI.Sta.Common.Primitives.Point(j.X, j.Y));
                            return string.Format("{0},{1}", temp.X, temp.Y);
                        }).ToArray()));
            }
            else
            {
                coordinates = polyline.parts
                    .Select(i =>
                        string.Join(" ", ShapeHelper.GetEsriPoints(polyline, i)
                        .Select(j => string.Format("{0},{1}", j.X, j.Y))
                        .ToArray()));
            }

            foreach (string item in coordinates)
            {
                IRI.Ket.KmlFormat.Primitives.LineStringType linestring = new KmlFormat.Primitives.LineStringType();

                linestring.coordinates = item;

                linestrings.Add(linestring);
            }

            multiGeometry.AbstractGeometry = linestrings.ToArray();

            //placemark.AbstractFeatureObjectExtensionGroup = new KmlFormat.Primitives.AbstractObjectType[] { multiGeometry };
            placemark.AbstractGeometry = multiGeometry;
            //IRI.Ket.KmlFormat.Primitives.MultiGeometryType t = new KmlFormat.Primitives.MultiGeometryType();

            if (color == null)
            {
                return placemark;
            }

            IRI.Ket.KmlFormat.Primitives.StyleType style =
                new KmlFormat.Primitives.StyleType();

            KmlFormat.Primitives.LineStyleType lineStyle = new KmlFormat.Primitives.LineStyleType();
            lineStyle.color = color;

            style.LineStyle = lineStyle;
            placemark.Styles = new KmlFormat.Primitives.AbstractStyleSelectorType[] { style };

            return placemark;
        }

        public string AsKml(Func<IRI.Sta.Common.Primitives.Point, IRI.Sta.Common.Primitives.Point> projectToGeodeticFunc = null)
        {
            return OgcKmlMapFunctions.AsKml(this.AsPlacemark(projectToGeodeticFunc));
        }

        public IEsriShape Transform(Func<IPoint, IPoint> transform)
        {
            return new EsriPolyline(this.Points.Select(i => i.Transform(transform)).Cast<EsriPoint>().ToArray(), this.Parts);
        }

        public Geometry AsGeometry()
        {
            if (this.NumberOfParts > 1)
            {
                Geometry[] parts = new Geometry[this.NumberOfParts];

                for (int i = 0; i < NumberOfParts; i++)
                {
                    parts[i] = new Geometry(ShapeHelper.GetPoints(this, Parts[i]), GeometryType.LineString);
                }

                return new Geometry(parts, GeometryType.MultiLineString);
            }
            else
            {
                return new Geometry(ShapeHelper.GetPoints(this, Parts[0]), GeometryType.LineString);
            }
        }

    }

}
