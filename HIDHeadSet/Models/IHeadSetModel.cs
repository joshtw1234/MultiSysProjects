using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace HIDHeadSet.Models
{
    public interface IHeadSetModel
    {
        bool Initialize();
        void CloseHID();
        byte[] SetColorData(string ledMode, List<Brush> lstBrush);
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
        HeadSetLEDModes ledCfgMode;
        public HeadSetCfg(HeadSetLEDModes ledMode, int arySize = 1) : base(HeadSetCmds.LEDCfg)
        {
            sz = arySize;
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
                //TODO:add data
            }
            return rev;
        }
    }

    class HeadSetColor : BaseHeadSetCmd
    {
        Brush displayColor;
        public HeadSetColor(Brush color) : base(HeadSetCmds.LEDColorArray)
        {
            displayColor = color;
        }

        public override byte[] ToByteArry()
        {
            byte[] rev = base.ToByteArry();
            rev[4] = (displayColor as SolidColorBrush).Color.R;
            rev[5] = (displayColor as SolidColorBrush).Color.G;
            rev[6] = (displayColor as SolidColorBrush).Color.B;
            return rev;
        }
    }
}
