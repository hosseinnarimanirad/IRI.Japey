﻿
using IRI.Sta.Persistence.DataSources;
using IRI.Sta.Common.Helpers;
using IRI.Jab.Common;
using IRI.Jab.Common.Model;
using IRI.Jab.Common.Enums;

namespace IRI.Jab.IranRepo;

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
