using AudioDemoModule.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioDemoModule.ViewModels
{
    class AudioDemoControlViewModel
    {
        IAudioDemoControlModel _model;
        public AudioDemoControlViewModel(IAudioDemoControlModel model)
        {
            _model = model;
        }
    }
}
