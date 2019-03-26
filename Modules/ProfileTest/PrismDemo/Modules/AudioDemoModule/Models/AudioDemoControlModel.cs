using AudioDemoModule.Interfaces;
using CommonUILib.Interfaces;
using CommonUILib.Models;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using SystemControlLib;
using SystemControlLib.Enums;
using SystemControlLib.Structures;

namespace AudioDemoModule.Models
{
    public class AudioDemoControlModel : IAudioDemoControlModel
    {
        private IViewItem _messageBox;
        public IViewItem GetMessageBoxVM
        {
            get
            {
                LogTool.Logger("Set MessageBox");
                return _messageBox = new DebugViewItem()
                {
                    MenuName = "Hello Word",
                    MenuStyle = Application.Current.Resources["BaseTextBoxStyle"] as Style,
                };
            }
        }

        public void InitializeSystemHook()
        {
            SystemHook.Instence.Initialize(HwndSource.FromHwnd(new WindowInteropHelper(Application.Current.MainWindow).Handle));
            SystemHook.Instence.RegisterWindowMessageCallback(OnWinMessageCallBack);
            Application.Current.Exit += Current_Exit;
            Application.Current.MainWindow.Closing += MainWindow_Closing;
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            System.Console.Write("MainWindow_Closing");
        }

        private void Current_Exit(object sender, ExitEventArgs e)
        {
            System.Console.Write("Current_Exit");
        }

        private void OnWinMessageCallBack(WinMessage _winMessage)
        {
            string msg = string.Empty;// = $"\n Message {_winMessage.Message} {_winMessage.WParam} {_winMessage.LParam} {_winMessage.IsHandled}";
            switch (_winMessage.Message)
            {
                case WinProc_Message.WM_KEYDOWN:
                    msg += $"\n {_winMessage.Message} {KeyInterop.KeyFromVirtualKey((int)_winMessage.WParam)}";
                    break;
                case WinProc_Message.WM_KEYUP:
                case WinProc_Message.WM_SYSKEYDOWN:
                case WinProc_Message.WM_SYSKEYUP:
                    msg += $"\n {_winMessage.Message} {KeyInterop.KeyFromVirtualKey((int)_winMessage.WParam)}";
                    break;
                case WinProc_Message.WM_WTSSESSION_CHANGE:
                    switch((WParam_Message)_winMessage.WParam)
                    {
                        case WParam_Message.WTS_SESSION_LOCK:
                            msg += " User Lock";
                            break;
                        case WParam_Message.WTS_SESSION_UNLOCK:
                            msg += " User UnLock";
                            break;
                    }
                    break;
                case WinProc_Message.WM_QUERYENDSESSION:
                    switch((LParam_Message)_winMessage.LParam)
                    {
                        case LParam_Message.ENDSESSION_CLOSEAPP:
                        case LParam_Message.ENDSESSION_CRITICAL:
                            msg += " WM_QUERYENDSESSION Not process now";
                            break;
                        case LParam_Message.ENDSESSION_LOGOFF:
                            msg += " WM_QUERYENDSESSION logging off";
                            break;
                    }
                    break;
                case WinProc_Message.WM_ENDSESSION:
                    switch ((LParam_Message)_winMessage.LParam)
                    {
                        case LParam_Message.ENDSESSION_CLOSEAPP:
                        case LParam_Message.ENDSESSION_CRITICAL:
                            msg += " WM_ENDSESSION Not process now";
                            break;
                        case LParam_Message.ENDSESSION_LOGOFF:
                            msg += " WM_ENDSESSION logging off";
                            break;
                    }
                    break;
            }
            _messageBox.MenuName += msg;
            LogTool.Logger(msg);
        }
    }
}
