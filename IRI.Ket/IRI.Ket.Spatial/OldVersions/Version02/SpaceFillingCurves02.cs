using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Hnr.Spatial.Version02
{
    public struct Point3D
    {
        public int X, Y, Z;

        public Point3D(int x, int y, int z)
        {
            this.X = x; this.Y = y; this.Z = z;
        }
    }

    public static class SpaceFillingCurves
    {
        public enum Direction
        {
            south = 0,
            east = 1,
            north = 2,
            west = 3,
            up = 4,
            down = 5
        }

        public struct BasicPath
        {
            public Direction first, second, third;

            public BasicPath(Direction first, Direction second, Direction third)
            {
                this.first = first;

                this.second = second;

                this.third = third;
            }
        }

        public struct BasicMovement
        {
            public Move firstMove, secondMove, thirdMove;

            public BasicMovement(Move firstMove, Move secondMove, Move thirdMove)
            {
                this.firstMove = firstMove; this.secondMove = secondMove; this.thirdMove = thirdMove;
            }
        }

        public delegate Point3D Move(Point3D current, int step);

        public static Move[] moves = new Move[] { MoveSouth, MoveEast, MoveNorth, MoveWest };

        public static Move MoveSouth = new Move((currentPoint, step) => new Point3D(currentPoint.X, currentPoint.Y - step, currentPoint.Z));

        public static Move MoveEast = new Move((currentPoint, step) => new Point3D(currentPoint.X + step, currentPoint.Y, currentPoint.Z));

        public static Move MoveNorth = new Move((currentPoint, step) => new Point3D(currentPoint.X, currentPoint.Y + step, currentPoint.Z));

        public static Move MoveWest = new Move((currentPoint, step) => new Point3D(currentPoint.X - step, currentPoint.Y, currentPoint.Z));

        public static Move MoveUp = new Move((currentPoint, step) => new Point3D(currentPoint.X, currentPoint.Y, currentPoint.Z + step));

        public static Move MoveDown = new Move((currentPoint, step) => new Point3D(currentPoint.X, currentPoint.Y, currentPoint.Z - step));

        public static BasicMovement Decide(Point3D currentPoint, Direction direction)
        {
            int pointPosition = GetPointPosition(currentPoint);

            if (pointPosition == (int)direction || (pointPosition + 1) % 4 == (int)direction)
            {
                int temp = pointPosition + 2;

                return new BasicMovement(moves[temp % 4], moves[(temp - 1) % 4], moves[(temp - 2) % 4]);
            }
            else
            {
                int temp = pointPosition + 1;

                return new BasicMovement(moves[temp % 4], moves[(temp + 1) % 4], moves[(temp + 2) % 4]);
            }
        }

        public static BasicPath Decide(int pointPosition, Direction direction)
        {
            if (pointPosition == (int)direction || (pointPosition + 1) % 4 == (int)direction)
            {
                int temp = pointPosition + 2;

                return new BasicPath((Direction)(temp % 4), (Direction)((temp - 1) % 4), (Direction)((temp - 2) % 4));
            }
            else
            {
                int temp = pointPosition + 1;

                return new BasicPath((Direction)(temp % 4), (Direction)((temp + 1) % 4), (Direction)((temp + 2) % 4));
            }
        }

        public static List<Point3D> DoBasicMove(Point3D current, Direction direction)
        {
            List<Point3D> result = new List<Point3D>();

            result.Add(current);

            BasicMovement movement = Decide(current, direction);

            Point3D temp = movement.firstMove(current, 1); result.Add(temp);

            temp = movement.secondMove(temp, 1); result.Add(temp);

            temp = movement.thirdMove(temp, 1); result.Add(temp);

            return result;
        }

        public static int GetPointPosition(Point3D point)
        {
            int dx = point.X % 2;

            int dy = point.Y % 2;

            return (dy == 1 && dx == 0) ? 3 : dx + dy;

        }

        public static List<Point3D> HilbertCurve(Point3D startPoint, int size, BasicPath path)
        {
            List<Point3D> result = new List<Point3D>();

            if (size == 2)
            {
                //path.second?
                return DoBasicMove(startPoint, path.second);
            }

            moves = new Move[] { MoveSouth, MoveEast, MoveNorth, MoveWest };

            Point3D firstStartPoint = startPoint;

            //List<Point3D> temp01 = HilbertCurve(firstStartPoint, size / 2, Decide(GetPointPosition(firstStartPoint), path.first));

            //Point3D secondStartPoint = moves[(int)path.first](temp01[temp01.Count - 1], 1);

            //List<Point3D> temp02 = HilbertCurve(secondStartPoint, size / 2, Decide(GetPointPosition(secondStartPoint), path.second));

            //Point3D thirdStartPoint = moves[(int)path.second](temp02[temp02.Count - 1], 1);

            //List<Point3D> temp03 = HilbertCurve(thirdStartPoint, size / 2, Decide(GetPointPosition(thirdStartPoint), path.third));

            //Point3D fourthStartPoint = moves[(int)path.third](temp03[temp03.Count - 1], 1);

            ////path.third?
            //List<Point3D> temp04 = HilbertCurve(fourthStartPoint, size / 2, Decide(GetPointPosition(fourthStartPoint), path.third));

            //result.AddRange(temp01); result.AddRange(temp02); result.AddRange(temp03); result.AddRange(temp04);

            return result;
        }
    }

}
