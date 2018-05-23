// besmellahe rahmane rahim
// Allahomma ajjel le-valiyek al-faraj

using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using IRI.Ket.ShapefileFormat.EsriType;

namespace IRI.Ket.ShapefileFormat.Writer
{
    public static class ShpWriter
    {
        public static void Write(string shpFileName, IEnumerable<IShape> shapes, bool createDbf = false, bool overwrite = false)
        {
            if (shapes == null || shapes.Count() < 1)
            {
                return;
            }

            if (shapes.Select(i => i.Type).Distinct().Count() > 1)
            {
                throw new NotImplementedException();
            }

            IShapeCollection collection = new ShapeCollection<IShape>(shapes?.ToList());

            Write(shpFileName, collection, createDbf, overwrite);
        }

        public static void Write(string shpFileName, IShapeCollection shapes, bool createDbf = false, bool overwrite = false)
        {
            if (shapes == null || shapes.Count < 1)
            {
                return;
            }

            var directory = System.IO.Path.GetDirectoryName(shpFileName);

            if (!System.IO.Directory.Exists(directory) && !string.IsNullOrEmpty(directory))
            {
                System.IO.Directory.CreateDirectory(directory);
            }

            ShapeType shapeType = shapes.First().Type;

            using (System.IO.MemoryStream featureWriter = new System.IO.MemoryStream())
            {
                int recordNumber = 0;

                foreach (IShape item in shapes)
                {
                    featureWriter.Write(WriteHeaderToByte(++recordNumber, item), 0, 2 * ShapeConstants.IntegerSize);

                    featureWriter.Write(item.WriteContentsToByte(), 0, 2 * item.ContentLength);
                }

                using (System.IO.MemoryStream shpWriter = new System.IO.MemoryStream())
                {
                    int fileLength = (int)featureWriter.Length / 2 + 50;

                    shpWriter.Write(WriteMainHeader(shapes, fileLength, shapeType), 0, 100);

                    shpWriter.Write(featureWriter.ToArray(), 0, (int)featureWriter.Length);

                    //var mode = overwrite ? System.IO.FileMode.Create : System.IO.FileMode.CreateNew;
                    var mode = Shapefile.GetMode(shpFileName, overwrite);

                    System.IO.FileStream stream = new System.IO.FileStream(shpFileName, mode);

                    shpWriter.WriteTo(stream);

                    stream.Close();

                    shpWriter.Close();

                    featureWriter.Close();
                }
            }

            ShxWriter.Write(Shapefile.GetShxFileName(shpFileName), shapes, shapeType, overwrite);

            if (createDbf)
            {
                Dbf.DbfFile.Write(Shapefile.GetDbfFileName(shpFileName), shapes.Count, overwrite);
            }
        }

        internal static byte[] WriteMainHeader(IShapeCollection shapes, int fileLength, ShapeType shapeType)
        {
            System.IO.MemoryStream result = new System.IO.MemoryStream();

            result.Write(IRI.Ket.Common.Helpers.StreamHelper.Int32ToBigEndianOrderedBytes(ShapeConstants.FileCode), 0, ShapeConstants.IntegerSize);

            result.Write(System.BitConverter.GetBytes(0), 0, ShapeConstants.IntegerSize);

            result.Write(System.BitConverter.GetBytes(0), 0, ShapeConstants.IntegerSize);

            result.Write(System.BitConverter.GetBytes(0), 0, ShapeConstants.IntegerSize);

            result.Write(System.BitConverter.GetBytes(0), 0, ShapeConstants.IntegerSize);

            result.Write(System.BitConverter.GetBytes(0), 0, ShapeConstants.IntegerSize);

            result.Write(IRI.Ket.Common.Helpers.StreamHelper.Int32ToBigEndianOrderedBytes(fileLength), 0, ShapeConstants.IntegerSize);

            result.Write(System.BitConverter.GetBytes(ShapeConstants.Version), 0, ShapeConstants.IntegerSize);

            result.Write(System.BitConverter.GetBytes((int)shapeType), 0, ShapeConstants.IntegerSize);

            result.Write(System.BitConverter.GetBytes(shapes.MainHeader.XMin), 0, ShapeConstants.DoubleSize);

            result.Write(System.BitConverter.GetBytes(shapes.MainHeader.YMin), 0, ShapeConstants.DoubleSize);

            result.Write(System.BitConverter.GetBytes(shapes.MainHeader.XMax), 0, ShapeConstants.DoubleSize);

            result.Write(System.BitConverter.GetBytes(shapes.MainHeader.YMax), 0, ShapeConstants.DoubleSize);


            double tempValue = shapes.MainHeader.ZMin;

            if (double.IsNaN(tempValue)) { tempValue = 0; }

            result.Write(System.BitConverter.GetBytes(tempValue), 0, ShapeConstants.DoubleSize);


            tempValue = shapes.MainHeader.ZMax;

            if (double.IsNaN(tempValue)) { tempValue = 0; }

            result.Write(System.BitConverter.GetBytes(tempValue), 0, ShapeConstants.DoubleSize);


            tempValue = shapes.MainHeader.MMin;

            if (double.IsNaN(tempValue)) { tempValue = 0; }

            result.Write(System.BitConverter.GetBytes(tempValue), 0, ShapeConstants.DoubleSize);


            tempValue = shapes.MainHeader.MMax;

            if (double.IsNaN(tempValue)) { tempValue = 0; }

            result.Write(System.BitConverter.GetBytes(tempValue), 0, ShapeConstants.DoubleSize);

            return result.ToArray();
        }

