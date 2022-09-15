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

        [Description("AAP")]
        AdditiveAreaPlus,

        [Description("A-ANG")]
        CumulativeAngle,

        [Description("A-DS")]
        CumulativeDistance,

        [Description("A-AR-ANG")]
        CumulativeAreaAngle,



        [Description("ANG")]
        Angle,

        [Description("ED")]
        EuclideanDistance,


        [Description("NTH")]
        NthPoint,

        // 1401.06.24
        [Description("RPS")]
        RandomPointSelection,


        [Description("LA")]
        Lang,

        [Description("RDP")]
        RamerDouglasPeucker,

        // 1400.05.11
        // http://psimpl.sourceforge.net/reumann-witkam.html
        [Description("RW")]
        ReumannWitkam,

        // 1400.05.11
        // http://psimpl.sourceforge.net/perpendicular-distance.html
        [Description("PD")]
        PerpendicularDistance,

        // 1400.06.24
        // http://psimpl.sourceforge.net/perpendicular-distance.html
        [Description("MPD")]
        ModifiedPerpendicularDistance,


        [Description("VW")]
        VisvalingamWhyatt,

        [Description("SF")]
        SleeveFitting,

        // 1400.05.20
        [Description("NOPW")]
        NormalOpeningWindow,

        // 1400.05.20
        [Description("BOPW")]
        BeforeOpeningWindow,


        // Triangular Routine
        [Description("TR")]
        Area,

        [Description("MTR")]
        ModifiedArea,

        [Description("CTR")]
        CumulativeArea,

    }
}
