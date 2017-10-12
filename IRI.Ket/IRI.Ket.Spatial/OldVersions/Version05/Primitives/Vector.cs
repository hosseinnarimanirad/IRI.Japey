using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hnr.Spatial.Version05.Primitives
{

    public struct Vector
    {
        int radius;

        double theta;

        public Vector(int radius, double theta)
        {
            this.radius = radius;

            this.theta = theta;
        }

        public int Radius
        {
            get { return this.radius; }
            set { this.radius = value; }
        }

        public double Theta
        {
            get { return this.theta; }
            set { this.theta = value; }
        }
    }
}
