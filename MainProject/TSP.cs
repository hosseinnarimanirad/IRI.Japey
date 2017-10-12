using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRI.MainProject
{
    public static class TSP
    {

        public static void Calculate()
        {
            double[,] weights = GetGraph();

            List<int> myList = new List<int>();

            int n = weights.GetLength(0);

            for (int i = 0; i < n; i++)
            {
                myList.Add(i);
            }

            List<string> subset = GetSubsets(myList, 1, 0);

            //Dictionary<Tuple<string, int>, double> A0 = new Dictionary<Tuple<string, int>, double>();
            double[,] A0 = new double[(int)Math.Pow(2, n), n];


            for (int i = 0; i < n; i++)
            {
                //A0.Add(new Tuple<string, int>(subset[i], 0), i == 0 ? 0.0 : double.PositiveInfinity);
                A0[GetIndex(subset[i], 1), 0] = double.PositiveInfinity;
            }

            A0[0, 0] = 0;

            //Dictionary<Tuple<string, int>, double> A1 = new Dictionary<Tuple<string, int>, double>();
            double[,] A1 = new double[1, 1];


            for (int m = 2; m <= n; m++)
            {
                List<string> subSets = GetSubsets(myList, m, 1);

                A1 = new double[(int)Math.Pow(2, n), n];

                foreach (string item in subSets)
                {
                    string[] nodes = item.Split(',');

                    int position = nodes[0].Length;

                    for (int i = 1; i < nodes.Length; i++)
                    {
                        string S = item.Remove(position, nodes[i].Length + 1);

                        position += (nodes[i].Length + 1);

                        int j = int.Parse(nodes[i]);

                        double temp = double.PositiveInfinity;

                        for (int k = 0; k < nodes.Length; k++)
                        {
                            int index = int.Parse(nodes[k]);

                            if (index == j)
                            {
                                continue;
                            }

                            //if (!A0.ContainsKey(new Tuple<string, int>(S, index)))
                            //{
                            //    continue;
                            //}
                            int sIndex = GetIndex(S, m);

                            if (temp > A0[sIndex, index] + weights[index, j])
                            {
                                temp = A0[sIndex, index] + weights[index, j];
                            }
                        }

                        A1[GetIndex(item, m), j] = temp;
                    }
                }

                //A0 = A1;
            }

            double result = double.PositiveInfinity;

            string totalS = GetSubsets(myList, myList.Count, 0)[0];

            for (int j = 1; j < n; j++)
            {
                //if (!A1.ContainsKey(new Tuple<string, int>(totalS, j)))
                //{
                //    continue;
                //}
                if (result > A1[GetIndex(totalS, n), j] + weights[j, 0])
                {
                    result = A1[GetIndex(totalS, n), j] + weights[j, 0];
                }
            }

            //var result = GetSubsets(new List<int>() { 0, 1, 2, 3, 4 }, 3,0);
            //var result = GetSubsets(new List<int>() { 0, 1, 2, 3, 4 }, 3).Distinct().ToList();
        }

        private static double[,] GetGraph()
        {
            string fileName = @"C:\Users\Hossein\Desktop\tsp0.txt";

            string[] lines = System.IO.File.ReadAllLines(fileName);

            int n = int.Parse(lines[0]);

            double[,] result = new double[n, n];

            double[] x = new double[n];
            double[] y = new double[n];

            for (int i = 0; i < n; i++)
            {
                string[] temp = lines[i + 1].Split(' ');

                x[i] = double.Parse(temp[0]);
                y[i] = double.Parse(temp[1]);
            }

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    double dx = (x[i] - x[j]);
                    double dy = (y[i] - y[j]);

                    result[i, j] = Math.Sqrt(dx * dx + dy * dy);
                }
            }

            return result;
        }

        static int startIndex = -1;

        public static List<string> GetSubsets(List<int> values, int m, int startIndex)
        {

            List<string> result = new List<string>();

            if (m == 1)
            {
                for (int i = 0; i < values.Count; i++)
                {
                    result.Add(i.ToString());
                }
            }
            else if (values.Count == m)
            {
                result.Add(string.Join(",", values));
            }
            else
            {
                for (int i = startIndex; i < values.Count; i++)
                {
                    List<int> sub = values.Where(input => !input.Equals(values[i])).ToList();

                    result.AddRange(GetSubsets(sub, m, i));
                }

                startIndex++;
            }

            return result;
        }

        public static int GetIndex(string zeroOnes, int length)
        {
            string[] temp = zeroOnes.Split(',');

            int result = 0;

            for (int i = 0; i < temp.Length; i++)
            {
                result += (int)Math.Pow(2, int.Parse(temp[i]));
            }

            return result;
        }
    }

   
}
