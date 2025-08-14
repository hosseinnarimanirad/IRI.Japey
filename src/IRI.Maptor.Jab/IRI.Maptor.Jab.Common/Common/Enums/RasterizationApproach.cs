using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Maptor.Jab.Common.Enums;

public enum RasterizationApproach
{
    GdiPlus,
    //OpenTk,
    DrawingVisual,
    WriteableBitmap,
    StreamGeometry,
    None
}
