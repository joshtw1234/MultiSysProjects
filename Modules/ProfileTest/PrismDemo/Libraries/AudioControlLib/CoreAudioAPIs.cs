using AudioControlLib.Enums;
using AudioControlLib.Structures;
using System;
using System.Runtime.InteropServices;

namespace AudioControlLib
{
    #region Core API Imports

    [ComImport]
    [Guid("A95664D2-9614-4F35-A746-DE8DB63617E6"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    interface IMMDeviceEnumerator
    {
        COMResult EnumAudioEndpoints(AudioDataFlow dataFlow, AudioDeviceState dwStateMask, out IMMDeviceCollection ppDevices);
        COMResult GetDefaultAudioEndpoint(AudioDataFlow dataFlow, EndPointRole role, out IMMDevice ppDevice);
        COMResult GetDevice(string id, out IMMDevice deviceName);

        COMResult RegisterEndpointNotificationCallback(IMMNotificationClient client);

        COMResult UnregisterEndpointNotificationCallback(IMMNotificationClient client);
    }

    [ComImport]
    [Guid("7991EEC9-7E89-4D85-8390-6C703CEC60C0"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    interface IMMNotificationClient
    {
        /// <summary>
        /// Device State Changed
        /// </summary>
        void OnDeviceStateChanged([MarshalAs(UnmanagedType.LPWStr)] string deviceId, [MarshalAs(UnmanagedType.I4)] AudioDeviceState newState);

        /// <summary>
        /// Device Added
        /// </summary>
        void OnDeviceAdded([MarshalAs(UnmanagedType.LPWStr)] string pwstrDeviceId);

        /// <summary>
        /// Device Removed
        /// </summary>
        void OnDeviceRemoved([MarshalAs(UnmanagedType.LPWStr)] string deviceId);

        /// <summary>
        /// Default Device Changed
        /// </summary>
        void OnDefaultDeviceChanged(AudioDataFlow flow, EndPointRole role, [MarshalAs(UnmanagedType.LPWStr)] string defaultDeviceId);

        /// <summary>
        /// Property Value Changed
        /// </summary>
        /// <param name="pwstrDeviceId"></param>
        /// <param name="key"></param>
        void OnPropertyValueChanged([MarshalAs(UnmanagedType.LPWStr)] string pwstrDeviceId, PropertyKey key);
    }

    [Guid("C8ADBD64-E71E-48a0-A4DE-185C395CD317"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown), ComImport]
    interface IAudioCaptureClient
    {
        COMResult GetBuffer(
            out IntPtr dataBuffer,
            out int numFramesToRead,
            out AudioClientBufferFlags bufferFlags,
            out long devicePosition,
            out long qpcPosition);

        COMResult ReleaseBuffer(int numFramesRead);

        COMResult GetNextPacketSize(out int numFramesInNextPacket);

    }

    [ComImport]
    [Guid("0BD7A1BE-7A1A-44DB-8397-CC5392387B5E"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    interface IMMDeviceCollection
    {
        COMResult GetCount(out int numDevices);
        COMResult Item(int deviceNumber, out IMMDevice device);
    }

    [Guid("D666063F-1587-4E43-81F1-B948E807363F"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    interface IMMDevice
    {
        COMResult Activate([MarshalAs(UnmanagedType.LPStruct)] Guid iid, ClsCtx dwClsCtx, IntPtr pActivationParams, [MarshalAs(UnmanagedType.IUnknown)] out object ppInterface);
        COMResult OpenPropertyStore(StorageAccessMode stgmAccess, out IPropertyStore properties);

        COMResult GetId([MarshalAs(UnmanagedType.LPWStr)] out string id);

        COMResult GetState(out AudioDeviceState state);
    }
    [Guid("886d8eeb-8cf2-4446-8d02-cdba1dbdcf99"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    interface IPropertyStore
    {
        COMResult GetCount(out int propCount);
        COMResult GetAt(int property, out PropertyKey key);
        COMResult GetValue(ref PropertyKey key, out PropVariant value);
        COMResult SetValue(ref PropertyKey key, ref PropVariant value);
        COMResult Commit();
    }
    [Guid("C02216F6-8C67-4B5B-9D00-D008E73E0064"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    interface IAudioMeterInformation
    {
        COMResult GetPeakValue(out float pfPeak);
        COMResult GetMeteringChannelCount(ref UInt32 pnChannelCount);
        COMResult GetChannelsPeakValues(UInt32 u32ChannelCount, ref float afPeakValues);
        COMResult QueryHardwareSupport(ref UInt32 pdwHardwareSupportMask);
    }
    [Guid("1CB9AD4C-DBFA-4c32-B178-C2F568A703B2"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    interface IAudioClient
    {
        COMResult Initialize(AudioClientMode ShareMode, AudioClientStreamFlags StreamFlags, Int64 hnsBufferDuration, Int64 hnsPeriodicity, WAVEFORMATEXTENSION pFormat, ref Guid AudioSessionGuid);
        COMResult GetBufferSize(ref UInt32 pNumBufferFrames);
        COMResult GetStreamLatency(ref Int64 phnsLatency);
        COMResult GetCurrentPadding(ref UInt32 pNumPaddingFrames);
        COMResult IsFormatSupported(int ShareMode, ref WAVEFORMATEXTENSION pFormat, out WAVEFORMATEXTENSION ppClosestMatch);
        COMResult GetMixFormat(out WAVEFORMATEXTENSION ppClosestMatch);
        COMResult GetDevicePeriod(ref Int64 phnsDefaultDevicePeriod, ref Int64 phnsMinimumDevicePeriod);
        COMResult Start();
        COMResult Stop();
        COMResult Reset();
        COMResult SetEventHandle(IntPtr eventHandle);
        //COMResult GetService(Guid riid, out object ppv);
        [PreserveSig]
        COMResult GetService([In, MarshalAs(UnmanagedType.LPStruct)] Guid interfaceId, [Out, MarshalAs(UnmanagedType.IUnknown)] out object interfacePointer);
    }

    #endregion
}
