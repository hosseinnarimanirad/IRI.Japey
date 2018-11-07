using IRI.Jab.Common;
using IRI.Jab.Common.Presenter.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace IRI.Jab.Controls.Common.Defaults
{
    public static class DefaultActions
    {

        public static Action GetDefaultGoToAction(Window ownerWindow, MapPresenter mapPresenter)
        {
            Action result = new Action(() =>
            {
                var gotoView = new IRI.Jab.Controls.View.Input.GoToMetroWindow();

                var gotoPresenter = IRI.Jab.Controls.Presenter.GoToPresenter.Create(mapPresenter);

                gotoView.DataContext = gotoPresenter;
                gotoView.Owner = ownerWindow;
                gotoView.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                gotoView.Show();

                gotoPresenter.SelectDefaultMenu();
            });

            return result;
        }


        public static void GetDefaultShowSymbologyView(Window ownerWindow, ILayer layer)
        {
            var view = new IRI.Jab.Controls.View.Symbology.SymbologyView();

            var presenter = new IRI.Jab.Common.Presenters.Symbology.SymbologyPresenter();

            if (layer is DrawingItemLayer)
            {
                presenter.Symbology = (layer as DrawingItemLayer).OriginalSymbology.Clone();
            }
            else
            {
                presenter.Symbology = layer.VisualParameters.Clone();
            }


            presenter.RequestCloseAction = () => { view.Close(); };

            presenter.RequestApplyAction = p =>
            {
                if (layer is DrawingItemLayer)
                {

                    (layer as DrawingItemLayer).OriginalSymbology.Fill = p.Symbology.Fill;
                    (layer as DrawingItemLayer).OriginalSymbology.Stroke = p.Symbology.Stroke;
                    (layer as DrawingItemLayer).OriginalSymbology.StrokeThickness = p.Symbology.StrokeThickness;

                }
                else
                {
                    layer.VisualParameters.Fill = p.Symbology.Fill;
                    layer.VisualParameters.Stroke = p.Symbology.Stroke;
                    layer.VisualParameters.StrokeThickness = p.Symbology.StrokeThickness;
                }

                view.Close();
            };

            //var gotoPresenter = IRI.Jab.Controls.Presenter.GoToPresenter.Create(mapPresenter);

            view.DataContext = presenter;
            view.Owner = ownerWindow;
            view.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            view.Show();

            //gotoPresenter.SelectDefaultMenu();            
        }

    }
}
