using IRI.Jab.Common.Assets.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Jab.Common.Model
{
    public class EditCommandAwareItem : Notifier
    {
        public Action RemoveAction, AddAction, EditAction;

        public EditCommandAwareItem()
        {

        }

        public EditCommandAwareItem(Action removeAction, Action addAction, Action editAction)
        {
            this.RemoveAction = removeAction;

            this.AddAction = addAction;

            this.EditAction = editAction;
        }

        private RelayCommand _removeCommand;

        public RelayCommand RemoveCommand
        {
            get
            {
                if (_removeCommand == null)
                {
                    _removeCommand = new RelayCommand(param => this.RemoveAction?.Invoke());
                }
                return _removeCommand;
            }
        }

        private RelayCommand _addCommand;

        public RelayCommand AddCommand
        {
            get
            {
                if (_addCommand == null)
                {
                    _addCommand = new RelayCommand(param => this.AddAction?.Invoke());
                }
                return _addCommand;
            }
        }

        private RelayCommand _editCommand;

        public RelayCommand EditCommand
        {
            get
            {
                if (_editCommand == null)
                {
                    _editCommand = new RelayCommand(param => this.EditAction?.Invoke());
                }
                return _editCommand;
            }
        }
    }
}
