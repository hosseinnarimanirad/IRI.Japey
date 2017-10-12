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
using sfc = IRI.Ket.Spatial.Primitives;
using spatial = IRI.Ham.SpatialBase;

namespace IRI.MainProjectWPF
{
    /// <summary>
    /// Interaction logic for MTF.xaml
    /// </summary>
    public partial class MTF : UserControl
    {
        bool isChoosen = false;

        List<IRI.Ket.Spatial. Primitives.Transform> transforms = new List<IRI.Ket.Spatial. Primitives.Transform>();

        List<sfc.Move> moves;

        public MTF()
        {
            InitializeComponent();
        }

        public void SetMoves(List<sfc.Move> moves)
        {
            this.moves = moves;

            Refresh();
        }

        public void Reset()
        {
            this.layoutCanvas.Children.Clear();

            this.moves.Clear();

            this.transforms.Clear();
        }

        public void ChangeState(bool isChoosenState)
        {
            isChoosen = isChoosenState;

            if (isChoosen == true)
            {
                this.scaleTransform.ScaleX = 1.2;
                this.scaleTransform.ScaleY = 1.2;
                this.Opacity = 1;
            }
            else
            {
                this.scaleTransform.ScaleX = 1;
                this.scaleTransform.ScaleY = 1;
                this.Opacity = .7;
            }
        }

        public void AddTransform(IRI.Ket.Spatial. Primitives.Transform transform)
        {
            this.transforms.Add(transform);

            Refresh();
        }

        public void RemoveTransform(IRI.Ket.Spatial. Primitives.Transform transform)
        {
            this.transforms.Remove(transform);

            Refresh();
        }

        public int TransformCount { get { return this.transforms.Count; } }

        public sfc.Transform GetTrasnform(int index)
        {
            return this.transforms[index];
        }

        public sfc.Transform GetCombinedTransform()
        {
            if (this.transforms.Count < 1)
            {
                return sfc.Transforms.DoNothing;
            }
            else if (this.transforms.Count == 1)
            {
                return this.transforms[0];
            }
            else
            {
                sfc.Transform result = this.transforms[0];

                for (int i = 1; i < this.transforms.Count; i++)
                {
                    result = sfc.Transforms.CompositeTransform(result, this.transforms[i]);
                }

                return result;
            }
        }

        private void Refresh()
        {
            this.layoutCanvas.Children.Clear();

            List<spatial.Point> points = this.GetPoints();

            for (int i = 1; i < points.Count; i++)
            {
                Line line = new Line()
                {
                    Stroke = new SolidColorBrush(Colors.Black),
                    StrokeThickness = 15,
                    X1 = points[i - 1].X * 45 + 15,
                    X2 = points[i].X * 45 + 15,
                    Y1 = points[i - 1].Y * 45 + 15,
                    Y2 = points[i].Y * 45 + 15,
                    StrokeEndLineCap = PenLineCap.Triangle,
                    StrokeStartLineCap = PenLineCap.Round,
                    Opacity = .7,
                    Tag = "line"
                };

                this.layoutCanvas.Children.Add(line);

                Canvas.SetZIndex(line, 0);
            }

        }

        private spatial.Point GetStartPoint(sfc.Move[] moves)
        {
            spatial.Point point = new spatial.Point(0, 0);

            double deltaX = 0, deltaY = 0;

            for (int i = 0; i < moves.Length - 1; i++)
            {
                point = moves[i](point, 1);

                deltaX = Math.Min(point.X, deltaX);

                deltaY = Math.Min(point.Y, deltaY);
            }

            return new spatial.Point(-deltaX, -deltaY);
        }

        private sfc.Move[] GetMoves(List<sfc.Move> moves)
        {
            List<sfc.Move> result = new List<sfc.Move>();

            foreach (sfc.Move item in moves)
            {
                sfc.Move move = item;

                for (int i = 0; i < this.transforms.Count; i++)
                {
                    move = this.transforms[i](move);
                }

                result.Add(move);
            }

            return result.ToArray();
        }

        private List<spatial.Point> GetPoints()
        {
            sfc.Move[] movements = GetMoves(this.moves);

            spatial.Point startPoint = GetStartPoint(movements);

            List<spatial.Point> result = new List<spatial.Point>();

            result.Add(startPoint);

            for (int i = 0; i < movements.Length; i++)
            {
                startPoint = movements[i](startPoint, 1);

                result.Add(startPoint);
            }

            return result;
        }
    }
}
