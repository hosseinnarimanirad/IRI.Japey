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

        [Description("CANG")]
        CumulativeAngle,

        [Description("CED")]
        CumulativeDistance,

        [Description("CTRANG")]
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
        [Description("PD")]
        PerpendicularDistance,

        // 1400.06.24 
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
