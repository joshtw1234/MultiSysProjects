using AudioDemoModule.Interfaces;
using CommonUILib.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioDemoModule.ViewModels
{
    class AdvanceControlViewModel : BaseViewModel
    {
        private IAudioDemoControlModel _model;
        public IViewItem MessgeBox { get; set; }
        public AdvanceControlViewModel(IAudioDemoControlModel model)
        {
            _model = model;
            MessgeBox = _model.GetMessageBoxVM;
            _model.InitializeSystemHook();
        }
    }
}
