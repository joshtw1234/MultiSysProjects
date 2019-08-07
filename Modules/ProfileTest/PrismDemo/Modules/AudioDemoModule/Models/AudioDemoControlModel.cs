using AudioDemoModule.Interfaces;
using CommonUILib.Interfaces;
using CommonUILib.Models;
using System.Windows;
using SystemControlLib;

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
    }
}
