// besmellahe rahmane rahim
// Allahomma ajjel le-valiyek al-faraj

using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Runtime.Serialization;
using IRI.Msh.Common.Primitives;

namespace IRI.Ket.ShapefileFormat.EsriType
{


    public struct EsriMultiPointZ : IRI.Ket.ShapefileFormat.EsriType.IEsriPointsWithZ
    {

        /// <summary>
        /// MinX, MinY, MaxX, MaxY
        /// </summary>
        private IRI.Msh.Common.Primitives.BoundingBox boundingBox;

        public int Srid { get; private set; }

        private EsriPoint[] points;

        public int NumberOfPoints
        {
            get { return this.points.Length; }
        }

        public EsriPoint[] Points
        {
            get { return this.points; }
        }

        public int[] Parts
        {
            get { return new int[] { 0 }; }
        }

        public int NumberOfParts
        {
            get { return this.Parts.Length; }
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

        public EsriMultiPointZ(EsriPoint[] points, double[] zValues, double[] measures, int srid)
        {
            if (points.Length != zValues.Length || points.Length != measures.Length)
            {
                throw new NotImplementedException();
            }

            this.boundingBox = IRI.Msh.Common.Primitives.BoundingBox.CalculateBoundingBox(points.Cast<IRI.Msh.Common.Primitives.IPoint>());

            this.Srid = srid;

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

        internal EsriMultiPointZ(IRI.Msh.Common.Primitives.BoundingBox boundingBox,
                                EsriPoint[] points,
                                double minZ,
                                double maxZ,
                                double[] zValues,
                                double minMeasure,
                                double maxMeasure,
                                double[] measures, 
                                int srid)
        {
            this.boundingBox = boundingBox;

            this.Srid = srid;

            this.points = points;

            this.minZ = minZ;

            this.maxZ = maxZ;

            this.zValues = zValues;

            this.minMeasure = minMeasure;

            this.maxMeasure = maxMeasure;

            this.measures = measures;
        }

        public EsriMultiPointZ(EsriPointZ[] points, int srid)
        {
            //this.boundingBox = new IRI.Msh.Common.Primitives.BoundingBox(xMin: MapStatistics.GetMinX(points),
            //                                    yMin: MapStatistics.GetMinY(points),
            //                                    xMax: MapStatistics.GetMaxX(points),
            //                                    yMax: MapStatistics.GetMaxY(points));
            this.boundingBox = IRI.Msh.Common.Primitives.BoundingBox.CalculateBoundingBox(points.Cast<IRI.Msh.Common.Primitives.IPoint>());

            this.Srid = srid;

            this.points = new EsriPoint[points.Length];

            this.measures = new double[points.Length];

            this.zValues = new double[points.Length];

            this.minMeasure = points[0].Measure;

            this.maxMeasure = points[0].Measure;

            this.minZ = points[0].Z;

            this.maxZ = points[0].Z;

            for (int i = 0; i < points.Length; i++)
            {
                this.points[i] = new EsriPoint(points[i].X, points[i].Y);

                this.measures[i] = points[i].Measure;

                this.zValues[i] = points[i].Z;

                if (this.minMeasure > points[i].Measure)
                {
                    this.minMeasure = points[i].Measure;
                }

                if (this.maxMeasure < points[i].Measure)
                {
                    this.maxMeasure = points[i].Measure;
                }

                if (this.minZ > points[i].Z)
                {
                    this.minZ = points[i].Z;
                }

                if (this.minZ < points[i].Z)
                {
                    this.minZ = points[i].Z;
                }
            }
        }

        #region IShape Members

        public byte[] WriteContentsToByte()
        {
            System.IO.MemoryStream result = new System.IO.MemoryStream();

            result.Write(System.BitConverter.GetBytes((int)EsriShapeType.EsriMultiPointZ), 0, ShapeConstants.IntegerSize);

            result.Write(Writer.ShpWriter.WriteBoundingBoxToByte(this), 0, 4 * ShapeConstants.DoubleSize);

            result.Write(System.BitConverter.GetBytes(this.NumberOfPoints), 0, ShapeConstants.IntegerSize);

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
            get { return 20 + 8 * NumberOfPoints + 2 * (8 + 4 * NumberOfPoints); }
        }

        public EsriShapeType Type
        {
            get { return EsriShapeType.EsriMultiPointZ; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="partNo">this parameter will be ignored</param>
        /// <returns></returns>
        public EsriPoint[] GetPart(int partNo)
        {
            return this.Points;
        }


        public IRI.Msh.Common.Primitives.BoundingBox MinimumBoundingBox
        {
            get { return boundingBox; }
        }

        public string AsSqlServerWkt()
        {
            //StringBuilder result = new StringBuilder("MULTIPOINT(");

            //for (int i = 0; i < NumberOfPoints - 1; i++)
            //{
            //    result.Append(string.Format(" {0} {1} {2} {3},", this.Points[i].X, this.Points[i].Y, this.ZValues[i], this.Measures[i] == ShapeConstants.NoDataValue ? "NULL" : this.Measures[i].ToString()));
            //}

            //result.Append(string.Format(" {0} {1} {2} {3})",
            //                                this.Points[NumberOfPoints - 1].X,
            //                                this.Points[NumberOfPoints - 1].Y,
            //                                this.ZValues[NumberOfPoints - 1],
            //                                this.Measures[NumberOfPoints - 1] == ShapeConstants.NoDataValue ? "NULL" : this.Measures[NumberOfPoints - 1].ToString()));

            //return result.ToString();
            return string.Format("MULTIPOINT{0}", SqlServerWktMapFunctions.PointZGroupElementToWkt(this.Points, this.ZValues, this.Measures));
        }

        public byte[] AsWkb()
        {
            //byte[] result = new byte[1 + 4 + 4 + 16 * this.NumberOfPoints];
            List<byte> result = new List<byte>();

            result.Add((byte)IRI.Standards.OGC.SFA.WkbByteOrder.WkbNdr);

            result.AddRange(BitConverter.GetBytes((int)IRI.Standards.OGC.SFA.WkbGeometryType.MultiPoint));

            result.AddRange(BitConverter.GetBytes((UInt32)this.NumberOfPoints));

            for (int i = 0; i < this.NumberOfPoints; i++)
            {
                result.AddRange(OgcWkbMapFunctions.ToWkbPoint(this.points[i]));
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
            return new EsriMultiPointZ(this.Points.Select(i => i.Transform(transform, newSrid)).Cast<EsriPoint>().ToArray(), this.ZValues, this.Measures, newSrid);
        }

        public Geometry AsGeometry()
        {
            return new Geometry(points.Select(p => new Point(p.X, p.Y)).ToArray(), GeometryType.MultiPoint, Srid);
        }

        #endregion
    }
}
