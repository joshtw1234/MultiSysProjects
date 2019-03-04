using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemControlLib
{
    public class LogTool
    {
        /// <summary>
        /// Log file name
        /// </summary>
        const string LogUtilityFileName = @"Logs\DemoLog.log";

        /// <summary>
        /// The locker object
        /// </summary>
        private static object locker = new Object();
        /// <summary>
        /// The logger
        /// </summary>
        /// <param name="logPath"></param>
        /// <param name="lines"></param>
        public static void Logger(string lines, string logPath = "")
        {
            try
            {
                if (string.IsNullOrEmpty(logPath))
                {
                    logPath = LogUtilityFileName;
                    var dirInfo = new DirectoryInfo(logPath);
                    if (!Directory.Exists(dirInfo.Parent.Name))
                    {
                        Directory.CreateDirectory(dirInfo.Parent.Name);
                    }
                }
                string strOutput = $"[{DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff")}]={lines}";
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
                Logger(LogUtilityFileName, $"{System.Reflection.MethodBase.GetCurrentMethod().Name}:Exception:{ex.Message}");
            }
        }
    }
}
