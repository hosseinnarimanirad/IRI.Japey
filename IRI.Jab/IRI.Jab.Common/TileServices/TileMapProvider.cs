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

        private string _subTitle = "ROAD";

        public string SubTitle
        {
            get { return _subTitle; }
            set
            {
                _subTitle = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(FullTitle));
            }
        }

        private string _providerName;

        public string ProviderName
        {
            get { return _providerName; }
            protected set
            {
                _providerName = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(FullTitle));
            }
        }

        public string FullTitle { get { return $"{ProviderName} - {SubTitle}"; } }

        public string FullName { get { return $"{ProviderName}{SubTitle}"; } }

        protected Func<TileInfo, string> MakeUrl { get; set; }

        public TileMapProvider(string providerName, string subTitle, Func<TileInfo, string> urlFunction)
        {
            this.MakeUrl = urlFunction;

            this.ProviderName = providerName;

            this.SubTitle = subTitle;
        }

        public virtual string GetUrl(TileInfo tile)
        {
            return MakeUrl?.Invoke(tile);
        }

        public override string ToString()
        {
            return FullTitle;
        }


    }
}
