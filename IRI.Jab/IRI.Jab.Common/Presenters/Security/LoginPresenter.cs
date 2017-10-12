using IRI.Jab.Common.Assets.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace IRI.Jab.Common.Presenters.Security
{
    public class LoginPresenter : Notifier
    {
        private Guid _id;

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

        private string _hashedPassword;

        public string HashedPassword
        {
            get { return _hashedPassword; }
            set
            {
                _hashedPassword = value;
                RaisePropertyChanged();
            }
        }


        private string _newUserName;

        public string NewUserName
        {
            get { return _newUserName; }
            set
            {
                _newUserName = value;
                RaisePropertyChanged();
            }
        }


        private string _newHashedPassword;

        public string NewHashedPassword
        {
            get { return _newHashedPassword; }
            set
            {
                _newHashedPassword = value;
                RaisePropertyChanged();
            }
        }


        #region Events

        private event EventHandler _onRequestClose;

        public event EventHandler OnRequestClose
        {
            add
            {
                if (_onRequestClose == null)
                {
                    _onRequestClose += value;
                }
            }
            remove
            {
                _onRequestClose -= value;
            }
        }

        public event EventHandler _onRequestLogin;

        public event EventHandler OnRequestLogin
        {
            add
            {
                if (_onRequestLogin == null)
                {
                    _onRequestLogin += value;
                }
            }
            remove
            {
                _onRequestLogin -= value;
            }
        }

        public event EventHandler _onRequestCancel;

        public event EventHandler OnRequestCancel
        {
            add
            {
                if (_onRequestCancel == null)
                {
                    _onRequestCancel += value;
                }
            }
            remove
            {
                _onRequestCancel -= value;
            }
        }

        public event EventHandler _onRequestLoginAsGuest;

        public event EventHandler OnRequestLoginAsGuest
        {
            add
            {
                if (_onRequestLoginAsGuest == null)
                {
                    _onRequestLoginAsGuest += value;
                }
            }
            remove
            {
                _onRequestLoginAsGuest -= value;
            }
        }

        #endregion

        #region Commands
         
        private RelayCommand _closeCommand;

        public RelayCommand CloseCommand
        {
            get
            {
                return this._closeCommand ?? (this._closeCommand = new RelayCommand(param => this._onRequestClose.SafeInvoke(this)));
            }
        }
         

        private RelayCommand _loginCommand;

        public RelayCommand LoginCommand
        {
            get
            {
                return this._loginCommand ??
                    (this._loginCommand = new RelayCommand(x => this._onRequestLogin.SafeInvoke(this),
                                            x => { return !string.IsNullOrEmpty(this.UserName) && !string.IsNullOrEmpty(this.HashedPassword); }));
            }
        }
         

        private RelayCommand _cancelCommand;

        public RelayCommand CancelCommand
        {
            get
            {
                return _cancelCommand ?? (_cancelCommand = new RelayCommand(param => this._onRequestCancel.SafeInvoke(this)));
            }
        }
         

        private RelayCommand _loginGuestCommand;

        public RelayCommand LoginGuestCommand
        {
            get
            {
                return this._loginGuestCommand ?? (this._loginGuestCommand = new RelayCommand(x => this._onRequestLoginAsGuest.SafeInvoke(this)));
            }
        }

        #endregion
    }
}
