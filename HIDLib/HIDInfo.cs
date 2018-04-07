using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;

namespace HIDLib
{

    /// <summary>
    /// Class for HID infomation.
    /// </summary>
    public class HIDInfo
    {
        const int InfomationBuffer = 256;

        /// <summary>
        /// HID HW Device class
        /// </summary>
        private HIDHWDev hidHWDev;

        IntPtr hidInfoSet;
        /* allocate mem for interface descriptor */
        HIDAPIs.DeviceInterfaceData iface;
        /* device path */
        public string HIDFullPath { get; private set; }
        /* vendor ID */
        public short Vid { get; private set; }
        /* product id */
        public short Pid { get; private set; }
        /* usb product string */
        public string Product { get; private set; }
        /* usb manufacturer string */
        public string Manufacturer { get; private set; }
        /* usb serial number string */
        public string SerialNumber { get; private set; }
        /// <summary>
        /// The Compare string for HID Device
        /// </summary>
        public string HIDCompareStr { get; private set; }

        public HIDInfo(IntPtr _hidInfoSet, HIDAPIs.DeviceInterfaceData _iface, out bool isWork)
        {
            hidInfoSet = _hidInfoSet;
            hidHWDev = new HIDHWDev();
            iface = _iface;
            string devPath = GetPath(hidInfoSet, ref iface);
            isWork = hidHWDev.Open(devPath);
            if (isWork)
            {
                Manufacturer = GetManufacturer(hidHWDev.HIDHandel);
                Product = GetProduct(hidHWDev.HIDHandel);
                SerialNumber = GetSerialNumber(hidHWDev.HIDHandel);
                HIDFullPath = devPath;
                GetVidPid(hidHWDev.HIDHandel);                
            }
        }

        #region Get Base Info
        /* get device path */
        private string GetPath(IntPtr hInfoSet, ref HIDAPIs.DeviceInterfaceData iface)
        {
            /* detailed interface information */
            var detIface = new HIDAPIs.DeviceInterfaceDetailData();
            /* required size */
            uint reqSize = (uint)Marshal.SizeOf(detIface);

            /* set size. The cbSize member always contains the size of the 
             * fixed part of the data structure, not a size reflecting the 
             * variable-length string at the end. */
            /* now stay with me and look at that x64/x86 maddness! */
            detIface.Size = Marshal.SizeOf(typeof(IntPtr)) == 8 ? 8 : 5;

            /* get device path */
            bool status = HIDAPIs.SetupDiGetDeviceInterfaceDetail(hInfoSet, ref iface, ref detIface, reqSize, ref reqSize, IntPtr.Zero);

            /* whops */
            if (!status)
            {
                /* fail! */
                throw new Win32Exception();
            }
            HIDCompareStr = detIface.DevicePath.Split('&')[2];
            /* return device path */
            return detIface.DevicePath;
        }

        /* get device manufacturer string */
        private string GetManufacturer(IntPtr handle)
        {
            /* buffer */
            var s = new StringBuilder(256);
            /* returned string */
            string rc = String.Empty;

            /* get string */
            if (HIDAPIs.HidD_GetManufacturerString(handle, s, s.Capacity))
            {
                rc = s.ToString();
            }

            /* report string */
            return rc;
        }

        /* get device product string */
        private string GetProduct(IntPtr handle)
        {
            /* buffer */
            var s = new StringBuilder(256);
            /* returned string */
            string rc = String.Empty;

            /* get string */
            if (HIDAPIs.HidD_GetProductString(handle, s, s.Capacity))
            {
                rc = s.ToString();
            }

            /* report string */
            return rc;
        }

        /* get device product string */
        private string GetSerialNumber(IntPtr handle)
        {
            /* buffer */
            var s = new StringBuilder(256);
            /* returned string */
            string rc = String.Empty;

            /* get string */
            if (HIDAPIs.HidD_GetSerialNumberString(handle, s, s.Capacity))
            {
                rc = s.ToString();
            }

            /* report string */
            return rc;
        }

        /* get vid and pid */
        private void GetVidPid(IntPtr handle)
        {
            /* attributes structure */
            var attr = new HIDAPIs.HiddAttributtes();
            /* set size */
            attr.Size = Marshal.SizeOf(attr);

            /* get attributes */
            if (HIDAPIs.HidD_GetAttributes(handle, ref attr) == false)
            {
                /* fail! */
                throw new Win32Exception();
            }

            /* update vid and pid */
            Vid = attr.VendorID; Pid = attr.ProductID;
        }
        #endregion

        public bool HIDOpen()
        {
            return hidHWDev.Open(HIDFullPath);
        }

        public void HIDRead(byte[] rData)
        {
            hidHWDev.Read(rData);
        }

        public bool HIDWrite(byte[] wData)
        {
            return hidHWDev.Write(wData);
        }

        public void HIDClose()
        {
            hidHWDev.Dispose();
        }
    }
}
