using System.Windows.Media;

namespace IRI.Jab.Cartography.Model.Symbol
{
    public interface ISymbol
    {
        double Opacity { get; set; }
        Brush Stroke { get; set; }
        double StrokeThickness { get; set; }
    }
}