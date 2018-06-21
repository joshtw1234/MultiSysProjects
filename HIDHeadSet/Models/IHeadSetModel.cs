using System.Collections.Generic;
using System.Windows.Media;

namespace HIDHeadSet.Models
{
    public interface IHeadSetModel
    {
        bool Initialize();
        bool OpenHID();
        void CloseHID();
        bool WriteHID(byte[] data);
        byte[] GetColorData(string ledMode, List<Brush> lstBrush);
    }

    enum FanModes
    {
        Off,
        Light,
        Medium,
        Heavy
    }

    class HeadSetConstants
    {
        /// <summary>
        /// Log file name
        /// </summary>
        public const string LogHeadSet = @"Logs\HeadSet.log";

        public const uint HeadSetPID = 0x8824;
        public const uint HeadSetVID = 0x0D8C;
        public const uint HeadSetBufferSize = 16;
        public const string LEDStatic = "Static";
        public const string LEDRepeatForward = "Repeat Forward";
        public const string LEDBackForth = "Back and Forth";
        public const string LEDLookupTable = "Lookup Table";
    }
}
