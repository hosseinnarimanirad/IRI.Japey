using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Jab.Common.ViewModel.Dialogs
{
    public abstract class DialogViewModelBase : Notifier
    {
        private bool? _dialogResult = false;

        public bool? DialogResult
        {
            get
            {
                return _dialogResult;
            }
            protected set
            {
                _dialogResult = value;

                this.OnSetResult?.Invoke(this, new CustomEventArgs<bool?>(value));
            }
        }
         
        public event EventHandler<CustomEventArgs<bool?>> OnSetResult;
    }
}
