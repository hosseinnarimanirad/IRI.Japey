//using System;
//using System.Collections.Generic;
//using System.Collections;
//using System.Text;

//namespace GRDAnalysis.Business
//{
//    public static class ImageProcessing
//    {

//        public static double[,] HorizontalDifference(double[,] grayValues)
//        {

//            double[,] result = new double[grayValues.GetLength(0), grayValues.GetLength(1) - 1];

//            for (int i = 0; i < grayValues.GetLength(0); i++)
//            {
//                for (int j = 0; j < grayValues.GetLength(1) - 1; j++)
//                {
//                    result[i, j] = grayValues[i, j] - grayValues[i, j + 1];

//                    if (result[i, j] == double.NaN)
//                        result[i, j] = 0;
//                }

//            }

//            return result;
//        }

//        public static double[,] VerticalDifference(double[,] grayValues)
//        {

//            double[,] result = new double[grayValues.GetLength(0) - 1, grayValues.GetLength(1)];

//            for (int i = 0; i < grayValues.GetLength(0) - 1; i++)
//            {
//                for (int j = 0; j < grayValues.GetLength(1); j++)
//                {
//                    result[i, j] = grayValues[i, j] - grayValues[i + 1, j];

//                    if (result[i, j] == double.NaN)
//                        result[i, j] = 0;
//                }

//            }

//            return result;
//        }

//        public static double[,] FindBreaksHorizontally(double[,] grayValues, double tolerance, out ArrayList x, out ArrayList y)
//        {
//            double[,] result = new double[grayValues.GetLength(0), grayValues.GetLength(1) - 1];

//            x = new ArrayList(); y = new ArrayList();

//            for (int i = 0; i < grayValues.GetLength(0); i++)
//            {
//                for (int j = 0; j < grayValues.GetLength(1) - 1; j++)
//                {
//                    result[i, j] = grayValues[i, j] - grayValues[i, j + 1];

//                    if (result[i, j] == double.NaN)
//                        result[i, j] = 0;

//                    if (Math.Abs(result[i, j]) > tolerance)
//                    {
//                        x.Add(j);

//                        y.Add(i);
//                    }
//                }
//            }

//            return result;
//        }

//        public static double[,] FindBreaksVertically(double[,] grayValues, double tolerance, out ArrayList x, out ArrayList y)
//        {
//            double[,] result = new double[grayValues.GetLength(0) - 1, grayValues.GetLength(1)];

//            x = new ArrayList(); y = new ArrayList();

//            for (int i = 0; i < grayValues.GetLength(0) - 1; i++)
//            {
//                for (int j = 0; j < grayValues.GetLength(1); j++)
//                {
//                    result[i, j] = grayValues[i, j] - grayValues[i + 1, j];

//                    if (result[i, j] == double.NaN)
//                        result[i, j] = 0;

//                    if (Math.Abs(result[i, j]) > tolerance)
//                    {
//                        x.Add(j);

//                        y.Add(i);
//                    }
//                }

//            }

//            return result;
//        }

//        public static double[,] FindBreaksHorizontallyBySlope(double[,] grayValues, double tolerance, out ArrayList x, out ArrayList y)
//        {
//            double[,] difference = HorizontalDifference(grayValues);

//            double[,] result = new double[difference.GetLength(0), difference.GetLength(1) - 1];

//            x = new ArrayList(); y = new ArrayList();

//            for (int i = 0; i < difference.GetLength(0); i++)
//            {
//                for (int j = 0; j < difference.GetLength(1) - 1; j++)
//                {
//                    result[i, j] = (Math.Atan(difference[i, j] / 10) - Math.Atan(difference[i, j + 1] / 10)) * 180 / Math.PI;

//                    if (result[i, j] == double.NaN)
//                        result[i, j] = 0;

//                    if (Math.Abs(result[i, j]) > tolerance)
//                    {
//                        x.Add(j);

//                        y.Add(i);
//                    }
//                }
//            }

//            return result;

//        }

//        public static double[,] FindBreaksVerticallyBySlope(double[,] grayValues, double tolerance, out ArrayList x, out ArrayList y)
//        {
//            double[,] difference = VerticalDifference(grayValues);

//            double[,] result = new double[difference.GetLength(0) - 1, difference.GetLength(1)];

//            x = new ArrayList(); y = new ArrayList();

//            for (int i = 0; i < difference.GetLength(0) - 1; i++)
//            {
//                for (int j = 0; j < difference.GetLength(1); j++)
//                {
//                    result[i, j] = (Math.Atan(difference[i, j] / 10) - Math.Atan(difference[i + 1, j] / 10)) * 180 / Math.PI;

//                    if (result[i, j] == double.NaN)
//                        result[i, j] = 0;

//                    if (Math.Abs(result[i, j]) > tolerance)
//                    {
//                        x.Add(j);

//                        y.Add(i);
//                    }
//                }

//            }

//            return result;

//        }

//        public static double[,] FindBreaksStatistically(double[,] grayValues, double tolerance, out ArrayList x, out ArrayList y)
//        {

//            double[,] result = new double[grayValues.GetLength(0) - 2, grayValues.GetLength(1) - 2];

//            x = new ArrayList(); y = new ArrayList();

//            for (int i = 1; i < grayValues.GetLength(0) - 1; i++)
//            {
//                for (int j = 1; j < grayValues.GetLength(1) - 1; j++)
//                {

//                    double[] tempValue = new double[]{grayValues[i-1,j-1],
//                                                        grayValues[i-1,j],
//                                                        grayValues[i-1,j+1],
//                                                        grayValues[i,j-1],
//                                                        grayValues[i,j],
//                                                        grayValues[i,j+1],
//                                                        grayValues[i+1,j-1],
//                                                        grayValues[i+1,j],
//                                                        grayValues[i+1,j+1]};

//                    result[i - 1, j - 1] = Mathematic.Statistic.Variance(tempValue);

//                    if (result[i - 1, j - 1] == double.NaN)
//                        result[i - 1, j - 1] = 0;

//                    if (Math.Abs(result[i - 1, j - 1]) > tolerance)
//                    {
//                        x.Add(j - 1);

//                        y.Add(i - 1);
//                    }
//                }

//            }

//            return result;

//        }

//        public static void FindGrossErrors(double[,] grayValues, out ArrayList x, out ArrayList y)
//        {

//            x = new ArrayList(); y = new ArrayList();

//            for (int i = 1; i < grayValues.GetLength(0) - 1; i++)
//            {
//                for (int j = 1; j < grayValues.GetLength(1) - 1; j++)
//                {

//                    double[] tempValue = new double[]{grayValues[i-1,j-1],
//                                                        grayValues[i-1,j],
//                                                        grayValues[i-1,j+1],
//                                                        grayValues[i,j-1],
//                                                        grayValues[i,j],
//                                                        grayValues[i,j+1],
//                                                        grayValues[i+1,j-1],
//                                                        grayValues[i+1,j],
//                                                        grayValues[i+1,j+1]};

//                    double tempVar = 2.576 * Mathematic.Statistic.Deviation(tempValue);

//                    double tempMean = Mathematic.Statistic.Mean(tempValue);

//                    if (tempVar == double.NaN || tempMean == double.NaN)
//                        continue;

//                    foreach (double item in tempValue)
//                    {
//                        if (Math.Abs(item - tempMean) > tempVar )
//                        {
//                            x.Add(j);

//                            y.Add(i);

//                            break;
//                        }
//                    }

//                }

//            }

//        }
//    }
//}
