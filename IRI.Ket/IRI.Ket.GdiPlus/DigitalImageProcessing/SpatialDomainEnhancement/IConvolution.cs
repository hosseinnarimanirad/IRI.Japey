// besmellahe rahmane rahim
// Allahomma ajjel le-valiyek al-faraj

using System;
using System.Collections.Generic;
using System.Text;
using IRI.Sta.Mathematics;

namespace IRI.Ket.DigitalImageProcessing
{
    public interface IConvolution
    {
        double Sigma{get;}

        Matrix Convolve(Matrix original);

    }
}
