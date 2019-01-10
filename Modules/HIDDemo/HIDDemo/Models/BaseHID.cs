using HIDLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace HIDDemo.Models
{
    public class BaseHID
    {
#if false
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
            if (HIDAPIs.SetupDiDestroyDeviceInfoList(hInfoSet) == false)
            {
                /* fail! */
                throw new Win32Exception();
            }

            /* return list */
            return info;
        }
#endif
    }
}
