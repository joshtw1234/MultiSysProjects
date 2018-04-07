using HIDLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace HIDDemo.Models
{
    public class BaseHID
    {
        /* browse all HID class devices */
        public List<HIDInfo> BrowseHID()
        {
            /* hid device class guid */
            Guid gHid;
            /* list of device information */
            List<HIDInfo> info = new List<HIDInfo>();

            /* obtain hid guid */
            HIDAPIs.HidD_GetHidGuid(out gHid);
            /* get list of present hid devices */
            var hInfoSet = HIDAPIs.SetupDiGetClassDevs(ref gHid, null, IntPtr.Zero,
                HIDAPIs.DIGCF_DEVICEINTERFACE | HIDAPIs.DIGCF_PRESENT);

            /* allocate mem for interface descriptor */
            var iface = new HIDAPIs.DeviceInterfaceData();
            /* set size field */
            iface.Size = Marshal.SizeOf(iface);
            /* interface index */
            uint index = 0;

            /* iterate through all interfaces */
            while (HIDAPIs.SetupDiEnumDeviceInterfaces(hInfoSet, 0, ref gHid, index, ref iface))
            {
#if true
                bool isWork = false;
                HIDInfo hidInfo = new HIDInfo(hInfoSet, iface, out isWork);
                if (isWork)
                {
                    info.Add(hidInfo);
                }
#else
                /* vid and pid */
                short vid, pid;

                /* get device path */
                var path = GetPath(hInfoSet, ref iface);

                /* open device */
                var handle = Open(path);
                /* device is opened? */
                if (handle != HIDAPIs.INVALID_HANDLE_VALUE)
                {
                    /* get device manufacturer string */
                    var man = GetManufacturer(handle);
                    /* get product string */
                    var prod = GetProduct(handle);
                    /* get serial number */
                    var serial = GetSerialNumber(handle);
                    /* get vid and pid */
                    GetVidPid(handle, out vid, out pid);

                    /* build up a new element */
                    HIDInfo i = new HIDInfo(prod, serial, man, path, vid, pid);
                    /* add to list */
                    info.Add(i);

                    /* close */
                    Close(handle);
                }
#endif
                /* next, please */
                index++;
            }

            /* clean up */
            if (HIDAPIs.SetupDiDestroyDeviceInfoList(hInfoSet) == false)
            {
                /* fail! */
                throw new Win32Exception();
            }

            /* return list */
            return info;
        }
    }
}
