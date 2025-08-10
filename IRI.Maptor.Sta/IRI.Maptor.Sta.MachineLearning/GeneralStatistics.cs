using IRI.Maptor.Sta.DataStructures;
using IRI.Maptor.Sta.Mathematics;
using System.Linq;

namespace IRI.Maptor.Sta.MachineLearning;

public static class GeneralStatistics
{
    public static StatisticsSummary CalculateSummary(double[] values)
    {
        if (values == null || values.Length == 0)
        {
            return new StatisticsSummary();
        }

        var length = values.Length;

        var sortedValues = SortAlgorithm.MergeSort<double>(values, (a, b) => a.CompareTo(b));

        var result = new StatisticsSummary();

        result.Min = sortedValues.First();

        result.Max = sortedValues.Last();

        result.FirstQuartile = sortedValues[length / 4];

        result.Median = sortedValues[length / 2];

        result.ThirdQuartile = sortedValues[length * 3 / 4];

        result.Mean = IRI.Maptor.Sta.Mathematics.Statistics.CalculateMean(values);

        return result;
    }

}
