// besmellahe rahmane rahim
// Allahomma ajjel le-valiyek al-faraj

using IRI.Ket.ShapefileFormat.EsriType;
using IRI.Ket.ShapefileFormat.ShpReader;
using System;
using System.Collections.Generic;
using System.Text;

namespace IRI.Ket.ShapefileFormat.Reader
{
    public class PolygonMReader : MeasuresReader<EsriPolygonM>
    {
        public PolygonMReader(string fileName)
            : base(fileName, EsriShapeType.EsriPolygonM)
        {
           
        }

        protected override EsriPolygonM ReadElement()
        {

            int shapeType = shpReader.ReadInt32();

            if ((EsriShapeType)this.MainHeader.ShapeType != EsriShapeType.EsriPolygonM)
            {
                throw new NotImplementedException();
            }

            IRI.Ham.SpatialBase.BoundingBox boundingBox = this.ReadBoundingBox();

            int numParts = shpReader.ReadInt32();

            int numPoints = shpReader.ReadInt32();

            int[] parts = new int[numParts];

            for (int i = 0; i < numParts; i++)
            {
                parts[i] = shpReader.ReadInt32();
            }

            EsriPoint[] points = this.ReadPoints(numPoints);

            double minMeasure, maxMeasure;

            double[] measures;

            this.ReadMeasures(numPoints, out minMeasure, out maxMeasure, out measures);

            return new EsriPolygonM(boundingBox, parts, points, minMeasure, maxMeasure, measures);
        }

        public static EsriPolygonM Read(System.IO.BinaryReader reader, int offset, int contentLength)
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

            double minMeasure, maxMeasure;

            double[] measures;

            ShpBinaryReader.ReadValues(reader, numPoints, out minMeasure, out maxMeasure, out measures);

            return new EsriPolygonM(boundingBox, parts, points, minMeasure, maxMeasure, measures);
        }

    }
}
