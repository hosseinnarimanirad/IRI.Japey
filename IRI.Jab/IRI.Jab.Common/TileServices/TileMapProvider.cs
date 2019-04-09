using IRI.Jab.Common.Model.Globalization;
using IRI.Msh.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Jab.Common.TileServices
{
    public class TileMapProvider : Notifier
    {
        public bool RequireInternetConnection { get; set; } = true;

        //in the case of google traffic map, caching should be avoided
        public bool AllowCache { get; set; } = true;

        //private string _mapName ;

        //public string MapName
        //{
        //    get { return _mapName; }
        //    set
        //    {
        //        _mapName = value;
        //        RaisePropertyChanged();
        //        RaisePropertyChanged(nameof(FullTitle));
        //    }
        //}

        //private string _providerName;

        //public string ProviderName
        //{
        //    get { return _providerName; }
        //    protected set
        //    {
        //        _providerName = value;
        //        RaisePropertyChanged();
        //        RaisePropertyChanged(nameof(FullTitle));
        //    }
        //}

        private PersianEnglishItem _mapType;

        public PersianEnglishItem MapType
        {
            get { return _mapType; }
            set
            {
                _mapType = value;
                RaisePropertyChanged();
            }
        }

        private PersianEnglishItem _provider;

        public PersianEnglishItem Provider
        {
            get { return _provider; }
            set
            {
                _provider = value;
                RaisePropertyChanged();
            }
        }

        private byte[] _thumbnail;

        public byte[] Thumbnail
        {
            get { return _thumbnail; }
            set
            {
                _thumbnail = value;
                RaisePropertyChanged();
            }
        }

        //public string FullTitle { get { return $"{ProviderName} - {MapName}"; } }

        public string Name { get { return $"{Provider?.EnglishTitle}{MapType?.EnglishTitle}"; } }

        public string Title { get { return $"{Provider} {MapType}"; } }

        protected Func<TileInfo, string> MakeUrl { get; set; }

        public TileMapProvider(string provider, string mapType, Func<TileInfo, string> urlFunction)
            : this(new PersianEnglishItem(string.Empty, provider, Model.LanguageMode.English),
                    new PersianEnglishItem(string.Empty, mapType, Model.LanguageMode.English),
                    urlFunction)
        {
            //this.MakeUrl = urlFunction;

            //this.Provider = new PersianEnglishItem(string.Empty, provider, Model.LanguageMode.English);

            //this.MapType = new PersianEnglishItem(string.Empty, mapType, Model.LanguageMode.English);
        }

        public TileMapProvider(PersianEnglishItem provider, PersianEnglishItem mapType, Func<TileInfo, string> urlFunction)
        {
            this.MakeUrl = urlFunction;

            this.Provider = provider;

            this.MapType = mapType;
        }

        public virtual string GetUrl(TileInfo tile)
        {
            return MakeUrl?.Invoke(tile);
        }

        public override string ToString()
        {
            return Title;
        }

        public override bool Equals(object obj)
        {
            if (object.Equals(obj, null))
            {
                return false;
            }

            return (obj as TileMapProvider)?.Name?.EqualsIgnoreCase(this.Name) == true;
        }

        public override int GetHashCode()
        {
            return this.ToString().GetHashCode() ;
        }

        public static bool operator ==(TileMapProvider first, TileMapProvider second)
        {
            //using object.Equals handle the case of null==null otherwise it will use Equals and return false in this case 
            return object.Equals(first, second);
        }

        public static bool operator !=(TileMapProvider first, TileMapProvider second)
        {
            //using object.Equals handle the case of null==null otherwise it will use Equals and return false in this case
            return !object.Equals(first, second);
        }

    }
}
