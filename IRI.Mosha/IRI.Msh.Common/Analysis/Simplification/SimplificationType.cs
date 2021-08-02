using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Msh.Common.Analysis
{
    public enum SimplificationType
    {
        ByArea,
        AdditiveByArea,
        AdditiveByAreaPlus,

        ByAngle,
        AdditiveByAngle,

        AdditiveByDistance,

        AdditiveByAreaAngle,

        Visvalingam,
        DouglasPeucker,
        Lang,

        // 1400.05.11
        // http://psimpl.sourceforge.net/reumann-witkam.html
        Reumann_Witkam,

        // 1400.05.11
        // http://psimpl.sourceforge.net/perpendicular-distance.html
        PerpendicularDistance,
    }
}
