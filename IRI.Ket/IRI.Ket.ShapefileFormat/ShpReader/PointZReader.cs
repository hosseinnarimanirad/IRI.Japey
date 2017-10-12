// besmellahe rahmane rahim
// Allahomma ajjel le-valiyek al-faraj

using IRI.Ket.ShapefileFormat.EsriType;
using System;
using System.Collections.Generic;
using System.Text;

namespace IRI.Ket.ShapefileFormat.Reader
{
    public class PointZReader : ShpReader<EsriPointZ> 
    {
        public PointZReader(string fileName)
            : base(fileName, ShapeType.PointZ)
        {

        }

        protected override EsriPointZ ReadElement()
        {
            int shapeType = shpReader.ReadInt32();

            if ((ShapeType)shapeType != ShapeType.PointZ)
            {
                throw new NotImplementedException();
            }

            double x = shpReader.ReadDouble();

            double y = shpReader.ReadDouble();

            double z = shpReader.ReadDouble();

            double m = shpReader.ReadDouble();

            return new EsriPointZ(x, y, z, m);
        }

        public static EsriPointZ Read(System.IO.BinaryReader reader, int offset, int contentLength)
        {
            //+8: pass the record header; +4 pass the shapeType
            reader.BaseStream.Position = offset * 2 + 8 + 4;

            //var byteArray = reader.ReadBytes(contentLength * 2 - 8);

            double x = reader.ReadDouble();

            double y = reader.ReadDouble();

            double z = reader.ReadDouble();

            double m = reader.ReadDouble();

            return new EsriPointZ(x, y, z, m);
        }
    }
}
