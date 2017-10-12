// besmellahe rahmane rahim
// Allahomma ajjel le-valiyek al-faraj

using IRI.Ham.SpatialBase;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace IRI.Ket.ShapefileFormat.EsriType
{
    public struct EsriPointZ : IRI.Ham.SpatialBase.IPoint, IShape
    {
        private double x, y, z, measure;

        public double X
        {
            get { return this.x; }
            set { this.x = value; }
        }

        public double Y
        {
            get { return this.y; }
            set { this.y = value; }
        }

        public double Z
        {
            get { return this.z; }
        }

        public double Measure
        {
            get { return this.measure; }
        }

        public EsriPointZ(double x, double y, double z)
            : this(x, y, z, ShapeConstants.NoDataValue)
        {
        }

        public EsriPointZ(double x, double y, double z, double measure)
        {
            this.x = x;

            this.y = y;

            this.z = z;

            this.measure = measure;
        }


        #region IShape Members


        public IRI.Ham.SpatialBase.BoundingBox MinimumBoundingBox
        {
            get { return new IRI.Ham.SpatialBase.BoundingBox(this.X, this.Y, this.X, this.Y); }
        }

        //public byte[] WriteContentsToByte()
        //{
        //    System.IO.MemoryStream result = new System.IO.MemoryStream();

        //    result.Write(System.BitConverter.GetBytes((int)ShapeType.PointZ), 0, ShapeConstants.IntegerSize);

        //    result.Write(System.BitConverter.GetBytes(this.X), 0, ShapeConstants.DoubleSize);

        //    result.Write(System.BitConverter.GetBytes(this.Y), 0, ShapeConstants.DoubleSize);

        //    result.Write(System.BitConverter.GetBytes(this.Z), 0, ShapeConstants.DoubleSize);

        //    result.Write(System.BitConverter.GetBytes(this.Measure), 0, ShapeConstants.DoubleSize);

        //    return result.ToArray();
        //}

        public byte[] WriteContentsToByte()
        {
            System.IO.MemoryStream result = new System.IO.MemoryStream();

            result.Write(System.BitConverter.GetBytes((int)ShapeType.PointZ), 0, ShapeConstants.IntegerSize);

            result.Write(Writer.ShpWriter.CheckNoDataAndGetByteValue(this.X), 0, ShapeConstants.DoubleSize);

            result.Write(Writer.ShpWriter.CheckNoDataAndGetByteValue(this.Y), 0, ShapeConstants.DoubleSize);

            result.Write(Writer.ShpWriter.CheckNoDataAndGetByteValue(this.Z), 0, ShapeConstants.DoubleSize);

            result.Write(Writer.ShpWriter.CheckNoDataAndGetByteValue(this.Measure), 0, ShapeConstants.DoubleSize);

            return result.ToArray();
        }

        public int ContentLength
        {
            get { return ShapeConstants.PointZContentLengthInWords; }
        }

        public ShapeType Type
        {
            get { return ShapeType.PointZ; }
        }

        public string AsSqlServerWkt()
        {
            return string.Format(System.Globalization.CultureInfo.InvariantCulture, "POINT({0:G17} {1:G17} {2:G17} {3})", this.X, this.Y, this.Z, this.Measure == ShapeConstants.NoDataValue ? "NULL" : this.Measure.ToString("G17"));
        }

        public byte[] AsWkb()
        {
            return OgcWkbMapFunctions.ToWkbPoint(this);
            //byte[] result = new byte[37];

            //Array.Copy(BitConverter.GetBytes((byte)IRI.Standards.OGC.SFA.WkbByteOrder.WkbNdr), 0, result, 0, 1);

            //Array.Copy(BitConverter.GetBytes((int)IRI.Standards.OGC.SFA.WkbGeometryType.PointZM), 0, result, 1, 4);

            //Array.Copy(BitConverter.GetBytes(this.X), 0, result, 5, 8);

            //Array.Copy(BitConverter.GetBytes(this.Y), 0, result, 13, 8);

            //Array.Copy(BitConverter.GetBytes(this.Z), 0, result, 21, 8);

            //Array.Copy(BitConverter.GetBytes(this.Measure), 0, result, 29, 8);

            //return result;
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

        public IShape Transform(Func<Ham.SpatialBase.IPoint, Ham.SpatialBase.IPoint> transform)
        {
            var result = transform(this);

            return new EsriPointZ(result.X, result.Y, this.Z, this.Measure);
        }

        #endregion

        public string AsExactString()
        {
            return string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:G17} {1:G17} {2:G17} {3:G17}", this.X, this.Y, this.Z, this.Measure);
        }

        public bool AreExactlyTheSame(object obj)
        {
            if (obj.GetType() != typeof(EsriPointZ))
            {
                return false;
            }

            return this.AsExactString() == ((EsriPointZ)obj).AsExactString();
        }

        public double DistanceTo(IPoint point)
        {
            if (point is EsriPointZ)
            {
                return Point3D.GetDistance(new Point3D(this.X, this.Y, this.Z), new Point3D(point.X, point.Y, ((EsriPointZ)point).Z));
            }
            else
            {
                return new Point3D(this.X, this.Y, this.Z).DistanceTo(point);
            }

        }

    }

}
