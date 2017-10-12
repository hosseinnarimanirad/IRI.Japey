// besmellahe rahmane rahim
// Allahomma ajjel le-valiyek al-faraj

using IRI.Ket.ShapefileFormat.EsriType;
using IRI.Ket.ShapefileFormat.ShpReader;
using System;
using System.Collections.Generic;
using System.Text;

namespace IRI.Ket.ShapefileFormat.Reader
{
    public class MultiPointReader : PointsReader<MultiPoint>
    {
        public MultiPointReader(string fileName)
            : base(fileName, ShapeType.MultiPoint)
        {

        }

        protected override MultiPoint ReadElement()
        {
            int shapeType = shpReader.ReadInt32();

            if ((ShapeType)shapeType != ShapeType.MultiPoint)
            {
                throw new NotImplementedException();
            }

            IRI.Ham.SpatialBase.BoundingBox boundingBox = this.ReadBoundingBox();

            int numPoints = shpReader.ReadInt32();

            EsriPoint[] points = this.ReadPoints(numPoints);

            return new MultiPoint(boundingBox, points);
        }

        public static MultiPoint Read(System.IO.BinaryReader reader, int offset, int contentLength)
        {
            //+8: pass the record header; +4 pass the shapeType
            reader.BaseStream.Position = offset * 2 + 8 + 4;

            //var byteArray = reader.ReadBytes(contentLength * 2 - 8);

            var boundingBox = ShpBinaryReader.ReadBoundingBox(reader);

            var numPoints = reader.ReadInt32();

            var points = ShpBinaryReader.ReadPoints(reader, numPoints);

            return new MultiPoint(boundingBox, points);
        }

        
    }
}
