using System; 
using System.Windows;

using IRI.Sta.Common.Abstrations;

namespace IRI.Jab.Common;

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
