using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRI.Msh.Common.Helpers
{
    public static class ConversionHelper
    {
        public const double InchToMeterFactor = 1200.0 / (3937.0 * 12.0);

        public static readonly double MeterToPixelFactor;

        public static readonly double PixelToMeterFactor;

        static ConversionHelper()
        {
            MeterToPixelFactor = MeterToPixel(1, 96);

            PixelToMeterFactor = PixelToMeter(1, 96);
        }

        public static double PixelToMeter(double pixels, int dpi = 96)
        {
            return pixels / dpi * InchToMeterFactor;
        }

        static public double MeterToPixel(double meter, int dpi = 96)
        {
            return meter / InchToMeterFactor * dpi;
        }


        //SQL VARBINARY TEXT REPRESENTATION
        public static byte[] VarbinaryTextRepresentationToByteArray(string stringFromSQL)
        {
            string hexPart = stringFromSQL.Substring(2);

            byte[] result = new byte[hexPart.Length / 2];

            for (int i = 0; i < hexPart.Length / 2; i++)
            {
                string hexNumber = hexPart.Substring(i * 2, 2);
                result[i] = ((byte)Convert.ToInt32(hexNumber, 16));
            }

            return result;
        }

    }
}
