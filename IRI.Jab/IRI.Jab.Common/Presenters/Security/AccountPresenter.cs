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
         

        public AccountPresenter(IUserRepository<TUser> repository, IDialogService dialogService)
        {
            this._repository = repository;

            this._dialogService = dialogService;
        }

        #region Actions

        //public Action RequestLoginWithGoogleOAuth;

        //public Action RequestHelpWithForgetPassword;

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


        //private RelayCommand _helpWithForgetPasswordCommand;

        //public RelayCommand HelpWithForgetPasswordCommand
        //{
        //    get
        //    {
        //        if (_helpWithForgetPasswordCommand == null)
        //        {
        //            _helpWithForgetPasswordCommand = new RelayCommand(param =>
        //            {
        //                this.RequestHelpWithForgetPassword?.Invoke();
        //            });
        //        }

        //        return _helpWithForgetPasswordCommand;
        //    }
        //}

        //private RelayCommand _loginWithGoogleOAuthCommand;

        //public RelayCommand LoginWithGoogleOAuthCommand
        //{
        //    get
        //    {
        //        if (_loginWithGoogleOAuthCommand == null)
        //        {
        //            _loginWithGoogleOAuthCommand = new RelayCommand(param =>
        //            {
        //                this.RequestLoginWithGoogleOAuth?.Invoke();
        //            });
        //        }

        //        return _loginWithGoogleOAuthCommand;
        //    }
        //}



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
                        var model = await _dialogService.ShowChangePasswordDialog(param);

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
         
         
    }
}
