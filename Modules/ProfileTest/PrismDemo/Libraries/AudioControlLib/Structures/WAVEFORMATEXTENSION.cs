using AudioControlLib.Enums;
using System;
using System.Runtime.InteropServices;

namespace AudioControlLib.Structures
{
    [StructLayout(LayoutKind.Sequential)]
    class WAVEFORMATEXTENSION
    {
        WaveFormat wFormatTag;
        UInt16 nChannels;
        UInt32 nSamplesPerSec;
        UInt32 nAvgBytesPerSec;
        UInt16 nBlockAlign;
        UInt16 wBitsPerSample;
        UInt16 cbSize;
        UInt16 wValidBitsPerSample;
        UInt32 dwChannelMask;
        Guid subFormat;
        public WaveFormat WFormatTag { get { return wFormatTag; } }
        public UInt16 NChannels { get { return nChannels; } }
        public UInt32 NSamplesPerSec { get { return nSamplesPerSec; } }
        public UInt32 NAvgBytesPerSec { get { return nAvgBytesPerSec; } set { nAvgBytesPerSec = value; } }
        public UInt16 NBlockAlign { get { return nBlockAlign; } set { nBlockAlign = value; } }
        public UInt16 WBitsPerSample { get { return wBitsPerSample; } set { wBitsPerSample = value; } }
        public UInt16 WValidBitsPerSample { get { return wValidBitsPerSample; } }
        public Guid SubFormat { get { return subFormat; } }
    }
}
