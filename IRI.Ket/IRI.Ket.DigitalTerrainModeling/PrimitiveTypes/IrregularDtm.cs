﻿// besmellahe rahmane rahim
// Allahomma ajjel le-valiyek al-faraj

using System;
using System.Collections.Generic;
using System.Text;
using IRI.Ket.Geometry;
using IRI.Ket.Graph.GraphRepresentation;

namespace IRI.Ket.DigitalTerrainModeling
{
    [Serializable]
    public class IrregularDtm
    {
        private AttributedPointCollection collection;

        DelaunayTriangulation triangulation;

        QuasiTriangle currenTriangle;

        public int NumberOfPoints
        {
            get { return this.collection.Count; }
        }

        public IrregularDtm(AttributedPointCollection collection)
        {
            this.collection = collection;

            //PointCollection c = new PointCollection();

            //for (int i = 0; i < collection.Count; i++)
            //{
            //    c.Add(new Point(collection[i].X, collection[i].Y));
            //}

            this.triangulation = new DelaunayTriangulation(this.collection);

            currenTriangle = triangulation.triangles[0];
        }

        public IrregularDtm(double[] east, double[] north, double[] value)
        {
            try
            {

                this.collection = new AttributedPointCollection(new List<double>(east),
                                                                new List<double>(north),
                                                                new List<double>(value));

                triangulation = new DelaunayTriangulation(collection);

                currenTriangle = triangulation.triangles[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public double Interpolate(Point point)
        {
            if (this.collection.Contains(point))
            {
                return this.collection[this.collection.IndexOf(point)].Value;
            }

            Triangle triangle = triangulation.GetTriangle(point, ref currenTriangle);

            if (triangle == null)
            {
                currenTriangle = triangulation.triangles[0];

                return double.NaN;
            }

            double firstValue = collection.GetValue(triangle.FirstPoint);

            double secondValue = collection.GetValue(triangle.SecondPoint);

            double thirdValue = collection.GetValue(triangle.ThirdPoint);

            double dx1 = triangle.SecondPoint.X - triangle.FirstPoint.X;
            double dy1 = triangle.SecondPoint.Y - triangle.FirstPoint.Y;
            double dz1 = secondValue - firstValue;
            double dx2 = triangle.ThirdPoint.X - triangle.FirstPoint.X;
            double dy2 = triangle.ThirdPoint.Y - triangle.FirstPoint.Y;
            double dz2 = thirdValue - firstValue;

            double a = dy1 * dz2 - dy2 * dz1;

            double b = -(dx1 * dz2 - dx2 * dz1);

            double c = dx1 * dy2 - dx2 * dy1;

            double d = a * triangle.FirstPoint.X + b * triangle.FirstPoint.Y + c * firstValue;

            return 1 / c * (d - a * point.X - b * point.Y);
        }

        public AttributedPoint GetValue(int index)
        {
            return collection[index];
        }

        public double CalculateSlope(Triangle triangle)
        {
            double firstValue = collection.GetValue(triangle.FirstPoint);

            double secondValue = collection.GetValue(triangle.SecondPoint);

            double thirdValue = collection.GetValue(triangle.ThirdPoint);

            double dx1 = triangle.SecondPoint.X - triangle.FirstPoint.X;
            double dy1 = triangle.SecondPoint.Y - triangle.FirstPoint.Y;
            double dz1 = secondValue - firstValue;
            double dx2 = triangle.ThirdPoint.X - triangle.FirstPoint.X;
            double dy2 = triangle.ThirdPoint.Y - triangle.FirstPoint.Y;
            double dz2 = thirdValue - firstValue;

            double a = dy1 * dz2 - dy2 * dz1;

            double b = -(dx1 * dz2 - dx2 * dz1);

            double c = dx1 * dy2 - dx2 * dy1;

            return Math.Sqrt(a * a + b * b) / Math.Sqrt(a * a + b * b + c * c);
        }

        public double CalculateAspect(Triangle triangle)
        {
            double firstValue = collection.GetValue(triangle.FirstPoint);

            double secondValue = collection.GetValue(triangle.SecondPoint);

            double thirdValue = collection.GetValue(triangle.ThirdPoint);

            double dx1 = triangle.SecondPoint.X - triangle.FirstPoint.X;
            double dy1 = triangle.SecondPoint.Y - triangle.FirstPoint.Y;
            double dz1 = secondValue - firstValue;
            double dx2 = triangle.ThirdPoint.X - triangle.FirstPoint.X;
            double dy2 = triangle.ThirdPoint.Y - triangle.FirstPoint.Y;
            double dz2 = thirdValue - firstValue;

            double a = dy1 * dz2 - dy2 * dz1;

            double b = -(dx1 * dz2 - dx2 * dz1);

            double c = dx1 * dy2 - dx2 * dy1;

            if (a == b && a == 0)
                return 0;

            double tempValue = Math.Atan2(b, a);

            return tempValue > 0 ? tempValue : (2 * Math.PI + tempValue);
        }

        public double CalculateVolume(double baseHeight)
        {
            if (this.triangulation.triangles.Count < 1)
            {
                throw new NotImplementedException();
            }

            double result = 0;

            foreach (QuasiTriangle item in this.triangulation.triangles)
            {
                Point first = collection.GetPoint(item.First);

                Point second = collection.GetPoint(item.Second);

                Point third = collection.GetPoint(item.Third);

                Triangle temp = new Triangle(first, second, third);

                double firstValue = collection.GetValue(temp.FirstPoint);

                double secondValue = collection.GetValue(temp.SecondPoint);

                double thirdValue = collection.GetValue(temp.ThirdPoint);

                result += temp.CalculateArea() * (firstValue + secondValue + thirdValue) / 3;
            }

            return result;
        }

        public AttributedPoint LowerLeft
        {
            get
            {
                double x = this.collection[0].X;

                double y = this.collection[0].Y;

                int index = 0;

                for (int i = 1; i < this.NumberOfPoints; i++)
                {
                    if (this.collection[i].X < x || this.collection[i].Y < y)
                    {
                        x = this.collection[i].X;

                        y = this.collection[i].Y;

                        index = i;
                    }
                }

                return new AttributedPoint(x, y, this.collection[index].Value);
            }
        }

        public AttributedPoint UpperRight
        {
            get
            {
                double x = this.collection[0].X;

                double y = this.collection[0].Y;

                int index = 0;

                for (int i = 1; i < this.NumberOfPoints; i++)
                {
                    if (this.collection[i].X > x || this.collection[i].Y > y)
                    {
                        x = this.collection[i].X;

                        y = this.collection[i].Y;

                        index = i;
                    }
                }

                return new AttributedPoint(x, y, this.collection[index].Value);
            }
        }

        public PointCollection GetPointCollection()
        {
            return (PointCollection)collection;
        }

        public RegularDtm ToRegularDtm(double cellSize)
        {
            return ToRegularDtm(cellSize, cellSize);
        }

        public RegularDtm ToRegularDtm(double cellWidth, double cellHeight)
        {
            double minX = this.collection.MinX; double maX = this.collection.MaxX;

            double minY = this.collection.MinY; double maxY = this.collection.MaxY;

            int numberOfColumns = (int)Math.Ceiling((maX - minX + 1) / cellWidth);

            int numberOfRows = (int)Math.Ceiling((maxY - minY + 1) / cellHeight);

            double[,] values = new double[numberOfRows, numberOfColumns];

            for (int i = 0; i < numberOfRows; i++)
            {
                for (int j = 0; j < numberOfColumns; j++)
                {
                    Point temp = new Point(minX + j * cellWidth, minY + (numberOfRows - 1 - i) * cellHeight);

                    values[i, j] = Interpolate(temp);
                }
            }

            return new RegularDtm(values, cellWidth, cellHeight, new Point(minX, minY));
        }

        public System.Drawing.Bitmap DrawSlopeMap(int scale)
        {
            if (this.triangulation.triangles.Count < 1)
            {
                throw new NotImplementedException();
            }

            double minX = this.LowerLeft.X; double minY = this.LowerLeft.Y;

            int width = (int)(this.UpperRight.X - minX);

            int height = (int)(this.UpperRight.Y - minY);

            System.Drawing.Bitmap result = new System.Drawing.Bitmap(width / scale, height / scale);

            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(result);

            foreach (QuasiTriangle item in this.triangulation.triangles)
            {
                Point first = collection.GetPoint(item.First);

                Point second = collection.GetPoint(item.Second);

                Point third = collection.GetPoint(item.Third);

                Triangle temp = new Triangle(first, second, third);

                double tempSlope = CalculateSlope(temp);

                //int color = (int)((tempSlope + Math.PI / 2) * 255 / (Math.PI));

                int color = (int)((Math.Abs(tempSlope) * 250 / (Math.PI / 2)));

                System.Drawing.SolidBrush brush = new System.Drawing.SolidBrush(System.Drawing.Color.FromArgb(color, color, color));

                g.FillPolygon(brush,
                    new System.Drawing.PointF[] { 
                        new System.Drawing.PointF((float)(first.X - minX), (float)(result.Height - (first.Y - minY))),
                        new System.Drawing.PointF((float)(second.X - minX), (float)(result.Height - (second.Y - minY))),
                        new System.Drawing.PointF((float)(third.X - minX), (float)(result.Height - (third.Y - minY)))});
            }

            return result;
        }

        public System.Drawing.Bitmap DrawAspectMap(int scale)
        {
            if (this.triangulation.triangles.Count < 1)
            {
                throw new NotImplementedException();
            }

            double minX = this.LowerLeft.X; double minY = this.LowerLeft.Y;

            int width = (int)(this.UpperRight.X - minX);

            int height = (int)(this.UpperRight.Y - minY);

            System.Drawing.Bitmap result = new System.Drawing.Bitmap(width / scale, height / scale);

            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(result);

            foreach (QuasiTriangle item in this.triangulation.triangles)
            {
                Point first = collection.GetPoint(item.First);

                Point second = collection.GetPoint(item.Second);

                Point third = collection.GetPoint(item.Third);

                Triangle temp = new Triangle(first, second, third);

                double tempSlope = CalculateAspect(temp);

                //int color = (int)((tempSlope + Math.PI / 2) * 255 / (Math.PI));

                int color = (int)(tempSlope * 255 / (2 * Math.PI));

                System.Drawing.SolidBrush brush = new System.Drawing.SolidBrush(System.Drawing.Color.FromArgb(color, color, color));

                g.FillPolygon(brush,
                    new System.Drawing.PointF[] { 
                        new System.Drawing.PointF((float)(first.X - minX), (float)(result.Height - (first.Y - minY))),
                        new System.Drawing.PointF((float)(second.X - minX), (float)(result.Height - (second.Y - minY))),
                        new System.Drawing.PointF((float)(third.X - minX), (float)(result.Height - (third.Y - minY)))});
            }

            return result;
        }

        public AdjacencyList<Point, double> GetSlopeGraph()
        {
            AdjacencyList<Point, double> result =
                new AdjacencyList<Point, double>();//this.triangulation.triangles.Count);

            for (int i = 0; i < this.triangulation.triangles.Count; i++)
            {
                QuasiTriangle tempQuasiTriangle = this.triangulation.triangles[i];

                int tempCode = tempQuasiTriangle.GetHashCode();

                Triangle tempTriangle = this.triangulation.GeTriangle(tempCode);

                Point firstPoint = tempTriangle.CalculateCentroid();

                double currentSlope = CalculateSlope(tempTriangle);

                foreach (int neighbour in tempQuasiTriangle.OrderedNeighbours)
                {
                    if (neighbour != -1)
                    {
                        Triangle neighbourTriangle = this.triangulation.GeTriangle(neighbour);

                        Point secondPoint = neighbourTriangle.CalculateCentroid();

                        double neighbourSlope = CalculateSlope(tempTriangle);

                        double weight = currentSlope + neighbourSlope;

                        Connection<Point, double> tempConnection = new Connection<Point, double>(secondPoint, weight);

                        result.AddUndirectedEdge(firstPoint, secondPoint, weight);
                    }
                }
            }

            return result;
        }
    }
}