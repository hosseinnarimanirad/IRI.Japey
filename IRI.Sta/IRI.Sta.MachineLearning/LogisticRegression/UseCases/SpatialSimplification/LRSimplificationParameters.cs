﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

using IRI.Extensions;
using IRI.Sta.Spatial.Analysis; 
using IRI.Sta.Common.Abstrations;

namespace IRI.Sta.MachineLearning;

public class LRSimplificationParameters<T> where T : IPoint, new()
{
    //public const int NumberOfFeatures = 4;

    /// <summary>
    /// in screen scale
    /// </summary>
    //public const double MinVerticalSquareDistanceThreshold = 0.1;

    //[JsonProperty]
    public double? DistanceToPrevious { get; private set; }

    //[JsonProperty]
    public double? DistanceToNext { get; private set; }

    //[JsonProperty]
    public double? Area { get; private set; }

    //[JsonProperty]
    public double? SquareCosineOfAngle { get; private set; }

    //[JsonProperty]
    public double? CosineOfAngle { get; private set; }

    //[JsonProperty]
    public double? VerticalDistance { get; private set; }

    //[JsonProperty]
    public double? BaseLength { get; private set; }


    //[JsonProperty]
    public double? dX12 { get; private set; }
    //[JsonProperty]
    public double? dX13 { get; private set; }
    //[JsonProperty]
    public double? dX23 { get; private set; }

    //[JsonProperty]
    public double? dY12 { get; private set; }
    //[JsonProperty]
    public double? dY13 { get; private set; }
    //[JsonProperty]
    public double? dY23 { get; private set; }


    //[JsonProperty]
    public List<LRSimplificationFeatures> Features { get; set; }

    [JsonIgnore]
    private List<double> _featureValues;

    [JsonIgnore]
    public List<double> FeatureValues
    {
        get
        {
            if (_featureValues.IsNullOrEmpty() || _featureValues.Count != Features?.Count)
                InitFeatureValues();

            return _featureValues;
        }
    }

    private void InitFeatureValues()
    {
        if (Features.IsNullOrEmpty())
            return;

        var result = new List<double>();

        foreach (var item in this.Features)
        {
            var value = item switch
            {
                LRSimplificationFeatures.DistanceToNext => this.DistanceToNext,
                LRSimplificationFeatures.DistanceToPrevious => this.DistanceToPrevious,
                LRSimplificationFeatures.VerticalDistance => this.VerticalDistance,
                LRSimplificationFeatures.BaseLength => this.BaseLength,
                LRSimplificationFeatures.CosineOfAngle => this.CosineOfAngle,
                LRSimplificationFeatures.SquareCosineOfAngle => this.SquareCosineOfAngle,
                LRSimplificationFeatures.Area => this.Area,
                LRSimplificationFeatures.dX12 => this.dX12,
                LRSimplificationFeatures.dX13 => this.dX13,
                LRSimplificationFeatures.dX23 => this.dX23,
                LRSimplificationFeatures.dY12 => this.dY12,
                LRSimplificationFeatures.dY13 => this.dY13,
                LRSimplificationFeatures.dY23 => this.dY23,
                _ => throw new NotImplementedException()
            };

            result.Add(value.Value);
        }

        //if (Features.Contains(LRSimplificationFeatures.DistanceToNext))
        //    result.Add(this.DistanceToNext);

        //if (Features.Contains(LRSimplificationFeatures.DistanceToPrevious))
        //    result.Add(this.DistanceToPrevious);

        //if (Features.Contains(LRSimplificationFeatures.VerticalDistance))
        //    result.Add(this.VerticalDistance);

        //if (Features.Contains(LRSimplificationFeatures.BaseLength))
        //    result.Add(this.BaseLength);

        //if (Features.Contains(LRSimplificationFeatures.CosineOfAngle))
        //    result.Add(this.CosineOfAngle);

        //if (Features.Contains(LRSimplificationFeatures.SquareCosineOfAngle))
        //    result.Add(this.SquareCosineOfAngle);

        //if (Features.Contains(LRSimplificationFeatures.Area))
        //    result.Add(this.Area);

        _featureValues = result;
    }

    //[JsonProperty]
    public bool IsRetained { get; set; }

    public LRSimplificationParameters()
    {
        if (Features.IsNullOrEmpty())
        {
            Features = new List<LRSimplificationFeatures>();
        }
    }