        internal static byte[] WriteHeaderToByte(int recordNumber, IShape shape)
        {
            System.IO.MemoryStream result = new System.IO.MemoryStream();

            result.Write(IRI.Ket.Common.Helpers.StreamHelper.Int32ToBigEndianOrderedBytes(recordNumber), 0, ShapeConstants.IntegerSize);

            result.Write(IRI.Ket.Common.Helpers.StreamHelper.Int32ToBigEndianOrderedBytes(shape.ContentLength), 0, ShapeConstants.IntegerSize);

            return result.ToArray();
        }

        internal static byte[] WriteBoundingBoxToByte(ISimplePoints shape)
        {
            System.IO.MemoryStream result = new System.IO.MemoryStream();

            result.Write(System.BitConverter.GetBytes(shape.MinimumBoundingBox.XMin), 0, ShapeConstants.DoubleSize);

            result.Write(System.BitConverter.GetBytes(shape.MinimumBoundingBox.YMin), 0, ShapeConstants.DoubleSize);

            result.Write(System.BitConverter.GetBytes(shape.MinimumBoundingBox.XMax), 0, ShapeConstants.DoubleSize);

            result.Write(System.BitConverter.GetBytes(shape.MinimumBoundingBox.YMax), 0, ShapeConstants.DoubleSize);

            return result.ToArray();
        }

        //internal static byte[] WritePointsToByte(EsriPoint[] values)
        //{
        //    System.IO.MemoryStream result = new System.IO.MemoryStream();

        //    foreach (EsriPoint item in values)
        //    {
        //        result.Write(System.BitConverter.GetBytes(item.X), 0, ShapeConstants.DoubleSize);

        //        result.Write(System.BitConverter.GetBytes(item.Y), 0, ShapeConstants.DoubleSize);
        //    }

        //    return result.ToArray();
        //}

        //internal static byte[] WriteAdditionalData(double minValue, double maxValue, double[] values)
        //{
        //    System.IO.MemoryStream result = new System.IO.MemoryStream();

        //    result.Write(System.BitConverter.GetBytes(minValue), 0, ShapeConstants.DoubleSize);

        //    result.Write(System.BitConverter.GetBytes(maxValue), 0, ShapeConstants.DoubleSize);

        //    foreach (double item in values)
        //    {
        //        result.Write(System.BitConverter.GetBytes(item), 0, ShapeConstants.DoubleSize);
        //    }

        //    return result.ToArray();
        //}

        internal static byte[] CheckNoDataAndGetByteValue(double value)
        {
            return BitConverter.GetBytes(double.IsNaN(value) ? ShapeConstants.NoDataValue : value);
        }

        internal static byte[] WritePointsToByte(EsriPoint[] values)
        {
            System.IO.MemoryStream result = new System.IO.MemoryStream();

            foreach (EsriPoint item in values)
            {
                result.Write(CheckNoDataAndGetByteValue(item.X), 0, ShapeConstants.DoubleSize);

                result.Write(CheckNoDataAndGetByteValue(item.Y), 0, ShapeConstants.DoubleSize);
            }

            return result.ToArray();
        }

        internal static byte[] WriteAdditionalData(double minValue, double maxValue, double[] values)
        {
            System.IO.MemoryStream result = new System.IO.MemoryStream();

            result.Write(CheckNoDataAndGetByteValue(minValue), 0, ShapeConstants.DoubleSize);

            result.Write(CheckNoDataAndGetByteValue(maxValue), 0, ShapeConstants.DoubleSize);

            foreach (double item in values)
            {
                result.Write(CheckNoDataAndGetByteValue(item), 0, ShapeConstants.DoubleSize);
            }

            return result.ToArray();
        }
    }
}
