using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRI.Jab.Common.Extensions;
using System.Windows;
using IRI.Jab.Common;
using IRI.Jab.Common.Helpers;
using IRI.Jab.Common.Assets.Commands;

namespace IRI.Jab.Cartography.Model
{
    public class EditableFeatureLayerOptions : Notifier
    {
        static readonly Brush _stroke = BrushHelper.Create("#FF1CA1E2");
        static readonly Brush _fill = BrushHelper.Create("#661CA1E2");

        public bool IsNewDrawing { get; set; } = false;

        private bool _isFeatureInfoVisible;

        public bool IsFeatureInfoVisible
        {
            get { return _isFeatureInfoVisible; }
            set
            {
                _isFeatureInfoVisible = value;
                RaisePropertyChanged();
            }
        }


        private bool _isVerticesLabelVisible = false;

        public bool IsVerticesLabelVisible
        {
            get { return _isVerticesLabelVisible && IsVerticesVisible; }
            set
            {
                _isVerticesLabelVisible = value;
                RaisePropertyChanged();
            }
        }


        private bool _isEdgeLabelVisible = false;

        public bool IsEdgeLabelVisible
        {
            get { return _isEdgeLabelVisible; }
            set
            {
                _isEdgeLabelVisible = value;
                RaisePropertyChanged();
                this.RequestHandleIsEdgeLabelVisibleChanged?.Invoke();
            }
        }

        private bool _isMeasureVisible = false;

        public bool IsMeasureVisible
        {
            get { return _isMeasureVisible; }
            set
            {
                _isMeasureVisible = value;
                RaisePropertyChanged();
            }
        }


        private bool _isVerticesVisible;

        public bool IsVerticesVisible
        {
            get { return _isVerticesVisible; }
            set
            {
                _isVerticesVisible = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(IsVerticesLabelVisible));
            }
        }

        private bool _isAutoMeasureEnabled = false;

        public bool IsAutoMeasureEnabled
        {
            get { return _isAutoMeasureEnabled; }
            set
            {
                _isAutoMeasureEnabled = value;
                RaisePropertyChanged();
            }
        }

        private bool _isFinishButtonVisible = true;

        public bool IsFinishButtonVisible
        {
            get { return _isFinishButtonVisible; }
            set
            {
                _isFinishButtonVisible = value;
                RaisePropertyChanged();
            }
        }

        private bool _isCancelButtonVisible = true;

        public bool IsCancelButtonVisible
        {
            get { return _isCancelButtonVisible; }
            set
            {
                _isCancelButtonVisible = value;
                RaisePropertyChanged();
            }
        }

        private bool _isDeleteButtonVisible;

        public bool IsDeleteButtonVisible
        {
            get { return _isDeleteButtonVisible; }
            set
            {
                _isDeleteButtonVisible = value;
                RaisePropertyChanged();
            }
        }

        private bool _isMeasureButtonVisible;

        public bool IsMeasureButtonVisible
        {
            get { return _isMeasureButtonVisible; }
            set
            {
                _isMeasureButtonVisible = value;
                RaisePropertyChanged();
            }
        }

        private string _editText;

        public string EditText
        {
            get { return _editText; }
            set
            {
                _editText = value;
                RaisePropertyChanged();
            }
        }


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

        public Action RequestHandleIsEdgeLabelVisibleChanged;

       



       

    }
}
