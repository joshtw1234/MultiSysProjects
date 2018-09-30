using UtilityUILib;
using WPFMVVM.Models;

namespace WPFMVVM.ViewModels
{
    public class WPFMVVMControlViewModel
    {
        public SliderDataContext TopSlider { get; set; }

        public SliderDataContext SecSlider { get; set; }

        public WPFMVVMControlViewModel(IWPFMVVMModel _model)
        {
            TopSlider = new SliderDataContext() { CoreMax = 100, CoreMin = 0, CoreTick = 20, CoreTitle = "Top Title", CoreValue = 100 };

            SecSlider = new SliderDataContext() { CoreMax = 100, CoreMin = 0, CoreTick = 20, CoreTitle = "Sec Title", CoreValue = 50 };
        }

    }

    
}
