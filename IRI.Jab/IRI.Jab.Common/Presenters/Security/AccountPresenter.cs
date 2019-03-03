using IRI.Jab.Common.Assets.Commands;
using IRI.Jab.Common.Model.Security;
using IRI.Jab.Common.Service.Dialog;
using IRI.Jab.Common.ViewModel.Dialogs;
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

        IDialogService _dialogService;

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
         
        //public System.Security.SecureString Password { get; private set; }

        //public System.Security.SecureString NewPassword { get; set; }

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

        //public int? CurrentUserId { get; set; }

        public AccountPresenter(IUserRepository<TUser> repository, IDialogService dialogService)
        {
            this._repository = repository;

            this._dialogService = dialogService;
        }

        #region Actions

        public Action RequestLoginWithGoogleOAuth;

        public Action RequestHelpWithForgetPassword;

        public Action RequestClose;

        public Func<IHavePassword, bool> RequestAuthenticate;
          
        public Action<AccountPresenter<TUser>> RequestLoginSuccessAction;

        public Action<AccountPresenter<TUser>> RequestLoginAsGuest;

        public Action RequestSignOut;
         
        public Action<SignUpDialogViewModel> RequestShowSignUpDialogView;

        public Action<ChangePasswordDialogViewModel> RequestShowChangePasswordDialogView;

        public Action RequestResetPassword;

        #endregion


        #region Commands

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


        //private RelayCommand _updateAccountCommand;

        //public RelayCommand UpdateAccountCommand
        //{
        //    get
        //    {
        //        if (this._updateAccountCommand == null)
        //        {
        //            this._updateAccountCommand = new RelayCommand(param => this.UpdateAccount(param));
        //        }

        //        return this._updateAccountCommand;
        //    }
        //}

        //private RelayCommand _closeUpdateAccountCommand;

        //public RelayCommand CloseUpdateAccountCommand
        //{
        //    get
        //    {
        //        if (_closeUpdateAccountCommand == null)
        //        {
        //            _closeUpdateAccountCommand = new RelayCommand(param => this.RequestCloseForUpdateAccountView());
        //        }

        //        return _closeUpdateAccountCommand;
        //    }
        //}


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



        private RelayCommand _showSignUpDialogViewCommand;
         
        public RelayCommand ShowSignUpDialogViewCommand
        {
            get
            {
                if (_showSignUpDialogViewCommand == null)
                {
                    //owner window should be passed via param
                    //sample: CommandParameter="{Binding RelativeSource={RelativeSource Mode=FindAncestor, AncestorType={x:Type esiDb:Shell}}}"
                    _showSignUpDialogViewCommand = new RelayCommand(async param =>
                    {
                        var model = await _dialogService.ShowUserNameSignUpDialog(param);

                        if (model == null)
                        {
                            return;
                        }

                        try
                        {
                            this.RequestShowSignUpDialogView?.Invoke(model);

                            await _dialogService?.ShowMessage(param, null, "کاربر جدید با موفقیت اضافه شد", "پیغام");
                        }
                        catch (Exception ex)
                        {
                            await _dialogService?.ShowMessage(param, null, ex.Message, "خطا");
                        }

                    });
                }

                return _showSignUpDialogViewCommand;
            }
        }
         

        private RelayCommand _showChangePasswordDialogViewCommand;

        public RelayCommand ShowChangePasswordDialogViewCommand
        {
            get
            {
                if (_showChangePasswordDialogViewCommand == null)
                {
                    _showChangePasswordDialogViewCommand = new RelayCommand(async param =>
                    {
                        var model = await _dialogService.ShowChangePasswordDialog(param, RequestAuthenticate);

                        if (model == null)
                        {
                            return;
                        }

                        try
                        {
                            this.RequestShowChangePasswordDialogView?.Invoke(model);

                            await _dialogService?.ShowMessage(param, null, "رمز عبور با موفقیت تغییر یافت", "پیغام");
                        }
                        catch (Exception ex)
                        {
                            await _dialogService?.ShowMessage(param, null, ex.Message, "خطا");
                        }
                    });
                }

                return _showChangePasswordDialogViewCommand;
            }
        }


        //private RelayCommand _signUpCommand;

        //public RelayCommand SignUpCommand
        //{
        //    get
        //    {
        //        if (this._signUpCommand == null)
        //        {
        //            this._signUpCommand = new RelayCommand(param => SignUp(param));
        //        }

        //        return this._signUpCommand;
        //    }
        //}

        //private RelayCommand _closeSignUpCommand;

        //public RelayCommand CloseSignUpCommand
        //{
        //    get
        //    {
        //        if (_closeSignUpCommand == null)
        //        {
        //            _closeSignUpCommand = new RelayCommand(param =>
        //            {
        //                this.RequestCloseForSignUpView?.Invoke();
        //            });
        //        }

        //        return _closeSignUpCommand;
        //    }
        //}


        #endregion

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
                    this.RequestLoginSuccessAction?.Invoke(this);
                }
            }
            else
            {
                this.UserName = string.Empty;

                passContainer.ClearInputValues();
            }
        }

        //private void UpdateAccount(object parameter)
        //{
        //    var model = parameter as IChangePassword;

        //    if (model != null)
        //    {
        //        this.Password = model.Password;

        //        this.NewPassword = model.NewPassword;

        //        //check old password is correct
        //        if (RequestAuthenticate?.Invoke(model) != true)
        //        {
        //            model.ClearInputValues();

        //            return;
        //        }

        //        //update the password
        //        if (NewPassword != null && NewPassword.Length > 0)
        //        {
        //            //check  new password is confirmed
        //            if (!SecureStringHelper.SecureStringEqual(this.NewPassword, model.ConfirmPassword))
        //            {
        //                model.ClearInputValues();

        //                return;
        //            }

        //            this.Password = model.NewPassword;
        //        }

        //        this.RequestUpdate?.Invoke(this);
        //    }

        //    this.RequestClose?.Invoke();
        //}

        //public void SignUp(object parameter)
        //{
        //    var model = parameter as INewUserPassword;

        //    if (model != null)
        //    {

        //        this.NewPassword = model.NewPassword;

        //        //update the password
        //        if (NewPassword != null && NewPassword.Length > 0)
        //        {
        //            //check  new password is confirmed
        //            if (!SecureStringHelper.SecureStringEqual(this.NewPassword, model.ConfirmPassword))
        //            {
        //                model.ClearInputValues();

        //                return;
        //            }

        //            //this.Password = model.NewPassword;
        //            this.RequestSignUp(model);
        //        }

        //    }

        //    this.RequestClose?.Invoke();
        //}
         
    }
}
