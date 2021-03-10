// besmellahe rahmane rahim
// Allahomma ajjel le-valiyek al-faraj

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Linq;
using System.Runtime.Serialization;
using IRI.Msh.Common.Primitives;

namespace IRI.Ket.ShapefileFormat.EsriType
{
    public struct EsriMultiPoint : IRI.Ket.ShapefileFormat.EsriType.IEsriSimplePoints
    {
        /// <summary>
        /// MinX, MinY, MaxX, MaxY
        /// </summary>
        private IRI.Msh.Common.Primitives.BoundingBox boundingBox;

        public int Srid { get; set; }

        private EsriPoint[] points;

        public int NumberOfPoints
        {
            get { return this.points.Length; }
        }

        public int NumberOfParts
        {
            get { return this.Parts.Length; }
        }

        public EsriPoint[] Points
        {
            get { return this.points; }
        }

        public int[] Parts
        {
            get { return new int[] { 0 }; }
        }

        public EsriMultiPoint(EsriPoint[] points)
        {
            if (points == null)
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

            this.boundingBox = IRI.Msh.Common.Primitives.BoundingBox.CalculateBoundingBox(points/*.Cast<IRI.Msh.Common.Primitives.IPoint>()*/);

            this.points = points;

        }

        internal EsriMultiPoint(IRI.Msh.Common.Primitives.BoundingBox boundingBox, EsriPoint[] points)
        {
            if (points == null || points.Length < 1)
            {
                throw new NotImplementedException();
            }

            this.boundingBox = boundingBox;

            this.points = points;

            this.Srid = points.First().Srid;
        }

        public byte[] WriteContentsToByte()
        {
            System.IO.MemoryStream result = new System.IO.MemoryStream();

            result.Write(System.BitConverter.GetBytes((int)EsriShapeType.EsriMultiPoint), 0, ShapeConstants.IntegerSize);

            result.Write(Writer.ShpWriter.WriteBoundingBoxToByte(this), 0, 4 * ShapeConstants.DoubleSize);

            result.Write(System.BitConverter.GetBytes(this.NumberOfPoints), 0, ShapeConstants.IntegerSize);

            byte[] tempPoints = Writer.ShpWriter.WritePointsToByte(this.points);

            result.Write(tempPoints, 0, tempPoints.Length);

            return result.ToArray();
        }

        public int ContentLength
        {
            get { return 20 + 8 * NumberOfPoints; }
        }

        public EsriShapeType Type
        {
            get { return EsriShapeType.EsriMultiPoint; }
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
            return string.Format(
                "MULTIPOINT({0})",
                string.Join(",", this.points.Select(i => string.Format("({0})", SqlServerWktMapFunctions.SinglePointElementToWkt(i))).ToArray()));
        }

        public byte[] AsWkb()
        {
            return OgcWkbMapFunctions.ToWkbMultiPoint(this.points, IRI.Standards.OGC.SFA.WkbGeometryType.MultiPoint);
            //byte[] result = new byte[1 + 4 + 4 + 16 * this.NumberOfPoints];
            //List<byte> result = new List<byte>();

            //result.Add((byte)IRI.Standards.OGC.SFA.WkbByteOrder.WkbNdr);

            //result.AddRange(BitConverter.GetBytes((int)IRI.Standards.OGC.SFA.WkbGeometryType.MultiPoint));

            //result.AddRange(BitConverter.GetBytes((UInt32)this.NumberOfPoints));

            //for (int i = 0; i < this.NumberOfPoints; i++)
            //{
            //    result.AddRange(OgcWkbMapFunctions.ToWkbPoint(this.points[i]));
            //}

            //return result.ToArray();
        }

        /// <summary>
        /// Returns Kml representation of the point. Note: Point must be in Lat/Long System
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

        public IEsriShape Transform(Func<IPoint, IPoint> transform, int newSrid) /*where TPoint : IPoint, new()*/
        {
            return new EsriMultiPoint(this.Points.Select(i => i.Transform(transform, newSrid)).Cast<EsriPoint>().ToArray());
        }

        public Geometry<Point> AsGeometry()
        {
            return new Geometry<Point>(points.Select(p => p.AsGeometry()).ToList(), GeometryType.MultiPoint, Srid);
        }

        public bool IsNullOrEmpty()
        {
            return Points == null || Points.Length < 1;
        }
    }
}
