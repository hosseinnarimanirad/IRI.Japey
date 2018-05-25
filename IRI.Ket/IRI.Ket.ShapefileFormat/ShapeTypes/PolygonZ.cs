// besmellahe rahmane rahim
// Allahomma ajjel le-valiyek al-faraj

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;


namespace IRI.Ket.ShapefileFormat.EsriType
{


    public struct EsriPolygonZ : IEsriPointsWithZ
    {

        /// <summary>
        /// MinX, MinY, MaxX, MaxY
        /// </summary>
        private IRI.Ham.SpatialBase.BoundingBox boundingBox;

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


        private double minZ, maxZ;

        private double[] zValues;

        public double MinZ
        {
            get { return this.minZ; }
        }

        public double MaxZ
        {
            get { return this.maxZ; }
        }

        public double[] ZValues
        {
            get { return this.zValues; }
        }

        public EsriPolygonZ(EsriPoint[] points, int[] parts, double[] zValues, double[] measures)
        {
            if (points.Length != zValues.Length || points.Length != measures.Length)
            {
                throw new NotImplementedException();
            }

            this.boundingBox = IRI.Ham.SpatialBase.BoundingBox.CalculateBoundingBox(points.Cast<IRI.Ham.SpatialBase.IPoint>());

            this.parts = parts;

            this.points = points;

            this.measures = measures;
             
            this.zValues = zValues;
             
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

            if (zValues?.Count() > 0)
            {
                this.minZ = zValues.Min();

                this.maxZ = zValues.Max();
            }
            else
            {
                this.minZ = ShapeConstants.NoDataValue;

                this.maxZ = ShapeConstants.NoDataValue;
            }
        }

        internal EsriPolygonZ(IRI.Ham.SpatialBase.BoundingBox boundingBox,
                            int[] parts,
                            EsriPoint[] points,
                            double minZ,
                            double maxZ,
                            double[] zValues,
                            double minMeasure,
                            double maxMeasure,
                            double[] measures)
        {
            this.boundingBox = boundingBox;

            this.parts = parts;

            this.points = points;

            this.minZ = minZ;

            this.maxZ = maxZ;

            this.zValues = zValues;

            this.minMeasure = minMeasure;

            this.maxMeasure = maxMeasure;

            this.measures = measures;
        }

        public byte[] WriteContentsToByte()
        {
            System.IO.MemoryStream result = new System.IO.MemoryStream();

            result.Write(System.BitConverter.GetBytes((int)EsriShapeType.EsriPolygonZ), 0, ShapeConstants.IntegerSize);

            result.Write(Writer.ShpWriter.WriteBoundingBoxToByte(this), 0, 4 * ShapeConstants.DoubleSize);

            result.Write(System.BitConverter.GetBytes(this.NumberOfParts), 0, ShapeConstants.IntegerSize);

            result.Write(System.BitConverter.GetBytes(this.NumberOfPoints), 0, ShapeConstants.IntegerSize);

            foreach (int item in this.parts)
            {
                result.Write(System.BitConverter.GetBytes(item), 0, ShapeConstants.IntegerSize);
            }


            byte[] tempPoints = Writer.ShpWriter.WritePointsToByte(this.points);

            result.Write(tempPoints, 0, tempPoints.Length);


            byte[] tempZ = Writer.ShpWriter.WriteAdditionalData(this.MinZ, this.MaxZ, this.ZValues);

            result.Write(tempZ, 0, tempZ.Length);


            byte[] tempMeasures = Writer.ShpWriter.WriteAdditionalData(this.MinMeasure, this.MaxMeasure, this.Measures);

            result.Write(tempMeasures, 0, tempMeasures.Length);

            return result.ToArray();
        }

        public int ContentLength
        {
            get { return 22 + 2 * NumberOfParts + 8 * NumberOfPoints + 2 * (8 + 4 * NumberOfPoints); }
        }

        public EsriShapeType Type
        {
            get { return EsriShapeType.EsriPolygonZ; }
        }

        public EsriPoint[] GetPart(int partNo)
        {
            return ShapeHelper.GetPoints(this, Parts[partNo]);
        }


        public IRI.Ham.SpatialBase.BoundingBox MinimumBoundingBox
        {
            get { return boundingBox; }
        }

        public string AsSqlServerWkt()
        {
            StringBuilder result = new StringBuilder("POLYGON(");

            for (int i = 0; i < NumberOfParts; i++)
            {
                result.Append(string.Format("{0},",
                    SqlServerWktMapFunctions.PointZGroupElementToWkt(
                        ShapeHelper.GetPoints(this, this.Parts[i]),
                        ShapeHelper.GetZValues(this, this.Parts[i]),
                        ShapeHelper.GetMeasures(this, this.Parts[i]))));
            }

            return result.Remove(result.Length - 1, 1).Append(")").ToString();
        }

        //Error Prone: not checking for multipolygon cases
        public byte[] AsWkb()
        {
            List<byte> result = new List<byte>();

            result.Add((byte)IRI.Standards.OGC.SFA.WkbByteOrder.WkbNdr);

            result.AddRange(BitConverter.GetBytes((uint)IRI.Standards.OGC.SFA.WkbGeometryType.PolygonZM));

            result.AddRange(BitConverter.GetBytes((uint)this.parts.Length));

            for (int i = 0; i < this.parts.Length; i++)
            {
                result.AddRange(OgcWkbMapFunctions.ToWkbLinearRingZM(
                        ShapeHelper.GetPoints(this, this.Parts[i]),
                        ShapeHelper.GetZValues(this, this.Parts[i]),
                        ShapeHelper.GetMeasures(this, this.Parts[i])));
            }

            return result.ToArray();
        }

        /// <summary>
        /// Returs Kml representation of the point. Note: Point must be in Lat/Long System
        /// </summary>
        /// <returns></returns>
        public IRI.Ket.KmlFormat.Primitives.PlacemarkType AsPlacemark(Func<IRI.Ham.SpatialBase.Point, IRI.Ham.SpatialBase.Point> projectFunc = null, byte[] color = null)
        {
            throw new NotImplementedException();
        }

        public string AsKml(Func<IRI.Ham.SpatialBase.Point, IRI.Ham.SpatialBase.Point> projectToGeodeticFunc = null)
        {
            return OgcKmlMapFunctions.AsKml(this.AsPlacemark(projectToGeodeticFunc));
        }

        public IEsriShape Transform(Func<Ham.SpatialBase.IPoint, Ham.SpatialBase.IPoint> transform)
        {
            return new EsriPolygonZ(this.Points.Select(i => i.Transform(transform)).Cast<EsriPoint>().ToArray(), this.Parts, this.ZValues, this.Measures);
        }
    }
}
