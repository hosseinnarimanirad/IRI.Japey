using System;
using System.Collections.Generic;
using System.Text;

namespace IRI.Sta.MachineLearning.LogisticRegressionUseCases
{
    public class LogisticGeometrySimplificationTrainingData
    {
        public List<LogisticGeometrySimplificationParameters> Records { get; set; }

        public void Save(string fileName)
        {
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(this);

            System.IO.File.WriteAllText(fileName, jsonString);
        }
    }
}
