using CommonUILib.Structures;
using System;
using System.Collections;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
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
                        DrawMonitorLinePoint((XYData)e.NewItems[0]);
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

        private void DrawMonitorLinePoint(XYData xYData)
        {
            DrawLineInCanvas(xYData.Yis, chartGrid.ActualHeight, YmainLine, Models.CommonUtility.Instance.GetGradientBrushByHex("#bf80ff", "#a044ff", 0.0));
        }

        #endregion
        public HistoryChartControl()
        {
            InitializeComponent();
        }
        const double lineStorke = 4;
        double bkVInterval;
        int selectedScale = 60;
        void DrawLineInCanvas(double inputValue, double aPoint, Canvas inputCanva, LinearGradientBrush lineColor)
        {
            double startY = chartGrid.ActualHeight, startX = 0;
            if (inputCanva.Children.Count > 0)
            {
                var lastXY = inputCanva.Children.Cast<Line>().ToList().Last();
                startY = lastXY.Y2;
                startX = lastXY.X2;
            }
            Line newLine = new Line();
            newLine.Stroke = lineColor;
            newLine.StrokeThickness = lineStorke;
            newLine.StrokeStartLineCap = PenLineCap.Round;
            newLine.StrokeEndLineCap = PenLineCap.Round;
            newLine.X1 = startX;
            newLine.X2 = startX + bkVInterval;
            newLine.Y1 = startY;
            newLine.Y2 = chartGrid.ActualHeight - inputValue * aPoint;
            inputCanva.Children.Add(newLine);
            if (inputCanva.Children.Count > selectedScale)
            {
                NormoralnizeCanvas(selectedScale, inputCanva);
            }
        }
        /// <summary>
        /// The Normoralnize Canvas
        /// </summary>
        /// <param name="reCanvas"></param>
        private void NormoralnizeCanvas(int selectScale, Canvas reCanvas)
        {
            //set unused line collapsed
            var listLine = reCanvas.Children.Cast<UIElement>().ToList();
            var hiddenLine = listLine.GetRange(0, listLine.Count - selectScale);
            foreach (var ll in listLine)
            {
                (ll as Line).X1 -= bkVInterval;
                (ll as Line).X2 -= bkVInterval;
            }
            var behidden = hiddenLine.Where(x => x.Visibility != Visibility.Collapsed);

            foreach (var vv in behidden)
            {
                vv.Visibility = Visibility.Collapsed;
            }
            if (reCanvas.Children.Count == 601)
            {
                reCanvas.Children.RemoveAt(0);
            }
        }
    }
}
