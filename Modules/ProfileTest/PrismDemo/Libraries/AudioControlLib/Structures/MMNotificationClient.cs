using AudioControlLib.Enums;
using System.Runtime.InteropServices;

namespace AudioControlLib.Structures
{
    class MMNotificationClient : IMMNotificationClient
    {
        public delegate void NotificationClientCallBack(string deviceId, AudioDeviceState newState);

        NotificationClientCallBack _deviceStateCallBack;
        public void RegisterAudioDeviceStateChange(NotificationClientCallBack callback)
        {
            _deviceStateCallBack += callback;
        }
        public void UnRegisterAudioDeviceStateChange(NotificationClientCallBack callback)
        {
            _deviceStateCallBack -= callback;
        }
        #region IMMNotificationClient interface Do Not Call from outside.
        /// <summary>
        /// On Device State Changed, IMMNotificationClient interface Do Not Call from outside.
        /// </summary>
        /// <param name="deviceId"></param>
        /// <param name="newState"></param>
        public void OnDeviceStateChanged([MarshalAs(UnmanagedType.LPWStr)] string deviceId, [MarshalAs(UnmanagedType.I4)] AudioDeviceState newState)
        {
            //Plug and Unplug
        }

        public void OnDeviceAdded([MarshalAs(UnmanagedType.LPWStr)] string pwstrDeviceId)
        {
            //Do Nothing.
        }

        public void OnDeviceRemoved([MarshalAs(UnmanagedType.LPWStr)] string deviceId)
        {
            //Do Nothing.
        }

        public void OnDefaultDeviceChanged(AudioDataFlow flow, EndPointRole role, [MarshalAs(UnmanagedType.LPWStr)] string defaultDeviceId)
        {
            //Change default device in windows page.
        }

        public void OnPropertyValueChanged([MarshalAs(UnmanagedType.LPWStr)] string pwstrDeviceId, PropertyKey key)
        {
            //Right click on windows sound control
        }
        #endregion
    }
}
