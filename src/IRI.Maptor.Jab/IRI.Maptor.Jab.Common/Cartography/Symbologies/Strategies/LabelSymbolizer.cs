using IRI.Maptor.Jab.Common.Models;

namespace IRI.Maptor.Jab.Common.Cartography.Symbologies;

public class LabelSymbolizer : SymbolizerBase
{
    public override SymbologyType Type =>  SymbologyType.Label;
     
    private LabelParameters _labels;

    public LabelParameters Labels
    {
        get { return _labels; }
        set
        {
            _labels = value;
            RaisePropertyChanged();

            //this.OnLabelChanged?.Invoke(this, new CustomEventArgs<LabelParameters>(value));
        }
    }

    public LabelSymbolizer(LabelParameters labels)
    {
        _labels = labels;
    } 
}
