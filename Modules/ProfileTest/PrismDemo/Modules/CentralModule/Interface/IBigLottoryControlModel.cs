using CentralModule.Models;
using CommonUILib.Interfaces;

namespace CentralModule.Interface
{
    public interface IBigLottoryControlModel
    {
        void LottoryDataProcess();
        IViewItem GetDebugMessage();
        IViewItem GetTextProgress();
        IViewItem GetViewProgressBar();
        void LottoryDataByOpen();
        DebugControlModel GetDebugMessageModel();
        DebugControlModel GetOpenMessage();
    }

    public enum LottoryArgs
    {
        None,
        Args1
    }
}
