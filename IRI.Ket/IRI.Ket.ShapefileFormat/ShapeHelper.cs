using IRI.Ket.ShapefileFormat.EsriType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRI.Ket.ShapefileFormat
{
    static class ShapeHelper
    {
        internal static EsriPoint[] GetPoints(ISimplePoints shape, int startIndex)
        {
            int partNumber = Array.IndexOf(shape.Parts, startIndex);

            //int startIndex = shape.Parts[partNumber];

            int lastIndex = (partNumber == shape.NumberOfParts - 1) ? shape.NumberOfPoints - 1 : shape.Parts[partNumber + 1] - 1;

            EsriPoint[] result = new EsriPoint[lastIndex - startIndex + 1];

            Array.ConstrainedCopy(shape.Points, startIndex, result, 0, result.Length);

            return result;
        }

        internal static double[] GetMeasures(IPointsWithMeasure shape, int startIndex)
        {
            int partNumber = Array.IndexOf(shape.Parts, startIndex);

            //int startIndex = shape.Parts[partNumber];

            int lastIndex = (partNumber == shape.NumberOfParts - 1) ? shape.NumberOfPoints - 1 : shape.Parts[partNumber + 1] - 1;

            double[] result = new double[lastIndex - startIndex + 1];

            Array.ConstrainedCopy(shape.Measures, startIndex, result, 0, result.Length);

            return result;
        }

        internal static double[] GetZValues(IPointsWithZ shape, int startIndex)
        {
            int partNumber = Array.IndexOf(shape.Parts, startIndex);

            //int startIndex = shape.Parts[partNumber];

            int lastIndex = (partNumber == shape.NumberOfParts - 1) ? shape.NumberOfPoints - 1 : shape.Parts[partNumber + 1] - 1;

            double[] result = new double[lastIndex - startIndex + 1];

            Array.ConstrainedCopy(shape.ZValues, startIndex, result, 0, result.Length);

            return result;
        }
    }
}
