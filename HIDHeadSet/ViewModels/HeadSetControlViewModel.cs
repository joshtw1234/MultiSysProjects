using HIDHeadSet.Models;

namespace HIDHeadSet.ViewModels
{
    class HeadSetControlViewModel
    {
        private IHeadSetModel headSetModel;
        public HeadSetControlViewModel(IHeadSetModel _model)
        {
            headSetModel = _model;
        }
    }
}
