using IRI.Msh.Common.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRI.Msh.Common.Analysis
{
    public class AreaStatistics
    {
        public AreaStatistics(List<Geometry> geometries)
        {
            this.NumberOfPoints = geometries.Sum(g => g.TotalNumberOfPoints);

            this.Areas = SpatialUtility.GetPrimitiveAreas(geometries);
        }

        public AreaStatistics(Geometry geometry)
        {
            this.NumberOfPoints = geometry.TotalNumberOfPoints;

            this.Areas = SpatialUtility.GetPrimitiveAreas(geometry);            
        }

        internal int NumberOfPoints;

        public readonly List<double> Areas;

        public string Description
        {
            get { return GetDescription(); }
        }

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

            return IRI.Msh.Statistics.Statistics.CalculateStandardDeviation(Areas);
        }

        public override string ToString()
        {
            if (this.NumberOfPoints == 0)
            {
                return "EMPTY STAT";
            }

            return $"St.Dev.: {GetStandardDeviation()}, AvgArea: {Areas.Average()}, Number of Points: {NumberOfPoints}, MinArea: {Areas.Min()}, MaxArea: {Areas.Max()}";
        }

        public string ToCSV()
        {
            if (this.NumberOfPoints == 0)
            {
                return "EMPTY STAT";
            }

            return $"{GetStandardDeviation()}; {Areas.Average()}; {NumberOfPoints}; {Areas.Min()}; {Areas.Max()}";
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
