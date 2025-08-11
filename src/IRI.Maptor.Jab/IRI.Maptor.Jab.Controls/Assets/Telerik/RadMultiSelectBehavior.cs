using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
//using System.Windows.Interactivity;
using Telerik.Windows.Controls;

namespace IRI.Maptor.Jab.Controls.Assets.Telerik
{
    public class RadMultiSelectBehavior : Behavior<RadGridView>
    {
        private RadGridView Grid
        {
            get
            {
                return AssociatedObject as RadGridView;
            }
        }

        public INotifyCollectionChanged SelectedItems
        {
            get { return (INotifyCollectionChanged)GetValue(SelectedItemsProperty); }
            set { SetValue(SelectedItemsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedItemsProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedItemsProperty =
            DependencyProperty.Register(nameof(SelectedItems), typeof(INotifyCollectionChanged), typeof(RadMultiSelectBehavior), new PropertyMetadata(OnSelectedItemsPropertyChanged));


        private static void OnSelectedItemsPropertyChanged(DependencyObject target, DependencyPropertyChangedEventArgs args)
        {
            var collection = args.NewValue as INotifyCollectionChanged;
            if (collection != null)
            {
                collection.CollectionChanged += ((RadMultiSelectBehavior)target).ContextSelectedItems_CollectionChanged;
            }
        }

        protected override void OnAttached()
        {
            base.OnAttached();

            Grid.SelectedItems.CollectionChanged += GridSelectedItems_CollectionChanged;
        }

        void ContextSelectedItems_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            UnsubscribeFromEvents();

            Transfer(SelectedItems as System.Collections.IList, Grid.SelectedItems);

            SubscribeToEvents();
        }

        void GridSelectedItems_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            UnsubscribeFromEvents();

            Transfer(Grid.SelectedItems, SelectedItems as System.Collections.IList);

            SubscribeToEvents();
        }

        private void SubscribeToEvents()
        {
            Grid.SelectedItems.CollectionChanged += GridSelectedItems_CollectionChanged;

            if (SelectedItems != null)
            {
                SelectedItems.CollectionChanged += ContextSelectedItems_CollectionChanged;
            }
        }

        private void UnsubscribeFromEvents()
        {
            Grid.SelectedItems.CollectionChanged -= GridSelectedItems_CollectionChanged;

            if (SelectedItems != null)
            {
                SelectedItems.CollectionChanged -= ContextSelectedItems_CollectionChanged;
            }
        }

        public static void Transfer(System.Collections.IList source, System.Collections.IList target)
        {
            if (source == null || target == null)
                return;

            target.Clear();

            foreach (var o in source)
            {
                target.Add(o);
            }
        }
    }
}
