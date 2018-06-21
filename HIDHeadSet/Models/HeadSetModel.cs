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

        public void SetFanData(HeadSetFanModes fMode)
        {
            WriteHID(new HeadSetFan(fMode).ToByteArry());
        }

        public void SetColorData(string ledMode, List<Brush> lstBrush, ushort colorInterval)
        {
            WriteHID(new BaseHeadSetCmd(HeadSetCmds.LEDOff).ToByteArry());
            if (ledMode.Equals(HeadSetConstants.LEDStatic))
            {
                WriteHID(new HeadSetCfg(HeadSetLEDModes.Static).ToByteArry());
                WriteHID(new HeadSetColor(HeadSetLEDModes.Static, lstBrush).ToByteArry());
            }
            if (ledMode.Equals(HeadSetConstants.LEDRepeatForward))
            {
                WriteHID(new HeadSetCfg(HeadSetLEDModes.RepeatForward, (ushort)lstBrush.Count, colorInterval).ToByteArry());
                SetColorArray(HeadSetLEDModes.RepeatForward, lstBrush);
            }
            if (ledMode.Equals(HeadSetConstants.LEDBackForth))
            {
                WriteHID(new HeadSetCfg(HeadSetLEDModes.BackandForth, (ushort)lstBrush.Count, colorInterval).ToByteArry());
                SetColorArray(HeadSetLEDModes.BackandForth, lstBrush);
            }
            if (ledMode.Equals(HeadSetConstants.LEDLookupTable))
            {

            }
            WriteHID(new BaseHeadSetCmd(HeadSetCmds.LEDOn).ToByteArry());
        }

        private void SetColorArray(HeadSetLEDModes ledMode, List<Brush> lstBrush)
        {
            int dvd = lstBrush.Count / 4;
            List<Brush> newLst = new List<Brush>();
            for (int i = 0; i < dvd; i++)
            {
                newLst.AddRange(lstBrush.GetRange(0, 4));
                lstBrush.RemoveRange(0, 4);
                WriteHID(new HeadSetColor(ledMode, newLst, (ushort)(i * 4)).ToByteArry());
                newLst.Clear();
            }
            if (0 != lstBrush.Count % 4)
            {
                newLst.AddRange(lstBrush);
                WriteHID(new HeadSetColor(ledMode, newLst, (ushort)(dvd * 4)).ToByteArry());
                newLst.Clear();
            }
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
            PrintByteToString(data);
            bool rev = hidDev.HIDWriteAsync(data);
            if (!rev)
            {
                Utilities.Logger(HeadSetConstants.LogHeadSet, $"WriteHID Failed");
            }
            return rev;
        }

        /// <summary>
        /// Print byte array to string.
        /// </summary>
        /// <param name="pData"></param>
        /// <param name="msg"></param>
        private void PrintByteToString(byte[] pData, string msg = "")
        {
#if DEBUG
            if (null == pData)
            {
                Utilities.Logger(HeadSetConstants.LogHeadSet, $"PrintByteToString Null Data");
                return;
            }
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            //
            foreach (byte bData in pData)
            {
                sb.Append(bData.ToString("X2"));
                sb.Append("|");
            }
            Utilities.Logger(HeadSetConstants.LogHeadSet, $"PrintByteToString {sb.ToString()}");
            sb.Clear();
#endif
        }


    }
}
