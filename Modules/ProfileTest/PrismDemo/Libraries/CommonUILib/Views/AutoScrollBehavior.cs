using System;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace CommonUILib.Views
{
    public class AutoScrollBehavior : Behavior<ScrollViewer>
    {
        /// <summary>
        /// The scroll viewer class
        /// </summary>
        private ScrollViewer _scrollViewer = null;

        /// <summary>
        /// Scroll height
        /// </summary>
        private double _height = 0.0d;

        /// <summary>
        /// On Attached
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();

            this._scrollViewer = base.AssociatedObject;
            this._scrollViewer.LayoutUpdated += new EventHandler(_scrollViewer_LayoutUpdated);
        }


        /// <summary>
        /// On scroll viewer layout update.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _scrollViewer_LayoutUpdated(object sender, EventArgs e)
        {
            if (this._scrollViewer.ExtentHeight != _height)
            {
                this._scrollViewer.ScrollToVerticalOffset(this._scrollViewer.ExtentHeight);
                this._height = this._scrollViewer.ExtentHeight;
            }
        }

        /// <summary>
        /// On detaching.
        /// </summary>
        protected override void OnDetaching()
        {
            base.OnDetaching();

            if (this._scrollViewer != null)
            {
                this._scrollViewer.LayoutUpdated -= new EventHandler(_scrollViewer_LayoutUpdated);
            }
        }
    }
}
