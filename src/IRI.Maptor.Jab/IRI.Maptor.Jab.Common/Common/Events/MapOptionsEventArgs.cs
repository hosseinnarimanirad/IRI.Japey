using System; 
using System.Windows;

using IRI.Maptor.Sta.Common.Abstrations;

namespace IRI.Maptor.Jab.Common;

public class MapOptionsEventArgs<T> : EventArgs where T : FrameworkElement, new()
{
    public T View { get; set; }

    public ILocateable DataContext { get; set; }

    public MapOptionsEventArgs(T view, ILocateable dataContext)
    {
        this.View = view;

        this.DataContext = dataContext;
    }
}
