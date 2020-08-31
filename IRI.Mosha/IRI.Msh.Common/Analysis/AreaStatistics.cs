using IRI.Msh.Common.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRI.Msh.Common.Analysis
{
    public class AreaStatistics
    {
        public AreaStatistics(Geometry shapes)
        {
            this.Areas = new List<double>();

            this.NumberOfPoints = 0;

            Threshold = 0;

            foreach (var item in shapes.)
            {
                this.NumberOfPoints += item.Points.Length;

                //var areas = ShapeUtility.GetAreas(item.Points);

                //if (areas != null && areas.Length > 0)
                //{
                //    this.Areas.AddRange(areas);
                //}
            }
        }

        private void Calculate(Geometry geometry)
        {
            switch (geometry.Type)
            {
                case GeometryType.Point:
                case GeometryType.MultiPoint:
                case GeometryType.GeometryCollection:
                case GeometryType.CircularString:
                case GeometryType.CompoundCurve:
                case GeometryType.CurvePolygon:
                    break;

                case GeometryType.LineString:
                    break;

                case GeometryType.Polygon:
                    break;

                case GeometryType.MultiLineString:
                    break;

                case GeometryType.MultiPolygon:
                    break;

                default:
                    break;
            }
        }

        private List<double> GetAreas(Geometry lineString)
        {
            List<double> result = new List<double>();

            if (lineString == null || lineString.NumberOfPoints == 0)
            {
                return result;
            }

            if (lineString.Type != GeometryType.LineString)
            {
                throw new NotImplementedException();
            }

            var points = lineString.GetAllPoints();

        }


        public static double[] GetAreas(IPoint[] points)
        {
            if (points == null || points.Length == 0 || points.Length == 2)
                return null;

            double[] result = new double[points.Length - 2];

            for (int i = 0; i < points.Length - 2; i++)
            {
                result[i] = GetArea(points[i], points[i + 1], points[i + 2]);
            }

            return result;
        }


        //public void Init()
        //{
        //    Areas = new List<double>();
        //}

        internal int NumberOfPoints;

        internal List<double> Areas;

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

            return string.Format("Threshold: {0}, St.Dev.: {1}, AvgArea: {2}, Number of Points: {3}, MinArea: {4}, MaxArea: {5}",
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
