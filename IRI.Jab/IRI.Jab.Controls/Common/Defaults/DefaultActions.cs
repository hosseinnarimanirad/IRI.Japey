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
            var gotoView = new IRI.Jab.Controls.View.Symbology.SymbologyView();

            //var gotoPresenter = IRI.Jab.Controls.Presenter.GoToPresenter.Create(mapPresenter);

            gotoView.DataContext = layer;
            gotoView.Owner = ownerWindow;
            gotoView.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            gotoView.Show();

            //gotoPresenter.SelectDefaultMenu();            
        }

    }
}
