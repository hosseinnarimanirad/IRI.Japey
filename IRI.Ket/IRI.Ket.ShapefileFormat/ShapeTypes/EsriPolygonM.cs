﻿// besmellahe rahmane rahim
// Allahomma ajjel le-valiyek al-faraj

using IRI.Msh.Common.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;



namespace IRI.Ket.ShapefileFormat.EsriType
{


    public struct EsriPolygonM : IEsriPointsWithMeasure
    {

        private IRI.Msh.Common.Primitives.BoundingBox boundingBox;

        public int Srid { get; set; }

        private EsriPoint[] points;

        public int NumberOfPoints
        {
            get { return this.points.Length; }
        }

        /// <summary>
        /// Points for All Parts
        /// </summary>
        public EsriPoint[] Points
        {
            get { return this.points; }
        }

        public int[] Parts
        {
            get { return this.parts; }
        }

        /// <summary>
        /// Index to First Point in Part
        /// </summary>
        private int[] parts;

        public int NumberOfParts
        {
            get { return this.parts.Length; }
        }


        private double minMeasure, maxMeasure;

        private double[] measures;

        public double MinMeasure
        {
            get { return this.minMeasure; }
        }

        public double MaxMeasure
        {
            get { return this.maxMeasure; }
        }

        public double[] Measures
        {
            get { return this.measures; }
        }


        public EsriPolygonM(EsriPoint[] points, int[] parts, double[] measures)
        {
            if (points == null || points.Length != measures.Length)
            {
                throw new NotImplementedException();
            }

            if (points.Length == 0)
            {
                this.Srid = 0;
            }
            else
            {
                this.Srid = points.First().Srid;
            }

            this.boundingBox = IRI.Msh.Common.Primitives.BoundingBox.CalculateBoundingBox(points);

            this.parts = parts;

            this.points = points;

            this.measures = measures;

            if (measures?.Count() > 0)
            {
                this.minMeasure = measures.Min();

                this.maxMeasure = measures.Max();
            }
            else
            {
                this.minMeasure = ShapeConstants.NoDataValue;

                this.maxMeasure = ShapeConstants.NoDataValue;
            }

        }

        internal EsriPolygonM(IRI.Msh.Common.Primitives.BoundingBox boundingBox, int[] parts, EsriPoint[] points, double minMeasure, double maxMeasure, double[] measures)
        {
            if (points == null || points.Length != measures.Length)
            {
                throw new NotImplementedException();
            }

            if (points.Length == 0)
            {
                this.Srid = 0;
            }
            else
            {
                this.Srid = points.First().Srid;
            }

            this.boundingBox = boundingBox;

            this.parts = parts;

            this.points = points;

            this.minMeasure = minMeasure;

            this.maxMeasure = maxMeasure;

            this.measures = measures;
        }

        public byte[] WriteContentsToByte()
        {
            System.IO.MemoryStream result = new System.IO.MemoryStream();

            result.Write(System.BitConverter.GetBytes((int)EsriShapeType.EsriPolygonM), 0, ShapeConstants.IntegerSize);

            result.Write(Writer.ShpWriter.WriteBoundingBoxToByte(this), 0, 4 * ShapeConstants.DoubleSize);

            result.Write(System.BitConverter.GetBytes(this.NumberOfParts), 0, ShapeConstants.IntegerSize);

            result.Write(System.BitConverter.GetBytes(this.NumberOfPoints), 0, ShapeConstants.IntegerSize);

            foreach (int item in this.parts)
            {
                result.Write(System.BitConverter.GetBytes(item), 0, ShapeConstants.IntegerSize);
            }


            byte[] tempPoints = Writer.ShpWriter.WritePointsToByte(this.points);

            result.Write(tempPoints, 0, tempPoints.Length);


            byte[] tempMeasures = Writer.ShpWriter.WriteAdditionalData(this.MinMeasure, this.MaxMeasure, this.Measures);

            result.Write(tempMeasures, 0, tempMeasures.Length);


            return result.ToArray();
        }

