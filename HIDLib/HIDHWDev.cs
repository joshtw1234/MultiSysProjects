/******************************************************************************
 *   
 *   Class for HID Device opreation.
 * 
 ******************************************************************************/
using Microsoft.Win32.SafeHandles;
using System;
using System.IO;

namespace HIDLib
{
    public class HIDHWDev : IDisposable
    {
        const int HIDBUfferSize = 64;
        /* device handle */
        private IntPtr handle;

        public IntPtr HIDHandel { get { return handle; } }
        /* stream */
        private FileStream _fileStream;

        ///* stream */
        //public FileStream fileStream
        //{
        //    get { return _fileStream; }
        //    /* do not expose this setter */
        //    internal set { _fileStream = value; }
        //}

        /* dispose */
        public void Dispose()
        {
            /* deal with file stream */
            if (_fileStream != null)
            {
                /* close stream */
                _fileStream.Close();
                /* get rid of object */
                _fileStream = null;
            }

            if (handle != IntPtr.Zero)
            {
                /* close handle */
                HIDAPIs.CloseHandle(handle);
                handle = IntPtr.Zero;
            }
        }

        /* open hid device */
        public bool Open(string devPath)
        {
            /* safe file handle */
            SafeFileHandle shandle;

            /* opens hid device file */
            handle = HIDAPIs.CreateFile(devPath,
                HIDAPIs.GENERIC_READ | HIDAPIs.GENERIC_WRITE,
                HIDAPIs.FILE_SHARE_READ | HIDAPIs.FILE_SHARE_WRITE,
                IntPtr.Zero, HIDAPIs.OPEN_EXISTING, HIDAPIs.FILE_FLAG_OVERLAPPED,
                IntPtr.Zero);

            /* whops */
            if (handle == HIDAPIs.INVALID_HANDLE_VALUE)
            {
                return false;
            }

            /* build up safe file handle */
            shandle = new SafeFileHandle(handle, false);

            /* prepare stream - async */
            _fileStream = new FileStream(shandle, FileAccess.ReadWrite, HIDBUfferSize, true);

            /* report status */
            return true;
        }

        /* write record */
        public bool Write(byte[] data)
        {
            bool rev = false;
            try
            {
                /* write some bytes */
                _fileStream.Write(data, 0, data.Length);
                /* flush! */
                _fileStream.Flush();
                rev = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
            }
            return rev;
        }

        /* read record */
        public void Read(byte[] data)
        {
            /* get number of bytes */
            int n = 0, bytes = data.Length;

            /* read buffer */
            while (n != bytes)
            {
                /* read data */
                int rc = _fileStream.Read(data, n, bytes - n);
                /* update pointers */
                n += rc;
            }
        }
    }
}
