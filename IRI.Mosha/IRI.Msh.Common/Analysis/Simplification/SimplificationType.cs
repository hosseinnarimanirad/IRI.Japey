using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Msh.Common.Analysis
{
    public enum SimplificationType
    {
        [Description("N")]
        ByNthPoint,

        [Description("DS")]
        ByDistanceSelection,

        [Description("A")]
        ByArea,

        [Description("AA")]
        AdditiveByArea,

        [Description("AAP")]
        AdditiveByAreaPlus,

        [Description("AN")]
        ByAngle,

        [Description("AAN")]
        AdditiveByAngle,

        [Description("AD")]
        AdditiveByDistance,

        [Description("AAA")]
        AdditiveByAreaAngle,

        [Description("V")]
        Visvalingam,

        [Description("RDP")]
        DouglasPeucker,

        [Description("L")]
        Lang,

        // 1400.05.11
        // http://psimpl.sourceforge.net/reumann-witkam.html
        [Description("RW")]
        Reumann_Witkam,

        // 1400.05.11
        // http://psimpl.sourceforge.net/perpendicular-distance.html
        [Description("PD")]
        PerpendicularDistance,

        // 1400.05.20
        [Description("NOPW")]
        NormalOpeningWindow,

        // 1400.05.20
        [Description("BOPW")]
        BeforeOpeningWindow
    }
}
