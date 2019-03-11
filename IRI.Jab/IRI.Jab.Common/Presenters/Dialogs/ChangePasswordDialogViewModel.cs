using IRI.Jab.Common.Assets.Commands;
using IRI.Jab.Common.Model.Security;
using IRI.Msh.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Jab.Common.ViewModel.Dialogs
{
    public class ChangePasswordDialogViewModel : Notifier
    {
        public Action RequestClose;

        //public Func<IHavePassword, bool> RequestAuthenticate;

        public bool IsOk { get; private set; } = false;

        private string _title;

        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                RaisePropertyChanged();
            }
        }


        private string _bannerText;

        public string BannerText
        {
            get { return _bannerText; }
            set
            {
                _bannerText = value;
                RaisePropertyChanged();
            }
        }


        private string _footerText;

        public string FooterText
        {
            get { return _footerText; }
            set
            {
                _footerText = value;
                RaisePropertyChanged();
            }
        }

        public ChangePasswordDialogViewModel(Action requestClose)
        {
            this.RequestClose = requestClose;

            //this.RequestAuthenticate = requestAuthenticate;
        }


        private string _oldPassword;

        public string OldPassword
        {
            get { return _oldPassword; }
            set
            {
                _oldPassword = value;
                RaisePropertyChanged();
            }
        }


        private string _newPassword;

        public string NewPassword
        {
            get { return _newPassword; }
            set
            {
                _newPassword = value;
                RaisePropertyChanged();
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
                        var model = param as IChangePassword;

                        if (model == null)
                        {
                            return;
                        }

                        this.OldPassword = SecureStringHelper.GetString(model.Password);

                        //if (model.IsNewPasswordValid() && this.RequestAuthenticate?.Invoke(model) == true)
                        if (model.IsNewPasswordValid())
                        {
                            this.NewPassword = SecureStringHelper.GetString(model.NewPassword);

                            this.IsOk = true;

                            this.RequestClose?.Invoke();
                        }
                        else
                        {
                            model.ClearInputValues();

                            return;
                        }

                    });
                }

                return _changePasswordCommand;
            }
        }


        private RelayCommand _cancelCommand;

        public RelayCommand CancelCommand
        {
            get
            {
                if (_cancelCommand == null)
                {
                    _cancelCommand = new RelayCommand(param =>
                    {
                        this.IsOk = false;

                        this.RequestClose?.Invoke();
                    });
                }

                return _cancelCommand;
            }
        }


    }
}
