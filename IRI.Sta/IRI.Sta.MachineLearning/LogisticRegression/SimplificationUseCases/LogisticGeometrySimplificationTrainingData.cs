using IRI.Msh.Common.Extensions;
using IRI.Msh.Common.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRI.Sta.MachineLearning.LogisticRegressionUseCases
{
    public class LogisticGeometrySimplificationTrainingData<T> where T : IPoint
    {
        public int GetNumberOfFeatures()
        {
            if (Records.IsNullOrEmpty())
            {
                return 0;
            }

            return Records.First().GetSelectedFeatures().Count;
        }

        public List<LogisticGeometrySimplificationParameters<T>> Records { get; set; }

        public LogisticGeometrySimplificationTrainingData()
        {
            Records = new List<LogisticGeometrySimplificationParameters<T>>();
        }

        public void Save(string fileName)
        {
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(this);

            System.IO.File.WriteAllText(fileName, jsonString);
        }

        public static LogisticGeometrySimplificationTrainingData<Point> Load(string fileName)
        {
            var jsonString = System.IO.File.ReadAllText(fileName);

            return Newtonsoft.Json.JsonConvert.DeserializeObject<LogisticGeometrySimplificationTrainingData<Point>>(jsonString);
        }
    }
}
