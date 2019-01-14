using UWPAudioTest.Models;

namespace UWPAudioTest.ViewModels
{
    class AudioTestViewModel
    {

        private IAudioTestModel _model;
        public AudioTestViewModel(IAudioTestModel model)
        {
            _model = model;
        }

    }
}
