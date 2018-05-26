﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace IRI.Jab.Common
{
    public class MapOptionsEventArgs<T> : EventArgs where T : FrameworkElement, new()
    {
        public T View { get; set; }

        public IRI.Sta.Common.Primitives.ILocateable DataContext { get; set; }

        public MapOptionsEventArgs(T view, IRI.Sta.Common.Primitives.ILocateable dataContext)
        {
            this.View = view;

            this.DataContext = dataContext;
        }
    }
}
