﻿using IRI.Jab.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace IRI.Jab.Common.Model.Symbology
{
    public class SimpleMarkerSymbol : Notifier, ISymbol
    {
        private Brush _fill;

        public Brush Fill
        {
            get { return _fill; }
            set
            {
                _fill = value;
                RaisePropertyChanged();
            }
        }


        private Brush _stroke;

        public Brush Stroke
        {
            get { return _stroke; }
            set
            {
                _stroke = value;
                RaisePropertyChanged();
            }
        }


        private double _strokeThickness;

        public double StrokeThickness
        {
            get { return _strokeThickness; }
            set
            {
                _strokeThickness = value;
                RaisePropertyChanged();
            }
        }


        private double _opacity;

        public double Opacity
        {
            get { return _opacity; }
            set
            {
                _opacity = value;
                RaisePropertyChanged();
            }
        }


        private Geometry _geometrySymbol = null;

        public Geometry GeometrySymbol
        {
            get { return _geometrySymbol; }
            set
            {
                _geometrySymbol = value;
                RaisePropertyChanged();
            }
        }


        private DrawingImage _imageSymbol = null;

        public DrawingImage ImageSymbol
        {
            get { return _imageSymbol; }
            set
            {
                _imageSymbol = value;
                RaisePropertyChanged();
            }
        } 
    }
}
