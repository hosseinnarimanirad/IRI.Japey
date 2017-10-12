using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hnr.Spatial.Version04.Primitives
{
    public delegate Point2D Move(Point2D point, int step);

    public static class Moves
    {
        //public static Move Move =
        //    new Move((Point2D point, Vector moveVector) =>
        //        new Point2D(point.X * moveVector.Radius * (int)Math.Cos(moveVector.Theta) - point.Y * moveVector.Radius * (int)Math.Sin(moveVector.Theta),
        //                    point.X * moveVector.Radius * (int)Math.Sin(moveVector.Theta) + point.Y * moveVector.Radius * (int)Math.Cos(moveVector.Theta)));

        public static Point2D Move(Point2D point, Vector moveVector)
        {
            return new Point2D(point.X * moveVector.Radius * (int)Math.Cos(moveVector.Theta) - point.Y * moveVector.Radius * (int)Math.Sin(moveVector.Theta),
                               point.X * moveVector.Radius * (int)Math.Sin(moveVector.Theta) + point.Y * moveVector.Radius * (int)Math.Cos(moveVector.Theta));
        }

        public static Move West = new Move((point, step) => new Point2D(point.X - step, point.Y));

        public static Move East = new Move((point, step) => new Point2D(point.X + step, point.Y));

        public static Move North = new Move((point, step) => new Point2D(point.X, point.Y + step));

        public static Move South = new Move((point, step) => new Point2D(point.X, point.Y - step));

        public static Move NorthWest = new Move((point, step) => West(North(point, step), step));

        public static Move NorthEast = new Move((point, step) => East(North(point, step), step));

        public static Move SouthWest = new Move((point, step) => West(South(point, step), step));

        public static Move SouthEast = new Move((point, step) => East(South(point, step), step));


    }

}
