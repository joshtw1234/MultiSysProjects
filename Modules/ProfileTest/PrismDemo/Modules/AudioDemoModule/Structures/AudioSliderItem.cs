using Prism.Commands;
using System.Windows;

namespace AudioDemoModule.Structures
{
    class AudioSliderItem : BaseViewItem
    {
        public string MaxValue { get; set; }
        public string MinValue { get; set; }
        public string Step { get; set; }
        private string _sliderValue;
        public string SliderValue
        {
            get
            {
                return _sliderValue;
            }
            set
            {
                SetProperty(ref _sliderValue, value);
            }
        }
        public DelegateCommand<RoutedPropertyChangedEventArgs<double>> SliderValueChanged { get; set; }
    }

    class MonitorAudioSliderItem : AudioSliderItem
    {
        private string _audioValue;
        public string AudioValue
        {
            get
            {
                return _audioValue;
            }
            set
            {
                SetProperty(ref _audioValue, value);
            }
        }
    }
}
