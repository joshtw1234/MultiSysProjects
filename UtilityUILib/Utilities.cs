using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;

namespace UtilityUILib
{
    public sealed class CommonUIConsts
    {
        /// <summary>
        /// Log file name
        /// </summary>
        public const string LogUtilityFileName = @"Logs\OCCUtility.log";

        /// <summary>
        /// Log file name
        /// </summary>
        public const string LogInstallerFileName = @"Logs\OCCSuperInstaller.log";

        #region WMI Constants
        //Root path
        public const string WMICIMRoot = "ROOT\\CIMV2";
        public const string WMIRoot = "ROOT\\WMI";
        //Query strings
        public const string WMIBIOSQueryStry = "SELECT * FROM Win32_BIOS";
        public const string WMISystemQueryStry = "SELECT * FROM Win32_ComputerSystem";
        public const string WMICPUQueryStry = "SELECT * FROM Win32_Processor";
        public const string WMIQueryStr = "SELECT * FROM Win32_OperatingSystem";
        public const string WMIHPQueryStr = "SELECT * FROM HP_BIOSString";
        public const string WMISMBIOSQueryStr = "SELECT * FROM MSSmBios_RawSMBiosTables";
        //Property string for Query
        public const string WinCaption = "Caption";
        public const string WinVersion = "Version";
        public const string WinPrimary = "Primary";
        public const string WinArchitecture = "OSArchitecture";
        public const string WinManufacturer = "Manufacturer";
        public const string WinName = "Name";
        public const string WinValue = "Value";
        public const string WinSMBIOS = "SMBiosData";
        #endregion

        /// <summary>
        /// Task scheduler command
        /// </summary>
        public const string CmdTasksSchedule = "schtasks";
    }

    public sealed class Utilities
    {
        /// <summary>
        /// The Get System Power status API
        /// </summary>
        /// <param name="lpSystemPowerStatus"></param>
        [DllImport("kernel32", EntryPoint = "GetSystemPowerStatus")]
        public static extern void GetSystemPowerStatus(ref SYSTEM_POWER_STATUS lpSystemPowerStatus);

        /// <summary>
        /// The locker object
        /// </summary>
        private static object locker = new Object();
        /// <summary>
        /// The logger
        /// </summary>
        /// <param name="logPath"></param>
        /// <param name="lines"></param>
        public static void Logger(string logPath, string lines)
        {
            try
            {
                string strOutput = $"[{DateTime.Now}]={lines}";
                lock (locker)
                {
                    using (FileStream file = new FileStream(logPath, FileMode.Append, FileAccess.Write, FileShare.Read))
                    {
                        StreamWriter writer = new StreamWriter(file);
                        writer.WriteLine(strOutput);
                        writer.Flush();
                        file.Flush(true);
                        writer.Close();
                        file.Close();
                    }
                }

            }
            catch (Exception ex)
            {
                Logger(GetPhysicalPath(CommonUIConsts.LogUtilityFileName), $"{System.Reflection.MethodBase.GetCurrentMethod().Name}:Exception:{ex.Message}");
            }
        }
        /// <summary>
        /// The method to get WMI value.
        /// </summary>
        /// <param name="queryScope"></param>
        /// <param name="queryStr"></param>
        /// <param name="propertyStr"></param>
        /// <returns></returns>
        public static object GetManageObjValue(string queryScope, string queryStr, string propertyStr)
        {
            object revStr = null;
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(queryScope, queryStr);
            try
            {
                foreach (ManagementObject wmi in searcher.Get())
                {
                    revStr = wmi.GetPropertyValue(propertyStr);
                    if (null == revStr)
                    {
                        Logger(GetPhysicalPath(CommonUIConsts.LogUtilityFileName), $"GetManageObjValue {propertyStr} \"Not Found!!\"");
                    }
                }
            }
            catch (Exception ex)
            {
                Logger(GetPhysicalPath(CommonUIConsts.LogUtilityFileName), $"GetManageObjValue {propertyStr} \"{ex.Message}\"");
            }
            return revStr;
        }

