using IRI.Jab.Common.Assets.Commands;
using IRI.Jab.Common.Model.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Jab.Common.ViewModel.Dialogs
{
    public class LoginDialogViewModel : DialogViewModelBase
    {
        public Action RequestLoginWithGoogleOAuth;

        public Action RequestHelpWithForgetPassword;

        public Action RequestLoginAsGuest;

        public Action RequestResetPassword;

        public Func<IHavePassword, bool> RequestAuthenticate;


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


        private RelayCommand _clearInputValuesCommand;

        public RelayCommand ClearInputValuesCommand
        {
            get
            {
                if (_clearInputValuesCommand == null)
                {
                    _clearInputValuesCommand = new RelayCommand(param =>
                    {
                        var view = param as ISecurityBase;

                        if (view != null)
                        {
                            view.ClearInputValues();
                        }
                    });
                }

                return _clearInputValuesCommand;
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
                    this._loginGuestCommand = new RelayCommand(param =>
                    {
                        this.RequestLoginAsGuest?.Invoke();

                        this.DialogResult = true;
                    });
                }

                return this._loginGuestCommand;
            }
        }


        private RelayCommand _resetPasswordCommand;

        public RelayCommand ResetPasswordCommand
        {
            get
            {
                if (_resetPasswordCommand == null)
                {
                    _resetPasswordCommand = new RelayCommand(param =>
                    {
                        this.RequestResetPassword?.Invoke();
                    });
                }

                return _resetPasswordCommand;
            }
        }


        private RelayCommand _helpWithForgetPasswordCommand;

        public RelayCommand HelpWithForgetPasswordCommand
        {
            get
            {
                if (_helpWithForgetPasswordCommand == null)
                {
                    _helpWithForgetPasswordCommand = new RelayCommand(param =>
                    {
                        this.RequestHelpWithForgetPassword?.Invoke();
                    });
                }

                return _helpWithForgetPasswordCommand;
            }
        }


        private RelayCommand _loginWithGoogleOAuthCommand;

        public RelayCommand LoginWithGoogleOAuthCommand
        {
            get
            {
                if (_loginWithGoogleOAuthCommand == null)
                {
                    _loginWithGoogleOAuthCommand = new RelayCommand(param =>
                    {
                        this.RequestLoginWithGoogleOAuth?.Invoke();
                    });
                }

                return _loginWithGoogleOAuthCommand;
            }
        }


        protected void Login(object parameter)
        {
            var passContainer = parameter as IHavePassword;

            if (passContainer != null)
            {
                //this.Password = passContainer.Password;

                if (this.RequestAuthenticate?.Invoke(passContainer) != true)
                {
                    this.UserName = string.Empty;

                    passContainer.ClearInputValues();
                }
                else
                {
                    this.DialogResult = true;
                }
            }
            else
            {
                this.UserName = string.Empty;

                passContainer.ClearInputValues();
            }
        }
    }
}
