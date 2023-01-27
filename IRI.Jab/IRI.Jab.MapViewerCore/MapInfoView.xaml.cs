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


        private PersianEnglishItem _utmZone = new PersianEnglishItem("ناحیهٔ UTM", "Zone", LanguageMode.Persian);

        public PersianEnglishItem UtmZone
        {
            get { return _utmZone; }
            set
            {
                _utmZone = value;
                RaisePropertyChanged();
            }
        }
 
        private PersianEnglishItem _utmText = new PersianEnglishItem("سیستم تصویر UTM", "UTM", LanguageMode.Persian);

        public PersianEnglishItem UTMText
        {
            get { return _utmText; }
            set
            {
                _utmText = value;
                RaisePropertyChanged();
            }
        }

        private PersianEnglishItem _geodeticWgs84Text= new PersianEnglishItem("سیستم مختصات ژئودتیک - WGS84", "Geodetic (WGS84)", LanguageMode.Persian);

        public PersianEnglishItem GeodeticWgs84Text
        {
            get { return _geodeticWgs84Text; }
            set
            {
                _geodeticWgs84Text = value;
                RaisePropertyChanged();
            }
        }

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

        private PersianEnglishItem _cancelDrawingText = new PersianEnglishItem("لغو ترسیم", "Cancel Drawing", LanguageMode.Persian);

        public PersianEnglishItem CancelDrawingText
        {
            get { return _cancelDrawingText; }
            set
            {
                _cancelDrawingText = value;
                RaisePropertyChanged();
            }
        }

        private PersianEnglishItem _finishDrawingText = new PersianEnglishItem("پایان ترسیم", "Finish Drawing", LanguageMode.Persian);

        public PersianEnglishItem FinishDrawingText
        {
            get { return _finishDrawingText; }
            set
            {
                _finishDrawingText = value;
                RaisePropertyChanged();
            }
        }

        private PersianEnglishItem _finishDrawingPartText = new PersianEnglishItem("تکمیل بخش", "Finish Drawing Part", LanguageMode.Persian);

        public PersianEnglishItem FinishDrawingPartText
        {
            get { return _finishDrawingPartText; }
            set
            {
                _finishDrawingPartText = value;
                RaisePropertyChanged();
            }
        }

        private PersianEnglishItem _addPointText = new PersianEnglishItem("افزودن نقطه", "Add Point", LanguageMode.Persian);

        public PersianEnglishItem AddPointText
        {
            get { return _addPointText; }
            set
            {
                _addPointText = value;
                RaisePropertyChanged();
            }
        }


        private void SetLanguage(LanguageMode mode)
        {
            //NewDrawingText.UILanguage = mode;
            var properties = (typeof(MapInfoView)).GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public)
                .Where(p => p.DeclaringType == typeof(PersianEnglishItem));

            foreach (var property in properties)
            {
                property.SetValue(this, mode);
            }

        }

    }
}
