using UtilityUILib;
using WPFMVVM.Models;

namespace WPFMVVM.ViewModels
{
    public class WPFMVVMControlViewModel : BindAbleBases
    {
        SliderDataContext _sliderCore;
        public SliderDataContext SliderCore
        {
            get
            {
                return _sliderCore;
            }
            set
            {
                _sliderCore = value;
                onPropertyChanged(this, "SliderCore");
            }
        }
        public WPFMVVMControlViewModel(IWPFMVVMModel _model)
        {
            _sliderCore = new SliderDataContext() { CoreMax = 100, CoreMin = 50, CoreTick = 10 };
        }

    }

    public class SliderDataContext
    {
        public double CoreMax { get; set; }
        public double CoreMin { get; set; }
        public double CoreTick { get; set; }
    }
}
