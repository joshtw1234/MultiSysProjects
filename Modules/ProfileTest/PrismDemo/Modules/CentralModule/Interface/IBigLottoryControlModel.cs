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
    }
}
