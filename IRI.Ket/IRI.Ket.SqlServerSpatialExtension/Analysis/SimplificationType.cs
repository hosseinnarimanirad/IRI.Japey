using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Ket.SqlServerSpatialExtension.Analysis
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
    }
}
