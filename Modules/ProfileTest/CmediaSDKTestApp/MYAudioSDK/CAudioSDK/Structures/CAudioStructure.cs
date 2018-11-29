using MYAudioSDK.CAudioSDK.Enums;

namespace MYAudioSDK.CAudioSDK.Structures
{
    class CAudioStructure
    {
        public CAudioJackDeviceInfo JackInfo { get; set; }
        public string ApiPropertyName { get; set; }
        public CAudioDriverReadWrite ReadWrite { get; set; }
        public byte[] WriteData { get; set; }
        public bool IsWriteExtra { get; set; }
        public byte[] WriteExtraData { get; set; }
    }
}
