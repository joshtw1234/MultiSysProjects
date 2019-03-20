using AudioDemoModule.Interfaces;
using CommonUILib.Interfaces;

namespace AudioDemoModule.ViewModels
{
    class DebugControlViewModel : BaseViewModel
    {
        private IAudioDemoControlModel _model;
        public IViewItem MessgeBox { get; set; }
        public DebugControlViewModel(IAudioDemoControlModel model)
        {
            _model = model;
            MessgeBox = _model.GetMessageBoxVM;
            _model.InitializeSystemHook();
        }
    }
}
