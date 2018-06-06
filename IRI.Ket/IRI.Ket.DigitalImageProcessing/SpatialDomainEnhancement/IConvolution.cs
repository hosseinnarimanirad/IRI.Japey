// besmellahe rahmane rahim
// Allahomma ajjel le-valiyek al-faraj

using System;
using System.Collections.Generic;
using System.Text;
using IRI.Msh.Algebra;

namespace IRI.Ket.DigitalImageProcessing
{
    public interface IConvolution
    {
        double Sigma{get;}

        Matrix Convolve(Matrix original);

    }
}
