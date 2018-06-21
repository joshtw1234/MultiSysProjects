using HIDLib;
using UtilityUILib;

namespace HIDHeadSet.Models
{
    class HeadSetModel : IHeadSetModel
    {
        /// <summary>
        /// Log file name
        /// </summary>
        const string LogHeadSet = @"Logs\HeadSet.log";

        const int PID = 0x8824;
        const int VID = 0x0D8C;
        HIDInfo hidDev;
        public void CloseHID()
        {
            hidDev.HIDClose();
        }

        public bool Initialize()
        {
            var hidLst = HIDAPIs.BrowseHID();
            hidDev = hidLst.Find(x => x.HIDInfoStruct.Pid == PID && x.HIDInfoStruct.Vid == VID);
            if (hidDev == null)
            {
                Utilities.Logger(LogHeadSet, $"{PID} {VID} not found!");
                return false;
            }
            return true;
        }

        public bool OpenHID()
        {
            bool rev = hidDev.HIDOpenAsync();
            if (!rev)
            {
                Utilities.Logger(LogHeadSet, $"hidDev Open Failed");
            }
            return rev;
        }

        public bool WriteHID(byte[] data)
        {
            bool rev = hidDev.HIDWriteAsync(data);
            if (!rev)
            {
                Utilities.Logger(LogHeadSet, $"WriteHID Failed");
            }
            return rev;
        }
    }
}
