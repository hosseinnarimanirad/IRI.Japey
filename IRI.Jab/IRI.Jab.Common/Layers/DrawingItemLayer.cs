﻿using System;
using System.Linq;
using System.Collections.Generic;

using IRI.Extensions;
using IRI.Jab.Common.Model;
using IRI.Sta.Common.Primitives;
using IRI.Sta.Spatial.Primitives;
using IRI.Jab.Common.Model.Legend;
using IRI.Sta.Persistence.DataSources;

using Geometry = IRI.Sta.Spatial.Primitives.Geometry<IRI.Sta.Common.Primitives.Point>;
using IRI.Sta.Common.Abstrations;
using IRI.Jab.Common.Enums;
using IRI.Sta.Persistence.Abstractions;

namespace IRI.Jab.Common;

public class DrawingItemLayer : VectorLayer, IIdentifiable
{
    public Action<DrawingItemLayer>? RequestHighlightGeometry;

    public Action<DrawingItemLayer>? RequestChangeVisibility;


    public int Id { get; set; }

    public Guid HighlightGeometryKey { get; private set; }

    public Geometry Geometry
    {
        get => Feature.TheGeometry;
        //set
        //{
        //    _geometry = value;
        //    RaisePropertyChanged();

        //    this.DataSource = new MemoryDataSource(new List<Geometry<Point>>() { value });
        //    this.Extent = value.GetBoundingBox();
        //}
    }

    private Feature<Point> _feature;

    public Feature<Point> Feature
    {
        get { return _feature; }
        set
        {
            _feature = value;
            RaisePropertyChanged();

            this.DataSource = new MemoryDataSource([_feature]);

            this.Extent = value.TheGeometry.GetBoundingBox();
        }
    }


    //public VectorDataSource/*<Feature<Point>>*/? OriginalSource { get; set; }

    public SpecialPointLayer? SpecialPointLayer { get; set; }


    private DrawingItemLayer(string layerName, int id, RasterizationApproach rasterizationApproach)
    {
        this.Id = id;
        this.LayerName = layerName;
        this.Rendering = RenderingApproach.Default;
        this.ToRasterTechnique = rasterizationApproach;
        this.ZIndex = int.MaxValue;
        this.HighlightGeometryKey = Guid.NewGuid();
    }

    //internal DrawingItemLayer(
    //    string layerName,
    //    Geometry geometry,
    //    VisualParameters? visualParameters = null,
    //    int id = int.MinValue,
    //    VectorDataSource<Feature<Point>, Point>? source = null) : this(layerName, id, RasterizationApproach.DrawingVisual)
    //{
    //    this.Extent = geometry.GetBoundingBox();

    //    this.VisualParameters = visualParameters ?? VisualParameters.GetDefaultForDrawingItems();

    //    this.OriginalSource = source;

    //    this.Geometry = geometry;

    //    var featureType =
    //        (geometry.Type == GeometryType.Point || geometry.Type == GeometryType.MultiPoint) ? LayerType.Point :
    //        ((geometry.Type == GeometryType.LineString || geometry.Type == GeometryType.MultiLineString) ? LayerType.Polyline :
    //        (geometry.Type == GeometryType.Polygon || geometry.Type == GeometryType.MultiPolygon) ? LayerType.Polygon : LayerType.None);

    //    this.Type = LayerType.Drawing | featureType;

    //    this.Commands = new List<ILegendCommand>();

    //    this.OnIsSelectedInTocChanged += (sender, e) =>
    //    {
    //        this.RequestHighlightGeometry?.Invoke(this);
    //    };

    //    this.OnVisibilityChanged += (sender, e) =>
    //    {
    //        this.RequestChangeVisibility?.Invoke(this);
    //    };

    //    //HighlightGeometryKey = Guid.NewGuid();

    //    //this.Rendering = RenderingApproach.Default;

    //    //this.ToRasterTechnique = RasterizationApproach.GdiPlus;

    //    //this.Id = id;

    //    //this.ZIndex = int.MaxValue;

    //    //this.LayerName = layerName;
    //}

    public bool IsSpecialLayer() => SpecialPointLayer != null;

    public bool CanShowHighlightGeometry() => this.IsSelectedInToc && VisualParameters?.Visibility == System.Windows.Visibility.Visible;

    public static DrawingItemLayer? Create(
        string layerName,
        Feature<Point> feature,
        VisualParameters? visualParameters = null,
        int id = int.MinValue)//,
                              //IVectorDataSource/*<Feature<Point>>*/? source = null)
    {
        if (feature is null || feature.TheGeometry.IsNotValidOrEmpty())
            return null;

        DrawingItemLayer result = new DrawingItemLayer(layerName, id, RasterizationApproach.DrawingVisual);

        //result.Extent = geometry.GetBoundingBox();

        result.VisualParameters = visualParameters ?? VisualParameters.GetDefaultForDrawingItems();

        result.Symbolizers = [new Cartography.Symbologies.SimpleSymbolizer(result.VisualParameters)];

        //result.OriginalSource = source;

        result.Feature = feature;

        //// 7/9/2025
        //// possibly error prone!
        //if (source is not null)
        //    result.DataSource = source;

        var geometryType = feature.TheGeometry.Type;

        var featureType =
            (geometryType == GeometryType.Point || geometryType == GeometryType.MultiPoint) ? LayerType.Point :
            ((geometryType == GeometryType.LineString || geometryType == GeometryType.MultiLineString) ? LayerType.Polyline :
            (geometryType == GeometryType.Polygon || geometryType == GeometryType.MultiPolygon) ? LayerType.Polygon : LayerType.None);

        result.Type = LayerType.Drawing | featureType;

        result.Commands = new List<ILegendCommand>();

        result.OnIsSelectedInTocChanged += (sender, e) =>
        {
            result.RequestHighlightGeometry?.Invoke(result);
        };

        result.OnVisibilityChanged += (sender, e) =>
        {
            result.RequestChangeVisibility?.Invoke(result);
        };

        result.OnLayerNameChanged += (sender, e) =>
        {
            feature.Attributes[feature.LabelAttribute] = e.Arg;
        };

        return result;
    }

    public static DrawingItemLayer CreateSpecialLayer(
        string layerName,
        List<Locateable> locateables,
        int id = int.MinValue)
    {
        DrawingItemLayer result = new DrawingItemLayer(layerName, id, RasterizationApproach.DrawingVisual)
        {
            Extent = BoundingBox.CalculateBoundingBox(locateables.Select(l => new Point() { X = l.Location.X, Y = l.Location.Y })),

            VisualParameters = VisualParameters.GetDefaultForDrawingItems(),

            Type = LayerType.MoveableItem,
        };

        result.SpecialPointLayer = new SpecialPointLayer(layerName, locateables, .8, null, LayerType.MoveableItem) { ParentLayerId = result.LayerId };

        return result;
    }
}
