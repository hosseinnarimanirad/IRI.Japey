using IRI.Jab.Common.Model.MapMarkers;

using System;
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
    public partial class ShapeWithLabelMarker : UserControl, IMapMarker//, INotifyPropertyChanged
    {
        //private bool _isExpanded;

        //public bool IsExpanded
        //{
        //    get { return _isExpanded; }
        //    set
        //    {
        //        this._isExpanded = value;
        //        RaisePropertyChanged();
        //    }
        //}

        public ShapeWithLabelMarker(Geometry shape, string count)
        {
            InitializeComponent();

            this.shape.Data = shape;

            this.labelBox.Text = count;
        }

        //public event PropertyChangedEventHandler PropertyChanged;
        //protected void RaisePropertyChanged([CallerMemberName] string propertyName = "")
        //{
        //    if (PropertyChanged != null)
        //    {
        //        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        //    }
        //}

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //this.IsExpanded = !this.IsExpanded;
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
