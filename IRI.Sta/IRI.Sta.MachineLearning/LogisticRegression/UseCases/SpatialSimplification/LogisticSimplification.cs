using IRI.Msh.Algebra;
using IRI.Extensions;
using IRI.Msh.Common.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRI.Sta.MachineLearning;

public class LogisticSimplification<T> where T : IPoint, new()
{
    private LogisticRegression _regression;

    private List<LRSimplificationFeatures> _features;


    private LogisticSimplification()
    {

    }

    public static LogisticSimplification<T> Create(LRSimplificationTrainingData<T> trainingData)
    {
        if (trainingData is null || trainingData.IsNullOrEmpty())
            return null;

        // 1400.03.20 
        var countOfFeatures = trainingData.GetCountOfFeatures();

        var countOfRecords = trainingData.GetCountOfRecords();

        Matrix xValues = new Matrix(countOfRecords, countOfFeatures);

        double[] yValues = new double[countOfRecords];

        //
        //var builder = new List<string>
        //{
        //    // add header
        //    string.Join(",", trainingData.Features.Select(f => f.ToString()).Concat(["IsRetained"]))
        //};

        for (int i = 0; i < countOfRecords; i++)
        {
            // 1400.03.20
            var features = trainingData.Records[i].FeatureValues;

            for (int j = 0; j < countOfFeatures; j++)
            {
                xValues[i, j] = features[j];
            }

            yValues[i] = trainingData.Records[i].IsRetained ? 1 : 0;

            //var temp = new List<double>();
            //temp.AddRange(features);
            //temp.Add(yValues[i]);

            //builder.Add(string.Join(",", temp));
        }

        //System.IO.File.WriteAllLines("data2.txt", builder);

        var result = new LogisticSimplification<T>();

        result._regression = new LogisticRegression(new LogisticRegressionOptions() { RegularizationMethod = RegularizationMethods.L1 });

        result._regression.Fit(xValues, yValues);

        result._features = trainingData.Features;

#if DEBUG
        if (trainingData.Features.Any())
        {
            System.Diagnostics.Debug.WriteLine(" ************************ ");
            for (int i = 0; i < trainingData.Features.Count; i++)
            {
                System.Diagnostics.Debug.WriteLine($"{trainingData.Features[i]}: {result._regression.Beta[i + 1]}");
            }
            System.Diagnostics.Debug.WriteLine(" ************************ ");
        }
#endif

        return result;
    }

    public static LogisticSimplification<T> Create(
        List<T> originalPoints,
        List<T> simplifiedPoints,
        Func<T, T> toScreenMap,
        bool isRingMode,
        List<LRSimplificationFeatures> features)
    {
        var parameters = LRSimplificationTrainingData<T>.Create(originalPoints, simplifiedPoints, isRingMode, features, toScreenMap);

        return Create(parameters);
    }


    private bool? IsRetained(LRSimplificationParameters<T> parameters)
    {
        // 1400.03.20
        //List<double> xValues = new List<double>
        //{
        //    //1,
        //    //parameters.ZoomLevel,
        //    parameters.SemiDistanceToNext,
        //    parameters.SemiDistanceToPrevious,
        //    //parameters.SemiVerticalDistance,
        //    parameters.SemiArea,
        //    parameters.SquareCosineOfAngle,
        //    //parameters.CosineOfAngle
        //};

        List<double> xValues = parameters.FeatureValues;

        var result = _regression.Predict(xValues);

        if (result.HasValue)
        {
            if (result.Value > 1 || result.Value < 0)
            {

            }

            //return Math.Round(result.Value) == 1;
            return result.Value > 0.5;
        }
        else
        {
            return null;
        }
    }

