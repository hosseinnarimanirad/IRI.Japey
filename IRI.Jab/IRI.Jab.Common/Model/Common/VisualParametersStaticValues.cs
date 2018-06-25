using IRI.Jab.Common.Model;
using IRI.Jab.Common.Extensions;
using IRI.Jab.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using IRI.Ket.SqlServerSpatialExtension.Model;
using IRI.Ket.SpatialExtensions;

namespace IRI.Jab.Common
{
    public partial class VisualParameters
    {
        public static VisualParameters GetFill(Color fill, double opacity = 1)
        {
            return new VisualParameters(new SolidColorBrush(fill), null, 0, opacity);
        }

        public static VisualParameters GetStroke(Color stroke, double strokeThickness = 1, double opacity = 1)
        {
            return new VisualParameters(null, new SolidColorBrush(stroke), strokeThickness, opacity);
        }

        public static VisualParameters Get(Color fill, Color stroke, double strokeThickness, double opacity = 1)
        {
            return new VisualParameters(new SolidColorBrush(fill), new SolidColorBrush(stroke), strokeThickness, opacity);
        }


        public static VisualParameters GetDefaultForDrawing(DrawMode mode)
        {
            var result = new VisualParameters(mode == DrawMode.Polygon ? DefaultDrawingFill : null, DefaultDrawingStroke, 2, .7);

            return result;
        }

        public static VisualParameters GetDefaultForSelection()
        {
            return new VisualParameters(DefaultSelectionFill, DefaultSelectionStroke, 2, .8);
        }

        public static VisualParameters GetDefaultForHighlight()
        {
            return new VisualParameters(DefaultHighlightFill, DefaultHighlightStroke, 2, .8);
        }


        internal static VisualParameters GetDefaultForHighlight(ISqlGeometryAware sqlGeometryAware)
        {
            VisualParameters result;

            if (sqlGeometryAware?.TheSqlGeometry?.IsPointOrMultiPoint() == true)
            {
                result = new VisualParameters(DefaultHighlightStroke, DefaultHighlightFill, 2, .8) { PointSymbol = new Model.Symbology.SimplePointSymbol(10) };
            }
            else
            {
                result = GetDefaultForHighlight();
            }

            return result;

        }


        public static SolidColorBrush DefaultHighlightStroke = new SolidColorBrush(Colors.Yellow);

        public static SolidColorBrush DefaultHighlightFill = new SolidColorBrush(new Color() { B = 0, G = 255, R = 255, A = 50 });

        public static SolidColorBrush DefaultSelectionStroke = new SolidColorBrush(Colors.Cyan);

        public static SolidColorBrush DefaultSelectionFill = new SolidColorBrush(new Color() { B = 255, G = 255, R = 0, A = 50 });


        public static SolidColorBrush DefaultDrawingStroke = new SolidColorBrush(new Color() { R = 255, G = 200, B = 0, A = 250 });

        public static SolidColorBrush DefaultDrawingFill = new SolidColorBrush(new Color() { R = 255, G = 200, B = 0, A = 160 });


        public static DashStyle GetDefaultDashStyleForMeasurements()
        {
            return new DashStyle(new double[] { 2, 1 }, 0);
        }

        public static VisualParameters GetDefaultForMeasurements()
        {

            return new VisualParameters(
                BrushHelper.CreateBrush(ColorHelper.ToWpfColor("#FBB03B"), 0.3),
                BrushHelper.CreateBrush("#FBB03B"),
                3,
                1,
                System.Windows.Visibility.Visible)
            {
                DashStyle = VisualParameters.GetDefaultDashStyleForMeasurements()
            };
        }

        public static VisualParameters GetRandomVisualParameters()
        {
            return new VisualParameters(null, BrushHelper.PickGoodBrush(), 4, 1);
        }

    }
}
