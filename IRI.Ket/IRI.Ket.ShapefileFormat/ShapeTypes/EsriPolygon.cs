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

    //A polygon consists of one or more rings.A ring is a connected sequence of four or more
    //points that form a closed, non-self-intersecting loop. A polygon may contain multiple
    //outer rings.The order of vertices or orientation for a ring indicates which side of the ring
    //is the interior of the polygon.The neighborhood to the right of an observer walking along
    //the ring in vertex order is the neighborhood inside the polygon.Vertices of rings defining
    //holes in polygons are in a counterclockwise direction.Vertices for a single, ringed
    //polygon are, therefore, always in clockwise order. The rings of a polygon are referred to
    //as its parts.
    public struct EsriPolygon : IEsriSimplePoints
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

        internal EsriPolygon(BoundingBox boundingBox, int[] parts, EsriPoint[] points)
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

            this.boundingBox = boundingBox;
              
            this.parts = parts;

            this.points = points;
        }

        public EsriPolygon(EsriPoint[] points)
            : this(points, new int[] { 0 })
        {

        }

        public EsriPolygon(EsriPoint[] points, int[] parts)
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

            this.boundingBox = IRI.Msh.Common.Primitives.BoundingBox.CalculateBoundingBox(points.Cast<IRI.Msh.Common.Primitives.IPoint>());
             
            this.parts = parts;

            this.points = points;
        }

        public EsriPolygon(EsriPoint[][] points)
        {
            if (points == null)
            {
                throw new NotImplementedException();
            }

            this.points = points.Where(i => i.Length > 3).SelectMany(i => i).ToArray();

            if (this.points.Length == 0)
            {
                this.Srid = 0;
            }
            else
            {
                this.Srid = this.points.First().Srid;
            }
             
            this.parts = new int[points.Length];

            for (int i = 1; i < points.Length; i++)
            {
                parts[i] = points.Where((array, index) => index < i).Sum(array => array.Length);
            }

            var boundingBoxes = points.Select(i => BoundingBox.CalculateBoundingBox(i.Cast<IRI.Msh.Common.Primitives.IPoint>()));

            this.boundingBox = BoundingBox.GetMergedBoundingBox(boundingBoxes);

            //this.boundingBox = IRI.Msh.Common.Infrastructure.CalculateBoundingBox(this.points.Cast<IRI.Msh.Common.Primitives.IPoint>());
        }



        public byte[] WriteContentsToByte()
        {
            System.IO.MemoryStream result = new System.IO.MemoryStream();

            result.Write(System.BitConverter.GetBytes((int)EsriShapeType.EsriPolygon), 0, ShapeConstants.IntegerSize);

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
            get { return EsriShapeType.EsriPolygon; }
        }

        public EsriPoint[] GetPart(int partNo)
        {
            return ShapeHelper.GetEsriPoints(this, Parts[partNo]);
        }


        public IRI.Msh.Common.Primitives.BoundingBox MinimumBoundingBox
        {
            get { return boundingBox; }
        }

        /// <summary>
        /// Note: the returned geometry may not be valid in the case of multigeometies
        /// </summary>
        /// <returns>Note: the returned geometry may not be valid in the case of multigeometies</returns>
        public string AsSqlServerWkt()
        {
            StringBuilder result = new StringBuilder("POLYGON(");

            for (int i = 0; i < NumberOfParts; i++)
            {
                result.Append(string.Format("{0},", SqlServerWktMapFunctions.PointGroupElementToWkt(ShapeHelper.GetEsriPoints(this, this.Parts[i]))));
            }

            return result.Remove(result.Length - 1, 1).Append(")").ToString();
        }

        //Error Prone: not checking for multipolygon cases
        public byte[] AsWkb()
        {
            List<byte> result = new List<byte>();

            result.Add((byte)IRI.Standards.OGC.SFA.WkbByteOrder.WkbNdr);

            result.AddRange(BitConverter.GetBytes((uint)IRI.Standards.OGC.SFA.WkbGeometryType.Polygon));

            result.AddRange(BitConverter.GetBytes((uint)this.parts.Length));

            for (int i = 0; i < this.parts.Length; i++)
            {
                result.AddRange(OgcWkbMapFunctions.ToWkbLinearRing(ShapeHelper.GetEsriPoints(this, this.Parts[i])));
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
            return new EsriPolygon(this.Points.Select(i => i.Transform(transform, newSrid)).Cast<EsriPoint>().ToArray(), this.Parts);
        }


        //always returns polygon not multi polygon
        public Geometry AsGeometry()
        {
            if (this.NumberOfParts > 1)
            {
                Geometry[] parts = new Geometry[this.NumberOfParts];

                for (int i = 0; i < NumberOfParts; i++)
                {
                    parts[i] = new Geometry(ShapeHelper.GetPoints(this, Parts[i]), GeometryType.LineString, Srid);
                }

                return new Geometry(parts, GeometryType.Polygon, Srid);
            }
            else
            {
                return new Geometry(new Geometry[] { new Geometry(ShapeHelper.GetPoints(this, Parts[0]), GeometryType.LineString, Srid) }, GeometryType.Polygon, Srid);
            }
        }


        public bool IsNullOrEmpty()
        {
            return Points == null || Points.Length < 1;
        }

    }

}
