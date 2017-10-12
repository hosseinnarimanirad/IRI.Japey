using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hnr.Spatial.Version04.Primitives
{
    public struct BasicPath
    {
        private int height, width;

        private List<Move> movements;

        private static List<Transform> subLevelTransforms;

        public BasicPath(int height, int width, List<Move> movements, List<Transform> subLevelTransforms)
        {
            if (movements.Count + 1 != subLevelTransforms.Count)
            {
                throw new NotImplementedException();
            }

            if (height * width != subLevelTransforms.Count)
            {
                throw new NotImplementedException();
            }

            this.height = height; this.width = width;

            this.movements = movements;

            BasicPath.subLevelTransforms = subLevelTransforms;

            //
            this.movements.Add(this.movements[this.movements.Count - 1]);
        }

        public Move this[int index]
        {
            get { return movements[index]; }
            //set { /* set the specified index to value here */ }
        }

        public int NumberOfSteps
        {
            get { return subLevelTransforms.Count; }
        }

        public int Height
        {
            get { return this.height; }
        }

        public int Width
        {
            get { return this.width; }
        }

        public Transform GetTransform(int index)
        {
            return subLevelTransforms[index];
        }

        public BasicPath DoTransform(int transformInex)
        {
            List<Move> newMovements = new List<Move>();

            for (int i = 0; i < this.NumberOfSteps - 1; i++)
            {
                newMovements.Add(subLevelTransforms[transformInex](this.movements[i]));

                ////********************************************

                //Point2D temp00 = this.movements[i](new Point2D(5, 5), 1);

                //Point2D temp01 = newMovements[i](new Point2D(5, 5), 1);
                //Point2D temp02 = newMovements[i](new Point2D(5, 5), 2);
            }

            return new BasicPath(this.Height, this.Width, newMovements, subLevelTransforms);
        }
    }


    public static class BasicPaths
    {
        public static BasicPath Hilbert(Move first, Move second)
        {
            Move third = Transforms.Rotate180(first);

            List<Move> movements = new List<Move> { first, second, third };

            Point2D temp1 = second(first(new Point2D(0, 0), 1), 1);

            Point2D temp2 = third(new Point2D(0, 0), 1);

            bool isCCW = (temp1.X * temp2.Y - temp2.X * temp1.Y) > 0;// ? Transforms.Rotate90CCW : Transforms.Rotate90CW;

            //List<Transform> transforms = new List<Transform>{Transforms.CompositeTransform(Transforms.Rotate90CW,Transforms.HorizontalReflection),
            //                                                            Transforms.CompositeTransform(Transforms.DoNothing,Transforms.DoNothing),
            //                                                            Transforms.CompositeTransform(Transforms.DoNothing,Transforms.DoNothing),
            //                                                            Transforms.CompositeTransform(Transforms.Rotate90CCW,Transforms.HorizontalReflection)};
            //List<Transform> transforms = new List<Transform>{Transforms.CompositeTransform(Transforms.Rotate90CCW,Transforms.Reflex(first)),
            //                                                    Transforms.CompositeTransform(Transforms.DoNothing,Transforms.DoNothing),
            //                                                    Transforms.CompositeTransform(Transforms.DoNothing,Transforms.DoNothing),
            //                                                    Transforms.CompositeTransform(Transforms.Rotate90CW,Transforms.Reflex(first))};
            List<Transform> transforms = new List<Transform>{
                                                        Transforms.CompositeTransform(
                                                                                Transforms.Reflex(first),
                                                                                isCCW?Transforms.Rotate90CCW:Transforms.Rotate90CW),
                                                        Transforms.DoNothing,
                                                        Transforms.DoNothing,
                                                        Transforms.CompositeTransform(
                                                                                Transforms.Reflex(first),
                                                                                isCCW?Transforms.Rotate90CW:Transforms.Rotate90CCW)};

            return new BasicPath(2, 2, movements, transforms);
        }

        public static BasicPath Gray(Move first, Move second, Move third)
        {
            List<Move> movements = new List<Move> { first, second, third };

            List<Transform> transforms = new List<Transform>{Transforms.CompositeTransform(Transforms.DoNothing,Transforms.DoNothing),
                                                                        Transforms.CompositeTransform(Transforms.Rotate90CW,Transforms.Rotate90CW),
                                                                        Transforms.CompositeTransform(Transforms.Rotate90CW,Transforms.Rotate90CW),
                                                                        Transforms.CompositeTransform(Transforms.DoNothing,Transforms.DoNothing)};

            return new BasicPath(2, 2, movements, transforms);
        }

        public static BasicPath Peano(Move first, Move second)
        {
            Move third = Transforms.Rotate180(first);

            List<Move> movements = new List<Move> { first, first, second, third, third, second, first, first };

            //List<Transform> transforms = 
            //    new List<Transform>{Transforms.Transform(Transforms.DoNothing,Transforms.DoNothing),
            //            Transforms.Transform(Transforms.Rotate180,Transforms.HorizontalReflection),
            //            Transforms.Transform(Transforms.DoNothing,Transforms.DoNothing),
            //            Transforms.Transform(Transforms.Rotate180,Transforms.VerticalReflection),
            //            Transforms.Transform(Transforms.Rotate180,Transforms.DoNothing),
            //            Transforms.Transform(Transforms.Rotate180,Transforms.VerticalReflection),
            //            Transforms.Transform(Transforms.DoNothing,Transforms.DoNothing),
            //            Transforms.Transform(Transforms.Rotate180,Transforms.HorizontalReflection),
            //            Transforms.Transform(Transforms.DoNothing,Transforms.DoNothing)};

            //List<Transform> transforms =
            //    new List<Transform>{
            //            Transforms.DoNothing,
            //            Transforms.VerticalReflection,
            //            Transforms.DoNothing,
            //            Transforms.HorizontalReflection,
            //            Transforms.Rotate180,
            //            Transforms.HorizontalReflection,
            //            Transforms.DoNothing,
            //            Transforms.VerticalReflection,
            //            Transforms.DoNothing};

            List<Transform> transforms =
                new List<Transform>{
                        Transforms.DoNothing,
                        Transforms.Reflex(first),
                        Transforms.DoNothing,
                        Transforms.Reflex(second),
                        Transforms.Rotate180,
                        Transforms.Reflex(second),
                        Transforms.DoNothing,
                        Transforms.Reflex(first),
                        Transforms.DoNothing};

            return new BasicPath(3, 3, movements, transforms);
        }

        public static BasicPath NOrdering(Move first, Move second)
        {
            List<Move> movements = new List<Move> { first, second, first };

            List<Transform> transforms = new List<Transform>{Transforms.CompositeTransform(Transforms.DoNothing,Transforms.DoNothing),
                                                                        Transforms.CompositeTransform(Transforms.DoNothing,Transforms.DoNothing),
                                                                        Transforms.CompositeTransform(Transforms.DoNothing,Transforms.DoNothing),
                                                                        Transforms.CompositeTransform(Transforms.DoNothing,Transforms.DoNothing)};

            return new BasicPath(2, 2, movements, transforms);

        }

        public static BasicPath Peano02(Move first, Move second)
        {
            Move third = Transforms.Rotate180(first);

            List<Move> movements = new List<Move> { first, first, second, third, third, second, first, first };

            List<Transform> transforms =
                            new List<Transform>{
                                Transforms.DoNothing,
                                Transforms.Rotate90CCW,
                                Transforms.DoNothing,
                                Transforms.Rotate90CW,
                                Transforms.Rotate180,
                                Transforms.Rotate90CW,
                                Transforms.DoNothing,
                                Transforms.Rotate90CCW,
                                Transforms.DoNothing};

            return new BasicPath(3, 3, movements, transforms);

        }

        public static BasicPath Peano03(Move first, Move second)
        {
            Move third = Transforms.Rotate180(first);

            Move fourth = Transforms.Rotate180(second);

            List<Move> movements = new List<Move> { first, first, second, second, third, fourth, third, second };

            List<Transform> transforms =
                            new List<Transform>{
                                Transforms.CompositeTransform(Transforms.Reflex(first),Transforms.Rotate90CW),
                                Transforms.CompositeTransform(Transforms.Reflex(first),Transforms.Rotate90CW),
                                Transforms.DoNothing,
                                Transforms.DoNothing,
                                Transforms.DoNothing,
                                Transforms.Rotate180,
                                Transforms.CompositeTransform(Transforms.Rotate90CW,Transforms.Reflex(first)),
                                Transforms.CompositeTransform(Transforms.Rotate90CW,Transforms.Reflex(first)),
                                Transforms.DoNothing};

            return new BasicPath(3, 3, movements, transforms);

        }

        public static BasicPath LebesgueDiagonal(Move first, Move second)
        {
            Move third = Transforms.Reflex(second)(first);

            List<Move> movements = new List<Move> { first, second, third };

            List<Transform> transforms =
                            new List<Transform>{
                                Transforms.DoNothing,
                                Transforms.DoNothing,
                                Transforms.DoNothing,
                                Transforms.DoNothing};

            return new BasicPath(2, 2, movements, transforms);

        }

        public static BasicPath LebesgueSquare(Move first, Move second)
        {
            Move third = Transforms.Rotate180(first);

            List<Move> movements = new List<Move> { first, second, third };

            List<Transform> transforms =
                            new List<Transform>{
                                Transforms.DoNothing,
                                Transforms.DoNothing,
                                Transforms.DoNothing,
                                Transforms.DoNothing};

            return new BasicPath(2, 2, movements, transforms);

        }
    }

}
