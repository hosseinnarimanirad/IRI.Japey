﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace IRI.Article.Sfc.View
{
    /// <summary>
    /// Interaction logic for RTreeWindow.xaml
    /// </summary>
    public partial class RTreeWindow : Window
    {
        Brush[] brushes;
        public RTreeWindow()
        {
            InitializeComponent();

            brushes = new Brush[] { Brushes.Black, Brushes.Brown, Brushes.Blue, Brushes.Orange, Brushes.Red, Brushes.Green, Brushes.YellowGreen };
        }

        List<IRI.Ket.Spatial.DataStructures.Rectangle> rectangles = new List<IRI.Ket.Spatial.DataStructures.Rectangle>();

        Point old;
        bool isDragging;
        RectangleGeometry geo;
        Path rectangle = new Path() { StrokeThickness = .5, Stroke = Brushes.Aqua };

        //int hilbertHeight = 0, simpleHeight = 0, mortonHeight = 0;

        private void canvas1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            isDragging = true;
            this.old = e.GetPosition(this.canvas1);
            this.canvas1.Children.Add(rectangle);
        }

        private void canvas1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                Point temp = e.GetPosition(this.canvas1);

                this.geo = new RectangleGeometry(new Rect(this.old, temp));

                this.rectangle.Data = geo;
            }

            Point currentPoint = e.GetPosition(this.canvas1);

            this.location.Content = string.Format("X:{0}, Y:{1}", currentPoint.X, currentPoint.Y);
        }

        private void canvas1_MouseUp(object sender, MouseButtonEventArgs e)
        {
            isDragging = false;

            this.canvas1.Children.Remove(this.rectangle);

            if (geo == null)
            {
                return;
            }

            Rect rect = this.geo.Bounds;

            this.rectangles.Add(new IRI.Ket.Spatial.DataStructures.Rectangle(rect.Left, rect.Top, rect.Right, rect.Bottom));

            Referesh();
        }

        private void Referesh()
        {
            IRI.Ket.Spatial.DataStructures.RTree tree01 = new IRI.Ket.Spatial.DataStructures.RTree(this.rectangles.ToArray(), 3);

            int height = 0;
            DrawNode(tree01.Root, 0, canvas1, ref height);
            title1.Content = string.Format("R-Tree (Simple Tree); Height={0}", height.ToString());


            //************************************ 02
            IRI.Ket.Spatial.DataStructures.SFCRTree tree02 = new IRI.Ket.Spatial.DataStructures.SFCRTree(
                                                                this.rectangles.ToArray(),
                                                                IRI.Ket.Spatial.DataStructures.SFCRTree.GrayComparer,
                                                                3);
            DoTheJob(this.canvas2, tree02, this.title2, "Gray");

            //************************************ 03
            IRI.Ket.Spatial.DataStructures.SFCRTree tree03 = new IRI.Ket.Spatial.DataStructures.SFCRTree(
                                                                this.rectangles.ToArray(),
                                                                IRI.Ket.Spatial.DataStructures.SFCRTree.NOrderingComparer,
                                                                3);
            DoTheJob(this.canvas3, tree03, this.title3, "NOrdering");

            //************************************ 04
            IRI.Ket.Spatial.DataStructures.SFCRTree tree04 = new IRI.Ket.Spatial.DataStructures.SFCRTree(
                                                                this.rectangles.ToArray(),
                                                                IRI.Ket.Spatial.DataStructures.SFCRTree.ZOrderingComparer,
                                                                3);
            DoTheJob(this.canvas4, tree04, this.title4, "ZOrdering");

            //************************************ 05
            IRI.Ket.Spatial.DataStructures.SFCRTree tree05 = new IRI.Ket.Spatial.DataStructures.SFCRTree(
                                                                this.rectangles.ToArray(),
                                                                IRI.Ket.Spatial.DataStructures.SFCRTree.HilbertComparer,
                                                                3);
            DoTheJob(this.canvas5, tree05, this.title5, "Hilbert");

            //************************************ 06
            IRI.Ket.Spatial.DataStructures.SFCRTree tree06 = new IRI.Ket.Spatial.DataStructures.SFCRTree(
                                                                this.rectangles.ToArray(),
                                                                IRI.Ket.Spatial.DataStructures.SFCRTree.DiagonalLebesgueComparer,
                                                                3);
            DoTheJob(this.canvas6, tree06, this.title6, "DiagonalLebesgue");

            //************************************ 07
            IRI.Ket.Spatial.DataStructures.SFCRTree tree07 = new IRI.Ket.Spatial.DataStructures.SFCRTree(
                                                                this.rectangles.ToArray(),
                                                                IRI.Ket.Spatial.DataStructures.SFCRTree.PeanoComparer,
                                                                3);
            DoTheJob(this.canvas7, tree07, this.title7, "Variant of Peano");

            //************************************ 08
            IRI.Ket.Spatial.DataStructures.SFCRTree tree08 = new IRI.Ket.Spatial.DataStructures.SFCRTree(
                                                                this.rectangles.ToArray(),
                                                                IRI.Ket.Spatial.DataStructures.SFCRTree.Peano02Comparer,
                                                                3);
            DoTheJob(this.canvas8, tree08, this.title8, "Variant of Peano");

            //************************************ 09
            IRI.Ket.Spatial.DataStructures.SFCRTree tree09 = new IRI.Ket.Spatial.DataStructures.SFCRTree(
                                                                this.rectangles.ToArray(),
                                                                IRI.Ket.Spatial.DataStructures.SFCRTree.Peano03Comparer,
                                                                3);
            DoTheJob(this.canvas9, tree09, this.title9, "Variant of Peano");
        }

        private void DoTheJob(Canvas canvas, IRI.Ket.Spatial.DataStructures.SFCRTree tree, Label label, string labelString)
        {
            canvas.Children.Clear();

            int height = 0;

            DrawNode(tree.Root, 0, canvas, ref height);

            label.Content = string.Format("R-Tree ({0}); Height={1}", labelString, height);
        }

        private void DrawNode(IRI.Ket.Spatial.DataStructures.RTreeNode rTreeNode, int level, Canvas canvas, ref int height)
        {
            height = Math.Max(height, level);

            if (rTreeNode.IsLeaf)
            {
                DrawRectangle(rTreeNode.Boundary, brushes[level], canvas);

                foreach (IRI.Ket.Spatial.DataStructures.Rectangle item in rTreeNode.GetSubRectangles())
                {
                    DrawRectangle(item, brushes[level + 1], canvas);
                }
            }
            else
            {
                DrawRectangle(rTreeNode.Boundary, brushes[level], canvas);

                for (int i = 0; i < rTreeNode.NumberOfKeys; i++)
                {
                    DrawNode(rTreeNode.GetPointer(i), level + 1, canvas, ref height);
                }
            }

        }

        private void DrawRectangle(IRI.Ket.Spatial.DataStructures.Rectangle rectangle, Brush brush, Canvas canvas)
        {
            RectangleGeometry geometry = new RectangleGeometry(new Rect(rectangle.minX, rectangle.minY, rectangle.Width, rectangle.Height));

            Path temp = new Path() { StrokeThickness = 1, Stroke = brush, Data = geometry };

            //this.canvas1.Children.Add(temp);
            canvas.Children.Add(temp);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
