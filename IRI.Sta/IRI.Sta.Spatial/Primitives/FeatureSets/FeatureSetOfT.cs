﻿using IRI.Extensions;
using IRI.Sta.Common.Abstrations;
using IRI.Sta.Common.Primitives;

namespace IRI.Sta.Spatial.Primitives;

public class FeatureSet<T> where T : IPoint, new()
{
    public Guid LayerId { get; set; }

    public string Title { get; set; }

    public int Srid { get; set; }

    public List<Field> Fields { get; set; }

    public List<Feature<T>> Features { get; set; }

    public BoundingBox Extent => BoundingBox.GetMergedBoundingBox(Features.Select(f => f.TheGeometry.GetBoundingBox()));

    //public FeatureSet(List<Feature<T>> features)
    //{
    //    this.Features = features;

    //    this.Fields = new List<Field>();

    //    Srid = features.SkipWhile(f => f is null || f.TheGeometry.IsNotValidOrEmpty())?.FirstOrDefault()?.TheGeometry.Srid ?? 0;
    //}

    protected FeatureSet() { }

    public FeatureSet<T>? FilterByGeometry(Predicate<Geometry<T>> predicate)
    {
        var filteredFeatures = Features.Where(f => predicate(f.TheGeometry)).ToList();

        if (filteredFeatures.IsNullOrEmpty())
            return null;

        var result = Create(string.Empty, filteredFeatures);

        result.Fields = this.Fields;

        return result;
    }

    // todo: add geometry type, srid, ... checkes
    public void Add(Feature<T> feature)
    {
        this.Features.Add(feature);
    }

    public void Remove(Feature<T> feature)
    {
        this.Features.Remove(feature);
    }

    public void Update(Feature<T> newFeature)
    {
        var index = Features.IndexOf(Features.FirstOrDefault(f => f.Id == newFeature.Id));

        if (index < 0)
            return;

        Features[index] = newFeature;
    }

    public static FeatureSet<T> Create(string title, List<Feature<T>> features)
    {
        if (features.IsNullOrEmpty())
            throw new NotImplementedException("FeatureSet<TGeometry, TPoint> => empty features not allowed");

        if (features.Select(f => f.TheGeometry.Srid).Distinct().Count() > 1)
            throw new NotImplementedException("FeatureSet<TGeometry, TPoint> => same SRID rule violated");

        return new FeatureSet<T>()
        {
            Title = title,
            Features = features,
            Fields = new List<Field>(),
            Srid = features.SkipWhile(f => f is null || f.TheGeometry.IsNotValidOrEmpty())?.FirstOrDefault()?.TheGeometry.Srid ?? 0,
            
        };
    }

}
