using IRI.Jab.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Jab.Common.Presenter
{
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

        public Func<string, string> RequestOpenFile;

        public Func<string, string> RequestSaveFile;

        public Action<string> RequestShowMessage;

        public event EventHandler<string> UserChanged;

        public void ShowMessage(string message)
        {
            this.RequestShowMessage?.Invoke(message);
        }

        public string OpenFile(string arg)
        {
            return this.RequestOpenFile?.Invoke(arg);
        }

        public string SaveFile(string arg)
        {
            return this.RequestSaveFile?.Invoke(arg);
        }

        public void RedirectRequestesTo(BasePresenter presenter)
        {
            if (presenter == this)
            {
                return;
            }

            this.RequestOpenFile = arg => presenter.RequestOpenFile(arg);
            this.RequestSaveFile = arg => presenter.RequestSaveFile(arg);
            this.RequestShowMessage = message => presenter.ShowMessage(message);
        }



    }
}
