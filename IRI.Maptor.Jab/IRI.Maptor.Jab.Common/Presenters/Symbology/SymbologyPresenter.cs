using System;
using IRI.Maptor.Jab.Common.Assets.Commands;

namespace IRI.Maptor.Jab.Common.Presenters.Symbology;

public class SymbologyPresenter : Notifier
{
    private VisualParameters _symbology;

    public VisualParameters Symbology
    {
        get { return _symbology; }
        set
        {
            _symbology = value;
            RaisePropertyChanged();
        }
    }

    public Action RequestCloseAction;

    public Action<SymbologyPresenter> RequestApplyAction;

    private RelayCommand _closeCommand;

    public RelayCommand CloseCommand
    {
        get
        {
            if (_closeCommand == null)
            {
                _closeCommand = new RelayCommand(param =>
                {
                    this.RequestCloseAction?.Invoke();
                });
            }

            return _closeCommand;
        }
    }

    private RelayCommand _applyCommand;

    public RelayCommand ApplyCommand
    {
        get
        {
            if (_applyCommand == null)
            {
                _applyCommand = new RelayCommand(param =>
                {
                    this.RequestApplyAction?.Invoke(this);
                });
            }

            return _applyCommand;
        }
    }

}
