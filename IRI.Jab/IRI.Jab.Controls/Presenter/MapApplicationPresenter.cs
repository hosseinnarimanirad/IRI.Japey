using IRI.Jab.Common.Presenter.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Jab.Controls.Presenter
{
    public class MapApplicationPresenter : MapPresenter
    {
        public MapApplicationPresenter()
        {

        }

        public  void Initialize(System.Windows.Window ownerWindow)
        {
            this.RequestClearAll = () =>
            {
                this.ClearAll();// (new Predicate<ILayer>(l => l.CanUserDelete == true), true);
            };


            this.RequestOpenFile = (filter) =>
            {
                Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog() { Filter = filter };

                if (dialog.ShowDialog() == true)
                    return dialog.FileName;
                else
                    return string.Empty;
            };

            this.RequestShowMessage = msg => System.Windows.MessageBox.Show(msg);

            this.MapSettings.BaseMapCacheDirectory = Environment.CurrentDirectory + "\\Data";

            this.MapSettings.MaxGoogleZoomLevel = 18;
            this.MapSettings.MinGoogleZoomLevel = 4;

            this.BaseMapType = IRI.Jab.Common.TileServices.TileType.Hybrid;

            this.RequestGoTo = IRI.Jab.Controls.Common.Defaults.DefaultActions.GetDefaultGoToAction(ownerWindow, this);

            this.RegisterMapOptions();
             
        }
    }
}
