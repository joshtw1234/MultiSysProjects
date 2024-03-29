﻿/******************************************************************************
 *   
 *   Class for HID Device opreation.
 * 
 ******************************************************************************/
using Microsoft.Win32.SafeHandles;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using UtilityUILib;

namespace HIDLib
{
    class HIDHWDev : IDisposable
    {
        const int DefBUfferSize = 64;
       
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
                //_fileStream.Dispose();
                _fileStream.Close();
                /* get rid of object */
                _fileStream = null;
            }

            if (!HIDHandel.IsClosed)
            {
                /* close handle */
                HIDNativeAPIs.CloseHandle(HIDHandel);
            }
        }

        /* open hid device */
        public bool Open()
        {
            /* opens hid device file */
            HIDHandel = HIDNativeAPIs.CreateFile(HWFullPath,
                HIDNativeAPIs.GENERIC_READ | HIDNativeAPIs.GENERIC_WRITE,
                HIDNativeAPIs.FILE_SHARE_READ | HIDNativeAPIs.FILE_SHARE_WRITE,
                IntPtr.Zero, HIDNativeAPIs.OPEN_EXISTING, 0, IntPtr.Zero);

            /* whops */
            if (HIDHandel.IsInvalid)
            {
                return false;
            }

            GetHIDDevInfos();
            int bufSize = (int)OutputBuffSize;
            if (bufSize == 0)
            {
                bufSize = DefBUfferSize;
            }
            /* prepare stream - sync buf size should same as output buf */
            _fileStream = new FileStream(HIDHandel, FileAccess.ReadWrite, bufSize, false);
            /* report status */
            return true;
        }

        public bool OpenAsync()
        {
            if (HWFullPath.Contains("28559b57"))
            {
                int my = -1;
            }
            /* opens hid device file */
            HIDHandel = HIDNativeAPIs.CreateFile(HWFullPath,
                HIDNativeAPIs.GENERIC_READ | HIDNativeAPIs.GENERIC_WRITE,
                HIDNativeAPIs.FILE_SHARE_READ | HIDNativeAPIs.FILE_SHARE_WRITE,
                IntPtr.Zero,
                //HIDNativeAPIs.OPEN_EXISTING, 
                HIDNativeAPIs.FILE_CREATE_NEW,
                HIDNativeAPIs.FILE_FLAG_OVERLAPPED, 
                IntPtr.Zero);

            /* whops */
            if (HIDHandel.IsInvalid)
            {
                int error = Marshal.GetLastWin32Error();
                string rev = HIDNativeAPIs.GetSysErrMsg(error);
                return false;
            }

            GetHIDDevInfos();
            int bufSize = (int)OutputBuffSize;
            if (bufSize == 0)
            {
                bufSize = DefBUfferSize;
            }
            /* prepare stream - async buf size should same as output buf */
            _fileStream = new FileStream(HIDHandel, FileAccess.ReadWrite, bufSize, true);
            /* report status */
            return true;
        }

        private void GetHIDDevInfos()
        {
            //get capabilities - use getPreParsedData, and getCaps
            //store the report lengths
            IntPtr ptrToPreParsedData = new IntPtr();
            bool ppdSucsess = HIDNativeAPIs.HidD_GetPreparsedData(HIDHandel, ref ptrToPreParsedData);
            HIDP_CAPS capabilities = new HIDP_CAPS();
            int hidCapsSucsess = HIDNativeAPIs.HidP_GetCaps(ptrToPreParsedData, ref capabilities);
            //Save buff size
            OutputBuffSize = capabilities.OutputReportByteLength;
            InputBuffSize = capabilities.InputReportByteLength;
            //Call freePreParsedData to release some stuff
            HIDNativeAPIs.HidD_FreePreparsedData(ref ptrToPreParsedData);
        }

        /* write record */
        public bool Write(byte[] data)
        {
            bool rev = false;
            if (data.Length > OutputBuffSize)
            {
                //Output data can't bigger then buff size.
                Utilities.Logger(HIDAPIs.LogHIDHWDev, $"Write Data {data.Length} Out of Buf Size {OutputBuffSize}");
                return rev;
            }
            
            byte[] wData = new byte[OutputBuffSize];
            //First byte is Report ID, if no defined should be 0
            Array.Copy(data, 0, wData, 0, data.Length);
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
                Utilities.Logger(HIDAPIs.LogHIDHWDev, $"Write Error {ex.Message}");
            }
            return rev;
        }

        public bool WriteAsync(byte[] data)
        {
            bool rev = false;
            if (data.Length > OutputBuffSize)
            {
                //Output data can't bigger then buff size.
                Utilities.Logger(HIDAPIs.LogHIDHWDev, $"WriteAsync Data {data.Length} Out of Buf Size {OutputBuffSize}");
                return rev;
            }

            byte[] wData = new byte[OutputBuffSize];
            //First byte is Report ID, if no defined should be 0
            Array.Copy(data, 0, wData, 0, data.Length);
            var revsu = Task.Run(async ()=> 
            {
                try
                {
                    /* write some bytes */
                    await _fileStream.WriteAsync(wData, 0, wData.Length);
                    /* flush! */
                    _fileStream.Flush();
                    rev = true;
                }
                catch (Exception ex)
                {
                    int err = Marshal.GetLastWin32Error();
                    Utilities.Logger(HIDAPIs.LogHIDHWDev, $"WriteAsync Error {ex.Message} {err}");
                }
            });
            revsu.Wait();
            return rev;
        }

        /* read record */
        public byte[] Read()
        {
#if true
            byte[] revbyte = new byte[InputBuffSize];
            revbyte[0] = 0x01;
            try
            {
                _fileStream.Read(revbyte, 0, revbyte.Length);
            }
            catch(Exception ex)
            {
                Utilities.Logger(HIDAPIs.LogHIDHWDev, $"Read Error {ex.Message}");
            }
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


        public byte[] ReadAsync()
        {
#if true
            
            byte[] revbyte = new byte[InputBuffSize];
            revbyte[0] = 0x01;
            var revsu = Task.Run(async () =>
            {
                try
                {
                   return await _fileStream.ReadAsync(revbyte, 0, revbyte.Length);
                }
                catch (Exception ex)
                {
                    Utilities.Logger(HIDAPIs.LogHIDHWDev, $"ReadAsync Error {ex.Message}");
                    return 99;
                }
            }).Result;
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

        public bool SetOutPutReport(byte[] data)
        {
            return HIDNativeAPIs.HidD_SetOutputReport(HIDHandel, data, (uint)data.Length);
        }

        public byte[] GetInputReport(byte reportID)
        {
            byte[] revbuf = new byte[16];
            revbuf[0] = reportID;
            if (!HIDNativeAPIs.HidD_GetInputReport(HIDHandel, revbuf, (uint)revbuf.Length)) revbuf = null;
            return revbuf;
        }
    }
}
