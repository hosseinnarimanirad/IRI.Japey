using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace IRI.Standards.OGC.SFA
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct OgcPoint<T> : IOgcGeometry where T : IPoint, new()
    {
        byte byteOrder;

        UInt32 type;

        T point;

        public OgcPoint(double x, double y)
        {
            byteOrder = (byte)WkbByteOrder.WkbNdr;

            type = (UInt32)WkbGeometryType.Point;

            point = new T();
        }

        public double X { get { return this.point.X; } }

        public double Y { get { return this.point.Y; } }

        public WkbByteOrder ByteOrder
        {
            get { return (WkbByteOrder)this.byteOrder; }
        }

        public WkbGeometryType Type
        {
            get { return (WkbGeometryType)this.type; }
        }

        public OgcPoint(byte[] buffer)
            : this(new System.IO.BinaryReader(new System.IO.MemoryStream(buffer)))
        {
        }

        public OgcPoint(System.IO.BinaryReader reader)
        {
            this.byteOrder = reader.ReadByte();

            this.type = reader.ReadUInt32();

            if (WkbGeometryTypeFactory.WkbTypeMap[typeof(OgcPoint<T>)] != (WkbGeometryType)this.type)
            {
                throw new NotImplementedException();
            }

            int length = System.Runtime.InteropServices.Marshal.SizeOf(typeof(T));

            point = IRI.Ket.Common.Helpers.StreamHelper.ByteArrayToStructure<T>(
                                reader.ReadBytes(length));
        }

        public byte[] ToWkb()
        {
            using (System.IO.MemoryStream stream = new System.IO.MemoryStream())
            {
                System.IO.BinaryWriter writer = new System.IO.BinaryWriter(stream);

                writer.Write(byteOrder);

                writer.Write(type);

                writer.Write(IRI.Ket.Common.Helpers.StreamHelper.StructureToByteArray(point));

                writer.Close();

                return stream.ToArray();
            }
        }

    }
}
