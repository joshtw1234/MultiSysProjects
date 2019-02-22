using AudioControlLib.AudioMethods;
using AudioControlLib.CallBacks;
using AudioControlLib.Enums;
using AudioControlLib.Structures;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace AudioControlLib
{
    public class AudioControl
    {
        // REFERENCE_TIME time units per second and per millisecond
        private const int REFTIMES_PER_SEC = 10000000;
        private const int REFTIMES_PER_MILLISEC = 10000;
        private bool isAudioMonitoring = false;
        private bool isAudioPluseMonitor;
        #region Core API structures
        IMMDevice _audioDevice;
        IAudioClient _audioClient;
        IAudioCaptureClient _audioCaptureClient;
        IAudioMeterInformation _audioMeter;
        AudioDataFlow _audioDataFlow;
        MMNotificationClient _notifyClient;
        WAVEFORMATEXTENSION waveFormat;
        #endregion
        Guid _sessionGuid;
        AudioVolumeCallBack _audioCallBack;
        AudioPluseCallBack _audiopluseCallBack;

        public AudioControl(AudioDataFlow audioFlow)
        {
            _audioDataFlow = audioFlow;
            //Create Instance
            IMMDeviceEnumerator deviceEnumerator = MMDeviceEnumeratorFactory.CreateInstance();
            InitializeAudio(audioFlow, deviceEnumerator);
        }

        private void InitializeAudio(AudioDataFlow audioFlow, IMMDeviceEnumerator deviceEnumerator)
        {
            //Get Audio Device
            COMResult result = deviceEnumerator.GetDefaultAudioEndpoint(audioFlow, EndPointRole.eMultimedia, out _audioDevice);
            //Register End point notification
            _notifyClient = new MMNotificationClient();
            result = deviceEnumerator.RegisterEndpointNotificationCallback(_notifyClient);
            //Get Audio Client from device
            result = _audioDevice.Activate(typeof(IAudioClient).GUID, 0, IntPtr.Zero, out object obj);
            _audioClient = (IAudioClient)obj;
            //Get Audio Meter from device
            result = _audioDevice.Activate(typeof(IAudioMeterInformation).GUID, 0, IntPtr.Zero, out obj);
            _audioMeter = (IAudioMeterInformation)obj;
            //Initialize Audio Client.
            _sessionGuid = new Guid();
            result = _audioClient.GetMixFormat(out waveFormat);
            AudioClientStreamFlags streamFlag = AudioClientStreamFlags.None;
            if (audioFlow == AudioDataFlow.eRender) streamFlag = AudioClientStreamFlags.Loopback;
            result = _audioClient.Initialize(AudioClientMode.Shared, streamFlag, 10000000, 0, waveFormat, ref _sessionGuid);
            //Get Capture Client.
            result = _audioClient.GetService(typeof(IAudioCaptureClient).GUID, out obj);
            Marshal.ThrowExceptionForHR((int)result);
            _audioCaptureClient = (IAudioCaptureClient)obj;
            result = _audioClient.Start();
            //Change wave format here
            SetupWaveFormat(waveFormat);
        }

        private void SetupWaveFormat(WAVEFORMATEXTENSION _waveFormat)
        {
            switch (_waveFormat.WFormatTag)
            {
                case WaveFormat.Extensible:
                    _waveFormat.WBitsPerSample = 16;
                    _waveFormat.NBlockAlign = (ushort)(_waveFormat.NChannels * _waveFormat.WBitsPerSample / 8);
                    _waveFormat.NAvgBytesPerSec = _waveFormat.NBlockAlign * _waveFormat.NSamplesPerSec;
                    break;
            }
        }

        public void RegisterAudioVolumeCallBack(AudioVolumeCallBack callBack)
        {
            _audioCallBack += callBack;
        }
        public void RegisterAudioPluseCallBack(AudioPluseCallBack callBack)
        {
            _audiopluseCallBack += callBack;
        }

        public void StartMonitor()
        {
            if (!isAudioMonitoring)
            {
                isAudioMonitoring = true;
                Task.Factory.StartNew(()=>{ AudioMonitorWork(); });
            }
        }

        public void StopMonitor()
        {
            if (isAudioMonitoring)
            {
                isAudioMonitoring =false;
            }
        }

        public void StartAudioPluse()
        {
            if (!isAudioPluseMonitor)
            {
                isAudioPluseMonitor = true;
                Task.Factory.StartNew(() => { AudioPluseMonitorWork(); });
            }
        }

        

        public void StopAudioPluse()
        {
            if (isAudioPluseMonitor)
            {
                isAudioPluseMonitor = false;
            }
        }

        private void AudioPluseMonitorWork()
        {
            uint bufferCount = 0;
            int packageSize = -1;
            int numberFramToRead = -1;
            long devicePosition = 0;
            long qpcPosition = 0;
            AudioClientBufferFlags bufferFlag;
            IntPtr packageBuffer = IntPtr.Zero;
            var blockAlign = waveFormat.NBlockAlign;
            var result = _audioClient.GetBufferSize(ref bufferCount);
            // Calculate the actual duration of the allocated buffer.
            var hnsActualDuration = (double)REFTIMES_PER_SEC *
                             bufferCount / waveFormat.NSamplesPerSec;

            while (isAudioPluseMonitor)
            {
                Thread.Sleep((int)(hnsActualDuration / REFTIMES_PER_MILLISEC / 2));
                result = _audioCaptureClient.GetNextPacketSize(out packageSize);
                if (result == 0 && packageSize > 0)
                {
                    while (packageSize > 0)
                    {
                        _audioCaptureClient.GetBuffer(out packageBuffer, out numberFramToRead, out bufferFlag, out devicePosition, out qpcPosition);
                        if (0 == numberFramToRead) continue;
                        long lBytesToWrite = numberFramToRead * blockAlign;

                        if (AudioClientBufferFlags.Silent == bufferFlag)
                        {
                            _audiopluseCallBack(0, 0);
                        }
                        else
                        {
                            var pkgData = StructureToByteArray(packageBuffer);
                            ComputeDo3Band(pkgData, lBytesToWrite, waveFormat);
                        }
                        _audioCaptureClient.ReleaseBuffer(numberFramToRead);
                        result = _audioCaptureClient.GetNextPacketSize(out packageSize);
                    }
                }
                else
                {
                    _audiopluseCallBack(0, 0);
                }
            }
        }

        private void AudioMonitorWork()
        {
            while(isAudioMonitoring)
            {
                Thread.Sleep(100);
                _audioMeter.GetPeakValue(out float peak);
                _audioCallBack?.Invoke(_audioDataFlow, peak);
            }
            _audioCallBack?.Invoke(_audioDataFlow, 0);
        }

        private void ComputeDo3Band(byte[] pkgData, long lBytesToWrite, WAVEFORMATEXTENSION _waveFormat)
        {
            if (GetAudioData(pkgData, lBytesToWrite, _waveFormat))
            {
                EQSTATE eqs = new EQSTATE();
                AudioBandsConfig.init_3band_state(eqs, LowFrequence, HighFrequence, (int)_waveFormat.NSamplesPerSec);
                //for (int i = 0; i < m_nNumSamples; i++)
                for (int i = 0; i < m_RealIn_RT.Count; i++)
                {
                    m_Band_arr.Add(AudioBandsConfig.do_3band(eqs, m_RealIn_RT[i]));
                    m_BandLow_arr.Add(AudioBandsConfig.do_3bandLow(eqs, m_RealIn_LT[i]));
                }
                double A2, dSum = 0, A2Low, dSumLow = 0;
                for (int i = 0; i < m_Band_arr.Count; i++)
                {
                    A2 = m_Band_arr[i] * m_Band_arr[i];
                    dSum += A2;
                    A2Low = m_BandLow_arr[i] * m_BandLow_arr[i];
                    dSumLow += A2Low;
                }
                double dAvg = dSum / m_nNumSamples;
                double dAvgLow = dSumLow / m_nNumSamples;

                double dFinal = Math.Sqrt(dAvg);
                double dFinalLow = Math.Sqrt(dAvgLow);

                double dOutput = dFinal * Math.Sqrt(2) / 32768 * 100;
                double dOutputLow = dFinalLow * Math.Sqrt(2) / 32768 * 100;
                _audiopluseCallBack(dOutput, dOutputLow);
                
            }
        }
        List<float> m_RealIn_RT = new List<float>();
        List<float> m_RealIn_LT = new List<float>();
        List<double> m_Band_arr = new List<double>();
        List<double> m_BandLow_arr = new List<double>();
        //threadArgs.iHighFr = 5000;			// 16.May.30 --------------------------
        //threadArgs.iLowFr = 880;			// as suggested by David Chu ----------
        const int HighFrequence = 5000;
        const int LowFrequence = 880;
        int m_nNumSamples = -1;



        private bool GetAudioData(byte[] pkgData, long lBytesToWrite, WAVEFORMATEXTENSION waveFormat)
        {
            const double FFT_SPEED = 0.006;
            var m_nBufferSize = AudioBandsConfig.NextPowerOfTwo((int)(waveFormat.NAvgBytesPerSec * FFT_SPEED));
            m_nNumSamples = m_nBufferSize / waveFormat.NBlockAlign;
            //m_RealIn_RT.Clear();
            //m_RealIn_LT.Clear();
            //m_Band_arr.Clear();
            //m_BandLow_arr.Clear();
            switch (waveFormat.WBitsPerSample)
            {
                case 8:
                    break;
                case 16:
                    if (waveFormat.NChannels == 1) // mono
                    {
                        int Samples = m_nNumSamples >> 1;
                        for (int i = 0; i < Samples; ++i)
                        {
                            m_RealIn_RT[i] = (float)(pkgData[i]);
                            m_RealIn_LT[i] = m_RealIn_RT[i];
                        }
                        m_nNumSamples = Samples;
                    }
                    //else if (waveFormat.NChannels == 2) // stereo
                    else
                    {
                        // Stereo has Left+Right channels
                        int Samples = m_nNumSamples >> 2;
                        //for (int i = 0, j = 0; i < Samples; ++i, j += 2)
                        for (int i = 0, j = 0; i < pkgData.Length; ++i, j += 2)
                        {
                            if (j >= pkgData.Length) continue;
                            if (pkgData[j] == 0)
                            {
                                m_RealIn_RT.Add(0);
                                if (pkgData[j + 1] == 0)
                                    m_RealIn_LT.Add(0);
                                else if (pkgData[j + 1] != 0)
                                    m_RealIn_LT.Add((float)(pkgData[j + 1]));

                            }
                            else
                            {
                                m_RealIn_RT.Add((float)(pkgData[j]));
                                m_RealIn_LT.Add((float)(pkgData[j + 1]));
                            }
                            m_nNumSamples = Samples;
                        }

                    }
                    break;
            }
            return true;
        }

        byte[] StructureToByteArray<T>(T obj)
        {
            int length = Marshal.SizeOf(obj);
            byte[] array = new byte[length];

            IntPtr ptr = Marshal.AllocHGlobal(length);
            Marshal.StructureToPtr(obj, ptr, true);
            Marshal.Copy(ptr, array, 0, length);
            Marshal.FreeHGlobal(ptr);

            return array;
        }
    }
}
