using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace IRI.Standards.OGC.SFA
{
    //[Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct LinearRing<T> : ILinearRing where T : IPoint
    {
        UInt32 numPoints;

        PointCollection<T> points;

        public IPointCollection Points
        {
            get { return this.points; }
        }

        public byte[] ToBinary()
        {
            using (System.IO.MemoryStream stream = new System.IO.MemoryStream())
            {
                System.IO.BinaryWriter writer = new System.IO.BinaryWriter(stream);

                writer.Write(numPoints);

                foreach (T item in points)
                {
                    writer.Write(IRI.Msh.Common.Helpers.StreamHelper.StructureToByteArray(item));
                }

                writer.Close();

                return stream.ToArray();
            }
        }

        public LinearRing(byte[] buffer)
            : this(new System.IO.BinaryReader(new System.IO.MemoryStream(buffer)))
        {
        }

        public LinearRing(System.IO.BinaryReader reader)
        {
            this.numPoints = reader.ReadUInt32();

            this.points = new PointCollection<T>((int)this.numPoints);

            int length = System.Runtime.InteropServices.Marshal.SizeOf(typeof(T));

            for (int i = 0; i < this.numPoints; i++)
            {
                this.points.Add(IRI.Msh.Common.Helpers.StreamHelper.ByteArrayToStructure<T>(
                                    reader.ReadBytes(length)));
            }
        }

        public IEnumerator<IPoint> GetEnumerator()
        {
            return points.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
