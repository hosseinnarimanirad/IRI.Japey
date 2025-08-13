
using IRI.Maptor.Sta.Persistence.DataSources;
using IRI.Maptor.Sta.Common.Helpers;
using IRI.Maptor.Jab.Common;
using IRI.Maptor.Jab.Common.Models;
using IRI.Maptor.Jab.Common.Enums;

namespace IRI.Maptor.Jab.IranRepo;

public static class NccRepository
{
    public static VectorLayer? GetLayer(string layerName, string layerTitle, VisualParameters visualParameters, LabelParameters? label)
    {
        var jsonString = ZipFileHelper.OpenAndReadAsString("iriRepo.dll", layerName);

        if (jsonString == null)
            return null;

        var features = OrdinaryJsonListSource.CreateFromJsonString<NccPoint>(jsonString, i => i.AsFeature()/*, p => p.Name*/);

        var vectorLayer = new VectorLayer(layerTitle, features, visualParameters, LayerType.VectorLayer, RenderingApproach.Default, RasterizationApproach.GdiPlus, ScaleInterval.All)
        {
            ShowInToc = false,
            CanUserDelete = false,
        };

        if (label is not null)
        {
            vectorLayer.Labels = label;
        }

        return vectorLayer;
    }
}
