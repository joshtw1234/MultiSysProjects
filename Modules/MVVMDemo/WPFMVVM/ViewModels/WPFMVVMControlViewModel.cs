using UtilityUILib;
using WPFMVVM.Models;

namespace WPFMVVM.ViewModels
{
    public class WPFMVVMControlViewModel
    {
        //SliderDataContext _sliderCore;
        //public SliderDataContext SliderCore
        //{
        //    get
        //    {
        //        return _sliderCore;
        //    }
        //    set
        //    {
        //        _sliderCore = value;
        //        onPropertyChanged(this, "SliderCore");
        //    }
        //}
        public WPFMVVMControlViewModel(IWPFMVVMModel _model)
        {
            //_sliderCore = new SliderDataContext() { CoreMax = 100, CoreMin = 0, CoreTick = 20, CoreTitle = "Custom Title", CoreValue = 100 };
        }

    }

    //public class SliderDataContext : WPFCommonLib.ViewModels.ICustomSliderViewModel
    //{
    //    public double CoreMax { get ; set ; }
    //    public double CoreMin { get ; set ; }
    //    public double CoreTick { get ; set ; }
    //    public string CoreTitle { get ; set ; }
    //    public double CoreValue { get ; set ; }
    //}
}
