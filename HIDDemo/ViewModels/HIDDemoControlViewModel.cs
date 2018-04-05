using HIDDemo.Models;

namespace HIDDemo.ViewModels
{
    class HIDDemoControlViewModel
    {
        private IHIDDemoControlModel hidGUIModel;
        public HIDDemoControlViewModel(IHIDDemoControlModel _model)
        {
            hidGUIModel = _model;
        }
    }
}
