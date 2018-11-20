using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWPAudioTest.Models;

namespace UWPAudioTest.ViewModels
{
    public class AudioTestViewModel
    {

        private IAudioTestModel _model;
        public AudioTestViewModel(IAudioTestModel model)
        {
            _model = model;
        }

    }
}
