using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using UtilityUILib;

namespace WPFLogicDemo.Models
{
    interface IWPFLogicModel
    {
        ResourceDictionary GetLocalStyle();
        void SetAsyncAwaitAooRun(IMenuItem messageText);
        void SetAsyncAwaitBooRun(IMenuItem messageText);
        void SetAsyncAwaitCooRun(IMenuItem messageText);
    }
}
