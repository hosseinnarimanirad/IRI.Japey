using IRI.Jab.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Jab.Cartography.Presenter
{
    public class BasePresenter : Notifier
    {

        public Func<string, string> RequestOpenFile;

        public Func<string, string> RequestSaveFile;

        public Action<string> RequestShowMessage;


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
