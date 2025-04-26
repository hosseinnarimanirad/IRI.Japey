using IRI.Msh.Common.Model.GeoJson;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Sta.Common.Helpers;

public static class IOHelper
{
    private const char csvDelimiterChar = ',';

    private const char tsvDelimiterChar = '\t';

    /// <summary>
    /// Depth-first recursive delete, with handling for descendant 
    /// directories open in Windows Explorer.
    /// </summary>
    public static void DeleteDirectory(string path)
    {
        foreach (string directory in Directory.GetDirectories(path))
        {
            DeleteDirectory(directory);
        }

        try
        {
            Directory.Delete(path, true);
        }
        catch (IOException)
        {
            Directory.Delete(path, true);
        }
        catch (UnauthorizedAccessException)
        {
            Directory.Delete(path, true);
        }
    }

    public static bool TryCreateDirectory(string path)
    {
        try
        {
            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }

            return true;
        }
        catch
        {
            return false;
        }
    }

    public static string GetExtensionWithoutDot(string path)
    {
        return System.IO.Path.GetExtension(path).Replace(".", "");
    }

    public static List<string[]> ReadAllDelimitedFile(string fileName, params char[] delimited)
    {
        if (!File.Exists(fileName))
        {
            return new List<string[]>();
        }

        var lines = System.IO.File.ReadAllLines(fileName);

        var result = new List<string[]>();

        foreach (var line in lines)
        {
            result.Add(line.Split(delimited));
        }

        return result;
    }

    public static List<string[]> ReadAsCsv(string fileName)
    {
        return ReadAllDelimitedFile(fileName, csvDelimiterChar);
    }

    public static List<string[]> ReadAsTsv(string fileName)
    {
        return ReadAllDelimitedFile(fileName, tsvDelimiterChar);
    }

    public static GeoJsonFeatureSet DelimitedToPointGeoJson(string fileName, bool userFirstLineAsHeader, params char[] delimited)
    {
        var rawData = ReadAllDelimitedFile(fileName, delimited);

        List<GeoJsonFeature> result = new List<GeoJsonFeature>();

        int startIndex = 0;

        List<string> header = new List<string>();

        if (userFirstLineAsHeader)
        {
            startIndex = 1;

            header = rawData[0].Skip(2).ToList();
        }
        else
        {
            header = Enumerable.Range(1, rawData[0].Length - 2).Select(i => $"header {i}").ToList();
        }

        for (int i = startIndex; i < rawData.Count; i++)
        {
            double longitude = double.Parse(rawData[i][0]);

            double latitude = double.Parse(rawData[i][1]);

            Dictionary<string, object> dictionary = new Dictionary<string, object>();

            for (int p = 2; p < rawData[i].Length; p++)
            {
                dictionary.Add(header[p - 2], rawData[i][p]);
            }

            result.Add(new GeoJsonFeature()
            {
                Geometry = GeoJsonPoint.Create(longitude, latitude),
                Geometry_name = $"point {i}",
                Id = i.ToString(),
                Type = GeoJson.Point,
                Properties = dictionary
            });
        }

        return new GeoJsonFeatureSet() { Features = result, TotalFeatures = result.Count, Type = "FeatureSet" };
    }

    public static GeoJsonFeatureSet CsvToPointGeoJson(string fileName, bool userFirstLineAsHeader)
    {
        return DelimitedToPointGeoJson(fileName, userFirstLineAsHeader, csvDelimiterChar);
    }

    public static GeoJsonFeatureSet TsvToPointGeoJson(string fileName, bool userFirstLineAsHeader)
    {
        return DelimitedToPointGeoJson(fileName, userFirstLineAsHeader, tsvDelimiterChar);
    }
}
