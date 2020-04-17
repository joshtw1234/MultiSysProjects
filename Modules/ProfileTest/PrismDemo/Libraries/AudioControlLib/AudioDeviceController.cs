// *****************************************************************************************************
// 
//   AudioDeviceControler.cs
// 
// 
//   Description:
//      The Audio Device Control class
// 
//  
// *****************************************************************************************************
using AudioControlLib.Enums;
using AudioControlLib.Structures;
using System;

namespace AudioControlLib
{
    /// <summary>
    /// A Class for handle audio device and provide audio controls
    /// </summary>
    public class AudioDeviceController
    {
        /// <summary>
        /// Const PKEY for Audio.
        /// </summary>
        PropertyKey PKEY_AUDIOENDPOINT_INTERFACE = new PropertyKey(new Guid("a45c254e-df1c-4efd-8020-67d146a850e0"), 2);
        PropertyKey PKEY_AUDIOENDPOINT_INFO = new PropertyKey(new Guid("a45c254e-df1c-4efd-8020-67d146a850e0"), 24);
        PropertyKey PKEY_AUDIOENDPOINT_FULL_NAME = new PropertyKey(new Guid("a45c254e-df1c-4efd-8020-67d146a850e0"), 14);
        PropertyKey PKEY_AUDIOENDPOINT_HWID = new PropertyKey(new Guid("233164c8-1b2c-4c7d-bc68-b671687a2567"), 1);
        PropertyKey PKEY_AUDIOENDPOINT_NAME = new PropertyKey(new Guid("b3f8fa53-0004-438e-9003-51a46e139bfc"), 6);

        /// <summary>
        /// The Constructor
        /// </summary>
        public AudioDeviceController()
        {
            _deviceEnumerator = MMDeviceEnumeratorFactory.CreateInstance();
            _deviceNotification = new MMNotificationClient();
            _deviceEnumerator.RegisterEndpointNotificationCallback(_deviceNotification);
            _deviceNotification.RegisterAudioDeviceStateChange(OnAudioDeviceChange);
        }

