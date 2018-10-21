using System;
using System.Diagnostics;
using System.Management;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using UtilityUILib;

namespace WPFLogicDemo.Models
{
    /// <summary>
    /// Business Logic class.
    /// For contain Business Logic
    /// </summary>
    class WPFLogicModel
    {
        private async Task StartTask1(IMenuItem messageText)
        {
            messageText.MenuName += $"[{DateTime.Now.ToString("hh:mm:ss.fff")}] Aoo StartTask1 start Thread ID {Thread.CurrentThread.ManagedThreadId}\n";
            await Task.Run(async () =>
            {
                messageText.MenuName += $"[{DateTime.Now.ToString("hh:mm:ss.fff")}] Aoo StartTask1 Delay GO Thread ID {Thread.CurrentThread.ManagedThreadId}\n";
                await Task.Delay(10000);
                messageText.MenuName += $"[{DateTime.Now.ToString("hh:mm:ss.fff")}] Aoo StartTask1 Delay Done Thread ID {Thread.CurrentThread.ManagedThreadId}\n";
            });
            messageText.MenuName += $"[{DateTime.Now.ToString("hh:mm:ss.fff")}] Aoo StartTask1 end Thread ID {Thread.CurrentThread.ManagedThreadId}\n";
        }

        private async Task StartTask2(IMenuItem messageText)
        {
#if true
            //This will A => B => C
            await StartForLoop(messageText, "Boo A");
            await StartForLoop(messageText, "Boo B");
            await StartForLoop(messageText, "Boo C");
#else
            //This is same as StartTask3
            var resu = StartForLoop(messageText, "Boo A");
            resu = StartForLoop(messageText, "Boo B");
            await StartForLoop(messageText, "Boo C");
#endif
        }

        private async Task StartForLoop(IMenuItem messageText, string funName)
        {
            await Task.Run(() =>
            {
                for (int i = 0; i < 30; i++)
                {
                    Thread.Sleep(500);
                    messageText.MenuName += $"[{DateTime.Now.ToString("hh:mm:ss.fff")}] {funName} StartForLoop {i} end Thread ID {Thread.CurrentThread.ManagedThreadId}\n";
                }
            });
        }

        private void StartTask3(IMenuItem messageText)
        {
            messageText.MenuName += $"[{DateTime.Now.ToString("hh:mm:ss.fff")}] StartTask3 {"IN"} end Thread ID {Thread.CurrentThread.ManagedThreadId}\n";
            Task.Run(() =>
            {
                for (int i = 0; i < 30; i++)
                {
                    Thread.Sleep(500);
                    messageText.MenuName += $"[{DateTime.Now.ToString("hh:mm:ss.fff")}] Coo A StartTask3 {i} end Thread ID {Thread.CurrentThread.ManagedThreadId}\n";
                }
            });
            Task.Run(() =>
            {
                for (int i = 0; i < 30; i++)
                {
                    Thread.Sleep(500);
                    messageText.MenuName += $"[{DateTime.Now.ToString("hh:mm:ss.fff")}] Coo B StartTask3 {i} end Thread ID {Thread.CurrentThread.ManagedThreadId}\n";
                }
            });
            Task.Run(() =>
            {
                for (int i = 0; i < 30; i++)
                {
                    Thread.Sleep(500);
                    messageText.MenuName += $"[{DateTime.Now.ToString("hh:mm:ss.fff")}] Coo C StartTask3 {i} end Thread ID {Thread.CurrentThread.ManagedThreadId}\n";
                }
            });
            messageText.MenuName += $"[{DateTime.Now.ToString("hh:mm:ss.fff")}] StartTask3 {"Out"} end Thread ID {Thread.CurrentThread.ManagedThreadId}\n";
        }

        const string LogFileName = @"Logs\WPFLogicDemo.log";

        protected void SetAsyncAwaitCooRun(IMenuItem messageText)
        {
            messageText.MenuName += $"[{DateTime.Now.ToString("hh:mm:ss.fff")}] Coo start Thread ID {Thread.CurrentThread.ManagedThreadId}\n";
            StartTask3(messageText);
            messageText.MenuName += $"[{DateTime.Now.ToString("hh:mm:ss.fff")}] Coo end Thread ID {Thread.CurrentThread.ManagedThreadId}\n";
        }

        protected void SetAsyncAwaitBooRun(IMenuItem messageText)
        {
            messageText.MenuName += $"[{DateTime.Now.ToString("hh:mm:ss.fff")}] Boo start Thread ID {Thread.CurrentThread.ManagedThreadId}\n";
            var result = StartTask2(messageText);
            messageText.MenuName += $"[{DateTime.Now.ToString("hh:mm:ss.fff")}] Boo end Thread ID {Thread.CurrentThread.ManagedThreadId}\n";
        }

