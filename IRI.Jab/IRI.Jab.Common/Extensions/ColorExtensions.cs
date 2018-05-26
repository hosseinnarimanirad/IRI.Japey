using IRI.Sta.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace IRI.Jab.Common.Extensions
{
    public static class ColorExtensions
    {
        public static System.Drawing.Color AsGdiColor(this Color color)
        {
            return System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B);
        }

        public static System.Drawing.SolidBrush AsGdiBrush(this Color color)
        {
            return new System.Drawing.SolidBrush(color.AsGdiColor());
        }

        public static string ToHexString(this Color color)
        {
            return color.ToString();
        }
         
    }
}
