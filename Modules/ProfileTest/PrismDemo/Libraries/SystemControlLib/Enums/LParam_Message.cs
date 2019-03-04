using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemControlLib.Enums
{
    public enum LParam_Message : ulong
    {
        ENDSESSION_CLOSEAPP = 0x1,
        ENDSESSION_CRITICAL = 0x40000000,
        ENDSESSION_LOGOFF = 0x80000000
    }
}
