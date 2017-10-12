using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using IRI.Ket.DataStructure;
using IRI.Ket.Graph.GraphRepresentation;

//using MeasurementUnit;

namespace MainProject
{
    public partial class MainForm : Form
    {
        #region General

        public MainForm()
        {
            InitializeComponent();
            //this.voronoiPoints = new IRI.Ket.Geometry.VoronoiPointCollection();
            //this.points = new IRI.Ket.Geometry.PointCollection();
        }

        int counter = -1;

        //IRI.Ket.Geometry.PointCollection points = new IRI.Ket.Geometry.PointCollection();

        IRI.Ket.Geometry.DelaunayTriangulation tin;

        IRI.Ket.Geometry.PointCollection points;

        IRI.Ket.Geometry.VoronoiPointCollection voronoiPoints;

        IRI.Ket.Geometry.QuasiTriangleCollection bugs;

        AdjacencyList<IRI.Ket.Geometry.Point, double> network;

        AdjacencyList<IRI.Ket.Geometry.Point, double> minimumSpanningTree;

        int triangleID = 0;

        #endregion

        #region PointInCircle

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            //counter++;

            //if (counter > 3)
            //{
            //    Restart();
            //}
            //else if (counter == 3)
            //{
            //    points.Add(new IRI.Ket.Geometry.Point(e.X, pictureBox1.Height - e.Y));

            //    Graphics g = pictureBox1.CreateGraphics();

            //    g.DrawEllipse(Pens.Red, new RectangleF((float)(e.X - 1), (float)(e.Y - 1), 2, 2));

            //    bool result = IRI.Ket.Geometry.ComputationalGeometry.IsPointInCircle(points[3], points[0], points[1], points[2]);

            //    label1.Text = (result ? "Point is in Circle" : "Point is not in Circle");

            //    label1.ForeColor = (result ? Color.Green : Color.Red);

            //    double x0 = points[3].X; double y0 = points[3].Y;
            //    double x1 = points[0].X; double y1 = points[0].Y;
            //    double x2 = points[1].X; double y2 = points[1].Y;
            //    double x3 = points[2].X; double y3 = points[2].Y;

            //    IRI.Ket.Geometry.Point center = IRI.Ket.Geometry.ComputationalGeometry.CalculateCircumcenterCenterPoint(points[0], points[1], points[2]);

            //    //double D = 2 * (x1 * (y2 - y3) + x2 * (y3 - y1) + x3 * (y1 - y2));

            //    //double x = ((y1 * y1 + x1 * x1) * (y2 - y3) + (y2 * y2 + x2 * x2) * (y3 - y1) + (y3 * y3 + x3 * x3) * (y1 - y2)) / D;

            //    //double y = ((y1 * y1 + x1 * x1) * (x3 - x2) + (y2 * y2 + x2 * x2) * (x1 - x3) + (y3 * y3 + x3 * x3) * (x2 - x1)) / D;

            //    double s12 = Math.Sqrt((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1));
            //    double s13 = Math.Sqrt((x3 - x1) * (x3 - x1) + (y3 - y1) * (y3 - y1));
            //    double s23 = Math.Sqrt((x2 - x3) * (x2 - x3) + (y2 - y3) * (y2 - y3));

            //    double s = (s12 + s13 + s23) / 2;
            //    float r = (float)((s12 * s13 * s23) / (2 * Math.Sqrt(s * (s - s12) * (s - s13) * (s - s23))));

            //    g.DrawLine(Pens.Orange, new PointF((float)x1, (float)(pictureBox1.Height - y1)), new PointF((float)center.X, (float)(pictureBox1.Height - center.Y)));

            //    g.DrawEllipse(Pens.Orange, new RectangleF((float)(center.X - r / 2), (float)((pictureBox1.Height - center.Y) - r / 2), (float)r, (float)r));
            //}
            //else
            //{
            //    points.Add(new IRI.Ket.Geometry.Point(e.X, pictureBox1.Height - e.Y));

            //    Graphics g = pictureBox1.CreateGraphics();

            //    g.DrawEllipse(Pens.Blue, new RectangleF((float)(e.X - 1), (float)(e.Y - 1), 2, 2));

            //}
        }

        private void Restart()
        {
            //Graphics g = pictureBox1.CreateGraphics();

            //g.Clear(System.Drawing.Color.White);

            //counter = -1;

            //points.Clear();

            //label1.Text = "NOTHING";
        }

