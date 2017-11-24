using IRI.Jab.Common.Model;
using IRI.Jab.Common.Model.Globalization;
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
using System.Windows.Shapes;

namespace IRI.Jab.MapViewer
{
    /// <summary>
    /// Interaction logic for MapInfoView.xaml
    /// </summary>
    public partial class MapInfoView : UserControl, INotifyPropertyChanged
    {
        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion


        public MapInfoView()
        {
            InitializeComponent();
        }

        public LanguageMode UILanguage
        {
            get { return (LanguageMode)GetValue(UILanguageProperty); }
            set
            {
                SetValue(UILanguageProperty, value);

            }
        }

        // Using a DependencyProperty as the backing store for UILanguage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UILanguageProperty =
            DependencyProperty.Register(
                nameof(UILanguage),
                typeof(LanguageMode),
                typeof(MapInfoView),
                new PropertyMetadata(LanguageMode.Persian, (d, dp) =>
                {
                    try
                    {
                        ((MapInfoView)d).SetLanguage((LanguageMode)dp.NewValue);
                    }
                    catch (Exception ex)
                    {
                        return;
                    }
                }));


        private PersianEnglishItem _newDrawingText = new PersianEnglishItem("برای اتمام ترسیم روی نقطهٔ آخر مجدد کلیک کنید.", "Click on the last point to finish drawing.", LanguageMode.Persian);

        public PersianEnglishItem NewDrawingText
        {
            get { return _newDrawingText; }
            set
            {
                _newDrawingText = value;
                RaisePropertyChanged();
            }
        }

        private void SetLanguage(LanguageMode mode)
        {
            NewDrawingText.UILanguage = mode;
        }

    }
}
