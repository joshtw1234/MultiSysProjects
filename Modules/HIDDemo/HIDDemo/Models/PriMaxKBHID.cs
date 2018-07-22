using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIDDemo.Models
{
    /// <summary>
    /// The Enum for Dragons Macro Key
    /// </summary>
    public enum HIDMacroKeys : byte
    {
        P1 = 0x41,
        P2,
        P3,
        P4,
        P5,
        P6,
        FN_P1,
        FN_P2,
        FN_P3,
        FN_P4,
        FN_P5,
        FN_P6
    }

    /// <summary>
    /// Enum for Dragon keyboard lighting mode
    /// </summary>
    public enum HIDKBLightingMode
    {
        Animation,
        Static,
        Off
    }

    /// <summary>
    /// Enum for Dragon Keyboard Static lighting mode
    /// </summary>
    public enum HIDKBStaticLightingMode
    {
        WASD,
        FPS,
        MOBA,
        MMO,
        P1P6,
        AllKeys,
        CUSTOM
    }

    /// <summary>
    /// The Keyboard Language/ layout
    /// </summary>
    public enum HIDKeyboardLang
    {
        US = 0x00,
        JP,
        UK,
        ENFR,
        USINTL,
        GR,
        FR,
        ITL,
        SP,
        Nordic,
        PORT,
        ENGARAB,
        BEL,
        HE,
        RUSS,
        SWISS,
        TURK,
        CSSK,
        THAI
    }

    /// <summary>
    /// Enum for MCU commands
    /// </summary>
    public enum PriMaxHIDCommand
    {
        SET_DEVICE_INFO = 0x01,
        SET_MACRO_KEY_DATA = 0x02,
        SET_LIGHT_EFFECT_DATA = 0x03,
        SET_LED_ON_OFF = 0x04,
        SET_LED_RED = 0x05,
        SET_LED_GREEN = 0x06,
        SET_LED_BLUE = 0x07,
        SET_VIBRATOR_DATA = 0x08,
        SET_KEYBOARD_ON_OFF = 0x09,
        GET_DEVICE_INFO = 0x80,
        GET_MACRO_KEY_DATA = 0x82,
        GET_LIGHT_EFFECT_DATA = 0x83
    }


    public struct HIDCmdHeaderPackage
    {
        public byte Command;
        public byte Index;
        public byte Blength_Low;
        public byte BLength_Height;
        public byte Data1;
        public byte Data2;
        public byte[] ToMCUBytes()
        {
            byte[] rev = new byte[6];
            rev[0] = Command;
            rev[1] = Index;
            rev[2] = Blength_Low;
            rev[3] = BLength_Height;
            rev[4] = Data1;
            rev[5] = Data2;
            return rev;
        }
    }
    public class PriMaxKBHID : BaseHID
    {
        /// <summary>
        /// Get Keyboard Language/ Layout
        /// </summary>
        public static byte[] GetCmdKeyboardLang()
        {
            HIDCmdHeaderPackage mPkg = new HIDCmdHeaderPackage() { Command = (byte)PriMaxHIDCommand.GET_DEVICE_INFO, Index = 0x01, Blength_Low = 0x00, BLength_Height = 0x00, Data1 = 0x00, Data2 = 0x00 };
            byte[] cmdByte = new byte[64];
            Array.Copy(mPkg.ToMCUBytes(), 0, cmdByte, 0, mPkg.ToMCUBytes().Length);
            return cmdByte;
        }
    }
}
