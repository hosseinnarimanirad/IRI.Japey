using IRI.Jab.Common.Assets.Commands;
using IRI.Jab.Common.Model.Security;
using IRI.Ket.Common.Service;
using IRI.Msh.Common.Model.Google;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Jab.Common.ViewModel.Dialogs
{
    public class AccountDialogViewModel : DialogViewModelBase
    {
        public AccountDialogViewModel(AuthenticationType type)
        {
            this.Type = type;
        }

        #region Actions & Funcs

        //login
        public Func<IUserEmailPassword, Task<bool>> RequestAuthenticateAsync;

        public Func<IUserEmailPassword, bool> RequestAuthenticate;

        public Action<Response<GoogleOAuthUserInfoResult>> RequestLoginWithGoogleOAuth;

        public Action RequestLoginAsGuest;

        //sign out/sign up
        public Action<INewUserEmailPassword> RequestSignUp;

        public Action RequestSignOut;

        //password
        public Action<IHaveEmail> RequestHandleForgetPassword;

        public Action RequestResetPassword;

        public Action<IChangePassword> RequestChangePassword;

        //other
        public Action<IHaveEmail> RequestVerifyEmailAddress;

        public Action RequestShowTermsOfUse;

        //dialog
        public Action<SignUpDialogViewModel> RequestShowSignUpDialogView;

        public Action<ChangePasswordDialogViewModel> RequestShowChangePasswordDialogView;

        #endregion

        #region Properties

        private bool _isBusy;

        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                if (value == true)
                {
                    SignUpMessage = string.Empty;
                    LoginMessage = string.Empty;
                }

                _isBusy = value;
                RaisePropertyChanged();
            }
        }

        public AuthenticationType Type { get; private set; }

        //private string _termsOfUseWebPageUrl;

        //public string TermsOfUseWebPageUrl
        //{
        //    get { return _termsOfUseWebPageUrl; }
        //    set
        //    {
        //        _termsOfUseWebPageUrl = value;
        //        RaisePropertyChanged();
        //    }
        //}

        private bool _isSignedIn;

        public bool IsSignedIn
        {
            get { return _isSignedIn; }
            set
            {
                _isSignedIn = value;
                RaisePropertyChanged();
            }
        }


        private bool _confirmTermOfUser;

        public bool ConfirmTermOfUser
        {
            get { return _confirmTermOfUser; }
            set
            {
                _confirmTermOfUser = value;
                RaisePropertyChanged();
            }
        }

        private string _signUpMessage;

        public string SignUpMessage
        {
            get { return _signUpMessage; }
            set
            {
                _signUpMessage = value?.Trim(Environment.NewLine.ToCharArray());
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(HasSignUpMessage));
            }
        }

        public bool HasSignUpMessage
        {
            get { return SignUpMessage?.Length > 0; }
        }

        private string _loginMessage;

        public string LoginMessage
        {
            get { return _loginMessage; }
            set
            {
                _loginMessage = value?.Trim(Environment.NewLine.ToCharArray());
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(HasLoginMessage));
            }
        }

        public bool HasLoginMessage
        {
            get { return LoginMessage?.Length > 0; }
        }


        #endregion

        #region Properties (Mode)

        private bool _isForgetPasswordMode;

        public bool IsForgetPasswordMode
        {
            get { return _isForgetPasswordMode; }
            set
            {
                if (_isForgetPasswordMode == value)
                    return;

                _isForgetPasswordMode = value;
                RaisePropertyChanged();

                if (value)
                {
                    SetMode(nameof(IsForgetPasswordMode));
                }
            }
        }


        private bool _isRegisterMode;

        public bool IsRegisterMode
        {
            get { return _isRegisterMode; }
            set
            {
                if (_isRegisterMode == value)
                    return;

                _isRegisterMode = value;
                RaisePropertyChanged();

                if (value)
                {
                    SetMode(nameof(IsRegisterMode));
                }
            }
        }


        private bool _isNormalMode = true;

        public bool IsLoginMode
        {
            get { return _isNormalMode; }
            set
            {
                if (_isNormalMode == value)
                    return;

                _isNormalMode = value;
                RaisePropertyChanged();

                if (value)
                {
                    SetMode(nameof(IsLoginMode));
                }
            }
        }

        private bool _isEmailVerificationMode;

        public bool IsEmailVerificationMode
        {
            get { return _isEmailVerificationMode; }
            set
            {
                if (_isEmailVerificationMode == value)
                    return;

                _isEmailVerificationMode = value;
                RaisePropertyChanged();

                if (value)
                {
                    SetMode(nameof(IsEmailVerificationMode));
                }
            }
        }

        private bool _isSetPasswordMode;

        public bool IsSetPasswordMode
        {
            get { return _isSetPasswordMode; }
            set
            {
                if (_isSetPasswordMode == value)
                    return;

                _isSetPasswordMode = value;
                RaisePropertyChanged();

                if (value)
                {
                    SetMode(nameof(IsSetPasswordMode));
                }
            }
        }

        private bool _isChangePasswordMode;

        public bool IsChangePasswordMode
        {
            get { return _isChangePasswordMode; }
            set
            {
                if (value == _isChangePasswordMode)
                    return;

                _isChangePasswordMode = value;
                RaisePropertyChanged();

                if (value)
                {
                    SetMode(nameof(IsChangePasswordMode));
                }
            }
        }

        #endregion

        #region Private Methods

        private void SetMode(string modeTitle)
        {
            IsForgetPasswordMode = nameof(IsForgetPasswordMode) == modeTitle;
            IsRegisterMode = nameof(IsRegisterMode) == modeTitle;
            IsLoginMode = nameof(IsLoginMode) == modeTitle;
            IsEmailVerificationMode = nameof(IsEmailVerificationMode) == modeTitle;
            IsSetPasswordMode = nameof(IsSetPasswordMode) == modeTitle;
            IsChangePasswordMode = nameof(IsChangePasswordMode) == modeTitle;
        }

        #endregion

        #region Commands

        //login
        private RelayCommand _loginAsyncCommand;

        public RelayCommand LoginAsyncCommand
        {
            get
            {
                if (this._loginAsyncCommand == null)
                {
                    this._loginAsyncCommand = new RelayCommand(param => this.LoginAsync(param));
                }

                return this._loginAsyncCommand;
            }
        }

        private RelayCommand _loginCommand;

        public RelayCommand LoginCommand
        {
            get
            {
                if (this._loginCommand == null)
                {
                    this._loginCommand = new RelayCommand(param => this.Login(param));
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

        private RelayCommand _loginWithGoogleOAuthCommand;

        public RelayCommand LoginWithGoogleOAuthCommand
        {
            get
            {
                if (_loginWithGoogleOAuthCommand == null)
                {
                    _loginWithGoogleOAuthCommand = new RelayCommand(async param =>
                    {
                        var result = await Ket.Common.Service.Google.GoogleOAuthApi.RunOAuth2();

                        this.RequestLoginWithGoogleOAuth?.Invoke(result);

                    });
                }

                return _loginWithGoogleOAuthCommand;
            }
        }


        //sign out/sign up
        private RelayCommand _signUpCommand;

        public RelayCommand SignUpCommand
        {
            get
            {
                if (this._signUpCommand == null)
                {
                    this._signUpCommand = new RelayCommand(param => this.RequestSignUp?.Invoke(param as INewUserEmailPassword));
                }

                return this._signUpCommand;
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


        //password
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

        private RelayCommand _handleForgetPasswordCommand;

        public RelayCommand HandleForgetPasswordCommand
        {
            get
            {
                if (_handleForgetPasswordCommand == null)
                {
                    _handleForgetPasswordCommand = new RelayCommand(param =>
                    {
                        this.RequestHandleForgetPassword?.Invoke(param as IHaveEmail);
                    });
                }

                return _handleForgetPasswordCommand;
            }
        }

        private RelayCommand _changePasswordCommand;

        public RelayCommand ChangePasswordCommand
        {
            get
            {
                if (_changePasswordCommand == null)
                {
                    _changePasswordCommand = new RelayCommand(param =>
                    {
                        this.RequestChangePassword?.Invoke(param as IChangePassword);
                    });
                }

                return _changePasswordCommand;
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
                        var model = await DialogService.ShowChangePasswordDialog(param);

                        if (model == null)
                        {
                            return;
                        }

                        try
                        {
                            this.RequestShowChangePasswordDialogView?.Invoke(model);

                            await DialogService?.ShowMessage(param, null, "رمز عبور با موفقیت تغییر یافت", "پیغام");
                        }
                        catch (Exception ex)
                        {
                            await DialogService?.ShowMessage(param, null, ex.Message, "خطا");
                        }
                    });
                }

                return _showChangePasswordDialogViewCommand;
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
                        var model = await DialogService.ShowUserNameSignUpDialog(param);

                        if (model == null)
                        {
                            return;
                        }

                        try
                        {
                            this.RequestShowSignUpDialogView?.Invoke(model);

                            await DialogService?.ShowMessage(param, null, "کاربر جدید با موفقیت اضافه شد", "پیغام");
                        }
                        catch (Exception ex)
                        {
                            await DialogService?.ShowMessage(param, null, ex.Message, "خطا");
                        }

                    });
                }

                return _showSignUpDialogViewCommand;
            }
        }


        //other
        private RelayCommand _verifyEmailAddressCommand;

        public RelayCommand VerifyEmailAddressCommand
        {
            get
            {
                if (_verifyEmailAddressCommand == null)
                {
                    _verifyEmailAddressCommand = new RelayCommand(param =>
                    {
                        this.RequestVerifyEmailAddress?.Invoke(param as IHaveEmail);
                    });
                }

                return _verifyEmailAddressCommand;
            }
        }

        private RelayCommand _goToTermsOfUserWebPage;

        public RelayCommand GoToTermsOfUserWebPage
        {
            get
            {
                if (_goToTermsOfUserWebPage == null)
                {
                    _goToTermsOfUserWebPage = new RelayCommand(param =>
                    {
                        //System.Diagnostics.Process.Start(TermsOfUseWebPageUrl);
                        this.RequestShowTermsOfUse?.Invoke();
                    });
                }

                return _goToTermsOfUserWebPage;
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


        #endregion

        #region Commands (Go To Mode)

        private RelayCommand _goToLoginModeCommand;

        public RelayCommand GoToLoginModeCommand
        {
            get
            {
                if (_goToLoginModeCommand == null)
                {
                    _goToLoginModeCommand = new RelayCommand(param =>
                    {
                        IsLoginMode = true;
                    });
                }

                return _goToLoginModeCommand;
            }
        }


        private RelayCommand _goToRegisterModeCommand;

        public RelayCommand GoToRegisterModeCommand
        {
            get
            {
                if (_goToRegisterModeCommand == null)
                {
                    _goToRegisterModeCommand = new RelayCommand(param =>
                    {
                        IsRegisterMode = true;
                    });
                }

                return _goToRegisterModeCommand;
            }
        }


        private RelayCommand _gotoForgetPasswordModeCommand;

        public RelayCommand GoToForgetPasswordModeCommand
        {
            get
            {
                if (_gotoForgetPasswordModeCommand == null)
                {
                    _gotoForgetPasswordModeCommand = new RelayCommand(param =>
                    {
                        IsForgetPasswordMode = true;
                    });
                }

                return _gotoForgetPasswordModeCommand;
            }
        }


        private RelayCommand _gotoEmailVerificationModeCommand;

        public RelayCommand GotoEmailVerificationModeCommand
        {
            get
            {
                if (_gotoEmailVerificationModeCommand == null)
                {
                    _gotoEmailVerificationModeCommand = new RelayCommand(param =>
                    {
                        IsEmailVerificationMode = true;
                    });
                }

                return _gotoEmailVerificationModeCommand;
            }
        }


        private RelayCommand _goToChangePasswordModeCommand;

        public RelayCommand GoToChangePasswordModeCommand
        {
            get
            {
                if (_goToChangePasswordModeCommand == null)
                {
                    _goToChangePasswordModeCommand = new RelayCommand(param =>
                    {
                        IsChangePasswordMode = true;
                    });
                }

                return _goToChangePasswordModeCommand;
            }
        }


        private RelayCommand _goToSetPasswordModeCommand;

        public RelayCommand GoToSetPasswordModeCommand
        {
            get
            {
                if (_goToSetPasswordModeCommand == null)
                {
                    _goToSetPasswordModeCommand = new RelayCommand(param =>
                    {
                        IsSetPasswordMode = true;
                    });
                }

                return _goToSetPasswordModeCommand;
            }
        }

        #endregion


        protected async void LoginAsync(object parameter)
        {
            try
            {
                this.IsBusy = true;

                var passContainer = parameter as IUserEmailPassword;

                if (passContainer != null)
                {
                    //this.Password = passContainer.Password;                

                    if (this.RequestAuthenticateAsync != null)
                    {
                        if (await this.RequestAuthenticateAsync(passContainer) != true)
                        {
                            this.IsSignedIn = false;

                            passContainer.ClearInputValues();
                        }
                        else
                        {
                            this.IsSignedIn = true;

                            this.DialogResult = true;
                        }
                    }
                }
                else
                {
                    passContainer.ClearInputValues();
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                this.IsBusy = false;
            }
        }

        protected void Login(object parameter)
        {
            try
            {
                this.IsBusy = true;

                var passContainer = parameter as IUserEmailPassword;

                if (passContainer != null)
                {
                    //this.Password = passContainer.Password;                

                    if (this.RequestAuthenticate != null)
                    {
                        if (this.RequestAuthenticate(passContainer) != true)
                        {
                            this.IsSignedIn = false;

                            passContainer.ClearInputValues();
                        }
                        else
                        {
                            this.IsSignedIn = true;

                            this.DialogResult = true;
                        }
                    }
                }
                else
                {
                    passContainer.ClearInputValues();
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                this.IsBusy = false;
            }
        }
    }
}
