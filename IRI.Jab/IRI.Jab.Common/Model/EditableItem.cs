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
        public Action RequestRemoveAction, AddAction, RequestEditAction;

        public EditCommandAwareItem()
        {

        }

        public EditCommandAwareItem(Action removeAction, Action addAction, Action editAction)
        {
            this.RequestRemoveAction = removeAction;

            this.AddAction = addAction;

            this.RequestEditAction = editAction;
        }

        private RelayCommand _removeCommand;

        public RelayCommand RemoveCommand
        {
            get
            {
                if (_removeCommand == null)
                {
                    _removeCommand = new RelayCommand(param => this.RequestRemoveAction?.Invoke());
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
                    _editCommand = new RelayCommand(param => this.RequestEditAction?.Invoke());
                }
                return _editCommand;
            }
        }
    }
}
