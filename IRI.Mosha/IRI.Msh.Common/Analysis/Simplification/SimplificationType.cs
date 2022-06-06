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
        NthPoint,

        [Description("DST")]
        DistanceSelection,

        [Description("AR")]
        Area,

        [Description("A-AR")]
        AdditiveByArea,

        [Description("AAP")]
        AdditiveByAreaPlus,

        [Description("ANG")]
        Angle,

        [Description("A-ANG")]
        AdditiveByAngle,

        [Description("A-DST")]
        AdditiveByDistance,

        [Description("A-AR-ANG")]
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

        [Description("SF")]
        SleeveFitting,

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
