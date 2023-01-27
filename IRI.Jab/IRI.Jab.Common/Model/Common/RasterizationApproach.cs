using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Jab.Common.Model
{
    public enum RasterizationApproach
    {
        GdiPlus,
        //OpenTk,
        DrawingVisual,
        WriteableBitmap,
        StreamGeometry,
        None
    }
}
