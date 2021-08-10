using System;
using System.Collections;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace CommonUILib.Views
{
   
    /// <summary>
    /// Interaction logic for HistoryChartControl.xaml
    /// </summary>
    public partial class HistoryChartControl : UserControl
    {
        #region Itemsources property
        /// <summary>
        /// The ItemsSource
        /// </summary>
        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }
        /// <summary>
        /// The ItemsSource Property
        /// </summary>
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(HistoryChartControl), new PropertyMetadata(new PropertyChangedCallback(OnItemsSourcePropertyChanged)));
        /// <summary>
        /// The ItemsSource property changed event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnItemsSourcePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var control = sender as HistoryChartControl;
            if (control != null)
                control.OnItemsSourceChanged((IEnumerable)e.OldValue, (IEnumerable)e.NewValue);
        }
        /// <summary>
        /// The ItemsSource change
        /// </summary>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        private void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            // Remove handler for oldValue.CollectionChanged
            var oldValueINotifyCollectionChanged = oldValue as INotifyCollectionChanged;

            if (null != oldValueINotifyCollectionChanged)
            {
                oldValueINotifyCollectionChanged.CollectionChanged -= new NotifyCollectionChangedEventHandler(newValueINotifyCollectionChanged_CollectionChanged);
            }
            // Add handler for newValue.CollectionChanged (if possible)
            var newValueINotifyCollectionChanged = newValue as INotifyCollectionChanged;
            if (null != newValueINotifyCollectionChanged)
            {
                newValueINotifyCollectionChanged.CollectionChanged += new NotifyCollectionChangedEventHandler(newValueINotifyCollectionChanged_CollectionChanged);
            }

        }
        /// <summary>
        /// The New value notify collection change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void newValueINotifyCollectionChanged_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            //Do your stuff here.
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new Action(() =>
                    {
                        //DrawMonitorLinePoint((TestConfigStruct)e.NewItems[0]);
                        //UpdateBackgroundLineData();
                    }));
                    break;
                case NotifyCollectionChangedAction.Move:
                case NotifyCollectionChangedAction.Remove:
                case NotifyCollectionChangedAction.Replace:
                case NotifyCollectionChangedAction.Reset:
                    break;
            }
        }

        #endregion
        public HistoryChartControl()
        {
            InitializeComponent();
        }
    }
}
