// *****************************************************************************************************
// 
//   AudioDeviceChangeData.cs
// 
// 
//   Description:
//      The structure for Audio Device Change Data
//
// *****************************************************************************************************
namespace AudioControlLib.Structures
{
    /// <summary>
    /// The structure for Audio Device Change Data
    /// </summary>
    public struct AudioDeviceChangeData
    {
        /// <summary>
        /// Audio Device State
        /// </summary>
        public Enums.AudioDeviceState DeviceState;
        /// <summary>
        /// Audio End point Interface.
        /// </summary>
        public string PKEY_AudioEndPoint_Interface;
        /// <summary>
        /// Audio End point Name.
        /// </summary>
        public string PKEY_AudioEndpoint_Name;
        /// <summary>
        /// Audio End point Full Name.
        /// </summary>
        public string PKEY_audioendpoint_full_name;
        /// <summary>
        /// Audio End point HW ID.
        /// </summary>
        public string PKEY_AudioEndpoint_HWID;
        /// <summary>
        /// Audio End point Info.
        /// </summary>
        public string PKEY_AudioEndpoint_Info;
    }
}
