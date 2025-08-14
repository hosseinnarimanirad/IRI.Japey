using IRI.Maptor.Jab.Common.Localization;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace IRI.Maptor.Jab.Controls.View.Symbology
{
    /// <summary>
    /// Interaction logic for SymbologyView.xaml
    /// </summary>
    public partial class SymbologyView : MetroWindow, IDisposable, INotifyPropertyChanged
    {
        private bool _disposed = false;

        public SymbologyView()
        {
            InitializeComponent();
            LocalizationManager.Instance.LanguageChanged += Instance_LanguageChanged;
        }

        public FlowDirection CurrentFlowDirection => LocalizationManager.Instance.CurrentFlowDirection;

        public string FillLabel => LocalizationManager.Instance[LocalizationResourceKeys.symbology_fillLabel.ToString()];

        public string StrokeLabel => LocalizationManager.Instance[LocalizationResourceKeys.symbology_strokeLabel.ToString()];

        public string StrokeWidthLabel => LocalizationManager.Instance[LocalizationResourceKeys.symbology_strokeWidthLabel.ToString()];

        public string WindowTitle => LocalizationManager.Instance[LocalizationResourceKeys.symbology_title.ToString()];

        private void Instance_LanguageChanged()
        {
            RaisePropertyChanged(nameof(FillLabel));
            RaisePropertyChanged(nameof(StrokeLabel));
            RaisePropertyChanged(nameof(StrokeWidthLabel));
            RaisePropertyChanged(nameof(WindowTitle));
            RaisePropertyChanged(nameof(CurrentFlowDirection));
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion


        #region IDispose

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                // Dispose managed resources
                LocalizationManager.Instance.LanguageChanged -= Instance_LanguageChanged;
            }

            // Dispose unmanaged resources here if any
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }



}
