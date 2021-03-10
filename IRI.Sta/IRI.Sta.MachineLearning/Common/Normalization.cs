﻿using IRI.Msh.Algebra;
using IRI.Msh.Common.Extensions;
using IRI.Msh.Statistics;
using System;
using System.Collections.Generic;
using System.Text;

namespace IRI.Ket.MachineLearning.Common
{    
    public static class Normalization
    {
        public static Matrix NormalizeColumnsUsingZScore(Matrix values)
        {
            if (values == null)
                return null;

            int numberOfColumns = values.NumberOfColumns;

            Matrix result = new Matrix(values.NumberOfRows, values.NumberOfColumns);

            for (int i = 0; i < numberOfColumns; i++)
            {
                var columnValues = values.GetColumn(i);

                result.SetColumn(i, NormalizeUsingZScore(columnValues));
            }

            return result;
        }


        // **********************************************************************
        // Z score standardization is one of the most popular 
        // method to normalize data. In this case, we rescale 
        // an original variable to have a mean of zero and 
        // standard deviation of one.
        // **********************************************************************
        public static double[] NormalizeUsingZScore(double[] values)
        {
            if (values.IsNullOrEmpty())
            {
                return null;
            }

            double[] result = new double[values.Length];

            //values.CopyTo(result, 0);

            var mean = IRI.Msh.Statistics.Statistics.CalculateMean(values);

            var std = IRI.Msh.Statistics.Statistics.CalculateStandardDeviation(values);

            // 1399.12.12
            // to prevent divide by zero
            if (std == 0)
                std = 1;

            for (int i = 0; i < values.Length; i++)
            {
                result[i] = (values[i] - mean) / std;
            }

            return result;
        }


        // **********************************************************************
        // It is also called 0-1 scaling because the standardized 
        // value using this method lies between 0 and 1.
        // **********************************************************************
        public static double[] NormalizeUsingMinMax(double[] values)
        {
            if (values.IsNullOrEmpty())
            {
                return null;
            }

            double[] result = new double[values.Length];

            var min = Statistics.GetMin(values);

            var max = Statistics.GetMax(values);

            var range = max - min;

            // to prevent divide by zero
            if (range == 0)
                range = 1;

            for (int i = 0; i < values.Length; i++)
            {
                result[i] = (values[i] - min) / range;
            }

            return result;
        }


        // **********************************************************************
        // In this method, we divide each value by the standard deviation. 
        // The idea is to have equal variance, but different means and ranges.
        // **********************************************************************
        public static double[] NormalizeUsingStandardDeviation(double[] values)
        {
            if (values.IsNullOrEmpty())
            {
                return null;
            }

            double[] result = new double[values.Length];

            var std = Statistics.CalculateStandardDeviation(values);

            // to prevent divide by zero
            if (std == 0)
                std = 1;

            for (int i = 0; i < values.Length; i++)
            {
                result[i] = (values[i]) / std;
            }

            return result;
        }


        // **********************************************************************
        // In this method, we divide each value by the standard deviation. 
        // The idea is to have equal variance, but different means and ranges.
        // **********************************************************************
        public static double[] NormalizeUsingRange(double[] values)
        {
            if (values.IsNullOrEmpty())
            {
                return null;
            }

            double[] result = new double[values.Length];

            var min = Statistics.GetMin(values);

            var max = Statistics.GetMax(values);

            var range = max - min;

            // to prevent divide by zero
            if (range == 0)
                range = 1;

            for (int i = 0; i < values.Length; i++)
            {
                result[i] = (values[i]) / range;
            }

            return result;
        }
    }
}