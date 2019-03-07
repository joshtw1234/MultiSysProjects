using AudioDemoModule.Interfaces;
using CommonUILib.Interfaces;
using CommonUILib.Models;
using System.Windows;
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
                return _messageBox = new MessageBox()
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
        }

        private void OnWinMessageCallBack(WinMessage _winMessage)
        {
            string msg = $"\n Message {_winMessage.Message} {_winMessage.WParam} {_winMessage.LParam} {_winMessage.IsHandled}";
            switch (_winMessage.Message)
            {
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
    class MessageBox : ViewItem
    {
        private string _menuName;
        public override string MenuName
        {
            get
            {
                return _menuName;
            }
            set
            {
                SetProperty(ref _menuName, value);
            }
        }
    }
}
