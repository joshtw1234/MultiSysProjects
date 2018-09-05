using System.Collections;
using System.Windows;
using System.Windows.Controls;

namespace WPFMVVM.Views
{
    /// <summary>
    /// Interaction logic for CustomSlider.xaml
    /// </summary>
    public partial class CustomSliderControl : UserControl
    {
        #region Custom Property.

        /// <summary>
        /// The Item Source property.
        /// </summary>
        public double SliderMax
        {
            get { return (double)GetValue(SliderMaxProperty); }
            set { SetValue(SliderMaxProperty, value); }
        }

        /// <summary>
        /// Register Item Source property
        /// </summary>
        public static readonly DependencyProperty SliderMaxProperty =
            DependencyProperty.Register("SliderMax", typeof(double), typeof(CustomSliderControl), new PropertyMetadata());

        /// <summary>
        /// Register Item Source property
        /// </summary>
        public static readonly DependencyProperty SliderMinProperty =
            DependencyProperty.Register("SliderMin", typeof(double), typeof(CustomSliderControl), new PropertyMetadata());

        /// <summary>
        /// The Item Source property.
        /// </summary>
        public double SliderMin
        {
            get { return (double)GetValue(SliderMinProperty); }
            set { SetValue(SliderMinProperty, value); }
        }

        /// <summary>
        /// Register Item Source property
        /// </summary>
        public static readonly DependencyProperty SliderTickProperty =
            DependencyProperty.Register("SliderTick", typeof(double), typeof(CustomSliderControl), new PropertyMetadata());

        /// <summary>
        /// The Item Source property.
        /// </summary>
        public double SliderTick
        {
            get { return (double)GetValue(SliderTickProperty); }
            set { SetValue(SliderTickProperty, value); }
        }
#if false

        /// <summary>
        /// Register Item Source property
        /// </summary>
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(CustomSliderControl),
                new PropertyMetadata(null, new PropertyChangedCallback(OnItemsSourcePropertyChanged)));

        /// <summary>
        /// The Item Source property.
        /// </summary>
        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }
#endif
        #endregion

        public CustomSliderControl()
        {
            InitializeComponent();
        }
    }
}
