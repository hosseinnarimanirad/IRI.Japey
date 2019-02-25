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
        string[] ShowOpenFilesDialog(string filter);

        string[] ShowOpenFilesDialog<T>(string filter);

        string ShowOpenFileDialog(string filter);

        string ShowOpenFileDialog<T>(string filter);

        string ShowSaveFileDialog(string filter);

        string ShowSaveFileDialog<T>(string filter);

        Task<bool> ShowYesNoDialog<T>(string message, string title = null);

        Task<bool> ShowYesNoDialog(object owner, string message, string title);

        Task ShowMessage<T>(string message, string pathMarkup, string title);

        Task ShowMessage(object ownerWindow, string pathMarkup, string message, string title);

        Task<SignUpDialogViewModel> ShowUserNameSignUpDialog<T>();
         
        Task<SignUpDialogViewModel> ShowUserNameSignUpDialog(object ownerWindow);


        Task<ChangePasswordDialogViewModel> ShowChangePasswordDialog<T>(Func<string, bool> requestAuthenticate);

        Task<ChangePasswordDialogViewModel> ShowChangePasswordDialog(object ownerWindow, Func<string, bool> requestAuthenticate);
    }
}
