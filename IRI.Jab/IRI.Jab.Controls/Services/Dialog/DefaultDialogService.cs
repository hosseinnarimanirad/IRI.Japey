using IRI.Jab.Common.Model.Security;
using IRI.Jab.Common.Service.Dialog;
using IRI.Jab.Common.ViewModel.Dialogs;
using IRI.Jab.Controls.ViewModel.Dialogs;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Effects;

namespace IRI.Jab.Controls.Services.Dialog
{
    public class DefaultDialogService : IDialogService
    {

        public int BlurRadius { get; set; } = 5;


        #region Open File Dialog

        public string ShowOpenFileDialog(string filter)
        {
            return ShowOpenFileDialog(filter, null);
        }

        public string ShowOpenFileDialog<T>(string filter)
        {
            var owner = Application.Current.Windows.OfType<T>().FirstOrDefault() as Window;

            return ShowOpenFileDialog(filter, owner);
        }

        public string ShowOpenFileDialog(string filter, Window owner)
        {
            OpenFileDialog dialog = new OpenFileDialog() { Filter = filter, Multiselect = false };

            string result = null;

            Effect defaultEffect = null;

            if (owner != null)
            {
                defaultEffect = owner.Effect;

                owner.Effect = new BlurEffect() { Radius = BlurRadius };
            }

            if (dialog.ShowDialog(owner) == true)
                result = dialog.FileName;

            if (owner != null)
            {
                owner.Effect = defaultEffect;
            }

            return result;
        }



        public string[] ShowOpenFilesDialog(string filter)
        {
            return ShowOpenFilesDialog(filter, null);
        }

        public string[] ShowOpenFilesDialog<T>(string filter)
        {
            var owner = Application.Current.Windows.OfType<T>().FirstOrDefault() as Window;

            return ShowOpenFilesDialog(filter, owner);
        }

        public string[] ShowOpenFilesDialog(string filter, Window owner)
        {
            OpenFileDialog dialog = new OpenFileDialog() { Filter = filter, Multiselect = true };

            string[] result = null;

            Effect defaultEffect = null;

            if (owner != null)
            {
                defaultEffect = owner.Effect;

                owner.Effect = new BlurEffect() { Radius = BlurRadius };
            }

            if (dialog.ShowDialog(owner) == true)
                result = dialog.FileNames;

            if (owner != null)
            {
                owner.Effect = defaultEffect;
            }

            return result;
        }

        #endregion


        #region Save File Dialog

        public string ShowSaveFileDialog(string filter)
        {
            return ShowSaveFileDialog(filter, null);
        }

        public string ShowSaveFileDialog<T>(string filter)
        {
            var owner = Application.Current.Windows.OfType<T>().FirstOrDefault() as Window;

            return ShowSaveFileDialog(filter, owner);
        }

        public string ShowSaveFileDialog(string filter, Window owner)
        {
            SaveFileDialog dialog = new SaveFileDialog() { Filter = filter };

            string result = null;

            Effect defaultEffect = null;

            if (owner != null)
            {
                defaultEffect = owner.Effect;

                owner.Effect = new BlurEffect() { Radius = BlurRadius };
            }

            if (dialog.ShowDialog(owner) == true)
                result = dialog.FileName;

            if (owner != null)
            {
                owner.Effect = defaultEffect;
            }

            return result;
        }

        #endregion

        public Task<bool?> ShowYesNoDialog<T>(string message, string title)
        {
            var owner = Application.Current.Windows.OfType<T>().FirstOrDefault() as Window;

            return ShowYesNoDialog(owner, message, title);
        }

        public Task<bool?> ShowYesNoDialog(object ownerWindow, string message, string title)
        {
            TaskCompletionSource<bool?> tcs = new TaskCompletionSource<bool?>();

            IRI.Jab.Controls.ViewModel.Dialogs.DialogViewModel viewModel = new Jab.Controls.ViewModel.Dialogs.DialogViewModel(true)
            {
                Message = message,
                Title = title,
                IsTwoOptionsMode = true
            };

            View.Dialogs.DialogView dialog = new View.Dialogs.DialogView();

            var owner = ownerWindow as Window;

            dialog.Owner = owner;
            //dialog.Owner = Application.Current.MainWindow;

            var ownerDefaultEffect = owner?.Effect;

            if (owner != null)
            {
                owner.Effect = new BlurEffect() { Radius = BlurRadius };
            }

            dialog.Closed += (sender, e) => { tcs.SetResult(viewModel.DialogResult); };

            viewModel.RequestClose = () =>
            {
                dialog.Close();

                if (owner != null)
                {
                    owner.Effect = ownerDefaultEffect;
                }

            };

            dialog.DataContext = viewModel;

            dialog.ShowDialog();

            return tcs.Task;
        }

