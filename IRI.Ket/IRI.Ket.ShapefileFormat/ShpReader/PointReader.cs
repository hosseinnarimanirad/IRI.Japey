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
        public PointReader(string fileName, int srid)
            : base(fileName, EsriShapeType.EsriPoint, srid)
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

            return new EsriPoint(x, y, this._srid);
        }


        public static EsriPoint Read(System.IO.BinaryReader reader, int offset, int contentLength, int srid)
        {
            //+8: pass the record header; +4 pass the shapeType
            reader.BaseStream.Position = offset * 2 + 8 + 4;

            //var byteArray = reader.ReadBytes(contentLength * 2 - 8);

            double x = reader.ReadDouble();

            double y = reader.ReadDouble();

            return new EsriPoint(x, y, srid);
        }
    }
}
