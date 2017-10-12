// besmellahe rahmane rahim
// Allahomma ajjel le-valiyek al-faraj

using IRI.Ket.ShapefileFormat.EsriType;
using System;
using System.Collections.Generic;
using System.Text;

namespace IRI.Ket.ShapefileFormat.Reader
{
    public abstract class zReader<T> : MeasuresReader<T> where T : IShape
    {
        public zReader(string fileName, ShapeType type)
            : base(fileName, type)
        {

        }

        protected void ReadZValues(int numberOfPoints, out double minZ, out double maxZ, out double[] zValues)
        {
            minZ = shpReader.ReadDouble();

            maxZ = shpReader.ReadDouble();

            zValues = new double[numberOfPoints];

            for (int i = 0; i < numberOfPoints; i++)
            {
                zValues[i] = shpReader.ReadDouble();
            }

        }
    }
}
