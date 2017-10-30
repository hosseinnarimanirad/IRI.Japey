using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRI.Jab.Common.Extensions;
using System.Windows;

namespace IRI.Jab.Cartography.Model
{
    public class EditableFeatureLayerOptions
    {
        static readonly Brush _stroke = BrushHelper.FromHex("#FF1CA1E2");
        static readonly Brush _fill = BrushHelper.FromHex("#661CA1E2");

        public bool IsNewDrawing { get; set; } = false;

        public bool IsVerticesLabelVisible { get; set; } = false;

        public bool IsEdgeLengthVisible { get; set; } = false;

        //public Brush Fill { get; set; } = _stroke;

        //public Brush Stroke { get; set; } = _fill;

        //public double StrokeThickness { get; set; } = 4;

        public ScaleInterval VisibleRange { get; set; } = ScaleInterval.All;

        public VisualParameters Visual { get; set; } = new VisualParameters(_fill, _stroke, 4, .9, Visibility.Visible);

        public Func<FrameworkElement> MakePrimaryVertex { get; set; } = () => new Common.View.MapMarkers.Circle(1);

        public Func<FrameworkElement> MakeSecondaryVertex { get; set; } = () => new Common.View.MapMarkers.Circle(.6);

        public EditableFeatureLayerOptions()
        {

        }

        public static EditableFeatureLayerOptions CreateDefault() => new EditableFeatureLayerOptions();
    }
}
