using System.Runtime.InteropServices;

namespace MYAudioSDK.CAudioSDK.CallBacks
{
    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    delegate void CAudioSDKCallback(int type, int id, int componentType, ulong eventId);
}
