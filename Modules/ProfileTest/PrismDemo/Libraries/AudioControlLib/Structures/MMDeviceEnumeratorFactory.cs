using System;

namespace AudioControlLib.Structures
{
    class MMDeviceEnumeratorFactory
    {
        public static IMMDeviceEnumerator CreateInstance()
        {
            return (IMMDeviceEnumerator)Activator.CreateInstance(Type.GetTypeFromCLSID(new Guid("BCDE0395-E52F-467C-8E3D-C4579291692E"))); // a MMDeviceEnumerator
        }
    }
}
