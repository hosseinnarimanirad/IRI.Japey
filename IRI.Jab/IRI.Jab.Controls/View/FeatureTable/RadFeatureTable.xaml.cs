﻿using IRI.Jab.Common.Model.Map;
using IRI.Ket.SqlServerSpatialExtension.Model;
using System;
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

namespace IRI.Jab.Controls.View
{
    /// <summary>
    /// Interaction logic for RadFeatureTable.xaml
    /// </summary>
    public partial class RadFeatureTable : UserControl
    {


        public bool CanUserEditGeometry
        {
            get { return (bool)GetValue(CanUserEditGeometryProperty); }
            set { SetValue(CanUserEditGeometryProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CanUserEditGeometry.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CanUserEditGeometryProperty =
            DependencyProperty.Register(nameof(CanUserEditGeometry), typeof(bool), typeof(RadFeatureTable), new PropertyMetadata(false));



        public ISelectedLayer Presenter { get { return this.DataContext as ISelectedLayer; } }

        public RadFeatureTable()
        {
            InitializeComponent();
        }

        private void RadGridView_AutoGeneratingColumn(object sender, Telerik.Windows.Controls.GridViewAutoGeneratingColumnEventArgs e)
        {
            if (e.Column.Header.ToString().ToLower() == nameof(ISqlGeometryAware.TheSqlGeometry))
            {
                e.Cancel = true;
            }
        }

        private void grid_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            this.Presenter.UpdateHighlightedFeatures(grid.SelectedItems.Cast<ISqlGeometryAware>());
        }
    }
}
