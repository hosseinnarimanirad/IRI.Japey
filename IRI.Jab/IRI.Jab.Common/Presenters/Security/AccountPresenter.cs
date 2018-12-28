using IRI.Jab.Common.Assets.Commands;
using IRI.Jab.Common.Model.Security;
using IRI.Msh.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace IRI.Jab.Common.Presenters.Security
{
    public class AccountPresenter<TUser> : Notifier where TUser : class
    {
        IUserRepository<TUser> _repository;

        //private Guid _id = Guid.NewGuid();

        //public Guid ID => _id;

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

        public System.Security.SecureString Password { get; private set; }

        public System.Security.SecureString NewPassword { get; set; }

        private string _message;

        public string Message
        {
            get { return _message; }
            set
            {
                _message = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(IsMessageVisible));
            }
        }

        public bool IsMessageVisible
        {
            get { return Message != null && Message.Length > 0; }
        }


        public AccountPresenter(IUserRepository<TUser> repository)
        {
            this._repository = repository;
        }

        #region Actions

        public Action RequestClose;

        public Action RequestCloseUpdateAccount;

        public Action<AccountPresenter<TUser>> RequestLogin;

        public Action<AccountPresenter<TUser>> RequestLoginAsGuest;

        public Action RequestSignOut;

        public Action<AccountPresenter<TUser>> RequestUpdateAccount;

        #endregion

        #region Events
         

        #endregion

        #region Commands

        private RelayCommand _closeCommand;

        public RelayCommand CloseCommand
        {
            get
            {
                if (this._closeCommand == null)
                {
                    this._closeCommand = new RelayCommand(param => this.RequestClose?.Invoke());
                }

                return this._closeCommand;
            }
        }


        private RelayCommand _loginCommand;

        public RelayCommand LoginCommand
        {
            get
            {
                if (this._loginCommand == null)
                {
                    this._loginCommand = new RelayCommand(param => this.Login(param), param => { return !string.IsNullOrEmpty(this.UserName); });
                }

                return this._loginCommand;
            }
        }

        private RelayCommand _loginGuestCommand;

        public RelayCommand LoginGuestCommand
        {
            get
            {
                if (this._loginGuestCommand == null)
                {
                    this._loginGuestCommand = new RelayCommand(param => this.RequestLoginAsGuest?.Invoke(this));
                }

                return this._loginGuestCommand;
            }
        }


        private RelayCommand _signOutCommand;

        public RelayCommand SignOutCommand
        {
            get
            {
                if (this._signOutCommand == null)
                {
                    this._signOutCommand = new RelayCommand(param => this.RequestSignOut?.Invoke());
                }

                return this._signOutCommand;
            }
        }


        private RelayCommand _updateAccountCommand;

        public RelayCommand UpdateAccountCommand
        {
            get
            {
                if (this._updateAccountCommand == null)
                {
                    this._updateAccountCommand = new RelayCommand(param => this.UpdateAccount(param));
                }

                return this._updateAccountCommand;
            }
        }

        private RelayCommand _closeUpdateAccountCommand;

        public RelayCommand CloseUpdateAccountCommand
        {
            get
            {
                if (_closeUpdateAccountCommand == null)
                {
                    _closeUpdateAccountCommand = new RelayCommand(param => this.RequestCloseUpdateAccount());
                }

                return _closeUpdateAccountCommand;
            }
        }


        #endregion

        protected void Login(object parameter)
        {
            var passContainer = parameter as IHavePassword;

            if (passContainer != null)
            {
                this.Password = passContainer.Password;

                this.RequestLogin?.Invoke(this);
            }
            else
            {
                this.Password = null;
            }
        }

        private void UpdateAccount(object parameter)
        {
            var model = parameter as IChangePassword;

            if (model != null)
            {
                var oldPassword = model.Password;

                var newPassword = model.NewPassword;

                if (!SecureStringHelper.SecureStringEqual(newPassword, model.ConfirmPassword))
                {
                    return;
                }

                if (oldPassword != null && newPassword != null)
                {
                    this.Password = oldPassword;

                    this.NewPassword = newPassword;

                    this.RequestUpdateAccount?.Invoke(this);

                    return;
                }
            }

            this.Password = null;

            this.NewPassword = null;

            this.RequestClose?.Invoke();
        }

        public static bool Validate(SecureString secureString, string stringValue)
        {
            return SecureStringHelper.SecureStringEqual(secureString, new NetworkCredential("", stringValue).SecurePassword);
        }


    }
}
