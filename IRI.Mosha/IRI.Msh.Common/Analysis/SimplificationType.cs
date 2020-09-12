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

        Visvalingham,
        DouglasPeucker
    }
}
