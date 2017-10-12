using IRI.Jab.Common;
using Microsoft.SqlServer.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace IRI.Jab.Cartography
{
    public class LabelParameters : Notifier
    {
        private ScaleInterval _visibleRange;

        public ScaleInterval VisibleRange
        {
            get { return _visibleRange; }
            set
            {
                _visibleRange = value;
                RaisePropertyChanged();
            }
        }

        private int _fontSize;

        public int FontSize
        {
            get { return _fontSize; }
            set
            {
                _fontSize = value;
                RaisePropertyChanged();
            }
        }

        private Brush _foreground;

        public Brush Foreground
        {
            get { return _foreground; }
            set
            {
                _foreground = value;
                RaisePropertyChanged();
            }
        }

        private FontFamily _fontFamily;

        public FontFamily FontFamily
        {
            get { return _fontFamily; }
            set
            {
                _fontFamily = value;
                RaisePropertyChanged();
            }
        }

        public bool IsLabeled(double scale)
        {
            return scale < this.VisibleRange.Upper && scale > this.VisibleRange.Lower;
        }

        public Func<SqlGeometry, SqlGeometry> PositionFunc { get; set; }

        public LabelParameters(ScaleInterval visibleRange, int fontSize, Brush background, FontFamily fontFamily, Func<SqlGeometry, SqlGeometry> positionFunc)
        {
            this.VisibleRange = visibleRange;

            this.FontSize = fontSize;

            this.Foreground = background;

            this.FontFamily = fontFamily;

            this.PositionFunc = positionFunc;
        }

        public LabelParameters(ScaleInterval visibleRange, int fontSize, Color background, FontFamily fontFamily, Func<SqlGeometry, SqlGeometry> positionFunc)
            : this(visibleRange, fontSize, new SolidColorBrush(background), fontFamily, positionFunc)
        {

        }
    }
}
