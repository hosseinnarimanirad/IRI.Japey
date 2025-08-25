using IRI.Maptor.Jab.Common.Cartography.Symbologies;
using IRI.Maptor.Jab.Common.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Maptor.Jab.Common;

public abstract class SymbolizableLayer : BaseLayer
{
    protected List<VisualParameters> _visualParameters = [];

    //public List<ISymbolizer> Symbolizers { get; protected set; } = [];

    private List<ISymbolizer> _symbolizers = [];

    public IReadOnlyCollection<ISymbolizer> Symbolizers
    {
        get => _symbolizers.AsReadOnly();
    }

    //public List<VisualParameters> VisualParameters
    //{
    //    get { return _visualParameters; }
    //    set
    //    {
    //        _visualParameters = value;

    //        RaisePropertyChanged();

    //        if (_visualParameters != null)
    //        {
    //            _visualParameters.OnVisibilityChanged -= RaiseVisibilityChanged;
    //            _visualParameters.OnVisibilityChanged += RaiseVisibilityChanged;
    //        }

    //    }
    //}

    public void SetSymbolizer(ISymbolizer symbolizer)
    {
        if (symbolizer.Param is not null)
        {
            symbolizer.Param.OnVisibilityChanged -= RaiseVisibilityChanged;
            symbolizer.Param.OnVisibilityChanged += RaiseVisibilityChanged;

            this._visualParameters.Add(symbolizer.Param);
        }

        this._symbolizers.Add(symbolizer);
    }

    public event EventHandler<CustomEventArgs<VisualParameters>>? OnLabelChanged;

    //public bool CanRenderLabels(double mapScale)
    //{
    //    return this.Labels?.IsLabeled(1.0 / mapScale) == true;
    //}

    public VisualParameters GetMainOrDefaultSymbology() => _symbolizers.FirstOrDefault(v => v is SimpleSymbolizer)?.Param ?? VisualParameters.CreateNew();

    public VisualParameters? GetDefaultLabelParams() => _symbolizers.FirstOrDefault(v => v is LabelSymbolizer)?.Param ?? null;
}
