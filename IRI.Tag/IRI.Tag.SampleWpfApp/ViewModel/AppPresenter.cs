using IRI.Jab.Common;
using IRI.Jab.Common.Assets.Commands;
using IRI.Jab.Common.Model;
using IRI.Jab.Common.View.MapMarkers;
using IRI.Jab.Controls.Presenter;
using IRI.Sta.Common.Primitives;
using System.Collections.Generic;

namespace IRI.Tag.SampleWpfApp.ViewModel
{
    public class AppPresenter : MapApplicationPresenter<object>
    {

        public void ShowSpecialPointLayerSample()
        {
            var markers = new List<Locateable>()
            {
                new Locateable(new Point(48, 30), AncherFunctionHandlers.BottomCenter) { Element = new MapMarker("A") },
                new Locateable(new Point(49, 31), AncherFunctionHandlers.BottomCenter) { Element = new MapMarker("B") },
                new Locateable(new Point(50, 32), AncherFunctionHandlers.CenterCenter) { Element = new PointMarker("C") },
                new Locateable(new Point(51, 33), AncherFunctionHandlers.CenterCenter) { Element = new RectangeMarker() },
                new Locateable(new Point(52, 34), AncherFunctionHandlers.BottomCenter) { Element = new LabelMarker("E", true) }
            };

            var pointLayer =
                new SpecialPointLayer(
                    name: "poi",
                    items: markers,
                    opacity: 0.8,
                    visibleRange: ScaleInterval.All,
                    type: LayerType.Complex);

            this.AddLayer(pointLayer);

            this.ZoomToExtent(pointLayer.Extent, isExactExtent: false, isNewExtent: true);
        }



        #region Commands

        private RelayCommand _showSpecialPointLayerSampleCommand;

        public RelayCommand ShowSpecialPointLayerSampleCommand
        {
            get
            {
                if (_showSpecialPointLayerSampleCommand == null)
                {
                    _showSpecialPointLayerSampleCommand = new RelayCommand(param =>
                    {
                        ShowSpecialPointLayerSample();
                    });
                }

                return _showSpecialPointLayerSampleCommand;
            }
        }

        #endregion

    }
}
