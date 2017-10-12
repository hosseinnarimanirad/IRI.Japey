using IRI.Ket.ShapefileFormat.EsriType;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IRI.MainProjectWPF.LargeData.Utilities
{
    public static class ShapeUtility
    {
        public static EsriPoint[] AdditiveSimplifyByArea(EsriPoint[] points, double threshold)
        {
            if (points == null || points.Length == 0)
            {
                return null;
            }
            else if (points.Length == 2)
            {
                return points;
            }

            List<int> filtered = new List<int>();

            filtered.Add(0);

            int firstIndex = 0, secondIndex = 1, thirdIndex = 2;

            double tempArea = 0;

            while (thirdIndex < points.Length)
            {
                var area = GetSignedArea(points[firstIndex], points[secondIndex], points[thirdIndex]);

                tempArea += area;

                if (Math.Abs(tempArea) > threshold)
                {
                    tempArea = 0;

                    filtered.Add(secondIndex);

                    firstIndex = secondIndex;
                }

                secondIndex = thirdIndex;

                thirdIndex = thirdIndex + 1;
            }

            if (filtered.Count == 1)
            {
                filtered.Add(filtered.Count / 2);
            }

            filtered.Add(points.Length - 1);

            var output1 = filtered.Select(i => points[i]).ToArray();
            //if (!output1[0].Equals(output1[output1.Length - 1]))
            //{

            //}
            return output1;
        }

        public static EsriPoint[] AdditiveSimplifyByAreaPlus(EsriPoint[] points, double threshold)
        {
            if (points == null || points.Length == 0)
            {
                return null;
            }
            else if (points.Length == 2)
            {
                return points;
            }

            List<int> filtered = new List<int>();

            filtered.Add(0);

            int firstIndex = 0, secondIndex = 1, thirdIndex = 2;

            var totalAreaCoef = CalculateArea(points) ? 1 : -1;

            double tempArea = 0;

            while (thirdIndex < points.Length)
            {
                var area = GetSignedArea(points[firstIndex], points[secondIndex], points[thirdIndex]);

                tempArea += area;

                if (Math.Abs(tempArea) > threshold || (area * totalAreaCoef > 0))
                {
                    tempArea = 0;

                    filtered.Add(secondIndex);

                    firstIndex = secondIndex;
                }

                secondIndex = thirdIndex;

                thirdIndex = thirdIndex + 1;
            }

            if (filtered.Count == 1)
            {
                filtered.Add(filtered.Count / 2);
            }

            filtered.Add(points.Length - 1);

            var output1 = filtered.Select(i => points[i]).ToArray();
            //if (!output1[0].Equals(output1[output1.Length - 1]))
            //{

            //}
            return output1;
        }

        //public static EsriPoint[] WiseTransform(EsriPoint[] points, double threshold)
        //{
        //    if (points == null || points.Length == 0)
        //    {
        //        return null;
        //    }
        //    else if (points.Length == 2)
        //    {
        //        return points;
        //    }

        //    List<int> areas = new List<int>();

        //    areas.Add(0);

        //    int firstIndex = 0, secondIndex = 1, thirdIndex = 2;

        //    while (thirdIndex < points.Length)
        //    {
        //        var area = GetArea(points[firstIndex], points[secondIndex], points[thirdIndex]);

        //        if (area > threshold)
        //        {
        //            areas.Add(secondIndex);

        //            firstIndex = secondIndex;
        //        }

        //        secondIndex = thirdIndex;

        //        thirdIndex = thirdIndex + 1;
        //    }

        //    areas.Add(points.Length - 1);

        //    var output1 = areas.Select(i => points[i]).ToArray();

        //    return output1;
        //}

        public static EsriPoint[] SimplifyByArea(EsriPoint[] points, double threshold)
        {
            if (points == null || points.Length == 0)
            {
                return null;
            }
            else if (points.Length == 2)
            {
                return points;
            }

            List<Tuple<int, double>> areas = new List<Tuple<int, double>>();

            for (int i = 0; i < points.Length - 2; i++)
            {
                var area = GetArea(points[i], points[i + 1], points[i + 2]);

                areas.Add(new Tuple<int, double>(i + 1, area));
            }

            var filter1 = areas.Where(i => i.Item2 > threshold).Select(i => i.Item1).ToList();

            filter1.Insert(0, 0);

            filter1.Add(points.Length - 1);

            var output1 = filter1.Select(i => points[i]).ToArray();

            return output1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="points"></param>
        /// <param name="threshold">Must be between 0 and 1</param>
        /// <returns></returns>
        internal static EsriPoint[] SimplifyByAngle(EsriPoint[] points, double threshold)
        {
            if (points == null || points.Length == 0)
            {
                return null;
            }
            else if (points.Length == 2)
            {
                return points;
            }

            List<Tuple<int, double>> angles = new List<Tuple<int, double>>();

            for (int i = 0; i < points.Length - 2; i++)
            {
                var angle = GetCosineOfAngle(points[i], points[i + 1], points[i + 2]);

                angles.Add(new Tuple<int, double>(i + 1, angle));
            }

            //When i.Item2 ~ 1 it means points are on a line
            var filter1 = angles.Where(i => i.Item2 < threshold).Select(i => i.Item1).ToList();

            filter1.Insert(0, 0);

            filter1.Add(points.Length - 1);

            var output1 = filter1.Select(i => points[i]).ToArray();

            return output1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="points"></param>
        /// <param name="threshold">Must be between 0 and 1</param>
        /// <returns></returns>
        internal static EsriPoint[] AdditiveSimplifyByAngle(EsriPoint[] points, double threshold)
        {
            if (points == null || points.Length == 0)
            {
                return null;
            }
            else if (points.Length == 2)
            {
                return points;
            }

            List<int> filtered = new List<int>();

            filtered.Add(0);

            int firstIndex = 0, secondIndex = 1, thirdIndex = 2;

            //double tempAngle = 0;

            while (thirdIndex < points.Length)
            {
                var angle = GetCosineOfAngle(points[firstIndex], points[secondIndex], points[thirdIndex]);

                //tempAngle += angle;

                if (Math.Abs(angle) < threshold)
                {
                    //tempAngle = 0;

                    filtered.Add(secondIndex);

                    firstIndex = secondIndex;
                }

                secondIndex = thirdIndex;

                thirdIndex = thirdIndex + 1;
            }

            if (filtered.Count == 1)
            {
                filtered.Add(filtered.Count / 2);
            }

            filtered.Add(points.Length - 1);

            var output1 = filtered.Select(i => points[i]).ToArray();

            //if (!output1[0].Equals(output1[output1.Length - 1]))
            //{

            //}
            return output1;
        }

        internal static EsriPoint[] AdditiveSimplifyByDistance(EsriPoint[] points, double threshold)
        {
            if (points == null || points.Length == 0)
            {
                return null;
            }
            else if (points.Length == 2)
            {
                return points;
            }

            List<int> filtered = new List<int>();

            filtered.Add(0);

            int firstIndex = 0, secondIndex = 1;

            var temp = 0.0;

            while (secondIndex < points.Length)
            {
                var semiDistance = GetSemiDistance(points[firstIndex], points[secondIndex]);

                temp += semiDistance;

                if (temp > threshold)
                {
                    temp = 0;

                    filtered.Add(secondIndex);

                    firstIndex = secondIndex;
                }

                secondIndex++;
            }

            if (filtered.Count == 1)
            {
                filtered.Add(filtered.Count / 2);
            }

            filtered.Add(points.Length - 1);

            var output1 = filtered.Select(i => points[i]).ToArray();

            return output1;
        }



        public static double GetSemiDistance(EsriPoint first, EsriPoint second)
        {
            var dx = first.X - second.X;

            var dy = first.Y - second.Y;

            return dx * dx + dy * dy;
        }

        public static bool CalculateArea(EsriPoint[] points)
        {
            double area = 0;

            for (int i = 0; i < points.Length-1; i++)
            {
                double temp = points[i].X * points[i + 1].Y - points[i].Y * points[i + 1].X;

                area += temp;
            }

            return area > 0;
        }

        public static double[] GetAreas(EsriPoint[] points)
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

        public static double GetArea(EsriPoint p1, EsriPoint p2, EsriPoint p3)
        {
            return Math.Abs(p1.X * (p2.Y - p3.Y) + p2.X * (p3.Y - p1.Y) + p3.X * (p1.Y - p2.Y)) / 2.0;
        }

        public static double GetSignedArea(EsriPoint p1, EsriPoint p2, EsriPoint p3)
        {
            return (p1.X * (p2.Y - p3.Y) + p2.X * (p3.Y - p1.Y) + p3.X * (p1.Y - p2.Y)) / 2.0;
        }


        public static double GetCosineOfAngle(EsriPoint p1, EsriPoint p2, EsriPoint p3)
        {
            if (p1.Equals(p2) || p2.Equals(p3))
            {
                return 1;
            }

            //cos(theta) = (A.B)/(|A|*|B|)
            var ax = p3.X - p2.X;
            var ay = p3.Y - p2.Y;

            var bx = p2.X - p1.X;
            var by = p2.Y - p1.Y;

            var dotProduct = ax * bx + ay * by;

            var result = dotProduct * dotProduct / ((ax * ax + ay * ay) * (bx * bx + by * by));

            if (double.IsNaN(result))
            {

            }

            return result;
        }

       

        public static double[] GetCosineOfAngles(EsriPoint[] points)
        {
            if (points == null || points.Length == 0 || points.Length == 2)
                return null;

            double[] result = new double[points.Length - 2];

            for (int i = 0; i < points.Length - 2; i++)
            {
                result[i] = GetCosineOfAngle(points[i], points[i + 1], points[i + 2]);
            }

            return result;
        }




    }
}
