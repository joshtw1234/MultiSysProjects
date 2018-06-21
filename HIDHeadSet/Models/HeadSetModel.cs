using HIDLib;

namespace HIDHeadSet.Models
{
    class HeadSetModel : IHeadSetModel
    {
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
                return false;
            }
            return true;
        }

        public bool OpenHID()
        {
            return hidDev.HIDOpenAsync();
        }

        public void ReadHID()
        {
            hidDev.HIDReadAsync();
        }

        public bool WriteHID(byte[] data)
        {
            return hidDev.HIDWriteAsync(data);
        }
    }
}
