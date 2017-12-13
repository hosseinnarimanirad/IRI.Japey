using System.Windows.Media;

namespace IRI.Jab.Cartography.Model.Symbology
{
    public interface ISymbol
    {
        double Opacity { get; set; }
        Brush Stroke { get; set; }
        double StrokeThickness { get; set; }
    }
}