namespace MYAudioSDK.CAudioSDK.Structures
{
    class CAudioJackDeviceInfo
    {
        public CAudioDeviceInfo m_devInfo;       // reference to DEVICEINFO
        // function attributes
        public int m_dwCMediaDSP0 { get; set; }      // CMedia DSP function tables
        public int m_dwThirdPartyDSP0 { get; set; }    // Third-Party DSP function tables
        public int m_dwExtraStreamFunc { get; set; }   // Extra stream function tables
    }
}
