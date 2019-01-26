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


        //public Func<string, string> RequestOpenFile;

        //public Func<string, string> RequestSaveFile;

        public Action<string> RequestShowMessage;

        public void ShowMessage(string message)
        {
            this.RequestShowMessage?.Invoke(message);
        }

        public string OpenFile(string filter)
        {
            return DialogService.ShowOpenFileDialog(filter);
            //return this.RequestOpenFile?.Invoke(filter);
        }

        public string OpenFile<T>(string filter)
        {
            return DialogService.ShowOpenFileDialog<T>(filter);
            //return this.RequestOpenFile?.Invoke(filter);
        }

        public string SaveFile(string filter)
        {
            return DialogService.ShowSaveFileDialog(filter);
            //return this.RequestSaveFile?.Invoke(filter);
        }

        public string SaveFile<T>(string filter)
        {
            return DialogService.ShowSaveFileDialog<T>(filter);
            //return this.RequestSaveFile?.Invoke(filter);
        }

        public void RedirectRequestesTo(BasePresenter presenter)
        {
            if (presenter == this)
            {
                return;
            }

            this.DialogService = presenter.DialogService;

            //this.RequestOpenFile = arg => presenter.RequestOpenFile(arg);
            //this.RequestSaveFile = arg => presenter.RequestSaveFile(arg);
            this.RequestShowMessage = message => presenter.ShowMessage(message);
        }

        public async Task<bool> RequestYesNoDialog<T>(string message, string title)
        {
            return await DialogService?.ShowYesNoDialog<T>(message, title);
        }

        public async Task<bool> RequestYesNoDialog(object owner, string message, string title)
        {
            return await DialogService?.ShowYesNoDialog(owner, message, title);
        }

        public IDialogService DialogService { get; set; }
    }
}