        /// <summary>
        /// The Core Audio Implement.
        /// </summary>
        IMMDeviceEnumerator _deviceEnumerator;
        MMNotificationClient _deviceNotification;
        private AudioControl _speakerControl;
        private AudioControl _microphoneControl;
        /// <summary>
        /// The Audio Device State Change Call back
        /// </summary>
        private CallBacks.AudioDeviceStateChangeCallBack _deviceChangeCallBack;
        /// <summary>
        /// The Audio Device Change event and collect audio data to client
        /// </summary>
        /// <param name="deviceId"></param>
        /// <param name="newState"></param>
        private void OnAudioDeviceChange(string deviceId, Enums.AudioDeviceState newState)
        {
            System.Threading.Tasks.Task.Factory.StartNew(() =>
            {
                int propCnt = -1;
                AudioDeviceChangeData audioData = new AudioDeviceChangeData() { DeviceState = newState };
                PropertyKey proKey;
                PropVariant proVar;
                COMResult comRev;
                comRev = _deviceEnumerator.GetDevice(deviceId, out IMMDevice device);
                comRev = device.OpenPropertyStore(Enums.StorageAccessMode.STGM_READ, out IPropertyStore ppp);
                comRev = ppp.GetCount(out propCnt);
                for (int j = 0; j < propCnt; j++)
                {
                    try
                    {
                        //Get values
                        //TODO:use keys get you want data
                        comRev = ppp.GetAt(j, out proKey);
                        comRev = ppp.GetValue(ref proKey, out proVar);

                        if (proVar.DataType == System.Runtime.InteropServices.VarEnum.VT_LPWSTR ||
                            proVar.DataType == System.Runtime.InteropServices.VarEnum.VT_LPSTR)
                        {
                            if (proKey.formatId.ToString().Equals(PKEY_AUDIOENDPOINT_INTERFACE.formatId.ToString()) &&
                                proKey.propertyId == PKEY_AUDIOENDPOINT_INTERFACE.propertyId)
                            {
                                audioData.PKEY_AudioEndPoint_Interface = proVar.Value.ToString();
                            }
                            if (proKey.formatId.ToString().Equals(PKEY_AUDIOENDPOINT_NAME.formatId.ToString()) &&
                               proKey.propertyId == PKEY_AUDIOENDPOINT_NAME.propertyId)
                            {
                                audioData.PKEY_AudioEndpoint_Name = proVar.Value.ToString();
                            }
                            if (proKey.formatId.ToString().Equals(PKEY_AUDIOENDPOINT_HWID.formatId.ToString()) &&
                               proKey.propertyId == PKEY_AUDIOENDPOINT_HWID.propertyId)
                            {
                                audioData.PKEY_AudioEndpoint_HWID = proVar.Value.ToString();
                            }
                            if (proKey.formatId.ToString().Equals(PKEY_AUDIOENDPOINT_INFO.formatId.ToString()) &&
                               proKey.propertyId == PKEY_AUDIOENDPOINT_INFO.propertyId)
                            {
                                audioData.PKEY_AudioEndpoint_Info = proVar.Value.ToString();
                            }
                            if (proKey.formatId.ToString().Equals(PKEY_AUDIOENDPOINT_FULL_NAME.formatId.ToString()) &&
                               proKey.propertyId == PKEY_AUDIOENDPOINT_FULL_NAME.propertyId)
                            {
                                audioData.PKEY_audioendpoint_full_name = proVar.Value.ToString();
                                _deviceChangeCallBack?.Invoke(audioData);
                                break;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        //Some reason Microphone will occur exception.
                    }
                }
            });
        }

        /// <summary>
        /// The register audio device state call back
        /// </summary>
        /// <param name="callBack"></param>
        public void RegisterAudioDeviceStateCallBack(CallBacks.AudioDeviceStateChangeCallBack callBack)
        {
            _deviceChangeCallBack += callBack;
        }
        /// <summary>
        /// The UnRegister Audio Device state call back
        /// </summary>
        /// <param name="callBack"></param>
        public void UnRegisterAudioDeviceStateCallBack(CallBacks.AudioDeviceStateChangeCallBack callBack)
        {
            _deviceChangeCallBack -= callBack;
        }
        #region Audio Control
        /// <summary>
        /// Get HID Audio Device UUID
        /// </summary>
        /// <param name="dataFlow"></param>
        /// <param name="pid"></param>
        /// <param name="vid"></param>
        /// <returns></returns>
        private string GetHIDAudioDeviceUUID(AudioDataFlow dataFlow, string pid, string vid)
        {
            COMResult result = COMResult.E_FAIL;
           
            PropertyKey proKey;
            PropVariant proVar;
            result = _deviceEnumerator.EnumAudioEndpoints(dataFlow, AudioDeviceState.DEVICE_STATE_ACTIVE, out IMMDeviceCollection devCollect);
            result = devCollect.GetCount(out int devCnt);
            for (int i = 0; i < devCnt; i++)
            {
                result = devCollect.Item(i, out IMMDevice tDev);
                tDev.GetId(out string devID);
                tDev.OpenPropertyStore(StorageAccessMode.STGM_READ, out IPropertyStore ppt);
                ppt.GetCount(out int ppCnt);
                for (int j = 0; j < ppCnt; j++)
                {
                    result = ppt.GetAt(j, out proKey);
                    result = ppt.GetValue(ref proKey, out proVar);
                    if (proVar.DataType == System.Runtime.InteropServices.VarEnum.VT_LPWSTR ||
                          proVar.DataType == System.Runtime.InteropServices.VarEnum.VT_LPSTR)
                    {
                        if (proKey.formatId.ToString().Equals(PKEY_AUDIOENDPOINT_HWID.formatId.ToString()) &&
                               proKey.propertyId == PKEY_AUDIOENDPOINT_HWID.propertyId)
                        {
                            string hwID = proVar.Value.ToString();
                            if (!GetPIDVIDFromHWIDString(hwID, out string hwPid, out string hwVid))
                            {
                                continue;
                            }
                            if (vid.Equals(hwVid, StringComparison.CurrentCultureIgnoreCase) && 
                                pid.Equals(hwPid, StringComparison.CurrentCultureIgnoreCase))
                            {
                                return devID;
                            }
                        }
                    }
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// Get PID VID from HW ID string
        /// </summary>
        /// <param name="hwID">HW ID</param>
        /// <param name="hwPid">out Pid</param>
        /// <param name="hwVid">out Vid</param>
        /// <returns>true for success</returns>
        private bool GetPIDVIDFromHWIDString(string hwID, out string hwPid, out string hwVid)
        {
            /*
            * update pattern to follow standard USB format
            * https://docs.microsoft.com/en-us/windows-hardware/drivers/install/standard-usb-identifiers
            * PCI device format
            * https://docs.microsoft.com/zh-tw/windows-hardware/drivers/install/identifiers-for-pci-devices
            */
            const string hidPattern = @"([\w]{3})_([\w]{4})&";
            const string hwPidString = "pid";
            const string hwVidString = "vid";
            bool isGetPIDVID = false;
            hwPid = string.Empty;
            hwVid = string.Empty;
            MatchCollection mt = Regex.Matches(hwID, hidPattern, RegexOptions.IgnoreCase);
            var matchPid = mt.Cast<Match>().FirstOrDefault(x => x.Groups[1].Value.Equals(hwPidString, StringComparison.CurrentCultureIgnoreCase));
            var matchVid = mt.Cast<Match>().FirstOrDefault(x => x.Groups[1].Value.Equals(hwVidString, StringComparison.CurrentCultureIgnoreCase));
            if (null == matchPid || null == matchVid)
            {
                return isGetPIDVID;
            }
            isGetPIDVID = true;
            hwPid = matchPid.Groups[2].Value;
            hwVid = matchVid.Groups[2].Value;
            return isGetPIDVID;
        }

        /// <summary>
        /// Get IMMDevice
        /// </summary>
        /// <param name="initFlow"></param>
        /// <param name="pid"></param>
        /// <param name="vid"></param>
        /// <returns>Null means Failed</returns>
        private IMMDevice GetIMMDevice(AudioDataFlow initFlow, string pid, string vid)
        {
            COMResult result = COMResult.E_FAIL;
            IMMDevice _device = null;
            if (initFlow == AudioDataFlow.eAll)
            {
                //Not support get both render and capture.
                return _device;
            }
            string devUUID = GetHIDAudioDeviceUUID(initFlow, pid, vid);
            if (string.IsNullOrEmpty(devUUID))
            {
                //Failed to get UUID, Get Default Endpoint
                result = _deviceEnumerator.GetDefaultAudioEndpoint(initFlow, EndPointRole.eCommunications, out _device);
            }
            else
            {
                result = _deviceEnumerator.GetDevice(devUUID, out _device);
            }
            return _device;
        }
        /// <summary>
        /// The Initialize Audio Controls
        /// </summary>
        public void InitializeAudioControls(AudioDataFlow initFlow, string pid, string vid)
        {
            IMMDevice _audioDevice = null;
            //Get Audio Device
            switch (initFlow)
            {
                case AudioDataFlow.eAll:
                    _audioDevice = GetIMMDevice(AudioDataFlow.eRender, pid, vid);
                    _speakerControl = new AudioControl(_audioDevice, AudioDataFlow.eRender);
                    _audioDevice = GetIMMDevice(AudioDataFlow.eCapture, pid, vid);
                    _microphoneControl = new AudioControl(_audioDevice, AudioDataFlow.eCapture);
                    break;
                case AudioDataFlow.eRender:
                    _audioDevice = GetIMMDevice(initFlow, pid, vid);
                    _speakerControl = new AudioControl(_audioDevice, initFlow);
                    break;
                case AudioDataFlow.eCapture:
                    _audioDevice = GetIMMDevice(initFlow, pid, vid);
                    _microphoneControl = new AudioControl(_audioDevice, initFlow);
                    break;
            }
        }
        /// <summary>
        /// The UnIntialize Audio Controls
        /// </summary>
        public void UnInitializeAudioControls()
        {
            if (null != _speakerControl)
            {
                _speakerControl.UninitializeAudio();
                _speakerControl = null;
            }
            if (null != _microphoneControl)
            {
                _microphoneControl.UninitializeAudio();
                _microphoneControl = null;
            }
        }
        /// <summary>
        /// The Get Speaker Muted
        /// </summary>
        /// <returns></returns>
        public bool GetSpeakerIsMuted()
        {
            if (null == _speakerControl) return false;

            return _speakerControl.GetMuted();
        }
        /// <summary>
        /// The Set Speaker Muted
        /// </summary>
        /// <param name="v"></param>
        public void SetSpeakerMute(bool v)
        {
            if (null != _speakerControl) _speakerControl.SetMuted(v);
        }
        /// <summary>
        /// Get Speaker Volume
        /// </summary>
        /// <returns></returns>
        public double GetSpeakerVolumeValue()
        {
            if (null == _speakerControl) return 0.0;

            return _speakerControl.GetMasterVolume();
        }
        /// <summary>
        /// Set Speaker Volume
        /// </summary>
        /// <param name="newValue"></param>
        public void SetSpeakerVolumeValue(double newValue)
        {
           if (null != _speakerControl) _speakerControl.SetMasterVolumeScalar(newValue);
        }
        /// <summary>
        /// Get Speaker Channel Value
        /// </summary>
        /// <param name="channelName"></param>
        /// <returns></returns>
        public float GetSpeakerChannelValue(uint channelName)
        {
            if (null == _speakerControl) return 0;

            return _speakerControl.GetChannelValue(channelName);
        }
        /// <summary>
        /// Set Speaker Channel value
        /// </summary>
        /// <param name="channelName"></param>
        /// <param name="v"></param>
        public void SetSpeakerChannelValue(uint channelName, double v)
        {
            if (null != _speakerControl) _speakerControl.SetChannelValue(channelName, (float)v);
        }
        /// <summary>
        /// Get Microphone Muted
        /// </summary>
        /// <returns></returns>
        public bool GetMicrophoneIsMuted()
        {
            if (null == _microphoneControl) return false;

            return _microphoneControl.GetMuted();
        }
        /// <summary>
        /// Set Microphone Muted
        /// </summary>
        /// <param name="v"></param>
        public void SetMicrophoneMute(bool v)
        {
            if (null != _microphoneControl) _microphoneControl.SetMuted(v);
        }
        /// <summary>
        /// Get Microphone Volume
        /// </summary>
        /// <returns></returns>
        public double GetMicrophoneVolumeValue()
        {
            if (null == _microphoneControl) return 0;

            return _microphoneControl.GetMasterVolume();
        }
        /// <summary>
        /// Set Microphone Volume
        /// </summary>
        /// <param name="newValue"></param>
        public void SetMicrophoneVolumeValue(double newValue)
        {
            if (null != _microphoneControl) _microphoneControl.SetMasterVolumeScalar(newValue);
        }
        /// <summary>
        /// Get Microphone Channel Value
        /// </summary>
        /// <param name="channelName"></param>
        /// <returns></returns>
        public float GetMicrophoneChannelValue(uint channelName)
        {
            if (null == _microphoneControl) return 0;

            return _microphoneControl.GetChannelValue(channelName);
        }
        /// <summary>
        /// Set Microphone Channel Value
        /// </summary>
        /// <param name="channelName"></param>
        /// <param name="v"></param>
        public void SetMicrophoneChannelValue(uint channelName, double v)
        {
            if (null != _microphoneControl) _microphoneControl.SetChannelValue(channelName, (float)v);
        }

        #region Register Audio Control CallBacks
        /// <summary>
        /// Register Speaker Volume Call back
        /// </summary>
        /// <param name="callBack"></param>
        public void RegisterSpeakerVolumeCallBack(CallBacks.AudioVolumeChangeCallBack callBack)
        {
            if (null == _speakerControl) return;

            _speakerControl.RegisterVolumeChangeCallBack(callBack);
        }
        /// <summary>
        /// UnRegister Speaker Volume Call back
        /// </summary>
        /// <param name="callBack"></param>
        public void UnRegisterSpeakerVolumeCallBack(CallBacks.AudioVolumeChangeCallBack callBack)
        {
            if (null == _speakerControl) return;
            _speakerControl.UnRegisterVolumeChangeCallBack(callBack);
        }
        /// <summary>
        /// Register Microphone Volume Call back
        /// </summary>
        /// <param name="callBack"></param>
        public void RegisterMicrophoneVolumeCallBack(CallBacks.AudioVolumeChangeCallBack callBack)
        {
            if (null == _microphoneControl) return;

            _microphoneControl.RegisterVolumeChangeCallBack(callBack);
        }
        /// <summary>
        /// UnRegister Microphone Volume Call back
        /// </summary>
        /// <param name="callBack"></param>
        public void UnRegisterMicrophoneVolumeCallBack(CallBacks.AudioVolumeChangeCallBack callBack)
        {
            if (null == _microphoneControl) return;
            _microphoneControl.UnRegisterVolumeChangeCallBack(callBack);
        }
        /// <summary>
        /// Register Speaker Peek meter Call back
        /// </summary>
        /// <param name="callBack"></param>
        public void RegisterSpeakerPeekMeterCallBack(CallBacks.AudioVolumePeekCallBack callBack)
        {
            if (null == _speakerControl) return;
            _speakerControl.StartPeekMonitor();
            _speakerControl.RegisterAudioVolumePeekCallBack(callBack);
        }
        /// <summary>
        /// UnRegister Speaker Peek meter Call back
        /// </summary>
        /// <param name="callBack"></param>
        public void UnRegisterSpeakerPeekMeterCallBack(CallBacks.AudioVolumePeekCallBack callBack)
        {
            if (null == _speakerControl) return;
            _speakerControl.StopPeekMonitor();
            _speakerControl.UnRegisterAudioVolumePeekCallBack(callBack);
        }
        /// <summary>
        /// Register Microphone Peek Meter Call back
        /// </summary>
        /// <param name="callBack"></param>
        public void RegisterMicrophonePeekMeterCallBack(CallBacks.AudioVolumePeekCallBack callBack)
        {
            if (null == _microphoneControl) return;

            _microphoneControl.StartPeekMonitor();
            _microphoneControl.RegisterAudioVolumePeekCallBack(callBack);
        }
        /// <summary>
        /// UnRegister Microphone Peek meter Call back
        /// </summary>
        /// <param name="callBack"></param>
        public void UnRegisterMicrophonePeekMeterCallBack(CallBacks.AudioVolumePeekCallBack callBack)
        {
            if (null == _microphoneControl) return;
            _microphoneControl.StopPeekMonitor();
            _microphoneControl.UnRegisterAudioVolumePeekCallBack(callBack);
        }
        #endregion
        #endregion
    }
}
