using IRI.Maptor.Jab.Common.Assets.Commands;

namespace IRI.Maptor.Jab.Common.Model.Legend;

public interface ILegendCommand
{
    RelayCommand Command { get; set; }

    string PathMarkup { get; set; }

    bool IsEnabled { get; set; }

    string ToolTip { get; /*set;*/ }

    ILayer Layer { get; set; }

    bool IsCommandVisible { get; set; }
}
