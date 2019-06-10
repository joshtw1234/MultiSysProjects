using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace HIDHeadSet.Models
{
    class HPLouieHeadSetCmd
    {
        private byte reportID = 0x01;
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
            switch (headSetCmd)
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
                case HeadSetCmds.UserData:
                    break;
            }
            return revbuf;
        }
    }
}
