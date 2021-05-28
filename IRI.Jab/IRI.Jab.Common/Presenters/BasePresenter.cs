using IRI.Jab.Common;
using IRI.Jab.Common.Service.Dialog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Jab.Common.Presenter
{
    //TO DO: consider replacing Action methods with "IDialogService" 
    public class BasePresenter : Notifier
    {
        public IDialogService DialogService { get; set; }

        private string _userName;

        public string UserName
        {
            get { return _userName; }
            set
            {
                _userName = value;
                RaisePropertyChanged();
            }
        }

        public GenericPrincipal CurrentGenericPrincipal
        {
            get { return System.Threading.Thread.CurrentPrincipal as GenericPrincipal; }
            set
            {
                System.Threading.Thread.CurrentPrincipal = value;

                this.UserName = value.Identity.Name;

                RaisePropertyChanged(nameof(UserName));

                RaisePropertyChanged();

                this.UserChanged?.Invoke(this, this.UserName);
            }
        }

        public event EventHandler<string> UserChanged;


        public Action RequestClose;

        public Action RequestActivateWindow;

        //public Func<string, string> RequestOpenFile;

        //public Func<string, string> RequestSaveFile;

        //public string[] OpenFiles(string filter, object owner = null)
        //{
        //    return DialogService.ShowOpenFilesDialog(filter, owner);
        //    //return this.RequestOpenFile?.Invoke(filter);
        //}

        //public string[] OpenFiles<T>(string filter)
        //{
        //    return DialogService.ShowOpenFilesDialog<T>(filter);
        //    //return this.RequestOpenFile?.Invoke(filter);
        //}

        //public string OpenFile(string filter, object owner = null)
        //{
        //    return DialogService.ShowOpenFileDialog(filter, owner);
        //    //return this.RequestOpenFile?.Invoke(filter);
        //}

        //public string OpenFile<T>(string filter)
        //{
        //    return DialogService.ShowOpenFileDialog<T>(filter);
        //    //return this.RequestOpenFile?.Invoke(filter);
        //}




        //public string SaveFile(string filter, object owner = null)
        //{
        //    return DialogService.ShowSaveFileDialog(filter, owner);
        //    //return this.RequestSaveFile?.Invoke(filter);
        //}

        //public string SaveFile<T>(string filter)
        //{
        //    return DialogService.ShowSaveFileDialog<T>(filter);
        //    //return this.RequestSaveFile?.Invoke(filter);
        //}

        public void RedirectRequestesTo(BasePresenter presenter)
        {
            if (presenter == this)
            {
                return;
            }

            this.DialogService = presenter.DialogService;

            //this.RequestOpenFile = arg => presenter.RequestOpenFile(arg);
            //this.RequestSaveFile = arg => presenter.RequestSaveFile(arg);
            //this.RequestShowMessage = message => presenter.ShowMessage(message);
        }

        //public async Task<bool?> RequestYesNoDialog<T>(string message, string title)
        //{
        //    return await DialogService?.ShowYesNoDialogAsync<T>(message, title);
        //}

        //public async Task<bool?> RequestYesNoDialog(object owner, string message, string title)
        //{
        //    return await DialogService?.ShowYesNoDialogAsync(message, title, owner);
        //}

        //public Action<string> RequestShowMessage;

        //public async Task ShowMessage<T>(string message, string title = null)
        //{
        //    await DialogService?.ShowMessageAsync<T>(message, null, title);
        //}

        //public async Task ShowMessageAsync(object owner, string message, string title = null)
        //{
        //    await DialogService?.ShowMessageAsync(null, message, title, owner);
        //    //this.RequestShowMessage?.Invoke(message);
        //}

    }
}
