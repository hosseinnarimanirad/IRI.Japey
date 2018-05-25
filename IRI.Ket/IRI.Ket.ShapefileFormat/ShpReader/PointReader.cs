// besmellahe rahmane rahim
// Allahomma ajjel le-valiyek al-faraj

using IRI.Ket.ShapefileFormat.EsriType;
using System;
using System.Collections.Generic;
using System.Text;

namespace IRI.Ket.ShapefileFormat.Reader
{
    public class PointReader : ShpReader<EsriPoint>
    {
        public PointReader(string fileName)
            : base(fileName, EsriShapeType.EsriPoint)
        {

        }

        protected override EsriPoint ReadElement()
        {
            int shapeType = shpReader.ReadInt32();

            if ((EsriShapeType)shapeType != EsriShapeType.EsriPoint)
            {
                throw new NotImplementedException();
            }

            double x = shpReader.ReadDouble();

            double y = shpReader.ReadDouble();

            return new EsriPoint(x, y);
        }


        public static EsriPoint Read(System.IO.BinaryReader reader, int offset, int contentLength)
        {
            //+8: pass the record header; +4 pass the shapeType
            reader.BaseStream.Position = offset * 2 + 8 + 4;

            //var byteArray = reader.ReadBytes(contentLength * 2 - 8);

            double x = reader.ReadDouble();

            double y = reader.ReadDouble();
             
            return new EsriPoint(x, y);
        }
    }
}
