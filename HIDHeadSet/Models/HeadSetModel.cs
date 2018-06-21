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

        public void SetColorData(string ledMode, List<Brush> lstBrush, int colorInterval)
        {
            WriteHID(new BaseHeadSetCmd(HeadSetCmds.LEDOff).ToByteArry());
            if (ledMode.Equals(HeadSetConstants.LEDStatic))
            {
                WriteHID(new HeadSetCfg(HeadSetLEDModes.Static).ToByteArry());
                System.Threading.Thread.Sleep(50);
                WriteHID(new HeadSetColor(HeadSetLEDModes.Static, lstBrush).ToByteArry());
            }
            if (ledMode.Equals(HeadSetConstants.LEDRepeatForward))
            {
                WriteHID(new HeadSetCfg(HeadSetLEDModes.RepeatForward, (short)lstBrush.Count, colorInterval).ToByteArry());
                System.Threading.Thread.Sleep(50);
                if (lstBrush.Count > 4)
                {
                    int dvd = lstBrush.Count / 4;
                    int left = 0;
                    List<Brush> newLst = new List<Brush>();
                    for(int i = 0; i <= dvd; i++)
                    {
                        left += 4;
                        if (left < lstBrush.Count)
                        {
                            newLst.AddRange(lstBrush.GetRange(i*4, 4));
                            lstBrush.RemoveRange(i*4, 4);
                        }
                        else
                        {
                            newLst.AddRange(lstBrush);
                        }
                        WriteHID(new HeadSetColor(HeadSetLEDModes.RepeatForward, newLst).ToByteArry());
                        System.Threading.Thread.Sleep(50);
                        newLst.Clear();
                    }
                }
                else
                {
                    WriteHID(new HeadSetColor(HeadSetLEDModes.RepeatForward, lstBrush).ToByteArry());
                }
            }
            if (ledMode.Equals(HeadSetConstants.LEDBackForth))
            {
                WriteHID(new HeadSetCfg(HeadSetLEDModes.BackandForth, (short)lstBrush.Count, colorInterval).ToByteArry());
                System.Threading.Thread.Sleep(50);
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
