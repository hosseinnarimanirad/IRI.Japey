﻿using IRI.Jab.Common.Assets.Commands;
using IRI.Jab.Common.Presenter.Map;
using IRI.Jab.Common.ViewModel.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Jab.Controls.Presenter
{
    public class MapApplicationPresenter<TUser> : MapPresenter where TUser : class
    {
        public MapApplicationPresenter()
        {

        }

        //private AccountPresenter<TUser> _account;

        //public AccountPresenter<TUser> Account
        //{
        //    get { return _account; }
        //    set
        //    {
        //        _account = value;
        //        RaisePropertyChanged();
        //    }
        //}
        private AccountDialogViewModel _account;

        public AccountDialogViewModel Account
        {
            get { return _account; }
            set
            {
                _account = value;
                RaisePropertyChanged();
            }
        }

        public virtual void Initialize(System.Windows.Window ownerWindow)
        {
            this.DialogService = new IRI.Jab.Controls.Services.Dialog.DefaultDialogService(ownerWindow);
 
            this.RequestShowGoToView = IRI.Jab.Controls.Common.Defaults.DefaultActions.GetDefaultGoToAction(ownerWindow, this);

            this.RequestShowSymbologyView = layer => Common.Defaults.DefaultActions.GetDefaultShowSymbologyView(ownerWindow, layer, this);
        }

        public override async Task Initialize()
        {
            this.RequestClearAll = () =>
            {
                this.ClearAll();// (new Predicate<ILayer>(l => l.CanUserDelete == true), true);
            };

            // 1400.03.04
            //this.DialogService = new IRI.Jab.Controls.Services.Dialog.DefaultDialogService();

            //13971106
            //this.RequestOpenFile = (filter) =>
            //{
            //    Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog() { Filter = filter };

            //    if (dialog.ShowDialog() == true)
            //        return dialog.FileName;
            //    else
            //        return string.Empty;
            //};

            //this.RequestSaveFile = (filter) =>
            //{
            //    Microsoft.Win32.SaveFileDialog dialog = new Microsoft.Win32.SaveFileDialog() { Filter = filter };

            //    if (dialog.ShowDialog() == true)
            //        return dialog.FileName;
            //    else
            //        return string.Empty;
            //};

            //this.RequestShowMessage = msg => System.Windows.MessageBox.Show(msg);

            this.MapSettings.BaseMapCacheDirectory = Environment.CurrentDirectory + "\\Data";

            this.MapSettings.MaxGoogleZoomLevel = 18;
            this.MapSettings.MinGoogleZoomLevel = 4;

            //await this.SetTileBaseMap("GOOGLESATELLITE");
            //this.SelectedMapProvider = this.MapProviders?.FirstOrDefault();

            //this.BaseMapType = IRI.Jab.Common.TileServices.TileType.Hybrid;
            this.SetMapCursorSet1();

            this.RegisterMapOptions();

            this.IsPanMode = true;
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
