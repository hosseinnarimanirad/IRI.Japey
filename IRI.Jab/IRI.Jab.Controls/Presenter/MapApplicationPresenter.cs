using IRI.Jab.Common.Assets.Commands;
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

        public void Initialize(System.Windows.Window ownerWindow)
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

            this.RequestSaveFile = (filter) =>
            {
                Microsoft.Win32.SaveFileDialog dialog = new Microsoft.Win32.SaveFileDialog() { Filter = filter };

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

            this.RequestShowGoToView = IRI.Jab.Controls.Common.Defaults.DefaultActions.GetDefaultGoToAction(ownerWindow, this);

            this.RequestShowSymbologyView = layer => Common.Defaults.DefaultActions.GetDefaultShowSymbologyView(ownerWindow, layer);

            this.SetMapCursorSet1();

            this.RegisterMapOptions();

        }


        private void ShowAboutMe()
        {
            this.OnRequestShowAboutMe?.Invoke();
        }


        private RelayCommand _showAboutMeCommand;

        public RelayCommand ShowAboutMeCommand
        {
            get
            {
                if (_showAboutMeCommand == null)
                {
                    _showAboutMeCommand = new RelayCommand(param => this.ShowAboutMe());
                }
                return _showAboutMeCommand;
            }
        }

    }
}
