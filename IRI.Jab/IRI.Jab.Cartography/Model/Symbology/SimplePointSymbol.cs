using IRI.Jab.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using media = System.Windows.Media;

namespace IRI.Jab.Cartography.Model.Symbology
{
    public class SimplePointSymbol : Notifier
    {
        private double _symbolWidth = 16;

        public double SymbolWidth
        {
            get { return _symbolWidth; }
            set
            {
                _symbolWidth = value;
                RaisePropertyChanged();
            }
        }

        private double _symbolHeight = 16;

        public double SymbolHeight
        {
            get { return _symbolWidth; }
            set
            {
                _symbolWidth = value;
                RaisePropertyChanged();
            }
        }


        private Geometry _geometryPointSymbol;

        public Geometry GeometryPointSymbol
        {
            get { return _geometryPointSymbol; }
            set
            {
                _geometryPointSymbol = value;
                RaisePropertyChanged();
            }
        }


        private media.ImageSource _imagePointSymbol;

        public media.ImageSource ImagePointSymbol
        {
            get { return _imagePointSymbol; }
            set
            {
                _imagePointSymbol = value;
                RaisePropertyChanged();
            }
        }


        private System.Drawing.Image _imagePointSymbolGdiPlus;

        public System.Drawing.Image ImagePointSymbolGdiPlus
        {
            get { return _imagePointSymbolGdiPlus; }
            set
            {
                _imagePointSymbolGdiPlus = value;
                RaisePropertyChanged();
            }
        }

        public SimplePointSymbol()
        {

        }

        public SimplePointSymbol(double pointSize)
        {
            this.SymbolHeight = pointSize;

            this.SymbolWidth = pointSize;
        }
    }
}
