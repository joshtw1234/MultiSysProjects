﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioControlLib.Enums
{
    [Flags]
    enum AudioDeviceState
    {
        DEVICE_STATE_ACTIVE = 0x00000001,
        DEVICE_STATE_DISABLED = 0x00000002,
        DEVICE_STATE_NOTPRESENT = 0x00000004,
        DEVICE_STATE_UNPLUGGED = 0x00000008,
        DEVICE_STATEMASK_ALL = 0x0000000F
    }
}
