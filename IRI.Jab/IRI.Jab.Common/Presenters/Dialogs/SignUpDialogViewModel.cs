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
    public class SignUpDialogViewModel : Notifier
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

        private RelayCommand _createUserCommand;

        public RelayCommand CreateUserCommand
        {
            get
            {
                if (_createUserCommand == null)
                {
                    _createUserCommand = new RelayCommand(param =>
                    {
                        var model = param as INewSimpleUserPass;

                        if (model == null)
                        {
                            return;
                        }

                        this.Model.UserName = model.UserName;

                        this.Model.Password = SecureStringHelper.GetString(model.NewPassword);

                        this.IsOk = true;

                        this.RequestClose?.Invoke();
                    });
                }

                return _createUserCommand;
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
