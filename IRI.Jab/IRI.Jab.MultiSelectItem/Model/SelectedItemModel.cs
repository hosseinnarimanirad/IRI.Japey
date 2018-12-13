﻿using IRI.Jab.Common;
using IRI.Jab.Common.Assets.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Jab.MultiSelectItem.Model
{
    public class SelectedItemModel<T> : Notifier
    {
        private T _value;

        public T Value
        {
            get { return _value; }
            set
            {
                _value = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(Title));
            }
        }

        private string _title;

        public string Title
        {
            get { return _getTitleFunc(Value); }
        }

        private Func<T, string> _getTitleFunc;

        public SelectedItemModel(T value, Func<T, string> getTitleFunc)
        {
            this._getTitleFunc = getTitleFunc;

            this.Value = value;
        }

        private RelayCommand _removeCommand;

        public RelayCommand RemoveCommand
        {
            get
            {
                if (_removeCommand == null)
                {
                    _removeCommand = new RelayCommand(param => this.RequestRemove());
                }
                return _removeCommand;
            }
        }

        private void RequestRemove()
        {
            this.OnRequestRemove?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler OnRequestRemove;
    }
}