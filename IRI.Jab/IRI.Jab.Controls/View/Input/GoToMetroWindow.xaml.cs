using System;
using System.Windows;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using MahApps.Metro.Controls;
using IRI.Jab.Common.Localization;

namespace IRI.Jab.Controls.View;

/// <summary>
/// Interaction logic for GoToMetroWindow.xaml
/// </summary>
public partial class GoToMetroWindow : MetroWindow, IDisposable, INotifyPropertyChanged
{
    private bool _disposed = false;

    public GoToMetroWindow()
    {
        InitializeComponent();
        LocalizationManager.Instance.LanguageChanged += Instance_LanguageChanged;
    }

    public GoToMetroWindow(Presenter.GoToPresenter presenter) : this()
    {
        this.DataContext = presenter;
    }

    public FlowDirection CurrentFlowDirection => LocalizationManager.Instance.CurrentFlowDirection;

    private void Instance_LanguageChanged()
    {
        RaisePropertyChanged(nameof(WindowTitle));
        RaisePropertyChanged(nameof(CurrentFlowDirection));
    }

    public string WindowTitle => LocalizationManager.Instance[LocalizationResourceKeys.goto_dialogTitle.ToString()];

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
