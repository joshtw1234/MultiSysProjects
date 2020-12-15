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
        /// <summary>
        /// The keyboard procedure delegate.
        /// </summary>
        public delegate IntPtr LowLevelHookProc(int nCode, IntPtr wParam, IntPtr lParam);

        /// <summary>
        /// Installs an application-defined hook procedure into a hook chain.
        /// </summary>
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr SetWindowsHookEx(int idHook,
           LowLevelHookProc lpfn, IntPtr hMod, uint dwThreadId);
        /// <summary>
        /// Removes a hook procedure installed in a hook chain by the SetWindowsHookEx function.
        /// </summary>
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool UnhookWindowsHookEx(IntPtr hhk);
        /// <summary>
        /// Passes the hook information to the next hook procedure in the current hook chain.
        /// </summary>
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
            IntPtr wParam, IntPtr lParam);
        /// <summary>
        /// Retrieves a module handle for the specified module.
        /// </summary>
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);
    }
}
