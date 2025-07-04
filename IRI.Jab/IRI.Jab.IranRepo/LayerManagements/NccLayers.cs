using System.Windows.Media;

using IRI.Jab.Common;
using IRI.Jab.Common.Model;
using IRI.Jab.Common.Symbology;
using IRI.Sta.Common.Primitives;
using IRI.Sta.Spatial.Primitives;
using IRI.Jab.Common.Model.Legend;
using IRI.Jab.Common.Presenter.Map;

namespace IRI.Jab.IranRepo;

public static class NccLayers
{

    public static List<ILayer> GetLayers(MapPresenter map)
    {
        var fontFamily = new FontFamily("Times New Roman");

        var leveling1 = NccRepository.GetLayer("leveling1", "ترازیابی درجه ۱", new VisualParameters("#88A10024", "#FFA10024", 1, .9)
        {
            PointSymbol = new SimplePointSymbol(10),
            Visibility = System.Windows.Visibility.Collapsed
        });

        if (leveling1 != null)
        {
            var leveling1Labels = new LabelParameters(ScaleInterval.Create(10), 11, leveling1.VisualParameters.Stroke, fontFamily, i => i);
            leveling1.Commands = GetCommands<NccPoint>(map, leveling1, leveling1Labels);
        }



        var leveling2 = NccRepository.GetLayer("leveling2", "ترازیابی درجه ۲", new VisualParameters("#88E51400", "#FFE51400", 1, .9)
        {
            PointSymbol = new SimplePointSymbol(8),
            Visibility = System.Windows.Visibility.Collapsed
        });
        if (leveling2 != null)
        {
            var leveling2Labels = new LabelParameters(ScaleInterval.Create(10), 11, leveling2.VisualParameters.Stroke, fontFamily, i => i);
            leveling2.Commands = GetCommands<NccPoint>(map, leveling2, leveling2Labels);
        }

        var leveling3 = NccRepository.GetLayer("leveling3", "ترازیابی درجه ۳", new VisualParameters("#88FA6900", "#FFFA6900", 1, .9)
        {
            PointSymbol = new SimplePointSymbol(6),
            Visibility = System.Windows.Visibility.Collapsed
        });
        if (leveling3 != null)
        {
            var leveling3Labels = new LabelParameters(ScaleInterval.Create(10), 11, leveling3.VisualParameters.Stroke, fontFamily, i => i);
            leveling3.Commands = GetCommands<NccPoint>(map, leveling3, leveling3Labels);
        }


        var geodesy1 = (NccRepository.GetLayer("geodesy1", "ژئودزی ۱", new VisualParameters("#880050EF", "#FF0050EF", 1, .9)
        {
            PointSymbol = new SimplePointSymbol(10),
            Visibility = System.Windows.Visibility.Collapsed
        }));
        if (geodesy1 != null)
        {
            var gedesy1Labels = new LabelParameters(ScaleInterval.Create(10), 11, geodesy1.VisualParameters.Stroke, fontFamily, i => i);
            geodesy1.Commands = GetCommands<NccPoint>(map, geodesy1, gedesy1Labels);
        }

        var geodesy2 = (NccRepository.GetLayer("geodesy2", "ژئودزی ۲", new VisualParameters("#881CA1E2", "#FF1CA1E2", 1, .9)
        {
            PointSymbol = new SimplePointSymbol(8),
            Visibility = System.Windows.Visibility.Collapsed
        }));
        if (geodesy2 != null)
        {
            var geodesy2Labels = new LabelParameters(ScaleInterval.Create(10), 11, geodesy2.VisualParameters.Stroke, fontFamily, i => i);
            geodesy2.Commands = GetCommands<NccPoint>(map, geodesy2, geodesy2Labels);
        }

        var gravity = (NccRepository.GetLayer("gravity", "ثقل سنجی", new VisualParameters("#88AA00FF", "#FFAA00FF", 1, .9)
        {
            PointSymbol = new SimplePointSymbol(10),
            Visibility = System.Windows.Visibility.Collapsed
        }));
        if (gravity != null)
        {
            var gravityLabels = new LabelParameters(ScaleInterval.Create(10), 11, gravity.VisualParameters.Stroke, fontFamily, i => i);
            gravity.Commands = GetCommands<NccPoint>(map, gravity, gravityLabels);
        }

        var geodynamic = (NccRepository.GetLayer("geodynamic", "ژئودینامیک", new VisualParameters("#88A4C401", "#FFA4C401", 1, .9)
        {
            PointSymbol = new SimplePointSymbol(10),
            Visibility = System.Windows.Visibility.Collapsed
        }));
        if (geodynamic != null)
        {
            var geodynamicLabels = new LabelParameters(ScaleInterval.Create(10), 11, geodynamic.VisualParameters.Stroke, fontFamily, i => i);
            geodynamic.Commands = GetCommands<NccPoint>(map, geodynamic, geodynamicLabels);
        }

        return new List<ILayer>() { leveling1, leveling2, leveling3, geodesy1, geodesy2, gravity, geodynamic }?.Where(l => l != null).ToList();
    }

    private static List<ILegendCommand> GetCommands<T>(MapPresenter map, VectorLayer layer, LabelParameters label)
        where T : class, IGeometryAware<Point>
    {
        return new List<ILegendCommand>()
        {
            LegendCommand.CreateZoomToExtentCommand(map, layer),
            LegendCommand.CreateShowAttributeTable/*<T>*/(map,layer),
            LegendCommand.CreateSelectByDrawing/*<T>*/(map,layer),
            LegendCommand.CreateClearSelected(map,layer),
            LegendToggleCommand.CreateToggleLayerLabelCommand(map, layer, label)
        };
    }

}
