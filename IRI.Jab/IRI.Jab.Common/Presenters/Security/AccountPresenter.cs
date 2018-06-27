using IRI.Jab.Common.Assets.Commands;
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
    public class AccountPresenter : Notifier
    {
        private Guid _id = Guid.NewGuid();

        public Guid ID => _id;

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

        public System.Security.SecureString Password { get; private set; }

        //private string _hashedPassword;

        //public string HashedPassword
        //{
        //    get { return _hashedPassword; }
        //    set
        //    {
        //        _hashedPassword = value;
        //        RaisePropertyChanged();
        //    }
        //}


        //private string _newUserName;

        //public string NewUserName
        //{
        //    get { return _newUserName; }
        //    set
        //    {
        //        _newUserName = value;
        //        RaisePropertyChanged();
        //    }
        //}


        //private string _newHashedPassword;

        //public string NewHashedPassword
        //{
        //    get { return _newHashedPassword; }
        //    set
        //    {
        //        _newHashedPassword = value;
        //        RaisePropertyChanged();
        //    }
        //}

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

        public System.Security.SecureString NewPassword { get; set; }


        #region Actions

        public Action RequestClose;

        public Action<AccountPresenter> RequestLogin;

        public Action<AccountPresenter> RequestLoginAsGuest;

        public Action RequestSignOut;

        public Action<AccountPresenter> RequestUpdateAccount;

        #endregion

        #region Events

        //private event EventHandler _onRequestLogin;

        //public event EventHandler OnRequestLogin
        //{
        //    add
        //    {
        //        if (_onRequestLogin == null)
        //        {
        //            _onRequestLogin += value;
        //        }
        //    }
        //    remove
        //    {
        //        _onRequestLogin -= value;
        //    }
        //}

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
            var passwords = parameter as object[];

            if (passwords != null || passwords.Count() == 2)
            {
                var oldPassword = passwords[0] as IHavePassword;

                var newPassword = passwords[1] as IHavePassword;

                if (oldPassword != null && newPassword != null)
                {
                    this.Password = oldPassword.Password;

                    this.NewPassword = newPassword.Password;

                    this.RequestUpdateAccount?.Invoke(this);

                    return;
                }
            }

            this.Password = null;

            this.NewPassword = null;

        }



        public static bool AreEqualPassword(SecureString secureString1, SecureString secureString2)
        {
            if (secureString1 == null)
            {
                //throw new ArgumentNullException("s1");
                return false;
            }
            if (secureString2 == null)
            {
                //throw new ArgumentNullException("s2");
                return false;
            }

            if (secureString1.Length != secureString2.Length)
            {
                return false;
            }

            IntPtr ss_bstr1_ptr = IntPtr.Zero;
            IntPtr ss_bstr2_ptr = IntPtr.Zero;

            try
            {
                ss_bstr1_ptr = Marshal.SecureStringToBSTR(secureString1);
                ss_bstr2_ptr = Marshal.SecureStringToBSTR(secureString2);

                String str1 = Marshal.PtrToStringBSTR(ss_bstr1_ptr);
                String str2 = Marshal.PtrToStringBSTR(ss_bstr2_ptr);

                return str1.Equals(str2);
            }
            finally
            {
                if (ss_bstr1_ptr != IntPtr.Zero)
                {
                    Marshal.ZeroFreeBSTR(ss_bstr1_ptr);
                }

                if (ss_bstr2_ptr != IntPtr.Zero)
                {
                    Marshal.ZeroFreeBSTR(ss_bstr2_ptr);
                }
            }
        }

        public static bool Validate(SecureString secureString, string stringValue)
        {
            return AreEqualPassword(secureString, new NetworkCredential("", stringValue).SecurePassword);
        }
    }
}
