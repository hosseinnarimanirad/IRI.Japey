using IRI.Msh.Common.Analysis;
using IRI.Extensions;
using IRI.Msh.Common.Primitives;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;


namespace IRI.Sta.MachineLearning;

public class LRSimplificationParameters<T> where T : IPoint, new()
{
    //public const int NumberOfFeatures = 4;

    /// <summary>
    /// in screen scale
    /// </summary>
    //public const double MinVerticalSquareDistanceThreshold = 0.1;

    public double? DistanceToPrevious { get; private set; }

    public double? DistanceToNext { get; private set; }

    public double? Area { get; private set; }

    public double? SquareCosineOfAngle { get; private set; }

    public double? CosineOfAngle { get; private set; }

    public double? VerticalDistance { get; private set; }

    public double? BaseLength { get; private set; }

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
    }

    public void UpdateArea(double newArea)
    {
        this.Area = newArea; 

        InitFeatureValues();
    }

    public override string ToString()
    {
        return $"DistanceToNext: {DistanceToNext} \n DistanceToPrevious: {DistanceToPrevious} \n Area: {Area} \n SquareCosineOfAngle: {SquareCosineOfAngle} \n CosineOfAngle: {CosineOfAngle} \n VerticalDistance: {VerticalDistance} \n BaseLength: {BaseLength}";
    }
}
