// besmellahe rahmane rahim
// Allahomma ajjel le-valiyek al-faraj

using IRI.Ket.ShapefileFormat.EsriType;
using System;
using System.Collections.Generic;
using System.Text;



namespace IRI.Ket.ShapefileFormat.Reader
{
    public abstract class PointsReader<T> : ShpReader<T> where T : IShape
    {
        public PointsReader(string fileName, ShapeType type)
            : base(fileName, type)
        {

        }

        protected IRI.Ham.SpatialBase.BoundingBox ReadBoundingBox()
        {
            double xMin = shpReader.ReadDouble();

            double yMin = shpReader.ReadDouble();

            double xMax = shpReader.ReadDouble();

            double yMax = shpReader.ReadDouble();

            return new IRI.Ham.SpatialBase.BoundingBox(xMin, yMin, xMax, yMax);
        }

        protected EsriPoint[] ReadPoints(int numberOfPoints)
        {
            EsriPoint[] result = new EsriPoint[numberOfPoints];

            for (int i = 0; i < numberOfPoints; i++)
            {
                double tempX = shpReader.ReadDouble();

                double tempY = shpReader.ReadDouble();

                result[i] = new EsriPoint(tempX, tempY);
            }

            return result;
        }
    
        //protected EsriPoint[] ReadPoints(int numberOfPoints)
        //{
        //    EsriPoint[] result = new EsriPoint[numberOfPoints];

        //    var byteArray = shpReader.ReadBytes(16 * numberOfPoints);

        //    for (int i = 0; i < numberOfPoints; i++)
        //    {
        //        double tempX = BitConverter.ToDouble(byteArray, i * 16);

        //        double tempY = BitConverter.ToDouble(byteArray, i * 16 + 8);

        //        result[i] = new EsriPoint(tempX, tempY);
        //    }

        //    return result;
        //}

    }
}
