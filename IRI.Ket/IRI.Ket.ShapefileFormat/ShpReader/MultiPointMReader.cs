// besmellahe rahmane rahim
// Allahomma ajjel le-valiyek al-faraj

using IRI.Ket.ShapefileFormat.EsriType;
using IRI.Ket.ShapefileFormat.ShpReader;
using System;
using System.Collections.Generic;
using System.Text;

namespace IRI.Ket.ShapefileFormat.Reader
{
    public class MultiPointMReader : MeasuresReader<EsriMultiPointM>
    {
        public MultiPointMReader(string fileName, int srid)
            : base(fileName, EsriShapeType.EsriMultiPointM, srid)
        {
        }

        protected override EsriMultiPointM ReadElement()
        {
            int shapeType = shpReader.ReadInt32();

            if ((EsriShapeType)shapeType != EsriShapeType.EsriMultiPointM)
            {
                throw new NotImplementedException();
            }

            IRI.Msh.Common.Primitives.BoundingBox boundingBox = this.ReadBoundingBox();

            int numPoints = shpReader.ReadInt32();

            EsriPoint[] points = this.ReadPoints(numPoints);

            double minMeasure, maxMeasure;

            double[] measures;

            this.ReadMeasures(numPoints, out minMeasure, out maxMeasure, out measures);

            return new EsriMultiPointM(boundingBox, points, minMeasure, maxMeasure, measures, this._srid);
        }


        public static EsriMultiPointM Read(System.IO.BinaryReader reader, int offset, int contentLength, int srid)
        {
            //+8: pass the record header; +4 pass the shapeType
            reader.BaseStream.Position = offset * 2 + 8 + 4;

            //var byteArray = reader.ReadBytes(contentLength * 2 - 8);

            var boundingBox = ShpBinaryReader.ReadBoundingBox(reader);

            var numPoints = reader.ReadInt32();

            var points = ShpBinaryReader.ReadPoints(reader, numPoints);

            double minMeasure, maxMeasure;

            double[] measures;

            ShpBinaryReader.ReadValues(reader, numPoints, out minMeasure, out maxMeasure, out measures);

            return new EsriMultiPointM(boundingBox, points, minMeasure, maxMeasure, measures, srid);
        }
 
    }
}
