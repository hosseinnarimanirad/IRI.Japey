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
    /// Interaction logic for DegreeMinuteSecondView.xaml
    /// </summary>
    public partial class DegreeMinuteSecondView : UserControl, INotifyPropertyChanged
    {
        const int _defaultMinInputWidth = 90;

        public DegreeMinuteSecondView()
        {
            InitializeComponent();
        }



        public bool HideUpDownButtons
        {
            get { return (bool)GetValue(HideUpDownButtonsProperty); }
            set { SetValue(HideUpDownButtonsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HideUpDownButtons.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HideUpDownButtonsProperty =
            DependencyProperty.Register(nameof(HideUpDownButtons), typeof(bool), typeof(DegreeMinuteSecondView), new PropertyMetadata(false, new PropertyChangedCallback((dpo, dpca) =>
            {
                if ((bool)dpca.NewValue)
                {
                    (dpo as DegreeMinuteSecondView).MinInputWidth = 44;
                }
                else
                {
                    (dpo as DegreeMinuteSecondView).MinInputWidth = _defaultMinInputWidth;
                }
            })));



        public int MaxDegreeValue
        {
            get { return (int)GetValue(MaxDegreeValueProperty); }
            set { SetValue(MaxDegreeValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MaxDegreeValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaxDegreeValueProperty =
            DependencyProperty.Register(nameof(MaxDegreeValue), typeof(int), typeof(DegreeMinuteSecondView), new PropertyMetadata(180));



        public int MinDegreeValue
        {
            get { return (int)GetValue(MinDegreeValueProperty); }
            set { SetValue(MinDegreeValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MinDegreeValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MinDegreeValueProperty =
            DependencyProperty.Register(nameof(MinDegreeValue), typeof(int), typeof(DegreeMinuteSecondView), new PropertyMetadata(-180));


        private int _minInputWidth = _defaultMinInputWidth;

        public int MinInputWidth
        {
            get { return _minInputWidth; }
            set
            {
                _minInputWidth = value;
                RaisePropertyChanged();
            }
        }



        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        //private void NumericUpDown_MouseDown(object sender, MouseButtonEventArgs e)
        //{
        //    var numericUpDown = sender as MahApps.Metro.Controls.NumericUpDown;

        //    numericUpDown.SelectAll();
        //}
    }
}
