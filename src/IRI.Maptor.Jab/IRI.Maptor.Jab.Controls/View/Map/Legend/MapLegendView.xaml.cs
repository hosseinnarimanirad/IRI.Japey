using System;
using System.Windows.Data;

using IRI.Maptor.Jab.Common;
using IRI.Maptor.Jab.Common.Models;
using IRI.Maptor.Jab.Common.Localization;

namespace IRI.Maptor.Jab.Controls.View;

/// <summary>
/// Interaction logic for UserControl1.xaml
/// </summary>
public partial class MapLegendView : NotifiableUserControl, IDisposable
{
    public MapLegendView()
    {
        InitializeComponent();

        LocalizationManager.Instance.LanguageChanged += Instance_LanguageChanged;
    }
    private void Instance_LanguageChanged()
    {
        
    }

    private void CollectionViewSource_Filter(object sender, FilterEventArgs e)
    {
        var item = e.Item as ILayer;

        e.Accepted =
            item.ShowInToc && (
            item.Type.HasFlag(LayerType.VectorLayer) ||
            item.Type.HasFlag(LayerType.Raster) ||
            item.Type.HasFlag(LayerType.ImagePyramid));
    }

    #region IDispose

    private bool _disposed = false;

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                // Dispose managed resources
                LocalizationManager.Instance.LanguageChanged -= Instance_LanguageChanged;
            }

            // Dispose unmanaged resources here if any
            _disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    #endregion
}