    public LRSimplificationParameters(T first, T middle, T last, List<LRSimplificationFeatures> features, Func<T, T> toScreenMap = null) : this()
    {
        var firstScreenPoint = toScreenMap == null ? first : toScreenMap(first);
        var middleScreenPoint = toScreenMap == null ? middle : toScreenMap(middle);
        var lastScreenPoint = toScreenMap == null ? last : toScreenMap(last);

        this.Features = features;

        if (Features.Contains(LRSimplificationFeatures.Area))
            this.Area = SpatialUtility.GetUnsignedTriangleArea(firstScreenPoint, middleScreenPoint, lastScreenPoint);

        if (Features.Contains(LRSimplificationFeatures.DistanceToNext))
            this.DistanceToNext = SpatialUtility.GetEuclideanDistance(middleScreenPoint, lastScreenPoint);

        if (Features.Contains(LRSimplificationFeatures.DistanceToPrevious))
            this.DistanceToPrevious = SpatialUtility.GetEuclideanDistance(middleScreenPoint, firstScreenPoint);

        if (Features.Contains(LRSimplificationFeatures.CosineOfAngle))
            this.CosineOfAngle = SpatialUtility.GetCosineOfOuterAngle(firstScreenPoint, middleScreenPoint, lastScreenPoint);

        if (Features.Contains(LRSimplificationFeatures.SquareCosineOfAngle))
            this.SquareCosineOfAngle = SpatialUtility.GetSquareCosineOfAngle(firstScreenPoint, middleScreenPoint, lastScreenPoint);
        //this.SquareCosineOfAngle = CosineOfAngle * CosineOfAngle;

        if (Features.Contains(LRSimplificationFeatures.VerticalDistance))
            this.VerticalDistance = SpatialUtility.GetPointToLineSegmentDistance(firstScreenPoint, lastScreenPoint, middleScreenPoint);

        if (Features.Contains(LRSimplificationFeatures.BaseLength))
            this.BaseLength = SpatialUtility.GetEuclideanDistance(firstScreenPoint, lastScreenPoint);

        if (Features.Contains(LRSimplificationFeatures.dX12))
            this.dX12 = Math.Abs(middleScreenPoint.X - firstScreenPoint.X);

        if (Features.Contains(LRSimplificationFeatures.dX13))
            this.dX13 = Math.Abs(lastScreenPoint.X - firstScreenPoint.X);

        if (Features.Contains(LRSimplificationFeatures.dX23))
            this.dX23 = Math.Abs(lastScreenPoint.X - middleScreenPoint.X);

        if (Features.Contains(LRSimplificationFeatures.dY12))
            this.dY12 = Math.Abs(middleScreenPoint.Y - firstScreenPoint.Y);

        if (Features.Contains(LRSimplificationFeatures.dY13))
            this.dY13 = Math.Abs(lastScreenPoint.Y - firstScreenPoint.Y);

        if (Features.Contains(LRSimplificationFeatures.dY23))
            this.dY23 = Math.Abs(lastScreenPoint.Y - middleScreenPoint.Y);
    }

    //public LRSimplificationParameters(
    //    double? distanceToPrevious, 
    //    double? distanceToNext, 
    //    double? area, 
    //    double? squareCosineOfAngle, 
    //    double? cosineOfAngle, 
    //    double? verticalDistance, 
    //    double? baseLength, 
    //    List<LRSimplificationFeatures> features, 
    //    bool isRetained)
    //{
    //    DistanceToPrevious = distanceToPrevious;
    //    DistanceToNext = distanceToNext;
    //    Area = area;
    //    SquareCosineOfAngle = squareCosineOfAngle;
    //    CosineOfAngle = cosineOfAngle;
    //    VerticalDistance = verticalDistance;
    //    BaseLength = baseLength;
    //    Features = features;
    //    IsRetained = isRetained;

    //    InitFeatureValues();
    //}

    public void UpdateArea(double newArea)
    {
        this.Area = newArea;

        InitFeatureValues();
    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();

        for (int i = 0; i < FeatureValues.Count; i++)
        {
            sb.AppendLine($"{Features[i]}: {FeatureValues[i]:N4}");
        }

        return sb.ToString();

        //return $"DistanceToNext: {DistanceToNext} \n DistanceToPrevious: {DistanceToPrevious} \n Area: {Area} \n SquareCosineOfAngle: {SquareCosineOfAngle} \n CosineOfAngle: {CosineOfAngle} \n VerticalDistance: {VerticalDistance} \n BaseLength: {BaseLength}";
    }
}
