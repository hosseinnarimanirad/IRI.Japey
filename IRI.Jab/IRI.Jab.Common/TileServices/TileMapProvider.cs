using IRI.Jab.Common.Model.Globalization;
using IRI.Msh.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Jab.Common.TileServices
{
    public class TileMapProvider : ValueObjectNotifier
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

        private readonly PersianEnglishItem _mapType;

        public PersianEnglishItem MapType
        {
            get { return _mapType; }
            //private set
            //{
            //    _mapType = value;
            //    RaisePropertyChanged();
            //}
        }

        private readonly PersianEnglishItem _provider;

        public PersianEnglishItem Provider
        {
            get { return _provider; }
            //private set
            //{
            //    _provider = value;
            //    RaisePropertyChanged();
            //}
        }

        private byte[] _thumbnail;

        public byte[] Thumbnail
        {
            get { return _thumbnail; }
            protected set
            {
                _thumbnail = value;
                RaisePropertyChanged();
            }
        }

        private byte[] _thumbnail72;

        public byte[] Thumbnail72
        {
            get { return _thumbnail72; }
            set
            {
                _thumbnail72 = value;
                RaisePropertyChanged();
            }
        }


        //public string FullTitle { get { return $"{ProviderName} - {MapName}"; } }

        public string FullName { get { return $"{Provider?.EnglishTitle}{MapType?.EnglishTitle}"; } }

        public string Title { get { return $"{Provider} {MapType}"; } }

        public Func<TileInfo, string> MakeUrl { get; protected set; }

        protected TileMapProvider()
        {

        }

        public TileMapProvider(string provider, string mapType, Func<TileInfo, string> urlFunction, byte[] thumbnail)
            : this(PersianEnglishItem.CreateUpperCasedEnglish(string.Empty, provider),
                    PersianEnglishItem.CreateUpperCasedEnglish(string.Empty, mapType),
                    urlFunction, thumbnail)
        {
            //this.MakeUrl = urlFunction;

            //this.Provider = new PersianEnglishItem(string.Empty, provider, Model.LanguageMode.English);

            //this.MapType = new PersianEnglishItem(string.Empty, mapType, Model.LanguageMode.English);
        }

        public TileMapProvider(PersianEnglishItem provider, PersianEnglishItem mapType, Func<TileInfo, string> urlFunction, byte[] thumbnail)
        {
            this.MakeUrl = urlFunction;

            this._provider = provider;

            this._mapType = mapType;

            this._thumbnail = thumbnail;
        }

        public virtual string GetUrl(TileInfo tile)
        {
            return MakeUrl?.Invoke(tile);
        }

        public override string ToString()
        {
            return Title;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Provider;
            yield return MapType;
        }

        //public override bool Equals(object obj)
        //{
        //    if (object.Equals(obj, null))
        //    {
        //        return false;
        //    }

        //    return (obj as TileMapProvider)?.Name?.EqualsIgnoreCase(this.Name) == true;
        //}

        //public override int GetHashCode()
        //{
        //    return this.ToString().GetHashCode();
        //}

        //public static bool operator ==(TileMapProvider first, TileMapProvider second)
        //{
        //    //using object.Equals handle the case of null==null otherwise it will use Equals and return false in this case 
        //    return object.Equals(first, second);
        //}

        //public static bool operator !=(TileMapProvider first, TileMapProvider second)
        //{
        //    //using object.Equals handle the case of null==null otherwise it will use Equals and return false in this case
        //    return !object.Equals(first, second);
        //}

    }
}
