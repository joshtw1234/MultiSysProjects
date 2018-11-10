using CmediaSDKTestApp.BaseModels;
using System.Windows;

namespace CmediaSDKTestApp.Models
{
    class HorzSliderControlModel
    {
        public string SliderName { get; set; }
        public IMenuItem Btn_Left { get; set; }
        public IMenuItem SliderValueStr { get; set; }
        public IMenuItem SlideUnitStr { get; set; }
        public IMenuItem Btn_Right { get; set; }
        public IMenuItem SliderTitle { get; set; }
        public IMenuItem SliderTickFrequency { get; set; }
        public IMenuItem SliderMinimum { get; set; }
        public IMenuItem SliderMaximum { get; set; }
        public MyDelegateCommond<RoutedPropertyChangedEventArgs<double>> SliderValueChangeCommand { get; set; }
        public MyDelegateCommond<RoutedEventArgs> MuteBoxCheckedCommand { get; set; }
    }
}
