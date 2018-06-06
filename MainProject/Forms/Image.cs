using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using IRI.Msh.Algebra;


namespace MainProject
{
    public partial class Image : Form
    {
        public Image()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        //private void button1_Click(object sender, EventArgs e)
        //{
        //    string fileName1 = @"E:\3.jpg";

        //    string fileName2 = @"E:\2.jpg";

        //    System.Drawing.Bitmap image = new System.Drawing.Bitmap(fileName1);

        //    Matrix values = myImaging.Conversion.GrayscaleImageToMatrixSlow(image);

        //    ScaleInvariantFeatureTransform sift = new ScaleInvariantFeatureTransform(values, 2, 2, .5, 10);

        //    sift.ConstructScaleSpace();
        //    sift.CalculateDifferenceOfGaussians();
        //    sift.FindExtermas();

        //    sift.FindExactExtermas();
        //    sift.RemoveEdgeResponses();
        //    sift.AssignOrientations();

        //    sift.CreateDescriptors();

        //    //
        //    Extermas e1 = sift.extermas;
        //    List<double[]> des1 = sift.descriptors;
        //    //

        //    image = new System.Drawing.Bitmap(fileName2);

        //    values = myImaging.Conversion.GrayscaleImageToMatrixSlow(image);

        //    sift = new ScaleInvariantFeatureTransform(values, 2, 2, .5, 10);

        //    sift.ConstructScaleSpace();
        //    sift.CalculateDifferenceOfGaussians();
        //    sift.FindExtermas();

        //    sift.FindExactExtermas();
        //    sift.RemoveEdgeResponses();
        //    sift.AssignOrientations();

        //    sift.CreateDescriptors();

        //    //
        //    Extermas e2 = sift.extermas;
        //    List<double[]> des2 = sift.descriptors;
        //    //

        //    List<KeyValuePair<int, int>> paires = new List<KeyValuePair<int, int>>();

        //    for (int i = 0; i < des1.Count; i++)
        //    {
        //        Vector v = new Vector(des1[i]);

        //        for (int j = 1; j < des2.Count; j++)
        //        {
        //            Vector tempV = v - new Vector(des2[j]);
        //            if (tempV.Norm < 0.03)
        //            {
        //                paires.Add(new KeyValuePair<int, int>(i, j));
        //                break;
        //            }
        //        }
        //    }
        //}
    }
}
