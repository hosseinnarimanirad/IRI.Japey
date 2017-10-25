using IRI.Jab.Common.Assets.Commands;
using IRI.Jab.Common.Assets.Data;
using IRI.Jab.Common.Assets.ShapeStrings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Jab.Common.Model.Spatialable
{

    public class EnvelopeMarkupLabelTriple : Notifier
    {
        private string _base64EnvelopeWm;

        public string Base64EnvelopeWm
        {
            get { return _base64EnvelopeWm; }
            set
            {
                _base64EnvelopeWm = value;
                RaisePropertyChanged();
            }
        }

        public byte[] GetEnvelopeWkbWm()
        {           
            return Convert.FromBase64String(Base64EnvelopeWm);
        }

        private string _pathMarkup;

        public string PathMarkup
        {
            get { return _pathMarkup; }
            set
            {
                _pathMarkup = value;
                RaisePropertyChanged();
            }
        }

        private string _label;

        public string Label
        {
            get { return _label; }
            set
            {
                _label = value;
                RaisePropertyChanged();
            }
        }

        public EnvelopeMarkupLabelTriple(string label, string markup, string base64EnvelopeWm)
        {
            this.Label = label;

            this.PathMarkup = markup;

            this.Base64EnvelopeWm = base64EnvelopeWm;
        }

        public Action<EnvelopeMarkupLabelTriple> RequestRaiseSelected { get; set; }

        private RelayCommand _selectedCommand;

        public RelayCommand SelectedCommand
        {
            get
            {
                if (_selectedCommand == null)
                {
                    _selectedCommand = new RelayCommand((param) => this.RequestRaiseSelected?.Invoke(this));
                }
                return _selectedCommand;
            }
        }


        public static List<EnvelopeMarkupLabelTriple> GetProvinces93Wm(Action<EnvelopeMarkupLabelTriple> selectAction)
        {
            return new List<EnvelopeMarkupLabelTriple>()
            {
                new EnvelopeMarkupLabelTriple("البرز",IriProvinces93.Alborz, IriProvinces93WmEnvelopes.Alborz){ RequestRaiseSelected = selectAction },
                new EnvelopeMarkupLabelTriple("اردبیل",IriProvinces93.Ardabil,IriProvinces93WmEnvelopes.Ardabil){ RequestRaiseSelected = selectAction },
                new EnvelopeMarkupLabelTriple("آذربایجان غربی",IriProvinces93.AzarbayejaneGarbi,  IriProvinces93WmEnvelopes.AzarbayejaneGarbi){ RequestRaiseSelected = selectAction },
                new EnvelopeMarkupLabelTriple("آذربایجان شرقی",IriProvinces93.AzarbayejaneShargi,IriProvinces93WmEnvelopes.AzarbayejaneShargi){ RequestRaiseSelected = selectAction },
                new EnvelopeMarkupLabelTriple("اصفهان",IriProvinces93.Esfahan,IriProvinces93WmEnvelopes.Esfahan){ RequestRaiseSelected = selectAction },
                new EnvelopeMarkupLabelTriple("بوشهر",IriProvinces93.Booshehr,IriProvinces93WmEnvelopes.Booshehr){ RequestRaiseSelected = selectAction },
                new EnvelopeMarkupLabelTriple("چهارمحال و...",IriProvinces93.Chaharmahal,IriProvinces93WmEnvelopes.Chaharmahal){ RequestRaiseSelected = selectAction },
                new EnvelopeMarkupLabelTriple("فارس",IriProvinces93.Fars,IriProvinces93WmEnvelopes.Fars){ RequestRaiseSelected = selectAction },
                new EnvelopeMarkupLabelTriple("قزوین",IriProvinces93.Gazvin,IriProvinces93WmEnvelopes.Gazvin){ RequestRaiseSelected = selectAction },
                new EnvelopeMarkupLabelTriple("گیلان",IriProvinces93.Gilan,IriProvinces93WmEnvelopes.Gilan){ RequestRaiseSelected = selectAction },
                new EnvelopeMarkupLabelTriple("گلستان",IriProvinces93.Golestan,IriProvinces93WmEnvelopes.Golestan){ RequestRaiseSelected = selectAction },
                new EnvelopeMarkupLabelTriple("همدان",IriProvinces93.Hamedan,IriProvinces93WmEnvelopes.Hamedan){ RequestRaiseSelected = selectAction },
                new EnvelopeMarkupLabelTriple("هرمزگان",IriProvinces93.Hormozgan,IriProvinces93WmEnvelopes.Hormozgan){ RequestRaiseSelected = selectAction },
                new EnvelopeMarkupLabelTriple("ایلام",IriProvinces93.Ilam,IriProvinces93WmEnvelopes.Ilam){ RequestRaiseSelected = selectAction },
                new EnvelopeMarkupLabelTriple("کرمان",IriProvinces93.Kerman,IriProvinces93WmEnvelopes.Kerman){ RequestRaiseSelected = selectAction },
                new EnvelopeMarkupLabelTriple("کرمانشاه",IriProvinces93.Kermanshah,IriProvinces93WmEnvelopes.Kermanshah){ RequestRaiseSelected = selectAction },
                new EnvelopeMarkupLabelTriple("خراسان جنوبی",IriProvinces93.KhorasanJonoobi,IriProvinces93WmEnvelopes.KhorasanJonoobi){ RequestRaiseSelected = selectAction },
                new EnvelopeMarkupLabelTriple("خراسان رضوی",IriProvinces93.KhorasanRazavi,IriProvinces93WmEnvelopes.KhorasanRazavi){ RequestRaiseSelected = selectAction },
                new EnvelopeMarkupLabelTriple("خراسان شمالی",IriProvinces93.KhorasanShomali,IriProvinces93WmEnvelopes.KhorasanShomali){ RequestRaiseSelected = selectAction },
                new EnvelopeMarkupLabelTriple("خوزستان",IriProvinces93.Khozestan,IriProvinces93WmEnvelopes.Khozestan){ RequestRaiseSelected = selectAction },
                new EnvelopeMarkupLabelTriple("کهگیلویه و ...",IriProvinces93.Kohkiloye,IriProvinces93WmEnvelopes.Kohkiloye){ RequestRaiseSelected = selectAction },
                new EnvelopeMarkupLabelTriple("کردستان",IriProvinces93.Kordestan,IriProvinces93WmEnvelopes.Kordestan){ RequestRaiseSelected = selectAction },
                new EnvelopeMarkupLabelTriple("لرستان",IriProvinces93.Lorestan,IriProvinces93WmEnvelopes.Lorestan){ RequestRaiseSelected = selectAction },
                new EnvelopeMarkupLabelTriple("مرکزی",IriProvinces93.Markazi,IriProvinces93WmEnvelopes.Markazi){ RequestRaiseSelected = selectAction },
                new EnvelopeMarkupLabelTriple("مازندران",IriProvinces93.Mazandaran,IriProvinces93WmEnvelopes.Mazandaran){ RequestRaiseSelected = selectAction },
                new EnvelopeMarkupLabelTriple("قم",IriProvinces93.Qom,IriProvinces93WmEnvelopes.Qom){ RequestRaiseSelected = selectAction },
                new EnvelopeMarkupLabelTriple("سمنان",IriProvinces93.Semnan,IriProvinces93WmEnvelopes.Semnan){ RequestRaiseSelected = selectAction },
                new EnvelopeMarkupLabelTriple("سیستان و ...",IriProvinces93.Sistan,IriProvinces93WmEnvelopes.Sistan){ RequestRaiseSelected = selectAction },
                new EnvelopeMarkupLabelTriple("تهران",IriProvinces93.Tehran,IriProvinces93WmEnvelopes.Tehran){ RequestRaiseSelected = selectAction },
                new EnvelopeMarkupLabelTriple("یزد",IriProvinces93.Yazd,IriProvinces93WmEnvelopes.Yazd){ RequestRaiseSelected = selectAction },
                new EnvelopeMarkupLabelTriple("زنجان",IriProvinces93.Zanjan,IriProvinces93WmEnvelopes.Zanjan){ RequestRaiseSelected = selectAction },
            }.OrderBy(i => i.Label).ToList();
        }
    }

}
