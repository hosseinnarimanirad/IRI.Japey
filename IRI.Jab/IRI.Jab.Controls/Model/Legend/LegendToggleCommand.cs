﻿using IRI.Jab.Common;
using IRI.Jab.Common.Assets.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Jab.Controls.Model.Legend
{
    public class LegendToggleCommand : Notifier, ILegendCommand
    {
        private RelayCommand _command;

        public RelayCommand Command
        {
            get { return _command; }
            set
            {
                _command = value;
                RaisePropertyChanged();
            }
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

        private bool _isSelected;

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                RaisePropertyChanged();

                Command.Execute(value);
            }
        }

    }
}
