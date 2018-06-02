// besmellahe rahmane rahim
// Allahomma ajjel le-valiyek al-faraj

using IRI.Ket.ShapefileFormat.EsriType;
using IRI.Ket.ShapefileFormat.ShpReader;
using System;
using System.Collections.Generic;
using System.Text;

namespace IRI.Ket.ShapefileFormat.Reader
{
    public class PolyLineReader : PointsReader<EsriPolyline>
    {
        //Because this specification does not forbid consecutive points with identical coordinates,
        //shapefile readers must handle such cases. On the other hand, the degenerate, zero length
        //parts that might result are not allowed.
        public PolyLineReader(string fileName)
            : base(fileName, EsriShapeType.EsriPolyLine)
        {

        }

        protected override EsriPolyline ReadElement()
        {
            int shapeType = shpReader.ReadInt32();

            if ((EsriShapeType)shapeType != EsriShapeType.EsriPolyLine)
            {
                throw new NotImplementedException();
            }

            IRI.Msh.Common.Primitives.BoundingBox boundingBox = this.ReadBoundingBox();

            int numParts = shpReader.ReadInt32();

            int numPoints = shpReader.ReadInt32();

            int[] parts = new int[numParts];

            for (int i = 0; i < numParts; i++)
            {
                parts[i] = shpReader.ReadInt32();
            }

            EsriPoint[] points = this.ReadPoints(numPoints);

            return new EsriPolyline(boundingBox, parts, points);
        }

        public static EsriPolyline Read(System.IO.BinaryReader reader, int offset, int contentLength)
        {
            //+8: pass the record header; +4 pass the shapeType
            reader.BaseStream.Position = offset * 2 + 8 + 4;

            //var byteArray = reader.ReadBytes(contentLength * 2 - 8);

            var boundingBox = ShpBinaryReader.ReadBoundingBox(reader);

            int numParts = reader.ReadInt32();

            int numPoints = reader.ReadInt32();

            int[] parts = new int[numParts];

            for (int i = 0; i < numParts; i++)
            {
                parts[i] = reader.ReadInt32();
            }

            var points = ShpBinaryReader.ReadPoints(reader, numPoints);

            return new EsriPolyline(boundingBox, parts, points);
        }
    }
}
