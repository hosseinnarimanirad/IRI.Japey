using IRI.Ket.ShapefileFormat.EsriType;
using IRI.MainProjectWPF.LargeData.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.MainProjectWPF.LargeData.Model
{

    public struct AreaStatistics
    {
        public AreaStatistics(IEsriShapeCollection shapes)
        {
            this.Areas = new List<double>();

            this.NumberOfPoints = 0;

            Threshold = 0;

            foreach (IEsriSimplePoints item in shapes)
            {
                this.NumberOfPoints += item.Points.Length;

                var areas = ShapeUtility.GetAreas(item.Points);

                if (areas != null && areas.Length > 0)
                {
                    this.Areas.AddRange(areas);
                }
            }
        }

        //public void Init()
        //{
        //    Areas = new List<double>();
        //}

        internal int NumberOfPoints;

        internal List<double> Areas;

        internal double Threshold;

        internal double GetAverage()
        {
            if (this.NumberOfPoints == 0)
                 return 0;
  
            return Areas.Average();
        }

        public double GetStandardDeviation()
        {
            if (this.NumberOfPoints == 0)
                return 0;
  
            return IRI.Sta.Statistics.Statistics.CalculateStandardDeviation(Areas);
        }

        public override string ToString()
        {
            if (this.NumberOfPoints == 0)
            {
                return "EMPTY STAT";
            }

            return string.Format("Threshold: {0}, St.Dev.: {1}, AvgArea: {2}, Number of Points: {3}, MinArea: {4}, MaxArea: {5}",
                Threshold,
                GetStandardDeviation(),
                Areas.Average(),
                NumberOfPoints,
                Areas.Min(),
                Areas.Max()
                );
        }

        public string ToCSV()
        {
            if (this.NumberOfPoints == 0)
            {
                return "EMPTY STAT";
            }

            return string.Format("{0}; {1}; {2}; {3}; {4}; {5}",
             Threshold,
             GetStandardDeviation(),
             Areas.Average(),
             NumberOfPoints,
             Areas.Min(),
             Areas.Max()
             );
        }

        public string GetDescription()
        {
            return this.ToString().Replace(',', '\n');
        }

        public double[] GetHistogram(int categories)
        {
            var areas = this.Areas;

            var ordered = areas.OrderBy(i => i).ToList();

            var size = areas.Count / categories;

            return Enumerable.Range(0, categories).Select(i => ordered[(i) * size]).ToArray();
        }
    }


}
