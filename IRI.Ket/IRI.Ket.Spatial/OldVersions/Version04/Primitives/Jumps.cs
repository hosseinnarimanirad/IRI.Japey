using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hnr.Spatial.Version04.Primitives
{

    public delegate Point2D Jump(Point2D point, int pointIndex, int height, int width, BasicPath path);

    public static class Jumps
    {
        public static Point2D HilbertJump(Point2D point, int pointIndex, int height, int width, BasicPath path)
        {
            if (height != width)
            {
                throw new NotImplementedException();
            }

            return path[pointIndex](point, 1);
        }

        public static Point2D GrayJump(Point2D point, int pointIndex, int height, int width, BasicPath path)
        {
            if (height != width)
            {
                throw new NotImplementedException();
            }

            return path[pointIndex](point, height - 1);
        }

        public static Point2D PeanoJump(Point2D point, int pointIndex, int height, int width, BasicPath path)
        {
            if (height != width)
            {
                throw new NotImplementedException();
            }

            return path[pointIndex](point, 1);
        }

        public static Point2D ZigzagJump(Point2D point, int pointIndex, int height, int width, BasicPath path)
        {
            if (height != width)
            {
                throw new NotImplementedException();
            }

            Move secondStep = Transforms.Rotate180(path[0]);

            Point2D temp;

            if (pointIndex == 1)
            {
                temp = path[1](point, 1);

                temp = secondStep(temp, height - 2);
            }
            else
            {
                temp = (Transforms.Rotate180(path[1]))(point, height / 2 - 1);

                temp = secondStep(temp, height / 2 - 2);
            }

            return temp;
        }


        public static Point2D LebesgueDiagonal(Point2D point, int pointIndex, int height, int width, BasicPath path)
        {
            if (height != width)
            {
                throw new NotImplementedException();
            }

            Point2D temp;

            if (pointIndex == 0)
            {
                Transform tempTransform = Transforms.CompositeTransform(Transforms.Reflex(path[0]), Transforms.Rotate180);

                Move secondStep = tempTransform(path[1]);

                temp = path[0](point, 1);

                return secondStep(temp, height / 2 - 1);
            }
            else if (pointIndex == 1)
            {
                return path[1](point, height - 1);
            }
            else if (pointIndex == 2)
            {
                Transform tempTransform = Transforms.Reflex(path[0]);

                Move secondStep = tempTransform(path[1]);

                temp = path[2](point, 1);

                return secondStep(temp, height / 2 - 1);
            }
            else
            {
                return new Point2D(0, 0);
            }
            //return temp;
        }

        public static Point2D LebesgueSquare(Point2D point, int pointIndex, int height, int width, BasicPath path)
        {
            if (height != width)
            {
                throw new NotImplementedException();
            }

            Point2D temp;

            if (pointIndex == 0)
            {
                temp = path[0](point, height / 2);

                temp = path[1](temp, -(height / 2 - 1));
            }
            else if (pointIndex == 1)
            {
                return path[1](point, 1);
            }
            else if (pointIndex == 2)
            {
                temp = path[2](point, height / 2);

                temp = path[1](temp, -(height / 2 - 1));
            }
            else
            {
                return new Point2D(0, 0);
            }

            return temp;
        }
    }
}
