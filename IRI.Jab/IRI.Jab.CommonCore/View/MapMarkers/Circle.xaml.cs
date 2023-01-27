﻿using IRI.Jab.Common.Model.MapMarkers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace IRI.Jab.Common.View.MapMarkers
{
    /// <summary>
    /// Interaction logic for Circle.xaml
    /// </summary>
    public partial class Circle : UserControl, IMapMarker
    {
        public Circle(double opacity = 1, Brush stroke = null, Brush fill = null, double strokeThickness = 1, int size = 12)
        {
            InitializeComponent();

            this.Opacity = opacity;

            this.ellipse.Opacity = opacity;

            if (stroke != null)
            {
                this.ellipse.Stroke = stroke;
            }

            if (fill != null)
            {
                this.ellipse.Fill = fill;
            }

            this.ellipse.StrokeThickness = strokeThickness;

            this.Width = size;

            this.Height = size;

            this._unSelectedFill = this.ellipse.Fill;

            this._unSelectedStroke = this.ellipse.Stroke;

            this._unSelectedStrokeThickness = this.ellipse.StrokeThickness;
        }

        Brush _unSelectedFill;

        Brush _unSelectedStroke;

        double _unSelectedStrokeThickness;

        private bool _isSelected;

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;

                this.ellipse.Fill = value ? Brushes.Red : _unSelectedFill;

                this.ellipse.Stroke = value ? Brushes.White : _unSelectedStroke;

                this.ellipse.StrokeThickness = value ? _unSelectedStrokeThickness + 1 : _unSelectedStrokeThickness;
            }
        }
    }
}
