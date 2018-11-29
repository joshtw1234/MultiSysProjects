namespace MYAudioSDK.CAudioSDK.Enums
{
    /// <summary>
    /// The Cmedia SDK API function names
    /// Use string type to call SDK API
    /// </summary>
    enum CmediaAPIFunctionPoint
    {
        #region CMI Device
        /// <summary>
        /// Get or Set Default Device Control
        /// Get 0 is default
        /// Set 1 is default
        /// Render/Capture
        /// </summary>
        DefaultDeviceControl,
        AmplifierControl,
        EndpointEnableControl,//Get or Set device states, 1 is enable, 0 is disable
        EXControl,//EX switch device.
        GetSupportFeature,
        GetDeviceFriendlyName,
        GetDeviceID,
        GetExtraInfo,
        GetSymbolicLink,
        GetAudioCodecName,
        GetFirmwareVer,
        GetDriverVer,
        //GetDirectXVer,//This will took long time then others.
        #endregion

        #region Volume Control
        VolumeControl,
        MuteControl,
        VolumeScalarControl,//set -1 get master volume scalar,you can get other channel volume scalar
        GetMaxVol,//The unit of Value is dB. 
        GetMinVol,//The unit of Value is dB. 
        GetVolStep,
        #endregion

        #region EQ & EM
        Enable_EQ_GFX,
        EQ_Slider1,
        EQ_Slider2,
        EQ_Slider3,
        EQ_Slider4,
        EQ_Slider5,
        EQ_Slider6,
        EQ_Slider7,
        EQ_Slider8,
        EQ_Slider9,
        EQ_Slider10,
        Enable_RFX_GFX,
        RFX_ENVIRONMENT,
        RFX_ROOMSIZE,
        #endregion

        #region Mic features
        Enable_KEYSHIFT_GFX,
        KEYSHIFT_LEVEL,
        Enable_VOCALCANCEL_GFX,
        VOCALCANCEL_LEVEL,
        #endregion

        #region 7.1 surround
        VirtualSurroundEffectControl,
        #endregion

        #region Bass & HWEQ
        Enable_BM_GFX,
        BM_Slider_Frequency,
        BM_Slider_BassLevel,
        BM_LargeSpeaker,
        SpeakerDelay_C,
        SpeakerDelay_S,
        SpeakerDelay_B,
        HWEQBassControl,
        HWEQTrebleControl,
        #endregion

        #region Smart Bass & Volume
        Enable_ADAPTIVEVOLUME_GFX,
        ADAPTIVEVOLUME_LEVEL,
        ADAPTIVEVOLUME_MODE,
        Enable_VIRTUALBASS_LFX,
        VIRTUALBASS_Level,
        VIRTUALBASS_CutOffFrequency,
        VIRTUALBASS_Mode,
        Enable_AUDIOBRILLIANT_LFX,
        AUDIOBRILLIANT_LEVEL,
        Enable_VOICECLARITY_LFX,
        VOICECLARITY_LEVEL,
        VOICECLARITY_NOISESUPP_LEVEL,
        #endregion

        //Below API is Capture only
        #region MIC device.
        Enable_MICECHO,
        MICECHO_Level,
        Enable_MAGICVOICE,
        MagicVoice_Selection,
        #endregion

        #region AA Volume Control
        AAVolumeControl,
        AAMuteControl,
        AAVolumeScalarControl,
        GetAAMaxVol,
        GetAAMinVol,
        GetAAVolStep
        #endregion
    }
}
