using System;
using SystemControlLib.Enums;

namespace SystemControlLib.Structures
{
    public class WinMessage
    {
        public WinProc_Message Message;
        public IntPtr WParam;
        public IntPtr LParam;
        public bool IsHandled;
    }
}
