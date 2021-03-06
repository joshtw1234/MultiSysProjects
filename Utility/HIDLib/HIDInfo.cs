﻿using Microsoft.Win32.SafeHandles;
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;
using UtilityUILib;

namespace HIDLib
{

    /// <summary>
    /// Class for HID infomation.
    /// </summary>
    public class HIDInfo
    {
        const int InfomationBuffer = 256;
        byte _ReportID = 0x00;

        /// <summary>
        /// HID HW Device class
        /// </summary>
        private HIDHWDev hidHWDev;

        IntPtr hidInfoSet;
        /* allocate mem for interface descriptor */
        DeviceInterfaceData iface;

        private HIDInfoStruct hidInfoStruct;

        public HIDInfoStruct HIDInfoStruct
        {
            get { return hidInfoStruct; }
        }

        public HIDInfo(IntPtr _hidInfoSet, DeviceInterfaceData _iface, out bool isWork)
        {
            hidInfoSet = _hidInfoSet;
            iface = _iface;
            //Get HW Path
            string devPath = GetPath(hidInfoSet, ref iface);

            //Open HID HW
            hidHWDev = new HIDHWDev(devPath);
            isWork = false;
            if (hidHWDev.OpenAsync())
            {
                hidInfoStruct = new HIDInfoStruct()
                {
                    Manufacturer = GetManufacturer(hidHWDev.HIDHandel),
                    Product = GetProduct(hidHWDev.HIDHandel),
                    SerialNumber = GetSerialNumber(hidHWDev.HIDHandel),
                    HIDFullPath = devPath,
                };
                hidInfoStruct.HIDCompareStr = devPath.Split('&')[2];
                HiddAttributtes hidAttr = new HiddAttributtes();
                try
                {
                    hidAttr = GetVidPid(hidHWDev.HIDHandel);
                }
                catch (Exception ex)
                {
                    Utilities.Logger(HIDAPIs.LogHIDHWDev, $"GetVidPid Error {ex.Message}");
                }
                hidInfoStruct.Vid = hidAttr.VendorID;
                hidInfoStruct.Pid = hidAttr.ProductID;

                hidInfoStruct.OutputBuffSize = hidHWDev.OutputBuffSize;
                hidInfoStruct.InputBuffSize = hidHWDev.InputBuffSize;
                isWork = true;
            }
        }

        #region Get Base Info
        /* get device path */
        private string GetPath(IntPtr hInfoSet, ref DeviceInterfaceData iface)
        {
            /* detailed interface information */
            var detIface = new DeviceInterfaceDetailData();
            /* required size */
            uint reqSize = (uint)Marshal.SizeOf(detIface);

            /* set size. The cbSize member always contains the size of the 
             * fixed part of the data structure, not a size reflecting the 
             * variable-length string at the end. */
            /* now stay with me and look at that x64/x86 maddness! */
            detIface.Size = Marshal.SizeOf(typeof(IntPtr)) == 8 ? 8 : 5;

            /* get device path */
            bool status = HIDNativeAPIs.SetupDiGetDeviceInterfaceDetail(hInfoSet, ref iface, ref detIface, reqSize, ref reqSize, IntPtr.Zero);

            /* whops */
            if (!status)
            {
                /* fail! */
                throw new Win32Exception();
            }
            
            /* return device path */
            return detIface.DevicePath;
        }

        /* get device manufacturer string */
        private string GetManufacturer(SafeFileHandle handle)
        {
            /* buffer */
            var s = new StringBuilder(256);
            /* returned string */
            string rc = String.Empty;

            /* get string */
            if (HIDNativeAPIs.HidD_GetManufacturerString(handle.DangerousGetHandle(), s, s.Capacity))
            {
                rc = s.ToString();
            }

            /* report string */
            return rc;
        }

        /* get device product string */
        private string GetProduct(SafeFileHandle handle)
        {
            /* buffer */
            var s = new StringBuilder(256);
            /* returned string */
            string rc = String.Empty;

            /* get string */
            if (HIDNativeAPIs.HidD_GetProductString(handle.DangerousGetHandle(), s, s.Capacity))
            {
                rc = s.ToString();
            }

            /* report string */
            return rc;
        }

        /* get device product string */
        private string GetSerialNumber(SafeFileHandle handle)
        {
            /* buffer */
            var s = new StringBuilder(256);
            /* returned string */
            string rc = String.Empty;

            /* get string */
            if (HIDNativeAPIs.HidD_GetSerialNumberString(handle.DangerousGetHandle(), s, s.Capacity))
            {
                rc = s.ToString();
            }

            /* report string */
            return rc;
        }

        /* get vid and pid */
        private HiddAttributtes GetVidPid(SafeFileHandle handle)
        {
            /* attributes structure */
            var attr = new HiddAttributtes();
            /* set size */
            attr.Size = Marshal.SizeOf(attr);

            /* get attributes */
            if (HIDNativeAPIs.HidD_GetAttributes(handle.DangerousGetHandle(), ref attr) == false)
            {
                /* fail! */
                throw new Win32Exception();
            }
            return attr;
        }
        #endregion

        public bool HIDOpen()
        {
            return hidHWDev.Open();
        }

        public bool HIDOpenAsync()
        {
            return hidHWDev.OpenAsync();
        }

        public byte[] HIDRead()
        {
            return hidHWDev.Read();
        }

        public bool HIDWrite(byte[] wData)
        {
            return hidHWDev.Write(wData);
        }

        public byte[] HIDReadAsync()
        {
            return hidHWDev.ReadAsync();
        }

        public bool HIDWriteAsync(byte[] wData)
        {
            return hidHWDev.WriteAsync(wData);
        }

        public void HIDClose()
        {
            hidHWDev.Dispose();
        }

        public bool HIDSetOutputReport(bool isDataWithReportID, byte[] data)
        {
            if (isDataWithReportID) _ReportID = data[0];
            return hidHWDev.SetOutPutReport(data);
        }

        public byte[] HIDGetReport(byte reportID)
        {
            _ReportID = reportID;
            return hidHWDev.GetInputReport(_ReportID);
        }
    }
}
