using System;
using System.Collections.Generic;
using System.Text;

namespace IRI.Sta.Spatial.Symbology;

public interface ISymbol
{
    double Opacity { get; set; }

    string HexStroke { get; set; }

    double StrokeThickness { get; set; }
}
