using IRI.Jab.Common.Presenter.Map;
using IRI.Jab.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Jab.Common.Model.Map
{
    public class MapSettingsModel : Notifier
    {
        public MapSettingsModel()
        {
        }
 


        private string _baseMapCacheDirectory = null;

        public string BaseMapCacheDirectory
        {
            get { return _baseMapCacheDirectory; }
            set
            {
                _baseMapCacheDirectory = value;
                RaisePropertyChanged();

                FireBaseMapCacheDirectoryChanged?.Invoke(value);

            }
        }

        public Action<string> FireBaseMapCacheDirectoryChanged;



        private bool _isBaseMapCacheEnabled;

        public bool IsBaseMapCacheEnabled
        {
            get { return _isBaseMapCacheEnabled; }
            set
            {
                _isBaseMapCacheEnabled = value;
                RaisePropertyChanged();

                FireIsBaseMapCacheEnabledChanged?.Invoke(value);
            }
        }

        public Action<bool> FireIsBaseMapCacheEnabledChanged;



        private bool _isMouseWheelZoomEnabled;

        public bool IsMouseWheelZoomEnabled
        {
            get { return _isMouseWheelZoomEnabled; }
            set
            {
                _isMouseWheelZoomEnabled = value;
                RaisePropertyChanged();

                this.FireIsMouseWheelZoomEnabledChanged?.Invoke(value);
            }
        }

        public Action<bool> FireIsMouseWheelZoomEnabledChanged;



        private bool _isOnDoubleClickZoomEnabled;

        public bool IsDoubleClickZoomEnabled
        {
            get { return _isOnDoubleClickZoomEnabled; }
            set
            {
                _isOnDoubleClickZoomEnabled = value;
                RaisePropertyChanged();

                this.FireIsDoubleClickZoomEnabledChanged?.Invoke(value);
            }
        }

        public Action<bool> FireIsDoubleClickZoomEnabledChanged;



        private bool _isGoogleZoomLevelsEnabled;

        public bool IsGoogleZoomLevelsEnabled
        {
            get { return _isGoogleZoomLevelsEnabled; }
            set
            {
                _isGoogleZoomLevelsEnabled = value;
                RaisePropertyChanged();

                this.FireIsGoogleZoomLevelsEnabledChanged?.Invoke(value);
            }
        }

        public Action<bool> FireIsGoogleZoomLevelsEnabledChanged;


        private int _minGoogleZoomLevel = 1;

        public int MinGoogleZoomLevel
        {
            get { return _minGoogleZoomLevel; }
            set
            {
                if (value > MaxGoogleZoomLevel)
                    return;

                _minGoogleZoomLevel = value;
                RaisePropertyChanged();

                this.FireMinGoogleZoomLevelChanged?.Invoke(value);
            }
        }

        public Action<int> FireMinGoogleZoomLevelChanged;


        private int _maxGoogleZoomLevel = 22;

        public int MaxGoogleZoomLevel
        {
            get { return _maxGoogleZoomLevel; }
            set
            {
                if (value < MinGoogleZoomLevel)
                    return;

                _maxGoogleZoomLevel = value;
                RaisePropertyChanged();

                this.FireMaxGoogleZoomLevelChanged?.Invoke(value);
            }
        }

        public Action<int> FireMaxGoogleZoomLevelChanged;


        public void Initialize()
        {
            this.IsBaseMapCacheEnabled = true;

            this.IsMouseWheelZoomEnabled = true;

            this.IsDoubleClickZoomEnabled = true;

            this.IsGoogleZoomLevelsEnabled = true;

        }
    }
}
