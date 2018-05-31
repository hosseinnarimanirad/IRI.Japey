// besmellahe rahmane rahim
// Allahomma ajjel le-valiyek al-faraj

using IRI.Sta.Common.Primitives;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Linq;

namespace IRI.Ket.ShapefileFormat.EsriType
{


    public struct EsriPoint : IPoint, IEsriShape
    {
        private double x, y;

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

        public EsriPoint(double x, double y)
        {
            this.x = x;

            this.y = y;
        }


        public BoundingBox MinimumBoundingBox
        {
            get { return new BoundingBox(this.X, this.Y, this.X, this.Y); }
        }

        //public byte[] WriteContentsToByte()
        //{
        //    System.IO.MemoryStream result = new System.IO.MemoryStream();

        //    result.Write(System.BitConverter.GetBytes((int)ShapeType.Point), 0, ShapeConstants.IntegerSize);

        //    result.Write(System.BitConverter.GetBytes(this.X), 0, ShapeConstants.DoubleSize);

        //    result.Write(System.BitConverter.GetBytes(this.Y), 0, ShapeConstants.DoubleSize);

        //    return result.ToArray();
        //}
        public byte[] WriteContentsToByte()
        {
            System.IO.MemoryStream result = new System.IO.MemoryStream();

            result.Write(System.BitConverter.GetBytes((int)EsriShapeType.EsriPoint), 0, ShapeConstants.IntegerSize);

            result.Write(Writer.ShpWriter.CheckNoDataAndGetByteValue(this.X), 0, ShapeConstants.DoubleSize);

            result.Write(Writer.ShpWriter.CheckNoDataAndGetByteValue(this.Y), 0, ShapeConstants.DoubleSize);

            return result.ToArray();
        }

        /// <summary>
        /// content length in 16bit words
        /// </summary>
        public int ContentLength
        {
            get { return ShapeConstants.PointContentLengthInWords; }
        }

        public EsriShapeType Type
        {
            get { return EsriShapeType.EsriPoint; }
        }

        public string AsSqlServerWkt()
        {
            return string.Format(System.Globalization.CultureInfo.InvariantCulture, "POINT({0:G17} {1:G17})", this.X, this.Y);
        }

        public byte[] AsWkb()
        {
            return OgcWkbMapFunctions.ToWkbPoint(this);
        }

        /// <summary>
        /// Returs Kml representation of the point. Note: Point must be in Lat/Long System
        /// </summary>
        /// <returns></returns>
        public IRI.Ket.KmlFormat.Primitives.PlacemarkType AsPlacemark(Func<IRI.Sta.Common.Primitives.Point, IRI.Sta.Common.Primitives.Point> projectToGeodeticFunc = null, byte[] color = null)
        {
            IRI.Ket.KmlFormat.Primitives.PlacemarkType placemark = new KmlFormat.Primitives.PlacemarkType();

            IRI.Ket.KmlFormat.Primitives.PointType point = new KmlFormat.Primitives.PointType();

            IRI.Sta.Common.Primitives.Point coordinates = new Point(this.x, this.Y);

            if (projectToGeodeticFunc != null)
            {
                coordinates = projectToGeodeticFunc(new Point(this.X, this.Y));
            }

            point.coordinates = string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:G17},{1:G17}", coordinates.X, coordinates.Y);

            //placemark.AbstractFeatureObjectExtensionGroup = new KmlFormat.Primitives.AbstractObjectType[] { point };
            placemark.AbstractGeometry = point;

            return placemark;

        }

        public string AsKml(Func<IRI.Sta.Common.Primitives.Point, IRI.Sta.Common.Primitives.Point> projectToGeodeticFunc = null)
        {
            //IRI.Ket.KmlFormat.Primitives.KmlType result = new KmlFormat.Primitives.KmlType();

            //IRI.Ket.KmlFormat.Primitives.DocumentType document = new KmlFormat.Primitives.DocumentType();

            //document.AbstractFeature = new KmlFormat.Primitives.AbstractFeatureType[] { this.AsPlacemark(projectToGeodeticFunc) };

            //result.KmlObjectExtensionGroup = new KmlFormat.Primitives.AbstractObjectType[] { document };

            //return IRI.Ket.IO.XmlStream.Parse(result);
            return OgcKmlMapFunctions.AsKml(this.AsPlacemark(projectToGeodeticFunc));
        }

        public IEsriShape Transform(Func<IPoint, IPoint> transform)
        {
            var result = transform(this);

            return new EsriPoint(result.X, result.Y);
        }

        public Geometry AsGeometry()
        {
            return new Geometry(new IPoint[] { new Point(X, Y) }, GeometryType.Point);
        }

        public static explicit operator EsriPoint(Point value)
        {
            return new EsriPoint(value.X, value.Y);
        }

        public string AsExactString()
        {
            return string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:G17} {1:G17}", this.X, this.Y);
        }

        public bool AreExactlyTheSame(object obj)
        {
            if (obj.GetType() != typeof(EsriPoint))
            {
                return false;
            }

            return this.AsExactString() == ((EsriPoint)obj).AsExactString();
        }

        public double DistanceTo(IPoint point)
        {
            return Point.GetDistance(new Point(this.X, this.Y), new Point(point.X, point.Y));
        }

        public override string ToString()
        {
            return $"X: {X}, Y:{Y}";
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() != typeof(EsriPoint))
            {
                return false;
            }

            return ((EsriPoint)obj).AsExactString() == this.AsExactString();
        }

        public override int GetHashCode()
        {
            return this.AsExactString().GetHashCode();
        }

    }

}
