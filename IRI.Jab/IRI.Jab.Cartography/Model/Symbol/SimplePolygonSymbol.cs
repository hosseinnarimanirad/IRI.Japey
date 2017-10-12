using IRI.Jab.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace IRI.Jab.Cartography.Model.Symbol
{
    public class SimplePolygonSymbol : Notifier, ISymbol
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


    }
}
