using IRI.Maptor.Jab.Common.Cartography.Common;
using IRI.Maptor.Sta.Common.Primitives;
using IRI.Maptor.Sta.Spatial.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Maptor.Jab.Common.Cartography.Symbologies;

public abstract class SymbolizerBase : Notifier, ISymbolizer
{
    public abstract SymbologyType Type { get; }

    public double? MinScaleDenominator { get; set; }

    public double? MaxScaleDenominator { get; set; }

    private Func<Feature<Point>, bool> _filter = f => true;
    public virtual Func<Feature<Point>, bool> IsFilterPassed
    {
        get => _filter;
        protected set => _filter = value;
    }

    public virtual Predicate<double> ScalePredicate => s => true;

    //public abstract VisualParameters Get( );

    public bool IsInScaleRange(double scale)
    {
        return scale > (MinScaleDenominator ?? 0) &&
                scale < (MaxScaleDenominator ?? double.MaxValue);
    }
}
