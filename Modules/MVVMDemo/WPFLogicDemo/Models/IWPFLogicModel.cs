using UtilityUILib;

namespace WPFLogicDemo.Models
{
    interface IWPFLogicModel
    {
        void SetAsyncAwaitAooRun(IMenuItem messageText);
        void SetAsyncAwaitBooRun(IMenuItem messageText);
        void SetAsyncAwaitCooRun(IMenuItem messageText);
        void SetKillProcess(IMenuItem messageText);
        void GetDriverVersion(string v, IMenuItem messageText);
    }
}
