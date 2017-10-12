using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Hnr.Spatial.Version01
{
    public static class SpaceFillingCurves
    {
        public enum Direction
        {
            south = 0,
            east = 1,
            north = 2,
            west = 3
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

        public delegate Point Move(Point current, int step);

        public static Move[] moves = new Move[] { MoveSouth, MoveEast, MoveNorth, MoveWest };

        public static Move MoveSouth = new Move((currentPoint, step) => new Point(currentPoint.X, currentPoint.Y - step));

        public static Move MoveEast = new Move((currentPoint, step) => new Point(currentPoint.X + step, currentPoint.Y));

        public static Move MoveNorth = new Move((currentPoint, step) => new Point(currentPoint.X, currentPoint.Y + step));

        public static Move MoveWest = new Move((currentPoint, step) => new Point(currentPoint.X - step, currentPoint.Y));

        public static BasicMovement Decide(Point currentPoint, Direction direction)
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

        public static List<Point> DoBasicMove(Point current, Direction direction)
        {
            List<Point> result = new List<Point>();

            result.Add(current);

            BasicMovement movement = Decide(current, direction);

            Point temp = movement.firstMove(current, 1); result.Add(temp);

            temp = movement.secondMove(temp, 1); result.Add(temp);

            temp = movement.thirdMove(temp, 1); result.Add(temp);

            return result;
        }

        public static int GetPointPosition(Point point)
        {
            int dx = point.X % 2;

            int dy = point.Y % 2;

            return (dy == 1 && dx == 0) ? 3 : dx + dy;
            
        }

        public static List<Point> HilbertCurve(Point startPoint, int size, BasicPath path)
        {
            List<Point> result = new List<Point>();

            if (size == 2)
            {
                //path.second?
                return DoBasicMove(startPoint, path.second);
            }

            moves = new Move[] { MoveSouth, MoveEast, MoveNorth, MoveWest };

            Point firstStartPoint = startPoint;

            //List<Point> temp01 = HilbertCurve(firstStartPoint, size / 2, Decide(GetPointPosition(firstStartPoint), path.first));

            //Point secondStartPoint = moves[(int)path.first](temp01[temp01.Count - 1], 1);
             
            //List<Point> temp02 = HilbertCurve(secondStartPoint, size / 2, Decide(GetPointPosition(secondStartPoint), path.second));

            //Point thirdStartPoint = moves[(int)path.second](temp02[temp02.Count - 1], 1);

            //List<Point> temp03 = HilbertCurve(thirdStartPoint, size / 2, Decide(GetPointPosition(thirdStartPoint), path.third));

            //Point fourthStartPoint = moves[(int)path.third](temp03[temp03.Count - 1], 1);

            ////path.third?
            //List<Point> temp04 = HilbertCurve(fourthStartPoint, size / 2, Decide(GetPointPosition(fourthStartPoint), path.third));

            //result.AddRange(temp01); result.AddRange(temp02); result.AddRange(temp03); result.AddRange(temp04);

            result.AddRange(HilbertCurve(firstStartPoint, size / 2, Decide(GetPointPosition(firstStartPoint), path.first)));

            Point secondStartPoint = moves[(int)path.first](result[result.Count - 1], 1);

            result.AddRange(HilbertCurve(secondStartPoint, size / 2, Decide(GetPointPosition(secondStartPoint), path.second)));

            Point thirdStartPoint = moves[(int)path.second](result[result.Count - 1], 1);

            result.AddRange(HilbertCurve(thirdStartPoint, size / 2, Decide(GetPointPosition(thirdStartPoint), path.third)));

            Point fourthStartPoint = moves[(int)path.third](result[result.Count - 1], 1);

            result.AddRange(HilbertCurve(fourthStartPoint, size / 2, Decide(GetPointPosition(fourthStartPoint), path.third)));

            return result;
        }
    }
}
