using IRI.Msh.Algebra;
using System;
using System.Collections.Generic;
using System.Text;

namespace IRI.Ket.MachineLearning.LogisticRegression
{

    // David Cox in 1958
    // The logistic regression is a form of supervised learning
    public class LogisticRegression
    {
        public LogisticRegression()
        {

        }

        public void Learn(Matrix xValues, Vector yValues)
        {
            // x: n*p
            // y: n*1

            var numberOfRow = xValues.NumberOfRows;
            var numberOfParameters = xValues.NumberOfColumns;


        }
    }
}
