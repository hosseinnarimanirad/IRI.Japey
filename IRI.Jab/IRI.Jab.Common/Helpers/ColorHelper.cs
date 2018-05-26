using IRI.Sta.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace IRI.Jab.Common.Helpers
{
    public static class ColorHelper
    {
        public static Color ToWpfColor(string hexColor)
        {
            if (string.IsNullOrEmpty(hexColor))
            {
                return Colors.Transparent;
            }

            if (!hexColor.StartsWith("#"))
            {
                hexColor = $"#{hexColor}";
            }

            return (Color)ColorConverter.ConvertFromString(hexColor);
        }

        public static System.Drawing.Color ToGdiColor(string hexColor)
        {
            if (string.IsNullOrEmpty(hexColor))
            {
                return System.Drawing.Color.Transparent;
            }

            if (!hexColor.StartsWith("#"))
            {
                hexColor = $"#{hexColor}";
            }

            return System.Drawing.ColorTranslator.FromHtml(hexColor);
        }

        public static string ToHexString(System.Drawing.Color color)
        {
            //1395.10.20: returns color name for system defined colors!
            //return System.Drawing.ColorTranslator.ToHtml(color); 
            return $"#{color.A.ToString("X2")}{color.R.ToString("X2") }{color.G.ToString("X2")}{color.B.ToString("X2")}";
        }

        public static Color GetRandomWpfColor()
        {
            int randomIndex = RandomHelper.Get(goodColors.Length);

            return ToWpfColor(goodColors[randomIndex]);
        }

        private readonly static string[] goodColors =
        {
            "#FFE51400", //red
            "#FF61A917", //green
            "#FF1CA1E2", //blue
            "#FF6900FF", //purple
            "#FFF1A30B", //orange
            "#FFA4C401", //lime    
            "#FFF572D0", //pink 
            "#FFE4C802", //yellow
            "#FF835A2C", //brown 
        };
    }
}
