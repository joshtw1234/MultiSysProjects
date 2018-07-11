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
    class HPLouieHeadSetCmd
    {
        private byte reportID = 0xFF;
        HeadSetCmds headSetCmd;
        HeadSetFanModes fanMode;
        HeadSetLEDModes ledMode;

        ushort colorArysz = 0;
        ushort colorInterval = 0;
        ushort aryOffset = 0;
        List<Brush> displayColors;

        /// <summary>
        /// Constructor for base command
        /// </summary>
        /// <param name="cmd">HID Command</param>
        public HPLouieHeadSetCmd(HeadSetCmds cmd)
        {
            headSetCmd = cmd;
        }

        /// <summary>
        /// Constructor command for TEC Cooling Device.
        /// </summary>
        /// <param name="cmd">HID Command</param>
        /// <param name="_fanMode">Cooling Mode</param>
        public HPLouieHeadSetCmd(HeadSetCmds cmd, HeadSetFanModes _fanMode)
        {
            headSetCmd = cmd;
            fanMode = _fanMode;
        }

        /// <summary>
        /// Constructor command for LED Configure.
        /// </summary>
        /// <param name="cmd">HID Command</param>
        /// <param name="_ledMode">LED mode</param>
        /// <param name="arySize">array Size</param>
        /// <param name="interVal">Interval</param>
        public HPLouieHeadSetCmd(HeadSetCmds cmd, HeadSetLEDModes _ledMode, ushort arySize = 1, ushort interVal = 0)
        {
            headSetCmd = cmd;
            ledMode = _ledMode;
            colorArysz = arySize;
            colorInterval = interVal;
        }

        /// <summary>
        /// Constructor command for LED Color arrays.
        /// </summary>
        /// <param name="cmd">HID Command</param>
        /// <param name="_ledMode">LED mode</param>
        /// <param name="lstColors">The Color List in Brush</param>
        /// <param name="offSet">Array offset</param>
        public HPLouieHeadSetCmd(HeadSetCmds cmd, HeadSetLEDModes _ledMode, List<Brush> lstColors, ushort offSet = 0)
        {
            headSetCmd = cmd;
            ledMode = _ledMode;
            displayColors = lstColors;
            aryOffset = offSet;
        }

        public virtual byte[] ToByteArry()
        {
            byte[] tmpbuf = null;
            byte[] revbuf = new byte[HeadSetConstants.HeadSetBufferSize];
            revbuf[0] = reportID;
            revbuf[1] = (byte)headSetCmd;
            switch(headSetCmd)
            {
                case HeadSetCmds.LEDOn:
                case HeadSetCmds.LEDOff:
                    break;
                case HeadSetCmds.LEDCfg:
                    revbuf[2] = (byte)ledMode;
                    tmpbuf = BitConverter.GetBytes(colorArysz);
                    revbuf[3] = tmpbuf[0];
                    revbuf[4] = tmpbuf[1];
                    if (ledMode != HeadSetLEDModes.Static && ledMode != HeadSetLEDModes.LookupTable)
                    {
                        tmpbuf = BitConverter.GetBytes(colorInterval);
                        revbuf[5] = tmpbuf[0];
                        revbuf[6] = tmpbuf[1];
                    }
                    break;
                case HeadSetCmds.LEDColorArray:
                    tmpbuf = BitConverter.GetBytes(aryOffset);
                    revbuf[2] = tmpbuf[0];
                    revbuf[3] = tmpbuf[1];
                    int colorIdx = 4;
                    foreach (SolidColorBrush sosh in displayColors)
                    {
                        revbuf[colorIdx] = sosh.Color.R;
                        revbuf[++colorIdx] = sosh.Color.G;
                        revbuf[++colorIdx] = sosh.Color.B;
                        ++colorIdx;
                        if (colorIdx >= HeadSetConstants.HeadSetBufferSize)
                        {
                            break;
                        }
                    }
                    break;
                case HeadSetCmds.Fan:
                    revbuf[2] = (byte)fanMode;
                    break;
            }
            return revbuf;
        }
    }
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
