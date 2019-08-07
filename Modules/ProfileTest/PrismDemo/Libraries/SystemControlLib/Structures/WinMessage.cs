using System;
using System.Runtime.InteropServices;
using SystemControlLib.Enums;

namespace SystemControlLib.Structures
{
    public class WinMessage
    {
        public WinProc_Message Message;
        public IntPtr WParam;
        public IntPtr LParam;
        public bool IsHandled;
    }

    public class DeviceMessage
    {
        public WM_DeviceChange Message;
        public IntPtr LParam;
        public bool IsHandled;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct DEV_BROADCAST_HDR
    {
        public uint dbch_Size;
        public WM_DeviceType dbch_DeviceType;
        public uint dbch_Reserved;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct DEV_BROADCAST_DEVICEINTERFACE
    {
        public int dbcc_size;
        public WM_DeviceType dbcc_devicetype;
        public int dbcc_reserved;
        public Guid dbcc_classguid;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 255)]
        public string dbcc_name;
    }

    /// <summary>
    /// Contains information about a class of devices.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct BroadcastDeviceinterface
    {
        /// <summary>
        /// The size of this structure, in bytes.
        /// </summary>
        public int Size;

        /// <summary>
        /// The device type.
        /// </summary>
        public WM_DeviceType DeviceType;

        /// <summary>
        /// Reserved.
        /// </summary>
        public int Reserved;

        /// <summary>
        /// The GUID for the interface device class.
        /// </summary>
        public Guid ClassGuid;

        /// <summary>
        /// A null-terminated string that specifies the name of the device.
        /// </summary>
        public short Name;
    }
}
