using IRI.Jab.Common;
using IRI.Jab.Common.Assets.Commands;
using IRI.Jab.MultiSelectItem.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Jab.MultiSelectItem.ViewModel
{

    public class MultiSelectItemViewModel<T> : IMultiSelectItemViewModel where T : class
    {
        public MultiSelectItemViewModel()
        {

        }

        private Func<T, string> _getSelectedItemTitleFunc;

        public event NotifyCollectionChangedEventHandler OnItemsChanged;

        private Func<string, T> _createFunc;

        public event EventHandler OnItemAddedToAllItems;

        public MultiSelectItemViewModel(IEnumerable<T> allItems, Func<T, string> getValueTitle, Func<string, T> createFunc = null) : this(allItems, null, getValueTitle, createFunc)
        {
            //this._allItems = new ObservableCollection<T>();

            //this._selectedItems = new ObservableCollection<SelectedItemModel<T>>();

            //if (allItems != null)
            //{
            //    foreach (var item in allItems)
            //    {
            //        this.AllItems.Add(item);
            //    }
            //}

            //this._getSelectedItemTitleFunc = getValueTitle;

            //this._createFunc = createFunc;

            //this._selectedItems.CollectionChanged += (sender, e) =>
            //{
            //    //ForceValidation();

            //    this.OnItemsChanged?.Invoke(this, EventArgs.Empty);
            //};

            ////ForceValidation();
        }

        public MultiSelectItemViewModel(IEnumerable<T> allItems, ICollection<T> listToBind, Func<T, string> getValueTitle, Func<string, T> createFunc = null)
        {
            this._allItems = new ObservableCollection<T>();

            this._selectedItems = new ObservableCollection<SelectedItemModel<T>>();

            if (allItems != null)
            {
                foreach (var item in allItems)
                {
                    this.AllItems.Add(item);
                }
            }

            this._getSelectedItemTitleFunc = getValueTitle;

            this._createFunc = createFunc;

            this._selectedItems.CollectionChanged += (sender, e) =>
            {
                this.OnItemsChanged?.Invoke(sender, e);
            };

            if (listToBind == null)
                return;

            foreach (var item in listToBind)
            {
                this.Add(item);
            }

            this.OnItemsChanged += (sender, e) =>
            {
                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        foreach (var item in e.NewItems)
                        {
                            listToBind.Add((item as SelectedItemModel<T>).Value);
                        }

                        break;
                    case NotifyCollectionChangedAction.Remove:
                        foreach (var item in e.OldItems)
                        {
                            listToBind.Remove((item as SelectedItemModel<T>).Value);
                        }

                        break;
                    case NotifyCollectionChangedAction.Replace:
                    case NotifyCollectionChangedAction.Move:
                    case NotifyCollectionChangedAction.Reset:
                    default:
                        throw new NotImplementedException("source: MultiSelectItemViewModel");
                }
                //listToBind.Clear();

                //for (int i = 0; i < this.SelectedValues.Count(); i++)
                //{
                //    listToBind.Add(this.SelectedValues.ElementAt(i));
                //}
            };
        }

        public void BindWith(List<T> selectedItems)
        {
            if (selectedItems == null)
            {
                throw new NotImplementedException();
            }

            foreach (var item in selectedItems)
            {
                this.Add(item);
            }

            this.OnItemsChanged = null;

            this.OnItemsChanged += (sender, e) =>
            {
                selectedItems.Clear();

                for (int i = 0; i < this.SelectedValues.Count(); i++)
                {
                    selectedItems.Add(this.SelectedValues.ElementAt(i));
                }
            };
        }

        private ObservableCollection<T> _allItems;

        public ObservableCollection<T> AllItems
        {
            get { return _allItems; }
            private set
            {
                _allItems = value;
                RaisePropertyChanged();
            }
        }


        private ObservableCollection<SelectedItemModel<T>> _selectedItems;

        public ObservableCollection<SelectedItemModel<T>> SelectedItems
        {
            get { return _selectedItems; }
            private set
            {
                _selectedItems = value;
                RaisePropertyChanged();
            }
        }

        public IEnumerable<T> SelectedValues
        {
            get { return this.SelectedItems?.Select(i => i.Value); }
        }

        private T _currentSelectedItem;

        public T CurrentSelectedItem
        {
            get { return _currentSelectedItem; }
            set
            {
                _currentSelectedItem = value;
                RaisePropertyChanged();

                if (!IsMultiSelectEnabled)
                {
                    RemoveAll();

                    AddItem();
                }

                //Validate();
            }
        }

        private string _currentText;

        public string CurrentText
        {
            get { return _currentText; }
            set
            {
                _currentText = value;
                RaisePropertyChanged();
                //Validate();
                //ForceValidation();
            }
        }

        private bool _isRequired = false;

        public bool IsRequired
        {
            get { return _isRequired; }
            set
            {
                _isRequired = value;
                RaisePropertyChanged();

                //ForceValidation();
            }
        }

        private string _displayMemberPath;

        public string DisplayMemberPath
        {
            get { return _displayMemberPath; }
            set
            {
                _displayMemberPath = value;
                RaisePropertyChanged();
            }
        }


        private RelayCommand _addCommand;

        public RelayCommand AddCommand
        {
            get
            {
                if (_addCommand == null)
                {
                    _addCommand = new RelayCommand(param => AddItem());
                }

                return _addCommand;
            }
        }

        internal override void AddItem()
        {
            if (this.CurrentSelectedItem == null)
            {
                if (this.IsMultiSelectEnabled)
                {
                    AddNewItem();
                }
            }
            else
            {
                //Check if multi select criteria is ok
                if (!this.IsMultiSelectEnabled && this.SelectedItems.Count > 0)
                    return;

                //Avoid empty string or values to be added
                if (string.IsNullOrWhiteSpace(_getSelectedItemTitleFunc(this.CurrentSelectedItem)))
                {
                    return;
                }

                AddItemToSelectedList(this.CurrentSelectedItem);
            }

            if (this.IsMultiSelectEnabled)
            {
                this.CurrentSelectedItem = null;

                this.CurrentText = string.Empty;
            }
        }

        public void Add(T value)
        {
            this.CurrentSelectedItem = value;

            AddItem();
        }

        private void AddNewItem()
        {
            if (this.IsMultiSelectEnabled && !string.IsNullOrWhiteSpace(this.CurrentText))
            {
                //Check if item already is available at list
                if (this.AllItems.Any(i => _getSelectedItemTitleFunc(i) == this.CurrentText))
                {
                    var current = this.AllItems.First(i => _getSelectedItemTitleFunc(i) == this.CurrentText);

                    this.CurrentSelectedItem = current;

                    AddItemToSelectedList(current);
                }
                else
                {
                    if (_createFunc == null)
                    {
                        return;
                    }

                    var current = _createFunc(this.CurrentText);

                    this.CurrentSelectedItem = current;

                    this.AllItems.Add(current);

                    this.OnItemAddedToAllItems?.Invoke(current, EventArgs.Empty);

                    AddItemToSelectedList(current);
                }
            }
        }

        private void AddItemToSelectedList(T value)
        {
            //Do not add duplicate values
            if (this.SelectedItems.Any(i => i.Value.Equals(value)))
                return;

            if (value == null || string.IsNullOrWhiteSpace(_getSelectedItemTitleFunc(value)))
            {
                return;
            }

            var newItem = new SelectedItemModel<T>(value, _getSelectedItemTitleFunc);

            newItem.OnRequestRemove += (sender, i) => { this.RemoveItem(((SelectedItemModel<T>)sender)); };

            this.SelectedItems.Add(newItem);

            //this.OnItemsChanged?.Invoke(this, EventArgs.Empty);
        }

        public void RemoveItem(SelectedItemModel<T> item)
        {
            if (this.SelectedItems.Contains(item))
            {
                this.SelectedItems.Remove(item);

                this.CurrentSelectedItem = null;

                //this.OnItemsChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public void RemoveAll()
        {
            if (this.SelectedItems != null && this.SelectedItems.Count > 0)
            {
                this.SelectedItems.Clear();

                //this.OnItemsChanged?.Invoke(this, EventArgs.Empty);
            }
        }







    }
}
