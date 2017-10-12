using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Ket.Common.Model.Esri
{
    //public class EsriJsonPoint
    //{
    //    public double? x { get; set; }

    //    public double? y { get; set; }

    //    public double? z { get; set; }

    //    public double? m { get; set; }


    //    public   string AsWkt()
    //    {
    //        var xValue = x.ToStringOrNull(false);

    //        var yValue = y.ToStringOrNull(false);

    //        if (string.IsNullOrEmpty(xValue) || string.IsNullOrEmpty(yValue))
    //        {
    //            return "POINT EMPTY";
    //        }

    //        var mValue = m.ToStringOrNull(false);
    //        var zValue = z.ToStringOrNull(mValue.Length > 0);

    //        if (string.IsNullOrEmpty(zValue) && string.IsNullOrEmpty(mValue))
    //        {
    //            return FormattableString.Invariant($"POINT({xValue} {yValue})");
    //        }
    //        else
    //        {
    //            return FormattableString.Invariant($"POINT({xValue} {yValue} {zValue} {mValue})");
    //        }

    //    }
    //}



}
