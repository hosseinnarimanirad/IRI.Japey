﻿using IRI.Extensions;
using IRI.Msh.Common.Model;
using IRI.Msh.CoordinateSystem.MapProjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRI.Msh.Common.Primitives
{
    public class FeatureSet<T> where T : IPoint, new()
    {
        public Guid LayerId { get; set; }

        public string Title { get; set; }

        public int Srid { get; set; }

        public List<Field> Fields { get; set; }

        public List<Feature<T>> Features { get; set; }


        public FeatureSet(List<Feature<T>> features)
        {
            this.Features = features;

            this.Fields = new List<Field>();
        }

        protected FeatureSet() { }

        public static FeatureSet<T> Create(string title, List<Feature<T>> features)
        {
            if (features.IsNullOrEmpty())
                throw new NotImplementedException("FeatureSet<TGeometry, TPoint> => empty features not allowed");

            if (features.Select(f => f.TheGeometry.Srid).Distinct().Count() > 1)
                throw new NotImplementedException("FeatureSet<TGeometry, TPoint> => same SRID rule violated");

            return new FeatureSet<T>()
            {
                Title = title,
                Features = features,
                Srid = features.First().TheGeometry.Srid
            };
        }

    }

    public class FeatureSet : FeatureSet<Point>
    {

    }

}
