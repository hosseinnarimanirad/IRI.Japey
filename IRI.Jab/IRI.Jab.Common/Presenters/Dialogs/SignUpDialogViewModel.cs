using IRI.Jab.Common;
using IRI.Jab.Common.Assets.Commands;
using IRI.Jab.Common.Model.Security;
using IRI.Msh.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Jab.Common.ViewModel.Dialogs
{
    public class SignUpDialogViewModel : DialogViewModelBase
    {
        public Action RequestClose;

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

        private SimpleUserPassModel _model;

        public SimpleUserPassModel Model
        {
            get { return _model; }
            set
            {
                _model = value;
                RaisePropertyChanged();
            }
        }

        public SignUpDialogViewModel(Action requestClose)
        {
            this.Model = new SimpleUserPassModel();

            this.RequestClose = requestClose;
        }

        private RelayCommand _signUpCommand;

        public RelayCommand SignUpCommand
        {
            get
            {
                if (_signUpCommand == null)
                {
                    _signUpCommand = new RelayCommand(param =>
                    {
                        var model = param as INewUserEmailPassword;

                        if (model == null)
                        {
                            return;
                        }

                        if (!model.IsNewPasswordValid())
                        {
                            model.ClearInputValues();

                            return;
                        }

                        this.Model.UserName = model.UserNameOrEmail;

                        this.Model.Password = SecureStringHelper.GetString(model.NewPassword);

                        this.IsOk = true;

                        this.RequestClose?.Invoke();
                    });
                }

                return _signUpCommand;
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
