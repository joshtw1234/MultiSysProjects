using System;
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
#if true
             WriteHID(new HPLouieHeadSetCmd(HeadSetCmds.Fan, fMode).ToByteArry());
#else
            WriteHID(new HeadSetFan(fMode).ToByteArry());
#endif
        }

        public void SetColorData(string ledMode, List<Brush> lstBrush, ushort colorInterval)
        {
#if true
            WriteHID(new HPLouieHeadSetCmd(HeadSetCmds.LEDOff).ToByteArry());
            if (ledMode.Equals(HeadSetConstants.LEDStatic))
            {
                WriteHID(new HPLouieHeadSetCmd(HeadSetCmds.LEDCfg, HeadSetLEDModes.Static).ToByteArry());
                WriteHID(new HPLouieHeadSetCmd(HeadSetCmds.LEDColorArray, HeadSetLEDModes.Static, lstBrush).ToByteArry());
            }
            if (ledMode.Equals(HeadSetConstants.LEDRepeatForward))
            {
                WriteHID(new HPLouieHeadSetCmd(HeadSetCmds.LEDCfg, HeadSetLEDModes.RepeatForward, (ushort)lstBrush.Count, colorInterval).ToByteArry());
                SetColorArray(HeadSetLEDModes.RepeatForward, lstBrush);
            }
            if (ledMode.Equals(HeadSetConstants.LEDBackForth))
            {
                WriteHID(new HPLouieHeadSetCmd(HeadSetCmds.LEDCfg, HeadSetLEDModes.BackandForth, (ushort)lstBrush.Count, colorInterval).ToByteArry());
                SetColorArray(HeadSetLEDModes.BackandForth, lstBrush);
            }
            if (ledMode.Equals(HeadSetConstants.LEDLookupTable))
            {
                WriteHID(new HPLouieHeadSetCmd(HeadSetCmds.LEDCfg, HeadSetLEDModes.LookupTable, (ushort)lstBrush.Count, colorInterval).ToByteArry());
                SetColorArray(HeadSetLEDModes.LookupTable, lstBrush);
            }
            WriteHID(new HPLouieHeadSetCmd(HeadSetCmds.LEDOn).ToByteArry());
#else
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
                if (0 == lstBrush.Count % 2)
                {
                    WriteHID(new HeadSetCfg(HeadSetLEDModes.LookupTable, (ushort)lstBrush.Count).ToByteArry());
                    SetColorArray(HeadSetLEDModes.LookupTable, lstBrush);
                }
            }
            WriteHID(new BaseHeadSetCmd(HeadSetCmds.LEDOn).ToByteArry());
#endif
        }

        private void SetColorArray(HeadSetLEDModes ledMode, List<Brush> lstBrush)
        {
            int dvd = lstBrush.Count / 4;
            List<Brush> newLst = new List<Brush>();
            for (int i = 0; i < dvd; i++)
            {
                newLst.AddRange(lstBrush.GetRange(0, 4));
                lstBrush.RemoveRange(0, 4);
                WriteHID(new HPLouieHeadSetCmd(HeadSetCmds.LEDColorArray, ledMode, newLst, (ushort)(i * 4)).ToByteArry());
                newLst.Clear();
            }
            if (0 != lstBrush.Count % 4)
            {
                newLst.AddRange(lstBrush);
                WriteHID(new HPLouieHeadSetCmd(HeadSetCmds.LEDColorArray, ledMode, newLst, (ushort)(dvd * 4)).ToByteArry());
                newLst.Clear();
            }
        }

        public bool Initialize()
        {
            var hidLst = HIDAPIs.BrowseHID();
            //hidDev = hidLst.Find(x => x.HIDInfoStruct.Pid == HeadSetConstants.HeadSetPID && x.HIDInfoStruct.Vid == HeadSetConstants.HeadSetVID);
            hidDev = hidLst.Find(x => x.HIDInfoStruct.Pid == HeadSetConstants.HeadSetZazuPID && x.HIDInfoStruct.Vid == HeadSetConstants.HeadSetVID);
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

        public void GetFWInfo()
        {
            WriteHID(new HPLouieHeadSetCmd(HeadSetCmds.UserData).ToByteArry());
            //WriteHID(new HPLouieHeadSetCmd(HeadSetCmds.Read).ToByteArry());
            //WriteHID(new HPLouieHeadSetCmd(HeadSetCmds.ProductNumber).ToByteArry());
            //WriteHID(new HPLouieHeadSetCmd(HeadSetCmds.SerialNumber).ToByteArry());
            //ReadHID(new byte[16]);
            //if (hidDev.HIDSetOutputReport(true, new HPLouieHeadSetCmd(HeadSetCmds.UserData).ToByteArry()))
            //{
            var data = ReadHID();
            //}
            //if (hidDev.HIDSetOutputReport(true, new HPLouieHeadSetCmd(HeadSetCmds.SerialNumber).ToByteArry()))
            //{
            //    var str = System.Text.Encoding.ASCII.GetString(ReadHID());
            //}
            //if (hidDev.HIDSetOutputReport(true, new HPLouieHeadSetCmd(HeadSetCmds.ProductNumber).ToByteArry()))
            //{
            //    var str = System.Text.Encoding.ASCII.GetString(ReadHID());
            //}

        }

        private byte[] ReadHID()
        {
            //return hidDev.HIDRead();
            //return hidDev.HIDReadAsync();

            var rev = hidDev.HIDGetReport(0x01);
            PrintByteToString(rev);
            byte[] newData = new byte[16];
            Array.Copy(rev, 1, newData, 0, 15);
            return newData;

        }
    }
}
