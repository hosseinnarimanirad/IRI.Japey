//using Microsoft.Xaml.Behaviors;
//using System;
//using System.Collections.Specialized;
////using System.Windows.Interactivity;

//namespace IRI.Maptor.Jab.Controls.Assets.Telerik
//{
//    public class RadGridViewAutoScrollToSelectedItemBehavior : Behavior<RadGridView>
//    {
//        private RadGridView Grid
//        {
//            get { return AssociatedObject; }
//        }

//        protected override void OnAttached()
//        {
//            base.OnAttached();
//            Grid.Items.CollectionChanged += Items_CollectionChanged;
//            Grid.SelectionChanged += Grid_SelectionChanged;
//        }

//        private void Grid_SelectionChanged(object sender, SelectionChangeEventArgs e)
//        {
//            try
//            {
//                var row = e.AddedItems[0];

//                Grid.ScrollIntoView(row);
//            }
//            catch (Exception ex)
//            {

//            }
//        }

//        private void Items_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
//        {
//            if (e.Action == NotifyCollectionChangedAction.Add)
//            {
//                var myRow = e.NewItems[0];

//                Grid.ScrollIntoView(myRow);

//            }
//        }
//    }

//}
