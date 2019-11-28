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
   
    [Guid("657804FA-D6AD-4496-8A60-352752AF4F89"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    interface IAudioEndpointVolumeCallback
    {
        void OnNotify([In] IntPtr pNotify);
    }

    [Guid("5CDF2C82-841E-4546-9722-0CF74078229A"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    interface IAudioEndpointVolume
    {
        [PreserveSig]
        COMResult RegisterControlChangeNotify([In] [MarshalAs(UnmanagedType.Interface)] IAudioEndpointVolumeCallback pNotify);

        [PreserveSig]
        COMResult UnregisterControlChangeNotify([In] [MarshalAs(UnmanagedType.Interface)]IAudioEndpointVolumeCallback pNotify);

        /// <summary>
        /// Gets a count of the channels in the audio stream.
        /// </summary>
        /// <param name="channelCount">The number of channels.</param>
        /// <returns>An HRESULT code indicating whether the operation passed of failed.</returns>
        [PreserveSig]
        COMResult GetChannelCount(
            [Out] [MarshalAs(UnmanagedType.U4)] out UInt32 channelCount);

        /// <summary>
        /// Sets the master volume level of the audio stream, in decibels.
        /// </summary>
        /// <param name="level">The new master volume level in decibels.</param>
        /// <param name="eventContext">A user context value that is passed to the notification callback.</param>
        /// <returns>An HRESULT code indicating whether the operation passed of failed.</returns>
        [PreserveSig]
        COMResult SetMasterVolumeLevel(
            [In] [MarshalAs(UnmanagedType.R4)] float level,
            [In] [MarshalAs(UnmanagedType.LPStruct)] Guid eventContext);

        /// <summary>
        /// Sets the master volume level, expressed as a normalized, audio-tapered value.
        /// </summary>
        /// <param name="level">The new master volume level expressed as a normalized value between 0.0 and 1.0.</param>
        /// <param name="eventContext">A user context value that is passed to the notification callback.</param>
        /// <returns>An HRESULT code indicating whether the operation passed of failed.</returns>
        [PreserveSig]
        COMResult SetMasterVolumeLevelScalar(
            [In] [MarshalAs(UnmanagedType.R4)] float level,
            [In] [MarshalAs(UnmanagedType.LPStruct)] Guid eventContext);

        /// <summary>
        /// Gets the master volume level of the audio stream, in decibels.
        /// </summary>
        /// <param name="level">The volume level in decibels.</param>
        /// <returns>An HRESULT code indicating whether the operation passed of failed.</returns>
        [PreserveSig]
        COMResult GetMasterVolumeLevel(
            [Out] [MarshalAs(UnmanagedType.R4)] out float level);

        /// <summary>
        /// Gets the master volume level, expressed as a normalized, audio-tapered value.
        /// </summary>
        /// <param name="level">The volume level expressed as a normalized value between 0.0 and 1.0.</param>
        /// <returns>An HRESULT code indicating whether the operation passed of failed.</returns>
        [PreserveSig]
        COMResult GetMasterVolumeLevelScalar(
            [Out] [MarshalAs(UnmanagedType.R4)] out float level);

        /// <summary>
        /// Sets the volume level, in decibels, of the specified channel of the audio stream.
        /// </summary>
        /// <param name="channelNumber">The channel number.</param>
        /// <param name="level">The new volume level in decibels.</param>
        /// <param name="eventContext">A user context value that is passed to the notification callback.</param>
        /// <returns>An HRESULT code indicating whether the operation passed of failed.</returns>
        [PreserveSig]
        COMResult SetChannelVolumeLevel(
            [In] [MarshalAs(UnmanagedType.U4)] UInt32 channelNumber,
            [In] [MarshalAs(UnmanagedType.R4)] float level,
            [In] [MarshalAs(UnmanagedType.LPStruct)] Guid eventContext);

        /// <summary>
        /// Sets the normalized, audio-tapered volume level of the specified channel in the audio stream.
        /// </summary>
        /// <param name="channelNumber">The channel number.</param>
        /// <param name="level">The new master volume level expressed as a normalized value between 0.0 and 1.0.</param>
        /// <param name="eventContext">A user context value that is passed to the notification callback.</param>
        /// <returns>An HRESULT code indicating whether the operation passed of failed.</returns>
        [PreserveSig]
        COMResult SetChannelVolumeLevelScalar(
            [In] [MarshalAs(UnmanagedType.U4)] UInt32 channelNumber,
            [In] [MarshalAs(UnmanagedType.R4)] float level,
            [In] [MarshalAs(UnmanagedType.LPStruct)] Guid eventContext);

        /// <summary>
        /// Gets the volume level, in decibels, of the specified channel in the audio stream.
        /// </summary>
        /// <param name="channelNumber">The zero-based channel number.</param>
		/// <param name="level">The volume level in decibels.</param>
        /// <returns>An HRESULT code indicating whether the operation passed of failed.</returns>
        [PreserveSig]
        COMResult GetChannelVolumeLevel(
            [In] [MarshalAs(UnmanagedType.U4)] UInt32 channelNumber,
            [Out] [MarshalAs(UnmanagedType.R4)] out float level);

        /// <summary>
        /// Gets the normalized, audio-tapered volume level of the specified channel of the audio stream.
        /// </summary>
        /// <param name="channelNumber">The zero-based channel number.</param>
		/// <param name="level">The volume level expressed as a normalized value between 0.0 and 1.0.</param>
        /// <returns>An HRESULT code indicating whether the operation passed of failed.</returns>
        [PreserveSig]
        COMResult GetChannelVolumeLevelScalar(
            [In] [MarshalAs(UnmanagedType.U4)] UInt32 channelNumber,
            [Out] [MarshalAs(UnmanagedType.R4)] out float level);

        /// <summary>
        /// Sets the muting state of the audio stream.
        /// </summary>
        /// <param name="isMuted">True to mute the stream, or false to unmute the stream.</param>
        /// <param name="eventContext">A user context value that is passed to the notification callback.</param>
        /// <returns>An HRESULT code indicating whether the operation passed of failed.</returns>
        [PreserveSig]
        COMResult SetMute(
            [In] [MarshalAs(UnmanagedType.Bool)] Boolean isMuted,
            [In] [MarshalAs(UnmanagedType.LPStruct)] Guid eventContext);

        /// <summary>
        /// Gets the muting state of the audio stream.
        /// </summary>
        /// <param name="isMuted">The muting state. True if the stream is muted, false otherwise.</param>
        /// <returns>An HRESULT code indicating whether the operation passed of failed.</returns>
        [PreserveSig]
        COMResult GetMute(
            [Out] [MarshalAs(UnmanagedType.Bool)] out Boolean isMuted);

        /// <summary>
        /// Gets information about the current step in the volume range.
        /// </summary>
        /// <param name="step">The current zero-based step index.</param>
        /// <param name="stepCount">The total number of steps in the volume range.</param>
        /// <returns>An HRESULT code indicating whether the operation passed of failed.</returns>
        [PreserveSig]
        COMResult GetVolumeStepInfo(
            [Out] [MarshalAs(UnmanagedType.U4)] out UInt32 step,
            [Out] [MarshalAs(UnmanagedType.U4)] out UInt32 stepCount);

        /// <summary>
        /// Increases the volume level by one step.
        /// </summary>
        /// <param name="eventContext">A user context value that is passed to the notification callback.</param>
        /// <returns>An HRESULT code indicating whether the operation passed of failed.</returns>
        [PreserveSig]
        COMResult VolumeStepUp(
            [In] [MarshalAs(UnmanagedType.LPStruct)] Guid eventContext);

        /// <summary>
        /// Decreases the volume level by one step.
        /// </summary>
        /// <param name="eventContext">A user context value that is passed to the notification callback.</param>
        /// <returns>An HRESULT code indicating whether the operation passed of failed.</returns>
        [PreserveSig]
        COMResult VolumeStepDown(
            [In] [MarshalAs(UnmanagedType.LPStruct)] Guid eventContext);

        /// <summary>
        /// Queries the audio endpoint device for its hardware-supported functions.
        /// </summary>
        /// <param name="hardwareSupportMask">A hardware support mask that indicates the capabilities of the endpoint.</param>
        /// <returns>An HRESULT code indicating whether the operation passed of failed.</returns>
        [PreserveSig]
        COMResult QueryHardwareSupport(
            [Out] [MarshalAs(UnmanagedType.U4)] out UInt32 hardwareSupportMask);

        /// <summary>
        /// Gets the volume range of the audio stream, in decibels.
        /// </summary>
		/// <param name="volumeMin">The minimum volume level in decibels.</param>
		/// <param name="volumeMax">The maximum volume level in decibels.</param>
		/// <param name="volumeStep">The volume increment level in decibels.</param>
        /// <returns>An HRESULT code indicating whether the operation passed of failed.</returns>
        [PreserveSig]
        COMResult GetVolumeRange(
            [Out] [MarshalAs(UnmanagedType.R4)] out float volumeMin,
            [Out] [MarshalAs(UnmanagedType.R4)] out float volumeMax,
            [Out] [MarshalAs(UnmanagedType.R4)] out float volumeStep);
    }

    #endregion
}
