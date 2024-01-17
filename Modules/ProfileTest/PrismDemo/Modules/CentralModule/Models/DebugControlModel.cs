using CommonUILib.Interfaces;
using System;
using System.Collections.Generic;

namespace CentralModule.Models
{
    public class DebugControlModel : IDebugOutPutControlViewModel, IProgressBarControlViewModel
    {
        public IViewItem DebugMessage { get; set; }
        public IViewItem TextProgress { get; set; }
        public IViewItem ViewProgressBar { get; set; }
    }

    public class LottoryInfo
    {
        public string LottoryCount { get; set; }
        public DateTime Date { get; set; }
        public string StrLottoryNumbers { get; set; }
        public int SpecialNumber { get; set; }
        public List<int> LottoryNumbers { get; set; }
    }
}
