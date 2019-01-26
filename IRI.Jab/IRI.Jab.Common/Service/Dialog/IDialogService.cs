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
        string ShowOpenFileDialog(string filter);

        string ShowOpenFileDialog<T>(string filter);

        string ShowSaveFileDialog(string filter);

        string ShowSaveFileDialog<T>(string filter);

        Task<bool> ShowYesNoDialog<T>(string message, string title = null);

        Task<bool> ShowYesNoDialog(object owner, string message, string title);

        Task ShowMessage<T>(string message, string pathMarkup, string title);

    }
}
