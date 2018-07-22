using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace HIDHeadSet.Models
{
    public interface IHeadSetModel
    {
        bool Initialize();
        void CloseHID();
        void SetColorData(string ledMode, List<Brush> lstBrush, ushort colorInterval);
        void SetFanData(HeadSetFanModes fMode);
    }

    class HeadSetConstants
    {
        /// <summary>
        /// Log file name
        /// </summary>
        public const string LogHeadSet = @"Logs\HeadSet.log";

        public const uint HeadSetPIDPro = 0x8824;
        public const uint HeadSetVIDPro = 0x0D8C;
        public const uint HeadSetPID = 0x2841;
        public const uint HeadSetVID = 0x03F0;
        public const uint HeadSetBufferSize = 16;
        public const string LEDStatic = "Static";
        public const string LEDRepeatForward = "Repeat Forward";
        public const string LEDBackForth = "Back and Forth";
        public const string LEDLookupTable = "Lookup Table";
    }

    public enum HeadSetFanModes
    {
        Off,
        Light,
        Medium,
        Heavy
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

#if true
#else
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

    class HeadSetFan : BaseHeadSetCmd
    {
        HeadSetFanModes fanMode;
        public HeadSetFan(HeadSetFanModes _fanMode) : base(HeadSetCmds.Fan)
        {
            fanMode = _fanMode;
        }
        public override byte[] ToByteArry()
        {
            byte[] rev =  base.ToByteArry();
            rev[2] = (byte)fanMode;
            return rev;
        }
    }

    class HeadSetCfg: BaseHeadSetCmd
    {
        ushort sz = 0;
        ushort colorInterval = 0;
        HeadSetLEDModes ledCfgMode;
        public HeadSetCfg(HeadSetLEDModes ledMode, ushort arySize = 1, ushort interVal = 0) : base(HeadSetCmds.LEDCfg)
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
        ushort aryOffset;
        public HeadSetColor(HeadSetLEDModes ledMode, List<Brush> lstColors, ushort offSet=0) : base(HeadSetCmds.LEDColorArray)
        {
            mode = ledMode;
            displayColors = lstColors;
            aryOffset = offSet;
        }

        public override byte[] ToByteArry()
        {
            byte[] rev = base.ToByteArry();
            byte[] lenByte = BitConverter.GetBytes(aryOffset);
            rev[2] = lenByte[0];
            rev[3] = lenByte[1];
            int colorIdx = 4;
            foreach (SolidColorBrush sosh in displayColors)
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
#endif
}