        /// <summary>
        /// The method for run process
        /// </summary>
        /// <param name="appName"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public static int RunProcess(string appName, string arguments)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            process.StartInfo.FileName = appName;
            process.StartInfo.Arguments = arguments;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory;
            try
            {
                process.Start();
                process.WaitForExit();
            }
            catch (Exception ex)
            {
                Logger(GetPhysicalPath(CommonUIConsts.LogUtilityFileName), $"RunProcess \"{ex.Message}\"");
                return -1;
            }
            return process.ExitCode;
        }

        /// <summary>
        /// The Get Physical path 
        /// </summary>
        /// <param name="fileSubPath"></param>
        /// <returns></returns>
        public static string GetPhysicalPath(string fileSubPath)
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileSubPath);
        }
        /// <summary>
        /// Initialize log file.
        /// </summary>
        /// <param name="logFileFullPath"></param>
        public static void InitializeLogFile(string logFileFullPath)
        {
            FileInfo fi = new FileInfo(logFileFullPath);
            if (!fi.Directory.Exists)
            {
                Directory.CreateDirectory(fi.Directory.FullName);
            }
            if (!File.Exists(logFileFullPath))
            {
                FileStream fs = File.Create(logFileFullPath);
                fs.Close();
            }
        }
        /// <summary>
        /// Query Task scheduler name.
        /// </summary>
        /// <param name="taskScheduleName"></param>
        /// <returns></returns>
        public static bool GetQueryTaskSchedulerResult(string taskScheduleName)
        {
            bool rev = false;
            string CmdQueryTaskArgs = $"/query /TN \"{taskScheduleName}\"";
            int qRev = RunProcess(CommonUIConsts.CmdTasksSchedule, CmdQueryTaskArgs);
            if (qRev == 0)
            {
                rev = true;
            }
            return rev;
        }
    }

    /// <summary>
    /// The power status structure.
    /// </summary>
    public struct SYSTEM_POWER_STATUS
    {
        /// <summary>
        /// The AC power status. This member can be one of the following values.
        /// 0 = offline,  1 = Online, 255 = UnKnown Status.
        /// </summary>
        public Byte ACLineStatus;

        /// <summary>
        /// The battery charge status. This member can contain one or more of the following flags.
        /// 1 = High—the battery capacity is at more than 66 percent
        /// 2 = Low—the battery capacity is at less than 33 percent
        /// 4 = Critical—the battery capacity is at less than five percent
        /// 8 = Charging
        /// 128 = No system battery
        /// 255 = Unknown status—unable to read the battery flag information
        /// </summary>
        public Byte BatteryFlag;

        /// <summary>
        /// The percentage of full battery charge remaining. 
        /// This member can be a value in the range 0 to 100, or 255 if status is unknown.
        /// </summary>
        public Byte BatteryLifePercent;

        /// <summary>
        /// The status of battery saver.
        /// 0 = Battery saver is off.
        /// 1 = Battery saver on.Save energy where possible.
        /// </summary>
        public Byte SystemStatusFlag;

        /// <summary>
        /// The number of seconds of battery life remaining, or –1 if remaining seconds are unknown 
        /// or if the device is connected to AC power.
        /// </summary>
        public int BatteryLifeTime;

        /// <summary>
        /// The number of seconds of battery life when at full charge, or –1 if full battery lifetime is unknown 
        /// or if the device is connected to AC power.
        /// </summary>
        public int BatteryFullLifeTime;
    }

    public class BindAbleBases : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Interface
        public event PropertyChangedEventHandler PropertyChanged;
        protected void onPropertyChanged(object sender, string propertyName)
        {
            if (this.PropertyChanged != null)
            { PropertyChanged(sender, new PropertyChangedEventArgs(propertyName)); }
        }
        #endregion
    }
}
