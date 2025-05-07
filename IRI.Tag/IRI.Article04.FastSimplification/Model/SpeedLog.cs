using IRI.Sta.Common.Primitives;
using IRI.Sta.Spatial.Analysis;
using IRI.Sta.Spatial.Primitives;

namespace IRI.Article04.FastSimplification;

public class SpeedLog
{
    public string FileName { get; set; }

    public string MethodName { get; set; }

    public double DistanceThreshold { get; set; }

    public double AreaThreshold { get; set; }

    public int NumberOfFeatures { get; set; }

    public int NumberOfPoints_Original { get; set; }

    public int NumberOfPoints_Simplified { get; set; }

    public long ElapsedMilliseconds { get; set; }
     
    public SpeedLog(
        string fileName,
        string methodName,
        List<Geometry<Point>> originalGeometries,
        List<Geometry<Point>> simplifiedGeometries,
        long elapsedMilliseconds,
        SimplificationParamters parameters)
    {
        this.FileName = fileName;
        
        this.MethodName = methodName;

        this.DistanceThreshold = parameters.DistanceThreshold ?? 0;

        this.AreaThreshold = parameters.AreaThreshold ?? 0;

        this.NumberOfFeatures = originalGeometries.Count;

        this.NumberOfPoints_Original = originalGeometries.Sum(f => f.TotalNumberOfPoints);

        this.NumberOfPoints_Simplified = simplifiedGeometries.Sum(f => f.TotalNumberOfPoints);
          
        this.ElapsedMilliseconds = elapsedMilliseconds;
    }

    public static string GetHeader()
    {
        var headers = new List<string>()
        {
            nameof(FileName),
            nameof(MethodName),
            nameof(DistanceThreshold),
            nameof(AreaThreshold),
            nameof(NumberOfFeatures),
            nameof(NumberOfPoints_Original),
            nameof(NumberOfPoints_Simplified),
            nameof(ElapsedMilliseconds),
        };

        return string.Join("\t", headers);
    }

    public string ToTsv()
    {
        var headers = new List<string>()
        {
            FileName,
            MethodName,
            DistanceThreshold.ToString(),
            AreaThreshold.ToString(),
            NumberOfFeatures.ToString(),
            NumberOfPoints_Original.ToString(),
            NumberOfPoints_Simplified.ToString(),
            ElapsedMilliseconds.ToString(),
        };

        return string.Join("\t", headers);
    }
}
