using System.Windows.Media;

namespace IRI.Jab.Common.Model.Symbology;

public interface ISymbol
{
    double Opacity { get; set; }
    Brush Stroke { get; set; }
    double StrokeThickness { get; set; }
}