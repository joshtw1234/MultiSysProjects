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
        /* stream */
        private FileStream _fileStream;
        /// <summary>
        /// HW Full Path
        /// </summary>
        private string HWFullPath;
        /// <summary>
        /// HID HW Handle
        /// </summary>
        public SafeFileHandle HIDHandle { get; private set; }
        /// <summary>
        /// HID Output buffer Size
        /// </summary>
        public uint OutputBuffSize { get; private set; }
        /// <summary>
        /// HID Input Buffer Size
        /// </summary>
        public uint InputBuffSize { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="hwPath">The HW Full Path</param>
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

            if (!HIDHandle.IsClosed)
            {
                /* close handle */
                HIDAPIs.CloseHandle(HIDHandle);
            }
        }

        /* open hid device */
        public bool Open()
        {
            /* opens hid device file */
            HIDHandle = HIDAPIs.CreateFile(HWFullPath,
                HIDAPIs.GENERIC_READ | HIDAPIs.GENERIC_WRITE,
                HIDAPIs.FILE_SHARE_READ | HIDAPIs.FILE_SHARE_WRITE,
                IntPtr.Zero, HIDAPIs.OPEN_EXISTING, 0, IntPtr.Zero);

            /* whops */
            if (HIDHandle.IsInvalid)
            {
                return false;
            }

            //get capabilites - use getPreParsedData, and getCaps
            //store the reportlengths
            IntPtr ptrToPreParsedData = new IntPtr();
            bool ppdSucsess = HIDAPIs.HidD_GetPreparsedData(HIDHandle, ref ptrToPreParsedData);
            HIDP_CAPS capabilities = new HIDP_CAPS();
            int hidCapsSucsess = HIDAPIs.HidP_GetCaps(ptrToPreParsedData, ref capabilities);
            //Save buff size
            OutputBuffSize = capabilities.OutputReportByteLength;
            InputBuffSize = capabilities.InputReportByteLength;
            //Call freePreParsedData to release some stuff
            HIDAPIs.HidD_FreePreparsedData(ref ptrToPreParsedData);

            /* prepare stream - async */
            _fileStream = new FileStream(HIDHandle, FileAccess.ReadWrite, (int)OutputBuffSize, false);

            /* report status */
            return true;
        }

        /* write record */
        public bool Write(byte[] data)
        {
            bool rev = false;
            if (data.Length > OutputBuffSize)
            {
                //Output data can't bigger then buff size.
                return rev;
            }
            byte[] wData = new byte[OutputBuffSize];
            //first byte must be 0
            Array.Copy(data, 0, wData, 1, data.Length);
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

        /// <summary>
        /// Read HID Data
        /// Note: before call read must call write first.
        /// </summary>
        /// <returns>Read backed data</returns>
        public byte[] Read()
        {
            byte[] revbyte = new byte[InputBuffSize];
            _fileStream.Read(revbyte, 0, revbyte.Length);
            return revbyte;
        }
    }
}
