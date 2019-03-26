using IRI.Jab.Common.Model.Security;
using IRI.Jab.Common.ViewModel.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace IRI.Jab.Common.Service.Dialog
{
    public interface IDialogService
    {
        string[] ShowOpenFilesDialog<T>(string filter);

        string[] ShowOpenFilesDialog(string filter, object owner);


        string ShowOpenFileDialog<T>(string filter);

        string ShowOpenFileDialog(string filter, object owner);


        string ShowSaveFileDialog<T>(string filter);

        string ShowSaveFileDialog(string filter, object owner);


        Task<bool?> ShowYesNoDialog<T>(string message, string title = null);

        Task<bool?> ShowYesNoDialog(string message, string title, object owner);


        Task ShowMessage<T>(string message, string pathMarkup, string title);

        Task ShowMessage(string pathMarkup, string message, string title, object ownerWindow);


        Task<SignUpDialogViewModel> ShowUserNameSignUpDialog<T>();

        Task<SignUpDialogViewModel> ShowUserNameSignUpDialog(object ownerWindow);


        #region Change Password Dialog

        Task<ChangePasswordDialogViewModel> ShowChangePasswordDialog<T>(Func<IHavePassword, Task<bool>> requestAuthenticateAsync);

        Task<ChangePasswordDialogViewModel> ShowChangePasswordDialog(object ownerWindow, Func<IHavePassword, Task<bool>> requestAuthenticateAsync);

        Task<ChangePasswordDialogViewModel> ShowChangePasswordDialog<T>(Func<IHavePassword, bool> requestAuthenticate);

        Task<ChangePasswordDialogViewModel> ShowChangePasswordDialog(object ownerWindow, Func<IHavePassword, bool> requestAuthenticate);

        Task<ChangePasswordDialogViewModel> ShowChangePasswordDialog(object ownerWindow, ChangePasswordDialogViewModel viewModel);

        #endregion

        Task<bool?> ShowDialg<TParent>(Window view, DialogViewModelBase viewModel);

        Task<bool?> ShowDialog<TParent>(object ownerWindow, Window view, DialogViewModelBase viewModel);

    }
}
