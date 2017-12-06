using IRI.Ham.SpatialBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRI.Ket.WorldfileFormat
{
    public class Worldfile
    {
        private double _xPixelSize;

        public double XPixelSize
        {
            get { return _xPixelSize; }
            set { _xPixelSize = value; }
        }

        private double _yPixelSize;

        public double YPixelSize
        {
            get { return _yPixelSize; }
            set { _yPixelSize = value; }
        }

        private double _xRotation;

        public double XRotation
        {
            get { return _xRotation; }
            set { _xRotation = value; }
        }

        private double _yRotation;

        public double YRotation
        {
            get { return _yRotation; }
            set { _yRotation = value; }
        }

        public double GroundXMin
        {
            get => CenterOfUpperLeftPixel.X - XPixelSize / 2.0;
        }

        public double GroundYMax
        {
            get => CenterOfUpperLeftPixel.Y + YPixelSize / 2.0;
        }

        public BoundingBox GetBoundingBox(int imagePixelWidth, int imagePixelHeight)
        {
            return new BoundingBox(xMin: GroundXMin,
                                    yMin: (CenterOfUpperLeftPixel.Y + YPixelSize / 2.0) - YPixelSize * imagePixelHeight,
                                    xMax: (CenterOfUpperLeftPixel.X - XPixelSize / 2.0) + XPixelSize * imagePixelWidth,
                                    yMax: GroundYMax);
        }

        public Point ToImageCoordinate(Point groundCoordinate, int imagePixelWidth, int imagePixelHeight)
        {
            var groundWidth = XPixelSize * imagePixelWidth;

            var groundHeight = YPixelSize * imagePixelHeight;

            var x = (groundCoordinate.X - GroundXMin) * imagePixelWidth / groundWidth;

            var y = (GroundYMax - groundCoordinate.Y) * imagePixelHeight / groundHeight;

            return new Point(x, y);
        }


        public Point ToGroundCoordinate(Point imageCoordinate, int imagePixelWidth, int imagePixelHeight)
        {
            var groundWidth = XPixelSize * imagePixelWidth;

            var groundHeight = YPixelSize * imagePixelHeight;

            var x = (imageCoordinate.X) * groundWidth / imagePixelWidth + GroundXMin;

            var y = GroundYMax - imageCoordinate.Y * groundHeight / imagePixelHeight;

            return new Point(x, y);
        }


        private IRI.Ham.SpatialBase.Point _centerOfUpperLeftPixel;

        public IRI.Ham.SpatialBase.Point CenterOfUpperLeftPixel
        {
            get { return _centerOfUpperLeftPixel; }
            set { _centerOfUpperLeftPixel = value; }
        }

        public Worldfile(double xPixelSize, double yPixelSize, IRI.Ham.SpatialBase.Point centerOfUpperLeftPixel)
            : this(xPixelSize, yPixelSize, 0, 0, centerOfUpperLeftPixel)
        {

        }

        public Worldfile(double xPixelSize, double yPixelSize, double xRotation, double yRotation, IRI.Ham.SpatialBase.Point centerOfUpperLeftPixel)
        {
            this._xPixelSize = xPixelSize;

            this._yPixelSize = yPixelSize;

            this._centerOfUpperLeftPixel = centerOfUpperLeftPixel;

            this._xRotation = xRotation;

            this._yRotation = yRotation;
        }

        public static Worldfile Read(string worldFileName)
        {
            string[] lines = System.IO.File.ReadAllLines(worldFileName);

            double xPixelSize = double.Parse(lines[0]);

            double rotationAboutY = double.Parse(lines[1]);

            double rotationAboutX = double.Parse(lines[2]);

            double yPixelSize = double.Parse(lines[3]);

            yPixelSize = (yPixelSize > 0) ? yPixelSize : -yPixelSize;

            double xOfCenterOfUpperLeftPixel = double.Parse(lines[4]);

            double yOfCenterOfUpperLeftPixel = double.Parse(lines[5]);

            return new Worldfile(xPixelSize, yPixelSize, rotationAboutX, rotationAboutY, new Ham.SpatialBase.Point(xOfCenterOfUpperLeftPixel, yOfCenterOfUpperLeftPixel));
        }

        public override string ToString()
        {
            return $"Upper Left Center: {CenterOfUpperLeftPixel}, XPixelSize: {XPixelSize}, YPixelSize: {YPixelSize}";
        }
    }
}
