﻿using System;
using System.Collections.Generic;
using System.Linq;
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
using IRI.Jab.Common.Model.MapMarkers;

namespace IRI.Jab.Common.View.MapMarkers
{
    /// <summary>
    /// Interaction logic for ImageSymbolWithCountMarker.xaml
    /// </summary>
    public partial class CountableImageMarker : UserControl, IMapMarker
    {
        public CountableImageMarker(ImageSource imageSource, string count)
        {
            InitializeComponent();

            this.image.Source = imageSource;

            this.labelBox.Text = count;
        }
        private bool _isSelected;

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
            }
        }
    }
}
