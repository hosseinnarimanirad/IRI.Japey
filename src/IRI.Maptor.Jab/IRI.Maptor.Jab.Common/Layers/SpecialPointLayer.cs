using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using IRI.Maptor.Jab.Common.Enums;
using IRI.Maptor.Jab.Common.Model;
using IRI.Maptor.Sta.Common.Abstrations;
using IRI.Maptor.Sta.Common.Primitives;

namespace IRI.Maptor.Jab.Common;

public class SpecialPointLayer : BaseLayer
{
    private ObservableCollection<Locateable> _items;

    public ObservableCollection<Locateable> Items
    {
        get { return _items; }
        set
        {
            _items = value;
            RaisePropertyChanged();
        }
    }

    public override BoundingBox Extent
    {
        get
        {
            if (this.Items.Count < 1)
            {
                return new BoundingBox(double.NaN, double.NaN, double.NaN, double.NaN);
            }

            return BoundingBox.CalculateBoundingBox(this.Items.Select(i => new Point(i.X, i.Y)));
        }
        protected set
        {
            throw new NotImplementedException();
            //_extent = value;
            //OnPropertyChanged("Extent");
        }
    }

    public override RenderingApproach Rendering
    {
        get { return RenderingApproach.Default; }
        protected set { }
    }

    private bool _insertTop;

    public bool AlwaysTop
    {
        get { return _insertTop; }
        set
        {
            _insertTop = value;
            RaisePropertyChanged();
        }
    }


    //public RasterizationApproach ToRasterTechnique { get { return RasterizationApproach.None; } }

    //public bool IsValid { get; set; }

    //public void Invalidate() => IsValid = false;

    private LayerType _type;

