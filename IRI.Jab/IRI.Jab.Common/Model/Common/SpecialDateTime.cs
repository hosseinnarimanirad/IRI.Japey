using IRI.Ket.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Jab.Common.Model.Common
{
    public class SpecialDateTime : Notifier
    {
        //public SpecialDateTime(DateTime? dateTime, Action<SpecialDateTime> onChangeAction)
        //{
        //    this.ADDate = dateTime;

        //    this.ChangeAction = onChangeAction;
        //}

        public SpecialDateTime(DateTime? dateTime, Action<SpecialDateTime> onChangeAction = null)
        {
            this.ADDate = dateTime;

            this.ChangeAction = onChangeAction;
        }

        private DateTime? _adDate;

        public DateTime? ADDate
        {
            get { return _adDate; }
            set
            {
                if (value == _adDate)
                    return;

                _adDate = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(FullPersianAlphabeticDate));
                RaisePropertyChanged(nameof(LongPersianDateTime));
                RaisePropertyChanged(nameof(Ticks));

                this.ChangeAction?.Invoke(this);
            }
        }

        public string FullPersianAlphabeticDate
        {
            get
            {
                return ADDate?.ToPersianAlphabeticDate() ?? string.Empty;
            }
        }

        public string PersianDate
        {
            get
            {
                return ADDate?.ToPersianDate() ?? string.Empty;
            }
        }


        public string LongPersianDateTime
        {
            get
            {
                return ADDate?.ToLongPersianDateTime() ?? string.Empty;
            }
        }

        public long Ticks
        {
            get
            {
                return ADDate?.Ticks ?? -1;
            }
        }

        public bool HasValue
        {
            get { return this.ADDate.HasValue; }
        }

        public Action<SpecialDateTime> ChangeAction;

        public event EventHandler<CustomEventArgs<SpecialDateTime>> OnChanged;
    }
}
