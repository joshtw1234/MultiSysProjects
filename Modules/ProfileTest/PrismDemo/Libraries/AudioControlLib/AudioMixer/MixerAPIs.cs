using AudioControlLib.AudioMixer.Enums;
using AudioControlLib.AudioMixer.Structures;
using System;
using System.Runtime.InteropServices;

namespace AudioControlLib.AudioMixer
{
    class MixerAPIs
    {
        [DllImport("winmm.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern uint mixerGetNumDevs();
        [DllImport("winmm.dll", CharSet = CharSet.Unicode)]
        public static extern MMResult mixerGetID(IntPtr hmxobj, ref int mxId, Mixer_ObjectFlag fdwId);
        [DllImport("winmm.dll", CharSet = CharSet.Unicode)]
        public static extern MMResult mixerGetDevCaps(int uMxId, ref MixerCaps pmxcaps, int cbmxcaps);
    }
}
