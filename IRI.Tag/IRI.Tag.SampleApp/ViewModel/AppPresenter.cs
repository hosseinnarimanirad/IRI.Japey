using IRI.Jab.Common;
using IRI.Jab.Common.Assets.Commands;
using IRI.Jab.Common.Model;
using IRI.Jab.Common.View.MapMarkers;
using IRI.Jab.Controls.Presenter;
using IRI.Msh.Common.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Tag.SampleApp.ViewModel
{
    public class AppPresenter : MapApplicationPresenter<object>
    {

        public void ShowSpecialPointLayerSample()
        {
            var markers = new List<Locateable>()
            {
                new Jab.Common.Locateable(new Point(48, 30), AncherFunctionHandlers.CenterCenter) { Element = new MapMarker("A") },
                new Jab.Common.Locateable(new Point(49, 31), AncherFunctionHandlers.CenterCenter) { Element = new MapMarker("B") },
                new Jab.Common.Locateable(new Point(50, 32), AncherFunctionHandlers.CenterCenter) { Element = new PointMarker("C") },
                new Jab.Common.Locateable(new Point(51, 33), AncherFunctionHandlers.CenterCenter) { Element = new RectangeMarker() },
                new Jab.Common.Locateable(new Point(52, 34), AncherFunctionHandlers.CenterCenter) { Element = new LabelMarker("E", true) }
            };

            var pointLayer =
                new Jab.Common.SpecialPointLayer(
                    name: "poi",
                    items: markers,
                    opacity: 0.8,
                    visibleRange: ScaleInterval.All,
                    type: LayerType.Complex);

            this.AddLayer(pointLayer);

            this.ZoomToExtent(pointLayer.Extent, isExactExtent: false);
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
