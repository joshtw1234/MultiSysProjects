using MYAudioSDK.CAudioSDK.Enums;
using System.Runtime.InteropServices;

namespace MYAudioSDK.CAudioSDK.Structures
{

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct CAudioRegisterOperation
    {
        public CAudioSurroundFunction Operation;
        public CAudioSurroundCommand Feature;
        public CAudioSurroundValueType ValueType;
        public byte[] ToBytes()
        {
            byte[] bytes = new byte[Marshal.SizeOf(typeof(CAudioRegisterOperation))];
            GCHandle pinStructure = GCHandle.Alloc(this, GCHandleType.Pinned);
            try
            {
                Marshal.Copy(pinStructure.AddrOfPinnedObject(), bytes, 0, bytes.Length);
                return bytes;
            }
            finally
            {
                pinStructure.Free();
            }
        }
    }
}
