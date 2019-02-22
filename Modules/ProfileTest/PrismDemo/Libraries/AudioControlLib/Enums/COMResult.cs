using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioControlLib.Enums
{
    enum COMResult : uint
    {
        E_ACCESSDENIED = 0x80070005,//Access denied.
        E_FAIL = 0x80004005,//	Unspecified error.
        E_INVALIDARG = 0x80070057,//	Invalid parameter value.
        E_OUTOFMEMORY = 0x8007000E,//	Out of memory.
        E_POINTER = 0x80004003,//	NULL was passed incorrectly for a pointer value.
        E_UNEXPECTED = 0x8000FFFF,//	Unexpected condition.
        S_OK = 0x0,//	Success.
        S_FALSE = 0x1,//	Success.
    }
}
