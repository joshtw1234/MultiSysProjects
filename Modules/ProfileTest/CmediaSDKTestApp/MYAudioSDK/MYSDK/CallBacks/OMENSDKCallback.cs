using System.Runtime.InteropServices;

namespace MYAudioSDK.MYSDK.CallBacks
{

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void OMENSDKCallback(int type, int id, int componentType, ulong eventId);
}