    public override LayerType Type
    {
        get { return _type; }

        protected set
        {
            if (value.HasFlag(LayerType.Complex) ||
                value.HasFlag(LayerType.RightClickOption) ||
                value.HasFlag(LayerType.GridAndGraticule) ||
                value.HasFlag(LayerType.MoveableItem) ||
                value.HasFlag(LayerType.EditableItem))
            {
                this._type = value;
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }

    //public int ZIndex { get; set; }

    //private FrameworkElement visualElement;

    //public FrameworkElement Element
    //{
    //    get { return this.visualElement; }

    //    set
    //    {
    //        this.visualElement = value;

    //        BindWithFrameworkElement(value);

    //        OnPropertyChanged("Element");
    //    }
    //}

    //public bool IsLabeled(double mapScale)
    //{
    //    return this.Labels != null && this.Labels.IsLabeled(1.0 / mapScale);
    //}

    //private LabelParameters _labels;

    //public LabelParameters Labels
    //{
    //    get { return _labels; }
    //    set
    //    {
    //        _labels = value;
    //        OnPropertyChanged("Labels");
    //    }
    //}

    //public Func<SqlGeometry, SqlGeometry> PositionFunc { get; set; }

    //private VisualParameters _visualParameters;

    //public VisualParameters VisualParameters
    //{
    //    get { return _visualParameters; }
    //    set
    //    {
    //        _visualParameters = value;
    //        OnPropertyChanged("VisualParameters");
    //    }
    //}

    //private Geometry _pointSymbol;

    //public Geometry PointSymbol
    //{
    //    get { return _pointSymbol; }
    //    set
    //    {
    //        _pointSymbol = value;
    //        OnPropertyChanged("PointSymbol");
    //    }
    //}

    //private ImageSource _imageSymbol;

    //public ImageSource ImageSymbol
    //{
    //    get { return _imageSymbol; }
    //    set
    //    {
    //        _imageSymbol = value;
    //        OnPropertyChanged("Symbol");
    //    }
    //}

    public SpecialPointLayer(string name, Locateable item, double opacity = 1, ScaleInterval visibleRange = null, LayerType type = LayerType.Complex)
        : this(name, new List<Locateable>() { item }, opacity, visibleRange, type)
    {

    }

    public SpecialPointLayer(string name, IEnumerable<Locateable> items, double opacity = 1, ScaleInterval visibleRange = null, LayerType type = LayerType.Complex)
    {
        this.LayerId = Guid.NewGuid();

        this.Type = type;

        this.LayerName = name;

        this.ZIndex = int.MaxValue;

        this.Items = new ObservableCollection<Locateable>();

        this.Items.CollectionChanged -= Items_CollectionChanged;
        this.Items.CollectionChanged += Items_CollectionChanged;

        if (visibleRange == null)
        {
            VisibleRange = ScaleInterval.All;
        }
        else
        {
            VisibleRange = visibleRange;
        }

        this.VisualParameters = new VisualParameters(null, null, 0, opacity, System.Windows.Visibility.Visible);

        if (items == null)
        {
            return;
        }

        foreach (var item in items)
        {
            this.Items.Add(item);
        }

    }

    private void Items_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        if (HandleCollectionChanged == null)
            return;

        HandleCollectionChanged(e);
    }

    public Action<System.Collections.Specialized.NotifyCollectionChangedEventArgs> HandleCollectionChanged;

    public Locateable Get(System.Windows.FrameworkElement element)
    {
        for (int i = 0; i < Items.Count; i++)
        {
            if (Items[i].Element == element)
            {
                return Items[i];
            }
        }

        return null;
    }

    public IEnumerable<Locateable> Get(Guid id)
    {
        return this.Items.Where(i => i.Id == id);
    }

    public void Remove(Guid id)
    {
        if (this.Items.Count(i => i.Id == id) > 1)
        {
            throw new NotImplementedException("more than one locateable found");
        }

        for (int i = this.Items.Count - 1; i >= 0; i--)
        {
            if (this.Items[i].Id == id)
            {
                this.Items.RemoveAt(i);
            }
        }
    }

    public void Remove(IPoint[] points)
    {
        for (int i = Items.Count - 1; i >= 0; i--)
        {
            for (int j = 0; j < points.Length; j++)
            {
                if (Items[i].X == points[j].X && Items[i].Y == points[j].Y)
                {
                    Items.Remove(Items[i]);

                    break;
                }
            }

        }
    }

    public void SelectLocatable(System.Windows.FrameworkElement element)
    {
        for (int i = 0; i < Items.Count; i++)
        {
            Items[i].IsSelected = Items[i].Element == element;
        }

        this.RequestSelectedLocatableChanged?.Invoke(FindSelectedLocatable());
    }

    public void SelectLocatable(int index)
    {
        for (int i = 0; i < Items.Count; i++)
        {
            Items[i].IsSelected = index == i;
        }

        this.RequestSelectedLocatableChanged?.Invoke(FindSelectedLocatable());
    }

    public Locateable FindSelectedLocatable()
    {
        for (int i = 0; i < Items.Count; i++)
        {
            if (Items[i].IsSelected)
                return Items[i];
        }

        return null;
    }

    public int FindSelectedLocatableIndex()
    {
        for (int i = 0; i < Items.Count; i++)
        {
            if (Items[i].IsSelected)
            {
                return i;
            }
        }

        return -1;
    }

    public void SelectNextLocatable()
    {
        if (!(Items?.Count > 0))
            return;

        var current = FindSelectedLocatableIndex();

        if (current < 0)
            SelectLocatable(0);

        var next = current + 1 < Items.Count ? current + 1 : 0;

        SelectLocatable(next);
    }

    public void SelectPreviousLocatable()
    {
        if (!(Items?.Count > 0))
            return;

        var current = FindSelectedLocatableIndex();

        if (current < 0)
            SelectLocatable(0);

        var next = current - 1 >= 0 ? current - 1 : Items.Count - 1;

        SelectLocatable(next);
    } 

    public Action<Locateable> RequestSelectedLocatableChanged;
}
