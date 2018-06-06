using IRI.Jab.Common;
using IRI.Ket.ShapefileFormat.EsriType;
using IRI.MainProjectWPF.LargeData.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.MainProjectWPF.LargeData.Model
{
    public class AngleStatistics : Notifier
    {
        public AngleStatistics(IEsriShapeCollection shapes)
        {
            this.Angles = new List<double>();

            this.NumberOfPoints = 0;

            Threshold = 0;

            foreach (IEsriSimplePoints item in shapes)
            {
                this.NumberOfPoints += item.Points.Length;

                var areas = ShapeUtility.GetCosineOfAngles(item.Points);

                if (areas != null && areas.Length > 0)
                {
                    this.Angles.AddRange(areas);
                }
            }

            RaisePropertyChanged("Average");
            RaisePropertyChanged("StandardDeviation");
            RaisePropertyChanged("Description");
        }

        private int _numberOfPoints;

        public int NumberOfPoints
        {
            get { return _numberOfPoints; }
            set
            {
                _numberOfPoints = value;
                RaisePropertyChanged();
            }
        }

        private List<double> _angles;

        public List<double> Angles
        {
            get { return _angles; }
            set
            {
                _angles = value;
                RaisePropertyChanged();
                RaisePropertyChanged("Description");

            }
        }

        private double _threshold;

        public double Threshold
        {
            get { return _threshold; }
            set
            {
                _threshold = value;
                RaisePropertyChanged();                
            }
        }

        public double Average
        {
            get { return Angles.Average(); }
        }

        public double StandardDeviation
        {
            get { return IRI.Msh.Statistics.Statistics.CalculateStandardDeviation(this.Angles); }
        }


        public override string ToString()
        {
            if (this.NumberOfPoints == 0)
            {
                return "EMPTY STAT";
            }

            return string.Format("Threshold: {0}, St.Dev.: {1}, AvgArea: {2}, Number of Points: {3}, MinArea: {4}, MaxArea: {5}",
                Threshold,
                StandardDeviation,
                Average,
                NumberOfPoints,
                Angles.Min(),
                Angles.Max()
                );
        }

        //public string ToCSV()
        //{
        //    if (this.NumberOfPoints == 0)
        //    {
        //        return "EMPTY STAT";
        //    }

        //    return string.Format("{0}; {1}; {2}; {3}; {4}; {5}",
        //     Threshold,
        //     GetStandardDeviation(),
        //     Angles.Average(),
        //     NumberOfPoints,
        //     Angles.Min(),
        //     Angles.Max()
        //     );
        //}

        public string Description { get { return this.GetDescription(); } }

        public string GetDescription()
        {
            return this.ToString().Replace(',', '\n');
        }

        public double[] GetHistogram(int categories)
        {
            var angles = this.Angles;

            var ordered = angles.OrderBy(i => i).ToList();

            var size = angles.Count / categories;

            return Enumerable.Range(0, categories).Select(i => ordered[(i) * size]).ToArray();
        }

    }
}
