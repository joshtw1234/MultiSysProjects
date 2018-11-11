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


using PCVolumeControl.Constants;
using System;
using System.Runtime.InteropServices;
//custom namespaces

namespace PCVolumeControl.Structs
{
    /// <summary>
    /// Class file for holding all the custom sructures we need
    /// for controlling the system sound
    /// </summary>
    public static class VolumeStructs
    {
        /// <summary>
        /// struct for holding data for the mixer caps
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct MixerCapsbbb
        {
            public int wMid;
            public int wPid;
            public int vDriverVersion;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = VolumeConstants.MAXPNAMELEN)]
            public string szPname;
            public int fdwSupport;
            public int cDestinations;
        }

        [StructLayout(LayoutKind.Sequential, Pack =1)]
        public struct MixerCaps
        {
            public ushort ManufacturerID;
            public ushort ProductId;
            public int Version;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string ProductName;
            public uint Support;
            public uint Destinations;
            public override string ToString()
            {
                return String.Format("Manufacturer ID: {0}, Product ID: {1}, Driver Version: {2}, Product Name: \"{3}\", Support: {4}, Destinations: {5}", ManufacturerID, ProductId, Version, ProductName, Support, Destinations);
            }
        }

        /// <summary>
        /// struct to hold data for the mixer control
        /// </summary>
        public struct Mixer
        {
            public int cbStruct;
            public int dwControlID;
            public int dwControlType;
            public int fdwControl;
            public int cMultipleItems;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = VolumeConstants.MIXER_SHORT_NAME_CHARS)]
            public string szShortName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = VolumeConstants.MIXER_LONG_NAME_CHARS)]
            public string szName;
            public int lMinimum;
            public int lMaximum;
            [MarshalAs(UnmanagedType.U4, SizeConst = 10)]  public int reserved;
        }

        /// <summary>
        /// struct for holding data about the details of the mixer control
        /// </summary>
        public struct MixerDetails
        {
            public int cbStruct;
            public int dwControlID;
            public int cChannels;
            public int item;
            public int cbDetails;
            public IntPtr paDetails;
        }

        /// <summary>
        /// struct to hold data for an unsigned mixer control details
        /// </summary>
        public struct UnsignedMixerDetails
        {
            public int dwValue;
        }

        /// <summary>
        /// struct to hold data for the mixer line
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct MixerLine
        {
            public int cbStruct;
            public uint dwDestination;
            public int dwSource;
            public int dwLineID;
            public int fdwLine;
            public int dwUser;
            public uint dwComponentType;
            public int cChannels;
            public int cConnections;
            public int cControls;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = VolumeConstants.MIXER_SHORT_NAME_CHARS)]
            public string szShortName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = VolumeConstants.MIXER_LONG_NAME_CHARS)]
            public string szName;
            public int dwType;
            public int dwDeviceID;
            public int wMid;
            public int wPid;
            public int vDriverVersion;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = VolumeConstants.MAXPNAMELEN)]
            public string szPname;
            public MIXERLINETARGET Target;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct MIXERLINE
        {
            private uint cbStruct;
            private uint dwDestination;
            private uint dwSource;
            private uint dwLineID;
            private uint fdwLine;
            private int dwUser;
            private uint dwComponentType;
            private uint cChannels;
            private uint cConnections;
            private uint cControls;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = VolumeConstants.MIXER_SHORT_NAME_CHARS)]
            private string szShortName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = VolumeConstants.MIXER_LONG_NAME_CHARS)]
            private string szName;
            public MIXERLINETARGET Target;
            private uint dwType;
            private uint dwDeviceID;
            private ushort wMid;
            private ushort wPid;
            private uint vDriverVersion;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = VolumeConstants.MAXPNAMELEN)]
            private string szPname;
           
            #region Properties

            /// <summary>MIXERLINE_TARGETTYPE_xxxx</summary>
            public VolumeConstants.MIXERLINE_TARGETTYPE Type
            {
                get { return (VolumeConstants.MIXERLINE_TARGETTYPE)this.dwType; }
                set { this.dwType = (uint)value; }
            }
            /// <summary>size of MIXERLINE structure</summary>
            public uint StructSize
            {
                get { return this.cbStruct; }
                set { this.cbStruct = value; }
            }
            /// <summary>target device ID of device type</summary>
            public uint DeviceID
            {
                get { return this.dwDeviceID; }
                set { this.dwDeviceID = value; }
            }
            /// <summary>zero based destination index</summary>
            public uint Destination
            {
                get { return this.dwDestination; }
                set { this.dwDestination = value; }
            }
            /// <summary>of target device</summary>
            public ushort ManufacturerID
            {
                get { return this.wMid; }
                set { this.wMid = value; }
            }
            /// <summary>zero based source index (if source)</summary>
            public uint Source
            {
                get { return this.dwSource; }
                set { this.dwSource = value; }
            }
            /// <summary></summary>
            public ushort ProductID
            {
                get { return this.wPid; }
                set { this.wPid = value; }
            }
            /// <summary>unique line id for mixer device</summary>
            public uint LineID
            {
                get { return this.dwLineID; }
                set { this.dwLineID = value; }
            }
            /// <summary></summary>
            public uint DriverVersion
            {
                get { return this.vDriverVersion; }
                set { this.vDriverVersion = value; }
            }
            /// <summary>state/information about line</summary>
            public VolumeConstants.MIXERLINE_LINEF LineInformation
            {
                get { return (VolumeConstants.MIXERLINE_LINEF)this.fdwLine; }
                set { this.fdwLine = (uint)value; }
            }
            /// <summary></summary>
            public string PName
            {
                get { return this.szPname; }
                set { this.szPname = value; }
            }
            /// <summary>driver specific information</summary>
            public int UserPointer
            {
                get { return this.dwUser; }
                set { this.dwUser = value; }
            }
            /// <summary>component type line connects to</summary>
            public VolumeConstants.MIXERLINE_COMPONENTTYPE ComponentType
            {
                get { return (VolumeConstants.MIXERLINE_COMPONENTTYPE)this.dwComponentType; }
                set { this.dwComponentType = (uint)value; }
            }
            /// <summary>number of channels line supports</summary>
            public uint Channels
            {
                get { return this.cChannels; }
                set { this.cChannels = value; }
            }
            /// <summary>number of connections [possible]</summary>
            public uint Connections
            {
                get { return this.cConnections; }
                set { this.cConnections = value; }
            }
            /// <summary>number of controls at this line</summary>
            public uint Controls
            {
                get { return this.cControls; }
                set { this.cControls = value; }
            }
            /// <summary></summary>
            public string ShortName
            {
                get { return this.szShortName; }
                set { this.szShortName = value; }
            }
            /// <summary></summary>
            public string Name
            {
                get { return this.szName; }
                set { this.szName = value; }
            }

            #endregion
        }

        [StructLayout(LayoutKind.Sequential, Pack = 2)]
        public struct MIXERLINETARGET
        {
            private uint dwType;
            private uint dwDeviceID;
            private ushort wMid;
            private ushort wPid;
            private uint vDriverVersion;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = VolumeConstants.MAXPNAMELEN)]
            private string szPname;
            #region Properties

            /// <summary>MIXERLINE_TARGETTYPE_xxxx</summary>
            public VolumeConstants.MIXERLINE_TARGETTYPE Type
            {
                get { return (VolumeConstants.MIXERLINE_TARGETTYPE)this.dwType; }
                set { this.dwType = (uint)value; }
            }
            /// <summary>target device ID of device type</summary>
            public uint DeviceID
            {
                get { return this.dwDeviceID; }
                set { this.dwDeviceID = value; }
            }
            /// <summary>of target device</summary>
            public ushort ManufacturerID
            {
                get { return this.wMid; }
                set { this.wMid = value; }
            }
            /// <summary></summary>
            public ushort ProductID
            {
                get { return this.wPid; }
                set { this.wPid = value; }
            }
            /// <summary></summary>
            public uint DriverVersion
            {
                get { return this.vDriverVersion; }
                set { this.vDriverVersion = value; }
            }
            /// <summary></summary>
            public string PName
            {
                get { return this.szPname; }
                set { this.szPname = value; }
            }

            #endregion
        }


        /// <summary>
        /// struct for holding data for the mixer line controls
        /// </summary>
        public struct LineControls
        {
            public int cbStruct;
            public int dwLineID;
            public int dwControl;
            public int cControls;
            public int cbmxctrl;
            public IntPtr pamxctrl;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
        public struct MIXERLINECONTROLS
        {
            private UInt32 cbStruct;       /* size in bytes of MIXERLINECONTROLS */
            private UInt32 dwLineID;       /* line id (from MIXERLINE.dwLineID) */
            private UInt32 dwControlID;    /* MIXER_GETLINECONTROLSF_ONEBYID */
                                           //    private UInt32 dwControlType;  //UNIONED with dwControlID /* MIXER_GETLINECONTROLSF_ONEBYTYPE */
            private UInt32 cControls;      /* count of controls pmxctrl points to */
            private UInt32 cbmxctrl;       /* size in bytes of _one_ MIXERCONTROL */
            private IntPtr pamxctrl;       /* pointer to first MIXERCONTROL array */

            #region Properties

            /// <summary>size in bytes of MIXERLINECONTROLS</summary>
            public UInt32 StructSize
            {
                get { return this.cbStruct; }
                set { this.cbStruct = value; }
            }
            /// <summary>line id (from MIXERLINE.dwLineID)</summary>
            public UInt32 LineID
            {
                get { return this.dwLineID; }
                set { this.dwLineID = value; }
            }
            /// <summary>MIXER_GETLINECONTROLSF_ONEBYID</summary>
            public UInt32 ControlID
            {
                get { return this.dwControlID; }
                set { this.dwControlID = value; }
            }
            /// <summary>MIXER_GETLINECONTROLSF_ONEBYTYPE</summary>
            public VolumeConstants.MIXERCONTROL_CONTROLTYPE ControlType
            {
                get { return (VolumeConstants.MIXERCONTROL_CONTROLTYPE)this.dwControlID; }
                set { this.dwControlID = (uint)value; }
            }
            /// <summary>count of controls pmxctrl points to</summary>
            public UInt32 Controls
            {
                get { return this.cControls; }
                set { this.cControls = value; }
            }
            /// <summary>size in bytes of _one_ MIXERCONTROL</summary>
            public UInt32 MixerControlItemSize
            {
                get { return this.cbmxctrl; }
                set { this.cbmxctrl = value; }
            }
            /// <summary>pointer to first MIXERCONTROL array</summary>
            public IntPtr MixerControlArray
            {
                get { return this.pamxctrl; }
                set { this.pamxctrl = value; }
            }

            #endregion
        }
    }
}
