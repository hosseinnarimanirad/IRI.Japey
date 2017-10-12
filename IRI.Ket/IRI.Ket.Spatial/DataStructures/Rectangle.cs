using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRI.Ket.Spatial.DataStructures
{
    public struct Rectangle
    {
        public double minX, minY, maxX, maxY;

        public Rectangle(double minX, double minY, double maxX, double maxY)
        {
            if (minX > maxX || minY > maxY)
            {
                throw new NotImplementedException();
            }

            this.minX = minX;

            this.minY = minY;

            this.maxX = maxX;

            this.maxY = maxY;
        }

        public double Width
        {
            get { return this.maxX - this.minX; }
        }

        public double Height
        {
            get { return this.maxY - this.minY; }
        }

        public double CenterX
        {
            get { return (this.minX + this.maxX) / 2; }
        }

        public double CenterY
        {
            get { return (this.minY + this.maxY) / 2; }
        }

        public static Rectangle nilValue = new Rectangle(0, 0, 0, 0);

        public double GetArea()
        {
            return this.Width * this.Height;
        }

        public double GetPerimeter
        {
            get { return (this.Width + this.Height) * 2; }
        }

        public Rectangle Add(Rectangle rectangle)
        {
            return new Rectangle(minX: Math.Min(rectangle.minX, this.minX),
                                    minY: Math.Min(rectangle.minY, this.minY),
                                    maxX: Math.Max(rectangle.maxX, this.maxX),
                                    maxY: Math.Max(rectangle.maxY, this.maxY));
        }

        public static Rectangle operator +(Rectangle first, Rectangle second)
        {
            return first.Add(second);
        }

        public override string ToString()
        {
            //return string.Format("MinX:{0}, MinY:{1}, MaxX:{2}, MaxY:{3}", minX, minY, maxX, maxY);
            return string.Format("CenterX:{0}, CenterY:{1}", CenterX, CenterY);
        }

        public double CalculateCenterDistance(Rectangle other)
        {
            double deltaX = this.CenterX - other.CenterX;

            double deltaY = this.CenterX - other.CenterY;

            return Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
        }


        /// <summary>
        /// How much this rectangle's area grows if we add other to it
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public double GetEnlargementArea(Rectangle other)
        {
            Rectangle temp = this.Add(other);

            return temp.GetArea() - this.GetArea();
        }
    }
}
