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
        public DateTime Date { get; set; }
        public List<int> LottoryNumbers { get; set; }
        public int SpecialNumber { get; set; }
    }
}
