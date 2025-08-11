using IRI.Maptor.Jab.Common.Model.Legend;
using IRI.Maptor.Jab.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Maptor.Jab.Common.Presenter;

public class LegendPresenter: Notifier
{
    public LegendPresenter(IEnumerable<LegendItem> layers)
    {
        if (layers == null)
        {
            this.Layers = new ObservableCollection<LegendItem>();
        }
        else
        {
            this.Layers = new ObservableCollection<LegendItem>(layers);
        }

    }

    private ObservableCollection<LegendItem> _layers;

    public ObservableCollection<LegendItem> Layers
    {
        get { return _layers; }
        set
        {
            _layers = value;
            RaisePropertyChanged();

            PrepareEvents();
        }
    }

    private void PrepareEvents()
    {
        this.Layers.CollectionChanged -= Layers_CollectionChanged;
        this.Layers.CollectionChanged += Layers_CollectionChanged;

        foreach (var item in Layers)
        {
            SetEvents(item);
        }
    }

    private void Layers_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        foreach (var item in Layers)
        {
            SetEvents(item);
        }
    }

    private void SetEvents(LegendItem item)
    {
        item.OnRequestForSelectByDrawing -= RequestForSelectByDrawing;
        item.OnRequestForSelectByDrawing += RequestForSelectByDrawing; ;

        item.OnRequestShowAll -= Item_OnRequestShowAll;
        item.OnRequestShowAll += Item_OnRequestShowAll;

        if (item.SubLayers == null)
            return;

        foreach (var subLayer in item.SubLayers)
        {
            SetEvents(subLayer);
        }
    }

    private void Item_OnRequestShowAll(object sender, LegendItemEventArgs e)
    {
        this.OnRequestShowAll.SafeInvoke(this, e);
    }

    private void RequestForSelectByDrawing(object sender, LegendItemEventArgs e)
    {
        this.OnRequestSelectByDrawing.SafeInvoke(this, e);
    }


    #region Events

    public event EventHandler<LegendItemEventArgs> OnRequestSelectByDrawing;

    public event EventHandler<LegendItemEventArgs> OnRequestShowAll;

    #endregion
}
