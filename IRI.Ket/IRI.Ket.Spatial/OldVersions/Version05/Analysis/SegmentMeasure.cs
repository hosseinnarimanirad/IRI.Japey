using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hnr.Spatial.Version05.Primitives;

namespace Hnr.Spatial.Version05.Analysis
{
    public static class SegmentMeasure
    {
        public delegate bool MeasureCriteria(Point2D first, Point2D second);

        public static int Measure(BasicPath path, Jump jumpFunc, int N, MeasureCriteria criteria)
        {
            int result01 = 0;

            int result02 = 0;

            for (int i = 0; i < path.NumberOfSteps - 1; i++)
            {
                Point2D point = new Point2D(0, 0);

                Point2D tempPoint = path[i](point, 1);

                if (criteria(point, tempPoint))
                {
                    result01++;
                }

                Point2D tempPoint02 = jumpFunc(point, i, path.Height * path.Height, path.Width * path.Width, path);

                if (criteria(point, tempPoint02))
                {
                    result02++;
                }
            }

            int totalCells = N * N;

            int totalRegions = (N / path.Height) * (N / path.Height);

            return totalRegions * result01 + (totalRegions - 1) / (path.NumberOfSteps - 1) * result02;
        }

        public static int MeasureJumps(BasicPath path, Jump jumpFunc, int N)
        {
            return Measure(path, jumpFunc, N,
                            (p1, p2) =>
                                (p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y) > 1);
        }
    }
}
