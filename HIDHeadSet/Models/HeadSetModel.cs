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

        public void SetColorData(string ledMode, List<Brush> lstBrush)
        {
            WriteHID(new BaseHeadSetCmd(HeadSetCmds.LEDOff).ToByteArry());
            if (ledMode.Equals(HeadSetConstants.LEDStatic))
            {
                
                WriteHID(new HeadSetCfg(HeadSetLEDModes.Static).ToByteArry());
                WriteHID(new HeadSetColor(HeadSetLEDModes.Static, lstBrush).ToByteArry());
                
            }
            if (ledMode.Equals(HeadSetConstants.LEDRepeatForward))
            {
                WriteHID(new HeadSetCfg(HeadSetLEDModes.RepeatForward, lstBrush.Count).ToByteArry());
                WriteHID(new HeadSetColor(HeadSetLEDModes.RepeatForward, lstBrush).ToByteArry());
            }
            if (ledMode.Equals(HeadSetConstants.LEDBackForth))
            {
                WriteHID(new HeadSetCfg(HeadSetLEDModes.BackandForth, lstBrush.Count).ToByteArry());
                WriteHID(new HeadSetColor(HeadSetLEDModes.BackandForth, lstBrush).ToByteArry());
            }
            if (ledMode.Equals(HeadSetConstants.LEDLookupTable))
            {

            }
            WriteHID(new BaseHeadSetCmd(HeadSetCmds.LEDOn).ToByteArry());
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
            bool rev = hidDev.HIDOpenAsync();
            if (!rev)
            {
                Utilities.Logger(HeadSetConstants.LogHeadSet, $"hidDev Open Failed");
            }
            return rev;
        }

        bool WriteHID(byte[] data)
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
