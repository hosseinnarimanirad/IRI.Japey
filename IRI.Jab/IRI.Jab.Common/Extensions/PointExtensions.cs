using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Jab.Common.Extensions
{
    public static class PointExtensions
    {
        public static System.Drawing.PointF AsGdiPointF(this System.Windows.Point point)
        {
            return new System.Drawing.PointF((float)point.X, (float)point.Y);
        }

        public static System.Drawing.Point AsGdiPoint(this System.Windows.Point point)
        {
            return new System.Drawing.Point(Convert.ToInt32(Math.Round(point.X)), Convert.ToInt32(Math.Round(point.Y)));
        }

        public static IRI.Msh.Common.Primitives.Point AsPoint(this System.Windows.Point point)
        {
            return new IRI.Msh.Common.Primitives.Point(point.X, point.Y);
        }
    }
}
