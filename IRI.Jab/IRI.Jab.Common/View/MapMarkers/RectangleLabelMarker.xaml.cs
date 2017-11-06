﻿using System;
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

namespace IRI.Jab.Common.View.MapMarkers
{
    /// <summary>
    /// Interaction logic for ShapeWithLabelMarker.xaml
    /// </summary>
    public partial class RectangleLabelMarker : UserControl
    {
         
        public string LabelValue
        {
            get { return (string)GetValue(LabelValueProperty); }
            set { SetValue(LabelValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LabelValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LabelValueProperty =
            DependencyProperty.Register(nameof(LabelValue), typeof(string), typeof(RectangleLabelMarker), new PropertyMetadata(string.Empty));



        public RectangleLabelMarker(string count, bool isExpandBringToFrontEnabled = false)
        {
            InitializeComponent();

            LabelValue = count;

            if (isExpandBringToFrontEnabled)
            {
                this.Style = (Style)this.FindResource("expandableStyle");
            }
        }
    }
}