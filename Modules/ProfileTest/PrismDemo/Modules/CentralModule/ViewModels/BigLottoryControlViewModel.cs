using System.Collections.Generic;
using CentralModule.Interface;
using CentralModule.Models;
using CommonUILib.Interfaces;

namespace CentralModule.ViewModels
{
    class BigLottoryControlViewModel
    {
        private IBigLottoryControlModel _model;

        
        public DebugControlModel LottoryNumMessage { get; set; }
        public DebugControlModel LottoryOpenMessage { get; set; }
        public BigLottoryControlViewModel(IBigLottoryControlModel model)
        {
            _model = model;
            LottoryNumMessage = _model.GetDebugMessageModel();
            LottoryOpenMessage = _model.GetOpenMessage();
            model.LottoryDataProcess();
            model.LottoryDataByOpen();
        }
    }
}
