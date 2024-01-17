using CentralModule.Models;
using CommonUILib.Interfaces;
using System.Collections.Generic;

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
        List<LottoryInfo> GetLottoryData();
        void ProcessOpenLottoryData2024(List<LottoryInfo> lottoryData);
    }
}
