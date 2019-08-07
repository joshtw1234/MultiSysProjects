using System;
using System.Runtime.InteropServices;

namespace SystemControlLib
{
    public class NativeImportMethods
    {
        /// <summary>
        /// Registers the device or type of device for which a window will receive notifications.
        /// </summary>
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr RegisterDeviceNotification(IntPtr recipient, IntPtr notificationFilter, int flags);

        /// <summary>
        /// Closes the specified device notification handle.
        /// </summary>
        [DllImport("user32.dll")]
        public static extern bool UnregisterDeviceNotification(IntPtr handle);
    }
}
