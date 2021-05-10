using System.Collections.Generic;
using CentralModule.Interface;
using CentralModule.Models;
using CommonUILib.Interfaces;

namespace CentralModule.ViewModels
{
    class BigLottoryControlViewModel : IDebugOutPutControlViewModel, IProgressBarControlViewModel
    {
        private IBigLottoryControlModel _model;

        List<LottoryInfo> LottoryHistory;
        Dictionary<int, List<int>> ResultNumbers;
        public IViewItem DebugMessage { get; set; }
        public IViewItem TextProgress { get; set; }
        public IViewItem ViewProgressBar { get; set; }

        public BigLottoryControlViewModel(IBigLottoryControlModel model)
        {
            _model = model;
            DebugMessage = model.GetDebugMessage();
            TextProgress = model.GetTextProgress();
            ViewProgressBar = model.GetViewProgressBar();
            model.LottoryDataProcess();
            //LottoryDataProcess();
            //LottoryDataByOpen();
        }

     

      

      

       

   

       

       

      
    }
   
}
