using System.Runtime.InteropServices;

namespace AudioControlLib.AudioMixer.Structures
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 2)]
    struct MixerCaps
    {
        public short wMid;
        public short wPid;
        public int vDriverVersion;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MixerConstants.MAXPNAMELEN)]
        public string szPname;
        public int fdwSupport;
        public int cDestinations;
    }
}
