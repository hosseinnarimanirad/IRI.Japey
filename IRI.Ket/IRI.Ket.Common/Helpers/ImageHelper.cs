using IRI.Msh.Algebra;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Ket.Common.Helpers
{
    public static class ImageHelper
    {
        public static double? GetLatitude(Bitmap bitmap)
        {
            try
            {
                ///Property Item 0x0001 - PropertyTagGpsLatitude
                PropertyItem propItemRef = bitmap.GetPropertyItem(1);
                //Property Item 0x0002 - PropertyTagGpsLatitude
                PropertyItem propItemLat = bitmap.GetPropertyItem(2);

                return ExifGpsToDouble(propItemRef, propItemLat);
            }
            catch (ArgumentException)
            {
                return null;
            }
        }

        public static double? GetLongitude(Bitmap bitmap)
        {
            try
            {
                ///Property Item 0x0003 - PropertyTagGpsLongitudeRef
                PropertyItem propItemRef = bitmap.GetPropertyItem(3);
                //Property Item 0x0004 - PropertyTagGpsLongitude
                PropertyItem propItemLong = bitmap.GetPropertyItem(4);

                return ExifGpsToDouble(propItemRef, propItemLong);
            }
            catch (ArgumentException)
            {
                return null;
            }
        }

        public static double? GetAltitude(Bitmap bitmap)
        {
            try
            {
                ///Property Item 0x0005 - !?
                PropertyItem propItemRef = bitmap.GetPropertyItem(5);
                //Property Item 0x0006 - PropertyTagGpsAltitude
                PropertyItem propItemAlt = bitmap.GetPropertyItem(6);

                var numerator = BitConverter.ToUInt32(propItemAlt.Value, 0);

                var denominator = BitConverter.ToUInt32(propItemAlt.Value, 4);

                return numerator / denominator;
            }
            catch (ArgumentException)
            {
                return null;
            }
        }

        private static double ExifGpsToDouble(PropertyItem propItemRef, PropertyItem propItem)
        {
            uint degreesNumerator = BitConverter.ToUInt32(propItem.Value, 0);
            uint degreesDenominator = BitConverter.ToUInt32(propItem.Value, 4);
            double degrees = degreesNumerator / (double)degreesDenominator;

            uint minutesNumerator = BitConverter.ToUInt32(propItem.Value, 8);
            uint minutesDenominator = BitConverter.ToUInt32(propItem.Value, 12);
            double minutes = minutesNumerator / (double)minutesDenominator;

            uint secondsNumerator = BitConverter.ToUInt32(propItem.Value, 16);
            uint secondsDenominator = BitConverter.ToUInt32(propItem.Value, 20);
            double seconds = secondsNumerator / (double)secondsDenominator;

            double coorditate = degrees + (minutes / 60d) + (seconds / 3600d);

            string gpsRef = Encoding.ASCII.GetString(new byte[1] { propItemRef.Value[0] }); //N, S, E, or W

            if (gpsRef == "S" || gpsRef == "W")
                coorditate = 0 - coorditate;

            return coorditate;
        }


        public static Size GetSize(string fileName)
        {
            Size result = new Size();

            using (Bitmap img = new Bitmap(fileName))
            {
                result.Height = img.Height;

                result.Width = img.Width;
            }

            return result;
        }

        public static IRI.Msh.Common.Primitives.Point3D GetWgs84Location(Bitmap bitmap)
        {
            var latitude = GetLatitude(bitmap);

            var longitude = GetLongitude(bitmap);

            var height = GetAltitude(bitmap);

            if (!(latitude.HasValue && longitude.HasValue))
            {
                return IRI.Msh.Common.Primitives.Point3D.NaN;
            }
            else
            {
                if (height.HasValue)
                {
                    return new IRI.Msh.Common.Primitives.Point3D(longitude.Value, latitude.Value, height.Value);
                }
                else
                {
                    return new IRI.Msh.Common.Primitives.Point3D(longitude.Value, latitude.Value, double.NaN);
                }
            }

        }

        //public static IRI.Msh.Common.Point3D GetMercatorLocation(Bitmap bitmap)
        //{
        //    var wgs84 = GetWgs84Location(bitmap);

        //    var mercator = IRI.Msh.CoordinateSystem.MapProjection.MapProjects.GeodeticToMercator((IRI.Msh.Common.Primitives.Point)wgs84);

        //    return new IRI.Msh.Common.Point3D(mercator.X, mercator.Y, wgs84.Z);
        //}

        /// <summary>
        /// Point3D is in geodetic
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="point"></param>
        public static void SaveGeoTagInfo(ref Bitmap bitmap, IRI.Msh.Common.Primitives.Point3D point)
        {
            if (point.Y < -90 || point.Y > 90 || point.X < -180 || point.X > 360)
                throw new NotImplementedException();

            var propertyItem = bitmap.PropertyItems[0];

            //
            //************* Latitude *************
            propertyItem.Id = 1;
            propertyItem.Len = 2;
            propertyItem.Type = 2;

            if (point.Y < 0)
            {
                propertyItem.Value = Encoding.Unicode.GetBytes("S");

                point.Y = -point.Y;
            }
            else
            {
                propertyItem.Value = Encoding.Unicode.GetBytes("N");//propertyItem.Value = new byte[] { 78, 0 };
            }

            bitmap.SetPropertyItem(propertyItem);

            propertyItem.Id = 2;
            propertyItem.Len = 24;
            propertyItem.Type = 5;
            propertyItem.Value = DoubleToExifGpsByte(point.Y);

            bitmap.SetPropertyItem(propertyItem);
            //******************************************************************


            //
            //************* Longitude *************
            propertyItem.Id = 3;
            propertyItem.Len = 2;
            propertyItem.Type = 2;

            if (point.X > 180) point.X = 180 - point.X;

            if (point.X < 0)
            {
                propertyItem.Value = Encoding.Unicode.GetBytes("W");

                point.X = -point.X;
            }
            else
            {
                propertyItem.Value = Encoding.Unicode.GetBytes("E"); //propertyItem.Value = new byte[] { 69, 0 };
            }

            bitmap.SetPropertyItem(propertyItem);


            propertyItem.Id = 4;
            propertyItem.Len = 24;
            propertyItem.Type = 5;
            propertyItem.Value = DoubleToExifGpsByte(point.X);

            bitmap.SetPropertyItem(propertyItem);
            //*****************************************************************


            //
            //************* Altitude *************
            propertyItem.Id = 5;
            propertyItem.Len = 1;
            propertyItem.Type = 1;
            propertyItem.Value = new byte[] { 0 };

            bitmap.SetPropertyItem(propertyItem);


            var altitude = BitConverter.GetBytes((uint)Math.Round(point.Z * 1000)).ToList();
            altitude.AddRange(BitConverter.GetBytes((uint)1000));

            propertyItem.Id = 6;
            propertyItem.Len = 8;
            propertyItem.Type = 5;
            propertyItem.Value = altitude.ToArray();

            bitmap.SetPropertyItem(propertyItem);
            //*****************************************************************
        }

        private static byte[] DoubleToExifGpsByte(double value)
        {
            Msh.MeasurementUnit.Degree degreeValue = new Msh.MeasurementUnit.Degree(value);

            //var longitudeNumerator = new int[3] { degreeValue.DegreePart, degreeValue.MinutePart, degreeValue.SecondPart };

            List<byte> result = new List<byte>();

            result.AddRange(BitConverter.GetBytes((uint)degreeValue.DegreePart));
            result.AddRange(BitConverter.GetBytes((uint)1));
            result.AddRange(BitConverter.GetBytes((int)degreeValue.MinutePart));
            result.AddRange(BitConverter.GetBytes((uint)1));
            result.AddRange(BitConverter.GetBytes((int)Math.Round(degreeValue.SecondPart * 1000)));
            result.AddRange(BitConverter.GetBytes((uint)1000));

            return result.ToArray();
        }

        public static (Bitmap image, double percent) CalculateDifBitmaps(Bitmap originalImage, Bitmap secondImage, bool ignoreWhitePixels)
        {
            if (originalImage.Width != secondImage.Width || originalImage.Height != secondImage.Height)
                throw new Exception("Sizes must be equal.");

            Bitmap result = new Bitmap(originalImage.Width, originalImage.Height);

            double differentPixels = 0;
            double equalNonWhitePixels = 0;

            var whiteColor = Color.White.ToArgb();

            for (int i = 0; i < originalImage.Width; i++)
            {
                for (int j = 0; j < originalImage.Height; j++)
                {
                    var pixel1 = originalImage.GetPixel(i, j).ToArgb();

                    var pixel2 = secondImage.GetPixel(i, j).ToArgb();

                    if (pixel1 != pixel2)
                    {
                        result.SetPixel(i, j, Color.Black);

                        differentPixels++;
                    }
                    //equal pixels
                    else if (pixel1 != whiteColor && pixel1 != 0)
                    {
                        equalNonWhitePixels++;
                    }
                }
            }

            double totalPixels = ignoreWhitePixels ? (differentPixels + equalNonWhitePixels) : (originalImage.Height * originalImage.Width);

            //1399.06.15
            //returns different percent
            return (result, differentPixels / totalPixels * 100);
        }

        //private unsafe Bitmap GetDiffBitmap(Bitmap bmp, Bitmap bmp2)
        //{
        //    if (bmp.Width != bmp2.Width || bmp.Height != bmp2.Height)
        //        throw new Exception("Sizes must be equal.");

        //    Bitmap bmpRes = null;

        //    System.Drawing.Imaging.BitmapData bmData = null;
        //    System.Drawing.Imaging.BitmapData bmData2 = null;
        //    System.Drawing.Imaging.BitmapData bmDataRes = null;

        //    try
        //    {
        //        bmpRes = new Bitmap(bmp.Width, bmp.Height);

        //        bmData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
        //        bmData2 = bmp2.LockBits(new Rectangle(0, 0, bmp2.Width, bmp2.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
        //        bmDataRes = bmpRes.LockBits(new Rectangle(0, 0, bmpRes.Width, bmpRes.Height), System.Drawing.Imaging.ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

        //        IntPtr scan0 = bmData.Scan0;
        //        IntPtr scan02 = bmData2.Scan0;
        //        IntPtr scan0Res = bmDataRes.Scan0;

        //        int stride = bmData.Stride;
        //        int stride2 = bmData2.Stride;
        //        int strideRes = bmDataRes.Stride;

        //        int nWidth = bmp.Width;
        //        int nHeight = bmp.Height;

        //        //for(int y = 0; y < nHeight; y++)
        //        System.Threading.Tasks.Parallel.For(0, nHeight, y =>
        //        {
        //            //define the pointers inside the first loop for parallelizing
        //            byte* p = (byte*)scan0.ToPointer();
        //            p += y * stride;
        //            byte* p2 = (byte*)scan02.ToPointer();
        //            p2 += y * stride2;
        //            byte* pRes = (byte*)scan0Res.ToPointer();
        //            pRes += y * strideRes;

        //            for (int x = 0; x < nWidth; x++)
        //            {
        //                //always get the complete pixel when differences are found
        //                if (p[0] != p2[0] || p[1] != p2[1] || p[2] != p2[2])
        //                {
        //                    pRes[0] = p2[0];
        //                    pRes[1] = p2[1];
        //                    pRes[2] = p2[2];

        //                    //alpha (opacity)
        //                    pRes[3] = p2[3];
        //                }

        //                p += 4;
        //                p2 += 4;
        //                pRes += 4;
        //            }
        //        });

        //        bmp.UnlockBits(bmData);
        //        bmp2.UnlockBits(bmData2);
        //        bmpRes.UnlockBits(bmDataRes);
        //    }
        //    catch
        //    {
        //        if (bmData != null)
        //        {
        //            try
        //            {
        //                bmp.UnlockBits(bmData);
        //            }
        //            catch
        //            {

        //            }
        //        }

        //        if (bmData2 != null)
        //        {
        //            try
        //            {
        //                bmp2.UnlockBits(bmData2);
        //            }
        //            catch
        //            {

        //            }
        //        }

        //        if (bmDataRes != null)
        //        {
        //            try
        //            {
        //                bmpRes.UnlockBits(bmDataRes);
        //            }
        //            catch
        //            {

        //            }
        //        }

        //        if (bmpRes != null)
        //        {
        //            bmpRes.Dispose();
        //            bmpRes = null;
        //        }
        //    }

        //    return bmpRes;
        //}


    }
}