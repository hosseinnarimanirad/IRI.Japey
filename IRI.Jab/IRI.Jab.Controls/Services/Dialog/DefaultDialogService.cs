using IRI.Jab.Common.Service.Dialog;
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
            OpenFileDialog dialog = new OpenFileDialog() { Filter = filter };

            string result = null;

            Effect defaultEffect = null;

            if (owner != null)
            {
                defaultEffect = owner.Effect;

                owner.Effect = new BlurEffect() { Radius = 5 };
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

                owner.Effect = new BlurEffect() { Radius = 5 };
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

        public Task<bool> ShowYesNoDialog<T>(string message, string title)
        {
            var owner = Application.Current.Windows.OfType<T>().FirstOrDefault() as Window;

            return ShowYesNoDialog(owner, message, title);
        }

        public Task<bool> ShowYesNoDialog(object ownerWindow, string message, string title)
        {
            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();

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
                owner.Effect = new BlurEffect() { Radius = 10 };
            }

            dialog.Closed += (sender, e) => { tcs.SetResult(viewModel.IsOk); };

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
            throw new NotImplementedException();
        }
    }
}
