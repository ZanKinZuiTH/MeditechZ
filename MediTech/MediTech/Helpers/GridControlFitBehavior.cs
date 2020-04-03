using DevExpress.Mvvm.UI.Interactivity;
using DevExpress.Xpf.Grid;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace MediTech.Helpers
{
    public class GridControlFitBehavior : Behavior<GridControl>
    {

        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.ItemsSourceChanged += AssociatedObject_ItemsSourceChanged;
            this.AssociatedObject.Loaded += AssociatedObject_Loaded;
        }

        void AssociatedObject_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            var collection = this.AssociatedObject.ItemsSource;
            this.AssociatedObject.ItemsSource = null;
            this.AssociatedObject.ItemsSource = collection;
            if ((this.AssociatedObject.View as TableView) != null)
                (this.AssociatedObject.View as TableView).BestFitColumns();
        }

        protected override void OnDetaching()
        {
            this.UnSubscribeCollectionChanged(this.AssociatedObject.ItemsSource);
            this.UnSubscribeItemChanged(this.AssociatedObject.ItemsSource);
            this.AssociatedObject.ItemsSourceChanged -= AssociatedObject_ItemsSourceChanged;
            this.AssociatedObject.Loaded -= AssociatedObject_Loaded;
            base.OnDetaching();
        }

        void AssociatedObject_ItemsSourceChanged(object sender, ItemsSourceChangedEventArgs e)
        {
            if (e.OldItemsSource != null)
            {
                UnSubscribeItemChanged(e.OldItemsSource);
                UnSubscribeCollectionChanged(e.OldItemsSource);
            }
            if (e.NewItemsSource != null)
            {
                SubscribeItemChanged(e.NewItemsSource);
                SubscribeCollectionChanged(e.NewItemsSource);
            }
            if ((this.AssociatedObject.View as TableView) != null)
                (this.AssociatedObject.View as TableView).BestFitColumns();
        }


        public void SubscribeCollectionChanged(object source)
        {
            var notifyCollection = source as INotifyCollectionChanged;
            if (notifyCollection != null)
            {
                notifyCollection.CollectionChanged += notifyCollection_CollectionChanged;
            }
        }

        public void UnSubscribeCollectionChanged(object source)
        {
            var notifyCollection = source as INotifyCollectionChanged;
            if (notifyCollection != null)
            {
                notifyCollection.CollectionChanged -= notifyCollection_CollectionChanged;
            }
        }

        public void SubscribeItemChanged(object source)
        {
            var collection = source as IList;
            foreach (object obj in collection)
            {
                var castObject = obj as INotifyPropertyChanged;
                if (castObject != null)
                {
                    castObject.PropertyChanged += castObject_PropertyChanged;
                }
            }
        }

        public void UnSubscribeItemChanged(object source)
        {
            var collection = source as IList;
            foreach (object obj in collection)
            {
                var castObject = obj as INotifyPropertyChanged;
                if (castObject != null)
                {
                    castObject.PropertyChanged -= castObject_PropertyChanged;
                }
            }
        }

        void notifyCollection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
                SubscribeItemChanged(e.NewItems);
            if (e.OldItems != null)
                UnSubscribeItemChanged(e.OldItems);
            (this.AssociatedObject.View as TableView).BestFitColumns();
        }


        void castObject_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            (this.AssociatedObject.View as TableView).BestFitColumns();
        }
    }
}
