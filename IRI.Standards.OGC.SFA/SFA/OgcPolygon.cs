using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;

namespace IRI.Standards.OGC.SFA
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct OgcPolygon<T> : IOgcGeometry where T : IPoint
    {
        byte byteOrder;

        UInt32 type;

        UInt32 numRings;

        LinearRing<T>[] rings;

        public LinearRing<T>[] Rings { get { return this.rings; } }

        public WkbByteOrder ByteOrder
        {
            get { return (WkbByteOrder)this.byteOrder; }
        }

        public WkbGeometryType Type
        {
            get { return (WkbGeometryType)this.type; }
        }

        public byte[] ToWkb()
        {
            using (System.IO.MemoryStream stream = new System.IO.MemoryStream())
            {
                System.IO.BinaryWriter writer = new System.IO.BinaryWriter(stream);

                writer.Write(byteOrder);

                writer.Write(type);

                writer.Write(numRings);

                foreach (LinearRing<T> item in Rings)
                {
                    writer.Write(item.ToBinary());
                }

                writer.Close();

                return stream.ToArray();
            }
        }

        public OgcPolygon(byte[] buffer)
            : this(new System.IO.BinaryReader(new System.IO.MemoryStream(buffer)))
        {
        }

        public OgcPolygon(System.IO.BinaryReader reader)
        {
            this.byteOrder = reader.ReadByte();

            this.type = reader.ReadUInt32();

            if (WkbGeometryTypeFactory.WkbTypeMap[typeof(OgcPolygon<T>)] != (WkbGeometryType)this.type)
            {
                throw new NotImplementedException();
            }

            this.numRings = reader.ReadUInt32();

            this.rings = new LinearRing<T>[this.numRings];

            for (int i = 0; i < this.numRings; i++)
            {
                this.rings[i] = new LinearRing<T>(reader);
            }
        }
    }
}
