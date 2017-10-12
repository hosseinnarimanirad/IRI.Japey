using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hnr.Spatial.Primitives;

namespace Hnr.Spatial.Version03
{
    public delegate Direction BasicTransform(Direction direction);

    public delegate BasicTransform Transform(BasicTransform firstTransform, BasicTransform secondTransform);

    public delegate Point2D Jump(Point2D point, int height, int width, Move moveFunc);

    public struct BasicPath
    {
        private int height, width;

        private List<Direction> directions;

        private static List<BasicTransform> subLevelTransforms;

        public BasicPath(int height, int width, List<Direction> directions, List<BasicTransform> subLevelTransforms)
        {
            if (directions.Count + 1 != subLevelTransforms.Count)
            {
                throw new NotImplementedException();
            }

            if (height * width != subLevelTransforms.Count)
            {
                throw new NotImplementedException();
            }

            this.height = height; this.width = width;

            this.directions = directions;

            BasicPath.subLevelTransforms = subLevelTransforms;

            //
            this.directions.Add(this.directions[this.directions.Count - 1]);
        }

        public Direction this[int index]
        {
            get { return directions[index]; }
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

        public BasicTransform GetTransform(int index)
        {
            return subLevelTransforms[index];
        }

        public BasicPath DoTransform(int transformInex)
        {
            List<Direction> newDirections = new List<Direction>();

            for (int i = 0; i < this.NumberOfSteps - 1; i++)
            {
                newDirections.Add(subLevelTransforms[transformInex](this.directions[i]));
            }

            return new BasicPath(this.Height, this.Width, newDirections, subLevelTransforms);
        }

    }

