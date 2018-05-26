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
using System.Windows.Navigation;
using System.Windows.Shapes;
using spatial = IRI.Sta.Common.Primitives;

namespace IRI.MainProjectWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<IRI.Ket.Geometry.Point> points = new List<IRI.Ket.Geometry.Point>();

        int hilbertHeight = 0, simpleHeight = 0, rbHeight = 0;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void canvas1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Point newPoint = e.GetPosition(this.canvas1);

            this.points.Add(new IRI.Ket.Geometry.Point(newPoint.X, newPoint.Y));

            Referesh();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.points.Clear();

            this.canvas1.Children.Clear();

            this.canvas2.Children.Clear();

            this.canvas3.Children.Clear();

            title1.Content = "K-d Tree (Binary Search Tree)";
            title2.Content = "K-d Tree (Red-Black Tree); Height={0}";
            title3.Content = "K-d Tree (Hilbert); Height={0}";
        }

        private void Referesh()
        {
            Ket.Spatial.Primitives.Boundary b = IRI.Ket.Spatial.PointSorting.PointOrdering.GetBoundary(points.ToArray(), 2);

            Func<IRI.Ket.Geometry.Point, IRI.Ket.Geometry.Point, int> simpleFuncX = (p1, p2) => p1.X.CompareTo(p2.X);
            Func<IRI.Ket.Geometry.Point, IRI.Ket.Geometry.Point, int> simpleFuncY = (p1, p2) => p1.Y.CompareTo(p2.Y);


            IRI.Ket.DataStructure.AdvancedStructures.KdTree<IRI.Ket.Geometry.Point> kdTree =
                new IRI.Ket.DataStructure.AdvancedStructures.KdTree<IRI.Ket.Geometry.Point>(
                                                                            points.ToArray(),
                                                                            new List<Func<IRI.Ket.Geometry.Point, IRI.Ket.Geometry.Point, int>>() { simpleFuncX, simpleFuncY });

            IRI.Ket.DataStructure.AdvancedStructures.BalancedKdTree<IRI.Ket.Geometry.Point> balancedKdTree =
                new IRI.Ket.DataStructure.AdvancedStructures.BalancedKdTree<IRI.Ket.Geometry.Point>(
                                                                            points.ToArray(),
                                                                            new List<Func<IRI.Ket.Geometry.Point, IRI.Ket.Geometry.Point, int>>() { simpleFuncX, simpleFuncY },
                                                                            new Ket.Geometry.Point(double.NaN, double.NaN),
                                                                            i => new spatial.Point(i.X, i.Y));

            canvas1.Children.Clear(); simpleHeight = 0;
            canvas2.Children.Clear(); rbHeight = 0;
            canvas3.Children.Clear(); hilbertHeight = 0;

            DrawLine(kdTree.Root, 0, 0, canvas1.ActualHeight, 0, canvas1.ActualWidth);
            DrawLine02(balancedKdTree.Root, 0, 0, canvas2.ActualHeight, 0, canvas2.ActualWidth);
            //IRI.Ket.DataStructure.AdvancedStructures.BalancedKdTree<Point> kdtree02 =
            //    new IRI.Ket.DataStructure.AdvancedStructures.BalancedKdTree<Point>(points.ToArray(), new List<Func<Point, Point, int>>() { myFunc });

            List<spatial.Point> points02 = new List<spatial.Point>();
            foreach (IRI.Ket.Geometry.Point item in points)
            {
                points02.Add(new spatial.Point(item.X, item.Y));
            }

            spatial.Point[] result = IRI.Ket.Spatial.PointSorting.PointOrdering.HilbertSorter(points02.ToArray());

            Func<spatial.Point, spatial.Point, int> simpleFuncX02 = (p1, p2) => p1.X.CompareTo(p2.X);
            Func<spatial.Point, spatial.Point, int> simpleFuncY02 = (p1, p2) => p1.Y.CompareTo(p2.Y);

            IRI.Ket.DataStructure.AdvancedStructures.KdTree<spatial.Point> kdTree02 =
              new IRI.Ket.DataStructure.AdvancedStructures.KdTree<spatial.Point>(
                                                                          result,
                                                                          new List<Func<spatial.Point, spatial.Point, int>>() { simpleFuncX02, simpleFuncY02 });

            DrawLine03(kdTree02.Root, 0, 0, canvas3.ActualHeight, 0, canvas3.ActualWidth);

            title1.Content = string.Format("K-d Tree (Binary Search Tree); Height={0}", simpleHeight.ToString());
            title2.Content = string.Format("K-d Tree (Red-Black Tree); Height={0}", rbHeight.ToString());
            title3.Content = string.Format("K-d Tree (Hilbert); Height={0}", hilbertHeight.ToString());
        }

        private void DrawLine(IRI.Ket.DataStructure.AdvancedStructures.KdTreeNode<IRI.Ket.Geometry.Point> node, int level, double y1, double y2, double x1, double x2)
        {
            simpleHeight = Math.Max(simpleHeight, level);

            bool xBased = (level % 2 == 0);

            if (node.LeftChild != null)
            {
                if (xBased)
                {
                    DrawLine(node.LeftChild, level + 1, y1, y2, x1, node.Point.X);
                }
                else
                {
                    DrawLine(node.LeftChild, level + 1, y1, node.Point.Y, x1, x2);
                }

            }
            if (node.RigthChild != null)
            {
                if (xBased)
                {
                    DrawLine(node.RigthChild, level + 1, y1, y2, node.Point.X, x2);
                }
                else
                {
                    DrawLine(node.RigthChild, level + 1, node.Point.Y, y2, x1, x2);
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

            canvas1.Children.Add(line);

            canvas1.Children.Add(rectangle);
        }

        private void DrawLine02(IRI.Ket.DataStructure.AdvancedStructures.BalancedKdTreeNode<IRI.Ket.Geometry.Point> node, int level, double y1, double y2, double x1, double x2)
        {
            rbHeight = Math.Max(rbHeight, level);
            bool xBased = (level % 2 == 0);

            if (node.LeftChild != IRI.Ket.DataStructure.AdvancedStructures.BalancedKdTree<IRI.Ket.Geometry.Point>.NilNode)
            {
                if (xBased)
                {
                    DrawLine02(node.LeftChild, level + 1, y1, y2, x1, node.Point.X);
                }
                else
                {
                    DrawLine02(node.LeftChild, level + 1, y1, node.Point.Y, x1, x2);
                }

            }
            if (node.RightChild != IRI.Ket.DataStructure.AdvancedStructures.BalancedKdTree<IRI.Ket.Geometry.Point>.NilNode)
            {
                if (xBased)
                {
                    DrawLine02(node.RightChild, level + 1, y1, y2, node.Point.X, x2);
                }
                else
                {
                    DrawLine02(node.RightChild, level + 1, node.Point.Y, y2, x1, x2);
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

            canvas2.Children.Add(line);

            canvas2.Children.Add(rectangle);
        }

        private void DrawLine03(IRI.Ket.DataStructure.AdvancedStructures.KdTreeNode<spatial.Point> node, int level, double y1, double y2, double x1, double x2)
        {
            hilbertHeight = Math.Max(hilbertHeight, level);

            bool xBased = (level % 2 == 0);

            if (node.LeftChild != null)
            {
                if (xBased)
                {
                    DrawLine03(node.LeftChild, level + 1, y1, y2, x1, node.Point.X);
                }
                else
                {
                    DrawLine03(node.LeftChild, level + 1, y1, node.Point.Y, x1, x2);
                }

            }
            if (node.RigthChild != null)
            {
                if (xBased)
                {
                    DrawLine03(node.RigthChild, level + 1, y1, y2, node.Point.X, x2);
                }
                else
                {
                    DrawLine03(node.RigthChild, level + 1, node.Point.Y, y2, x1, x2);
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

            canvas3.Children.Add(line);

            canvas3.Children.Add(rectangle);
        }

        private void canvas1_MouseMove(object sender, MouseEventArgs e)
        {
            Point currentPoint = e.GetPosition(this.canvas1);

            this.location.Content = string.Format("X:{0}, Y:{1}", currentPoint.X, currentPoint.Y);
        }
    }
}
