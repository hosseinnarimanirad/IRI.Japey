using IRI.Jab.Common;
using IRI.Jab.Controls.Model.CoordinatePanel;
using IRI.Ket.SqlServerSpatialExtension.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRI.Jab.Common.Model;
using System.Windows;

namespace IRI.Jab.Controls.Presenter
{
    public class CoordinatePanelPresenter : Notifier
    {
        private ObservableCollection<SpatialReferenceItem> _spatialReferences = new ObservableCollection<SpatialReferenceItem>();

        public ObservableCollection<SpatialReferenceItem> SpatialReferences
        {
            get { return _spatialReferences; }
            private set
            {
                _spatialReferences = value;
                RaisePropertyChanged();
            }
        }

        private SpatialReferenceItem _selectedItem;

        public SpatialReferenceItem SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                RaisePropertyChanged();
            }
        }


        public CoordinatePanelPresenter()
        {
            this.SpatialReferences = new ObservableCollection<SpatialReferenceItem>();

            this.SpatialReferences.CollectionChanged += (sender, e) =>
            {
                UpdateSelectedItem();
            };

            this.SpatialReferences.Add(SpatialReferenceItems.GeodeticWgs84);
            this.SpatialReferences.Add(SpatialReferenceItems.GeodeticDmsWgs84);
            this.SpatialReferences.Add(SpatialReferenceItems.UtmWgs84);

            this.SpatialReferences.First().IsSelected = true;

            SetLanguage(LanguageMode.Persian);
        }

        private void UpdateSelectedItem()
        { 
            foreach (var item in SpatialReferences)
            {
                item.FireIsSelectedChanged = e => { this.SelectedItem = e; };
            }
        }

        internal void SetLanguage(LanguageMode value)
        {
            foreach (var item in SpatialReferences)
            {
                item.UILanguage = value;
            }

            this.UIFlow = value == LanguageMode.Persian ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;
        }

        private FlowDirection _uiFlow;

        public FlowDirection UIFlow
        {
            get { return _uiFlow; }
            set
            {
                _uiFlow = value;
                RaisePropertyChanged();
                System.Diagnostics.Debug.WriteLine("UIFlow: " + value.ToString());
            }
        }

    }
}
