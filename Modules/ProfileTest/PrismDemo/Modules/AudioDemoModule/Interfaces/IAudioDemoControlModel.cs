using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonUILib.Interfaces;

namespace AudioDemoModule.Interfaces
{
    public interface IAudioDemoControlModel
    {
        IViewItem GetMessageBoxVM { get; }
    }
}
