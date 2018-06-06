using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using spatial = IRI.Msh.Common.Primitives;

namespace IRI.MainProjectWPF
{
    /// <summary>
    /// Interaction logic for KdTreeWindow.xaml
    /// </summary>
    public partial class CopyOfKdTreeWindow : Window
    {
        List<spatial.Point> points = new List<spatial.Point>();

        public CopyOfKdTreeWindow()
        {
            InitializeComponent();
        }

        private void canvas1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Point newPoint = e.GetPosition(this.canvas1);

            this.points.Add(new spatial.Point(newPoint.X, newPoint.Y));

            Referesh();
        }

        private void Referesh()
        {
            //spatial.Primitives.Boundary b = IRI.Ket.Spatial.PointSorting.PointOrdering.GetBoundary(points.ToArray(), 2);


            //******************************************* 01
            DoTheJob(this.points.ToArray(), this.canvas1, this.title1, "K-d Tree (Simple)");

            //******************************************* 02
            spatial.Point[] result = IRI.Ket.Spatial.PointSorting.PointOrdering.GraySorter(this.points.ToArray());

            DoTheJob(result, this.canvas2, this.title2, "K-d Tree (Gray)");

            //******************************************* 03
            result = IRI.Ket.Spatial.PointSorting.PointOrdering.NOrderingSorter(this.points.ToArray());

            DoTheJob(result, this.canvas3, this.title3, "K-d Tree (NOrdering)");

            //******************************************* 04
            result = IRI.Ket.Spatial.PointSorting.PointOrdering.ZOrderingSorter(this.points.ToArray());

            DoTheJob(result, this.canvas4, this.title4, "K-d Tree (ZOrdering)");

            //******************************************* 05
            result = IRI.Ket.Spatial.PointSorting.PointOrdering.HilbertSorter(this.points.ToArray());

            DoTheJob(result, this.canvas5, this.title5, "K-d Tree (Hilbert)");

            //******************************************* 06
            result = IRI.Ket.Spatial.PointSorting.PointOrdering.DiagonalLebesgueSorter(this.points.ToArray());

            DoTheJob(result, this.canvas6, this.title6, "K-d Tree (Variant of Lebesgue)");

            //******************************************* 07
            result = IRI.Ket.Spatial.PointSorting.PointOrdering.PeanoSorter(this.points.ToArray());

            DoTheJob(result, this.canvas7, this.title7, "K-d Tree (Variant of Peano)");

            //******************************************* 08
            result = IRI.Ket.Spatial.PointSorting.PointOrdering.Peano02Sorter(this.points.ToArray());

            DoTheJob(result, this.canvas8, this.title8, "K-d Tree (Variant of Peano)");

            //******************************************* 09
            //result = IRI.Ket.Spatial.PointSorting.PointOrdering.HosseinSorter(this.points.ToArray());

            //DoTheJob(result, this.canvas9, this.title9, "K-d Tree (Hossein's SFC)");
        }

        private void DoTheJob(spatial.Point[] points, Canvas canvas, Label label, string labelString)
        {
            canvas.Children.Clear();

            Func<spatial.Point, spatial.Point, int> simpleFuncX = (p1, p2) => p1.X.CompareTo(p2.X);
            Func<spatial.Point, spatial.Point, int> simpleFuncY = (p1, p2) => p1.Y.CompareTo(p2.Y);
            List<Func<spatial.Point, spatial.Point, int>> comparers = new List<Func<spatial.Point, spatial.Point, int>> { simpleFuncX, simpleFuncY };

            int height = 0;

            IRI.Ket.DataStructure.AdvancedStructures.KdTree<spatial.Point> kdTree02 =
              new IRI.Ket.DataStructure.AdvancedStructures.KdTree<spatial.Point>(points, comparers);

            Draw(canvas, kdTree02.Root, 0, 0, canvas.ActualHeight, 0, canvas.ActualWidth, ref height);

            label.Content = string.Format("{0}; Height={1}", labelString, height.ToString());
        }

        private void Draw(Canvas canvas, IRI.Ket.DataStructure.AdvancedStructures.KdTreeNode<spatial.Point> node, int level, double y1, double y2, double x1, double x2, ref int height)
        {
            height = Math.Max(height, level);

            bool xBased = (level % 2 == 0);

            if (node.LeftChild != null)
            {
                if (xBased)
                {
                    Draw(canvas, node.LeftChild, level + 1, y1, y2, x1, node.Point.X, ref height);
                }
                else
                {
                    Draw(canvas, node.LeftChild, level + 1, y1, node.Point.Y, x1, x2, ref height);
                }

            }
            if (node.RigthChild != null)
            {
                if (xBased)
                {
                    Draw(canvas, node.RigthChild, level + 1, y1, y2, node.Point.X, x2, ref  height);
                }
                else
                {
                    Draw(canvas, node.RigthChild, level + 1, node.Point.Y, y2, x1, x2, ref height);
                }
            }

            Line line = new Line() { StrokeThickness = 1, Stroke = new SolidColorBrush(Colors.Blue) };

            double x = node.Point.X; double y = node.Point.Y;

            if (xBased)
            {
                line.X1 = x; line.X2 = x; line.Y1 = y1; line.Y2 = y2;
            }
            else
            {
                line.X1 = x1; line.X2 = x2; line.Y1 = y; line.Y2 = y;
            }

            RectangleGeometry geometry = new RectangleGeometry(new Rect(x - 2, y - 2, 4, 4));

            Path rectangle = new Path() { Data = geometry, Fill = new SolidColorBrush(Colors.Red) };

            canvas.Children.Add(line);

            canvas.Children.Add(rectangle);
        }

        private void canvas1_MouseMove(object sender, MouseEventArgs e)
        {
            Point currentPoint = e.GetPosition(this.canvas1);

            //this.location.Content = string.Format("X:{0}, Y:{1}", currentPoint.X, currentPoint.Y);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            this.points.Clear();

            this.canvas1.Children.Clear();
            this.canvas2.Children.Clear();
            this.canvas3.Children.Clear();
            this.canvas4.Children.Clear();
            this.canvas5.Children.Clear();
            this.canvas6.Children.Clear();
            this.canvas7.Children.Clear();
            this.canvas8.Children.Clear();
            //this.canvas9.Children.Clear();
        }
    }
}
