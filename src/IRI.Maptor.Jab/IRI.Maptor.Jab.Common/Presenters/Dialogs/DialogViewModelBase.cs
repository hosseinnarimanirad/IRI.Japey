using IRI.Maptor.Jab.Common.Presenter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Maptor.Jab.Common.ViewModel.Dialogs;

public abstract class DialogViewModelBase : BasePresenter
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