        #endregion

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            this.Text = string.Format("X:{0}, Y:{1}", e.X, e.Y);
        }

        private void voronoi_Click(object sender, EventArgs e)
        {
            DrawVoronoi(Pens.Red, Pens.Green);
        }

        private void generateValues()
        {
            Graphics g = pictureBox1.CreateGraphics(); //g.Clear(Color.White);

            //Draw(points, Pens.Black);

            // DateTime t1 = DateTime.Now;

            this.tin = new IRI.Ket.Geometry.DelaunayTriangulation(points);

            //DateTime t2 = DateTime.Now;

            // MessageBox.Show(string.Format("time to tin: {0}", (t2 - t1).ToString()));

            voronoiPoints = tin.GetVoronoiDiagram(points.MinX - 50, points.MinY - 50, points.MaxX + 50, points.MaxY + 50);

            //DateTime t3 = DateTime.Now;

            //TimeSpan s1 = t2 - t1;

            //TimeSpan s2 = t3 - t2;

            DrawItems();
        }

        private void checkConvexHull()
        {
            Graphics g = pictureBox1.CreateGraphics(); g.Clear(Color.White);
            Random r = new Random(); Random r2 = new Random(3);
            List<IRI.Ket.Geometry.Point> collection = new List<IRI.Ket.Geometry.Point>();
            int n = int.Parse(textBox1.Text);
            for (int i = 0; i < n; i++)
            {
                collection.Add(new IRI.Ket.Geometry.Point(r.Next(500), r2.Next(500)));
            }
            IRI.Ket.Geometry.PointCollection points = new IRI.Ket.Geometry.PointCollection(collection);
            Draw(points, Pens.Black);
            //IRI.Ket.Geometry.PointCollection c = IRI.Ket.Geometry.ComputationalGeometry.CreateConvexHull(collection);0
            List<int> result = IRI.Ket.Geometry.ComputationalGeometry.GetConvexHullVertexes(points);

            for (int i = 1; i < result.Count - 1; i++)
            {
                int x1 = (int)points[result[i - 1]].X;
                int y1 = (int)points[result[i - 1]].Y;
                int x2 = (int)points[result[i]].X;
                int y2 = (int)points[result[i]].Y;
                g.DrawLine(Pens.Orange, x1, y1, x2, y2);

            }

            g.DrawLine(Pens.Orange,
                        (int)points[result[result.Count - 1]].X,
                        (int)points[result[result.Count - 1]].Y,
                        (int)points[result[0]].X,
                        (int)points[result[0]].Y);

            //DrawConvexHull(result, Pens.Orange);
        }

        private void delaunay_Click(object sender, EventArgs e)
        {
            Draw(points, Pens.Black);

            DrawTin(Pens.Blue);
        }

        private void convexHull_Click(object sender, EventArgs e)
        {

            DrawConvexHull(Pens.Purple);

        }

        private void export_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();

            dialog.Filter = "*.txt|*.txt";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                System.IO.StreamWriter writer = new System.IO.StreamWriter(dialog.FileName);

                foreach (IRI.Ket.Geometry.Point item in this.points)
                {
                    writer.WriteLine(string.Format("{0};{1}", item.X, item.Y));
                }

                writer.Close();
            }
        }

        private void import_Click(object sender, EventArgs e)
        {
            this.points = new IRI.Ket.Geometry.PointCollection();

            OpenFileDialog dialog = new OpenFileDialog();

            dialog.Filter = "*.txt|*.txt";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                System.IO.StreamReader reader = new System.IO.StreamReader(dialog.FileName);

                while (!reader.EndOfStream)
                {
                    string[] temp = (reader.ReadLine()).Split(';');

                    this.points.Add(new IRI.Ket.Geometry.Point(double.Parse(temp[0]), double.Parse(temp[1])));
                }

                reader.Close();

                generateValues();
            }
        }

        private void generate_Click(object sender, EventArgs e)
        {
            Random r = new Random(); Random r2 = new Random(3); Random r3 = new Random(2);
            IRI.Ket.DigitalTerrainModeling.AttributedPointCollection ps = new IRI.Ket.DigitalTerrainModeling.AttributedPointCollection();
            int n = int.Parse(textBox1.Text);
            double[] east = new double[n];
            double[] north = new double[n];
            double[] value = new double[n];
            for (int i = 0; i < n; i++)
            {
                IRI.Ket.DigitalTerrainModeling.AttributedPoint point = new IRI.Ket.DigitalTerrainModeling.AttributedPoint(r.Next(50, 550), r2.Next(50, 550), r3.Next(-200, 200));

                //if (!ps.Contains(new IRI.Ket.Geometry.Point(point.X, point.Y)))
                //{
                ps.Add(point);
                //}

            }
            this.points = ps;

            IRI.Ket.DigitalTerrainModeling.IrregularDtm dtm = new IRI.Ket.DigitalTerrainModeling.IrregularDtm(ps);

            this.network = dtm.GetSlopeGraph();
            this.minimumSpanningTree = IRI.Ket.Graph.MinimumSpanningTree.CalculateByKruskal<IRI.Ket.Geometry.Point, double>(this.network);
            //this.tin = new IRI.Ket.Geometry.DelaunayTriangulation(points);
            generateValues();
            //DrawTin(Pens.Black, Pens.Blue);
        }

        private void do_Click(object sender, EventArgs e)
        {

            //tin.Do();

        }

        private void Draw(IRI.Ket.Geometry.PointCollection c, Pen p)
        {
            Graphics g = pictureBox1.CreateGraphics();
            
            //int index = 0;

            foreach (IRI.Ket.Geometry.Point item in c)
            {
                int x = (int)item.X;

                int y = (int)item.Y;

                g.DrawEllipse(p, new Rectangle(x - 2, y - 2, 4, 4));

                //g.DrawString(index.ToString(), new Font(FontFamily.GenericSansSerif, 5), Brushes.Black, (float)(x + 2.5), (float)(y - 1));

                //index++;
            }

            int x1 = (int)c[c.LowerBoundIndex].X;

            int y1 = (int)c[c.LowerBoundIndex].Y;

            g.DrawEllipse(Pens.Red, new Rectangle(x1 - 2, y1 - 2, 4, 4));


        }

        private void DrawConvexHull(Pen p)
        {
            try
            {
                int i = points.Count;
            }
            catch
            {
                return;
            }

            IRI.Ket.Geometry.PointCollection c = IRI.Ket.Geometry.ComputationalGeometry.CreateConvexHull(points);

            Graphics g = pictureBox1.CreateGraphics();

            g.DrawEllipse(p, new Rectangle((int)c[0].X - 2, (int)(c[0].Y) - 2, 4, 4));

            for (int i = 0; i < c.Count; i++)
            {
                int j = (i + 1) % c.Count;

                int x1 = (int)c[i].X;

                int y1 = (int)(c[i].Y);

                int x2 = (int)c[j].X;

                int y2 = (int)(c[j].Y);

                g.DrawEllipse(p, new Rectangle(x2 - 2, y2 - 2, 4, 4));

                g.DrawLine(p, new Point(x1, y1), new Point(x2, y2));
            }
        }

        public void DrawTin(Pen linePen)
        {

            try
            {
                int i = points.Count;
            }
            catch
            {
                return;
            }

            Graphics g = pictureBox1.CreateGraphics();

            if (tin == null || tin.triangles == null)
            {
                return;
            }

            foreach (IRI.Ket.Geometry.QuasiTriangle item in this.tin.triangles)
            {
                g.DrawLines(linePen, new Point[]{new Point((int)points.GetPoint(item.First).X,(int)points.GetPoint(item.First).Y),
                                                    new Point((int)points.GetPoint(item.Second).X,(int)points.GetPoint(item.Second).Y),
                                                    new Point((int)points.GetPoint(item.Third).X,(int)points.GetPoint(item.Third).Y),
                                                    new Point((int)points.GetPoint(item.First).X, (int)points.GetPoint(item.First).Y)});
            }
        }

        public void DrawVoronoi(Pen pointPen, Pen linePen)
        {
            try
            {
                int i = points.Count;
            }
            catch
            {
                return;
            }
            if (this.voronoiPoints == null)
            {
                return;
            }

            //if (pCheck.Checked)
            //{
            //    Draw(points, Pens.Black);
            //}

            System.Drawing.Graphics g = this.pictureBox1.CreateGraphics();

            g.DrawRectangle(Pens.Black, (float)(points.MinX - 50), (float)(points.MinY - 50), (float)(points.MaxX - points.MinX + 100), (float)(points.MaxY - points.MinY + 100));

            foreach (IRI.Ket.Geometry.VoronoiPoint item in voronoiPoints)
            {
                g.DrawEllipse(pointPen, new RectangleF((float)(item.X - 2), (float)(item.Y - 2), 4, 4));

                for (int i = 0; i < item.NeigboursCode.Count; i++)
                {
                    IRI.Ket.Geometry.VoronoiPoint p = voronoiPoints.GetPointByCode(item.NeigboursCode[i]);

                    if (item.TriangleCode == -1 || p.TriangleCode == -1)
                    {
                        g.DrawLine(Pens.Red, new PointF((float)item.X, (float)item.Y), new PointF((float)p.X, (float)p.Y));
                    }
                    else
                    {
                        g.DrawLine(linePen, new PointF((float)item.X, (float)item.Y), new PointF((float)p.X, (float)p.Y));
                    }

                }
            }
        }

        private void checkCircle_Click(object sender, EventArgs e)
        {
            Graphics g = pictureBox1.CreateGraphics();

            g.Clear(Color.White);

            Draw(points, Pens.Black);

            DrawTin(Pens.Blue);

            double x1 = points.GetPoint(tin.triangles[triangleID].First).X; double y1 = points.GetPoint(tin.triangles[triangleID].First).Y;
            double x2 = points.GetPoint(tin.triangles[triangleID].Second).X; double y2 = points.GetPoint(tin.triangles[triangleID].Second).Y;
            double x3 = points.GetPoint(tin.triangles[triangleID].Third).X; double y3 = points.GetPoint(tin.triangles[triangleID].Third).Y;

            //g.DrawLines(Pens.Aqua, new Point[]{new Point((int)points.GetPoint(item.First).X,(int)points.GetPoint(item.First).Y),
            //                                    new Point((int)points.GetPoint(item.Second).X,(int)points.GetPoint(item.Second).Y),
            //                                    new Point((int)points.GetPoint(item.Third).X,(int)points.GetPoint(item.Third).Y),
            //                                    new Point((int)points.GetPoint(item.First).X, (int)points.GetPoint(item.First).Y)});

            double s12 = Math.Sqrt((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1));
            double s13 = Math.Sqrt((x3 - x1) * (x3 - x1) + (y3 - y1) * (y3 - y1));
            double s23 = Math.Sqrt((x2 - x3) * (x2 - x3) + (y2 - y3) * (y2 - y3));

            double s = (s12 + s13 + s23) / 2;
            float radius = (float)((s12 * s13 * s23) / (2 * Math.Sqrt(s * (s - s12) * (s - s13) * (s - s23))));

            //g.DrawLine(Pens.Orange, new PointF((float)x1, (float)(pictureBox1.Height - y1)), new PointF((float)center.X, (float)(pictureBox1.Height - center.Y)));

            IRI.Ket.Geometry.Point center =
                IRI.Ket.Geometry.ComputationalGeometry.CalculateCircumcenterCenterPoint(points.GetPoint(tin.triangles[triangleID].First),
                                                                                        points.GetPoint(tin.triangles[triangleID].Second),
                                                                                        points.GetPoint(tin.triangles[triangleID].Third));


            g.DrawEllipse(Pens.Orange, new RectangleF((float)(center.X - radius / 2), (float)(center.Y - radius / 2), (float)radius, (float)radius));

            triangleID = (++triangleID) % this.tin.triangles.Count;
        }

        private void dCheck_CheckedChanged(object sender, EventArgs e)
        {
            DrawItems();
        }

        private void DrawItems()
        {
            System.Drawing.Graphics g = this.pictureBox1.CreateGraphics();

            g.Clear(Color.White);

            Draw(points, Pens.Black);

            if (cCheck.Checked)
            {
                DrawConvexHull(Pens.Fuchsia);
            }
            if (vCheck.Checked)
            {
                DrawVoronoi(Pens.CadetBlue, Pens.Green);
            }
            if (dCheck.Checked)
            {
                DrawTin(Pens.Gold);
            }
            if (nCheck.Checked)
            {
                Draw(network, g, Pens.Firebrick, Pens.Red);
            }
            if (mCheck.Checked)
            {
                Draw(this.minimumSpanningTree, g, Pens.Firebrick, Pens.CornflowerBlue);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //tin.Do();
        }

        private void bug_Click(object sender, EventArgs e)
        {
            Graphics g = pictureBox1.CreateGraphics();

            g.Clear(Color.White);

            Draw(points, Pens.Red);

            bugs = new IRI.Ket.Geometry.QuasiTriangleCollection();

            if (this.tin != null)
            {
                foreach (IRI.Ket.Geometry.QuasiTriangle item in tin.triangles)
                {
                    IRI.Ket.Geometry.Point p1 = points.GetPoint(item.First);
                    IRI.Ket.Geometry.Point p2 = points.GetPoint(item.Second);
                    IRI.Ket.Geometry.Point p3 = points.GetPoint(item.Third);

                    foreach (IRI.Ket.Geometry.Point p in points)
                    {
                        if (IRI.Ket.Geometry.ComputationalGeometry.GetPointCircleRelation(p, p1, p2, p3) ==
                                                                                IRI.Ket.Geometry.PointCircleRelation.In)
                        {
                            bugs.Add(item);
                            break;
                            //    g.DrawLines(Pens.Green, new Point[]{new Point((int)points.GetPoint(item.First).X,(int)points.GetPoint(item.First).Y),
                            //                            new Point((int)points.GetPoint(item.Second).X,(int)points.GetPoint(item.Second).Y),
                            //                            new Point((int)points.GetPoint(item.Third).X,(int)points.GetPoint(item.Third).Y),
                            //                            new Point((int)points.GetPoint(item.First).X, (int)points.GetPoint(item.First).Y)});
                        }
                    }
                }


            }
        }

        private void DrawBUGS()
        {
            Graphics g = pictureBox1.CreateGraphics();

            g.Clear(Color.White);

            Draw(points, Pens.Red);

            foreach (IRI.Ket.Geometry.QuasiTriangle item in bugs)
            {
                g.DrawLines(Pens.Green, new Point[]{new Point((int)points.GetPoint(item.First).X,(int)points.GetPoint(item.First).Y),
                                                new Point((int)points.GetPoint(item.Second).X,(int)points.GetPoint(item.Second).Y),
                                                new Point((int)points.GetPoint(item.Third).X,(int)points.GetPoint(item.Third).Y),
                                                new Point((int)points.GetPoint(item.First).X, (int)points.GetPoint(item.First).Y)});

            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            AboutMe dialog = new AboutMe();

            dialog.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Random r = new Random(); Random r2 = new Random(3); Random r3 = new Random(2);
            //IRI.Ket.DigitalTerrainModeling.AttributedPointCollection ps = new IRI.Ket.DigitalTerrainModeling.AttributedPointCollection();
            //int n = int.Parse(textBox1.Text);
            //double[] east = new double[n];
            //double[] north = new double[n];
            //double[] value = new double[n];
            //for (int i = 0; i < n; i++)
            //{
            //    ps.Add(new IRI.Ket.DigitalTerrainModeling.AttributedPoint(r.Next(50, 550), r2.Next(50, 550), r3.Next(-200, 200)));
            //}

            //System.Drawing.Graphics g = pictureBox1.CreateGraphics();

            //Draw(result, g, System.Drawing.Pens.Red, System.Drawing.Pens.Green);
            //Spatial.CoordinatedGraph<WeightedConnection<IRI.Ket.Geometry.Point, double>> result = dtm.GetSlopeGraph();
        }

        public void Draw(AdjacencyList<IRI.Ket.Geometry.Point, double> network,
            System.Drawing.Graphics g, System.Drawing.Pen nodePen, System.Drawing.Pen edgePen)
        {
            for (int i = 0; i < network.NumberOfNodes; i++)
            {
                float currentX = (float)network[i].X;

                float currentY = (float)network[i].Y;

                g.DrawEllipse(nodePen, new System.Drawing.RectangleF(currentX - 1, currentY - 1, 2, 2));

                LinkedList<Connection<IRI.Ket.Geometry.Point, double>> temp = network.GetConnectionsByNodeIndex(i);

                foreach (Connection<IRI.Ket.Geometry.Point,double> item in temp)
                {
                    IRI.Ket.Geometry.Point neighbour = item.Node;

                    g.DrawLine(edgePen,
                                new System.Drawing.PointF(currentX, currentY),
                                new System.Drawing.PointF((float)neighbour.X, (float)neighbour.Y));
                }
            }
        }

     
    }
}