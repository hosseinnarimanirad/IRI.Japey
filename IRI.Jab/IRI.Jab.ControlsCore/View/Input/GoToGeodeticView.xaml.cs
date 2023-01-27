﻿using IRI.Jab.Common.Model;
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

namespace IRI.Jab.Controls.View.Input
{
    /// <summary>
    /// Interaction logic for GoToGeodetic.xaml
    /// </summary>
    public partial class GoToGeodeticView : UserControl, INotifyPropertyChanged
    {

        public GoToGeodeticView()
        {
            InitializeComponent();

            this.UILanguage = LanguageMode.Persian;
        }

        private string _xLabel;

        public string XLabel
        {
            get { return _xLabel; }
            set
            {
                _xLabel = value;
                RaisePropertyChanged();
            }
        }

        private string _yLabel;

        public string YLabel
        {
            get { return _yLabel; }
            set
            {
                _yLabel = value;
                RaisePropertyChanged();
            }
        }

        private string _panToLabel;

        public string PanToLabel
        {
            get { return _panToLabel; }
            set
            {
                _panToLabel = value;
                RaisePropertyChanged();
            }
        }

        private string _zoomToLabel;

        public string ZoomToLabel
        {
            get { return _zoomToLabel; }
            set
            {
                _zoomToLabel = value;
                RaisePropertyChanged();
            }
        }


        public string Note { get => string.Empty; }

        private LanguageMode _uiLanguage;

        public LanguageMode UILanguage
        {
            get { return _uiLanguage; }
            set
            {
                _uiLanguage = value;
                RaisePropertyChanged();

                UpdateUI();
            }
        }

        private void UpdateUI()
        {
            bool isPersian = this.UILanguage == LanguageMode.Persian;

            this.FlowDirection = isPersian ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;

            this.XLabel = isPersian ? "طول جغرافیایی" : "Longitude";

            this.YLabel = isPersian ? "عرض جغرافیایی" : "Latitude";

            this.PanToLabel = isPersian ? "حرکت" : "Pan To";

            this.ZoomToLabel = isPersian ? "بزرگ‌نمایی" : "Zoom To";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        //public IRI.Jab.Common.Model.Language Language
        //{
        //    get { return (IRI.Jab.Common.Model.Language)GetValue(LanguageProperty); }
        //    set { SetValue(LanguageProperty, value); }
        //}

        //// Using a DependencyProperty as the backing store for Language.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty LanguageProperty =
        //    DependencyProperty.Register("Language", typeof(IRI.Jab.Common.Model.Language), typeof(GoToGeodeticView), new PropertyMetadata(Common.Model.Language.Persian));


    }
}