    public static class PositionIndex
    {
        public static int NOrderingPosition(Point2D point, int size)
        {
            int dx = point.X % size;

            int dy = point.Y % size;

            //return (dy == 0 && dx == 1) ? 2 : dx + dy;
            if (dx == size / 2 - 1 && dy == size / 2 - 1)
            {
                return 0;
            }
            else if (dx == size / 2 - 1 && dy == size - 1)
            {
                return 1;
            }
            else if (dx == size - 1 && dy == size / 2 - 1)
            {
                return 2;
            }
            else if (dx == size - 1 && dy == size - 1)
            {
                return 3;
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }

    public static class Jumbs
    {
        public static Point2D HilbertJump(Point2D point, int height, int width, Move moveFunc)
        {
            if (height != width)
            {
                throw new NotImplementedException();
            }

            return moveFunc(point, 1);
        }

        public static Point2D GrayJump(Point2D point, int height, int width, Move moveFunc)
        {
            if (height != width)
            {
                throw new NotImplementedException();
            }

            return moveFunc(point, height - 1);
        }

        public static Point2D PeanoJump(Point2D point, int height, int width, Move moveFunc)
        {
            if (height != width)
            {
                throw new NotImplementedException();
            }

            return moveFunc(point, 1);
        }

        public static Point2D NOrderingJump(Point2D point, int height, int width, Move moveFunc)
        {
            if (height != width)
            {
                throw new NotImplementedException();
            }

            int position = PositionIndex.NOrderingPosition(point, height);

            Point2D temp = moveFunc(point, 1);

            switch (position)
            {
                case 0:
                case 2:
                    return Primitives.Moves.West(temp, width / 2 - 1);

                case 1:
                    return Primitives.Moves.South(temp, height - 2);

                case 3:
                    return Primitives.Moves.West(temp, width / 2 - 1);
                //case 1:
                //return Primitives.Moves.

                default:
                    throw new NotImplementedException();
            }
        }
    }

    public static class Transforms
    {

        public static Direction DoNothing(Direction direction)
        {
            return direction;
        }

        public static Direction Rotate90Clockwise(Direction direction)
        {
            if (direction == Direction.West)
            {
                return Direction.North;
            }
            else if (direction == Direction.NorthWest)
            {
                return Direction.NorthEast;
            }
            else
            {
                return (Direction)((int)direction + 1);
            }
        }

        public static Direction Rotate180(Direction direction)
        {
            return Rotate90Clockwise(Rotate90Clockwise(direction));
        }

        public static Direction Rotate90CounterClockwise(Direction direction)
        {
            if (direction == Direction.North)
            {
                return Direction.West;
            }
            else if (direction == Direction.NorthEast)
            {
                return Direction.NorthWest;
            }
            else
            {
                return (Direction)((int)direction - 1);
            }
        }

        public static Direction FlipHorizontally(Direction direction)
        {
            if (direction == Direction.North || direction == Direction.South)
            {
                return (Direction)(((int)direction + 2) % 4);
            }
            else if (direction == Direction.NorthEast || direction == Direction.SouthWest)
            {
                return (Direction)((int)direction + 1);
            }
            else if (direction == Direction.SouthEast || direction == Direction.NorthWest)
            {
                return (Direction)((int)direction - 1);
            }

            return direction;
        }

        public static Direction FlipVertically(Direction direction)
        {
            if (direction == Direction.East || direction == Direction.West)
            {
                return (Direction)(((int)direction + 2) % 4);
            }
            else if (direction == Direction.NorthEast)
            {
                return Direction.NorthWest;
            }
            else if (direction == Direction.NorthWest)
            {
                return Direction.NorthEast;
            }
            else if (direction == Direction.SouthEast)
            {
                return Direction.SouthWest;
            }
            else if (direction == Direction.SouthWest)
            {
                return Direction.SouthEast;
            }

            return direction;
        }

        public static BasicTransform Transform(BasicTransform firstTransform, BasicTransform secondTransform)
        {
            return new BasicTransform(direction => secondTransform(firstTransform(direction)));
        }
    }

    public static class BasicPaths
    {
        public static BasicPath Hilbert(Direction first, Direction second, Direction third)
        {
            List<Direction> directions = new List<Direction> { first, second, third };

            List<BasicTransform> transforms = new List<BasicTransform>{Transforms.Transform(Transforms.Rotate90Clockwise,Transforms.FlipHorizontally),
                                                                        Transforms.Transform(Transforms.DoNothing,Transforms.DoNothing),
                                                                        Transforms.Transform(Transforms.DoNothing,Transforms.DoNothing),
                                                                        Transforms.Transform(Transforms.Rotate90CounterClockwise,Transforms.FlipHorizontally)};

            return new BasicPath(2, 2, directions, transforms);
        }

        public static BasicPath Gray(Direction first, Direction second, Direction third)
        {
            List<Direction> directions = new List<Direction> { first, second, third };

            List<BasicTransform> transforms = new List<BasicTransform>{Transforms.Transform(Transforms.DoNothing,Transforms.DoNothing),
                                                                        Transforms.Transform(Transforms.Rotate90Clockwise,Transforms.Rotate90Clockwise),
                                                                        Transforms.Transform(Transforms.Rotate90Clockwise,Transforms.Rotate90Clockwise),
                                                                        Transforms.Transform(Transforms.DoNothing,Transforms.DoNothing)};

            return new BasicPath(2, 2, directions, transforms);
        }

        public static BasicPath PeanoVertical(Direction first, Direction second)
        {
            Direction third = Transforms.Rotate180(first);

            List<Direction> directions = new List<Direction> { first, first, second, third, third, second, first, first };

            List<BasicTransform> transforms = new List<BasicTransform>{Transforms.Transform(Transforms.DoNothing,Transforms.DoNothing),
                                                                        Transforms.Transform(Transforms.Rotate180,Transforms.FlipHorizontally),
                                                                        Transforms.Transform(Transforms.DoNothing,Transforms.DoNothing),
                                                                        Transforms.Transform(Transforms.Rotate180,Transforms.FlipVertically),
                                                                        Transforms.Transform(Transforms.Rotate180,Transforms.DoNothing),
                                                                        Transforms.Transform(Transforms.Rotate180,Transforms.FlipVertically),
                                                                        Transforms.Transform(Transforms.DoNothing,Transforms.DoNothing),
                                                                        Transforms.Transform(Transforms.Rotate180,Transforms.FlipHorizontally),
                                                                        Transforms.Transform(Transforms.DoNothing,Transforms.DoNothing)};

            return new BasicPath(3, 3, directions, transforms);

        }

        public static BasicPath NOrdering()
        {
            List<Direction> directions = new List<Direction> { Direction.North, Direction.SouthEast, Direction.North };

            List<BasicTransform> transforms = new List<BasicTransform>{Transforms.Transform(Transforms.DoNothing,Transforms.DoNothing),
                                                                        Transforms.Transform(Transforms.DoNothing,Transforms.DoNothing),
                                                                        Transforms.Transform(Transforms.DoNothing,Transforms.DoNothing),
                                                                        Transforms.Transform(Transforms.DoNothing,Transforms.DoNothing)};

            return new BasicPath(2, 2, directions, transforms);

        }
    }

    public static class SpaceFillingCurves
    {
        private static List<Point2D> DoBasicMove(Point2D current, BasicPath path)
        {
            List<Point2D> result = new List<Point2D>();

            Point2D temp = current;

            result.Add(temp);

            for (int i = 0; i < path.NumberOfSteps - 1; i++)
            {
                temp = moves[(int)path[i]](temp, 1);

                result.Add(temp);
            }

            return result;
        }

        private static Move[] moves;

        static SpaceFillingCurves()
        {
            moves = new Move[] { Moves.North, Moves.East, Moves.South, Moves.West, Moves.NorthEast, Moves.SouthEast, Moves.SouthWest, Moves.NorthWest };
        }

        public static List<Point2D> Construct(Point2D startPoint, int height, int width, BasicPath path, Jump jumpFunc)
        {
            List<Point2D> result = new List<Point2D>();

            if (height * width == path.NumberOfSteps)
            {
                return DoBasicMove(startPoint, path);
            }

            int newHeight = height / path.Height;

            int newWidth = width / path.Width;

            Point2D tempPoint = startPoint;

            for (int i = 0; i < path.NumberOfSteps; i++)
            {
                result.AddRange(Construct(tempPoint, newHeight, newWidth, path.DoTransform(i), jumpFunc));

                tempPoint = jumpFunc(result[result.Count - 1], height, width, moves[(int)path[i]]);
            }

            return result;
        }
    }
}