    public List<T> SimplifyByLogisticRegression(List<T> points, Func<T, T> toScreenMap, bool retain3Points = false)
    {
        if (points == null || points.Count == 0)
        {
            return null;
        }
        else if (points.Count == 2)
        {
            return points;
        }

        List<T> result = new List<T>();

        var screenPoints = points.Select(p => toScreenMap(p)).ToList();

        // add first point automatically
        result.Add(points.First());

        int firstIndex = 0, middleIndex = 1, lastIndex = 2;

        while (lastIndex < points.Count)
        {
            middleIndex = lastIndex - 1;

            int secondFloating = 1;

            while (middleIndex > firstIndex)
            {
                // 1400.06.04
                // var parameters = new LogisticGeometrySimplificationParameters<T>(points[firstIndex], points[middleIndex], points[lastIndex], toScreenMap);
                var parameters = new LRSimplificationParameters<T>(screenPoints[firstIndex], screenPoints[middleIndex], screenPoints[lastIndex], _features, null);

                if (IsRetained(parameters) == true)
                {
                    result.Add(points[middleIndex]);
                    result.Add(points[lastIndex]);

                    firstIndex = lastIndex;
                    //middleIndex = lastIndex + 1;
                    //lastIndex = lastIndex + 2;

                    break;
                }
                else
                {
                    middleIndex = middleIndex - secondFloating;
                    secondFloating = secondFloating * 2;
                }
            }

            lastIndex++;
        }

        if (retain3Points && result.Count == 1)
        {
            result.Add(points[points.Count() / 2]);
        }

        result.Add(points.Last());

        return result;
    }


    public List<T> SimplifyByLogisticRegression_Fast_O_n_CumulativeArea(List<T> points, /*int zoomLevel, */Func<T, T> toScreenMap, bool retain3Points = false)
    {
        if (points == null || points.Count == 0)
        {
            return null;
        }
        else if (points.Count == 2)
        {
            return points;
        }

        List<T> result = new List<T>();

        var screenPoints = points.Select(p => toScreenMap(p)).ToList();

        // add first point automatically
        result.Add(points.First());

        int firstIndex = 0, middleIndex = 1, lastIndex = 2;

        // 1400.03.10
        var tempArea = 0.0;

        while (lastIndex < points.Count)
        {
            middleIndex = lastIndex - 1;

            var parameters = new LRSimplificationParameters<T>(screenPoints[firstIndex], screenPoints[middleIndex], screenPoints[lastIndex], _features, null);

            tempArea += parameters.Area ?? 0;
            //parameters.Area = tempArea; ;
            parameters.UpdateArea(tempArea);

            if (IsRetained(parameters) == true)
            {
                result.Add(points[middleIndex]);
                result.Add(points[lastIndex]);

                firstIndex = lastIndex;
                tempArea = 0;
            }
            else
            {

            }

            lastIndex++;
        }

        if (retain3Points && result.Count == 1)
        {
            result.Add(points[points.Count() / 2]);
        }

        result.Add(points.Last());

        return result;
    }

    public List<T> SimplifyByLogisticRegression_Fast_O_n(List<T> points, /*int zoomLevel, */Func<T, T> toScreenMap, bool retain3Points = false)
    {
        if (points == null || points.Count == 0)
        {
            return null;
        }
        else if (points.Count == 2)
        {
            return points;
        }

        List<T> result = new List<T>();

        var screenPoints = points.Select(p => toScreenMap(p)).ToList();

        // add first point automatically
        result.Add(points.First());

        int firstIndex = 0, middleIndex = 1, lastIndex = 2;

        while (lastIndex < points.Count)
        {
            middleIndex = lastIndex - 1;

            var parameters = new LRSimplificationParameters<T>(screenPoints[firstIndex], screenPoints[middleIndex], screenPoints[lastIndex], _features, null);

            if (IsRetained(parameters) == true)
            {
                result.Add(points[middleIndex]);
                result.Add(points[lastIndex]);

                firstIndex = lastIndex;
                lastIndex++;
            }

            lastIndex++;
        }


        if (retain3Points && result.Count == 1)
        {
            result.Add(points[points.Count() / 2]);
        }

        result.Add(points.Last());

        return result;
    }
}
