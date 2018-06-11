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
       
        /* stream */
        private FileStream _fileStream;
        /// <summary>
        /// HW Full Path
        /// </summary>
        private string HWFullPath;
        /* device handle */
        public SafeFileHandle HIDHandel { get; private set; }
        public uint OutputBuffSize { get; private set; }
        public uint InputBuffSize { get; private set; }

        public HIDHWDev(string hwPath)
        {
            HWFullPath = hwPath;
        }

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

            if (!HIDHandel.IsClosed)
            {
                /* close handle */
                HIDAPIs.CloseHandle(HIDHandel);
            }
        }

        /* open hid device */
        public bool Open()
        {
            /* opens hid device file */
            HIDHandel = HIDAPIs.CreateFile(HWFullPath,
                HIDAPIs.GENERIC_READ | HIDAPIs.GENERIC_WRITE,
                HIDAPIs.FILE_SHARE_READ | HIDAPIs.FILE_SHARE_WRITE,
                IntPtr.Zero, HIDAPIs.OPEN_EXISTING, 0, IntPtr.Zero);

            /* whops */
            if (HIDHandel.IsInvalid)
            {
                return false;
            }

            //get capabilites - use getPreParsedData, and getCaps
            //store the reportlengths
            IntPtr ptrToPreParsedData = new IntPtr();
            bool ppdSucsess = HIDAPIs.HidD_GetPreparsedData(HIDHandel, ref ptrToPreParsedData);
            HIDP_CAPS capabilities = new HIDP_CAPS();
            int hidCapsSucsess = HIDAPIs.HidP_GetCaps(ptrToPreParsedData, ref capabilities);
            //Save buff size
            OutputBuffSize = capabilities.OutputReportByteLength;
            InputBuffSize = capabilities.InputReportByteLength;
            //Call freePreParsedData to release some stuff
            HIDAPIs.HidD_FreePreparsedData(ref ptrToPreParsedData);

            /* prepare stream - async */
            _fileStream = new FileStream(HIDHandel, FileAccess.ReadWrite, HIDBUfferSize, false);

            /* report status */
            return true;
        }

        /* write record */
        public bool Write(byte[] data)
        {
            bool rev = false;
            int cpIdx = 0;
            if (data[0] != 0x00 && data.Length >= OutputBuffSize)
            {
                //Output data can't bigger then buff size.
                return rev;
            }
            else
            {
                if (data[0] != 0x00)
                {
                    cpIdx = 1;
                }
            }
            byte[] wData = new byte[OutputBuffSize];
            //first byte must be 0
            Array.Copy(data, 0, wData, cpIdx, data.Length);
            try
            {
                /* write some bytes */
                _fileStream.Write(wData, 0, wData.Length);
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
        public byte[] Read()
        {
#if true
            byte[] revbyte = new byte[InputBuffSize];
            _fileStream.Read(revbyte, 0, revbyte.Length);
            return revbyte;
#else
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
#endif
        }
    }
}
