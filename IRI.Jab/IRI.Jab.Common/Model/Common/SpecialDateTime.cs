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

                FullPersianAlphabeticDate = value?.ToPersianAlphabeticDate() ?? string.Empty;

                PersianDate = value?.ToPersianDate() ?? string.Empty; ;

                LongPersianDateTime = value?.ToLongPersianDateTime() ?? string.Empty;

                RaisePropertyChanged(nameof(Ticks));

                this.ChangeAction?.Invoke(this);
            }
        }

        private string _fullPersianAlphabeticDate;

        public string FullPersianAlphabeticDate
        {
            get
            {
                return _fullPersianAlphabeticDate;
            }
            private set
            {
                _fullPersianAlphabeticDate = value;
                RaisePropertyChanged();
            }
        }


        private string _persianDate;

        public string PersianDate
        {
            get
            {
                return _persianDate;
            }
            private set
            {
                _persianDate = value;
                RaisePropertyChanged();
            }
        }


        private string _longPersianDateTime;

        public string LongPersianDateTime
        {
            get
            {
                return _longPersianDateTime;
            }
            private set
            {
                _longPersianDateTime = value;
                RaisePropertyChanged();
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

        public override int GetHashCode()
        {
            return ADDate.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var item = obj as SpecialDateTime;

            if (item == null)
            {
                return false;
            }

            return this.ADDate.Equals(item.ADDate);
        }

        public Action<SpecialDateTime> ChangeAction;

        public event EventHandler<CustomEventArgs<SpecialDateTime>> OnChanged;
    }
}
