using IRI.Jab.Common;
using IRI.Ket.ShapefileFormat.EsriType;
using IRI.Ket.SpatialExtensions;
using Microsoft.SqlServer.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.MainProjectWPF.LargeData.Model
{
    public class ShapeCollection : Notifier
    {
        private Guid _id;

        public Guid Id
        {
            get { return _id; }
            set
            {
                _id = value;
                RaisePropertyChanged();
            }
        }

        private string _title;

        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                RaisePropertyChanged();
            }
        }

        private AreaStatistics _statistics;

        public AreaStatistics Statistics
        {
            get { return _statistics; }
            set
            {
                _statistics = value;
                RaisePropertyChanged();
                RaisePropertyChanged("AverageArea");
                RaisePropertyChanged("StandardDeviation");
                RaisePropertyChanged("NumberOfPoints");
                RaisePropertyChanged("Histogram");
                RaisePropertyChanged("Description");
            }
        }

        private AngleStatistics _angleStat;

        public AngleStatistics AngleStat
        {
            get { return _angleStat; }
            set
            {
                _angleStat = value;
                RaisePropertyChanged();
            }
        }


        public List<SqlGeometry> Geometries { get; set; }

        public IEsriShapeCollection Shapes { get; set; }

        public ShapeCollection(IEsriShapeCollection shapes, string title)
        {
            this.Title = title;

            this.Shapes = shapes;

            this.Geometries = shapes.Select(i => i.AsSqlGeometry(0)).ToList();


            this.Statistics = new AreaStatistics(shapes);

            this.AngleStat = new AngleStatistics(shapes);
        }

        public double AverageArea { get { return this.Statistics.GetAverage(); } }

        public double StandardDeviation { get { return this.Statistics.GetStandardDeviation(); } }

        public double NumberOfPoints { get { return this.Statistics.NumberOfPoints; } }

        public double[] Histogram { get { return this.Statistics.GetHistogram(5); } }


        public string Description
        {
            get { return Statistics.GetDescription(); }
        }

    }
}
