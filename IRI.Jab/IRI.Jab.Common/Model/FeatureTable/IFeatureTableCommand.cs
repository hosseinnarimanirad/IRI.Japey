using IRI.Jab.Common.Assets.Commands; 

namespace IRI.Jab.Common.Model;

public interface IFeatureTableCommand
{
    RelayCommand Command { get; set; }

    string PathMarkup { get; set; }

    bool IsEnabled { get; set; }

    string ToolTip { get; set; }

    //ILayer Layer { get; set; }

    bool IsCommandVisible { get; set; }
}
