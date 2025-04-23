
using IRI.Sta.MachineLearning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Sta.MachineLearning;

public class SyntheticDataFile
{
    public List<SyntheticDataItem> LineSamples { get; set; }

    public List<LRSimplificationFeatures> Features { get; set; }

    public void Save(string fileName)
    {
        System.IO.File.WriteAllText(fileName, Newtonsoft.Json.JsonConvert.SerializeObject(this));
    }

    public static SyntheticDataFile Load(string fileName)
    {
        if (!System.IO.File.Exists(fileName))
            return null;

        var jsonString = System.IO.File.ReadAllText(fileName);

        if (string.IsNullOrWhiteSpace(jsonString))
            return null;

        return Newtonsoft.Json.JsonConvert.DeserializeObject<SyntheticDataFile>(jsonString);
    }
}
