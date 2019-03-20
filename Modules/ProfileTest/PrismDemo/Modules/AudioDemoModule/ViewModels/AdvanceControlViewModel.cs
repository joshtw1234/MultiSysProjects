﻿using AudioDemoModule.Interfaces;
using CommonUILib.Interfaces;

namespace AudioDemoModule.ViewModels
{
    class AdvanceControlViewModel : BaseViewModel
    {
        private IAudioDemoControlModel _model;
        public IViewItem MessgeBox { get; set; }
        public AdvanceControlViewModel(IAudioDemoControlModel model)
        {
            _model = model;
        }
    }
}
