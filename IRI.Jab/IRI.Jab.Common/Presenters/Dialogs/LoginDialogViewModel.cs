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
        public LoginDialogViewModel(AuthenticationType type)
        {
            this.Type = type;
        }


        public Action<IHaveEmail> RequestValidateEmailAddress;

        public Action<INewUserEmailPassword> RequestSignUp;

        public Action RequestSignOut;

        public Action RequestLoginWithGoogleOAuth;

        public Action<IHaveEmail> RequestHelpWithForgetPassword;

        public Action RequestLoginAsGuest;

        public Action RequestResetPassword;

        public Func<IUserEmailPassword, Task<bool>> RequestAuthenticate;


        public AuthenticationType Type { get; private set; }

        private string _termsOfUseWebPageUrl;

        public string TermsOfUseWebPageUrl
        {
            get { return _termsOfUseWebPageUrl; }
            set
            {
                _termsOfUseWebPageUrl = value;
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
                    IsNormalMode = false;
                    IsRegisterMode = false;
                    IsEmailVerificationMode = false;
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
                    IsNormalMode = false;
                    IsForgetPasswordMode = false;
                    IsEmailVerificationMode = false;
                }
            }
        }


        private bool _isNormalMode = true;

        public bool IsNormalMode
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
                    IsForgetPasswordMode = false;
                    IsRegisterMode = false;
                    IsEmailVerificationMode = false;
                }
            }
        }

        private bool _isEmailVerificationMode;

        public bool IsEmailVerificationMode
        {
            get { return _isEmailVerificationMode; }
            set
            {
                _isEmailVerificationMode = value;
                RaisePropertyChanged();

                if (value)
                {
                    IsNormalMode = false;
                    IsRegisterMode = false;
                    IsForgetPasswordMode = false;
                }
            }
        }


        //private string _userName;

        //public string UserName
        //{
        //    get { return _userName; }
        //    set
        //    {
        //        _userName = value;
        //        RaisePropertyChanged();
        //    }
        //}


        //private string _emailAddress;

        //public string EmailAddress
        //{
        //    get { return _emailAddress; }
        //    set
        //    {
        //        _emailAddress = value;
        //        RaisePropertyChanged();
        //    }
        //}


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
                        this.RequestHelpWithForgetPassword?.Invoke(param as IHaveEmail);
                    });
                }

                return _helpWithForgetPasswordCommand;
            }
        }

        private RelayCommand _verifyEmailAddressCommand;

        public RelayCommand VerifyEmailAddressCommand
        {
            get
            {
                if (_verifyEmailAddressCommand == null)
                {
                    _verifyEmailAddressCommand = new RelayCommand(param =>
                    {
                        this.RequestValidateEmailAddress?.Invoke(param as IHaveEmail);
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
                        System.Diagnostics.Process.Start(TermsOfUseWebPageUrl);
                    });
                }

                return _goToTermsOfUserWebPage;
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


        private RelayCommand _goToNormalModeCommand;

        public RelayCommand GoToNormalModeCommand
        {
            get
            {
                if (_goToNormalModeCommand == null)
                {
                    _goToNormalModeCommand = new RelayCommand(param =>
                    {
                        IsNormalMode = true;
                    });
                }

                return _goToNormalModeCommand;
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

       

        protected async void Login(object parameter)
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
                        if (await this.RequestAuthenticate(passContainer) != true)
                        {
                            passContainer.ClearInputValues();
                        }
                        else
                        {
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