        public int ContentLength
        {
            get { return 22 + 2 * NumberOfParts + 8 * NumberOfPoints + 8 + 4 * NumberOfPoints; }
        }

        public EsriShapeType Type
        {
            get { return EsriShapeType.EsriPolygonM; }
        }

        public EsriPoint[] GetPart(int partNo)
        {
            return ShapeHelper.GetEsriPoints(this, Parts[partNo]);
        }


        public IRI.Msh.Common.Primitives.BoundingBox MinimumBoundingBox
        {
            get { return boundingBox; }
        }

        public string AsSqlServerWkt()
        {
            StringBuilder result = new StringBuilder("POLYGON(");

            for (int i = 0; i < NumberOfParts; i++)
            {
                result.Append(
                    string.Format("{0},",
                    SqlServerWktMapFunctions.PointMGroupElementToWkt(
                        ShapeHelper.GetEsriPoints(this, this.Parts[i]),
                        ShapeHelper.GetMeasures(this, this.Parts[i]))));
            }

            return result.Remove(result.Length - 1, 1).Append(")").ToString();
        }

        //Error Prone: not checking for multipolygon cases
        public byte[] AsWkb()
        {
            List<byte> result = new List<byte>();

            result.Add((byte)IRI.Standards.OGC.SFA.WkbByteOrder.WkbNdr);

            result.AddRange(BitConverter.GetBytes((uint)IRI.Standards.OGC.SFA.WkbGeometryType.PolygonM));

            result.AddRange(BitConverter.GetBytes((uint)this.parts.Length));

            for (int i = 0; i < this.parts.Length; i++)
            {
                result.AddRange(
                    OgcWkbMapFunctions.ToWkbLinearRingM(
                    ShapeHelper.GetEsriPoints(this, this.Parts[i]),
                    ShapeHelper.GetMeasures(this, this.Parts[i])));
            }

            return result.ToArray();
        }

        /// <summary>
        /// Returs Kml representation of the point. Note: Point must be in Lat/Long System
        /// </summary>
        /// <returns></returns>
        public IRI.Ket.KmlFormat.Primitives.PlacemarkType AsPlacemark(Func<IRI.Msh.Common.Primitives.Point, IRI.Msh.Common.Primitives.Point> projectFunc = null, byte[] color = null)
        {
            throw new NotImplementedException();
        }

        public string AsKml(Func<IRI.Msh.Common.Primitives.Point, IRI.Msh.Common.Primitives.Point> projectToGeodeticFunc = null)
        {
            return OgcKmlMapFunctions.AsKml(this.AsPlacemark(projectToGeodeticFunc));
        }

        public IEsriShape Transform(Func<IPoint, IPoint> transform, int newSrid)
        {
            return new EsriPolygonM(this.Points.Select(i => i.Transform(transform, newSrid)).Cast<EsriPoint>().ToArray(), this.Parts, this.Measures);
        }

        //always returns polygon not multi polygon
        public Geometry<Point> AsGeometry()
        {
            if (this.NumberOfParts > 1)
            {
                //Geometry[] parts = new Geometry[this.NumberOfParts];
                List<Geometry<Point>> parts = new List<Geometry<Point>>(this.NumberOfParts);

                for (int i = 0; i < NumberOfParts; i++)
                {
                    //parts[i] = new Geometry(ShapeHelper.GetPoints(this, Parts[i]), GeometryType.LineString, Srid);
                    parts.Add(new Geometry<Point>(ShapeHelper.GetPoints(this, Parts[i]), GeometryType.LineString, Srid));
                }

                return new Geometry<Point>(parts, GeometryType.Polygon, Srid);
            }
            else if (this.NumberOfParts == 1)
            {
                return new Geometry<Point>(new List<Geometry<Point>> { new Geometry<Point>(ShapeHelper.GetPoints(this, Parts[0]), GeometryType.LineString, Srid) }, GeometryType.Polygon, Srid);
            }
            else
            {
                return Geometry<Point>.CreateEmpty(GeometryType.Polygon, Srid);
            }
        }

        public bool IsNullOrEmpty()
        {
            return Points == null || Points.Length < 1;
        }

    }
}
