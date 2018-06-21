using System.Collections.Generic;
using System.Windows.Media;
using HIDLib;
using UtilityUILib;

namespace HIDHeadSet.Models
{
    class HeadSetModel : IHeadSetModel
    {
        HIDInfo hidDev;
        public void CloseHID()
        {
            hidDev.HIDClose();
        }

        public byte[] GetColorData(string ledMode, List<Brush> lstBrush)
        {
            byte[] revData = new byte[HeadSetConstants.HeadSetBufferSize];
            if (ledMode.Equals(HeadSetConstants.LEDStatic))
            {

            }
            if (ledMode.Equals(HeadSetConstants.LEDRepeatForward))
            {

            }
            if (ledMode.Equals(HeadSetConstants.LEDBackForth))
            {

            }
            if (ledMode.Equals(HeadSetConstants.LEDLookupTable))
            {

            }
            return revData;
        }

        public bool Initialize()
        {
            var hidLst = HIDAPIs.BrowseHID();
            hidDev = hidLst.Find(x => x.HIDInfoStruct.Pid == HeadSetConstants.HeadSetPID && x.HIDInfoStruct.Vid == HeadSetConstants.HeadSetVID);
            if (hidDev == null)
            {
                Utilities.Logger(HeadSetConstants.LogHeadSet, $"{HeadSetConstants.HeadSetPID} {HeadSetConstants.HeadSetVID} not found!");
                return false;
            }
            return true;
        }

        public bool OpenHID()
        {
            bool rev = hidDev.HIDOpenAsync();
            if (!rev)
            {
                Utilities.Logger(HeadSetConstants.LogHeadSet, $"hidDev Open Failed");
            }
            return rev;
        }

        public bool WriteHID(byte[] data)
        {
            bool rev = hidDev.HIDWriteAsync(data);
            if (!rev)
            {
                Utilities.Logger(HeadSetConstants.LogHeadSet, $"WriteHID Failed");
            }
            return rev;
        }
    }
}
