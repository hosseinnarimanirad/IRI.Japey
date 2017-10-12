// besmellahe rahmane rahim
// Allahomma ajjel le-valiyek al-faraj

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Linq;
using System.Runtime.Serialization;
using IRI.Ham.SpatialBase;

namespace IRI.Ket.ShapefileFormat.EsriType
{


    public struct MultiPoint : IRI.Ket.ShapefileFormat.EsriType.ISimplePoints
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

        public MultiPoint(EsriPoint[] points)
        {
            this.boundingBox = IRI.Ham.SpatialBase.BoundingBox.CalculateBoundingBox(points.Cast<IRI.Ham.SpatialBase.IPoint>());

            this.points = points;
        }

        internal MultiPoint(IRI.Ham.SpatialBase.BoundingBox boundingBox, EsriPoint[] points)
        {
            this.boundingBox = boundingBox;

            this.points = points;
        }

        public byte[] WriteContentsToByte()
        {
            System.IO.MemoryStream result = new System.IO.MemoryStream();

            result.Write(System.BitConverter.GetBytes((int)ShapeType.MultiPoint), 0, ShapeConstants.IntegerSize);

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

        public ShapeType Type
        {
            get { return ShapeType.MultiPoint; }
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


        public IRI.Ham.SpatialBase.BoundingBox MinimumBoundingBox
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

        public IShape Transform(Func<IPoint, Ham.SpatialBase.IPoint> transform)
        {
            return new MultiPoint(this.Points.Select(i => i.Transform(transform)).Cast<EsriPoint>().ToArray());
        }
    }
}
