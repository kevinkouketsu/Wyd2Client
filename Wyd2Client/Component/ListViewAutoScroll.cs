using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Wyd2.Client.Component
{

    public static class ListView
    {
        public static readonly DependencyProperty AutoScrollProperty =
            DependencyProperty.RegisterAttached("AutoScroll", typeof(bool), typeof(System.Windows.Controls.ListView),
            new PropertyMetadata(false));

        public static readonly DependencyProperty AutoScrollHandlerProperty =
            DependencyProperty.RegisterAttached("AutoScrollHandler", typeof(AutoScrollHandler), typeof(System.Windows.Controls.ListView));

        public static bool GetAutoScroll(System.Windows.Controls.ListView instance)
        {
            return (bool)instance.GetValue(AutoScrollProperty);
        }

        public static void SetAutoScroll(System.Windows.Controls.ListView instance, bool value)
        {
            AutoScrollHandler OldHandler = (AutoScrollHandler)instance.GetValue(AutoScrollHandlerProperty);
            if (OldHandler != null)
            {
                OldHandler.Dispose();
                instance.SetValue(AutoScrollHandlerProperty, null);
            }
            instance.SetValue(AutoScrollProperty, value);
            if (value)
                instance.SetValue(AutoScrollHandlerProperty, new AutoScrollHandler(instance));
        }

    }

    public class AutoScrollHandler : DependencyObject, IDisposable
    {

        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(IEnumerable),
            typeof(AutoScrollHandler), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None,
                new PropertyChangedCallback(ItemsSourcePropertyChanged)));

        private System.Windows.Controls.ListBox Target;

        public AutoScrollHandler(System.Windows.Controls.ListBox target)
        {
            Target = target;
            Binding B = new Binding("ItemsSource");
            B.Source = Target;
            BindingOperations.SetBinding(this, ItemsSourceProperty, B);
        }

        public void Dispose()
        {
            BindingOperations.ClearBinding(this, ItemsSourceProperty);
        }

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        static void ItemsSourcePropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            ((AutoScrollHandler)o).ItemsSourceChanged((IEnumerable)e.OldValue, (IEnumerable)e.NewValue);
        }

        void ItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            INotifyCollectionChanged Collection = oldValue as INotifyCollectionChanged;
            if (Collection != null)
                Collection.CollectionChanged -= new NotifyCollectionChangedEventHandler(Collection_CollectionChanged);
            Collection = newValue as INotifyCollectionChanged;
            if (Collection != null)
                Collection.CollectionChanged += new NotifyCollectionChangedEventHandler(Collection_CollectionChanged);
        }

        void Collection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action != NotifyCollectionChangedAction.Add || e.NewItems == null || e.NewItems.Count < 1)
                return;
            Target.ScrollIntoView(e.NewItems[e.NewItems.Count - 1]);
        }

    }
}
