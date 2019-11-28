using System;

namespace AudioControlLib.Structures
{
    class WAVEFORMATEX
    {
        UInt16 wFormatTag;
        UInt16 nChannels;
        UInt32 nSamplesPerSec;
        UInt32 nAvgBytesPerSec;
        UInt16 nBlockAlign;
        UInt16 wBitsPerSample;
        UInt16 cbSize;
    }
}
