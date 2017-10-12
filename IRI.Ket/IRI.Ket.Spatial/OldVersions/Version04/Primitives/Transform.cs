using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hnr.Spatial.Version04.Primitives
{
    public delegate Move Transform(Move moveFunc);

    //public delegate Transform CompositeTransform(Transform firstTransform, Transform secondTransform);

    public static class Transforms
    {
        public static Transform DoNothing = new Transform(moveFunc => moveFunc);

        public static Transform Rotate90CW = new Transform((moveFunc) => new Move((point, step) => Rotate(point, step, moveFunc, -Math.PI / 2)));

        public static Transform Rotate90CCW = new Transform((moveFunc) => new Move((point, step) => Rotate(point, step, moveFunc, Math.PI / 2)));

        public static Transform Rotate180 = new Transform((moveFunc) => new Move((point, step) => Rotate(point, step, moveFunc, Math.PI)));

        public static Transform HorizontalReflection = new Transform((moveFunc) => new Move((point, step) => ReflectHorizontally(point, step, moveFunc)));

        public static Transform VerticalReflection = new Transform((moveFunc) => new Move((point, step) => ReflectVertically(point, step, moveFunc)));

        private static Point2D ReflectHorizontally(Point2D point, int step, Move moveFunc)
        {
            Point2D temp = moveFunc(point, step);

            int dy = temp.Y - point.Y;

            return new Point2D(temp.X, point.Y - dy);

            //return new Point2D(point.X + temp.X, point.Y + temp.Y);
        }

        private static Point2D ReflectVertically(Point2D point, int step, Move moveFunc)
        {
            Point2D temp = moveFunc(point, step);

            int dx = temp.X - point.X;

            return new Point2D(point.X - dx, temp.Y);

            //return new Point2D(point.X + temp.X, point.Y + temp.Y);
        }

        private static Point2D Rotate(Point2D point, int step, Move moveFunc, double rotationAngle)
        {
            Point2D temp = moveFunc(point, step);

            temp = new Point2D(temp.X - point.X, temp.Y - point.Y);

            temp = Moves.Move(temp, new Vector(1, rotationAngle));

            return new Point2D(point.X + temp.X, point.Y + temp.Y);
        }

        public static Transform CompositeTransform(Transform firstTransform, Transform secondTransform)
        {
            return new Transform(moveFunc => secondTransform(firstTransform(moveFunc)));
        }

        public static Transform Reflex(Move baseFunc)
        {
            return new Transform(moveFunc => new Move((point, step) => Reflex(point, step, moveFunc, baseFunc)));
        }

        public static Point2D Reflex(Point2D point, int step, Move moveFunc, Move baseFunc)
        {
            Point2D basePoint = new Point2D(0, 0);

            Point2D first = baseFunc(basePoint, step);

            Point2D second = moveFunc(basePoint, step);

            //delegate distanceToBase = (Point2D p) => Math.Sqrt(p.X * p.X + p.Y * p.Y);

            double firstNorm = Math.Sqrt(first.X * first.X + first.Y * first.Y);
            //
            //double secondNorm = Math.Sqrt(second.X * second.X + second.Y * second.Y);

            double coef = (first.X * second.X + first.Y * second.Y) / (firstNorm * firstNorm);

            double dx = coef * first.X - second.X;

            double dy = coef * first.Y - second.Y;

            return new Point2D(point.X + second.X + (int)Math.Round(2 * dx), point.Y + second.Y + (int)Math.Round(2 * dy));
        }

    }
}
