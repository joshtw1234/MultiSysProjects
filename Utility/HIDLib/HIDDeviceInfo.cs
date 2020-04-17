using Microsoft.Win32.SafeHandles;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using UtilityUILib;

namespace HIDLib
{
    public class HIDDeviceInfo
    {
        private const int InfomationBuffer = 256;
        private byte _ReportID = 0x00;

        /// <summary>
        /// HID HW Device class
        /// </summary>
        private HIDDeviceControl _hidHWDev;

        private IntPtr _hidInfoSet;
        /* allocate mem for interface descriptor */
        private DeviceInterfaceData _iface;

        Queue<byte[]> _queueHIDRead;
        BackgroundWorker _hidBackgroundWorker;

        private HIDInfoStruct _hidInfoStruct;
        public HIDInfoStruct HIDInfoStruct { get { return _hidInfoStruct; } }
        public bool IsHIDOpened { get { return _hidBackgroundWorker.IsBusy; } }

        public delegate void HIDReadDataCallBack(byte[] rData);
        public event HIDReadDataCallBack OnHIDReadDataCallBack;


        public HIDDeviceInfo(IntPtr _hidInfoSet, DeviceInterfaceData _iface, out bool isWork)
        {
            this._hidInfoSet = _hidInfoSet;
            this._iface = _iface;
            //Get HW Path
            string devPath = GetPath(this._hidInfoSet, ref this._iface);

            //Open HID HW
            _hidHWDev = new HIDDeviceControl(devPath);
            isWork = false;
            if (_hidHWDev.Open())
            {
                _hidInfoStruct = new HIDInfoStruct()
                {
                    Manufacturer = GetManufacturer(_hidHWDev.HIDHandel),
                    Product = GetProduct(_hidHWDev.HIDHandel),
                    SerialNumber = GetSerialNumber(_hidHWDev.HIDHandel),
                    HIDFullPath = devPath,
                };
                _hidInfoStruct.HIDCompareStr = devPath.Split('&')[2];
                HiddAttributtes hidAttr = new HiddAttributtes();
                try
                {
                    hidAttr = GetVidPid(_hidHWDev.HIDHandel);
                }
                catch (Exception ex)
                {
                    Utilities.Logger(HIDAPIs.LogHIDHWDev, $"GetVidPid Error {ex.Message}");
                }
                _hidInfoStruct.Vid = hidAttr.VendorID;
                _hidInfoStruct.Pid = hidAttr.ProductID;

                _hidInfoStruct.OutputBuffSize = _hidHWDev.OutputBuffSize;
                _hidInfoStruct.InputBuffSize = _hidHWDev.InputBuffSize;
                isWork = true;
                _hidHWDev.Dispose();

                _queueHIDRead = new Queue<byte[]>();
                _hidBackgroundWorker = new BackgroundWorker();
                _hidBackgroundWorker.DoWork += _hidBackgroundWorker_DoWork;
                _hidBackgroundWorker.WorkerSupportsCancellation = true;
            }
        }

        private void _hidBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            const int loopsInterval = 50;
            BackgroundWorker worker = sender as BackgroundWorker;
            while (true)
            {
                System.Threading.Thread.Sleep(loopsInterval);
                if (_queueHIDRead.Count > 0)
                {
                    OnHIDReadDataCallBack?.Invoke(_queueHIDRead.Dequeue());
                }
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

        #region HID operation
        private bool HIDOpen()
        {
            return _hidHWDev.Open();
        }

        public bool HIDOpenAsync()
        {
            _hidBackgroundWorker.RunWorkerAsync();
            return _hidHWDev.OpenAsync();
        }

        private byte[] HIDRead(byte reportID)
        {
            return _hidHWDev.Read(reportID);
        }

        private bool HIDWrite(byte[] wData)
        {
            return _hidHWDev.Write(wData);
        }

        public async Task<byte[]> HIDReadAsync(byte reportID)
        {
            return await _hidHWDev.ReadAsync(reportID);
        }

        public async Task<bool> HIDWriteAsync(byte[] wData)
        {
            return await _hidHWDev.WriteAsync(wData);
        }

        public async Task<int> HIDWriteReadAsync(byte[] wData)
        {
            var result = await _hidHWDev.WriteAsync(wData);
            if (!result) return -1;

            int error = Marshal.GetLastWin32Error();
            if (error != 0) return error;

            
            var readData = await _hidHWDev.ReadAsync(wData[0]);
            _queueHIDRead.Enqueue(readData);
            return 0;
        }

        public void HIDClose()
        {
            _hidBackgroundWorker.CancelAsync();
            _hidHWDev.Dispose();
        }

        public bool HIDSetOutputReport(bool isDataWithReportID, byte[] data)
        {
            if (isDataWithReportID) _ReportID = data[0];
            return _hidHWDev.SetOutPutReport(data);
        }

        public byte[] HIDGetReport(byte reportID)
        {
            _ReportID = reportID;
            return _hidHWDev.GetInputReport(_ReportID);
        }
        #endregion
    }
}
