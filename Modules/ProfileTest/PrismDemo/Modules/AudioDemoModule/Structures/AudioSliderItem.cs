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
                _sliderValue = value;
                onPropertyChanged(this, "SliderValue");
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
                _audioValue = value;
                onPropertyChanged(this, "AudioValue");
            }
        }
    }
}