        protected void SetAsyncAwaitAooRun(IMenuItem messageText)
        {
            messageText.MenuName += $"[{DateTime.Now.ToString("hh:mm:ss.fff")}] Aoo start Thread ID {Thread.CurrentThread.ManagedThreadId}\n";
            var result = StartTask1(messageText);
            messageText.MenuName += $"[{DateTime.Now.ToString("hh:mm:ss.fff")}] Aoo end Thread ID {Thread.CurrentThread.ManagedThreadId}\n";
        }

        protected ManagementObjectCollection GetProcessOwner(string processName)
        {
            string query = "Select * from Win32_Process Where Name = \"" + processName + "\"";
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);
            return searcher.Get();
        }

        protected void SetKillProcess(IMenuItem messageText)
        {
            //const string svcHost = "svchost.exe";
            const string msmpEng = "MsMpEng";
            //const string localService = "LOCAL SERVICE";
            //const string localSystem = "SYSTEM";
            //const string networkService = "NETWORK SERVICE";
#if false

            var processList = GetProcessOwner(svcHost);

            foreach (ManagementObject obj in processList)
            {
                string[] argList = new string[] { string.Empty, string.Empty };
                int returnVal = Convert.ToInt32(obj.InvokeMethod("GetOwner", argList));
                if (returnVal == 0)
                {
                    // return DOMAIN\user
                    string owner = argList[1] + "\\" + argList[0];
                    messageText.MenuName += $"[{DateTime.Now.ToString("hh:mm:ss.fff")}] GetProcessOwner {owner}\n";
                    if (!owner.Contains(networkService) && !owner.Contains(localSystem) && !owner.Contains(localService))
                    {
                        var pId = Convert.ToInt32(obj["ProcessId"]);
                        var process = Process.GetProcessById(pId);
                        process.Kill();
                    }
                }
                
            }
#else
            var process = Process.GetProcessesByName(msmpEng);
            for (int i = 0; i < process.Length; i++)
            {
                //if (process[i].ProcessName != "Idle")
                //{
                //    IntPtr AnswerBytes;
                //    IntPtr AnswerCount;
                //    if (WTSQuerySessionInformationW(WTS_CURRENT_SERVER_HANDLE,
                //                                    process[i].SessionId,
                //                                    WTS_UserName,
                //                                    out AnswerBytes,
                //                                    out AnswerCount))
                //    {
                //        string userName = Marshal.PtrToStringUni(AnswerBytes);
                //        messageText.MenuName += $"[{DateTime.Now.ToString("hh:mm:ss.fff")}] SetKillProcess {process[i].ProcessName} {userName}\n";
                //    }
                //    else
                //    {
                //        messageText.MenuName += $"[{DateTime.Now.ToString("hh:mm:ss.fff")}] SetKillProcess Could Access {process[i].ProcessName}\n";
                //    }
                //}
                process[i].Kill();
            }
            //bool rev = Utilities.ProcessKiller(svcHost);
            //messageText.MenuName += $"[{DateTime.Now.ToString("hh:mm:ss.fff")}] SetKillProcess {svcHost} {rev}\n";
#endif

        }

        protected bool GetDriverVersion(string driverArgs, IMenuItem messageText)
        {
            const string procName = "wmic";
            const string sysName = "SystemName";
            char[] trimData = new char[] { '\r', '\n' };
            const string strPatten = "([A-Za-z.]+[ ])+|([^ ]+)|([ ]+)[ ]{2}";
            const int driverVersionIdx = 13;
            //driverArgs = "HP Wireless Button Driver";
            string driverFullArgs = $"PATH Win32_PnpSignedDriver where DeviceName=\"{driverArgs}\"";
            string outString = string.Empty;
            //Utilities.RunProcess(procName, driverInArgs, out outString);
            var runRev = Utilities.RunProcessAsync(procName, driverFullArgs);
            if (runRev.ReturnCode != 0)
            {
                messageText.MenuName = $"WMIC return error {runRev.ReturnCode}";
                return false;
            }
            outString = runRev.ConsoleOutput.Replace('\r', ' ');
            outString = outString.Replace('\n', ' ');
            if (string.IsNullOrEmpty(outString))
            {
                //No get console out put string.
                messageText.MenuName = $"WMIC output error {runRev.ReturnCode}";
                return false;
            }
            int keyW = outString.IndexOf(sysName);
            if (0>keyW)
            {
                return false;
            }
            string subStr = outString.Substring(keyW + sysName.Length);
            System.Text.RegularExpressions.MatchCollection ma = System.Text.RegularExpressions.Regex.Matches(subStr, strPatten);
            messageText.MenuName = ma[driverVersionIdx].Groups[0].Value;
            return true;
            //Utilities.Logger(LogFileName, outString);
        }

        //just use the current TS server context.
        internal static IntPtr WTS_CURRENT_SERVER_HANDLE = IntPtr.Zero;

        //the User Name is the info we want returned by the query.
        internal static int WTS_UserName = 5;

        [DllImport("Wtsapi32.dll")]
        public static extern bool WTSQuerySessionInformationW(IntPtr hServer,
            int SessionId,
            int WTSInfoClass,
            out IntPtr ppBuffer,
            out IntPtr pBytesReturned);

    }
}
