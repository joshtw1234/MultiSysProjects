using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace HIDHeadSet.Models
{
    public interface IHeadSetModel
    {
        bool Initialize();
        void CloseHID();
        void SetColorData(string ledMode, List<Brush> lstBrush, int colorInterval);
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

    enum HeadSetCmds
    {
        LEDOn = 0x01,
        LEDOff,
        LEDCfg,
        LEDColorArray,
        Fan
    }

    enum HeadSetLEDModes
    {
        Static,
        RepeatForward,
        BackandForth,
        LookupTable
    }

    class BaseHeadSetCmd
    {
        protected byte reportID = 0xFF;
        protected byte headSetCmd;
        public BaseHeadSetCmd(HeadSetCmds cmd)
        {
            headSetCmd = (byte)cmd;
        }

        public virtual byte[] ToByteArry()
        {
            byte[] rev = new byte[HeadSetConstants.HeadSetBufferSize];
            rev[0] = reportID;
            rev[1] = headSetCmd;
            return rev;
        }
    }

    class HeadSetCfg: BaseHeadSetCmd
    {
        int sz = 0;
        int colorInterval = 0;
        HeadSetLEDModes ledCfgMode;
        public HeadSetCfg(HeadSetLEDModes ledMode, int arySize = 1, int interVal = 50) : base(HeadSetCmds.LEDCfg)
        {
            sz = arySize;
            colorInterval = interVal;
            ledCfgMode = ledMode;
        }

        public override byte[] ToByteArry()
        {
            byte[] rev = base.ToByteArry();
            rev[2] = (byte)ledCfgMode;
            byte[] lenByte = BitConverter.GetBytes(sz);
            rev[3] = lenByte[0];
            rev[4] = lenByte[1];
            if (ledCfgMode != HeadSetLEDModes.Static && ledCfgMode != HeadSetLEDModes.LookupTable)
            {
                lenByte = BitConverter.GetBytes(colorInterval);
                rev[5] = lenByte[0];
                rev[6] = lenByte[1];
            }
            return rev;
        }
    }

    class HeadSetColor : BaseHeadSetCmd
    {
        HeadSetLEDModes mode;
        List<Brush> displayColors;
        public HeadSetColor(HeadSetLEDModes ledMode, List<Brush> lstColors) : base(HeadSetCmds.LEDColorArray)
        {
            mode = ledMode;
            displayColors = lstColors;
        }

        public override byte[] ToByteArry()
        {
            byte[] rev = base.ToByteArry();
            if (mode != HeadSetLEDModes.Static)
            {
                int cnt = displayColors.Count;
                byte[] lenByte = BitConverter.GetBytes(cnt);
                rev[2] = lenByte[0];
                rev[3] = lenByte[1];
            }
            int colorIdx = 4;
            foreach(SolidColorBrush sosh in displayColors)
            {
                rev[colorIdx] = sosh.Color.R;
                rev[++colorIdx] = sosh.Color.G;
                rev[++colorIdx] = sosh.Color.B;
                ++colorIdx;
                if (colorIdx >= HeadSetConstants.HeadSetBufferSize)
                {
                    break;
                }
            }
            return rev;
        }
    }
}
