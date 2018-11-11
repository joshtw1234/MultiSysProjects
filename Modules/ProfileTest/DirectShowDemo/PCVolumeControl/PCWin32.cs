//*****************************************************************************************
//                           LICENSE INFORMATION
//*****************************************************************************************
//   PC_VolumeControl Version 1.0.0.0
//   A class library for creating a mixer control and controlling the volume on your computer
//
//   Copyright (C) 2007  
//   Richard L. McCutchen 
//   Email: psychocoder@dreamincode.net
//   Created: 04OCT06
//
//   This program is free software: you can redistribute it and/or modify
//   it under the terms of the GNU General Public License as published by
//   the Free Software Foundation, either version 3 of the License, or
//   (at your option) any later version.
//
//   This program is distributed in the hope that it will be useful,
//   but WITHOUT ANY WARRANTY; without even the implied warranty of
//   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//   GNU General Public License for more details.
//
//   You should have received a copy of the GNU General Public License
//   along with this program.  If not, see <http://www.gnu.org/licenses/>.
//*****************************************************************************************

using PCVolumeControl.Structs;
using System;
using System.Runtime.InteropServices;
//custom Namespaces

namespace PCVolumeControl.Win32
{
    /// <summary>
    /// Class that contains all the Win32 API calls
    /// we need for controlling the system sound
    /// </summary>
    public static class PCWin32
    {
        [DllImport("winmm.dll", EntryPoint = "mixerClose", CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int MixerClose(IntPtr hmx);

        [DllImport("winmm.dll", CharSet=CharSet.Ansi)]
        public static extern int mixerGetControlDetailsA(int hmxobj, ref MixerDetails pmxcd, int fdwDetails);

        [DllImport("winmm.dll", EntryPoint = "mixerGetDevCaps", CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int MixerGetDevCaps(int mixerId, ref MixerCaps mixerCaps, int mixerCapsSize);

        [DllImport("winmm.dll", EntryPoint = "mixerGetID", CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int MixerGetID(IntPtr mixerHandel, out int mixerID, int flagID);

        [DllImport("winmm.dll", CharSet=CharSet.Ansi)]
        public static extern int mixerGetLineControlsA(int hmxobj, ref LineControls pmxlc, int fdwControls);

        [DllImport("winmm.dll", EntryPoint = "mixerGetLineControls", CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int MixerGetLineControls(IntPtr hmxobj, ref MIXERLINECONTROLS pmxlc, uint fdwControls);

        [DllImport("winmm.dll", EntryPoint = "mixerGetLineInfo", CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int MixerGetLineInfo(IntPtr hmxobj, ref MIXERLINE pmxl, uint fdwInfo);

        [DllImport("winmm.dll", CharSet=CharSet.Ansi)]
        public static extern int mixerGetNumDevs();

        [DllImport("winmm.dll", CharSet=CharSet.Ansi)]
        public static extern int mixerMessage(int hmx, int uMsg, int dwParam1, int dwParam2);

        [DllImport("winmm.dll", EntryPoint = "mixerOpen", CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int MixerOpen(ref IntPtr phmx, int pMxId, IntPtr dwCallback, IntPtr dwInstance, int fdwOpen);

        [DllImport("winmm.dll", CharSet=CharSet.Ansi)]
        public static extern int mixerSetControlDetails(int hmxobj, ref MixerDetails pmxcd, int fdwDetails);
    }
}
