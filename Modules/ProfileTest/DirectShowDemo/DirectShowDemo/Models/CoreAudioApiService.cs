using AudioDeviceUtil;

namespace DirectShowDemo.Models
{
    class CoreAudioApiService
    {
        private static CoreAudioApiService _instance;
        /// <summary>
        /// Gets the instance.
        /// </summary>
        public static CoreAudioApiService Instance
        {
            get
            {
                return _instance ?? (_instance = new CoreAudioApiService());
            }
        }
        /// <summary>
        /// Codeproject sample 
        /// https://www.codeproject.com/Articles/829712/WebControls/
        /// </summary>
        AudioDeviceManager audioDeviceSwitcher { get; set; }
        public void InitializeAudioDevice()
        {
            audioDeviceSwitcher = new AudioDeviceManager();
        }
    }
}