        public Task ShowMessage<T>(string message, string pathMarkup, string title = null)
        {
            var owner = Application.Current.Windows.OfType<T>().FirstOrDefault() as Window;

            return ShowMessage(owner, pathMarkup, message, title);
        }

        public Task ShowMessage(object ownerWindow, string pathMarkup, string message, string title)
        {
            TaskCompletionSource<bool?> tcs = new TaskCompletionSource<bool?>();

            IRI.Jab.Controls.ViewModel.Dialogs.DialogViewModel viewModel = new Jab.Controls.ViewModel.Dialogs.DialogViewModel(true)
            {
                Message = message,
                Title = title,
                IsTwoOptionsMode = false,
                IconPathMarkup = pathMarkup
            };

            View.Dialogs.MessageBoxView dialog = new View.Dialogs.MessageBoxView();

            var owner = ownerWindow as Window;

            if (owner != null)
            {
                dialog.Owner = owner;
            }
            //dialog.Owner = Application.Current.MainWindow;

            var ownerDefaultEffect = owner?.Effect;

            if (owner != null)
            {
                owner.Effect = new BlurEffect() { Radius = BlurRadius };
            }

            dialog.Closed += (sender, e) => { tcs.SetResult(viewModel.DialogResult); };

            viewModel.RequestClose = () =>
            {
                dialog.Close();

                if (owner != null)
                {
                    owner.Effect = ownerDefaultEffect;
                }

            };

            dialog.DataContext = viewModel;

            dialog.ShowDialog();

            return tcs.Task;
        }


        public Task<SignUpDialogViewModel> ShowUserNameSignUpDialog<T>()
        {
            var owner = Application.Current.Windows.OfType<T>().FirstOrDefault() as Window;

            return ShowUserNameSignUpDialog(owner);
        }

        public Task<SignUpDialogViewModel> ShowUserNameSignUpDialog(object ownerWindow)
        {
            TaskCompletionSource<SignUpDialogViewModel> tcs = new TaskCompletionSource<SignUpDialogViewModel>();

            View.Dialogs.UserNameSignUpDialogView dialog = new View.Dialogs.UserNameSignUpDialogView();

            var owner = ownerWindow as Window;

            if (owner != null)
            {
                dialog.Owner = owner;
            }

            var ownerDefaultEffect = owner?.Effect;

            if (owner != null)
            {
                owner.Effect = new BlurEffect() { Radius = BlurRadius };
            }

            Action requestClose = () =>
            {
                dialog.Close();

                if (owner != null)
                {
                    owner.Effect = ownerDefaultEffect;
                }
            };

            IRI.Jab.Common.ViewModel.Dialogs.SignUpDialogViewModel viewModel = new Jab.Common.ViewModel.Dialogs.SignUpDialogViewModel(requestClose);
            //{
            //    Message = message,
            //    Title = title,
            //    IsTwoOptionsMode = false,
            //    IconPathMarkup = pathMarkup
            //};

            dialog.Closed += (sender, e) =>
            {
                if (viewModel.IsOk)
                {
                    tcs.SetResult(viewModel);
                }
                else
                {
                    tcs.SetResult(null);
                }

            };

            dialog.DataContext = viewModel;

            dialog.ShowDialog();

            return tcs.Task;
        }


        #region Change Password Dialog

        
        public Task<ChangePasswordDialogViewModel> ShowChangePasswordDialog<T>(Func<IHavePassword, Task<bool>> requestAuthenticateAsync)
        {
            var owner = Application.Current.Windows.OfType<T>().FirstOrDefault() as Window;

            return ShowChangePasswordDialog(owner, requestAuthenticateAsync);
        }

        public Task<ChangePasswordDialogViewModel> ShowChangePasswordDialog(object ownerWindow, Func<IHavePassword, Task<bool>> requestAuthenticateAsync)
        {
            //requestClose parameter for viewModel is set in the next function
            ChangePasswordDialogViewModel viewModel = ChangePasswordDialogViewModel.Create(null, requestAuthenticateAsync);

            return ShowChangePasswordDialog(ownerWindow, viewModel);
        }

        public Task<ChangePasswordDialogViewModel> ShowChangePasswordDialog<T>(Func<IHavePassword, bool> requestAuthenticate)
        {
            var owner = Application.Current.Windows.OfType<T>().FirstOrDefault() as Window;
             
            return ShowChangePasswordDialog(owner, requestAuthenticate);
        }
         
