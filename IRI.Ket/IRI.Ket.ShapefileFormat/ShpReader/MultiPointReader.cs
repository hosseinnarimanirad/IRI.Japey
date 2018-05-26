// besmellahe rahmane rahim
// Allahomma ajjel le-valiyek al-faraj

using IRI.Ket.ShapefileFormat.EsriType;
using IRI.Ket.ShapefileFormat.ShpReader;
using System;
using System.Collections.Generic;
using System.Text;

namespace IRI.Ket.ShapefileFormat.Reader
{
    public class MultiPointReader : PointsReader<EsriMultiPoint>
    {
        public MultiPointReader(string fileName)
            : base(fileName, EsriShapeType.EsriMultiPoint)
        {

        }

        protected override EsriMultiPoint ReadElement()
        {
            int shapeType = shpReader.ReadInt32();

            if ((EsriShapeType)shapeType != EsriShapeType.EsriMultiPoint)
            {
                throw new NotImplementedException();
            }

            IRI.Sta.Common.Primitives.BoundingBox boundingBox = this.ReadBoundingBox();

            int numPoints = shpReader.ReadInt32();

            EsriPoint[] points = this.ReadPoints(numPoints);

            return new EsriMultiPoint(boundingBox, points);
        }

        public static EsriMultiPoint Read(System.IO.BinaryReader reader, int offset, int contentLength)
        {
            //+8: pass the record header; +4 pass the shapeType
            reader.BaseStream.Position = offset * 2 + 8 + 4;

            //var byteArray = reader.ReadBytes(contentLength * 2 - 8);

            var boundingBox = ShpBinaryReader.ReadBoundingBox(reader);

            var numPoints = reader.ReadInt32();

            var points = ShpBinaryReader.ReadPoints(reader, numPoints);

            return new EsriMultiPoint(boundingBox, points);
        }

        
    }
}
