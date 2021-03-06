﻿/******************************************************************************
 *   
 *   Class for HID APIs.
 * 
 ******************************************************************************/
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;

namespace HIDLib
{
    /// <summary>
    /// Class for HID APIs
    /// </summary>
    public class HIDAPIs
    {
        public const string LogHIDHWDev = @"Logs\HIDLib.log";
        /* invalid handle value */
        public static IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);

        static IntPtr GetClassDevs(Guid guid, uint flags)
        {
            return HIDNativeAPIs.SetupDiGetClassDevs(ref guid, null, IntPtr.Zero, flags);
        }
        /* browse all HID class devices */
        public static List<HIDInfo> BrowseHID()
        {
            /* hid device class guid */
            Guid gHid;
            /* list of device information */
            List<HIDInfo> info = new List<HIDInfo>();

            /* obtain hid guid */
            HIDNativeAPIs.HidD_GetHidGuid(out gHid);
            /* get list of present hid devices */
            //var hInfoSet = GetClassDevs(gHid, (uint)DIGCF.DIGCF_PRESENT | (uint)DIGCF.DIGCF_DEVICEINTERFACE);
            var hInfoSet = GetClassDevs(Guid.Empty, (uint)DIGCF.DIGCF_ALLCLASSES | (uint)DIGCF.DIGCF_DEVICEINTERFACE);

            /* allocate mem for interface descriptor */
            var iface = new DeviceInterfaceData();
            /* set size field */
            iface.Size = Marshal.SizeOf(iface);
            /* interface index */
            uint index = 0;

            /* iterate through all interfaces */
            while (HIDNativeAPIs.SetupDiEnumDeviceInterfaces(hInfoSet, 0, ref gHid, index, ref iface))
            {
                bool isWork = false;
                HIDInfo hidInfo = new HIDInfo(hInfoSet, iface, out isWork);
                if (isWork)
                {
                    info.Add(hidInfo);
                }
                /* next, please */
                index++;
            }

            /* clean up */
            if (HIDNativeAPIs.SetupDiDestroyDeviceInfoList(hInfoSet) == false)
            {
                /* fail! */
                //throw new Win32Exception();
            }

            /* return list */
            return info;
        }
    }

    class HIDNativeAPIs
    {
        #region kernel32.dll
        /* read access */
        public const uint GENERIC_READ = 0x80000000;
        /* write access */
        public const uint GENERIC_WRITE = 0x40000000;
        /* Enables subsequent open operations on a file or device to request 
         * write access.*/
        public const uint FILE_SHARE_WRITE = 0x2;
        /* Enables subsequent open operations on a file or device to request
         * read access. */
        public const uint FILE_SHARE_READ = 0x1;
        /* The file or device is being opened or created for asynchronous I/O. */
        public const uint FILE_FLAG_OVERLAPPED = 0x40000000;
        /* Opens a file or device, only if it exists. */
        public const uint FILE_CREATE_NEW = 1;
        /* Opens a file or device, only if it exists. */
        public const uint OPEN_EXISTING = 3;
        /* Opens a file, always. */
        public const uint OPEN_ALWAYS = 4;

        [DllImport("kernel32.dll", SetLastError = true)]
        /* opens files that access usb hid devices */
        public static extern SafeFileHandle CreateFile(
            [MarshalAs(UnmanagedType.LPStr)] string strName,
            uint nAccess, uint nShareMode, IntPtr lpSecurity,
            uint nCreationFlags, uint nAttributes, IntPtr lpTemplate);

        [DllImport("kernel32", SetLastError = true)]
        public static extern bool ReadFile(
            IntPtr hFile,      // handle to file
            byte[] pBuffer,            // data buffer
            int NumberOfBytesToRead,  // number of bytes to read
            int pNumberOfBytesRead,  // number of bytes read
            int Overlapped            // overlapped buffer
            );

        [DllImport("kernel32.dll", SetLastError = true)]
        /* closes file */
        public static extern bool CloseHandle(SafeFileHandle hObject);

        [DllImport("Kernel32.dll")]
        public extern static int FormatMessage(int flag, ref IntPtr source, int msgid, int langid, ref string buf, int size, ref IntPtr args);

        public static string GetSysErrMsg(int errCode)
        {
            IntPtr tempptr = IntPtr.Zero;
            string msg = null;
            FormatMessage(0x1300, ref tempptr, errCode, 0, ref msg, 255, ref tempptr);
            return msg;
        }
        #endregion

        #region hid.dll


        [DllImport("hid.dll", SetLastError = true)]
        /* gets HID class Guid */
        public static extern void HidD_GetHidGuid(out Guid gHid);

        /* gets hid device attributes */
        [DllImport("hid.dll", SetLastError = true)]
        public static extern Boolean HidD_GetAttributes(IntPtr hFile,
            ref HiddAttributtes attributes);

        /* gets usb manufacturer string */
        [DllImport("hid.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern Boolean HidD_GetManufacturerString(IntPtr hFile,
            StringBuilder buffer, Int32 bufferLength);

        /* gets product string */
        [DllImport("hid.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern Boolean HidD_GetProductString(IntPtr hFile,
            StringBuilder buffer, Int32 bufferLength);

        /* gets serial number string */
        [DllImport("hid.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool HidD_GetSerialNumberString(IntPtr hDevice,
            StringBuilder buffer, Int32 bufferLength);

        [DllImport("hid.dll", SetLastError = true)]
        public static extern bool HidD_GetPreparsedData(
           SafeFileHandle hObject,
           ref IntPtr PreparsedData);

        [DllImport("hid.dll", SetLastError = true)]
        public static extern Boolean HidD_FreePreparsedData(ref IntPtr PreparsedData);

        [DllImport("hid.dll", SetLastError = true)]
        public static extern int HidP_GetCaps(
            IntPtr pPHIDP_PREPARSED_DATA,					// IN PHIDP_PREPARSED_DATA  PreparsedData,
            ref HIDP_CAPS myPHIDP_CAPS);				// OUT PHIDP_CAPS  Capabilities

        [DllImport("hid.dll", SetLastError = true)]
        public static extern bool HidD_SetOutputReport(SafeFileHandle hidDeviceObject, byte[] lpReportBuffer, uint reportBufferLength);

        [DllImport("hid.dll", SetLastError = true)]
        public static extern bool HidD_GetInputReport(SafeFileHandle hidDeviceObject, byte[] reportBuffer, uint reportBufferLength);

        #endregion

        #region setupapi.dll
        /* Return only devices that are currently present in a system. */
        public const int DIGCF_PRESENT = 0x02;
        /* Return devices that support device interfaces for the specified 
         * device interface classes. */
        public const int DIGCF_DEVICEINTERFACE = 0x10;



        /* function returns a handle to a device information set that contains
         * requested device information elements for a local computer */
        [DllImport("setupapi.dll", SetLastError = true)]
        public static extern IntPtr SetupDiGetClassDevs(ref Guid gClass,
            [MarshalAs(UnmanagedType.LPStr)] string strEnumerator,
            IntPtr hParent, uint nFlags);

        /* The function enumerates the device interfaces that are contained in 
         * a device information set.*/
        [DllImport("setupapi.dll", SetLastError = true)]
        public static extern bool SetupDiEnumDeviceInterfaces(
            IntPtr lpDeviceInfoSet, uint nDeviceInfoData, ref Guid gClass,
            uint nIndex, ref DeviceInterfaceData oInterfaceData);

        /* The SetupDiGetDeviceInterfaceDetail function returns details about 
         * a device interface.*/
        [DllImport("setupapi.dll", SetLastError = true)]
        public static extern bool SetupDiGetDeviceInterfaceDetail(
            IntPtr lpDeviceInfoSet, ref DeviceInterfaceData oInterfaceData,
            ref DeviceInterfaceDetailData oDetailData,
            uint nDeviceInterfaceDetailDataSize, ref uint nRequiredSize,
            IntPtr lpDeviceInfoData);

        /* destroys device list */
        [DllImport("setupapi.dll", SetLastError = true)]
        public static extern bool SetupDiDestroyDeviceInfoList(IntPtr lpInfoSet);
        #endregion
    }

    /* The HIDD_ATTRIBUTES structure contains vendor information about a 
         * HIDClass device.*/
    [StructLayout(LayoutKind.Sequential)]
    public struct HiddAttributtes
    {
        /* size in bytes */
        public Int32 Size;
        /* vendor id */
        public Int16 VendorID;
        /* product id */
        public UInt16 ProductID;
        /* hid vesion number */
        public Int16 VersionNumber;
    }

    /* structure returned by SetupDiEnumDeviceInterfaces */
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct DeviceInterfaceData
    {
        /* size of fixed part of structure */
        public int Size;
        /* The GUID for the class to which the device interface belongs. */
        public Guid InterfaceClassGuid;
        /* Can be one or more of the following: SPINT_ACTIVE, 
         * SPINT_DEFAULT, SPINT_REMOVED */
        public int Flags;
        /* do not use */
        public IntPtr Reserved;
    }

    /* A structure contains the path for a device interface.*/
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct DeviceInterfaceDetailData
    {
        /* size of fixed part of structure */
        public int Size;
        /* device path, as to be used by CreateFile */
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 512)]
        public string DevicePath;
    }

    // HIDP_CAPS
    [StructLayout(LayoutKind.Sequential)]
    public struct HIDP_CAPS
    {
        public System.UInt16 Usage;                 // USHORT
        public System.UInt16 UsagePage;             // USHORT
        public System.UInt16 InputReportByteLength;
        public System.UInt16 OutputReportByteLength;
        public System.UInt16 FeatureReportByteLength;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)]
        public System.UInt16[] Reserved;                // USHORT  Reserved[17];			
        public System.UInt16 NumberLinkCollectionNodes;
        public System.UInt16 NumberInputButtonCaps;
        public System.UInt16 NumberInputValueCaps;
        public System.UInt16 NumberInputDataIndices;
        public System.UInt16 NumberOutputButtonCaps;
        public System.UInt16 NumberOutputValueCaps;
        public System.UInt16 NumberOutputDataIndices;
        public System.UInt16 NumberFeatureButtonCaps;
        public System.UInt16 NumberFeatureValueCaps;
        public System.UInt16 NumberFeatureDataIndices;
    }

    public struct HIDInfoStruct
    {
        /* device path */
        public string HIDFullPath { get; set; }
        /* vendor ID */
        public short Vid { get; set; }
        /* product id */
        public uint Pid { get; set; }
        /* usb product string */
        public string Product { get; set; }
        /* usb manufacturer string */
        public string Manufacturer { get; set; }
        /* usb serial number string */
        public string SerialNumber { get; set; }

        /// <summary>
        /// The Compare string for HID Device
        /// </summary>
        public string HIDCompareStr { get; set; }

        public uint OutputBuffSize { get; set; }
        public uint InputBuffSize { get; set; }
    }

    /// <summary>
    /// Flags controlling what is included in the device information set built by SetupDiGetClassDevs
    /// </summary>
    [Flags]
    public enum DIGCF : uint
    {
        DIGCF_DEFAULT = 0x00000001,    // only valid with DIGCF_DEVICEINTERFACE
        DIGCF_PRESENT = 0x00000002,
        DIGCF_ALLCLASSES = 0x00000004,
        DIGCF_PROFILE = 0x00000008,
        DIGCF_DEVICEINTERFACE = 0x00000010,
    }
}