        public Task<ChangePasswordDialogViewModel> ShowChangePasswordDialog(object ownerWindow, Func<IHavePassword, bool> requestAuthenticate)
        {
            //requestClose parameter for viewModel is set in the next function
            ChangePasswordDialogViewModel viewModel = ChangePasswordDialogViewModel.Create(null, requestAuthenticate);

            return ShowChangePasswordDialog(ownerWindow, viewModel);
        }

        //public Task<ChangePasswordDialogViewModel> ShowChangePasswordDialog(object ownerWindow, Func<IHavePassword, bool> requestAuthenticate)
        //{
        //    TaskCompletionSource<ChangePasswordDialogViewModel> tcs = new TaskCompletionSource<ChangePasswordDialogViewModel>();

        //    View.Dialogs.ChangePasswordDialogView dialog = new View.Dialogs.ChangePasswordDialogView();

        //    var owner = ownerWindow as Window;

        //    if (owner != null)
        //    {
        //        dialog.Owner = owner;
        //    }

        //    var ownerDefaultEffect = owner?.Effect;

        //    if (owner != null)
        //    {
        //        owner.Effect = new BlurEffect() { Radius = BlurRadius };
        //    }

        //    Action requestClose = () =>
        //    {
        //        dialog.Close();

        //        if (owner != null)
        //        {
        //            owner.Effect = ownerDefaultEffect;
        //        }
        //    };

        //    ChangePasswordDialogViewModel viewModel = ChangePasswordDialogViewModel.Create(requestClose, requestAuthenticate);
        //    //{
        //    //    Message = message,
        //    //    Title = title,
        //    //    IsTwoOptionsMode = false,
        //    //    IconPathMarkup = pathMarkup
        //    //};

        //    dialog.Closed += (sender, e) =>
        //    {
        //        if (viewModel.IsOk)
        //        {
        //            tcs.SetResult(viewModel);
        //        }
        //        else
        //        {
        //            tcs.SetResult(null);
        //        }

        //    };

        //    dialog.DataContext = viewModel;

        //    dialog.ShowDialog();

        //    return tcs.Task;
        //}

        public Task<ChangePasswordDialogViewModel> ShowChangePasswordDialog(object ownerWindow, ChangePasswordDialogViewModel viewModel)
        {
            TaskCompletionSource<ChangePasswordDialogViewModel> tcs = new TaskCompletionSource<ChangePasswordDialogViewModel>();

            View.Dialogs.ChangePasswordDialogView dialog = new View.Dialogs.ChangePasswordDialogView();

            var owner = ownerWindow as Window;

            if (owner != null)
            {
                dialog.Owner = owner;
            }

            var ownerDefaultEffect = owner?.Effect;

            if (owner != null)
            {
                owner.Effect = new BlurEffect() { Radius = BlurRadius };
            }

            Action requestClose = () =>
            {
                dialog.Close();

                if (owner != null)
                {
                    owner.Effect = ownerDefaultEffect;
                }
            };

            viewModel.RequestClose = requestClose;

            dialog.Closed += (sender, e) =>
            {
                if (viewModel.IsOk)
                {
                    tcs.SetResult(viewModel);
                }
                else
                {
                    tcs.SetResult(null);
                }
            };

            dialog.DataContext = viewModel;

            dialog.ShowDialog();

            return tcs.Task;
        }

        #endregion

        public Task<bool?> ShowDialg<TParent>(Window view, DialogViewModelBase viewModel)
        {
            var owner = Application.Current.Windows.OfType<TParent>().FirstOrDefault() as Window;

            return ShowDialog<TParent>(owner, view, viewModel);
        }

        public Task<bool?> ShowDialog<TParent>(object ownerWindow, Window view, DialogViewModelBase viewModel)
        {
            TaskCompletionSource<bool?> tcs = new TaskCompletionSource<bool?>();

            var owner = ownerWindow as Window;

            if (owner != null)
            {
                view.Owner = owner;
            }

            var ownerDefaultEffect = owner?.Effect;

            if (owner != null)
            {
                owner.Effect = new BlurEffect() { Radius = BlurRadius };
            }

            view.Closed += (sender, e) =>
            {
                if (owner != null)
                {
                    owner.Effect = ownerDefaultEffect;
                }

                tcs.SetResult(viewModel.DialogResult);
            };

            viewModel.OnSetResult += (sender, e) =>
            {
                view.Close();
                //if (viewModel.DialogResult == true)
                //{
                //    tcs.SetResult(viewModel.DialogResult);

                //    view.Close();
                //}
                //else
                //{
                //    tcs.SetResult(null);

                //    view.Close();
                //}

            };

            view.DataContext = viewModel;

            view.ShowDialog();

            return tcs.Task;
        }

    }
}
