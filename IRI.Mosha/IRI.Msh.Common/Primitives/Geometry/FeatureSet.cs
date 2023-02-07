using IRI.Msh.Common.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace IRI.Msh.Common.Primitives
{
    public class FeatureSet
    {
        public string Title { get; set; }

        public int Srid { get; set; }

        public List<Field> Fields { get; set; }

        public List<Feature<Point>> Features { get; set; }
         
        public FeatureSet(int srid)
        {
            this.Srid = srid;

            this.Fields = new List<Field>();
        }
    }

    public class FeatureSet<T> where T : IGeometryAware<Point>
    {
        public string Title { get; set; }

        public int Srid { get; set; }

        public List<Field> Fields { get; set; }

        public List<T> Features { get; set; }

        public FeatureSet(List<T> features)
        {
            this.Features = features;

            this.Fields = new List<Field>();
        }

        public FeatureSet()
        {

        }
    }
}
